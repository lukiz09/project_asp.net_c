using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aplikacja.Models
{
    public class Opinion
    {
        //Deklaracje pól w tabeli opinions
        public int Id { get; set; }
        public string Body { get; set; }
        public string AuthId { get; set; }

        public string AuthName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}