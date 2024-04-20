Add-Type -AssemblyName "Microsoft.Office.Interop.Outlook" -ErrorAction Stop
$olFolders = “Microsoft.Office.Interop.Outlook.OlDefaultFolders” -as [type]
$outlook = new-object -comobject outlook.application
$namespace = $outlook.GetNamespace("MAPI")
$calendar = $outlook.Session.GetDefaultFolder(9)
$folder = $namespace.getDefaultFolder($olFolders::olFolderCalendar)  
$startDate = (Get-Date).AddDays(-7)
$folderitems = $folder.Items | Where-Object { 
    $_.Start -ge $startDate -and $_.End -le (Get-Date) 
}
#$folderitems | Select-Object -Property Subject, Start, Duration, Location

$meeting_list = @()

foreach($item in $folderitems | Select-Object -Property Subject, Start, Duration, Location){
    $meeting = [PSCustomObject]@{
        "Subject" = $item.Subject
        "StartTime" = $item.Start
        "EndTime" = if($item.End -ne $null){ $item.End } else { $item.Start.AddDays(1) }
    }
    $meeting_list += $meeting
}

$meeting_list | Export-Csv -Path "$PWD\meetings.csv" -NoTypeInformation
$outlook.quit()

[System.Runtime.InteropServices.Marshal]::ReleaseComObject($calendar) | Out-Null
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($outlook) | Out-Null
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($namespace) | Out-Null
Start-Sleep -Seconds 1
Get-Process -Name outlook -ErrorAction SilentlyContinue | Stop-Process -Force

if(Test-Path "$PWD\task.log" -PathType Leaf){
    $curr_date = (Get-Date)
    "[$curr_date] Task run" | Out-File -FilePath "$PWD\task.log" -Append
} else {
    New-Item -Path "$PWD\task.log" -ItemType File
    "[$curr_date] Log file missing, now created and task run" | Out-File -FilePath "$PWD\task.log" -Append
}