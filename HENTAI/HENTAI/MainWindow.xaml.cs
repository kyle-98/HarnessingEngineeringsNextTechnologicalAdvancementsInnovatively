using HENTAI.Resources;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;

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
               if (Setup.CheckInstall(this)) 
               {
                    forcefetch_button.IsEnabled = true;
                    install_button.IsEnabled = false; 
                    uninstall_button.IsEnabled = true;
                    AddDebugOutputLine("Installation valid");
               }
               else
               {
                    install_button.IsEnabled = true; 
                    uninstall_button.IsEnabled = false;
                    AddDebugOutputLine("Installation not valid");
               }
               AddDebugOutputLine("-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-");
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
               if (Setup.CheckInstall(this))
               {
                    forcefetch_button.IsEnabled = true;
                    install_button.IsEnabled = false;
                    uninstall_button.IsEnabled = true;
                    AddColoredDebugOutputLine("Installation successful PogU's in the chat", Colors.LightGreen);
               }
               else { AddColoredDebugOutputLine("Installation failed, try again", Colors.Red); }
               
               AddDebugOutputLine("-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-");

          }

          private void uninstall_button_Click(object sender, RoutedEventArgs e)
          {
               Setup.RemoveTask(this);
               Setup.DeleteLogFile(this);
               Setup.DeletePowershellScript(this);
               if (!Setup.CheckInstall(this))
               {
                    forcefetch_button.IsEnabled = false;
                    uninstall_button.IsEnabled = false;
                    install_button.IsEnabled = true;
                    AddColoredDebugOutputLine("Uninstallation successful", Colors.LightGreen);
               }
               else { AddColoredDebugOutputLine("Uninstallation failed, try again", Colors.Red); }
               AddDebugOutputLine("-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-");
          }

          public void AddDebugOutputLine(string output_line)
          {
               Color default_color = (Color)ColorConverter.ConvertFromString("#F7F7F7F7");
               //DebugOutputTextbox.AppendText($"[{DateTime.Now}] {output_line}{Environment.NewLine}");
               AddColoredDebugOutputLine(output_line, default_color);
               DebugOutputTextbox.ScrollToEnd();
          }

          private void AddColoredDebugOutputLine(string output_line, Color color)
          {
               Run run = new Run($"[{DateTime.Now}] {output_line}");
               run.Foreground = new SolidColorBrush(color);
               FlowDocument fd = DebugOutputTextbox.Document;
               fd.Blocks.Add(new Paragraph(run));
               DebugOutputTextbox.ScrollToEnd();
          }


          private void forcefetch_button_Click(object sender, RoutedEventArgs e)
          {

          }
     }
}
