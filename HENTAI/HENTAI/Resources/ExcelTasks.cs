using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using HENTAI;
using HENTAI.Resources;
using Microsoft.VisualBasic.FileIO;

namespace HENTAI.Resources
{
    public static class ExcelTasks
    {
          public static void GetCSVData(MainWindow MW)
          {
               string filepath = $@"{Environment.CurrentDirectory}\Resources\meetings.csv";
               if (!File.Exists(filepath)) { MW.AddColoredDebugOutputLine("Cannot find CSV datapath", Colors.LightSalmon); }

               DataTable meetings_table = new();
               try
               {
                    using (TextFieldParser parser = new(filepath))
                    {
                         parser.TextFieldType = FieldType.Delimited;
                         parser.SetDelimiters(",");
                         bool is_first_row = true;

                         while (!parser.EndOfData && parser != null)
                         {
                              string[] fields = parser.ReadFields();
                              if (is_first_row)
                              {
                                   foreach (string field in fields) { meetings_table.Columns.Add(field); }
                                   is_first_row = false;
                              }
                              else
                              {
                                   DataRow row = meetings_table.NewRow();
                                   for (int i = 0; i < fields.Length; i++) { row[i] = fields[i]; }
                                   meetings_table.Rows.Add(row);
                              }
                         }
                    }
                    foreach(DataRow row in meetings_table.Rows)
                    {
                         foreach(var item in row.ItemArray)
                         {
                              Debug.WriteLine($"{item}\t");
                         }
                         Debug.WriteLine("\n");
                    }
               }
               catch(Exception ex)
               {
                    MW.AddColoredDebugOutputLine($"Error parsing meetings CSV file: {ex.Message}", Colors.LightSalmon);
               }
               
          }
    }
}
