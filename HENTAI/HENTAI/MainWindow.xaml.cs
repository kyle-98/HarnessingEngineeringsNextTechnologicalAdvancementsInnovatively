using HENTAI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HENTAI
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          string app_config_filepath = @$"{Environment.CurrentDirectory}\Resources\app_config.json";

          public MainWindow()
          {
               InitializeComponent();
          }

          public async Task RefreshConfigs()
          {
               //read json file here
          }

          private void install_button_Click(object sender, RoutedEventArgs e)
          {
               Setup.CheckForResources(this);
               Setup.ScheduleTask(this);
               Setup.CreateLogFile(this);
               Setup.CreatePowershellScript(this);
          }

          private void uninstall_button_Click(object sender, RoutedEventArgs e)
          {
               Setup.RemoveTask(this);
               Setup.DeleteLogFile(this);
               Setup.DeletePowershellScript(this);
          }

          public void AddDebugOutputLine(string outputLine)
          {
               DebugOutputTextbox.AppendText($"[{DateTime.Now}] {outputLine}{Environment.NewLine}");
               DebugOutputTextbox.ScrollToEnd();
          }
     }
}
