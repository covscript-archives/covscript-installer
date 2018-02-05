@echo off
mkdir build
cd build-cache
copy /Y cspkg\cspkg ..\build\bin\
copy /Y cspkg\cspkg.bat ..\build\bin\
xcopy /E /Y covscript\build ..\build\
xcopy /E /Y covscript-regex\build ..\build\
xcopy /E /Y covscript-darwin\build ..\build\
xcopy /E /Y covscript-sqlite\build ..\build\
xcopy /E /Y covscript-network\build ..\build\