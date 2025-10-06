# DeviantArt Scrapper Installer

This directory contains the NSIS installer script and related files for creating a professional Windows installer for DeviantArt Scrapper.

## Requirements

### NSIS (Nullsoft Scriptable Install System)
- **Download**: https://nsis.sourceforge.io/Download
- **Version**: NSIS 3.0 or later required
- **Installation**: Install with default options and ensure `makensis` is in your PATH

### Optional Assets
The installer script references some optional graphical assets that enhance the installation experience:

- `header.bmp` (150x57 pixels) - Header image for installer pages
- `welcome.bmp` (164x314 pixels) - Welcome page sidebar image
- `app.ico` - Application icon for installer

If these files are not present, NSIS will use default graphics.

## Building the Installer

### Method 1: Automated Build Script
```batch
# Run the automated build script (recommended)
build-installer.bat
```

This script will:
1. Clean previous builds
2. Build the application in Release mode
3. Publish the application with required runtime
4. Create the installer using NSIS

### Method 2: Manual Process
```batch
# 1. Find MSBuild (will search Visual Studio installations)
# The script automatically detects MSBuild from VS 2017/2019/2022

# 2. Restore packages and build
msbuild DeviantArtScrapper.sln /t:Restore /p:Configuration=Release
msbuild DeviantArtScrapper.sln /p:Configuration=Release /p:Platform="Any CPU"

# 3. Publish the application
msbuild DeviantArtScrapper\DeviantArtScrapper.csproj /t:Publish /p:Configuration=Release /p:RuntimeIdentifier=win-x64 /p:SelfContained=false

# 4. Create installer
cd installer
makensis DeviantArtScrapper.nsi
```

## Installer Features

### Professional Installation Experience
- **Modern UI**: Clean, professional installer interface
- **Component Selection**: Optional components (desktop shortcut, file associations, etc.)
- **Prerequisites Check**: Verifies Windows version and .NET runtime
- **Smart Upgrades**: Detects and handles existing installations

### Installation Components
- **Main Application** (Required): Core application files and documentation
- **Desktop Shortcut** (Optional): Creates desktop shortcut for easy access
- **File Associations** (Optional): Associates project files with the application
- **Firewall Exception** (Optional): Adds Windows Firewall rule for API access

### Registry Integration
- **Add/Remove Programs**: Proper uninstaller registration
- **Application Paths**: Enables running from command line
- **File Associations**: Double-click support for project files
- **Start Menu**: Application and documentation shortcuts

### Security Features
- **Administrator Rights**: Requests elevation for proper installation
- **Running Application Check**: Prevents installation while app is running
- **Clean Uninstall**: Removes all traces during uninstallation
- **Single Instance**: Respects application's single-instance design

## Customization

### Branding
Edit these variables in `DeviantArtScrapper.nsi`:
```nsis
!define PRODUCT_NAME "DeviantArt Scrapper"
!define PRODUCT_VERSION "0.1.25.1005"
!define PRODUCT_PUBLISHER "ByteForge"
!define PRODUCT_WEB_SITE "https://github.com/yourusername/DeviantArtScrapper"
```

### Graphics
Add these files to the installer directory:
- `header.bmp` - Top header image (150x57 pixels)
- `welcome.bmp` - Welcome page sidebar (164x314 pixels)
- `app.ico` - Application icon

### Additional Components
To add new optional components, add sections like:
```nsis
Section "New Component" SecNewComp
  ; Component installation code here
SectionEnd
```

## Output

The installer script creates:
- **Filename**: `DeviantArtScrapper-Setup.exe`
- **Size**: Approximately 15-25 MB (depending on dependencies)
- **Target**: Windows 10 version 1809 or later
- **Architecture**: x64 only

## Testing

### Test Environment
- Clean Windows 10/11 virtual machine
- No .NET runtime pre-installed
- Standard user account (installer will request elevation)

### Test Scenarios
1. **Fresh Installation**: Install on system without .NET 9
2. **Upgrade Installation**: Install over existing version
3. **Custom Installation**: Test component selection
4. **Uninstallation**: Verify complete removal
5. **Multiple Users**: Test per-machine vs per-user installation

### Verification Points
- [ ] Application launches correctly after installation
- [ ] Start Menu shortcuts work
- [ ] Desktop shortcut works (if selected)
- [ ] Uninstaller removes all files and registry entries
- [ ] No UAC prompts during normal application use
- [ ] API connectivity works (firewall exception effective)

## Troubleshooting

### Common Build Issues

**"makensis is not recognized"**
- Install NSIS and ensure it's in your PATH
- Restart command prompt after NSIS installation

**"File not found" errors**
- Ensure application is built and published first
- Check that publish output directory exists
- Verify all referenced files exist

**"Access denied" during installation**
- Run installer as administrator
- Close any running instances of the application
- Check antivirus software isn't blocking installation

### NSIS Script Errors

**"Invalid command" errors**
- Check NSIS version (requires 3.0+)
- Verify script syntax with NSIS documentation
- Look for missing includes or plugins

**"File already exists" warnings**
- These are usually safe to ignore
- Indicates files being overwritten during upgrade

## Distribution

### Release Package Contents
```
DeviantArtScrapper-v0.1.25.1005/
├── DeviantArtScrapper-Setup.exe    # Main installer
├── README.md                       # Project readme
├── USER_MANUAL.md                  # Complete user guide
├── LICENSE                         # Software license
└── CHANGELOG.md                    # Version history
```

### Distribution Checklist
- [ ] Test installer on clean system
- [ ] Verify application functionality post-installation
- [ ] Check all documentation is included
- [ ] Validate uninstaller works completely
- [ ] Test on multiple Windows versions
- [ ] Scan for malware (some antivirus flag self-extracting executables)

---

*For technical development information, see CLAUDE.md in the project root.*