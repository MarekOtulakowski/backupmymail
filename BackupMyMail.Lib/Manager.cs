using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security;
using System.Xml; 

namespace BackupMyMail.Lib
{
    public class Manager
    {
        public string ErrorCode { get; set; }
        private readonly string executeFolderPath;
        private string pathToOutputFolder;
        private string defaultPstPath;
        private Process p;
        public bool DeleteOldPst = false;
        public bool CopyRegistrySettings = false;
        public string pathToLog;
        private DateTime startBackupTime;
        private List<string> listBackupPst;
        private List<string> listOrgLocationPst;
        private string OutlookVersion;
        public bool NotUseVSSandHobocopy = false;

        public Manager(string _executeFolderPath)
        {
            executeFolderPath = _executeFolderPath;
            listBackupPst = new List<string>();
        }

        private static int GetMajorVersion(string _path)
        {
            int result = 0;
            if (String.IsNullOrEmpty(_path))
            {
                return result;
            }

            if (!File.Exists(_path))
                return result;

            try
            {
                var _fileVersion = FileVersionInfo.GetVersionInfo(_path);
                result = _fileVersion.FileMajorPart;
            }
            catch
            {
            }
            return result;
        }

        private List<string> GetPstPathFromDefaultOutlookProfile()
        {
            List<string> listToPst = new List<string>();

            try
            {
                //check Outlook version
                RegistryKey regVersionOutlook = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("OUTLOOK.EXE");
                string pathToOutlook = string.Empty;
                pathToOutlook = (string)regVersionOutlook.GetValue("Path");
                pathToOutlook = pathToOutlook + "OUTLOOK.EXE";
                int noVersion = GetMajorVersion(pathToOutlook);

                OutlookVersion = noVersion.ToString();

                if (noVersion < 15) //< Outlook 2013
                {
                    //get paths to archive actually using
                    using (RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").OpenSubKey("Windows Messaging Subsystem").OpenSubKey("Profiles"))
                    {
                        string defaultProfile = (string)reg.GetValue("DefaultProfile");
                        if (string.IsNullOrEmpty(defaultProfile))
                            defaultProfile = "Outlook";

                        using (RegistryKey regInDefaultProfile = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").OpenSubKey("Windows Messaging Subsystem").OpenSubKey("Profiles").OpenSubKey(defaultProfile))
                        {
                            string[] tabAllSubKeyNames = regInDefaultProfile.GetSubKeyNames();
                            for (int i = 0; i < tabAllSubKeyNames.Length; i++)
                            {
                                //add all actual using archive without default pst
                                using (RegistryKey regAdditionalPst = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").OpenSubKey("Windows Messaging Subsystem").OpenSubKey("Profiles").OpenSubKey(defaultProfile).OpenSubKey(tabAllSubKeyNames[i]))
                                {
                                    byte[] bytes = (byte[])regAdditionalPst.GetValue("01020fff");
                                    if (bytes != null)
                                    {
                                        if (noVersion < 13)
                                        {
                                            //for MS Outlook 2003, 2007
                                            string output = Encoding.Unicode.GetString(bytes, 12, bytes.Length - 12);
                                            output = output.Substring(21, output.Length - 22);
                                            byte[] bytes2 = (byte[])regAdditionalPst.GetValue("001f3d09");
                                            string output2;
                                            output2 = Encoding.Unicode.GetString(bytes2);
                                            if (!string.IsNullOrEmpty(output2))
                                                if (output2 == "MSPST MS\0")
                                                {
                                                    if (File.Exists(output))
                                                        listToPst.Add(output);
                                                }
                                                else
                                                    if (output2 == "MSUPST MS\0")
                                                    {
                                                        if (File.Exists(output))
                                                            listToPst.Add(output);
                                                    }
                                                    else
                                                        if (output2 == "INTERSTOR\0")
                                                        {
                                                            output = output.Replace("\0", "");
                                                            if (File.Exists(output))
                                                                listToPst.Add(output);
                                                        }
                                        }
                                        else
                                        {
                                            //for MS Outlook 2010
                                            byte[] bytes2 = (byte[])regAdditionalPst.GetValue("001f3d09");
                                            string output2;
                                            output2 = Encoding.Unicode.GetString(bytes2);
                                            string output4;
                                            if (output2 == "MSPST MS\0")
                                            {
                                                output4 = Encoding.Unicode.GetString(bytes, 12, bytes.Length - 12);
                                                output4 = output4.Substring(21, output4.Length - 22);
                                                if (File.Exists(output4))
                                                    listToPst.Add(output4);
                                            }
                                            else
                                                if (output2 == "MSUPST MS\0")
                                                {
                                                    output4 = Encoding.Unicode.GetString(bytes, 12, bytes.Length - 12);
                                                    output4 = output4.Substring(21, output4.Length - 22);
                                                    if (File.Exists(output4))
                                                        listToPst.Add(output4);
                                                }
                                                else
                                                    if (output2 == "INTERSTOR\0")
                                                    {
                                                        output4 = Encoding.Unicode.GetString(bytes, 12, bytes.Length - 12);
                                                        output4 = output4.Substring(23, output4.Length - 24);
                                                        if (File.Exists(output4))
                                                            listToPst.Add(output4);
                                                    }
                                        }
                                    }
                                }

                                //add main Outlook profile
                                using (RegistryKey regDefaultPst = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows NT").OpenSubKey("CurrentVersion").OpenSubKey("Windows Messaging Subsystem").OpenSubKey("Profiles").OpenSubKey(defaultProfile).OpenSubKey(tabAllSubKeyNames[i]))
                                {
                                    byte[] bytes3 = (byte[])regDefaultPst.GetValue("1102039b");
                                    if (bytes3 != null)
                                    {
                                        string output3 = Encoding.Unicode.GetString(bytes3, 12, bytes3.Length - 12);
                                        output3 = output3.Substring(27, output3.Length - 29);
                                        if (noVersion < 12)
                                            //for MS Outlook 2003
                                            if (File.Exists(output3))
                                                listToPst.Add(output3);
                                            else if (noVersion < 13)
                                            {
                                                //for MS Outlook 2007
                                                if (File.Exists(output3 + "t"))
                                                    listToPst.Add(output3 + "t");
                                            }
                                            else
                                            {
                                                //for MS Outlook 2010
                                                output3 = output3.Substring(4, output3.Length - 4);
                                                if (File.Exists(output3 + "t"))
                                                    listToPst.Add(output3 + "t");
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
                else //for MS Outlook 2013 and highter (not implement highter yet)
                {
                    using (RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Office").OpenSubKey("15.0").OpenSubKey("Outlook"))
                    {
                        string defaultProfile = (string)reg.GetValue("DefaultProfile");
                        if (string.IsNullOrEmpty(defaultProfile))
                            defaultProfile = "Outlook";

                        using (RegistryKey regInDefaultProfile = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Office").OpenSubKey("15.0").OpenSubKey("Outlook").OpenSubKey("Profiles").OpenSubKey(defaultProfile))
                        {
                            string[] tabAllSubKeyNames = regInDefaultProfile.GetSubKeyNames();
                            for (int i = 0; i < tabAllSubKeyNames.Length; i++)
                            {
                                //add all actual using archive with default pst
                                using (RegistryKey regAdditionalPst = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Office").OpenSubKey("15.0").OpenSubKey("Outlook").OpenSubKey("Profiles").OpenSubKey(defaultProfile).OpenSubKey(tabAllSubKeyNames[i]))
                                {
                                    byte[] bytes = (byte[])regAdditionalPst.GetValue("01020fff");
                                    if (bytes != null)
                                    {
                                        if (noVersion == 15)//MS Outlook 2013
                                        {
                                            byte[] bytes2 = (byte[])regAdditionalPst.GetValue("001f3d09");
                                            string output2;
                                            output2 = Encoding.Unicode.GetString(bytes2);
                                            string output4;
                                            if (output2 == "MSPST MS\0" ||
                                                output2 == "MSUPST MS\0")
                                            {
                                                output4 = Encoding.Unicode.GetString(bytes);
                                                output4 = output4.Substring(27, output4.Length - 28);
                                                if (File.Exists(output4))
                                                    listToPst.Add(output4);
                                            }
                                        }                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SaveToLog("Error durring read pst file in Windows Registry" + ex.Message);
            }

            return listToPst;
        }

        public bool RunBackup(string _outputFolderBackup)
        {
            pathToOutputFolder = _outputFolderBackup;
            listBackupPst.Clear();

            bool result = false;

            bool existHobocopy = true;
            var listHobocopy = new List<string>();
            listHobocopy.Add(String.Format("{0}\\HoboCopy-x86.exe", executeFolderPath));
            listHobocopy.Add(String.Format("{0}\\HoboCopy-x64.exe", executeFolderPath));
            listHobocopy.Add(String.Format("{0}\\HoboCopy-x86-XP.exe", executeFolderPath));
            listHobocopy.ForEach(oHobocopy =>
            {
                if (!File.Exists(oHobocopy))
                {
                    existHobocopy = false;
                }
            });
            if (!existHobocopy)
            {
                MessageBox.Show("Hobocopy program doesn't exist in program folder\nTerminate backup",
                "Terminate backup",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return false;
            }

            startBackupTime = DateTime.Now;
            SaveToLog(String.Format("{0} Start backup my mail", startBackupTime));

            if (DeleteOldPst)
            {
                DirectoryInfo di = new DirectoryInfo(_outputFolderBackup);
                FileInfo[] fi = di.GetFiles("*.pst");
                FileInfo[] fi2 = di.GetFiles("*.xml");
                FileInfo[] fi3 = di.GetFiles("*.reg");
                string deletedFile = string.Empty;
                List<string> listToDelete = new List<string>();
                foreach (var oFile in fi)
	            {
                    listToDelete.Add(oFile.FullName);
	            }
                foreach (var oFile2 in fi2)
                {
                    listToDelete.Add(oFile2.FullName);
                }
                foreach (var oFile3 in fi3)
                {
                    listToDelete.Add(oFile3.FullName);
                }

                string fileName = string.Empty;
                while (listToDelete.Count > 0)
                {
                    try
                    {
                        deletedFile = listToDelete[0];
                        File.Delete(listToDelete[0]);
                        listToDelete.RemoveAt(0);
                        SaveToLog(String.Format("{0} Deleted old, files:{1}", DateTime.Now, deletedFile));
                    }
                    catch 
                    {
                        break;
                    }
                } 
            }

            using (var regVersionOutlook = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("OUTLOOK.EXE"))
            {
                string pathToOutlook = string.Empty;
                pathToOutlook = (string)regVersionOutlook.GetValue("Path");
                pathToOutlook = pathToOutlook + "OUTLOOK.EXE";
                int noVersion = GetMajorVersion(pathToOutlook);
                if (noVersion == 0)
                {
                    MessageBox.Show("Microsoft Outlook is not installed or your installation is corrupted", "Missing Microsoft Outlook", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                listOrgLocationPst = GetPstPathFromDefaultOutlookProfile();
                defaultPstPath = listOrgLocationPst[listOrgLocationPst.Count - 1];

                bool res = true;
                bool is64bit = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
                var listPstFromOneFolder = new List<string>();
                var listPstFromDifferentFolder = new List<string>();
                string folderName = string.Empty;
                listOrgLocationPst.ForEach(onePst =>
                {
                    if (String.Compare(listOrgLocationPst[0], onePst, false) == 0)
                    {
                        folderName = onePst.Substring(0, onePst.LastIndexOf("\\"));
                        listPstFromOneFolder.Add(onePst);
                    }
                    else
                    {
                        if (String.Compare(folderName, onePst.Substring(0, onePst.LastIndexOf("\\")), false) == 0)
                        {
                            listPstFromOneFolder.Add(onePst);
                        }
                        else
                        {
                            listPstFromDifferentFolder.Add(onePst);
                        }
                    }
                });
                if (listPstFromDifferentFolder.Count > 0)
                {
                    listOrgLocationPst.ForEach(onePst =>
                    {
                        if (!RunBackupToOnePst(_outputFolderBackup, onePst, is64bit))
                        {
                            res = false;
                        }
                    });
                    if (res)
                    {
                        result = res;
                    }
                }
                else
                {
                    result = !RunBackupToOneFolder(_outputFolderBackup, listPstFromOneFolder, is64bit) ? false : true;
                }
            }

            if (CopyRegistrySettings)
            {
                string fullNameBackupRegistryFile = "backupSettings.reg";
                string uniqueSig = DateTime.Now.ToString();
                uniqueSig = uniqueSig.Replace(":", "-");
                uniqueSig = uniqueSig.Replace(" ", "-");

                uniqueSig = uniqueSig.Replace("/", "-");
                fullNameBackupRegistryFile = "backupSettings" + uniqueSig + ".reg";
                if (SaveRegistryOutlookSettingInFile(_outputFolderBackup + "\\" + fullNameBackupRegistryFile))
                {
                    SaveToLog(String.Format("{0} Saved registry settings in => {1}\\{2}", DateTime.Now, _outputFolderBackup, fullNameBackupRegistryFile));
                }
            }

            SaveInfoAboutPstFiles();

            var stopBackupTime = DateTime.Now;
            SaveToLog(String.Format("{0} Stop Backup my mail{1}All time is = {2}{1}<<<**********>>>{1}", stopBackupTime, Environment.NewLine, (stopBackupTime - startBackupTime)));

            return result;
        }

        private void SaveInfoAboutPstFiles()
        {
            try
            {
                string uniqueSig = DateTime.Now.ToString();
                uniqueSig = uniqueSig.Replace(":", "-");
                uniqueSig = uniqueSig.Replace(" ", "-");
                uniqueSig = uniqueSig.Replace("/", "-");
                string pathToInfoXmlFile = pathToOutputFolder + "\\" + "infoAboutMSOutlook" + uniqueSig + ".xml";
                if (File.Exists(pathToInfoXmlFile))
                {
                    File.Delete(pathToInfoXmlFile);
                }

                using (var textWriter = new XmlTextWriter(pathToInfoXmlFile, null))
                {
                    textWriter.WriteStartDocument();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteComment("Backup My Mail for Microsoft Outlook - info about pst files");

                    //info about this outlook, windows and user
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteStartElement("InfoAboutBackup");
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("BackupTime", DateTime.Now.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("OutlookVersion", OutlookVersion);
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("WindowsVersion", System.Environment.OSVersion.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("UserName", System.Environment.UserDomainName + "\\" + System.Environment.UserName);
                    textWriter.WriteEndElement();

                    //info about pst(s) paths
                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("MainPst", listOrgLocationPst[listOrgLocationPst.Count - 1]);
                    textWriter.WriteEndElement();

                    if (listOrgLocationPst.Count > 1)
                    {
                        for (int i = 0; i < listOrgLocationPst.Count - 1; i++)
                        {
                            textWriter.WriteWhitespace(Environment.NewLine);
                            textWriter.WriteWhitespace("   ");
                            textWriter.WriteStartElement("ArchivePst", listOrgLocationPst[i]);
                            textWriter.WriteEndElement();
                        }
                    }

                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteWhitespace("   ");
                    textWriter.WriteStartElement("BackupMainPst", listBackupPst[listBackupPst.Count - 1]);
                    textWriter.WriteEndElement();

                    if (listBackupPst.Count > 1)
                    {
                        for (int j = 0; j < listBackupPst.Count - 1; j++)
                        {
                            textWriter.WriteWhitespace(Environment.NewLine);
                            textWriter.WriteWhitespace("   ");
                            textWriter.WriteStartElement("BackupArchivePst", listBackupPst[j]);
                            textWriter.WriteEndElement();
                        }
                    }

                    textWriter.WriteWhitespace(Environment.NewLine);
                    textWriter.WriteEndDocument();
                    textWriter.Close();

                    SaveToLog(String.Format("{0} Saved info about pst(s) in => {1}", DateTime.Now, pathToInfoXmlFile));
                }
            }
            catch
            {
            }
        }

        public bool RunBackupToOnePst(string _outputFolderBackup, string _pathToPst, bool is64bit)
        {
            bool result = false;

            if (NotUseVSSandHobocopy)
            {
                try
                {
                    List<string> _listPathsToPst = new List<string>();
                    _listPathsToPst.Add(_pathToPst);
                    RunBackupNotUsingVSS(_outputFolderBackup, _listPathsToPst);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                var psi = new ProcessStartInfo();
                GetHobocopyVersion(is64bit, psi);

                string pstFileName = _pathToPst.Substring(_pathToPst.LastIndexOf("\\") + 1, _pathToPst.Length - _pathToPst.LastIndexOf("\\") - 1);

                string newName = string.Empty;
                Array.ForEach(pstFileName.ToArray(), item =>
                {
                    newName += (int)item == 20 ? (char)8212 : item;
                });
                pstFileName = newName;

                psi.Arguments = String.Format("\"{0}\" \"{1}\" \"{2}\"",
                _pathToPst.Substring(0, _pathToPst.LastIndexOf("\\")),
                _outputFolderBackup,
                pstFileName);

                if (Environment.OSVersion.Version.Major >= 6)
                {
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                }
                else
                {
                    psi.UseShellExecute = true;
                }

                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;

                p = Process.Start(psi);

                p.WaitForExit();

                SaveToLog(String.Format("{0} Backup pst, file using command:{1}{2} {3}", DateTime.Now, Environment.NewLine, psi.FileName, psi.Arguments));

                if (!File.Exists(String.Format("{0}\\{1}", _outputFolderBackup, pstFileName)))
                {
                    ErrorCode = String.Format("RunBackup failed copying file to: {0}\\{1}", _outputFolderBackup, pstFileName);
                    return false;
                }

                string uniqueSig = DateTime.Now.ToString();
                uniqueSig = uniqueSig.Replace(":", "-");
                uniqueSig = uniqueSig.Replace(" ", "-");

                uniqueSig = uniqueSig.Replace("/", "-");

                string pName = pstFileName.Substring(0, pstFileName.LastIndexOf('.'));

                if (_pathToPst == defaultPstPath) pName += "_DEFAULT";
                else
                {                        
                    pName += "_ARCHIVE";
                }
                const string pExtension = ".pst";
                string pstFileName2 = String.Format("{0}-{1}{2}", pName, uniqueSig, pExtension);
                File.Move(String.Format("{0}\\{1}", _outputFolderBackup, pstFileName),
                String.Format("{0}\\{1}", _outputFolderBackup, pstFileName2));   
         
                listBackupPst.Add(_outputFolderBackup + "\\" + pstFileName2);

                SaveToLog(String.Format("{0} Changed PST's file name to => {1}\\{2}", DateTime.Now, _outputFolderBackup, pstFileName2));

                result = true;
            }
            catch (Exception ex)
            {
                ErrorCode = "RunBackup\n" + ex.Message;
            }

            return result;
        }

        private bool SaveRegistryOutlookSettingInFile(string pathToRegFile)
        {
            try
            {
                if (string.IsNullOrEmpty(OutlookVersion))
                    throw new Exception("Outlook version is empty! break copy registy settings");

                int intOutlookVersion = Int32.Parse(OutlookVersion);

                string fullRegPath;
                if (intOutlookVersion < 15)
                {
                    fullRegPath = "\"" + @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\Windows Messaging Subsystem" + "\"";
                }
                else
                {
                    fullRegPath = "\"" + @"HKEY_CURRENT_USER\Software\Microsoft\Office\15.0\Outlook" + "\"";
                }

                string command = @"/E " + pathToRegFile + " " + fullRegPath;
                var psiRegistry = new ProcessStartInfo();
                psiRegistry.FileName = "regedit.exe";
                psiRegistry.Arguments = command;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    psiRegistry.Verb = "runas";
                    psiRegistry.UseShellExecute = true;
                }
                else
                {
                    psiRegistry.UseShellExecute = true;
                }
                psiRegistry.CreateNoWindow = true;
                psiRegistry.WindowStyle = ProcessWindowStyle.Hidden;
                System.Diagnostics.Process pRegistry = Process.Start(psiRegistry);
            }
            catch (Exception ex)
            {
                ErrorCode = "Registry settings didn't copy correclty!\nDetail: " + ex.Message;
                return false;
            }

            return true;
        }

        private void GetHobocopyVersion(bool is64bit, ProcessStartInfo psi)
        {
            psi.FileName = Environment.OSVersion.VersionString.Contains("Microsoft Windows NT 5.1") ? String.Format("{0}\\HoboCopy-x86-XP.exe", executeFolderPath) : is64bit ? String.Format("{0}\\HoboCopy-x64.exe", executeFolderPath) : String.Format("{0}\\HoboCopy-x86.exe", executeFolderPath);
        }

        private void RunBackupNotUsingVSS(string _outputFolderBackup, List<string> _listPathToPst)
        {
            string uniqueSig = DateTime.Now.ToString();
            uniqueSig = uniqueSig.Replace(":", "-");
            uniqueSig = uniqueSig.Replace(" ", "-");
            uniqueSig = uniqueSig.Replace("/", "-");
            string pstFileName = string.Empty;
            string newName = string.Empty;
            
            foreach (var _onePathToPst in _listPathToPst)
            {
                pstFileName = _onePathToPst.Substring(_onePathToPst.LastIndexOf("\\") + 1, _onePathToPst.Length - _onePathToPst.LastIndexOf("\\") - 1);
                
                Array.ForEach(pstFileName.ToArray(), item =>
                {
                    newName += (int)item == 20 ? (char)8212 : item;
                });
                pstFileName = newName;
                pstFileName = pstFileName.Replace(".pst", "");
                string newCopyFileName = string.Empty;

                if (defaultPstPath == _onePathToPst)
                    newCopyFileName = _outputFolderBackup + "\\" + pstFileName + "_DEFAULT" + uniqueSig + ".pst";
                else
                    newCopyFileName = _outputFolderBackup + "\\" + pstFileName + "_ARCHIVE" + uniqueSig + ".pst";

                File.Copy(_onePathToPst, newCopyFileName, true);
                listBackupPst.Add(newCopyFileName);
            }
        }

        public bool RunBackupToOneFolder(string _outputFolderBackup, List<string> _listPathsToPst, bool is64bit)
        {
            bool result = false;

            if (NotUseVSSandHobocopy)
            {
                try
                {
                    RunBackupNotUsingVSS(_outputFolderBackup, _listPathsToPst);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                var psi = new ProcessStartInfo();
                GetHobocopyVersion(is64bit, psi);
                var listPstFiles = new List<string>();
                _listPathsToPst.ForEach(oPst => listPstFiles.Add(oPst.Substring(oPst.LastIndexOf("\\") + 1, oPst.Length - oPst.LastIndexOf("\\") - 1)));
                string newName = string.Empty;
                string argumentPst = string.Empty;
                for (int i = 0; i < listPstFiles.Count; i++)
                {
                    Array.ForEach(listPstFiles[i].ToArray(), item =>
                    {
                        newName += (int)item == 20 ? (char)8212 : item;
                    });
                    listPstFiles[i] = newName;
                    if (String.Compare(argumentPst, string.Empty, false) == 0)
                    {
                        argumentPst += string.Format("\"{0}\"", newName);
                    }
                    else
                    {
                        argumentPst += " ";
                        argumentPst += String.Format("\"{0}\"", newName);
                    }
                    newName = string.Empty;
                }
                psi.Arguments = String.Format("\"{0}\" \"{1}\" {2}", _listPathsToPst[0].Substring(0, _listPathsToPst[0].LastIndexOf("\\")), _outputFolderBackup, argumentPst);
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                }
                else
                {
                    psi.UseShellExecute = true;
                }
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                p = Process.Start(psi);
                p.WaitForExit();
                StreamWriter log;
                log = !File.Exists(pathToLog) ? new StreamWriter(pathToLog) : File.AppendText(pathToLog);
                try
                {
                    log.WriteLine(String.Format("{0} Backup pst, file using command:{1}{2} {3}", DateTime.Now, Environment.NewLine, psi.FileName, psi.Arguments));
                }
                catch
                {
                }
                log.Close();

                bool res = true;
                listPstFiles.ForEach(oPst =>
                {
                    if (!File.Exists(String.Format("{0}\\{1}", _outputFolderBackup, oPst)))
                    {
                        res = false;
                    }
                });
                if (!res)
                {
                    ErrorCode = "RunBackup not coped correct all files";
                    return false;
                }

                int cElement = 0;
                listPstFiles.ForEach(oPst =>
                {
                    cElement++;
                    string uniqueSig = DateTime.Now.ToString();
                    uniqueSig = uniqueSig.Replace(":", "-");
                    uniqueSig = uniqueSig.Replace(" ", "-");
                    uniqueSig = uniqueSig.Replace("/", "-");
                    string pName = oPst.Substring(0, oPst.LastIndexOf('.'));
                    if (listPstFiles.Count == cElement) pName += "_DEFAULT";
                    else
                    {
                        pName += "_ARCHIVE";
                    }
                    const string pExtension = ".pst";
                    string pstFileName2 = String.Format("{0}-{1}{2}", pName, uniqueSig, pExtension);

                    listBackupPst.Add(_outputFolderBackup + "\\" + pstFileName2);

                    File.Move(String.Format("{0}\\{1}", _outputFolderBackup, oPst), String.Format("{0}\\{1}", _outputFolderBackup, pstFileName2));
                    SaveToLog(String.Format("{0} Changed PST's file name to => {1}\\{2}", DateTime.Now, _outputFolderBackup, pstFileName2));
                });

                result = true;
            }
            catch (Exception ex)
            {
                ErrorCode = "RunBackup\n" + ex.Message;
            }

            return result;
        }

        public bool TerminateBackup()
        {
            bool result = false;

            try
            {
                p.Refresh();
                p.Kill();

                if (p.HasExited)
                {
                    result = true;
                }
            }
            catch
            {
            }

            return result;
        }

        private void SaveToLog(string message)
        {
            var log = !File.Exists(pathToLog) ? new StreamWriter(pathToLog) : File.AppendText(pathToLog);

            try
            {
                log.WriteLine(message);
            }
            catch
            {
            }

            log.Close();
        }
    }
}
