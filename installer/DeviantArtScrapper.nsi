; DeviantArt Scrapper NSIS Installer Script
; Professional Windows installer for DeviantArt Scrapper application
; Requires NSIS 3.0+ with modern UI

!define PRODUCT_NAME "DeviantArt Scrapper"
!define PRODUCT_VERSION "0.1.25.1005"
!define PRODUCT_PUBLISHER "ByteForge"
!define PRODUCT_WEB_SITE "https://github.com/yourusername/DeviantArtScrapper"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\DeviantArtScrapper.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; Modern UI includes
!include "MUI2.nsh"
!include "Sections.nsh"
!include "LogicLib.nsh"
!include "FileFunc.nsh"
!include "WinVer.nsh"

; Installer properties
Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "DeviantArtScrapper-Setup.exe"
InstallDir "$PROGRAMFILES64\${PRODUCT_NAME}"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show
SetCompressor lzma
RequestExecutionLevel admin

; Version information
VIProductVersion "${PRODUCT_VERSION}"
VIAddVersionKey ProductName "${PRODUCT_NAME}"
VIAddVersionKey ProductVersion "${PRODUCT_VERSION}"
VIAddVersionKey CompanyName "${PRODUCT_PUBLISHER}"
VIAddVersionKey FileDescription "${PRODUCT_NAME} Installer"
VIAddVersionKey FileVersion "${PRODUCT_VERSION}"
VIAddVersionKey LegalCopyright "Copyright © 2025 ${PRODUCT_PUBLISHER}"

; Modern UI configuration
!define MUI_ABORTWARNING
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!define MUI_HEADERIMAGE
; !define MUI_HEADERIMAGE_BITMAP "header.bmp"
; !define MUI_WELCOMEFINISHPAGE_BITMAP "welcome.bmp"


; Installer pages
!insertmacro MUI_PAGE_WELCOME
; License page will be inserted with custom license file
!define MUI_PAGE_CUSTOMFUNCTION_PRE PreLicensePage
!insertmacro MUI_PAGE_LICENSE "..\LICENSE"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN "$INSTDIR\DeviantArtScrapper.exe"
!define MUI_FINISHPAGE_RUN_TEXT "$(LaunchApp)"
!define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\USER_MANUAL.md"
!define MUI_FINISHPAGE_SHOWREADME_TEXT "$(ViewManual)"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Languages
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "PortugueseBR"

; Language strings for English
LangString LaunchApp ${LANG_ENGLISH} "Launch DeviantArt Scrapper"
LangString ViewManual ${LANG_ENGLISH} "View User Manual"
LangString ManualShortcut ${LANG_ENGLISH} "User Manual"
LangString DotNetRequired ${LANG_ENGLISH} "Microsoft .NET 9 Runtime is required but not detected.$\n$\nWould you like to download and install it now?"
LangString DotNetInstallMsg ${LANG_ENGLISH} "Please install .NET 9 Runtime and then run this installer again."
LangString AppRunningInstall ${LANG_ENGLISH} "DeviantArt Scrapper is currently running.$\n$\nPlease close the application before installing."
LangString AlreadyInstalled ${LANG_ENGLISH} "DeviantArt Scrapper is already installed.$\n$\nDo you want to uninstall the previous version first?"
LangString AppRunningUninstall ${LANG_ENGLISH} "DeviantArt Scrapper is currently running.$\n$\nPlease close the application before uninstalling."
LangString ConfirmUninstall ${LANG_ENGLISH} "Are you sure you want to completely remove $(^Name) and all of its components?"
LangString UninstallSuccess ${LANG_ENGLISH} "$(^Name) was successfully removed from your computer."
LangString Win10Required ${LANG_ENGLISH} "This application requires Windows 10 version 1809 or later."

