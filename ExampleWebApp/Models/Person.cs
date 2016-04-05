using BloodhoundHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleWebApp.Models
{
    public class Person
    {

        [BloodhoundToken]
        public string FirstName { get; set; }
        [BloodhoundToken]
        public string LastName { get; set; }
        [BloodhoundValue]
        public string FullName { get { return String.Format("{0} {1}", FirstName, LastName); } }
        public string JobTitle { get; set; }
        [BloodhoundToken(Format = "{0:d}")]
        [BloodhoundToken(Format = "{0:D}")]
        [BloodhoundToken(Format = "{0:yyyy}")]
        [BloodhoundData(Name = "dateOfBirth", Format = "{0:D}")]
        public DateTime DateOfBirth { get; set; }

    }
}