@echo off
SET IPFS_PATH=%2
%~dp0/ipfs.exe pin add %1  > %~dp0/out/pin_add.txt