; Language strings for Portuguese (Brazil)
LangString LaunchApp ${LANG_PORTUGUESEBR} "Iniciar DeviantArt Scrapper"
LangString ViewManual ${LANG_PORTUGUESEBR} "Ver Manual do Usuário"
LangString ManualShortcut ${LANG_PORTUGUESEBR} "Manual do Usuário"
LangString DotNetRequired ${LANG_PORTUGUESEBR} "O Microsoft .NET 9 Runtime é necessário mas não foi detectado.$\n$\nDeseja baixar e instalar agora?"
LangString DotNetInstallMsg ${LANG_PORTUGUESEBR} "Por favor, instale o .NET 9 Runtime e execute este instalador novamente."
LangString AppRunningInstall ${LANG_PORTUGUESEBR} "O DeviantArt Scrapper está em execução.$\n$\nPor favor, feche o aplicativo antes de instalar."
LangString AlreadyInstalled ${LANG_PORTUGUESEBR} "O DeviantArt Scrapper já está instalado.$\n$\nDeseja desinstalar a versão anterior primeiro?"
LangString AppRunningUninstall ${LANG_PORTUGUESEBR} "O DeviantArt Scrapper está em execução.$\n$\nPor favor, feche o aplicativo antes de desinstalar."
LangString ConfirmUninstall ${LANG_PORTUGUESEBR} "Tem certeza de que deseja remover completamente o $(^Name) e todos os seus componentes?"
LangString UninstallSuccess ${LANG_PORTUGUESEBR} "$(^Name) foi removido com sucesso do seu computador."
LangString Win10Required ${LANG_PORTUGUESEBR} "Este aplicativo requer Windows 10 versão 1809 ou posterior."

; Section descriptions for English
LangString DESC_SecMain ${LANG_ENGLISH} "Core application files and documentation. This component is required."
LangString DESC_SecDesktop ${LANG_ENGLISH} "Creates a desktop shortcut for easy access to the application."
LangString DESC_SecFileAssoc ${LANG_ENGLISH} "Associates DeviantArt Scrapper with project files for double-click opening."
LangString DESC_SecFirewall ${LANG_ENGLISH} "Adds Windows Firewall exception for API connectivity (recommended)."

; Section descriptions for Portuguese (Brazil)
LangString DESC_SecMain ${LANG_PORTUGUESEBR} "Arquivos principais do aplicativo e documentação. Este componente é obrigatório."
LangString DESC_SecDesktop ${LANG_PORTUGUESEBR} "Cria um atalho na área de trabalho para fácil acesso ao aplicativo."
LangString DESC_SecFileAssoc ${LANG_PORTUGUESEBR} "Associa o DeviantArt Scrapper com arquivos de projeto para abertura com clique duplo."
LangString DESC_SecFirewall ${LANG_PORTUGUESEBR} "Adiciona exceção no Firewall do Windows para conectividade com API (recomendado)."

; Variable to store the manual filename
Var ManualFileName

; Function to set the correct license file based on language
Function PreLicensePage
  ; License page handled by standard MUI macros
FunctionEnd

; Installer sections
Section "Main Application" SecMain
  SectionIn RO
  
  ; Check Windows version
  ${IfNot} ${AtLeastWin10}
    MessageBox MB_ICONSTOP "$(Win10Required)"
    Abort
  ${EndIf}
  
  ; Check for .NET 9 (simplified check)
  nsExec::ExecToLog 'dotnet --version'
  Pop $0
  ${If} $0 != 0
    MessageBox MB_YESNO|MB_ICONQUESTION "$(DotNetRequired)" IDNO SkipDotNet
    ExecShell "open" "https://dotnet.microsoft.com/download/dotnet/9.0"
    MessageBox MB_OK "$(DotNetInstallMsg)"
    Abort
    SkipDotNet:
  ${EndIf}
  
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  
  ; Main application files (from publish output)
  File /r "..\DeviantArtScrapper\bin\Release\net9.0-windows10.0.17763.0\publish\*.*"
  
  ; Documentation files
  File "..\README.md"
  File "..\CLAUDE.md"
  
  ; Install appropriate license file based on language
  ${If} $LANGUAGE == ${LANG_PORTUGUESEBR}
    File /oname=LICENSE.txt "..\LICENSE.pt-BR"
  ${Else}
    File /oname=LICENSE.txt "..\LICENSE"
  ${EndIf}
  
  ; Determine which manual to install based on installer language
  ${If} $LANGUAGE == ${LANG_PORTUGUESEBR}
    StrCpy $ManualFileName "MANUAL_USUARIO.md"
    File /oname=MANUAL_USUARIO.md "..\MANUAL_USUARIO.md"
  ${Else}
    StrCpy $ManualFileName "USER_MANUAL.md"
    File /oname=USER_MANUAL.md "..\USER_MANUAL.md"
  ${EndIf}
  
  ; Create application shortcuts
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\DeviantArtScrapper.exe"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\$(ManualShortcut).lnk" "$INSTDIR\$ManualFileName"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\uninst.exe"
  
  ; Registry entries
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\DeviantArtScrapper.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\DeviantArtScrapper.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoModify" 1
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "NoRepair" 1
  
  ; Calculate installed size
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "EstimatedSize" "$0"
  
  ; Write uninstaller
  WriteUninstaller "$INSTDIR\uninst.exe"
