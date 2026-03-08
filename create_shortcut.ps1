$objShell = New-Object -ComObject WScript.Shell
$desktopPath = [System.Environment]::GetFolderPath("Desktop")
$shortcutLink = Join-Path $desktopPath "Vacation Tracker - Berkane.lnk"
$targetPath = "C:\Users\Nasr\Desktop\VacationTracker\bin\Debug\net8.0-windows10.0.19041.0\win10-x64\VacationTracker.exe"
$workingDir = "C:\Users\Nasr\Desktop\VacationTracker\bin\Debug\net8.0-windows10.0.19041.0\win10-x64"

$lnk = $objShell.CreateShortcut($shortcutLink)
$lnk.TargetPath = $targetPath
$lnk.WorkingDirectory = $workingDir
$lnk.Description = "Gestion des Congés - Province de Berkane"
$lnk.IconLocation = "C:\Users\Nasr\Desktop\VacationTracker\Platforms\Windows\appicon.ico"
$lnk.Save()

Write-Host "Shortcut created successfully on the desktop."
