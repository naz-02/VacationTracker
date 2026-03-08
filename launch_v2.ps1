$objShell = New-Object -ComObject WScript.Shell
$lnk = $objShell.CreateShortcut("C:\Users\Nasr\Desktop\VacationTracker_V2.lnk")
$lnk.TargetPath = "C:\Users\Nasr\Desktop\VacationTracker\bin\Debug\net8.0-windows10.0.19041.0\win10-x64\VacationTracker.exe"
$lnk.WorkingDirectory = "C:\Users\Nasr\Desktop\VacationTracker\bin\Debug\net8.0-windows10.0.19041.0\win10-x64"
$lnk.Save()
Start-Process "C:\Users\Nasr\Desktop\VacationTracker\bin\Debug\net8.0-windows10.0.19041.0\win10-x64\VacationTracker.exe"