SectionEnd

Section "Desktop Shortcut" SecDesktop
  CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\DeviantArtScrapper.exe"
SectionEnd

Section "File Associations" SecFileAssoc
  ; Register file associations for export files
  WriteRegStr HKCR ".deviantart-scrapper" "" "DeviantArtScrapper.Project"
  WriteRegStr HKCR "DeviantArtScrapper.Project" "" "DeviantArt Scrapper Project"
  WriteRegStr HKCR "DeviantArtScrapper.Project\DefaultIcon" "" "$INSTDIR\DeviantArtScrapper.exe,0"
  WriteRegStr HKCR "DeviantArtScrapper.Project\shell\open\command" "" '"$INSTDIR\DeviantArtScrapper.exe" "%1"'
SectionEnd

Section "Windows Firewall Exception" SecFirewall
  ; Add Windows Firewall exception for API access
  nsExec::ExecToLog 'netsh advfirewall firewall add rule name="${PRODUCT_NAME}" dir=out action=allow program="$INSTDIR\DeviantArtScrapper.exe"'
SectionEnd

; Section descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SecMain} "$(DESC_SecMain)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} "$(DESC_SecDesktop)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SecFileAssoc} "$(DESC_SecFileAssoc)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SecFirewall} "$(DESC_SecFirewall)"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

; Installer functions
Function .onInit
  ; Check if application is already running
  System::Call 'kernel32::CreateMutexA(i 0, i 0, t "DeviantArtScrapper_SingleInstance_Mutex") i .r1 ?e'
  Pop $R0
  ${If} $R0 != 0
    MessageBox MB_ICONEXCLAMATION|MB_OK "$(AppRunningInstall)"
    Abort
  ${EndIf}
  
  ; Check for existing installation
  ReadRegStr $R0 ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString"
  StrCmp $R0 "" done
  
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "$(AlreadyInstalled)" IDNO done
  
  ; Run the uninstaller
  ClearErrors
  ExecWait '$R0 _?=$INSTDIR'
  
  IfErrors no_remove_uninstaller done
    ; Remove the uninstaller if it exists
    IfFileExists "$INSTDIR\uninst.exe" 0 no_remove_uninstaller
      Delete "$INSTDIR\uninst.exe"
      RMDir "$INSTDIR"
  
  no_remove_uninstaller:
  done:
FunctionEnd

; Uninstaller section
Section Uninstall
  ; Check if application is running
  System::Call 'kernel32::CreateMutexA(i 0, i 0, t "DeviantArtScrapper_SingleInstance_Mutex") i .r1 ?e'
  Pop $R0
  ${If} $R0 != 0
    MessageBox MB_ICONEXCLAMATION|MB_OK "$(AppRunningUninstall)"
    Abort
  ${EndIf}
  
  ; Remove shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\User Manual.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Manual do Usuário.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk"
  RMDir "$SMPROGRAMS\${PRODUCT_NAME}"
  Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
  
  ; Remove firewall rule
  nsExec::ExecToLog 'netsh advfirewall firewall delete rule name="${PRODUCT_NAME}"'
  
  ; Remove file associations
  DeleteRegKey HKCR ".deviantart-scrapper"
  DeleteRegKey HKCR "DeviantArtScrapper.Project"
  
  ; Remove application files
  RMDir /r "$INSTDIR\runtimes"
  Delete "$INSTDIR\*.dll"
  Delete "$INSTDIR\*.exe"
  Delete "$INSTDIR\*.json"
  Delete "$INSTDIR\*.pdb"
  Delete "$INSTDIR\*.deps.json"
  Delete "$INSTDIR\*.runtimeconfig.json"
  Delete "$INSTDIR\README.md"
  Delete "$INSTDIR\USER_MANUAL.md"
  Delete "$INSTDIR\MANUAL_USUARIO.md"
  Delete "$INSTDIR\LICENSE.txt"
  Delete "$INSTDIR\CLAUDE.md"
  Delete "$INSTDIR\FIRST_RUN_SETUP.txt"
  
  ; Remove installation directory if empty
  RMDir "$INSTDIR"
  
  ; Remove registry entries
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  
  SetAutoClose true
