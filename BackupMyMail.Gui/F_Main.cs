using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml;
using BackupMyMail.Lib;
using System.Threading;

namespace BackupMyMail.Gui
{
    public partial class F_Main : Form
    {
        private readonly Manager manager;
        private readonly string executeProgramFolder;
        private Thread proc;
        private System.Windows.Forms.Timer timer, timerSchedule;
        private string actualState = "null";
        private string folderBackup;
        private bool closeComputerAfterBackup;
        private readonly string pathToSettings = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\settingsBackupMyMail.xml";
        private string pathToLog = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\logBackupMyMail.txt";
        private DateTime scheduleRun;
        private static bool backupRunning = false;
        private List<string> listPstArchiveOrg;
        private List<string> listPstBackupArchive;
        private string pathToBackupMainPst;
        private Thread procCopy;
        private bool copyState = false;
        private bool notUseVss = false;

        public F_Main()
        {
            InitializeComponent();
            executeProgramFolder = Path.GetDirectoryName(Application.ExecutablePath);
            manager = new Manager(executeProgramFolder);
            timer = new System.Windows.Forms.Timer() { Interval = 1000, Enabled = true };
            timer.Tick += timer_Tick;
            timer.Start();
            timerSchedule = new System.Windows.Forms.Timer() { Interval = 1000, Enabled = true };
            timerSchedule.Tick += timerSchedule_Tick;
            timerSchedule.Start();

            listPstArchiveOrg = new List<string>();
            listPstBackupArchive = new List<string>();
        }

        private void timerSchedule_Tick(object sender, EventArgs e)
        {
            var dateNow = DateTime.Now;
            if (!(scheduleRun.Year == dateNow.Year && scheduleRun.Month == dateNow.Month && scheduleRun.Day == dateNow.Day && scheduleRun.Hour == dateNow.Hour && scheduleRun.Minute == dateNow.Minute && scheduleRun.Second == dateNow.Second))
                return;

            BackupNow_Action();
            if (CB_repeatEvery.Checked && NUD_repeatEveryNum.Value > 0)
            {
                Double everyNum = Double.Parse(NUD_repeatEveryNum.Value.ToString());
                if (CB_repeadScheduleKind.SelectedIndex == 0)
                {
                    scheduleRun.AddHours(everyNum);
                }
                else if (CB_repeadScheduleKind.SelectedIndex == 1)
                {
                    scheduleRun.AddDays(everyNum);
                }
                else if (CB_repeadScheduleKind.SelectedIndex == 2)
                {
                    scheduleRun.AddDays(everyNum);
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (copyState)
            {
                //during copy
                L_copyState.Text = "Copying now, don't stop the progress...";
                B_restoreDefaultPST.Enabled = false;
                B_restoreDefaultPstAndArchivePstS.Enabled = false;

                if (!procCopy.IsAlive)
                {
                    copyState = false;
                    B_restoreDefaultPST.Enabled = true;
                    B_restoreDefaultPstAndArchivePstS.Enabled = true;
                }
            }
            else
            {
                //not copy
                L_copyState.Text = "original, not copy";
                if (L_copyState.Text != "original, not copy")
                {
                    B_restoreDefaultPST.Enabled = false;
                    B_restoreDefaultPstAndArchivePstS.Enabled = false;
                }
            }

            L_actualState.Text = actualState;

            if (String.Compare(actualState, "Backup successfull!", false) == 0)
            {
                B_terminateBackupNow.Enabled = false;
                B_startBackupNow.Enabled = true;

                if (scheduleRun != null &&
                scheduleRun.Year != 1999)
                {
                    if (scheduleRun.Year == 0001)
                    {
                        this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                        this.Icon = new Icon(GetType(), "normal.ico");
                    }
                    else
                    {
                        this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                        this.Icon = new Icon(GetType(), "schedule.ico");
                    }
                }
                else
                {
                    this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                    this.Icon = new Icon(GetType(), "normal.ico");
                }

                if (backupRunning)
                {
                    NI_F_Main.ShowBalloonTip(1500, "Backup My Mail", "Backup successful!", ToolTipIcon.Info);
                    backupRunning = false;
                }
            }

            if (String.Compare(actualState, "Backup termination successful!", false) == 0)
            {
                B_startBackupNow.Enabled = true;
                B_terminateBackupNow.Enabled = false;

                if (scheduleRun != null &&
                scheduleRun.Year != 1999 &&
                scheduleRun.Year != 0001)
                {
                    this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                    this.Icon = new Icon(GetType(), "schedule.ico");
                }
                else
                {
                    this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                    this.Icon = new Icon(GetType(), "normal.ico");
                }
            }

            if (String.Compare(manager.ErrorCode, "null", false) == 0 || manager.ErrorCode == null)
                return;

            timer.Stop();
            actualState = "Error!, action terminated!";
            L_actualState.Text = actualState;
            if (scheduleRun != null &&
            scheduleRun.Year != 1999 &&
            scheduleRun.Year != 0001)
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                this.Icon = new Icon(GetType(), "schedule.ico");
            }
            else
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                this.Icon = new Icon(GetType(), "normal.ico");
            }

            manager.TerminateBackup();
            B_startBackupNow.Enabled = true;
            B_terminateBackupNow.Enabled = false;

            MessageBox.Show("Error code:\n" + manager.ErrorCode,
            "Error Message",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        }

        private void B_browseBackupOutputFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
                var dr = fbd.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return;
                switch (fbd.SelectedPath.Substring(fbd.SelectedPath.Length - 2, 1))
                {
                    case ":":
                        MessageBox.Show("This version of hobocopy cannot copy to the root of a drive\n" + "Please choose folder on drive!", "Hobocopy v.1.0.0.0 limitations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        TB_pathToBackupOutputFolder.Text = fbd.SelectedPath;
                        break;
                }
            }
        }

