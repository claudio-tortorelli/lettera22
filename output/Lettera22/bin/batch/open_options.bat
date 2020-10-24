@echo off
rem LETTERA22 script - MS Windows - www.claudiotortorelli.it
cd %~dp0
call ./config.bat
cls

%npp% %optionsFile%

:parse

:compile

:link

:end
