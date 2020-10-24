@echo off
rem LETTERA22 script - MS Windows - www.claudiotortorelli.it
cd %~dp0
call ./config.bat
cls
set docName=%~1
if "%docName%"=="" (
	set /P docName=input the document name: 
)
if "%docName%"=="" goto end

copy %template% %docs%\%docName%.txt
"%npp%" %docs%\%docName%.txt
	
:parse

:compile

:link

:end