        private void B_startBackupNow_Click(object sender, EventArgs e)
        {
            BackupNow_Action();
        }

        private void BackupNow_Action()
        {
            //close MS Outlook user choose for "not use vss service and hobocopy"
            bool runningOutlook = false;
            Process[] currentProcesses = Process.GetProcesses();
            foreach (var cProcess in currentProcesses)
	        {
                if (cProcess.ProcessName.Trim().Contains("OUTLOOK"))
                {
                    runningOutlook = true;
                }
	        }

            if (runningOutlook)
            {
                if (CB_notUseVss.Checked)
                {
                    DialogResult dr = MessageBox.Show("If you want to backup pst files not using VSS and Hobocopy(C), please close MS Outlook.\nClose MS Outlook now?",
                                                      "Outlook actual state",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        foreach (var cProcess in currentProcesses)
                        {
                            if (cProcess.ProcessName.Trim().Contains("OUTLOOK"))
                            {
                                cProcess.Kill();
                                break;
                            }
                        }
                    }
                    else return;
                }
            }

            if (CB_deleteAllPstFileFromBackup.Checked)
            {
                manager.DeleteOldPst = true;
            }

            if (CB_copyRegistrySettings.Checked)
            {
                manager.CopyRegistrySettings = true;
            }

            if (CB_notUseVss.Checked)
            {
                manager.NotUseVSSandHobocopy = true;
            }

            if (string.IsNullOrEmpty(TB_pathToBackupOutputFolder.Text))
            {
                MessageBox.Show("Backup destination folder path empty, please enter correct backup destination path", "Input Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }

            manager.pathToLog = pathToLog;

            folderBackup = TB_pathToBackupOutputFolder.Text;
            proc = new Thread(RunBackup);
            proc.Start();

            if (!timer.Enabled)
            {
                timer.Start();
            }

            actualState = "Backup in progress...";
            B_terminateBackupNow.Enabled = true;
            this.NI_F_Main.Icon = new Icon(GetType(), "working.ico");
            this.Icon = new Icon(GetType(), "working.ico");

            backupRunning = true;
        }

        private void RunBackup()
        {
            if (string.IsNullOrEmpty(folderBackup))
                return;

            if (manager.RunBackup(folderBackup))
            {
                actualState = "Backup successfull!";

                if (closeComputerAfterBackup)
                {
                    CloseComputer();
                }
            }
            else
            {
                actualState = "Terminate backup successfull!";
            }
        }

        private void F_Main_Load(object sender, EventArgs e)
        {
            ReadXML();

            TB_pathToLogFile.Text = pathToLog;

            B_terminateBackupNow.Enabled = false;

            if (CB_afterBackupCloseComputer.Checked)
            {
                closeComputerAfterBackup = true;
            }

            if (CB_notUseVss.Checked)
            {
                notUseVss = true;
            }

            if (CB_minimalizeWindowOnStartup.Checked)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.ShowInTaskbar = false;
            }

            CB_repeadScheduleKind.SelectedIndex = 1;
            DTP_startScheduleDate.Value = DateTime.Now;
            DTP_startScheduleDate.MinDate = DateTime.Now;
            NUD_startHourSchedule.Value = DateTime.Now.Hour;
            NUD_startMinuteSchedule.Value = DateTime.Now.Minute;
            if (String.Compare(L_actualScheduleSet.Text, "null", false) != 0)
                return;

            B_editSchedule.Enabled = false;
            B_removeSchedule.Enabled = false;
        }

        private static void CloseComputer()
        {
            try
            {
                Process.Start("shutdown", "/s /t 0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Description:\n" + ex.Message,
                "Close Computer Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
        }

        private void B_terminateBackupNow_Click(object sender, EventArgs e)
        {
            TerminateBackup_Action();
        }

        private void TerminateBackup_Action()
        {
            bool resultTerminateFromLibrary = manager.TerminateBackup();
            if (proc != null)
            {
                proc.Abort();

                if (!proc.IsAlive && resultTerminateFromLibrary)
                {
                    actualState = "Terminate backup successfull!";
                    L_actualState.Text = actualState;
                    NI_F_Main.ShowBalloonTip(1500, "Backup My Mail", "Terminate backup successful!", ToolTipIcon.Info);
                }
                else
                {
                    actualState = "Terminate backup in progress...";
                    while (manager.TerminateBackup())
                    {
                    }
                    manager.ErrorCode = "null";
                    while (proc.IsAlive)
                    {
                        proc.Abort();
                        Thread.Sleep(100);
                    }
                }

                if (!proc.IsAlive)
                {
                    actualState = "Terminate backup successfull!";
                    L_actualState.Text = actualState;
                    NI_F_Main.ShowBalloonTip(1500, "Backup My Mail", "Terminate backup successful!", ToolTipIcon.Info);
                    backupRunning = false;
                }
            }

            B_terminateBackupNow.Enabled = false;

            if (timer.Enabled)
            {
                timer.Stop();
            }

            if (scheduleRun != null &&
            scheduleRun.Year != 1999 &&
            scheduleRun.Year != 0001)
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                this.Icon = new Icon(GetType(), "schedule.ico");
            }
            else
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                this.Icon = new Icon(GetType(), "normal.ico");
            }
        }

        private void F_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            timerSchedule.Stop();

            SaveXML();
        }

