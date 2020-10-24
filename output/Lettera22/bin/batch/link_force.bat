@echo off
rem LETTERA22 script - MS Windows - www.claudiotortorelli.it
cd %~dp0
call ./config.bat
cls
set docPath=%~1
if "%docPath%"=="" (
	set /P docPath=input the document file path...
)
if "%docPath%"=="" goto end
for %%i in ("%docPath%") do (
	set _drive=%%~di
	set _path=%%~pi
	set _fname=%%~ni
	set _ext=%%~xi
)

:parse

:compile

:link
%linker% -show -force %_drive%%_path%%_fname%.xml

:end
