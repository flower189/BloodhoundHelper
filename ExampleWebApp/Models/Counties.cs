using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ExampleWebApp.Models
{
    public static class Counties
    {

        public static string[] Data { get; set; }

        static Counties()
        {
            LoadCounties();
        }

        private static void LoadCounties()
        {
            string filePath = HostingEnvironment.MapPath("~/Counties.txt");
            Data = File.ReadAllLines(filePath);
        }

    }
}