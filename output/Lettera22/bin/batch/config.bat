@echo off
rem LETTERA22 command line script - MS Windows - www.claudiotortorelli.it
set baseFolder=..\..\docRoot
set bin=..\lettera22
set docs=%baseFolder%\textWorks
set prePub=%baseFolder%\pre_publish
set published=%baseFolder%\published

set template=%baseFolder%\_docTemplate_.txt
set npp=..\npp\notepad++.exe
set optionsFile=%bin%\options.txt

set parser="%bin%\Lettera22.Parser.exe"
set compiler="%bin%\Lettera22.Compiler.exe"
set linker="%bin%\Lettera22.Linker.exe"

