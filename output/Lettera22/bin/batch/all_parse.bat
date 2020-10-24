@echo off
rem LETTERA22 script - MS Windows - www.claudiotortorelli.it
cd %~dp0
call ./config.bat
cls
	
:parse
for /d %%D in (%docs%) do %parser% -force %%~fD

:compile

:link

:end
