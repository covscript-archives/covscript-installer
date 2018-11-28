@echo off
rd /S /Q .\build
mkdir build
cd build-cache
xcopy /Y cspkg\cspkg ..\build\bin\
xcopy /Y cspkg\cspkg.bat ..\build\bin\
xcopy /E /Y covscript\build ..\build\
xcopy /E /Y covscript-regex\build ..\build\
xcopy /E /Y covscript-darwin\build ..\build\
xcopy /E /Y covscript-sqlite\build ..\build\
xcopy /E /Y covscript-network\build ..\build\
xcopy /E /Y covscript-streams\build ..\build\
xcopy /E /Y covscript-imgui\build ..\build\