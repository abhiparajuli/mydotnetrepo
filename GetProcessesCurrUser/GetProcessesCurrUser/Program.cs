using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;

namespace GetProcessesCurrUser
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No process name given. Aborting...");
                return;
            }
            Console.WriteLine(string.Format("Getting instances of process {0} in current user session...",args[0]));
            Console.WriteLine("Number of instances = " + (GetProcessesInCurrentUserSession(args[0])).Count);
        }

        /// <summary>
        /// Returns a list of running processes in current user's session given a process name
        /// </summary>
        /// <param name="ProcName"></param>
        /// <returns></returns>
        public static List<Process> GetProcessesInCurrentUserSession(string ProcName)
        {
            Console.WriteLine("Begin GetProcessesInCurrentUserSession()=" + ProcName);
            List<Process> processList = new List<Process>();
            try
            {
                string currUsername = getCurrentUsername();
                
                //wmi query to grab all the processes with the specified name
                ObjectQuery searchQuery = new ObjectQuery("Select * from Win32_Process Where Name = '" + ProcName + ".exe'");
                ManagementObjectSearcher processFinder = new ManagementObjectSearcher(searchQuery);
                ManagementObjectCollection processes = processFinder.Get();

                //iterate over processes' management objects
                foreach (ManagementObject managementObject in processes)
                {
                    try
                    {
                        //grab the process id
                        Int32 pId = Convert.ToInt32(managementObject["ProcessId"]);
                        Process process = Process.GetProcessById(pId);

                        //determine the process owner
                        string processOwner = getProcessOwner(managementObject);

                        //add the process to the list if the owner is the current user                
                        if (processOwner.Equals(currUsername, StringComparison.OrdinalIgnoreCase))
                        {
                            processList.Add(process);
                        }
                    }
                    catch (Exception exInner)
                    {
                        Console.WriteLine("ManagementObject error in {GetProcessesInCurrentUserSession}." + Environment.NewLine + exInner.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing {GetProcessesInCurrentUserSession}." + Environment.NewLine + ex.ToString());
            }
            Console.WriteLine("End GetProcessesInCurrentUserSession()=" + ProcName + " Number of processes=" + processList.Count);
            return processList;
        }

        /// <summary>
        /// get current username
        /// </summary>
        /// <returns></returns>
        private static string getCurrentUsername()
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            string currentUsername = user.Name;
            if (user == null)
            {
                Console.WriteLine("Current user is NULL. Aborting...");
                Environment.Exit(1);
            }
            return currentUsername;
        }

        /// <summary>
        /// helper method to determine the process owner given the management object of the process
        /// </summary>
        /// <param name="managementObject"></param>
        /// <returns></returns>
        private static string getProcessOwner(ManagementObject managementObject)
        {
            string processOwner = "";
            object[] processOwnerInfo = new object[2];
            managementObject.InvokeMethod("GetOwner", processOwnerInfo);

            processOwner = (string)processOwnerInfo[0];
            string domainQ = (string)processOwnerInfo[1];

            if (!string.IsNullOrEmpty(domainQ))
            {
                processOwner = string.Format("{0}\\{1}", domainQ, processOwner);
            }
            return processOwner;
        }
    }
}
