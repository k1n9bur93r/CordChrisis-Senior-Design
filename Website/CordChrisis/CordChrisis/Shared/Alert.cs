using System;
using System.Collections.Generic;
using System.Text;

namespace CordChrisis.Shared
{
    class Alert
    {

        public string Message { get; set; }
        public int AlertType { get; set; }
        public Alert()
        {
            Message = "Something went wrong!";
            AlertType = 0;
        }
        public Alert(string message, int alertType)
        {
            this.Message = message;
            this.AlertType = alertType;
        }
    }

    //class AlertService { 
    //public class AlertService 
    //}
}
