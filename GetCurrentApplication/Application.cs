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
        private DateTime time;
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

        public DateTime getTime()
        {
            return time;
        }
        public void getDate(DateTime passedTime)
        {
            time = passedTime;
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
