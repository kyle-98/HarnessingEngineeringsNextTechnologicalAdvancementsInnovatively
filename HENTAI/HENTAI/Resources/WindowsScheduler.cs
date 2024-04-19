using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32.TaskScheduler;

public static class WindowsScheduler
{
	public static void ScheduleTask()
	{
		using (TaskService this_service = new())
		{
			TaskDefinition outlook_task = this_service.NewTask();
			outlook_task.RegistrationInfo.Description = "Harnessing Engineering's Next Technological Advancements Innovatively";
			outlook_task.Principal.UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

			WeeklyTrigger weeklyTrigger = new WeeklyTrigger();
			weeklyTrigger.StartBoundary = DateTime.Today.AddDays(1).AddHours(3);

			string action_path = @$"{Environment.CurrentDirectory}\Resources\fruitsnacks.ps1";
			outlook_task.Actions.Add(action_path);
			try
			{
				this_service.RootFolder.RegisterTaskDefinition(@"HarnessingEngineeringsNextTechnologicalAdvancementsInnovatively", outlook_task);
			}
			catch(Exception ex)
			{

			}
		}
	}

	public static void RemoveTask()
	{
		string task_name = "HarnessingEngineeringsNextTechnologicalAdvancementsInnovatively";
		using(TaskService this_service = new())
		{
			Task task = this_service.GetTask(task_name);
			if(task != null)
			{
				this_service.RootFolder.DeleteTask(task_name);
			}
		}

     }
}
