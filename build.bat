@echo off
setlocal enabledelayedexpansion

:: If no argument was passed, set the default configuration to Debug
set cfg=%1
if "!cfg!"=="" set cfg=Debug

echo ========================================
echo DeviantArt Scrapper - Build Script
echo ========================================
echo Configuration: !cfg!
echo.

:: Find MSBuild
call :FindMSBuild
if errorlevel 1 (
    echo ERROR: MSBuild not found
    pause
    exit /b 1
)

:: Clean previous builds
echo [1/3] Cleaning previous builds...
if exist "DeviantArtScrapper\bin\!cfg!" (
    rmdir /s /q "DeviantArtScrapper\bin\!cfg!"
)
if exist "DeviantArtScrapper\obj\!cfg!" (
    rmdir /s /q "DeviantArtScrapper\obj\!cfg!"
)

:: Restore NuGet packages
echo [2/3] Restoring NuGet packages...
"!MSBUILD_PATH!" "DeviantArtScrapper.sln" /t:Restore /p:Configuration=!cfg! /p:Platform="Any CPU" /v:m
if errorlevel 1 (
    echo ERROR: NuGet restore failed
    pause
    exit /b 1
)

:: Build the application
echo [3/3] Building application...
"!MSBUILD_PATH!" "DeviantArtScrapper.sln" /p:Configuration=!cfg! /p:Platform="Any CPU" /v:m
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

:: Success message
echo.
echo ========================================
echo BUILD SUCCESSFUL
echo ========================================
echo.
echo Build Summary:
echo - MSBuild: !MSBUILD_PATH!
echo - Configuration: !cfg!
echo - Platform: Any CPU
echo.
echo Application built successfully!
echo Run from: DeviantArtScrapper\bin\!cfg!\net9.0-windows10.0.17763.0\DeviantArtScrapper.exe
echo.
if "!cfg!"=="Debug" (
    echo TIP: For release build, run: build.bat Release
)
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