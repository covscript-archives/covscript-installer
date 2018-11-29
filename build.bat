@echo off
mkdir build-cache
cd build-cache
call:git_fetch cspkg
call:git_fetch covscript
call:git_fetch covscript-regex
call:git_fetch covscript-darwin
call:git_fetch covscript-sqlite
call:git_fetch covscript-network
call:git_fetch covscript-streams
call:git_fetch covscript-imgui
start /WAIT /D covscript make.bat
start /WAIT /D covscript install.bat
set CS_DEV_PATH=%cd%\covscript\csdev
start /D covscript-regex make.bat
start /D covscript-darwin make.bat
start /D covscript-sqlite make.bat
start /D covscript-network make.bat
start /D covscript-streams make.bat
start /D covscript-imgui make.bat
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