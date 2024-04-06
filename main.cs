using System;
using System.Diagnostics;
using System.Management;

class Program
{
    static void Main(string[] args)
    {
        int choice = 0;
        do
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Start a process");
            Console.WriteLine("2. List processes");
            Console.WriteLine("3. Kill a process");
            Console.WriteLine("4. Display processes tree and kill process");
            Console.WriteLine("5. Exit");
            Console.Write("Please enter the number of the operation you want to perform: ");

            try
            {
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        StartProcess();
                        break;
                    case 2:
                        ListProcesses();
                        break;
                    case 3:
                        KillProcess();
                        break;
                    case 4:
                        DisplayProcessesTreeAndKillProcess();
                        break;
                    case 5:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid operation!");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine();
        } while (choice != 5);
    }

    static void StartProcess()
    {
        Console.Write("Enter the process name: ");
        string processName = Console.ReadLine();

        Process.Start(processName);
    }

    static void ListProcesses()
    {
        Process[] processes = Process.GetProcesses();
        Console.WriteLine("List of processes:");
        foreach (Process process in processes)
        {
            Console.WriteLine($"Name: {process.ProcessName}, PID: {process.Id}, Parent PID: {GetParentId(process.Id)}");
        }
    }

    static void KillProcess()
    {
        Console.Write("Please enter the PID of the process: ");
        int pid = Convert.ToInt32(Console.ReadLine());

        Process process = Process.GetProcessById(pid);
        process.Kill();
        Console.WriteLine($"Process with PID {pid} has been terminated.");
    }

    static int GetParentId(int processId)
    {
        using (ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId=" + processId))
        {
            foreach (ManagementObject mo in mos.Get())
            {
                if (mo["ParentProcessId"] != null)
                {
                    return Convert.ToInt32(mo["ParentProcessId"]);
                }
            }
        }
        return -1; // If parent process ID is not found
    }

    static void DisplayProcessesTreeAndKillProcess()
    {
        Console.WriteLine("Processes Tree:");
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            Console.WriteLine($"Name: {process.ProcessName}, PID: {process.Id}, Parent PID: {GetParentId(process.Id)}");
        }

        Console.Write("Enter the PID of the process you want to terminate: ");
        int pid = Convert.ToInt32(Console.ReadLine());

        KillProcess(pid);
    }

    static void KillProcess(int pid)
    {
        Process process = Process.GetProcessById(pid);
        process.Kill();
        Console.WriteLine($"Process with PID {pid} has been terminated.");
    }
}