SectionEnd

; Uninstaller functions
Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "$(ConfirmUninstall)" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(UninstallSuccess)"
FunctionEnd

; Custom installer functions
Function .onInstSuccess
  ; Create sample configuration note in the appropriate language
  FileOpen $0 "$INSTDIR\FIRST_RUN_SETUP.txt" w
  
  ${If} $LANGUAGE == ${LANG_PORTUGUESEBR}
    FileWrite $0 "DeviantArt Scrapper - Configuração Inicial$\r$\n"
    FileWrite $0 "========================================$\r$\n$\r$\n"
    FileWrite $0 "Antes de usar o aplicativo, você precisará:$\r$\n$\r$\n"
    FileWrite $0 "1. Obter credenciais da API do DeviantArt:$\r$\n"
    FileWrite $0 "   - Visite: https://www.deviantart.com/developers/apps$\r$\n"
    FileWrite $0 "   - Crie um novo aplicativo com tipo de concessão 'Client Credentials'$\r$\n"
    FileWrite $0 "   - Anote seu Client ID e Client Secret$\r$\n$\r$\n"
    FileWrite $0 "2. Configurar o aplicativo:$\r$\n"
    FileWrite $0 "   - Inicie o DeviantArt Scrapper$\r$\n"
    FileWrite $0 "   - Clique no botão 'Configurações'$\r$\n"
    FileWrite $0 "   - Insira suas credenciais da API$\r$\n"
    FileWrite $0 "   - Teste a conexão e salve$\r$\n$\r$\n"
    FileWrite $0 "3. Começar a extrair dados:$\r$\n"
    FileWrite $0 "   - Digite um nome de usuário do DeviantArt$\r$\n"
    FileWrite $0 "   - Escolha o formato de exportação e nome do arquivo$\r$\n"
    FileWrite $0 "   - Clique em 'Iniciar Extração'$\r$\n$\r$\n"
    FileWrite $0 "Para instruções completas, consulte MANUAL_USUARIO.md$\r$\n$\r$\n"
    FileWrite $0 "Este arquivo pode ser excluído com segurança após a configuração.$\r$\n"
  ${Else}
    FileWrite $0 "DeviantArt Scrapper - First Run Setup$\r$\n"
    FileWrite $0 "================================$\r$\n$\r$\n"
    FileWrite $0 "Before using the application, you'll need to:$\r$\n$\r$\n"
    FileWrite $0 "1. Get DeviantArt API credentials:$\r$\n"
    FileWrite $0 "   - Visit: https://www.deviantart.com/developers/apps$\r$\n"
    FileWrite $0 "   - Create a new application with 'Client Credentials' grant type$\r$\n"
    FileWrite $0 "   - Note your Client ID and Client Secret$\r$\n$\r$\n"
    FileWrite $0 "2. Configure the application:$\r$\n"
    FileWrite $0 "   - Launch DeviantArt Scrapper$\r$\n"
    FileWrite $0 "   - Click 'Settings' button$\r$\n"
    FileWrite $0 "   - Enter your API credentials$\r$\n"
    FileWrite $0 "   - Test connection and save$\r$\n$\r$\n"
    FileWrite $0 "3. Start scraping:$\r$\n"
    FileWrite $0 "   - Enter a DeviantArt username$\r$\n"
    FileWrite $0 "   - Choose export format and filename$\r$\n"
    FileWrite $0 "   - Click 'Start Scraping'$\r$\n$\r$\n"
    FileWrite $0 "For complete instructions, see USER_MANUAL.md$\r$\n$\r$\n"
    FileWrite $0 "This file can be safely deleted after setup.$\r$\n"
  ${EndIf}
  
  FileClose $0
FunctionEnd

; Modern UI customization
!define MUI_CUSTOMFUNCTION_GUIINIT onGUIInit
Function onGUIInit
  ; Add custom initialization if needed
FunctionEnd

; EOF