using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Management;

namespace ConsoleApp1 
{
    internal class Program
    {
        static async Task Main()
        {
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                Console.WriteLine($"Number Of Physical Processors on this machine: {item["NumberOfProcessors"]} ");
            }
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                Console.WriteLine($"Number Of Cores on this machine: {int.Parse(item["NumberOfCores"].ToString())}");
            }
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                Console.WriteLine($"Number Of Logical Processors on this machine: {item["NumberOfLogicalProcessors"]}");
            }

            List<TaskWrapper> stuffToDelegate = new List<TaskWrapper>();

            for (var i = 0; i < 40; ++i)
            {
                var t = new Task(() =>
                {
                    Console.WriteLine($"I am task with Id {Task.CurrentId}. I was assigned a logical processor {Thread.GetCurrentProcessorId()} at {DateTime.Now.ToString("hh.mm.ss.ffff")}.");
                    Thread.Sleep(4000);
                    Console.WriteLine($"I am task with Id {Task.CurrentId}. My work is now done, and I was released from a logical processor {Thread.GetCurrentProcessorId()} at {DateTime.Now.ToString("hh.mm.ss.ffff")}. My work load took {4000} milliseconds to do.");
                });
                stuffToDelegate.Add(new TaskWrapper() {TheTask = t, TaskCreatedAt = DateTime.Now.ToString("hh.mm.ss.fff"), TaskId = i});
            }

            Console.WriteLine("Chillar lite, om 4 sekunder kör vi task.Start() på alla 40 tasks samtidigt (ish)");
            Thread.Sleep(4000);

            foreach (var item in stuffToDelegate)
            {
                Console.WriteLine($"Task with id {item.TheTask.Id}. Was created at {item.TaskCreatedAt}. Was added to the task pool's todo list at {DateTime.Now.ToString("hh.mm.ss.fff")}");
                item.TheTask.Start();
            }
            Console.ReadKey();
        }
    }

    internal class TaskWrapper
    {
        public Task TheTask  { get; init; }
        public string TaskCreatedAt { get; init; }
        public int TaskId  { get; set; }
    }
}
