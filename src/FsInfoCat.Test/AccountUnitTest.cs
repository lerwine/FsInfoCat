using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FsInfoCat.Models.Accounts;
using FsInfoCat.Models.DB;
using NUnit.Framework;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class AccountUnitTest
    {
        static object[] GetNormalizeTestCases()
        {
            return new object[]
            {
                new object[]
                {
                    new Account { AccountID = Guid.Empty, DisplayName = "FS InfoCat Administrator", LoginName = "admin",
                        Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 2, 14, 18, 41, 25, 171, DateTimeKind.Utc), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 2, 14, 13, 41, 25, 171, DateTimeKind.Unspecified), ModifiedBy = Guid.Empty },
                    new Account { AccountID = Guid.Empty, DisplayName = "FS InfoCat Administrator", LoginName = "admin",
                        Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 2, 14, 18, 41, 25, 171, DateTimeKind.Utc).ToLocalTime(), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 2, 14, 13, 41, 25, 171, DateTimeKind.Local), ModifiedBy = Guid.Empty }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e"), DisplayName = " Leonard\nErwine ", LoginName = " erwinel ",
                        Role = UserRole.Admin, Notes = " ",
                        CreatedOn = new DateTime(2014, 6, 5, 14, 50, 37, 729, DateTimeKind.Unspecified), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 6, 5, 14, 50, 37, 729, DateTimeKind.Unspecified), ModifiedBy = Guid.Empty },
                    new Account { AccountID = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e"), DisplayName = "Leonard Erwine", LoginName = "erwinel",
                        Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 6, 5, 14, 50, 37, 729, DateTimeKind.Local), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 6, 5, 14, 50, 37, 729, DateTimeKind.Local), ModifiedBy = Guid.Empty }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"), DisplayName = "  \n  ", LoginName = "blackwellt",
                        Role = UserRole.Admin, Notes = "\r\n\n\r\r\n\n\r",
                        CreatedOn = new DateTime(2014, 9, 22, 14, 21, 47, 861, DateTimeKind.Local), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 9, 22, 14, 21, 47, 861, DateTimeKind.Local), ModifiedBy = Guid.Empty },
                    new Account { AccountID = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"), DisplayName = "blackwellt", LoginName = "blackwellt",
                        Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 9, 22, 14, 21, 47, 861, DateTimeKind.Local), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 9, 22, 14, 21, 47, 861, DateTimeKind.Local), ModifiedBy = Guid.Empty }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("96077af4-e35d-45b5-9094-02213cd0ba80"), DisplayName = "Nur Murillo", LoginName = " \t ",
                        Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 1, 8, 13, 19, 32, 381, DateTimeKind.Utc), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2015, 8, 3, 16, 45, 24, 631, DateTimeKind.Unspecified), ModifiedBy = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e") },
                    new Account { AccountID = new Guid("96077af4-e35d-45b5-9094-02213cd0ba80"), DisplayName = "Nur Murillo", LoginName = "",
                        Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 1, 8, 13, 19, 32, 381, DateTimeKind.Utc).ToLocalTime(), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2015, 8, 3, 16, 45, 24, 631, DateTimeKind.Local), ModifiedBy = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e") }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("0cf932e4-be15-4797-84ec-3eb9d86a9376"), DisplayName = "", LoginName = "",
                        Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 4, 22, 20, 11, 52, 39, DateTimeKind.Utc), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2020, 3, 17, 20, 9, 49, 842, DateTimeKind.Utc), ModifiedBy = Guid.Empty },
                    new Account { AccountID = new Guid("0cf932e4-be15-4797-84ec-3eb9d86a9376"), DisplayName = "", LoginName = "",
                        Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 4, 22, 20, 11, 52, 39, DateTimeKind.Utc).ToLocalTime(), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2020, 3, 17, 20, 9, 49, 842, DateTimeKind.Utc).ToLocalTime(), ModifiedBy = Guid.Empty }
                }
            };
        }

        static object[] GetValidateAllTestCases()
        {
            return new object[]
            {
                new object[]
                {
                    new Account { AccountID = Guid.Empty, DisplayName = "FS InfoCat Administrator", LoginName = "admin", Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 2, 14, 13, 41, 25, 171, DateTimeKind.Local), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 2, 14, 13, 41, 25, 171, DateTimeKind.Local), ModifiedBy = Guid.Empty },
                    new ValidationResult[0]
                },
                new object[]
                {
                    new Account { AccountID = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e"), DisplayName = "\n", LoginName = "erwinel", Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 6, 5, 14, 50, 37, 729, DateTimeKind.Local), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 6, 5, 14, 50, 37, 729, DateTimeKind.Local), ModifiedBy = Guid.Empty },
                    new ValidationResult[0]
                },
                new object[]
                {
                    new Account { AccountID = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"), DisplayName = "Tanya Blackwell", LoginName = "", Role = UserRole.Admin, Notes = "",
                        CreatedOn = new DateTime(2014, 9, 22, 14, 21, 47, 861, DateTimeKind.Local), CreatedBy = Guid.Empty,
                        ModifiedOn = new DateTime(2014, 9, 22, 14, 21, 47, 861, DateTimeKind.Local), ModifiedBy = Guid.Empty },
                    new ValidationResult[]
                    {
                        new ValidationResult("Login name cannot be empty.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("96077af4-e35d-45b5-9094-02213cd0ba80"), DisplayName = "Nur Murillo", LoginName = "  ", Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 1, 8, 8, 19, 32, 381, DateTimeKind.Local), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2015, 8, 3, 16, 45, 24, 631, DateTimeKind.Local), ModifiedBy = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e") },
                    new ValidationResult[]
                    {
                        new ValidationResult("Login name cannot be empty.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("0cf932e4-be15-4797-84ec-3eb9d86a9376"), DisplayName = "Hubert Davies", LoginName = ".daviesh", Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 4, 22, 16, 11, 52, 39, DateTimeKind.Local), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2020, 3, 17, 16, 9, 49, 842, DateTimeKind.Local), ModifiedBy = Guid.Empty },
                    new ValidationResult[]
                    {
                        new ValidationResult("Invalid login name.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("8389a46a-0617-4180-80fd-9f3719b77b71"), DisplayName = "NUnit is a unit-testing framework for all .NET languages. Initially ported from JUnit, the current production release has been rewritten with many new features and support for a wide range of .NET platforms. It is a project of the .NET Foundation.",
                    LoginName = "graham.u.", Role = UserRole.Crawler, Notes = "",
                        CreatedOn = new DateTime(2015, 11, 15, 16, 11, 29, 725, DateTimeKind.Local), CreatedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7"),
                        ModifiedOn = new DateTime(2015, 11, 15, 16, 11, 29, 725, DateTimeKind.Local), ModifiedBy = new Guid("7050184e-a998-4088-b547-d70cd806b2c7") },
                    new ValidationResult[]
                    {
                        new ValidationResult("Display name too long.", new string[] { "DisplayName" }),
                        new ValidationResult("Invalid login name.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("79010060-4bb5-47b8-ad33-7f92623a5f3e"), DisplayName = "Doctor of Democrat Socializm Maeinendtacious Mcleodeofolindinski the great of the lasting foreverking and don't you forget it bub", LoginName = "mcleod.m", Role = UserRole.None, Notes = "",
                        CreatedOn = new DateTime(2016, 2, 23, 16, 35, 3, 408, DateTimeKind.Local), CreatedBy = new Guid("90b932df-3ab5-4299-a3f3-dd1655cbf93e"),
                        ModifiedOn = new DateTime(2020, 7, 22, 15, 6, 58, 108, DateTimeKind.Local), ModifiedBy = Guid.Empty },
                    new ValidationResult[]
                    {
                        new ValidationResult("Display name too long.", new string[] { "DisplayName" })
                    }

                },
                new object[]
                {
                    new Account { AccountID = new Guid("71fffce1-ed11-4efe-92fe-7096c27a4665"), DisplayName = "Doctor     of    Democrat               Statist                 Moses   Connerifficexpialicious                  the   fourteenth", LoginName = "\r\n", Role = UserRole.User, Notes = "",
                        CreatedOn = new DateTime(2016, 5, 31, 16, 16, 23, 404, DateTimeKind.Local), CreatedBy = new Guid("79010060-4bb5-47b8-ad33-7f92623a5f3e"),
                        ModifiedOn = new DateTime(2016, 5, 31, 16, 16, 23, 404, DateTimeKind.Local), ModifiedBy = new Guid("79010060-4bb5-47b8-ad33-7f92623a5f3e") },
                    new ValidationResult[]
                    {
                        new ValidationResult("Login name cannot be empty.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("ef412251-042e-4ef3-8afb-5afef51dc66f"), DisplayName = "Ayaan Mercado", LoginName = ".mercadoamccabernichollsdhebertax", Role = UserRole.User, Notes = "",
                        CreatedOn = new DateTime(2016, 9, 5, 16, 37, 18, 775, DateTimeKind.Local), CreatedBy = new Guid("79010060-4bb5-47b8-ad33-7f92623a5f3e"),
                        ModifiedOn = new DateTime(2016, 9, 5, 16, 37, 18, 775, DateTimeKind.Local), ModifiedBy = new Guid("79010060-4bb5-47b8-ad33-7f92623a5f3e") },
                    new ValidationResult[]
                    {
                        new ValidationResult("Login name too long.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("c60904fe-8bae-4300-80dd-679dd174db42"), DisplayName = "Lucian Firth", LoginName = ".firthllittledortizmpenningtonla", Role = UserRole.User, Notes = "",
                        CreatedOn = new DateTime(2017, 9, 12, 16, 35, 58, 453, DateTimeKind.Local), CreatedBy = new Guid("19bb64d7-3f9f-4ff5-ae29-089b2f74a991"),
                        ModifiedOn = new DateTime(2017, 9, 12, 16, 35, 58, 453, DateTimeKind.Local), ModifiedBy = new Guid("19bb64d7-3f9f-4ff5-ae29-089b2f74a991") },
                    new ValidationResult[]
                    {
                        new ValidationResult("Invalid login name.", new string[] { "LoginName" })
                    }
                },
                new object[]
                {
                    new Account { AccountID = new Guid("7e2cd7b2-32de-420e-8ac2-6ab9a1c735c7"), DisplayName = "Woodrow Lindsay", LoginName = "lindsayw", Role = UserRole.User, Notes = "",
                        CreatedOn = new DateTime(2020, 1, 5, 16, 46, 22, 951, DateTimeKind.Local), CreatedBy = new Guid("b511096c-af4e-47e3-9de0-d3fb7aa7a09f"),
                        ModifiedOn = new DateTime(2020, 11, 18, 16, 14, 4, 945, DateTimeKind.Local), ModifiedBy = new Guid("7e2cd7b2-32de-420e-8ac2-6ab9a1c735c7") },
                    new ValidationResult[0]
                }
            };
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test, Property("Priority", 1)]
        public void AttributesTest()
        {
            Type type = typeof(Account);
            System.Reflection.PropertyInfo property = type.GetProperty("AccountID");
            object[] attributes = property.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true);
            Assert.IsNotNull(attributes);
            Assert.AreNotEqual(0, attributes.Length);
        }

        [TestCaseSource(nameof(GetNormalizeTestCases))]
        [Property("Priority", 1)]
        public void NormalizeTest(Account target, Account expected)
        {
            target.Normalize();
            Assert.AreEqual(expected.AccountID, target.AccountID);
            Assert.AreEqual(expected.DisplayName, target.DisplayName);
            Assert.AreEqual(expected.LoginName, target.LoginName);
            Assert.AreEqual(expected.Role, target.Role);
            Assert.AreEqual(expected.Notes, target.Notes);
            Assert.AreEqual(expected.CreatedOn, target.CreatedOn);
            Assert.AreEqual(DateTimeKind.Local, target.CreatedOn.Kind);
            Assert.AreEqual(expected.CreatedBy, target.CreatedBy);
            Assert.AreEqual(expected.ModifiedOn, target.ModifiedOn);
            Assert.AreEqual(DateTimeKind.Local, target.ModifiedOn.Kind);
            Assert.AreEqual(expected.ModifiedBy, target.ModifiedBy);
        }

        [TestCaseSource(nameof(GetValidateAllTestCases))]
        [Property("Priority", 2)]
        public void ValidateAllTest(Account target, IList<ValidationResult> expected)
        {
            IList<ValidationResult> actual = target.ValidateAll();
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.NotNull(actual[i], "Item " + i.ToString() + " null check");
                Assert.AreEqual(expected[i].ErrorMessage, actual[i].ErrorMessage, "Item " + i.ToString() + " ErrorMessage equality check");
                Assert.NotNull(actual[i].MemberNames, "Item " + i.ToString() + " MemberNames null check");
                using (IEnumerator<string> expEnumerator = expected[i].MemberNames.GetEnumerator())
                {
                    using (IEnumerator<string> resultEnumerator = actual[i].MemberNames.GetEnumerator())
                    {
                        while (expEnumerator.MoveNext())
                        {
                            Assert.IsTrue(resultEnumerator.MoveNext());
                            Assert.NotNull(resultEnumerator.Current);
                            Assert.AreEqual(expEnumerator.Current, resultEnumerator.Current);
                        }
                        Assert.False(resultEnumerator.MoveNext());
                    }
                }
            }
        }
    }
}
