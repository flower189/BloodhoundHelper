using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BloodhoundHelper;
using System.Collections;
using Xunit.Extensions;

namespace BloodhoundHelper.Tests
{
    public class BloodhoundTests
    {

        #region GetValue

        [Fact]
        public void GetValueTest()
        {
            string expected = "Matthew Lawrence";
            PersonSimple person = new PersonSimple { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound();
            string actual = bloodhound.GetValue(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValueEmptyTest()
        {
            string expected = " ";
            PersonSimple person = new PersonSimple();

            Bloodhound bloodhound = new Bloodhound();
            string actual = bloodhound.GetValue(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetValueNullTest()
        {
            string expected = String.Empty;
            PersonComplex person = new PersonComplex();

            Bloodhound bloodhound = new Bloodhound();
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

            Bloodhound bloodhound = new Bloodhound();
            string[] actual = bloodhound.GetTokens(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetTokensComplexTest()
        {
            string[] expected = { "Matthew", "Lawrence", "Information Technology" };
            PersonComplex person = new PersonComplex { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2), Department = new Department { Name = "Information Technology" } };

            Bloodhound bloodhound = new Bloodhound();
            string[] actual = bloodhound.GetTokens(person);
            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetData

        [Fact]
        public void GetDataTest()
        {
            IDictionary<string, object> expected = new Dictionary<string, object>
            {
                { "FirstName", "Matthew" },
                { "LastName", "Lawrence" },
                { "DateOfBirth", "02/05/1990 00:00:00" }
            };

            PersonSimple person = new PersonSimple { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound();
            IDictionary<string, object> actual = bloodhound.GetData(person);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetDataNameTest()
        {
            IDictionary<string, object> expected = new Dictionary<string, object>
            {
                { "firstName", "Matthew" },
                { "lastName", "Lawrence" },
                { "dob", "02/05/1990 00:00:00" }
            };

            PersonComplex person = new PersonComplex { FirstName = "Matthew", LastName = "Lawrence", DateOfBirth = new DateTime(1990, 5, 2) };

            Bloodhound bloodhound = new Bloodhound();
            IDictionary<string, object> actual = bloodhound.GetData(person);
            Assert.Equal(expected, actual);
        }

        #endregion

    }


    public class PersonSimple
    {
        [BloodhoundToken]
        [BloodhoundData]
        public string FirstName { get; set; }
        [BloodhoundToken]
        [BloodhoundData]
        public string LastName { get; set; }
        [BloodhoundData]
        public DateTime DateOfBirth { get; set; }
        [BloodhoundValue]
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

}
