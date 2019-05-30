using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Aplikacja.Models
{
    public class Guide
    {
        //Deklaracje pól w tabeli guides
        public int Id { get; set; }
        public string AuthId { get; set; }
        public string AuthName { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Category Category { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
    public enum Category//przypisanie wartości dla kategorii
    {
        Base = 1,
        Static,
        Freestyle,
        Other
    }
}