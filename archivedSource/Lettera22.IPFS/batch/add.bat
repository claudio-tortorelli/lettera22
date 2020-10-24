@echo off
SET IPFS_PATH=%2
%~dp0/ipfs.exe add %1 > %~dp0/out/add.txt