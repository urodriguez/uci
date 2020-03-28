@echo off

net session >nul 2>&1
if %errorLevel% == 0 (
    echo Success: Administrator mode confirmed.
) else (
    echo Failure: This command must be run as Administrator mode.
    goto :end
)

@echo on
start chrome http://localhost:8082
java -jar "C:\Program Files (x86)\Jenkins\jenkins.war" --httpPort=8082

:end
pause