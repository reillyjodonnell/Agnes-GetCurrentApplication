using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace GetCurrentApplication
{
    public class Application
    {
        //Has a date
        private string date;
        //A time
        private TimeSpan timeStarted;
        private TimeSpan timeEnded;
        //A string (name of the application)
        private string nameOfApplication;
        //Amount of time
        private double timeSpentOnApplication;

        //the timer
        private Stopwatch stopwatch = Stopwatch.StartNew();

        


        // When I create an application I want several things:
        // 1. To immediately start the timer
        // 2. To create variable instances
        public Application(string applicationName)
        {
            nameOfApplication = applicationName;
            date = DateTime.Now.ToString("MM-dd-yyyy");
            timeStarted = DateTime.Now.TimeOfDay;      
        }

        public void PauseStopwatch()
        {
            stopwatch.Stop();
        }

        public void ResumeStopwatch()
        {
            stopwatch.Start();
        }

        public void consoleTimeElapsed()
        {
            Console.WriteLine(stopwatch.Elapsed.TotalSeconds);
        }
        

        public void ViewTimeOfCurrentApplication( )
        {
            Console.WriteLine($"{nameOfApplication} has been running for {stopwatch.Elapsed.TotalSeconds} seconds");
        }





        
        ///getter and setter methods
        public string getDate()
        {
            return date;
        }


        public TimeSpan getTimeStarted()
        {
            return timeStarted;
        }
        public void setTimeStarted(TimeSpan time)
        {
            timeStarted = time;
        }

        public TimeSpan getTimeEnded()
        {
            return timeEnded;
        }
        public void setTimeEnded(TimeSpan time)
        {
            timeEnded = time;
        }
        public string getNameOfApplication()
        {
            return nameOfApplication;
        }
        public void setNameOfApplication(string name)
        {
            nameOfApplication = name;
        }
        


        public double getTimeSpentOnApplication()
        {
            return stopwatch.Elapsed.TotalSeconds;
        }
        public void setTimeSpentOnApplication(double time)
        {
            timeSpentOnApplication = time;
        }
     

        /*
        // Set a variable to the Documents path.
        string docPath =
          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //Pause any existing timers
        pauseAllRunningTimers();

            if (listOfApplicationNames.Contains(currentApplicationName) == false)
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
        */
    }
}
