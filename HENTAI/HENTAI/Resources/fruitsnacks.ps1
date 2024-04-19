$outlook = New-Object -ComObject Outlook.Application

$calendar = $outlook.Session.GetDefaultFolder(9)

$meeting_list = @()

foreach(item in $calendar.Items){
     if(item -is [Microsoft.Office.Interop.Outlook.AppointmentItem]){
          $meeting = New-Object PSObject -Property @{
               "Subject" = $item.Subject
               "StartTime" = $item.Start
               "EndTime" = $item.End
          }
          $meeting_list += $meeting
     }
}

$meeting_list | Export-Csv -Path $PWD.Path + "meetings.csv" -NoTypeInformation
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($calendar) | Out-Null
[System.Runtime.InteropServices.Marshal]::ReleaseComObject($outlook) | Out-Null