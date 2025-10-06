@echo off
setlocal enabledelayedexpansion

echo ========================================
echo DeviantArt Scrapper - Build Installer
echo ========================================
echo.

:: Check if NSIS is installed
where makensis >nul 2>&1
if errorlevel 1 (
    echo ERROR: NSIS (Nullsoft Scriptable Install System) not found in PATH
    echo Please install NSIS from: https://nsis.sourceforge.io/Download
    echo.
    pause
    exit /b 1
)

:: Find MSBuild
call :FindMSBuild
if errorlevel 1 (
    echo ERROR: MSBuild not found
    pause
    exit /b 1
)

:: Clean previous builds
echo [1/5] Cleaning previous builds...
if exist "DeviantArtScrapper\bin\Release" (
    rmdir /s /q "DeviantArtScrapper\bin\Release"
)
if exist "DeviantArtScrapper\obj\Release" (
    rmdir /s /q "DeviantArtScrapper\obj\Release"
)
if exist "installer\DeviantArtScrapper-Setup.exe" (
    del "installer\DeviantArtScrapper-Setup.exe"
)

:: Restore NuGet packages
echo [2/5] Restoring NuGet packages...
"!MSBUILD_PATH!" "DeviantArtScrapper.sln" /t:Restore /p:Configuration=Release /p:Platform="Any CPU" /v:m
if errorlevel 1 (
    echo ERROR: NuGet restore failed
    pause
    exit /b 1
)

:: Build the application in Release mode
echo [3/5] Building application in Release mode...
"!MSBUILD_PATH!" "DeviantArtScrapper.sln" /p:Configuration=Release /p:Platform="Any CPU" /v:m
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

:: Publish the application
echo [4/5] Publishing application...
"!MSBUILD_PATH!" "DeviantArtScrapper\DeviantArtScrapper.csproj" /t:Publish /p:Configuration=Release /p:Platform="Any CPU" /p:RuntimeIdentifier=win-x64 /p:SelfContained=false /p:PublishDir="bin\Release\net9.0-windows10.0.17763.0\publish\" /v:m
if errorlevel 1 (
    echo ERROR: Publish failed
    pause
    exit /b 1
)

:: Create installer with NSIS
echo [5/5] Creating installer with NSIS...
cd installer
makensis DeviantArtScrapper.nsi
if errorlevel 1 (
    echo ERROR: NSIS compilation failed
    cd ..
    pause
    exit /b 1
)
cd ..

:: Success message
echo.
echo ========================================
echo BUILD SUCCESSFUL
echo ========================================
echo.
echo Installer created: installer\DeviantArtScrapper-Setup.exe
echo.
echo Build Summary:
echo - MSBuild: !MSBUILD_PATH!
echo - Configuration: Release
echo - Platform: Any CPU
echo - Runtime: win-x64
echo - Self-contained: false
echo.
echo Next steps:
echo 1. Test the installer on a clean system
echo 2. Create release notes
echo 3. Upload to release repository
echo.
pause
exit /b 0

:FindMSBuild
echo Searching for MSBuild...

:: Try to find Visual Studio 2022 installation
set VS2022_PATH=
for %%e in (Enterprise Professional Community) do (
    if exist "C:\Program Files\Microsoft Visual Studio\2022\%%e\MSBuild\Current\Bin\MSBuild.exe" (
        set VS2022_PATH=C:\Program Files\Microsoft Visual Studio\2022\%%e\MSBuild\Current\Bin\MSBuild.exe
        goto :found_msbuild
    )
)

:: Try to find Visual Studio 2019 installation
set VS2019_PATH=
for %%e in (Enterprise Professional Community) do (
    if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\%%e\MSBuild\Current\Bin\MSBuild.exe" (
        set VS2019_PATH=C:\Program Files (x86)\Microsoft Visual Studio\2019\%%e\MSBuild\Current\Bin\MSBuild.exe
        goto :found_msbuild
    )
)

:: Try to find Visual Studio 2017 installation
set VS2017_PATH=
for %%e in (Enterprise Professional Community) do (
    if exist "C:\Program Files (x86)\Microsoft Visual Studio\2017\%%e\MSBuild\15.0\Bin\MSBuild.exe" (
        set VS2017_PATH=C:\Program Files (x86)\Microsoft Visual Studio\2017\%%e\MSBuild\15.0\Bin\MSBuild.exe
        goto :found_msbuild
    )
)

:: Try to find older MSBuild from .NET Framework
set NETFX_MSBUILD=
for %%v in (4.0.30319 14.0 12.0) do (
    if exist "C:\Windows\Microsoft.NET\Framework\v%%v\MSBuild.exe" (
        set NETFX_MSBUILD=C:\Windows\Microsoft.NET\Framework\v%%v\MSBuild.exe
        goto :found_msbuild
    )
)

:: Try to find MSBuild in the path
where msbuild >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set MSBUILD_PATH=msbuild
    goto :found_msbuild
)

echo ERROR: MSBuild not found. Please install Visual Studio or .NET Framework SDK.
exit /b 1

:found_msbuild
if defined VS2022_PATH (
    echo Found MSBuild from Visual Studio 2022
    set MSBUILD_PATH=!VS2022_PATH!
) else if defined VS2019_PATH (
    echo Found MSBuild from Visual Studio 2019
    set MSBUILD_PATH=!VS2019_PATH!
) else if defined VS2017_PATH (
    echo Found MSBuild from Visual Studio 2017
    set MSBUILD_PATH=!VS2017_PATH!
) else if defined NETFX_MSBUILD (
    echo Found MSBuild from .NET Framework
    set MSBUILD_PATH=!NETFX_MSBUILD!
) else (
    echo Found MSBuild in PATH
)

echo Using MSBuild: !MSBUILD_PATH!
exit /b 0