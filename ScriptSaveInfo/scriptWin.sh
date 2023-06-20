# get hostname
hostname 
# get ip address
ipconfig | findstr IPv4
# anydesk
@echo off
for /f "delims=" %%i in ('"C:\Program Files (x86)\AnyDesk\AnyDesk.exe" --get-id') do set ID=%%i 
echo AnyDesk ID is: %ID%
pause
# ultraview
Get-ItemPropertyValue "HKLM:\SOFTWARE\WOW6432Node\UltraViewer" "PreferID"
# CPU
wmic cpu get name 
# Ram
systeminfo | grep "Total Physical Memory"
# Mac
wmic bios get serialnumber
# OS
ver
# ShutDownTime
wevtutil qe system “/q:*[System [(EventID=1074)]]” /rd:true /f:text /c:1
