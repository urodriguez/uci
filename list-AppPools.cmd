@echo off

net session >nul 2>&1
if %errorLevel% == 0 (
    echo Success: Administrator mode confirmed.
) else (
    echo Failure: This command must be run as Administrator mode.
    goto :end
)

:start
cd C:\Windows\System32\inetsrv
echo ----------------------------
appcmd.exe list wp
echo ----------------------------
echo.
set /p "refresg=Press 'r' to refresh app pool list ... "
if /i "%refresg%"=="r" echo.
if /i "%refresg%"=="r" goto :start

:end
pause