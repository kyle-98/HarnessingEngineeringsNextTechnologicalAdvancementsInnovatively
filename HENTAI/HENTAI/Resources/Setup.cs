using System;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using HENTAI;
using System.IO;
using System.Data;

namespace HENTAI.Resources
{
     public static class Setup
     {
          public static void ScheduleTask(MainWindow MainWindow)
          {
               using (TaskService this_service = new())
               {
                    TaskDefinition outlook_task = this_service.NewTask();
                    outlook_task.RegistrationInfo.Description = "Harnessing Engineering's Next Technological Advancements Innovatively";
                    outlook_task.Principal.UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                    WeeklyTrigger weeklyTrigger = new WeeklyTrigger { StartBoundary = DateTime.Today.AddDays(1).AddHours(3) };

                    string action_path = @$"{Environment.CurrentDirectory}\Resources\Resources\fruitsnacks.ps1";
                    outlook_task.Actions.Add(action_path);
                    try
                    {
                         this_service.RootFolder.RegisterTaskDefinition(@"HarnessingEngineeringsNextTechnologicalAdvancementsInnovatively", outlook_task);
                         MainWindow.AddDebugOutputLine("Successfully added task");
                    }
                    catch (Exception ex)
                    {
                         MainWindow.AddDebugOutputLine("ERROR >>> Failed to add task");
                         MainWindow.AddDebugOutputLine($"ERROR >>> {ex.Message}");
                    }
               }
          }

          public static void RemoveTask(MainWindow MainWindow)
          {
               string task_name = "HarnessingEngineeringsNextTechnologicalAdvancementsInnovatively";
               using (TaskService this_service = new())
               {
                    Task task = this_service.GetTask(task_name);
                    if (task != null)
                    {
                         this_service.RootFolder.DeleteTask(task_name);
                         MainWindow.AddDebugOutputLine("Successfully deleted task");
                    }
                    else
                    {
                         MainWindow.AddDebugOutputLine("WARNING >>> Failed to delete task, no task by that name was found");
                    }
               }
          }

          public static void CreateLogFile(MainWindow MainWindow)
          {
               if (File.Exists(@$"{Environment.CurrentDirectory}\Resources\task.log"))
               {
                    MainWindow.AddDebugOutputLine("WARNING >>> Task log file already exists");
               }
               else
               {
                    File.Create($@"{Environment.CurrentDirectory}\Resources\task.log").Dispose();
                    MainWindow.AddDebugOutputLine("Task log file created");
               }
          }

          public static void DeleteLogFile(MainWindow MainWindow)
          {
               if (File.Exists(@$"{Environment.CurrentDirectory}\Resources\task.log"))
               {
                    File.Delete($@"{Environment.CurrentDirectory}\Resources\task.log");
                    MainWindow.AddDebugOutputLine("Task log file deleted");
               }
               else { MainWindow.AddDebugOutputLine("WARNING >>> Task log file does not exist"); }
          }

