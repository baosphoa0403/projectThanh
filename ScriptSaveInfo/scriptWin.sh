# get hostname
hostname 
# get ip address
ipconfig | findstr IPv4
# anydesk
@echo off
for /f "delims=" %%i in ('"C:\Program Files (x86)\AnyDesk\AnyDesk.exe" --get-id') do set ID=%%i 
echo AnyDesk ID is: %ID%
pause
# CPU
wmic cpu get name 
# Ram
systeminfo | grep "Total Physical Memory"
# Disk

# OS
ver
# ShutDownTime
wevtutil qe system “/q:*[System [(EventID=1074)]]” /rd:true /f:text /c:1