        private void SaveXML()
        {
            try
            {
                if (File.Exists(pathToSettings))
                {
                    File.Delete(pathToSettings);
                }

                bool deleteAllPstFromBackupFolder = false;
                if (CB_deleteAllPstFileFromBackup.Checked)
                    deleteAllPstFromBackupFolder = true;

                bool afterBackupCloseComputer = false;
                if (CB_afterBackupCloseComputer.Checked)
                    afterBackupCloseComputer = true;

                bool copyRegistrySettings = false;
                if (CB_copyRegistrySettings.Checked)
                    copyRegistrySettings = true;

                bool minimalizeWindowOnStartUp = false;
                if (CB_minimalizeWindowOnStartup.Checked)
                    minimalizeWindowOnStartUp = true;

                bool notUseVss = false;
                if (CB_notUseVss.Checked)
                    notUseVss = true;

                using (var textWriter = new XmlTextWriter(pathToSettings, null))
                {
                    textWriter.WriteStartDocument();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteComment("Backup My Mail for Microsoft Outlook - file settings");
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteStartElement("ApplicationSettings");
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("FolderBackup", TB_pathToBackupOutputFolder.Text);
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("DeleteAllPstFromBackupFolder", deleteAllPstFromBackupFolder.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("AfterBackupCloseComputer", afterBackupCloseComputer.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("MinimalizeWindowOnStartUp", minimalizeWindowOnStartUp.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("ScheduleTime", scheduleRun.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("ActualSetSchedule", L_actualScheduleSet.Text);
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("PathToLog", pathToLog);
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("CopyRegistrySettings", copyRegistrySettings.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("NotUseVss", notUseVss.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteEndDocument();
                    textWriter.Close();
                }
            }
            catch
            {
            }
        }

        private bool ReadXML()
        {
            try
            {
                if (!File.Exists(pathToSettings))
                {
                    return false;
                }

                using (var reader = new XmlTextReader(pathToSettings))
                {
                    while (reader.Read())
                    {
                        var nType = reader.NodeType;
                        if (nType == XmlNodeType.Element)
                        {
                            if (String.Compare(reader.Name, "FolderBackup", false) == 0)
                            {
                                TB_pathToBackupOutputFolder.Text = reader.NamespaceURI;
                            }
                            if (String.Compare(reader.Name, "NotUseVss", false) == 0)
                            {
                                CB_notUseVss.Checked = bool.Parse(reader.NamespaceURI) ? true : false;
                            }
                            if (string.Compare(reader.Name, "DeleteAllPstFromBackupFolder", false) == 0)
                            {
                                CB_deleteAllPstFileFromBackup.Checked = bool.Parse(reader.NamespaceURI) ? true : false;
                            }
                            if (String.Compare(reader.Name, "AfterBackupCloseComputer", false) == 0)
                            {
                                CB_afterBackupCloseComputer.Checked = bool.Parse(reader.NamespaceURI) ? true : false;
                            }
                            if (String.Compare(reader.Name, "MinimalizeWindowOnStartUp", false) == 0)
                            {
                                CB_minimalizeWindowOnStartup.Checked = bool.Parse(reader.NamespaceURI) ? true : false;
                            }
                            if (String.Compare(reader.Name, "CopyRegistrySettings", false) == 0)
                            {
                                CB_copyRegistrySettings.Checked = bool.Parse(reader.NamespaceURI) ? true : false;
                            }
                            if (String.Compare(reader.Name, "ScheduleTime", false) == 0)
                            {
                                if (String.Compare(reader.NamespaceURI, string.Empty, false) != 0)
                                {
                                    scheduleRun = DateTime.Parse(reader.NamespaceURI);
                                    if (scheduleRun.Year > 1999)
                                    {
                                        this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                                        this.Icon = new Icon(GetType(), "schedule.ico");
                                    }
                                    else
                                    {
                                        this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                                        this.Icon = new Icon(GetType(), "normal.ico");
                                    }
                                }
                            }
                            if (String.Compare(reader.Name, "ActualSetSchedule", false) == 0)
                            {
                                L_actualScheduleSet.Text = reader.NamespaceURI;
                            }
                            if (String.Compare(reader.Name, "PathToLog", false) == 0)
                            {
                                pathToLog = reader.NamespaceURI;
                            }
                        }
                    }
                    reader.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CB_afterBackupCloseComputer_CheckedChanged(object sender, EventArgs e)
        {
            closeComputerAfterBackup = CB_afterBackupCloseComputer.Checked ? true : false;
        }

        private void SWTSMI_showWindow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void SWTSMI_closeProgram_Click(object sender, EventArgs e)
        {
            TerminateBackup_Action();
            this.Close();
        }

        private void F_Main_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                return;

            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void NI_F_Main_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void SWTSMI_backupNow_Click(object sender, EventArgs e)
        {
            BackupNow_Action();
        }

        private void LL_homeWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://backupmymail.codeplex.com");
        }

        private void B_addSchedule_Click(object sender, EventArgs e)
        {
            var userChooseDateAndTime = new DateTime(DTP_startScheduleDate.Value.Year,
                        DTP_startScheduleDate.Value.Month,
                        DTP_startScheduleDate.Value.Day,
                        Int32.Parse(NUD_startHourSchedule.Value.ToString()),
                        Int32.Parse(NUD_startMinuteSchedule.Value.ToString()),
                        0);

            if (userChooseDateAndTime < DateTime.Now)
            {
                MessageBox.Show("Cannot add schedule eariel then date-time now!",
                "Add schedule time",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }

            scheduleRun = userChooseDateAndTime;

            L_actualScheduleSet.Text = !CB_repeatEvery.Checked ? "At " +
                scheduleRun.Hour + ":" +
                scheduleRun.Minute +
                " , starting: " +
                scheduleRun.Month + "/" +
                scheduleRun.Day + "/" +
                scheduleRun.Year +
                "\nRun Backup with actual backup path" : "At " +
                scheduleRun.Hour + ":" +
                scheduleRun.Minute +
                ", every " +
                NUD_repeatEveryNum.Value + " " +
                CB_repeadScheduleKind.SelectedItem +
                ", starting: " +
                scheduleRun.Month + "/" +
                scheduleRun.Day + "/" +
                scheduleRun.Year +
                " \nRun Backup with actual backup path";

            if (scheduleRun != null)
            {
                B_addSchedule.Enabled = false;
                B_editSchedule.Enabled = true;
                B_removeSchedule.Enabled = true;
            }

            if (String.Compare(actualState, "Backup in progress...", false) == 0)
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "working.ico");
                this.Icon = new Icon(GetType(), "working.ico");
            }
            else
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                this.Icon = new Icon(GetType(), "schedule.ico");
            }
        }

        private void B_removeSchedule_Click(object sender, EventArgs e)
        {
            scheduleRun = new DateTime(1999, 1, 1, 0, 0, 0);
            L_actualScheduleSet.Text = "null";
            B_addSchedule.Enabled = true;
            B_editSchedule.Enabled = false;
            B_removeSchedule.Enabled = false;

            if (String.Compare(actualState, "Backup in progress...", false) == 0)
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "working.ico");
                this.Icon = new Icon(GetType(), "working.ico");
            }
            else
            {
                this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                this.Icon = new Icon(GetType(), "normal.ico");
            }
        }

        private void B_editSchedule_Click(object sender, EventArgs e)
        {
            if (scheduleRun == null || scheduleRun.Year == 1999)
                return;

            DTP_startScheduleDate.Value = scheduleRun.AddHours(1);
            NUD_startHourSchedule.Value = scheduleRun.Hour;
            NUD_startMinuteSchedule.Value = scheduleRun.Minute;

            B_addSchedule.Enabled = true;
            B_editSchedule.Enabled = false;
            B_removeSchedule.Enabled = true;
        }

        private void B_browseLogFile_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(pathToLog))
                {
                    sfd.InitialDirectory = pathToLog.Substring(0, pathToLog.LastIndexOf("\\"));
                }
                sfd.Filter = "txt files (*.txt)|*.txt";
                var dr = sfd.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.OK)
                    return;
                pathToLog = sfd.FileName;
                TB_pathToLogFile.Text = pathToLog;
            }
        }

