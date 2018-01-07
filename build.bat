@echo off
mkdir build-cache
cd build-cache
call:git_fetch covscript
call:git_fetch covscript-regex
call:git_fetch covscript-darwin
call:git_fetch covscript-sqlite
call:git_fetch covscript-network
xcopy /E /Y covscript\include covscript-regex\include\
xcopy /E /Y covscript\include covscript-darwin\include\
xcopy /E /Y covscript\include covscript-sqlite\include\
xcopy /E /Y covscript\include covscript-network\include\
start /D covscript make.bat
start /D covscript-regex make.bat
start /D covscript-darwin make.bat
start /D covscript-sqlite make.bat
start /D covscript-network make.bat
goto:eof
:git_fetch
if exist %1% (
    cd %1%
    git fetch
    git pull
    git clean -dfx
    cd ..
) else (
    git clone https://github.com/covscript/%1%
)
goto:eof