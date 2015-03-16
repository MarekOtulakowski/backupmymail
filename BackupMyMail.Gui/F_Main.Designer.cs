namespace BackupMyMail.Gui
{
    partial class F_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Main));
            this.TP_others = new System.Windows.Forms.TabPage();
            this.CB_autoStartApp = new System.Windows.Forms.CheckBox();
            this.L_backupRegistryInfo = new System.Windows.Forms.Label();
            this.CB_notUseVss = new System.Windows.Forms.CheckBox();
            this.CB_copyRegistrySettings = new System.Windows.Forms.CheckBox();
            this.B_browseLogFile = new System.Windows.Forms.Button();
            this.L_descriptionPathToFile = new System.Windows.Forms.Label();
            this.TB_pathToLogFile = new System.Windows.Forms.TextBox();
            this.LL_homeWebsite = new System.Windows.Forms.LinkLabel();
            this.CB_minimalizeWindowOnStartup = new System.Windows.Forms.CheckBox();
            this.CB_afterBackupCloseComputer = new System.Windows.Forms.CheckBox();
            this.CB_deleteAllPstFileFromBackup = new System.Windows.Forms.CheckBox();
            this.TP_backup = new System.Windows.Forms.TabPage();
            this.GB_accountPermission = new System.Windows.Forms.GroupBox();
            this.LB_password = new System.Windows.Forms.Label();
            this.L_userName = new System.Windows.Forms.Label();
            this.TB_passWord = new System.Windows.Forms.TextBox();
            this.TB_userName = new System.Windows.Forms.TextBox();
            this.L_actualState = new System.Windows.Forms.Label();
            this.L_labelActualState = new System.Windows.Forms.Label();
            this.B_browseBackupOutputFolder = new System.Windows.Forms.Button();
            this.L_infoBackupPath = new System.Windows.Forms.Label();
            this.TB_pathToBackupOutputFolder = new System.Windows.Forms.TextBox();
            this.B_terminateBackupNow = new System.Windows.Forms.Button();
            this.B_startBackupNow = new System.Windows.Forms.Button();
            this.TC_Main = new System.Windows.Forms.TabControl();
            this.TP_schedule = new System.Windows.Forms.TabPage();
            this.B_saveSettings = new System.Windows.Forms.Button();
            this.L_actualScheduleSet = new System.Windows.Forms.Label();
            this.L_descriptionScheduleState = new System.Windows.Forms.Label();
            this.GB_addEditRemoveSchedule = new System.Windows.Forms.GroupBox();
            this.B_removeSchedule = new System.Windows.Forms.Button();
            this.B_editSchedule = new System.Windows.Forms.Button();
            this.CB_repeadScheduleKind = new System.Windows.Forms.ComboBox();
            this.L_descriptionStartTimeSchedule = new System.Windows.Forms.Label();
            this.L_descriptionRepeatSchedule = new System.Windows.Forms.Label();
            this.NUD_repeatEveryNum = new System.Windows.Forms.NumericUpDown();
            this.CB_repeatEvery = new System.Windows.Forms.CheckBox();
            this.B_addSchedule = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.L_descriptionStartDateSchedule = new System.Windows.Forms.Label();
            this.NUD_startMinuteSchedule = new System.Windows.Forms.NumericUpDown();
            this.NUD_startHourSchedule = new System.Windows.Forms.NumericUpDown();
            this.DTP_startScheduleDate = new System.Windows.Forms.DateTimePicker();
            this.TP_restore = new System.Windows.Forms.TabPage();
            this.GB_pathsToRestorePst = new System.Windows.Forms.GroupBox();
            this.GB_pathToPstInfo = new System.Windows.Forms.GroupBox();
            this.L_pathToDefaultPst = new System.Windows.Forms.Label();
            this.B_browsePathToPstInfo = new System.Windows.Forms.Button();
            this.TB_pathToPstInfo = new System.Windows.Forms.TextBox();
            this.B_readPstInfo = new System.Windows.Forms.Button();
            this.GB_restorePstS = new System.Windows.Forms.GroupBox();
            this.L_copyInfoAdd2 = new System.Windows.Forms.Label();
            this.L_copyInfoAdd = new System.Windows.Forms.Label();
            this.B_terminateCopy = new System.Windows.Forms.Button();
            this.L_copyState = new System.Windows.Forms.Label();
            this.B_restoreDefaultPstAndArchivePstS = new System.Windows.Forms.Button();
            this.B_restoreDefaultPST = new System.Windows.Forms.Button();
            this.B_browseRegOutlookRestore = new System.Windows.Forms.Button();
            this.TB_pathToRegOutlookRestore = new System.Windows.Forms.TextBox();
            this.B_restoreOutlookRegistry = new System.Windows.Forms.Button();
            this.NI_F_Main = new System.Windows.Forms.NotifyIcon(this.components);
            this.CMS_NI_F_Main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SWTSMI_showWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.SWTSMI_backupNow = new System.Windows.Forms.ToolStripMenuItem();
            this.SWTSMI_separate1 = new System.Windows.Forms.ToolStripSeparator();
            this.SWTSMI_closeProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.TP_others.SuspendLayout();
            this.TP_backup.SuspendLayout();
            this.GB_accountPermission.SuspendLayout();
            this.TC_Main.SuspendLayout();
            this.TP_schedule.SuspendLayout();
            this.GB_addEditRemoveSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_repeatEveryNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_startMinuteSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_startHourSchedule)).BeginInit();
            this.TP_restore.SuspendLayout();
            this.GB_pathsToRestorePst.SuspendLayout();
            this.GB_pathToPstInfo.SuspendLayout();
            this.GB_restorePstS.SuspendLayout();
            this.CMS_NI_F_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // TP_others
            // 
            this.TP_others.Controls.Add(this.CB_autoStartApp);
            this.TP_others.Controls.Add(this.L_backupRegistryInfo);
            this.TP_others.Controls.Add(this.CB_notUseVss);
            this.TP_others.Controls.Add(this.CB_copyRegistrySettings);
            this.TP_others.Controls.Add(this.B_browseLogFile);
            this.TP_others.Controls.Add(this.L_descriptionPathToFile);
            this.TP_others.Controls.Add(this.TB_pathToLogFile);
            this.TP_others.Controls.Add(this.LL_homeWebsite);
            this.TP_others.Controls.Add(this.CB_minimalizeWindowOnStartup);
            this.TP_others.Controls.Add(this.CB_afterBackupCloseComputer);
            this.TP_others.Controls.Add(this.CB_deleteAllPstFileFromBackup);
            this.TP_others.Location = new System.Drawing.Point(4, 22);
            this.TP_others.Name = "TP_others";
            this.TP_others.Padding = new System.Windows.Forms.Padding(3);
            this.TP_others.Size = new System.Drawing.Size(410, 291);
            this.TP_others.TabIndex = 1;
            this.TP_others.Text = "Others";
            this.TP_others.UseVisualStyleBackColor = true;
            // 
            // CB_autoStartApp
            // 
            this.CB_autoStartApp.AutoSize = true;
            this.CB_autoStartApp.Checked = true;
            this.CB_autoStartApp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_autoStartApp.Location = new System.Drawing.Point(18, 158);
            this.CB_autoStartApp.Name = "CB_autoStartApp";
            this.CB_autoStartApp.Size = new System.Drawing.Size(194, 17);
            this.CB_autoStartApp.TabIndex = 18;
            this.CB_autoStartApp.Tag = "";
            this.CB_autoStartApp.Text = "Autostart application after user login";
            this.CB_autoStartApp.UseVisualStyleBackColor = true;
            this.CB_autoStartApp.CheckedChanged += new System.EventHandler(this.CB_autoStartApp_CheckedChanged);
            // 
            // L_backupRegistryInfo
            // 
            this.L_backupRegistryInfo.AutoSize = true;
            this.L_backupRegistryInfo.Location = new System.Drawing.Point(15, 264);
            this.L_backupRegistryInfo.Name = "L_backupRegistryInfo";
            this.L_backupRegistryInfo.Size = new System.Drawing.Size(376, 13);
            this.L_backupRegistryInfo.TabIndex = 17;
            this.L_backupRegistryInfo.Text = "\"Backup registy\" copy specyfic path in MS Registy. Other in MS Outlook 2013";
            // 
            // CB_notUseVss
            // 
            this.CB_notUseVss.AutoSize = true;
            this.CB_notUseVss.Location = new System.Drawing.Point(18, 129);
            this.CB_notUseVss.Name = "CB_notUseVss";
            this.CB_notUseVss.Size = new System.Drawing.Size(341, 17);
            this.CB_notUseVss.TabIndex = 16;
            this.CB_notUseVss.Tag = "";
            this.CB_notUseVss.Text = "No use VSS (MS Outlook must be disable when backup is running)";
            this.CB_notUseVss.UseVisualStyleBackColor = true;
            // 
            // CB_copyRegistrySettings
            // 
            this.CB_copyRegistrySettings.AutoSize = true;
            this.CB_copyRegistrySettings.Checked = true;
            this.CB_copyRegistrySettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_copyRegistrySettings.Location = new System.Drawing.Point(18, 100);
            this.CB_copyRegistrySettings.Name = "CB_copyRegistrySettings";
            this.CB_copyRegistrySettings.Size = new System.Drawing.Size(367, 17);
            this.CB_copyRegistrySettings.TabIndex = 15;
            this.CB_copyRegistrySettings.Text = "Backup registry settings (for all Outlook profiles in this Windows account)";
            this.CB_copyRegistrySettings.UseVisualStyleBackColor = true;
            // 
            // B_browseLogFile
            // 
            this.B_browseLogFile.Location = new System.Drawing.Point(301, 204);
            this.B_browseLogFile.Name = "B_browseLogFile";
            this.B_browseLogFile.Size = new System.Drawing.Size(75, 23);
            this.B_browseLogFile.TabIndex = 13;
            this.B_browseLogFile.Text = "Browse";
            this.B_browseLogFile.UseVisualStyleBackColor = true;
            this.B_browseLogFile.Click += new System.EventHandler(this.B_browseLogFile_Click);
            // 
            // L_descriptionPathToFile
            // 
            this.L_descriptionPathToFile.AutoSize = true;
            this.L_descriptionPathToFile.Location = new System.Drawing.Point(15, 190);
            this.L_descriptionPathToFile.Name = "L_descriptionPathToFile";
            this.L_descriptionPathToFile.Size = new System.Drawing.Size(74, 13);
            this.L_descriptionPathToFile.TabIndex = 12;
            this.L_descriptionPathToFile.Text = "Path to log file";
            // 
            // TB_pathToLogFile
            // 
            this.TB_pathToLogFile.Location = new System.Drawing.Point(18, 206);
            this.TB_pathToLogFile.Name = "TB_pathToLogFile";
            this.TB_pathToLogFile.ReadOnly = true;
            this.TB_pathToLogFile.Size = new System.Drawing.Size(277, 20);
            this.TB_pathToLogFile.TabIndex = 11;
            // 
            // LL_homeWebsite
            // 
            this.LL_homeWebsite.AutoSize = true;
            this.LL_homeWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LL_homeWebsite.Location = new System.Drawing.Point(281, 238);
            this.LL_homeWebsite.Name = "LL_homeWebsite";
            this.LL_homeWebsite.Size = new System.Drawing.Size(91, 13);
            this.LL_homeWebsite.TabIndex = 5;
            this.LL_homeWebsite.TabStop = true;
            this.LL_homeWebsite.Text = "project\'s websitee";
            this.LL_homeWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_homeWebsite_LinkClicked);
            // 
            // CB_minimalizeWindowOnStartup
            // 
            this.CB_minimalizeWindowOnStartup.AutoSize = true;
            this.CB_minimalizeWindowOnStartup.Location = new System.Drawing.Point(18, 71);
            this.CB_minimalizeWindowOnStartup.Name = "CB_minimalizeWindowOnStartup";
            this.CB_minimalizeWindowOnStartup.Size = new System.Drawing.Size(197, 17);
            this.CB_minimalizeWindowOnStartup.TabIndex = 4;
            this.CB_minimalizeWindowOnStartup.Text = "Minimalize program to tray on startup";
            this.CB_minimalizeWindowOnStartup.UseVisualStyleBackColor = true;
            // 
            // CB_afterBackupCloseComputer
            // 
            this.CB_afterBackupCloseComputer.AutoSize = true;
            this.CB_afterBackupCloseComputer.Location = new System.Drawing.Point(18, 42);
            this.CB_afterBackupCloseComputer.Name = "CB_afterBackupCloseComputer";
            this.CB_afterBackupCloseComputer.Size = new System.Drawing.Size(235, 17);
            this.CB_afterBackupCloseComputer.TabIndex = 3;
            this.CB_afterBackupCloseComputer.Text = "Shutdown Windows after backup completes";
            this.CB_afterBackupCloseComputer.UseVisualStyleBackColor = true;
            this.CB_afterBackupCloseComputer.CheckedChanged += new System.EventHandler(this.CB_afterBackupCloseComputer_CheckedChanged);
            // 
            // CB_deleteAllPstFileFromBackup
            // 
            this.CB_deleteAllPstFileFromBackup.AutoSize = true;
            this.CB_deleteAllPstFileFromBackup.Location = new System.Drawing.Point(18, 13);
            this.CB_deleteAllPstFileFromBackup.Name = "CB_deleteAllPstFileFromBackup";
            this.CB_deleteAllPstFileFromBackup.Size = new System.Drawing.Size(274, 17);
            this.CB_deleteAllPstFileFromBackup.TabIndex = 2;
            this.CB_deleteAllPstFileFromBackup.Text = "Delete all pst, xml, reg files from output folder backup";
            this.CB_deleteAllPstFileFromBackup.UseVisualStyleBackColor = true;
            // 
            // TP_backup
            // 
            this.TP_backup.Controls.Add(this.GB_accountPermission);
            this.TP_backup.Controls.Add(this.L_actualState);
            this.TP_backup.Controls.Add(this.L_labelActualState);
            this.TP_backup.Controls.Add(this.B_browseBackupOutputFolder);
            this.TP_backup.Controls.Add(this.L_infoBackupPath);
            this.TP_backup.Controls.Add(this.TB_pathToBackupOutputFolder);
            this.TP_backup.Controls.Add(this.B_terminateBackupNow);
            this.TP_backup.Controls.Add(this.B_startBackupNow);
            this.TP_backup.Location = new System.Drawing.Point(4, 22);
            this.TP_backup.Name = "TP_backup";
            this.TP_backup.Padding = new System.Windows.Forms.Padding(3);
            this.TP_backup.Size = new System.Drawing.Size(410, 291);
            this.TP_backup.TabIndex = 0;
            this.TP_backup.Text = "Backup";
            this.TP_backup.UseVisualStyleBackColor = true;
            // 
            // GB_accountPermission
            // 
            this.GB_accountPermission.Controls.Add(this.LB_password);
            this.GB_accountPermission.Controls.Add(this.L_userName);
            this.GB_accountPermission.Controls.Add(this.TB_passWord);
            this.GB_accountPermission.Controls.Add(this.TB_userName);
            this.GB_accountPermission.Location = new System.Drawing.Point(26, 68);
            this.GB_accountPermission.Name = "GB_accountPermission";
            this.GB_accountPermission.Size = new System.Drawing.Size(357, 116);
            this.GB_accountPermission.TabIndex = 14;
            this.GB_accountPermission.TabStop = false;
            this.GB_accountPermission.Text = "Use bellow administration account";
            this.GB_accountPermission.Visible = false;
            // 
            // LB_password
            // 
            this.LB_password.AutoSize = true;
            this.LB_password.Location = new System.Drawing.Point(42, 71);
            this.LB_password.Name = "LB_password";
            this.LB_password.Size = new System.Drawing.Size(53, 13);
            this.LB_password.TabIndex = 3;
            this.LB_password.Text = "Password";
            // 
            // L_userName
            // 
            this.L_userName.AutoSize = true;
            this.L_userName.Location = new System.Drawing.Point(40, 36);
            this.L_userName.Name = "L_userName";
            this.L_userName.Size = new System.Drawing.Size(55, 13);
            this.L_userName.TabIndex = 2;
            this.L_userName.Text = "Username";
            // 
            // TB_passWord
            // 
            this.TB_passWord.Location = new System.Drawing.Point(101, 68);
            this.TB_passWord.Name = "TB_passWord";
            this.TB_passWord.PasswordChar = '*';
            this.TB_passWord.Size = new System.Drawing.Size(208, 20);
            this.TB_passWord.TabIndex = 1;
            // 
            // TB_userName
            // 
            this.TB_userName.Location = new System.Drawing.Point(101, 33);
            this.TB_userName.Name = "TB_userName";
            this.TB_userName.Size = new System.Drawing.Size(208, 20);
            this.TB_userName.TabIndex = 0;
            // 
            // L_actualState
            // 
            this.L_actualState.AutoSize = true;
            this.L_actualState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.L_actualState.Location = new System.Drawing.Point(86, 206);
            this.L_actualState.Name = "L_actualState";
            this.L_actualState.Size = new System.Drawing.Size(27, 13);
            this.L_actualState.TabIndex = 13;
            this.L_actualState.Text = "null";
            // 
            // L_labelActualState
            // 
            this.L_labelActualState.AutoSize = true;
            this.L_labelActualState.Location = new System.Drawing.Point(23, 206);
            this.L_labelActualState.Name = "L_labelActualState";
            this.L_labelActualState.Size = new System.Drawing.Size(69, 13);
            this.L_labelActualState.TabIndex = 12;
            this.L_labelActualState.Text = "Actual state: ";
            // 
            // B_browseBackupOutputFolder
            // 
            this.B_browseBackupOutputFolder.Location = new System.Drawing.Point(309, 28);
            this.B_browseBackupOutputFolder.Name = "B_browseBackupOutputFolder";
            this.B_browseBackupOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.B_browseBackupOutputFolder.TabIndex = 10;
            this.B_browseBackupOutputFolder.Text = "Browse";
            this.B_browseBackupOutputFolder.UseVisualStyleBackColor = true;
            this.B_browseBackupOutputFolder.Click += new System.EventHandler(this.B_browseBackupOutputFolder_Click);
            // 
            // L_infoBackupPath
            // 
            this.L_infoBackupPath.AutoSize = true;
            this.L_infoBackupPath.Location = new System.Drawing.Point(23, 14);
            this.L_infoBackupPath.Name = "L_infoBackupPath";
            this.L_infoBackupPath.Size = new System.Drawing.Size(112, 13);
            this.L_infoBackupPath.TabIndex = 9;
            this.L_infoBackupPath.Text = "Path to backup folder:";
            // 
            // TB_pathToBackupOutputFolder
            // 
            this.TB_pathToBackupOutputFolder.Location = new System.Drawing.Point(26, 30);
            this.TB_pathToBackupOutputFolder.Name = "TB_pathToBackupOutputFolder";
            this.TB_pathToBackupOutputFolder.Size = new System.Drawing.Size(277, 20);
            this.TB_pathToBackupOutputFolder.TabIndex = 8;
            // 
            // B_terminateBackupNow
            // 
            this.B_terminateBackupNow.Location = new System.Drawing.Point(261, 226);
            this.B_terminateBackupNow.Name = "B_terminateBackupNow";
            this.B_terminateBackupNow.Size = new System.Drawing.Size(122, 23);
            this.B_terminateBackupNow.TabIndex = 7;
            this.B_terminateBackupNow.Text = "Terminate now";
            this.B_terminateBackupNow.UseVisualStyleBackColor = true;
            this.B_terminateBackupNow.Click += new System.EventHandler(this.B_terminateBackupNow_Click);
            // 
            // B_startBackupNow
            // 
            this.B_startBackupNow.Location = new System.Drawing.Point(26, 226);
            this.B_startBackupNow.Name = "B_startBackupNow";
            this.B_startBackupNow.Size = new System.Drawing.Size(122, 23);
            this.B_startBackupNow.TabIndex = 6;
            this.B_startBackupNow.Text = "Backup now";
            this.B_startBackupNow.UseVisualStyleBackColor = true;
            this.B_startBackupNow.Click += new System.EventHandler(this.B_startBackupNow_Click);
            // 
            // TC_Main
            // 
            this.TC_Main.Controls.Add(this.TP_backup);
            this.TC_Main.Controls.Add(this.TP_schedule);
            this.TC_Main.Controls.Add(this.TP_restore);
            this.TC_Main.Controls.Add(this.TP_others);
            this.TC_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TC_Main.Location = new System.Drawing.Point(0, 0);
            this.TC_Main.Name = "TC_Main";
            this.TC_Main.SelectedIndex = 0;
            this.TC_Main.Size = new System.Drawing.Size(418, 317);
            this.TC_Main.TabIndex = 6;
            // 
            // TP_schedule
            // 
            this.TP_schedule.Controls.Add(this.B_saveSettings);
            this.TP_schedule.Controls.Add(this.L_actualScheduleSet);
            this.TP_schedule.Controls.Add(this.L_descriptionScheduleState);
            this.TP_schedule.Controls.Add(this.GB_addEditRemoveSchedule);
            this.TP_schedule.Location = new System.Drawing.Point(4, 22);
            this.TP_schedule.Name = "TP_schedule";
            this.TP_schedule.Padding = new System.Windows.Forms.Padding(3);
            this.TP_schedule.Size = new System.Drawing.Size(410, 291);
            this.TP_schedule.TabIndex = 2;
            this.TP_schedule.Text = "Schedule";
            this.TP_schedule.UseVisualStyleBackColor = true;
            // 
            // B_saveSettings
            // 
            this.B_saveSettings.Location = new System.Drawing.Point(313, 251);
            this.B_saveSettings.Name = "B_saveSettings";
            this.B_saveSettings.Size = new System.Drawing.Size(75, 23);
            this.B_saveSettings.TabIndex = 4;
            this.B_saveSettings.Text = "Save settigs";
            this.B_saveSettings.UseVisualStyleBackColor = true;
            this.B_saveSettings.Visible = false;
            // 
            // L_actualScheduleSet
            // 
            this.L_actualScheduleSet.AutoSize = true;
            this.L_actualScheduleSet.Location = new System.Drawing.Point(110, 218);
            this.L_actualScheduleSet.Name = "L_actualScheduleSet";
            this.L_actualScheduleSet.Size = new System.Drawing.Size(23, 13);
            this.L_actualScheduleSet.TabIndex = 3;
            this.L_actualScheduleSet.Text = "null";
            // 
            // L_descriptionScheduleState
            // 
            this.L_descriptionScheduleState.AutoSize = true;
            this.L_descriptionScheduleState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.L_descriptionScheduleState.Location = new System.Drawing.Point(12, 218);
            this.L_descriptionScheduleState.Name = "L_descriptionScheduleState";
            this.L_descriptionScheduleState.Size = new System.Drawing.Size(106, 13);
            this.L_descriptionScheduleState.TabIndex = 2;
            this.L_descriptionScheduleState.Text = "Actual schedule: ";
            // 
            // GB_addEditRemoveSchedule
            // 
            this.GB_addEditRemoveSchedule.Controls.Add(this.B_removeSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.B_editSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.CB_repeadScheduleKind);
            this.GB_addEditRemoveSchedule.Controls.Add(this.L_descriptionStartTimeSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.L_descriptionRepeatSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.NUD_repeatEveryNum);
            this.GB_addEditRemoveSchedule.Controls.Add(this.CB_repeatEvery);
            this.GB_addEditRemoveSchedule.Controls.Add(this.B_addSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.label3);
            this.GB_addEditRemoveSchedule.Controls.Add(this.label2);
            this.GB_addEditRemoveSchedule.Controls.Add(this.L_descriptionStartDateSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.NUD_startMinuteSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.NUD_startHourSchedule);
            this.GB_addEditRemoveSchedule.Controls.Add(this.DTP_startScheduleDate);
            this.GB_addEditRemoveSchedule.Location = new System.Drawing.Point(14, 6);
            this.GB_addEditRemoveSchedule.Name = "GB_addEditRemoveSchedule";
            this.GB_addEditRemoveSchedule.Size = new System.Drawing.Size(380, 203);
            this.GB_addEditRemoveSchedule.TabIndex = 1;
            this.GB_addEditRemoveSchedule.TabStop = false;
            this.GB_addEditRemoveSchedule.Text = "Add/Edit/Remove scheduled time";
            // 
            // B_removeSchedule
            // 
            this.B_removeSchedule.Location = new System.Drawing.Point(299, 164);
            this.B_removeSchedule.Name = "B_removeSchedule";
            this.B_removeSchedule.Size = new System.Drawing.Size(75, 23);
            this.B_removeSchedule.TabIndex = 16;
            this.B_removeSchedule.Text = "Remove";
            this.B_removeSchedule.UseVisualStyleBackColor = true;
            this.B_removeSchedule.Click += new System.EventHandler(this.B_removeSchedule_Click);
            // 
            // B_editSchedule
            // 
            this.B_editSchedule.Location = new System.Drawing.Point(218, 164);
            this.B_editSchedule.Name = "B_editSchedule";
            this.B_editSchedule.Size = new System.Drawing.Size(75, 23);
            this.B_editSchedule.TabIndex = 15;
            this.B_editSchedule.Text = "Edit";
            this.B_editSchedule.UseVisualStyleBackColor = true;
            this.B_editSchedule.Click += new System.EventHandler(this.B_editSchedule_Click);
            // 
            // CB_repeadScheduleKind
            // 
            this.CB_repeadScheduleKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_repeadScheduleKind.FormattingEnabled = true;
            this.CB_repeadScheduleKind.Items.AddRange(new object[] {
            "hour(s)",
            "day(s)",
            "weekday(s)"});
            this.CB_repeadScheduleKind.Location = new System.Drawing.Point(132, 89);
            this.CB_repeadScheduleKind.Name = "CB_repeadScheduleKind";
            this.CB_repeadScheduleKind.Size = new System.Drawing.Size(121, 21);
            this.CB_repeadScheduleKind.TabIndex = 14;
            // 
            // L_descriptionStartTimeSchedule
            // 
            this.L_descriptionStartTimeSchedule.AutoSize = true;
            this.L_descriptionStartTimeSchedule.Location = new System.Drawing.Point(199, 20);
            this.L_descriptionStartTimeSchedule.Name = "L_descriptionStartTimeSchedule";
            this.L_descriptionStartTimeSchedule.Size = new System.Drawing.Size(54, 13);
            this.L_descriptionStartTimeSchedule.TabIndex = 13;
            this.L_descriptionStartTimeSchedule.Text = "Start time:";
            // 
            // L_descriptionRepeatSchedule
            // 
            this.L_descriptionRepeatSchedule.AutoSize = true;
            this.L_descriptionRepeatSchedule.Location = new System.Drawing.Point(18, 74);
            this.L_descriptionRepeatSchedule.Name = "L_descriptionRepeatSchedule";
            this.L_descriptionRepeatSchedule.Size = new System.Drawing.Size(45, 13);
            this.L_descriptionRepeatSchedule.TabIndex = 12;
            this.L_descriptionRepeatSchedule.Text = "Repeat:";
            // 
            // NUD_repeatEveryNum
            // 
            this.NUD_repeatEveryNum.Location = new System.Drawing.Point(80, 89);
            this.NUD_repeatEveryNum.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.NUD_repeatEveryNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUD_repeatEveryNum.Name = "NUD_repeatEveryNum";
            this.NUD_repeatEveryNum.Size = new System.Drawing.Size(41, 20);
            this.NUD_repeatEveryNum.TabIndex = 9;
            this.NUD_repeatEveryNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CB_repeatEvery
            // 
            this.CB_repeatEvery.AutoSize = true;
            this.CB_repeatEvery.Location = new System.Drawing.Point(21, 90);
            this.CB_repeatEvery.Name = "CB_repeatEvery";
            this.CB_repeatEvery.Size = new System.Drawing.Size(53, 17);
            this.CB_repeatEvery.TabIndex = 8;
            this.CB_repeatEvery.Text = "Every";
            this.CB_repeatEvery.UseVisualStyleBackColor = true;
            // 
            // B_addSchedule
            // 
            this.B_addSchedule.Location = new System.Drawing.Point(137, 164);
            this.B_addSchedule.Name = "B_addSchedule";
            this.B_addSchedule.Size = new System.Drawing.Size(75, 23);
            this.B_addSchedule.TabIndex = 6;
            this.B_addSchedule.Text = "Add/Update";
            this.B_addSchedule.UseVisualStyleBackColor = true;
            this.B_addSchedule.Click += new System.EventHandler(this.B_addSchedule_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(282, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Minute";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hour";
            // 
            // L_descriptionStartDateSchedule
            // 
            this.L_descriptionStartDateSchedule.AutoSize = true;
            this.L_descriptionStartDateSchedule.Location = new System.Drawing.Point(18, 20);
            this.L_descriptionStartDateSchedule.Name = "L_descriptionStartDateSchedule";
            this.L_descriptionStartDateSchedule.Size = new System.Drawing.Size(56, 13);
            this.L_descriptionStartDateSchedule.TabIndex = 3;
            this.L_descriptionStartDateSchedule.Text = "Start date:";
            // 
            // NUD_startMinuteSchedule
            // 
            this.NUD_startMinuteSchedule.Location = new System.Drawing.Point(321, 35);
            this.NUD_startMinuteSchedule.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.NUD_startMinuteSchedule.Name = "NUD_startMinuteSchedule";
            this.NUD_startMinuteSchedule.Size = new System.Drawing.Size(41, 20);
            this.NUD_startMinuteSchedule.TabIndex = 2;
            // 
            // NUD_startHourSchedule
            // 
            this.NUD_startHourSchedule.Location = new System.Drawing.Point(235, 35);
            this.NUD_startHourSchedule.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.NUD_startHourSchedule.Name = "NUD_startHourSchedule";
            this.NUD_startHourSchedule.Size = new System.Drawing.Size(41, 20);
            this.NUD_startHourSchedule.TabIndex = 1;
            // 
            // DTP_startScheduleDate
            // 
            this.DTP_startScheduleDate.Location = new System.Drawing.Point(18, 36);
            this.DTP_startScheduleDate.Name = "DTP_startScheduleDate";
            this.DTP_startScheduleDate.Size = new System.Drawing.Size(164, 20);
            this.DTP_startScheduleDate.TabIndex = 0;
            // 
            // TP_restore
            // 
            this.TP_restore.Controls.Add(this.GB_pathsToRestorePst);
            this.TP_restore.Controls.Add(this.B_browseRegOutlookRestore);
            this.TP_restore.Controls.Add(this.TB_pathToRegOutlookRestore);
            this.TP_restore.Controls.Add(this.B_restoreOutlookRegistry);
            this.TP_restore.Location = new System.Drawing.Point(4, 22);
            this.TP_restore.Name = "TP_restore";
            this.TP_restore.Padding = new System.Windows.Forms.Padding(3);
            this.TP_restore.Size = new System.Drawing.Size(410, 291);
            this.TP_restore.TabIndex = 3;
            this.TP_restore.Text = "Restore";
            this.TP_restore.UseVisualStyleBackColor = true;
            // 
            // GB_pathsToRestorePst
            // 
            this.GB_pathsToRestorePst.Controls.Add(this.GB_pathToPstInfo);
            this.GB_pathsToRestorePst.Controls.Add(this.GB_restorePstS);
            this.GB_pathsToRestorePst.Location = new System.Drawing.Point(8, 61);
            this.GB_pathsToRestorePst.Name = "GB_pathsToRestorePst";
            this.GB_pathsToRestorePst.Size = new System.Drawing.Size(394, 222);
            this.GB_pathsToRestorePst.TabIndex = 3;
            this.GB_pathsToRestorePst.TabStop = false;
            this.GB_pathsToRestorePst.Text = "Paths to PST files";
            // 
            // GB_pathToPstInfo
            // 
            this.GB_pathToPstInfo.Controls.Add(this.L_pathToDefaultPst);
            this.GB_pathToPstInfo.Controls.Add(this.B_browsePathToPstInfo);
            this.GB_pathToPstInfo.Controls.Add(this.TB_pathToPstInfo);
            this.GB_pathToPstInfo.Controls.Add(this.B_readPstInfo);
            this.GB_pathToPstInfo.Location = new System.Drawing.Point(6, 16);
            this.GB_pathToPstInfo.Name = "GB_pathToPstInfo";
            this.GB_pathToPstInfo.Size = new System.Drawing.Size(381, 86);
            this.GB_pathToPstInfo.TabIndex = 6;
            this.GB_pathToPstInfo.TabStop = false;
            this.GB_pathToPstInfo.Text = "Pst info";
            // 
            // L_pathToDefaultPst
            // 
            this.L_pathToDefaultPst.AutoSize = true;
            this.L_pathToDefaultPst.Location = new System.Drawing.Point(6, 70);
            this.L_pathToDefaultPst.Name = "L_pathToDefaultPst";
            this.L_pathToDefaultPst.Size = new System.Drawing.Size(23, 13);
            this.L_pathToDefaultPst.TabIndex = 6;
            this.L_pathToDefaultPst.Text = "null";
            // 
            // B_browsePathToPstInfo
            // 
            this.B_browsePathToPstInfo.Location = new System.Drawing.Point(301, 17);
            this.B_browsePathToPstInfo.Name = "B_browsePathToPstInfo";
            this.B_browsePathToPstInfo.Size = new System.Drawing.Size(75, 23);
            this.B_browsePathToPstInfo.TabIndex = 5;
            this.B_browsePathToPstInfo.Text = "Browse";
            this.B_browsePathToPstInfo.UseVisualStyleBackColor = true;
            this.B_browsePathToPstInfo.Click += new System.EventHandler(this.B_browsePathToPstInfo_Click);
            // 
            // TB_pathToPstInfo
            // 
            this.TB_pathToPstInfo.Location = new System.Drawing.Point(7, 19);
            this.TB_pathToPstInfo.Name = "TB_pathToPstInfo";
            this.TB_pathToPstInfo.Size = new System.Drawing.Size(289, 20);
            this.TB_pathToPstInfo.TabIndex = 4;
            // 
            // B_readPstInfo
            // 
            this.B_readPstInfo.Location = new System.Drawing.Point(6, 45);
            this.B_readPstInfo.Name = "B_readPstInfo";
            this.B_readPstInfo.Size = new System.Drawing.Size(118, 23);
            this.B_readPstInfo.TabIndex = 3;
            this.B_readPstInfo.Text = "Read pst info";
            this.B_readPstInfo.UseVisualStyleBackColor = true;
            this.B_readPstInfo.Click += new System.EventHandler(this.B_readPstInfo_Click);
            // 
            // GB_restorePstS
            // 
            this.GB_restorePstS.Controls.Add(this.L_copyInfoAdd2);
            this.GB_restorePstS.Controls.Add(this.L_copyInfoAdd);
            this.GB_restorePstS.Controls.Add(this.B_terminateCopy);
            this.GB_restorePstS.Controls.Add(this.L_copyState);
            this.GB_restorePstS.Controls.Add(this.B_restoreDefaultPstAndArchivePstS);
            this.GB_restorePstS.Controls.Add(this.B_restoreDefaultPST);
            this.GB_restorePstS.Enabled = false;
            this.GB_restorePstS.Location = new System.Drawing.Point(6, 108);
            this.GB_restorePstS.Name = "GB_restorePstS";
            this.GB_restorePstS.Size = new System.Drawing.Size(381, 108);
            this.GB_restorePstS.TabIndex = 0;
            this.GB_restorePstS.TabStop = false;
            this.GB_restorePstS.Text = "Restore Pst(s)";
            // 
            // L_copyInfoAdd2
            // 
            this.L_copyInfoAdd2.AutoSize = true;
            this.L_copyInfoAdd2.Location = new System.Drawing.Point(6, 93);
            this.L_copyInfoAdd2.Name = "L_copyInfoAdd2";
            this.L_copyInfoAdd2.Size = new System.Drawing.Size(371, 13);
            this.L_copyInfoAdd2.TabIndex = 10;
            this.L_copyInfoAdd2.Text = "If you want restore account settings (password etc.) run *.reg file from backup";
            // 
            // L_copyInfoAdd
            // 
            this.L_copyInfoAdd.AutoSize = true;
            this.L_copyInfoAdd.Location = new System.Drawing.Point(6, 80);
            this.L_copyInfoAdd.Name = "L_copyInfoAdd";
            this.L_copyInfoAdd.Size = new System.Drawing.Size(262, 13);
            this.L_copyInfoAdd.TabIndex = 9;
            this.L_copyInfoAdd.Text = "Restore buttons above, restores only choosed pst files";
            // 
            // B_terminateCopy
            // 
            this.B_terminateCopy.Location = new System.Drawing.Point(235, 50);
            this.B_terminateCopy.Name = "B_terminateCopy";
            this.B_terminateCopy.Size = new System.Drawing.Size(142, 23);
            this.B_terminateCopy.TabIndex = 8;
            this.B_terminateCopy.Text = "Terminate Copy";
            this.B_terminateCopy.UseVisualStyleBackColor = true;
            this.B_terminateCopy.Click += new System.EventHandler(this.B_terminateCopy_Click);
            // 
            // L_copyState
            // 
            this.L_copyState.AutoSize = true;
            this.L_copyState.Location = new System.Drawing.Point(6, 47);
            this.L_copyState.Name = "L_copyState";
            this.L_copyState.Size = new System.Drawing.Size(80, 13);
            this.L_copyState.TabIndex = 7;
            this.L_copyState.Text = "actual not copy";
            // 
            // B_restoreDefaultPstAndArchivePstS
            // 
            this.B_restoreDefaultPstAndArchivePstS.Location = new System.Drawing.Point(192, 21);
            this.B_restoreDefaultPstAndArchivePstS.Name = "B_restoreDefaultPstAndArchivePstS";
            this.B_restoreDefaultPstAndArchivePstS.Size = new System.Drawing.Size(185, 23);
            this.B_restoreDefaultPstAndArchivePstS.TabIndex = 6;
            this.B_restoreDefaultPstAndArchivePstS.Text = "Restore all PSTs (default & all archive)";
            this.B_restoreDefaultPstAndArchivePstS.UseVisualStyleBackColor = true;
            this.B_restoreDefaultPstAndArchivePstS.Click += new System.EventHandler(this.B_restoreDefaultPstAndArchivePstS_Click);
            // 
            // B_restoreDefaultPST
            // 
            this.B_restoreDefaultPST.Location = new System.Drawing.Point(4, 21);
            this.B_restoreDefaultPST.Name = "B_restoreDefaultPST";
            this.B_restoreDefaultPST.Size = new System.Drawing.Size(185, 23);
            this.B_restoreDefaultPST.TabIndex = 3;
            this.B_restoreDefaultPST.Text = "Restore only default PSTs";
            this.B_restoreDefaultPST.UseVisualStyleBackColor = true;
            this.B_restoreDefaultPST.Click += new System.EventHandler(this.B_restoreDefaultPST_Click);
            // 
            // B_browseRegOutlookRestore
            // 
            this.B_browseRegOutlookRestore.Location = new System.Drawing.Point(315, 4);
            this.B_browseRegOutlookRestore.Name = "B_browseRegOutlookRestore";
            this.B_browseRegOutlookRestore.Size = new System.Drawing.Size(75, 23);
            this.B_browseRegOutlookRestore.TabIndex = 2;
            this.B_browseRegOutlookRestore.Text = "Browse";
            this.B_browseRegOutlookRestore.UseVisualStyleBackColor = true;
            this.B_browseRegOutlookRestore.Click += new System.EventHandler(this.B_browseRegOutlookRestore_Click);
            // 
            // TB_pathToRegOutlookRestore
            // 
            this.TB_pathToRegOutlookRestore.Location = new System.Drawing.Point(21, 6);
            this.TB_pathToRegOutlookRestore.Name = "TB_pathToRegOutlookRestore";
            this.TB_pathToRegOutlookRestore.Size = new System.Drawing.Size(288, 20);
            this.TB_pathToRegOutlookRestore.TabIndex = 1;
            // 
            // B_restoreOutlookRegistry
            // 
            this.B_restoreOutlookRegistry.Location = new System.Drawing.Point(21, 32);
            this.B_restoreOutlookRegistry.Name = "B_restoreOutlookRegistry";
            this.B_restoreOutlookRegistry.Size = new System.Drawing.Size(156, 23);
            this.B_restoreOutlookRegistry.TabIndex = 0;
            this.B_restoreOutlookRegistry.Text = "Restore Outlook registry";
            this.B_restoreOutlookRegistry.UseVisualStyleBackColor = true;
            this.B_restoreOutlookRegistry.Click += new System.EventHandler(this.B_restoreOutlookRegistry_Click);
            // 
            // NI_F_Main
            // 
            this.NI_F_Main.ContextMenuStrip = this.CMS_NI_F_Main;
            this.NI_F_Main.Icon = ((System.Drawing.Icon)(resources.GetObject("NI_F_Main.Icon")));
            this.NI_F_Main.Text = "Backup my mail";
            this.NI_F_Main.Visible = true;
            this.NI_F_Main.DoubleClick += new System.EventHandler(this.NI_F_Main_DoubleClick);
            // 
            // CMS_NI_F_Main
            // 
            this.CMS_NI_F_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SWTSMI_showWindow,
            this.SWTSMI_backupNow,
            this.SWTSMI_separate1,
            this.SWTSMI_closeProgram});
            this.CMS_NI_F_Main.Name = "CMS_NI_F_Main";
            this.CMS_NI_F_Main.Size = new System.Drawing.Size(153, 76);
            // 
            // SWTSMI_showWindow
            // 
            this.SWTSMI_showWindow.Name = "SWTSMI_showWindow";
            this.SWTSMI_showWindow.Size = new System.Drawing.Size(152, 22);
            this.SWTSMI_showWindow.Text = "Show Window";
            this.SWTSMI_showWindow.Click += new System.EventHandler(this.SWTSMI_showWindow_Click);
            // 
            // SWTSMI_backupNow
            // 
            this.SWTSMI_backupNow.Image = ((System.Drawing.Image)(resources.GetObject("SWTSMI_backupNow.Image")));
            this.SWTSMI_backupNow.Name = "SWTSMI_backupNow";
            this.SWTSMI_backupNow.Size = new System.Drawing.Size(152, 22);
            this.SWTSMI_backupNow.Text = "Backup Now";
            this.SWTSMI_backupNow.Click += new System.EventHandler(this.SWTSMI_backupNow_Click);
            // 
            // SWTSMI_separate1
            // 
            this.SWTSMI_separate1.Name = "SWTSMI_separate1";
            this.SWTSMI_separate1.Size = new System.Drawing.Size(149, 6);
            // 
            // SWTSMI_closeProgram
            // 
            this.SWTSMI_closeProgram.Name = "SWTSMI_closeProgram";
            this.SWTSMI_closeProgram.Size = new System.Drawing.Size(152, 22);
            this.SWTSMI_closeProgram.Text = "Close Program";
            this.SWTSMI_closeProgram.Click += new System.EventHandler(this.SWTSMI_closeProgram_Click);
            // 
            // F_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 317);
            this.Controls.Add(this.TC_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "F_Main";
            this.Text = "Backup my mail for MS Outlook - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_Main_FormClosing);
            this.Load += new System.EventHandler(this.F_Main_Load);
            this.Resize += new System.EventHandler(this.F_Main_Resize);
            this.TP_others.ResumeLayout(false);
            this.TP_others.PerformLayout();
            this.TP_backup.ResumeLayout(false);
            this.TP_backup.PerformLayout();
            this.GB_accountPermission.ResumeLayout(false);
            this.GB_accountPermission.PerformLayout();
            this.TC_Main.ResumeLayout(false);
            this.TP_schedule.ResumeLayout(false);
            this.TP_schedule.PerformLayout();
            this.GB_addEditRemoveSchedule.ResumeLayout(false);
            this.GB_addEditRemoveSchedule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_repeatEveryNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_startMinuteSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_startHourSchedule)).EndInit();
            this.TP_restore.ResumeLayout(false);
            this.TP_restore.PerformLayout();
            this.GB_pathsToRestorePst.ResumeLayout(false);
            this.GB_pathToPstInfo.ResumeLayout(false);
            this.GB_pathToPstInfo.PerformLayout();
            this.GB_restorePstS.ResumeLayout(false);
            this.GB_restorePstS.PerformLayout();
            this.CMS_NI_F_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage TP_others;
        private System.Windows.Forms.CheckBox CB_deleteAllPstFileFromBackup;
        private System.Windows.Forms.TabPage TP_backup;
        private System.Windows.Forms.Label L_actualState;
        private System.Windows.Forms.Label L_labelActualState;
        private System.Windows.Forms.Button B_browseBackupOutputFolder;
        private System.Windows.Forms.Label L_infoBackupPath;
        private System.Windows.Forms.TextBox TB_pathToBackupOutputFolder;
        private System.Windows.Forms.Button B_terminateBackupNow;
        private System.Windows.Forms.Button B_startBackupNow;
        private System.Windows.Forms.TabControl TC_Main;
        private System.Windows.Forms.CheckBox CB_afterBackupCloseComputer;
        private System.Windows.Forms.NotifyIcon NI_F_Main;
        private System.Windows.Forms.ContextMenuStrip CMS_NI_F_Main;
        private System.Windows.Forms.ToolStripMenuItem SWTSMI_showWindow;
        private System.Windows.Forms.ToolStripSeparator SWTSMI_separate1;
        private System.Windows.Forms.ToolStripMenuItem SWTSMI_closeProgram;
        private System.Windows.Forms.CheckBox CB_minimalizeWindowOnStartup;
        private System.Windows.Forms.ToolStripMenuItem SWTSMI_backupNow;
        private System.Windows.Forms.TabPage TP_schedule;
        private System.Windows.Forms.LinkLabel LL_homeWebsite;
        private System.Windows.Forms.Label L_actualScheduleSet;
        private System.Windows.Forms.Label L_descriptionScheduleState;
        private System.Windows.Forms.GroupBox GB_addEditRemoveSchedule;
        private System.Windows.Forms.Button B_removeSchedule;
        private System.Windows.Forms.Button B_editSchedule;
        private System.Windows.Forms.ComboBox CB_repeadScheduleKind;
        private System.Windows.Forms.Label L_descriptionStartTimeSchedule;
        private System.Windows.Forms.Label L_descriptionRepeatSchedule;
        private System.Windows.Forms.NumericUpDown NUD_repeatEveryNum;
        private System.Windows.Forms.CheckBox CB_repeatEvery;
        private System.Windows.Forms.Button B_addSchedule;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label L_descriptionStartDateSchedule;
        private System.Windows.Forms.NumericUpDown NUD_startMinuteSchedule;
        private System.Windows.Forms.NumericUpDown NUD_startHourSchedule;
        private System.Windows.Forms.DateTimePicker DTP_startScheduleDate;
        private System.Windows.Forms.Button B_browseLogFile;
        private System.Windows.Forms.Label L_descriptionPathToFile;
        private System.Windows.Forms.TextBox TB_pathToLogFile;
        private System.Windows.Forms.CheckBox CB_copyRegistrySettings;
        private System.Windows.Forms.TabPage TP_restore;
        private System.Windows.Forms.TextBox TB_pathToRegOutlookRestore;
        private System.Windows.Forms.Button B_restoreOutlookRegistry;
        private System.Windows.Forms.Button B_browseRegOutlookRestore;
        private System.Windows.Forms.GroupBox GB_pathsToRestorePst;
        private System.Windows.Forms.GroupBox GB_pathToPstInfo;
        private System.Windows.Forms.Button B_browsePathToPstInfo;
        private System.Windows.Forms.TextBox TB_pathToPstInfo;
        private System.Windows.Forms.Button B_readPstInfo;
        private System.Windows.Forms.GroupBox GB_restorePstS;
        private System.Windows.Forms.Button B_restoreDefaultPST;
        private System.Windows.Forms.Label L_pathToDefaultPst;
        private System.Windows.Forms.Button B_restoreDefaultPstAndArchivePstS;
        private System.Windows.Forms.Label L_copyState;
        private System.Windows.Forms.Button B_terminateCopy;
        private System.Windows.Forms.CheckBox CB_notUseVss;
        private System.Windows.Forms.Label L_copyInfoAdd2;
        private System.Windows.Forms.Label L_copyInfoAdd;
        private System.Windows.Forms.Label L_backupRegistryInfo;
        private System.Windows.Forms.CheckBox CB_autoStartApp;
        private System.Windows.Forms.GroupBox GB_accountPermission;
        private System.Windows.Forms.Label LB_password;
        private System.Windows.Forms.Label L_userName;
        private System.Windows.Forms.TextBox TB_passWord;
        private System.Windows.Forms.TextBox TB_userName;
        private System.Windows.Forms.Button B_saveSettings;


    }
}

