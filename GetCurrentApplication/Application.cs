using System;
using System.Collections.Generic;
using System.Text;

namespace GetCurrentApplication
{
    class Application
    {
        //Has a date
        private DateTime date;
        //A time
        private DateTime timeStarted;
        private DateTime timeEnded;
        //A string (name of the application)
        private string nameOfApplication;
        //Amount of time
        private double timeSpentOnApplication;



        ///functions
        public DateTime getDate()
        {
            return date;
        }
        public void setDate(DateTime passedDate)
        {
            date = passedDate;
        }

        public DateTime getTimeStarted()
        {
            return timeStarted;
        }


        public void setTimeStarted(DateTime time)
        {
            timeStarted = time;
        }
        public DateTime getTimeEnded( )
        {
            return timeEnded;
        }
        public void setTimeEnded(DateTime time)
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
            return timeSpentOnApplication;
        }
        public void setTimeSpentOnApplication(double time)
        {
            timeSpentOnApplication = time;
        }
    }
}
