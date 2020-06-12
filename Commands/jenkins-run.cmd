@echo off

net session >nul 2>&1
if %errorLevel% == 0 (
    echo Success: Administrator mode confirmed.
) else (
    echo Failure: This command must be run as Administrator mode.
    goto :end
)

REM "Open a delayed chrome windows after 8 seconds"
start "" /b cmd /c "timeout /nobreak 8 >nul & start "" chrome http://localhost:8082"

REM "Start Jenkins process"
java -jar "C:\Program Files (x86)\Jenkins\jenkins.war" --httpPort=8082

:end
pause