        private void B_browseRegOutlookRestore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.SpecialFolder.Desktop.ToString();
            ofd.Filter = "Registry files (*.reg)|*.reg";
            DialogResult dr = ofd.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                TB_pathToRegOutlookRestore.Text = ofd.FileName;
            }
        }

        private void B_restoreOutlookRegistry_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TB_pathToRegOutlookRestore.Text.Trim()))
            {
                MessageBox.Show("Path to *.reg file is empty",
                                "Import fail!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            if (!File.Exists(TB_pathToRegOutlookRestore.Text.Trim()))
            {
                MessageBox.Show("File doesn't exists",
                                "Import fail!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            string command = @"/s " + TB_pathToRegOutlookRestore.Text;
            var psiRestoreReg = new ProcessStartInfo();
            psiRestoreReg.FileName = "regedit.exe";
            psiRestoreReg.Arguments = command;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                psiRestoreReg.Verb = "runas";
                psiRestoreReg.UseShellExecute = true;
            }
            else
            {
                psiRestoreReg.UseShellExecute = true;
            }
            System.Diagnostics.Process pRestoreReg = Process.Start(psiRestoreReg);
            pRestoreReg.WaitForExit();
            if (!pRestoreReg.HasExited)
            {
                pRestoreReg.Kill();
            }

            MessageBox.Show("Import from file: " + TB_pathToRegOutlookRestore.Text + " done successfull!",
                            "Import profile from file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            TB_pathToRegOutlookRestore.Text = string.Empty;
        }

        private void B_browsePathToPstInfo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.SpecialFolder.Desktop.ToString();
            ofd.Filter = "Xml files (*.xml)|*.xml";
            DialogResult dr = ofd.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                TB_pathToPstInfo.Text = ofd.FileName;
            }
        }

        private void B_readPstInfo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TB_pathToPstInfo.Text.Trim()))
            {
                MessageBox.Show("Path to *.xml file is empty",
                                "Read file fail!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            if (!File.Exists(TB_pathToPstInfo.Text.Trim()))
            {
                MessageBox.Show("File doesn't exists",
                                "Read file fail!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (ReadXmlPstInfo(TB_pathToPstInfo.Text))
                GB_restorePstS.Enabled = true;
        }

        private bool ReadXmlPstInfo(string pathToSettings)
        {
            try
            {
                using (var reader = new XmlTextReader(pathToSettings))
                {
                    listPstArchiveOrg.Clear();
                    listPstBackupArchive.Clear();
                    while (reader.Read())
                    {
                        var nType = reader.NodeType;
                        if (nType == XmlNodeType.Element)
                        {
                            if (String.Compare(reader.Name, "MainPst", false) == 0)
                            {
                                L_pathToDefaultPst.Text = reader.NamespaceURI;
                            }
                            else if (String.Compare(reader.Name, "ArchivePst", false) == 0)
                            {
                                listPstArchiveOrg.Add(reader.NamespaceURI);
                            }
                            else if (String.Compare(reader.Name, "BackupMainPst", false) == 0)
                            {
                                pathToBackupMainPst = reader.NamespaceURI;
                            }
                            else if (String.Compare(reader.Name, "BackupArchivePst", false) == 0)
                            {
                                listPstBackupArchive.Add(reader.NamespaceURI);
                            }
                        }
                    }
                    reader.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool CloseRunningMSOutlook()
        {
            try
            {
                //close MS Outlook user choose for "not use vss service and hobocopy"
                bool runningOutlook = false;
                Process[] currentProcesses = Process.GetProcesses();
                foreach (var cProcess in currentProcesses)
                {
                    if (cProcess.ProcessName.Trim().Contains("OUTLOOK"))
                    {
                        runningOutlook = true;
                    }
                }

                if (runningOutlook)
                {
                    if (CB_notUseVss.Checked)
                    {
                        DialogResult dr = MessageBox.Show("If you want restore pst file(s), please close you running MS Outlook.\nClose you MS Outlook now?",
                                                          "Outlook actual state",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Question);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            foreach (var cProcess in currentProcesses)
                            {
                                if (cProcess.ProcessName.Trim().Contains("OUTLOOK"))
                                {
                                    cProcess.Kill();
                                    break;
                                }
                            }
                        }
                        else return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void B_restoreDefaultPST_Click(object sender, EventArgs e)
        {
            if (!CloseRunningMSOutlook()) return;

            pstPathOrg = L_pathToDefaultPst.Text;
            if (procCopy != null) procCopy = null;
            procCopy = new Thread(CopyOnlyDefaultPst);
            copyState = true;
            procCopy.Start();
        }

        private string pstPathOrg;
        private void B_restoreDefaultPstAndArchivePstS_Click(object sender, EventArgs e)
        {
            if (!CloseRunningMSOutlook()) return;

            pstPathOrg = L_pathToDefaultPst.Text;
            if (procCopy != null) procCopy = null;
            procCopy = new Thread(CopyAllPst);
            copyState = true;
            procCopy.Start();
        }

        private void CopyOnlyDefaultPst()
        {
            try
            {
                File.Copy(pathToBackupMainPst, pstPathOrg, true);
            }
            catch 
            {
            }
        }

        private void CopyAllPst()
        {
            try
            {
                //default pst
                if (!Directory.Exists(pstPathOrg.Substring(0, pstPathOrg.LastIndexOf("\\"))))
                {
                    Directory.CreateDirectory(pstPathOrg.Substring(0, pstPathOrg.LastIndexOf("\\")));
                }
                File.Copy(pathToBackupMainPst, pstPathOrg, true);

                //all archive
                for (int i = 0; i < listPstArchiveOrg.Count; i++)
                {
                    if (!Directory.Exists(listPstArchiveOrg[i].Substring(0, listPstArchiveOrg[i].LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(listPstArchiveOrg[i].Substring(0, listPstArchiveOrg[i].LastIndexOf("\\")));
                    }

                    File.Copy(listPstBackupArchive[i], listPstArchiveOrg[i], true);
                }
            }
            catch
            {
            }
        }

        private void B_terminateCopy_Click(object sender, EventArgs e)
        {
            if (procCopy.IsAlive)
            {
                procCopy.Abort();
                copyState = true;
                MessageBox.Show("Terminate Copy done successfull!",
                                "Terminate Copy",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Not terminate copy, becouse copy not running now!",
                                "Terminate Copy",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        //for test only
        //private void B_copyPstFromNetworkShare_Click(object sender, EventArgs e)
        //{
        //    if (manager.CopyPstFromNetworkShare(@"\\127.0.0.1\archive\folder\Outlook.pst",
        //                                    @"D:\n\Outlook_from_network_share.pst"))
        //    {
        //        MessageBox.Show("Copy from network share successfull!");
        //    }
        //}
    }
}
