!include "MUI.nsh"

!define VERSION "0.4.2.0"
!define PRODUCT "Serenity"
!define COMPANY "Serenity Project"

!define BINDIR "${PRODUCT}-${VERSION}\bin"
!define SRCDIR "${PRODUCT}-${VERSION}\src"

!define KEYFILE "%USERPROFILE%\Documents\Keys\WShelley.snk"

# Preexecution tasks
!system "rmdir /s /q ${BINDIR}"
!system "rmdir /s /q ${SRCDIR}"
!system "del ${PRODUCT}-${VERSION}-src.7z"
!system "del ${PRODUCT}-${VERSION}-bin.7z"
!system "mkdir ${BINDIR}"
!system "mkdir ${SRCDIR}"
!system "xcopy /S Output\* ${BINDIR}"
!system "copy License.txt ${BINDIR}\License.txt"
!system "copy Serenity.sln ${SRCDIR}\Serenity.sln"
!system "copy License.txt ${SRCDIR}\License.txt"
!system "copy SolutionInfo.cs ${SRCDIR}\SolutionInfo.cs"
!system "copy WShelley.public.snk ${SRCDIR}\WShelley.public.snk"
!system "xcopy /E /I Serenity\* ${SRCDIR}\Serenity"
!system "xcopy /E /I Server\* ${SRCDIR}\Server"
!system "xcopy /E /I system\* ${SRCDIR}\system"
!system "mt -manifest Server\Server.exe.manifest -outputresource:${BINDIR}\Server.exe;#1"
!system "sn -Ra ${BINDIR}\Serenity.dll ${KEYFILE}"
!system "sn -Ra ${BINDIR}\Server.exe ${KEYFILE}"
!system "sn -Ra ${BINDIR}\Modules\system.dll ${KEYFILE}"
!system "7z a -t7z -mx=7 -ms=on ${PRODUCT}-${VERSION}-src.7z -r ${SRCDIR}\* -x!*.user -x!bin -x!obj -x!.svn"
!system "7z a -t7z -mx=7 -ms=on ${PRODUCT}-${VERSION}-bin.7z -r ${BINDIR}\* -x!.svn"

# Installer Attributes
Name "${PRODUCT}"
OutFile "${PRODUCT}-${VERSION}-Installer.exe"
CRCCheck on
SetCompressor /solid LZMA
XPStyle on
RequestExecutionLevel admin

InstallDir "$PROGRAMFILES\${COMPANY}\${PRODUCT}"
InstallDirRegKey HKLM "Software\${COMPANY}\${PRODUCT}" "InstallDir"

# Installer Interface definitions
; !define MUI_ABORTWARNING
; !define OMUI_THEME "Clean"
; !define MUI_ICON "${NSISDIR}\Contrib\MUI Orange Vista Theme\${OMUI_THEME}\installer-nopng.ico"
; !define MUI_UNICON "${NSISDIR}\Contrib\MUI Orange Vista Theme\${OMUI_THEME}\uninstaller-nopng.ico"
; !define MUI_HEADERIMAGE
; !define MUI_HEADERIMAGE_RIGHT
; !define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\MUI Orange Vista Theme\${OMUI_THEME}\header-r.bmp"
; !define MUI_HEADERIMAGE_UNBITMAP "${NSISDIR}\Contrib\MUI Orange Vista Theme\${OMUI_THEME}\header-r-un.bmp"
; !define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\MUI Orange Vista Theme\${OMUI_THEME}\wizard.bmp"
; !define MUI_UNWELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\MUI Orange Vista Theme\${OMUI_THEME}\wizard-un.bmp"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "license.txt"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

Section "Serenity"
	SectionIn RO
	SetShellVarContext all
	
	SetOutPath "$INSTDIR"
	File /r "${BINDIR}\*"
	File "license.txt"
	
	WriteUninstaller "Uninstall.exe"
	CreateDirectory "$SMPROGRAMS\${COMPANY}\${PRODUCT}"
	CreateShortCut "$SMPROGRAMS\${COMPANY}\${PRODUCT}\Uninstall ${PRODUCT}.lnk" "$INSTDIR\Uninstall.exe"
	CreateShortCut "$SMPROGRAMS\${COMPANY}\${PRODUCT}\${PRODUCT}.lnk" "$INSTDIR\Server.exe"
	
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "DisplayName" "${PRODUCT}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "DisplayVersion" "${VERSION}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "InstallLocation" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "Publisher" "${COMPANY}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "UninstallString" "$INSTDIR\Uninstall.exe"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "URLUpdateInfo" "http://serenityproject.net/"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "URLInfoAbout" "http://serenityproject.net/"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}" "NoRepair" 1
	WriteRegStr HKLM "Software\${COMPANY}\${PRODUCT}" "InstallDir" "$INSTDIR"

SectionEnd
Section "un.Serenity"
	SectionIn RO
	SetShellVarContext all
	
	RMDir /r "$SMPROGRAMS\${COMPANY}\${PRODUCT}"
	RMDir "$SMPROGRAMS\${COMPANY}"
	RMDir /r "$INSTDIR\"
	DeleteRegKey HKLM "Software\${COMPANY}\${PRODUCT}"
	DeleteRegKey /ifempty HKLM  "Software\${COMPANY}"
	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT}"
SectionEnd

# Cleanup tasks
!system "rmdir /s /q ${BINDIR}"
!system "rmdir /s /q ${SRCDIR}"
!system "rmdir /s /q ${PRODUCT}-${VERSION}"