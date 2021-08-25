using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.Sqlite;

namespace GetCurrentApplication
{
    public class Worker : BackgroundService
    {
       
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        //Create a dynamic array of Applications
        //List<Application> listOfAllRunningApplications = new List<Application>();


        //static Dictionary<string, Stopwatch> stop = new Dictionary<string, Stopwatch>();





        /*
        public static void initStopwatch()
        {
            Console.WriteLine("the number of unqiue applcations is currently: ", uniqueApplicationNames.Count);
            foreach (string item in uniqueApplicationNames)
            {
                if(stop.ContainsKey(item) == false)
                {
                    stop.Add(item, new Stopwatch());
                }
                
            }
        }
        */
        static void createStopwatch(string item)
        {
            stop.Add(item, new Stopwatch());
        }

        static void StartStopwatch(string item)
        {
            stop[item].Start();

        }
        static void PauseStopwatch(string item)
        {
            stop[item].Stop();
        }
        
        static void ViewTimesOfAllRunningApplications()
        {
            foreach(var item in stop)
            {
                Console.WriteLine($"Gazed at {item.Key} for {item.Value.Elapsed.TotalSeconds} seconds");
            }
        }
       
        static string findCurrentApplicationName()
        {
            string applicationTitle = GetTitleOfActiveApplication().ProcessName;
            return applicationTitle;
        }

        static void createTimerForCurrentApplication(string currentApplication)
        {
            createStopwatch(currentApplication);
        }

        static void startTimingCurrentApplication(string currentApplication)
        {
            StartStopwatch(currentApplication);
        }



       


        internal struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        
        private string GetCaptionOfActiveWindow()
        {

            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;

        }
       

        static Process GetTitleOfActiveApplication()
        {
            IntPtr handle = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(handle, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p;

        }

        [DllImport("User32.dll")]
        public static extern bool LockWorkStation();

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();

        public static uint GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            GetLastInputInfo(ref lastInPut);

            


            return ((uint)Environment.TickCount - lastInPut.dwTime);
        }

        public static long GetLastInputTimeInMilliSeconds()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
            if (!GetLastInputInfo(ref lastInPut))
            {
                throw new Exception(GetLastError().ToString());
            }

            return lastInPut.dwTime;
        }


      













        //Create a list of instances of the Application class
        List<Application> listOfAllRunningApplications = new List<Application>();

        Application activeApplication;

        List<string> namesOfAllRunningApplications = new List<string>();

        static Dictionary<string, Stopwatch> stop = new Dictionary<string, Stopwatch>();

        //static Dictionary<string, Application> collectionOfApplications = new Dictionary<string, Application>

  
        public string getTheCurrentApplicationName()
        {
            string currentApplicationName = GetTitleOfActiveApplication().ProcessName;

            return currentApplicationName;
            
        }

       


        public void pauseAllTimersExceptActiveApplication()
        {
            string activeApplication = getTheCurrentApplicationName();

            foreach(var list in listOfAllRunningApplications)
            {
                if (list.getNameOfApplication() != activeApplication)
                {
                    list.PauseStopwatch();
                }
                else 
                { 
                    list.ResumeStopwatch();
                    list.ViewTimeOfCurrentApplication();

                }
            }
           
        }


        public bool checkIfCurrentApplicationHasRunBefore()
        {
            string currentApplication = getTheCurrentApplicationName();

            if (namesOfAllRunningApplications.Contains(currentApplication))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void checkActiveApplication()
        {
            pauseAllTimersExceptActiveApplication();
            Console.WriteLine($"{getTheCurrentApplicationName()} is the active screen");

        }




        public void setCurrentApplicationToList()
        {
            if(checkIfCurrentApplicationHasRunBefore() == true)
            {
                checkActiveApplication();
            }
            else if(checkIfCurrentApplicationHasRunBefore() == false)
            {
                //stop all timers
                pauseTheTimer(activeApplication);

                Application currentRunningApplication = createInstanceOfApplication();

                

                activeApplication = currentRunningApplication; 
                
                addInstatiatedObjectToApplicationList(currentRunningApplication);

                addInstatiatedObjectToStringList();
            }
        }

        public void pauseTheTimer(Application currentApplication)
        {
            if (currentApplication != null)
            {
                currentApplication.PauseStopwatch();

            }
            else return;
        }

       

 
        public Application createInstanceOfApplication()
        {
            string nameOfApplication = getTheCurrentApplicationName();

            Application application = new Application(nameOfApplication);

            return application;
            
        }

        public void addInstatiatedObjectToApplicationList(Application activeApplication)
        {
            listOfAllRunningApplications.Add(activeApplication);
        }

        public void addInstatiatedObjectToStringList()
        {
            string currentApplication = getTheCurrentApplicationName();
            namesOfAllRunningApplications.Add(currentApplication);
        }

        public void lookForApplications()
        {
            setCurrentApplicationToList();
        }

        























        





     

        public static void writeAllDataToFile()
        {
            Console.WriteLine("Program safely closing");
            using (StreamWriter file = new StreamWriter("getCurrentApplicationData.txt"))
            foreach(var item in stop)
                {
                    file.WriteLine("{0}: {1}", item.Key, item.Value.Elapsed.TotalSeconds);
                }
            writeToDatabase();
        }

        public static void writeToDatabase()
        {
            using (var connection = new SqliteConnection("Data Source=hello.db"))
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS userActivity (date application VARCHAR(20), time DOUBLE)";

                SqliteCommand command = new SqliteCommand(sql, connection);

                command.ExecuteNonQuery();

                foreach (var item in stop)
                {
                    string sql2 = $"INSERT into userActivity (application, time) values('{item.Key}', {item.Value.Elapsed.TotalSeconds} )";
                    SqliteCommand command2 = new SqliteCommand(sql2, connection);
                    command2.ExecuteNonQuery();

                }


                connection.Close();
            }

        }


        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public static string test()
        {
            return "Hello world";
        }


        




        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine("Test");
            
            //
            while (!stoppingToken.IsCancellationRequested)
            {
                //Call the method to get the unqiue applications and record the amount of time spent on the applications
                //Only problem is that this is being called repeatedly every second. So the function never truly stores anything in it's scope.
                //getTheUniqueApplications();

                lookForApplications();


                await Task.Delay(1000, stoppingToken);
            }
            

        }
    }
}
