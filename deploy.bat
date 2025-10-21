@echo off
setlocal enabledelayedexpansion

REM Configuration variables - modify these as needed
set SOURCE_DIR=.\ExamplePlugin\bin\Debug\netstandard2.1
set TARGET_DIR=C:\Users\tgree\AppData\Roaming\Thunderstore Mod Manager\DataFolder\RiskOfRain2\profiles\test\BepInEx\plugins\ExamplePlugin
set BUILD_CONFIG=Debug
set TARGET_FRAMEWORK=netstandard2.1

REM Core DLLs to copy explicitly
set CORE_DLL_LIST=ExamplePlugin.dll

REM Additional framework DLLs that might be needed for BepInEx compatibility
set FRAMEWORK_DLL_LIST=netstandard.dll System.Runtime.dll System.Core.dll System.dll

echo Deploying mod files...
echo Source directory: %SOURCE_DIR%
echo Target directory: %TARGET_DIR%
echo.

echo Cleaning target directory for fresh deployment...
if exist "%TARGET_DIR%" (
    echo Removing existing files from target directory...
    rmdir /s /q "%TARGET_DIR%" 2>nul
    if !ERRORLEVEL! EQU 0 (
        echo Target directory cleaned successfully.
    ) else (
        echo WARNING: Could not fully clean target directory. Some files may be in use.
    )
) else (
    echo Target directory does not exist yet.
)

echo Creating target directory...
mkdir "%TARGET_DIR%" 2>nul
if !ERRORLEVEL! EQU 0 (
    echo Target directory created successfully.
) else (
    echo Target directory already exists or could not be created.
)
echo.

echo Checking if source directory exists...
if exist "%SOURCE_DIR%" (
    echo Source directory found.
    
    echo Copying core DLL list...
    set COPIED_COUNT=0
    set MISSING_COUNT=0
    
    for %%f in (%CORE_DLL_LIST%) do (
        if exist "%SOURCE_DIR%\%%f" (
            echo Copying %%f...
            copy "%SOURCE_DIR%\%%f" "%TARGET_DIR%\" >nul
            if !ERRORLEVEL! EQU 0 (
                set /a COPIED_COUNT=!COPIED_COUNT!+1
            ) else (
                echo ERROR: Failed to copy %%f
            )
        ) else (
            echo WARNING: %%f not found in source directory
            set /a MISSING_COUNT=!MISSING_COUNT!+1
        )
    )
    
    
    echo.
    echo Summary:
    echo   Files copied: !COPIED_COUNT!
    echo   Files missing: !MISSING_COUNT!
    
    if !MISSING_COUNT! GTR 0 (
        echo.
        echo Missing files may need to be added to the build output.
        echo Check if they are referenced properly in the project files.
    )
) else (
    echo ERROR: Source directory not found at %SOURCE_DIR%
    echo Current directory is: %CD%
    echo.
    echo Make sure you have built the project first with:
    echo   dotnet build --configuration %BUILD_CONFIG%
)