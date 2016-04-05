using ExampleWebApp.Models;
using BloodhoundHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExampleWebApp.Controllers
{
    public class HomeController : Controller
    {

        private IBloodhound _bloodhound;

        public HomeController()
        {
            _bloodhound = new Bloodhound();
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("Home/CountySearch/{query}")]
        public ActionResult CountySearch(string query)
        {
            IEnumerable<string> results = _bloodhound.Search(Counties.Data, query);

            IDictionary<string, object>[] bloodhoundResults = _bloodhound.BuildResults(results);

            return Json(bloodhoundResults, JsonRequestBehavior.AllowGet);
        }

        [Route("Home/NumberSearch/{query}")]
        public ActionResult NumberSearch(string query)
        {
            // Generate data.
            IEnumerable<int> numbers = Enumerable.Range(0, 500);

            IEnumerable<int> results = _bloodhound.Search(numbers, query);

            IDictionary<string, object>[] bloodhoundResults = _bloodhound.BuildResults(results);

            return Json(bloodhoundResults, JsonRequestBehavior.AllowGet);
        }

        [Route("Home/PersonSearch/{query}")]
        public ActionResult PersonSearch(string query)
        {
            Person person1 = new Person
            {
                FirstName = "Cameron",
                LastName = "Davidson",
                DateOfBirth = new DateTime(1983, 7, 19),
                JobTitle = "Software Developer"
            };
            Person person2 = new Person
            {
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = new DateTime(1982, 4, 24),
                JobTitle = "The Average Man"
            };
            Person person3 = new Person
            {
                FirstName = "Jill",
                LastName = "Kape",
                DateOfBirth = new DateTime(1947, 2, 11),
                JobTitle = "Developer"
            };

            List<Person> people = new List<Person>();
            people.Add(person1);
            people.Add(person2);
            people.Add(person3);

            //var results = people.Where(x => x.FirstName.ToLower().Contains(query) || x.LastName.ToLower().Contains(query) || x.JobTitle.ToLower().Contains(query));

            var results = _bloodhound.Search(people, query);

            var bloodhoundResults = _bloodhound.BuildResults(results);

            return Json(bloodhoundResults, JsonRequestBehavior.AllowGet);
        }
    }
}