          public static void CreatePowershellScript(MainWindow MainWindow)
          {
               if (File.Exists($@"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1"))
               {
                    MainWindow.AddDebugOutputLine("WARNING >>> Powershell script already exists");
               }
               else
               {
                    try
                    {
                         string content = @"
                              Add-Type -AssemblyName ""Microsoft.Office.Interop.Outlook"" -ErrorAction Stop
                              $olFolders = ""Microsoft.Office.Interop.Outlook.OlDefaultFolders"" -as [type]
                              $outlook = new-object -comobject outlook.application
                              $namespace = $outlook.GetNamespace(""MAPI"")
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
                                        ""Subject"" = $item.Subject
                                        ""StartTime"" = $item.Start
                                        ""EndTime"" = if($item.End -ne $null){ $item.End } else { $item.Start.AddDays(1) }
                                   }
                                   $meeting_list += $meeting
                              }

                              $meeting_list | Export-Csv -Path ""$PWD\Resources\meetings.csv"" -NoTypeInformation
                              $outlook.quit()

                              [System.Runtime.InteropServices.Marshal]::ReleaseComObject($calendar) | Out-Null
                              [System.Runtime.InteropServices.Marshal]::ReleaseComObject($outlook) | Out-Null
                              [System.Runtime.InteropServices.Marshal]::ReleaseComObject($namespace) | Out-Null
                              Start-Sleep -Seconds 1
                              Get-Process -Name outlook -ErrorAction SilentlyContinue | Stop-Process -Force

                              if(Test-Path ""$PWD\task.log"" -PathType Leaf){
                                   $curr_date = (Get-Date)
                                   ""[$curr_date] Task run"" | Out-File -FilePath ""$PWD\task.log"" -Append
                              } else {
                                   New-Item -Path ""$PWD\task.log"" -ItemType File
                                   ""[$curr_date] Log file missing, now created and task run"" | Out-File -FilePath ""$PWD\task.log"" -Append
                              }
                         ";
                         File.Create($@"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1").Dispose();
                         MainWindow.AddDebugOutputLine("Powershell script created");
                         File.WriteAllText($@"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1", content);
                         MainWindow.AddDebugOutputLine("Powershell script populated");

                    }
                    catch (Exception ex)
                    {
                         MainWindow.AddDebugOutputLine("ERROR >>> Powershell script creation encountered an error");
                         MainWindow.AddDebugOutputLine($"ERROR >>> {ex.Message}");
                    }
               }
          }

          public static void DeletePowershellScript(MainWindow MainWindow)
          {
               if (File.Exists($@"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1"))
               {
                    File.Delete($@"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1");
                    MainWindow.AddDebugOutputLine("Powershell script deleted");
               }
               else { MainWindow.AddDebugOutputLine("WARNING >>> Powershell script doesn't exist"); }
          }

          public static bool CheckForResources(MainWindow? MainWindow = null)
          {
               if (Directory.Exists($@"{Environment.CurrentDirectory}\Resources"))
               {
                    if(MainWindow != null) { MainWindow.AddDebugOutputLine("Resources directory exists"); }
                    return true;
               }
               else
               {
                    if(MainWindow != null) { MainWindow.AddDebugOutputLine("Resources directory doesn't exist... creating"); }
                    return false;
               }
          }

          public static void CreateResources(MainWindow MainWindow)
          {
               Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Resources");
               MainWindow.AddDebugOutputLine("Resources directory created");
          }

          public static bool CheckInstall(MainWindow MainWindow)
          {
               string curr_path = $@"{Environment.CurrentDirectory}\Resources";
               bool is_installed = false;
               MainWindow.AddDebugOutputLine("Validating installation status...");
               if (CheckForResources()) 
               { 
                    MainWindow.AddDebugOutputLine("Resources directory exists");
                    is_installed = true;

                    //powershell script
                    if (File.Exists($@"{curr_path}\fruitsnacks.ps1")) 
                    { 
                         MainWindow.AddDebugOutputLine("Powershell script exists");
                         is_installed = true;
                    }
                    else
                    {
                         MainWindow.AddDebugOutputLine("WARNING >>> Powershell script doesn't exist");
                         is_installed = false;
                    }

                    //config file
                    if (File.Exists($@"{curr_path}\app_config.json"))
                    {
                         MainWindow.AddDebugOutputLine("Config file exists");
                         is_installed = true;
                    }
                    else
                    {
                         MainWindow.AddDebugOutputLine("WARNING >>> Config file doesn't exist");
                         is_installed = false;
                    }

                    //Check task log file
                    if (File.Exists($@"{curr_path}\task.log"))
                    {
                         MainWindow.AddDebugOutputLine("Task log file exists");
                         is_installed = true;
                    }
                    else
                    {
                         MainWindow.AddDebugOutputLine("WARNING >>> Task log file doesn't exist. This isn't a fatal issue, installation still valid");
                         is_installed = true;
                    }
               }
               else 
               { 
                    MainWindow.AddDebugOutputLine("Resources directory missing. Please use install again to fix issues");
                    is_installed = false;
               }

               //check scheduled task exists
               string task_name = "HarnessingEngineeringsNextTechnologicalAdvancementsInnovatively";
               using (TaskService this_service = new())
               {
                    Task task = this_service.GetTask(task_name);
                    if (task != null)
                    {
                         MainWindow.AddDebugOutputLine("Task exists");
                         is_installed = true;
                    }
                    else
                    {
                         MainWindow.AddDebugOutputLine("WARNING >>> Task doesn't exist");
                         is_installed = false;
                    }
               }

               return is_installed;

          }
     }
}