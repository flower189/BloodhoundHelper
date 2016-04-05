using BloodhoundHelper.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BloodhoundHelper.Tests
{
    public class BloodhoundFluentTests
    {

        #region GetValue

        [Fact]
        public void FluentGetValueTest()
        {
            string expected = "Matthew Lawrence";
            PersonSimple person = new PersonSimple { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            string actual = bloodhound.GetValue(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FluentGetValueEmptyTest()
        {
            string expected = " ";
            PersonSimple person = new PersonSimple();

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            string actual = bloodhound.GetValue(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FluentGetValueNullTest()
        {
            string expected = String.Empty;
            PersonComplex person = new PersonComplex();

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            string actual = bloodhound.GetValue(person);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetTokens

        [Fact]
        public void GetTokensTest()
        {
            string[] expected = { "Matthew", "Lawrence" };
            PersonSimple person = new PersonSimple { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            string[] actual = bloodhound.GetTokens(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FluentGetTokensComplexTest()
        {
            string[] expected = { "Matthew", "Lawrence", "Information Technology" };
            PersonComplex person = new PersonComplex { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2), Department = new Department { Name = "Information Technology" } };

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            string[] actual = bloodhound.GetTokens(person);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetData

        [Fact]
        public void FluentGetDataTest()
        {
            IDictionary<string, object> expected = new Dictionary<string, object>
            {
                { "FirstName", "Matthew" },
                { "LastName", "Lawrence" },
                { "DateOfBirth", "02/05/1990 00:00:00" }
            };

            PersonSimple person = new PersonSimple { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            IDictionary<string, object> actual = bloodhound.GetData(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FluentGetDataNameTest()
        {
            IDictionary<string, object> expected = new Dictionary<string, object>
            {
                { "firstName", "Matthew" },
                { "lastName", "Lawrence" },
                { "dob", "02/05/1990 00:00:00" }
            };

            PersonComplex person = new PersonComplex { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound(GetConfiguration());
            IDictionary<string, object> actual = bloodhound.GetData(person);
            Assert.Equal(expected, actual);
        }

        #endregion

        private BloodhoundConfiguration GetConfiguration()
        {
            var config = new BloodhoundConfiguration();
            config.Maps.Add(new PersonSimpleMap());
            config.Maps.Add(new PersonComplexMap());
            return config;
        }

        public class PersonSimple
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string FullName { get { return FirstName + " " + LastName; } }
        }

        public class PersonComplex
        {
            [BloodhoundToken]
            [BloodhoundData(Name = "firstName")]
            public string FirstName { get; set; }
            [BloodhoundToken]
            [BloodhoundData(Name = "lastName")]
            public string LastName { get; set; }
            [BloodhoundData(Name = "dob")]
            public DateTime DateOfBirth { get; set; }
            [BloodhoundValue]
            [BloodhoundToken]
            public Department Department { get; set; }
            public string FullName { get { return FirstName + " " + LastName; } }
        }

        public class Department
        {
            public string Name { get; set; }

            public override string ToString()
            {
                return this.Name;
            }
        }

        public class PersonSimpleMap : BloodhoundMap<PersonSimple>
        {
            public PersonSimpleMap()
            {
                Value(x => x.FullName);
                Data(x => x.FirstName);
                Data(x => x.FirstName);
                Data(x => x.LastName);
                Data(x => x.DateOfBirth);
                Token(x => x.FirstName);
                Token(x => x.LastName);
            }
        }

        public class PersonComplexMap : BloodhoundMap<PersonComplex>
        {
            public PersonComplexMap()
            {
                Value(x => x.Department);
                Data(x => x.FirstName, name: "firstName");
                Data(x => x.LastName, name: "lastName");
                Data(x => x.DateOfBirth, name: "dob");
                Token(x => x.FirstName);
                Token(x => x.LastName);
                Token(x => x.Department);
            }
        }

    }
}
