using System;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using Microsoft.Win32.TaskScheduler;
using HENTAI;
using System.IO;
using System.Data;
using HENTAI.Resources;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Threading;

public static class OutlookOperations
{
	public static void ForceFetch(MainWindow MainWindow)
	{
          string script_path = @$"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1";
          ProcessStartInfo process_info = new ProcessStartInfo
          {
               FileName = @"C:\Windows\System32\WindowsPowershell\v1.0\powershell.exe",
               Arguments = $"-ExecutionPolicy Bypass -File \"{script_path}\"",
               RedirectStandardOutput = false,
               RedirectStandardError = true,
               UseShellExecute = false,
               CreateNoWindow = true
          };

          using (Process process = new())
          {
               process.StartInfo = process_info;
               process.Start();
               string errors = process.StandardError.ReadToEnd();
               process.WaitForExitAsync();
               if (errors != string.Empty) { MainWindow.AddColoredDebugOutputLine(errors, Colors.LightSalmon); }
               else { MainWindow.AddColoredDebugOutputLine("Outlook data fetched", Colors.LightGreen); }
          }
     }

     public static void KillOutlook(MainWindow MainWindow)
     {
          Process[] outlook_process = Process.GetProcessesByName("OUTLOOK");
          foreach(Process process in outlook_process)
          {
               try
               {
                    process.Kill();
                    process.WaitForExit();
                    process.Dispose();
                    MainWindow.AddColoredDebugOutputLine($"Process: {process} forcefully killed", Colors.LightGreen);
               }
               catch (Exception ex)
               {
                    MainWindow.AddColoredDebugOutputLine($"{process} failed to be killed: {ex.Message}", Colors.LightSalmon);
               }
          }
     }
}
