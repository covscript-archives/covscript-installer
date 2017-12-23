@echo off
mkdir build-cache
cd build-cache
set git_repo=https://github.com/covscript
git clone %git_repo%/covscript
git clone %git_repo%/covscript-regex
git clone %git_repo%/covscript-darwin
git clone %git_repo%/covscript-sqlite
git clone %git_repo%/covscript-network
xcopy /E /Y covscript\include covscript-regex\include\
xcopy /E /Y covscript\include covscript-darwin\include\
xcopy /E /Y covscript\include covscript-sqlite\include\
xcopy /E /Y covscript\include covscript-network\include\
start /D covscript make.bat
start /D covscript-regex make.bat
start /D covscript-darwin make.bat
start /D covscript-sqlite make.bat
start /D covscript-network make.bat