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


        static Dictionary<string, Stopwatch> stop = new Dictionary<string, Stopwatch>();

        

        static string currentTask = "";

        static List<string> uniqueApplicationNames = new List<string>();

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

        static void ViewTimeOfCurrentApplication(string currentApplication)
        {
            Console.WriteLine($"{currentApplication} has been running for {stop[currentApplication]} seconds");
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


        private void countActivityOnActiveWindow()
        {
            var title = GetTitleOfActiveApplication().MainWindowTitle;
            if(title != currentTask )
            {
                //Set the currentTask as this
                currentTask = title;
                StartStopwatch(title);

                


            }
        }

        public static void pauseAllRunningTimers()
        {
            foreach(var item in stop)
            {
                PauseStopwatch(item.Key);
            }

        }




        //This function gets the unique applications and calls subscriber
        public void getTheUniqueApplications()
        {
            /*
             * CHEAT SHEET *
            //Both Get the executable path for the application running  *BELOW*
            //Console.WriteLine( GetTitleOfActiveApplication().MainModule.FileName);

            //Gets the common name for the application like "Spotify" "Chrome" etc..
            //Console.WriteLine(GetTitleOfActiveApplication().ProcessName);

            //Gets the time the user started the applciation (even works on specific tabs on Chrome)
            //Console.WriteLine(GetTitleOfActiveApplication().StartTime);
            */


            //Gets the name of the tab for the active window along with the process name
            //Console.WriteLine(GetTitleOfActiveApplication().MainWindowTitle);


            /*
            //Console.WriteLine(GetCaptionOfActiveWindow());
            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            */

            //For dynamic sizing it is easier to use a list compared to an array bc you don't have to know size
            ///Declare a list of applications to store the unique processes.
           
 
            ///Get the application name
            string applicationName = GetTitleOfActiveApplication().ProcessName;

            ///Write to a file to test this
            // Set a variable to the Documents path.
            string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //Pause any existing timers
            pauseAllRunningTimers();

            if (uniqueApplicationNames.Contains(applicationName) == false)
            {
                Console.WriteLine("Detected new application running!");


                

                /// Append text to an existing file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "CurrentApplication.txt"), true))
                {
                    outputFile.WriteLine(applicationName, GetTitleOfActiveApplication().StartTime);
                    
                }
                uniqueApplicationNames.Add(applicationName);

                //Initialize a timer to track how long the current application is being used for
                createTimerForCurrentApplication(applicationName);
                //Begin the timer
                startTimingCurrentApplication(applicationName);



                
            } else
            {
                //The item has already previously existed and the timer should just continue
                startTimingCurrentApplication(applicationName);
            }
            //Console.WriteLine(uniqueApplicationNames.Count);
        }
        

        /// <summary>
        /// This function will subscribe a process to the counter and then unsubscribe when it's closed
        /// </summary>
        /// 

        public DateTime StartTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public object activeSession { get; set; }

        public void subscribeProcess(object activeSession)
        {
            this.activeSession = activeSession;
        }
        public void programActivated()
        {
            this.StartTime = DateTime.Now;
            Console.WriteLine(this.StartTime);
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

                string sql = "CREATE TABLE IF NOT EXISTS userActivity (application VARCHAR(20), time DOUBLE)";

                SqliteCommand command = new SqliteCommand(sql, connection);

                command.ExecuteNonQuery();

                foreach(var item in stop)
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




        




        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            //
            while (!stoppingToken.IsCancellationRequested)
            {
                //Call the method to get the unqiue applications and record the amount of time spent on the applications
                //Only problem is that this is being called repeatedly every second. So the function never truly stores anything in it's scope.
                getTheUniqueApplications();

                ViewTimesOfAllRunningApplications();


                await Task.Delay(1000, stoppingToken);
            }
            

        }
    }
}
