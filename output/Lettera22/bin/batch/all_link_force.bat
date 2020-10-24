@echo off
rem LETTERA22 script - MS Windows - www.claudiotortorelli.it
cd %~dp0
call ./config.bat
cls

:parse

:compile

:link
%linker% -show -force

:end
