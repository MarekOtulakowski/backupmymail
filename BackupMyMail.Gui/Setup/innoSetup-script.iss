#define ApplicationName 'BackupMyMail'
#define ApplicationVersion GetFileVersion('C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\bin\Release\BackupMyMail.Gui.exe')
#define ApplicationLibVersion GetFileVersion('C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Lib\bin\Release\BackupMyMail.Lib.dll')

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{A3D4DCAC-1A90-496D-B2F3-2CA59AF02CA1}
AppName={#ApplicationName}
AppVersion={#ApplicationVersion}
AppVerName={#ApplicationName} {#ApplicationVersion}
AppPublisher=MarekOtulakowski
AppPublisherURL=https://github.com/MarekOtulakowski/BackupMyMail
AppSupportURL=https://github.com/MarekOtulakowski/BackupMyMail
AppUpdatesURL=https://github.com/MarekOtulakowski/BackupMyMail
DefaultDirName=C:\Program Files\BackupMyMail
DefaultGroupName={#ApplicationName}
AllowNoIcons=yes
LicenseFile=C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\Setup\License.txt
OutputDir=C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\Setup
OutputBaseFilename=setup-backupmymail-v{#ApplicationVersion}
VersionInfoVersion={#ApplicationVersion}
VersionInfoCopyright=Copyright © MarekOtulakowski 2011-2015
VersionInfoDescription=Installer for {#ApplicationName}
SetupIconFile=C:\Users\Marek\Documents\GitHub\Codeplex\backupMyMail\BackupMyMail.Gui\Setup\SetupIcon.ico
Compression=lzma
SolidCompression=yes
UninstallDisplayIcon={app}\Uninstaller.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\bin\Release\BackupMyMail.Gui.exe"; DestDir: "{app}"; StrongAssemblyName: "BackupMyMail.Gui, Version={#ApplicationVersion}, Culture=neutral, PublicKeyToken=eef6af1b44dc2ad0, ProcessorArchitecture=MSIL"; Flags: ignoreversion
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\bin\Release\BackupMyMail.Lib.dll"; DestDir: "{app}"; StrongAssemblyName: "BackupMyMail.Lib, Version={#ApplicationLibVersion}, Culture=neutral, PublicKeyToken=5639001310f70d19, ProcessorArchitecture=MSIL"; Flags: ignoreversion
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\bin\Release\HoboCopy-x64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\bin\Release\HoboCopy-x86.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\bin\Release\HoboCopy-x86-XP.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\Setup\License.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Marek\Documents\GitHub\Codeplex\BackupMyMail\BackupMyMail.Gui\Setup\Uninstaller.ico"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\BackupMyMail"; Filename: "{app}\BackupMyMail.Gui.exe"
Name: "{group}\{cm:ProgramOnTheWeb,BackupMyMail}"; Filename: "https://github.com/MarekOtulakowski/BackupMyMail"
Name: "{group}\{cm:UninstallProgram,BackupMyMail}"; Filename: "{uninstallexe}"; IconFilename: "{app}\Uninstaller.ico"
Name: "{commondesktop}\BackupMyMail"; Filename: "{app}\BackupMyMail.Gui.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\BackupMyMail"; Filename: "{app}\BackupMyMail.Gui.exe"; Tasks: quicklaunchicon


