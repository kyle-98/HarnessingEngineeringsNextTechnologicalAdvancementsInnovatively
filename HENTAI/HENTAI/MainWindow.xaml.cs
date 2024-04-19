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

          private async void scheduleTask_button_Click(object sender, RoutedEventArgs e)
          {
               await Task.Run(() => WindowsScheduler.ScheduleTask());
          }

          private async void removeTask_button_Click(object sender, RoutedEventArgs e)
          {
               await Task.Run(() => WindowsScheduler.RemoveTask());
          }

          public async Task AddDebugOutputLine(string outputLine)
          {

          }
     }
}
