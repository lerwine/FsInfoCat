using FsInfoCat.Collections;
using FsInfoCat.Local.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class LocalSummaryPropertySetUnitTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void OnTestInitialize()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            TestHelper.UndoChanges(dbContext);
        }

        [TestMethod("SummaryPropertySet Constructor Tests"), Ignore]
        public void SummaryPropertySetConstructorTestMethod()
        {
            DateTime @then = DateTime.Now;
            SummaryPropertySet target = new();
            Assert.IsTrue(target.CreatedOn <= DateTime.Now);
            Assert.IsTrue(target.CreatedOn >= @then);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.AreEqual(string.Empty, target.ApplicationName);
            Assert.IsNull(target.Author);
            Assert.AreEqual(string.Empty, target.Comment);
            Assert.AreEqual(string.Empty, target.Company);
            Assert.AreEqual(string.Empty, target.ContentType);
            Assert.AreEqual(string.Empty, target.Copyright);
            Assert.AreEqual(string.Empty, target.FileDescription);
            Assert.AreEqual(string.Empty, target.FileVersion);
            Assert.IsNull(target.ItemAuthors);
            Assert.AreEqual(string.Empty, target.ItemType);
            Assert.AreEqual(string.Empty, target.ItemTypeText);
            Assert.IsNull(target.Keywords);
            Assert.IsNull(target.Kind);
            Assert.AreEqual(string.Empty, target.MIMEType);
            Assert.AreEqual(string.Empty, target.ParentalRating);
            Assert.AreEqual(string.Empty, target.ParentalRatingReason);
            Assert.AreEqual(string.Empty, target.ParentalRatingsOrganization);
            Assert.AreEqual(string.Empty, target.ProductName);
            Assert.IsNull(target.Rating);
            Assert.IsNull(target.Sensitivity);
            Assert.AreEqual(string.Empty, target.SensitivityText);
            Assert.IsNull(target.SimpleRating);
            Assert.AreEqual(string.Empty, target.Subject);
            Assert.AreEqual(string.Empty, target.Title);
            Assert.AreEqual(string.Empty, target.Trademarks);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
        }

        [TestMethod("SummaryPropertySet Add/Remove Tests"), Ignore]
        public void SummaryPropertySetAddRemoveTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            SummaryPropertySet target = new();
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.Entry(target);
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
            entityEntry = dbContext.SummaryPropertySets.Add(target);
            Assert.AreEqual(EntityState.Added, entityEntry.State);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            DateTime now = DateTime.Now;
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            entityEntry.Reload();
            // DEFERRED: Validate default values
            Assert.AreEqual(string.Empty, target.ApplicationName);
            Assert.IsNull(target.Author);
            Assert.AreEqual(string.Empty, target.Comment);
            Assert.IsNull(target.Keywords);
            Assert.AreEqual(string.Empty, target.Subject);
            Assert.AreEqual(string.Empty, target.Title);
            Assert.AreEqual(string.Empty, target.FileDescription);
            Assert.AreEqual(string.Empty, target.FileVersion);
            Assert.AreEqual(string.Empty, target.Company);
            Assert.AreEqual(string.Empty, target.ContentType);
            Assert.AreEqual(string.Empty, target.Copyright);
            Assert.AreEqual(string.Empty, target.ParentalRating);
            Assert.IsNull(target.Rating);
            Assert.IsNull(target.ItemAuthors);
            Assert.AreEqual(string.Empty, target.ItemType);
            Assert.AreEqual(string.Empty, target.ItemTypeText);
            Assert.IsNull(target.Kind);
            Assert.AreEqual(string.Empty, target.MIMEType);
            Assert.AreEqual(string.Empty, target.ParentalRatingReason);
            Assert.AreEqual(string.Empty, target.ParentalRatingsOrganization);
            Assert.IsNull(target.Sensitivity);
            Assert.AreEqual(string.Empty, target.SensitivityText);
            Assert.IsNull(target.SimpleRating);
            Assert.AreEqual(string.Empty, target.Trademarks);
            Assert.AreEqual(string.Empty, target.ProductName);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.IsNull(target.UpstreamId);
            Assert.IsTrue(target.CreatedOn >= now);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            entityEntry = dbContext.Remove(target);
            Assert.AreEqual(EntityState.Deleted, entityEntry.State);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Detached, entityEntry.State);
        }

        [TestMethod("Guid Id"), Ignore]
        public void SummaryPropertySetIdTestMethod()
        {
            SummaryPropertySet target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsExactly<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("SummaryPropertySet ApplicationName Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ApplicationName: NVARCHAR(1024)")]
        public void SummaryPropertySetApplicationNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ApplicationName = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ApplicationName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ApplicationName);

            expected = default; // DEFERRED: Set valid value
            target.ApplicationName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ApplicationName);

            expected = default; // DEFERRED: Set invalid value
            target.ApplicationName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ApplicationName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ApplicationName);
        }

        [TestMethod("SummaryPropertySet Author Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Author: TEXT")]
        public void SummaryPropertySetAuthorTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            MultiStringValue expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Author = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Author), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Author);

            expected = default; // DEFERRED: Set valid value
            target.Author = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Author);

            expected = default; // DEFERRED: Set invalid value
            target.Author = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Author), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Author);
        }

        [TestMethod("SummaryPropertySet Comment Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Comment: TEXT")]
        public void SummaryPropertySetCommentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Comment = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Comment), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Comment);

            expected = default; // DEFERRED: Set valid value
            target.Comment = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Comment);

            expected = default; // DEFERRED: Set invalid value
            target.Comment = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Comment), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Comment);
        }

        [TestMethod("SummaryPropertySet FileDescription Validation Tests"), Ignore]
        [Description("SummaryPropertySet.FileDescription: TEXT")]
        public void SummaryPropertySetFileDescriptionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { FileDescription = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.FileDescription), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.FileDescription);

            expected = default; // DEFERRED: Set valid value
            target.FileDescription = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.FileDescription);

            expected = default; // DEFERRED: Set invalid value
            target.FileDescription = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.FileDescription), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.FileDescription);
        }

        [TestMethod("SummaryPropertySet FileVersion Validation Tests"), Ignore]
        [Description("SummaryPropertySet.FileVersion: TEXT")]
        public void SummaryPropertySetFileVersionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { FileVersion = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.FileVersion), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.FileVersion);

            expected = default; // DEFERRED: Set valid value
            target.FileVersion = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.FileVersion);

            expected = default; // DEFERRED: Set invalid value
            target.FileVersion = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.FileVersion), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.FileVersion);
        }

        [TestMethod("SummaryPropertySet Keywords Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Keywords: TEXT")]
        public void SummaryPropertySetKeywordsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            MultiStringValue expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Keywords = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Keywords), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Keywords);

            expected = default; // DEFERRED: Set valid value
            target.Keywords = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Keywords);

            expected = default; // DEFERRED: Set invalid value
            target.Keywords = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Keywords), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Keywords);
        }

        [TestMethod("SummaryPropertySet Subject Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Subject: NVARCHAR(1024)")]
        public void SummaryPropertySetSubjectTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Subject = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Subject), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Subject);

            expected = default; // DEFERRED: Set valid value
            target.Subject = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Subject);

            expected = default; // DEFERRED: Set invalid value
            target.Subject = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Subject), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Subject);
        }

        [TestMethod("SummaryPropertySet Title Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Title: NVARCHAR(1024)")]
        public void SummaryPropertySetTitleTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Title = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Title), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Title);

            expected = default; // DEFERRED: Set valid value
            target.Title = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Title);

            expected = default; // DEFERRED: Set invalid value
            target.Title = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Title), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Title);
        }

        [TestMethod("SummaryPropertySet Company Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Company: NVARCHAR(1024)")]
        public void SummaryPropertySetCompanyTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Company = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Company), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Company);

            expected = default; // DEFERRED: Set valid value
            target.Company = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Company);

            expected = default; // DEFERRED: Set invalid value
            target.Company = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Company), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Company);
        }

        [TestMethod("SummaryPropertySet ContentType Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ContentType: NVARCHAR(1024)")]
        public void SummaryPropertySetContentTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ContentType = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ContentType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ContentType);

            expected = default; // DEFERRED: Set valid value
            target.ContentType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ContentType);

            expected = default; // DEFERRED: Set invalid value
            target.ContentType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ContentType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ContentType);
        }

        [TestMethod("SummaryPropertySet Copyright Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Copyright: NVARCHAR(1024)")]
        public void SummaryPropertySetCopyrightTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Copyright = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Copyright), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Copyright);

            expected = default; // DEFERRED: Set valid value
            target.Copyright = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Copyright);

            expected = default; // DEFERRED: Set invalid value
            target.Copyright = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Copyright), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Copyright);
        }

        [TestMethod("SummaryPropertySet ParentalRating Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ParentalRating: NVARCHAR(32)")]
        public void SummaryPropertySetParentalRatingTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ParentalRating = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ParentalRating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ParentalRating);

            expected = default; // DEFERRED: Set valid value
            target.ParentalRating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ParentalRating);

            expected = default; // DEFERRED: Set invalid value
            target.ParentalRating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ParentalRating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ParentalRating);
        }

        [TestMethod("SummaryPropertySet Rating Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "SummaryPropertySet.Rating: TINYINT \"Rating\" IS NULL OR (\"Rating\">0 AND \"Rating\"<100)")]
        public void SummaryPropertySetRatingTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Rating = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Rating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Rating);

            expected = default; // DEFERRED: Set valid value
            target.Rating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Rating);

            expected = default; // DEFERRED: Set invalid value
            target.Rating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Rating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Rating);
        }

        [TestMethod("SummaryPropertySet ItemAuthors Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ItemAuthors: TEXT")]
        public void SummaryPropertySetItemAuthorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            MultiStringValue expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ItemAuthors = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ItemAuthors), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ItemAuthors);

            expected = default; // DEFERRED: Set valid value
            target.ItemAuthors = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ItemAuthors);

            expected = default; // DEFERRED: Set invalid value
            target.ItemAuthors = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ItemAuthors), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ItemAuthors);
        }

        [TestMethod("SummaryPropertySet ItemType Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ItemType: NVARCHAR(32)")]
        public void SummaryPropertySetItemTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ItemType = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ItemType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ItemType);

            expected = default; // DEFERRED: Set valid value
            target.ItemType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ItemType);

            expected = default; // DEFERRED: Set invalid value
            target.ItemType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ItemType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ItemType);
        }

        [TestMethod("SummaryPropertySet ItemTypeText Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ItemTypeText: NVARCHAR(64)")]
        public void SummaryPropertySetItemTypeTextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ItemTypeText = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ItemTypeText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ItemTypeText);

            expected = default; // DEFERRED: Set valid value
            target.ItemTypeText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ItemTypeText);

            expected = default; // DEFERRED: Set invalid value
            target.ItemTypeText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ItemTypeText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ItemTypeText);
        }

        [TestMethod("SummaryPropertySet Kind Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Kind: NVARCHAR(128)")]
        public void SummaryPropertySetKindTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            MultiStringValue expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Kind = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Kind), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Kind);

            expected = default; // DEFERRED: Set valid value
            target.Kind = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Kind);

            expected = default; // DEFERRED: Set invalid value
            target.Kind = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Kind), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Kind);
        }

        [TestMethod("SummaryPropertySet MIMEType Validation Tests"), Ignore]
        [Description("SummaryPropertySet.MIMEType: NVARCHAR(1024)")]
        public void SummaryPropertySetMIMETypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { MIMEType = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.MIMEType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.MIMEType);

            expected = default; // DEFERRED: Set valid value
            target.MIMEType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.MIMEType);

            expected = default; // DEFERRED: Set invalid value
            target.MIMEType = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.MIMEType), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.MIMEType);
        }

        [TestMethod("SummaryPropertySet ParentalRatingReason Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ParentalRatingReason: NVARCHAR(1024)")]
        public void SummaryPropertySetParentalRatingReasonTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ParentalRatingReason = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ParentalRatingReason), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ParentalRatingReason);

            expected = default; // DEFERRED: Set valid value
            target.ParentalRatingReason = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ParentalRatingReason);

            expected = default; // DEFERRED: Set invalid value
            target.ParentalRatingReason = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ParentalRatingReason), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ParentalRatingReason);
        }

        [TestMethod("SummaryPropertySet ParentalRatingsOrganization Validation Tests"), Ignore]
        [Description("SummaryPropertySet.ParentalRatingsOrganization: NVARCHAR(1024)")]
        public void SummaryPropertySetParentalRatingsOrganizationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ParentalRatingsOrganization = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ParentalRatingsOrganization), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ParentalRatingsOrganization);

            expected = default; // DEFERRED: Set valid value
            target.ParentalRatingsOrganization = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ParentalRatingsOrganization);

            expected = default; // DEFERRED: Set invalid value
            target.ParentalRatingsOrganization = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ParentalRatingsOrganization), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ParentalRatingsOrganization);
        }

        [TestMethod("SummaryPropertySet Sensitivity Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "SummaryPropertySet.Sensitivity: INT \"Sensitivity\" IS NULL OR (\"Sensitivity\">=0 AND \"Sensitivity\"<65536)")]
        public void SummaryPropertySetSensitivityTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            ushort? expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Sensitivity = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Sensitivity), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Sensitivity);

            expected = default; // DEFERRED: Set valid value
            target.Sensitivity = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Sensitivity);

            expected = default; // DEFERRED: Set invalid value
            target.Sensitivity = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Sensitivity), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Sensitivity);
        }

        [TestMethod("SummaryPropertySet SensitivityText Validation Tests"), Ignore]
        [Description("SummaryPropertySet.SensitivityText: NVARCHAR(1024)")]
        public void SummaryPropertySetSensitivityTextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { SensitivityText = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.SensitivityText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.SensitivityText);

            expected = default; // DEFERRED: Set valid value
            target.SensitivityText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.SensitivityText);

            expected = default; // DEFERRED: Set invalid value
            target.SensitivityText = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.SensitivityText), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.SensitivityText);
        }

        [TestMethod("SummaryPropertySet SimpleRating Validation Tests"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description, "SummaryPropertySet.SimpleRating: TinyInt \"SimpleRating\" IS NULL OR (\"SimpleRating\">=0 AND \"SimpleRating\"<6)")]
        public void SummaryPropertySetSimpleRatingTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            uint? expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { SimpleRating = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.SimpleRating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.SimpleRating);

            expected = default; // DEFERRED: Set valid value
            target.SimpleRating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.SimpleRating);

            expected = default; // DEFERRED: Set invalid value
            target.SimpleRating = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.SimpleRating), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.SimpleRating);
        }

        [TestMethod("SummaryPropertySet Trademarks Validation Tests"), Ignore]
        [Description("SummaryPropertySet.Trademarks: NVARCHAR(1024)")]
        public void SummaryPropertySetTrademarksTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { Trademarks = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Trademarks), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.Trademarks);

            expected = default; // DEFERRED: Set valid value
            target.Trademarks = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.Trademarks);

            expected = default; // DEFERRED: Set invalid value
            target.Trademarks = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.Trademarks), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.Trademarks);
        }

        [TestMethod("SoftwarePropertySet ProductName Validation Tests"), Ignore]
        [Description("SoftwarePropertySet.ProductName: NVARCHAR(256)")]
        public void SoftwarePropertySetProductNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            string expected = default; // DEFERRED: Set invalid value
            SummaryPropertySet target = new() { ProductName = expected };
            EntityEntry<SummaryPropertySet> entityEntry = dbContext.SummaryPropertySets.Add(target);
            Collection<ValidationResult> results = new();
            bool success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ProductName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(expected, target.ProductName);

            expected = default; // DEFERRED: Set valid value
            target.ProductName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsTrue(success);
            Assert.AreEqual(0, results.Count);
            dbContext.SaveChanges();
            Assert.AreEqual(EntityState.Unchanged, entityEntry.State);
            entityEntry.Reload();
            Assert.AreEqual(expected, target.ProductName);

            expected = default; // DEFERRED: Set invalid value
            target.ProductName = expected;
            results = new();
            success = Validator.TryValidateObject(target, new ValidationContext(target), results, true);
            Assert.IsFalse(success);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(1, results[0].MemberNames.Count());
            Assert.AreEqual(nameof(SummaryPropertySet.ProductName), results[0].MemberNames.First());
            Assert.AreEqual(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, results[0].ErrorMessage);
            entityEntry = dbContext.SummaryPropertySets.Update(target);
            Assert.ThrowsExactly<ValidationException>(() => dbContext.SaveChanges());
            Assert.AreEqual(EntityState.Modified, entityEntry.State);
            Assert.AreEqual(expected, target.ProductName);
        }
    }
}
