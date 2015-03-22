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
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace BackupMyMail.Gui
{
    public partial class F_Main : Form
    {
        private Manager manager;
        private string executeProgramFolder;
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
        private bool autostart = false;
        private string scheduledTaskName = "BackupMyMail";
        private bool minimalizeAfterStart = false;
        private static bool closeProgram = false;
        bool bShutDownPending = false;
        bool enableRepeat = false;
        int repeatNumber = 0;
        int repeatKind = 0;
        Version appSettingsVersion = new Version("0.0.0.0");

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetSystemMetrics(int nIndex);

        public F_Main(string[] args)
        {
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToLower() == "/min")
                    {
                        minimalizeAfterStart = true;
                    }
                }
            }

            InitializeComponent();
            AfterStart();
        }

        public F_Main()
        {
            InitializeComponent();
            AfterStart();
        }

        private void AfterStart()
        {
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
            timerSchedule.Stop();
            timerSchedule.Enabled = false;
            var dateNow = DateTime.Now;
            if (!(scheduleRun.Year == dateNow.Year && scheduleRun.Month == dateNow.Month && scheduleRun.Day == dateNow.Day && scheduleRun.Hour == dateNow.Hour && scheduleRun.Minute == dateNow.Minute && scheduleRun.Second == dateNow.Second))
            {
                timerSchedule.Enabled = true;
                timerSchedule.Start();
                return;
            }

            BackupNow_Action();
            if (CB_repeatEvery.Checked && NUD_repeatEveryNum.Value > 0)
            {
                Double everyNum = Double.Parse(NUD_repeatEveryNum.Value.ToString());
                if (CB_repeadScheduleKind.SelectedIndex == 0)
                {
                    scheduleRun = scheduleRun.AddHours(everyNum);
                }
                else if (CB_repeadScheduleKind.SelectedIndex == 1)
                {
                    scheduleRun = scheduleRun.AddDays(everyNum);
                }
                else if (CB_repeadScheduleKind.SelectedIndex == 2)
                {
                    scheduleRun = scheduleRun.AddDays(everyNum * 7);
                }
                SaveXML();
            }
            timerSchedule.Enabled = true;
            timerSchedule.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //SM_SHUTTINGDOWN = 0x2000
            bShutDownPending = GetSystemMetrics(0x2000) != 0;
            if (bShutDownPending)
            {
                timer.Stop();
                timer.Enabled = false;
                closeProgram = true;
                Application.Exit();
            }

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
            this.NI_F_Main.MouseClick += NI_F_Main_MouseClick;

            if (minimalizeAfterStart)
                this.WindowState = FormWindowState.Minimized;

            Version version = Assembly.GetEntryAssembly().GetName().Version;
            this.Text = this.Text + version.ToString();
            
            ReadXML();
            if (appSettingsVersion.CompareTo(new Version("1.3.2.3")) == -1)
            {
                //reorder save settings for older version < 1.3.2.3
                SaveXML();
                ReadXML();
            }

            executeProgramFolder = Path.GetDirectoryName(Application.ExecutablePath);
            manager = new Manager(executeProgramFolder);

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

            //set actual date
            if (scheduleRun.Year < 2000)
                SetDefaultScheduler();

            if (String.Compare(L_actualScheduleSet.Text, "null", false) != 0)
                return;

            B_editSchedule.Enabled = false;
            B_removeSchedule.Enabled = false;
        }

        void NI_F_Main_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            }
        }

        private void SetDefaultScheduler()
        {
            CB_repeatEvery.Checked = false;
            CB_repeadScheduleKind.SelectedIndex = 1;
            DTP_startScheduleDate.Value = DateTime.Now;
            DTP_startScheduleDate.MinDate = DateTime.Now;
            NUD_startHourSchedule.Value = DateTime.Now.Hour;
            NUD_startMinuteSchedule.Value = DateTime.Now.Minute;
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
            SaveXML();
            this.WindowState = FormWindowState.Minimized;

            if (!closeProgram)
                e.Cancel = true; //abort closing
            else
            {
                TerminateBackup_Action();
                timer.Stop();
                timerSchedule.Stop();
            }
        }

        private bool IsSetAutostartApp()
        {
            string tbl = string.Empty;

            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "SCHTASKS.exe";
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                if (System.Environment.OSVersion.Version.Major >= 6)
                    p.StartInfo.Verb = "runas";

                p.StartInfo.Arguments = String.Format("/Query /S {0} /TN {1} /FO TABLE /NH", Environment.MachineName, scheduledTaskName);

                p.Start();
                string error = p.StandardError.ReadToEnd();

                p.StandardOutput.ReadLine();
                tbl = p.StandardOutput.ReadToEnd();

                p.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading is task exist", "BackupMyMail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (string.IsNullOrEmpty(tbl))
                return false;
            else
                return true;
        }

        private void DeleteAutostartTask()
        {
            if (!IsSetAutostartApp())
                return;

            try
            {
                Process p = new Process();
                if (System.Environment.OSVersion.Version.Major >= 6)
                    p.StartInfo.Verb = "runas";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "SCHTASKS.exe";
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                p.StartInfo.Arguments = String.Format("/delete /TN {0} /f",
                                        scheduledTaskName);

                p.Start();
                string error = p.StandardError.ReadToEnd();

                p.StandardOutput.ReadLine();
                string output = p.StandardOutput.ReadToEnd();

                p.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error remove autostart task\n" + ex.Message, scheduledTaskName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateAutostartTask()
        {
            if (IsSetAutostartApp())
                return;

            try
            {
                Process p = new Process();
                if (System.Environment.OSVersion.Version.Major >= 6)
                    p.StartInfo.Verb = "runas";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "SCHTASKS.exe";
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                p.StartInfo.Arguments = String.Format("/create /sc onlogon /tn {0} /rl highest /tr \"\\\"{1}\\\" {2}\"",
                                        scheduledTaskName,
                                        Assembly.GetEntryAssembly().Location,
                                        "/min"); //minimalize window

                p.Start();
                string error = p.StandardError.ReadToEnd();

                p.StandardOutput.ReadLine();
                string output = p.StandardOutput.ReadToEnd();

                p.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to add autostart task\n" + ex.Message, scheduledTaskName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                if (CB_autoStartApp.Checked)
                {
                    autostart = true;

                    //default option if user don't change control
                    CreateAutostartTask();
                }
                else
                    autostart = false;

                bool enableRepeat = false;
                if (CB_repeatEvery.Checked)
                    enableRepeat = true;

                int repeatNumber = 0;
                Double everyNum = Double.Parse(NUD_repeatEveryNum.Value.ToString());
                if (everyNum > 0)
                    repeatNumber = (int)everyNum;

                //0 - hour, 1 - day, 2 - weekday
                int repeatKind = CB_repeadScheduleKind.SelectedIndex;

                Version version = Assembly.GetEntryAssembly().GetName().Version;

                using (var textWriter = new XmlTextWriter(pathToSettings, null))
                {
                    textWriter.WriteStartDocument();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteComment("Backup My Mail for Microsoft Outlook - file settings");
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteStartElement("ApplicationSettings");
                    textWriter.WriteAttributeString("Version", version.ToString());
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");                    
                    textWriter.WriteStartElement("EnableRepeat", enableRepeat.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("FolderBackup", TB_pathToBackupOutputFolder.Text);                    
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("RepeatKind", repeatKind.ToString());                    
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("RepeatNumber", repeatNumber.ToString());                    
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("AfterBackupCloseComputer", afterBackupCloseComputer.ToString());
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
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("NotUseVss", notUseVss.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("Autostart", autostart.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("DeleteAllPstFromBackupFolder", deleteAllPstFromBackupFolder.ToString());
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
                            if (String.Compare(reader.Name, "ApplicationSettings", false) == 0)
                            {
                                if (reader.AttributeCount > 0)
                                {
                                    reader.MoveToAttribute("Version");
                                    try
                                    {
                                        appSettingsVersion = new Version(reader.Value);
                                    }
                                    catch { }
                                }
                            }
                            if (String.Compare(reader.Name, "FolderBackup", false) == 0)
                            {
                                folderBackup = reader.NamespaceURI;
                                TB_pathToBackupOutputFolder.Text = folderBackup;
                            }
                            if (String.Compare(reader.Name, "ActualSetSchedule", false) == 0)
                            {
                                L_actualScheduleSet.Text = reader.NamespaceURI;
                            }
                            if (String.Compare(reader.Name, "EnableRepeat", false) == 0)
                            {
                                enableRepeat = bool.Parse(reader.NamespaceURI);
                                if (enableRepeat)
                                    CB_repeatEvery.Checked = true;
                                else
                                    CB_repeatEvery.Checked = false;
                            }
                            if (String.Compare(reader.Name, "RepeatNumber", false) == 0)
                            {
                                repeatNumber = Int32.Parse(reader.NamespaceURI);
                                if (repeatNumber > -1 && repeatNumber < 10)
                                    NUD_repeatEveryNum.Value = (decimal)repeatNumber;
                                else
                                    NUD_repeatEveryNum.Value = 0;
                            }
                            if (String.Compare(reader.Name, "RepeatKind", false) == 0)
                            {
                                //0 - hour, 1 - day, 2 - weekday
                                repeatKind = Int32.Parse(reader.NamespaceURI);
                                if (repeatKind > -1 && repeatKind < 4)
                                    CB_repeadScheduleKind.SelectedIndex = repeatKind;
                                else
                                    CB_repeadScheduleKind.SelectedIndex = 0;
                            }
                            if (String.Compare(reader.Name, "ScheduleTime", false) == 0)
                            {
                                if (String.Compare(reader.NamespaceURI, string.Empty, false) != 0)
                                {
                                    try
                                    {
                                        scheduleRun = DateTime.Parse(reader.NamespaceURI);
                                        DTP_startScheduleDate.Value = scheduleRun.Date;
                                        NUD_startHourSchedule.Value = (decimal)scheduleRun.Hour;
                                        NUD_startMinuteSchedule.Value = (decimal)scheduleRun.Minute;

                                        if (!SetNextScheduler())
                                        {
                                            L_actualScheduleSet.Text = string.Empty;                                         
                                        }

                                        if (scheduleRun > DateTime.Now)
                                        {
                                            DTP_startScheduleDate.Value = scheduleRun.Date;
                                            NUD_startHourSchedule.Value = (decimal)scheduleRun.Hour;
                                            NUD_startMinuteSchedule.Value = (decimal)scheduleRun.Minute;
                                            this.NI_F_Main.Icon = new Icon(GetType(), "schedule.ico");
                                            this.Icon = new Icon(GetType(), "schedule.ico");
                                        }
                                        else
                                        {
                                            this.NI_F_Main.Icon = new Icon(GetType(), "normal.ico");
                                            this.Icon = new Icon(GetType(), "normal.ico");
                                        }
                                    }
                                    catch { }
                                }
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
                            if (String.Compare(reader.Name, "PathToLog", false) == 0)
                            {
                                pathToLog = reader.NamespaceURI;
                            }
                            if (String.Compare(reader.Name, "Autostart", false) == 0)
                            {
                                autostart = bool.Parse(reader.NamespaceURI);
                                if (autostart)
                                    CB_autoStartApp.Checked = true;
                                else
                                    CB_autoStartApp.Checked = false;
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

        private bool IsDirectoryAccessibleRW(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                string pathToFile = Path.Combine(path, "test");
                File.WriteAllText(pathToFile, "test");
                File.Delete(pathToFile);
                return true;
            }
            catch
            {
                return false; //directory is not correctly
            }
        }

        private bool SetNextScheduler()
        {
            if (!enableRepeat) return false; //repeat disable
            if (string.IsNullOrEmpty(folderBackup)) return false; //path to folder backup is empty
            if (!IsDirectoryAccessibleRW(folderBackup)) return false; //directory isn't accessible

            //int maxIterations = 0;
            while (scheduleRun < DateTime.Now)
            {
                //for safe (badly settings in file)
                if (repeatNumber < 1) break;
                if (repeatKind < 1 || repeatKind > 2) break;

                //repeatKind => 0 - hour, 1 - day, 2 - weekday
                if (repeatKind == 0)
                {
                    scheduleRun = scheduleRun.AddHours(repeatNumber);
                    NUD_startHourSchedule.Value = (decimal)scheduleRun.Hour;
                }
                else if (repeatKind == 1)
                {
                    scheduleRun = scheduleRun.AddDays(repeatNumber);                  
                }
                else if (repeatKind == 2)
                {
                    scheduleRun = scheduleRun.AddDays(repeatNumber * 7);
                }
                DTP_startScheduleDate.Value = scheduleRun;

                ////for safe (badly settings in file)
                //if (++maxIterations > 100) break;
            }

            if (scheduleRun < DateTime.Now)
            {
                return false;
            }

            return true;
        }

        private System.Security.SecureString StringToSecureString(string input)
        {
            System.Security.SecureString secureString = new System.Security.SecureString();
            foreach (char c in input.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
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
            closeProgram = true;
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
            Process.Start("https://github.com/MarekOtulakowski/backupmymail");
        }

        private void B_addSchedule_Click(object sender, EventArgs e)
        {            
            if (string.IsNullOrEmpty(TB_pathToBackupOutputFolder.Text))
            {
                MessageBox.Show("Cannot add schedule if path to backup folder is empty!",
                "Add schedule time",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }

            if (!IsDirectoryAccessibleRW(folderBackup))
            {
                //directory isn't accessible
                MessageBox.Show("Cannot add schedule if folder backup isn't accessible with Read-Write laws!",
                "Add schedule time",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            } 

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
                MessageBoxIcon.Warning);
                return;
            }

            scheduleRun = userChooseDateAndTime;
            SaveXML();

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
            SetDefaultScheduler();

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
            SaveXML();
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
            if (System.Environment.OSVersion.Version.Major >= 6)
                psiRestoreReg.Verb = "runas";
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

        private void CB_autoStartApp_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_autoStartApp.Checked)
                CreateAutostartTask();
            else
                DeleteAutostartTask();
        }
    }
}
