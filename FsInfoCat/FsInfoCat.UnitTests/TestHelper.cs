using System;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.UnitTests
{
    public static class TestHelper
    {
        private static readonly object _syncRoot = new();
        private static Task _ensureLocalDbTestDataTask;
        internal const string TestCategory_LocalDb = "LocalDb";
        internal const string TestProperty_Description = "Description";
        internal const string TEST_DB_PATH = @"Resources\TestLocal.db";

        internal static Local.FileSystem GetVFatFileSystem(Local.LocalDbContext dbContext)
        {
            Guid id = Guid.Parse("{53a9e9a4-f5f0-4b4c-9f1e-4e3a80a93cfd}");
            return dbContext.FileSystems.ToList().FirstOrDefault(fs => fs.Id.Equals(id));
        }

        //private static bool CheckProperty<T>([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Func<XAttribute, T> ifPresent, out T result)
        //{
        //    XAttribute attribute = element.Attribute(attributeName);
        //    if (attribute is null)
        //    {
        //        result = default;
        //        return false;
        //    }
        //    result = ifPresent(attribute);
        //    return true;
        //}

        //private static T CheckProperty<T>([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Func<XAttribute, T> ifPresent, [NotNull] Func<T> ifNotPresent)
        //{
        //    XAttribute attribute = element.Attribute(attributeName);
        //    if (attribute is null)
        //        return ifNotPresent();
        //    return ifPresent(attribute);
        //}

        //private static T CheckProperty<T>([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Func<XAttribute, T> ifPresent, T ifNotPresent)
        //{
        //    XAttribute attribute = element.Attribute(attributeName);
        //    if (attribute is null)
        //        return ifNotPresent;
        //    return ifPresent(attribute);
        //}

        //private static void EnsureStringProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<string> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => a.Value, out string result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static string EnsureStringProperty([NotNull] XElement element, [NotNull] XName attributeName, string ifNotPresent) => CheckProperty(element, attributeName, a => a.Value, ifNotPresent);

        //private static void EnsureBooleanProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<bool> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => XmlConvert.ToBoolean(a.Value), out bool result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static bool EnsureBooleanProperty([NotNull] XElement element, [NotNull] XName attributeName, bool ifNotPresent) => CheckProperty(element, attributeName, a => XmlConvert.ToBoolean(a.Value), ifNotPresent);

        //private static void EnsureBooleanProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<bool?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToBoolean(a.Value) : null, out bool? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static bool? EnsureBooleanProperty([NotNull] XElement element, [NotNull] XName attributeName, bool? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToBoolean(a.Value) : null, ifNotPresent);

        //private static void EnsureInt32Property([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<int> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => XmlConvert.ToInt32(a.Value), out int result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static int EnsureInt32Property([NotNull] XElement element, [NotNull] XName attributeName, int ifNotPresent) => CheckProperty(element, attributeName, a => XmlConvert.ToInt32(a.Value), ifNotPresent);

        //private static void EnsureInt32Property([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<int?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToInt32(a.Value) : null, out int? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static int? EnsureInt32Property([NotNull] XElement element, [NotNull] XName attributeName, int? ifNotPresent) => CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToInt32(a.Value) : null, ifNotPresent);

        //private static void EnsureDriveTypeProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<DriveType> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (DriveType)Enum.Parse(typeof(DriveType), a.Value), out DriveType result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DriveType EnsureDriveTypeProperty([NotNull] XElement element, [NotNull] XName attributeName, DriveType ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (DriveType)Enum.Parse(typeof(DriveType), a.Value), ifNotPresent);

        //private static void EnsureDriveTypeProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<DriveType?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? (DriveType?)Enum.Parse(typeof(DriveType), a.Value) : null, out DriveType? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DriveType? EnsureDriveTypeProperty([NotNull] XElement element, [NotNull] XName attributeName, DriveType? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? (DriveType?)Enum.Parse(typeof(DriveType), a.Value) : null, ifNotPresent);

        //private static void EnsureGuidProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<Guid?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? Guid.Parse(a.Value) : null, out Guid? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static Guid? EnsureGuidProperty([NotNull] XElement element, [NotNull] XName attributeName, Guid? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? Guid.Parse(a.Value) : null, ifNotPresent);

        //private static void EnsureDateTimeProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<DateTime> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local), out DateTime result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DateTime EnsureDateTimeProperty([NotNull] XElement element, [NotNull] XName attributeName, DateTime ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local), ifNotPresent);

        //private static void EnsureDateTimeProperty([NotNull] XElement element, [NotNull] XName attributeName, [NotNull] Action<DateTime?> ifPresent, Action ifNotPresent = null)
        //{
        //    if (CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local) : null, out DateTime? result))
        //        ifPresent(result);
        //    else
        //        ifNotPresent?.Invoke();
        //}

        //private static DateTime? EnsureDateTimeProperty([NotNull] XElement element, [NotNull] XName attributeName, DateTime? ifNotPresent) =>
        //    CheckProperty(element, attributeName, a => (a.Value.Length > 0) ? XmlConvert.ToDateTime(a.Value, XmlDateTimeSerializationMode.Local) : null, ifNotPresent);

        //private static void EnsureDbEntityProperties([NotNull] XElement element, DbEntity entity)
        //{
        //    EnsureDateTimeProperty(element, nameof(DbEntity.CreatedOn), dateTime => entity.CreatedOn = dateTime);
        //    EnsureDateTimeProperty(element, nameof(DbEntity.ModifiedOn), dateTime => entity.ModifiedOn = dateTime);
        //}

        //private static void EnsureLocalDbEntityProperties([NotNull] XElement element, Local.LocalDbEntity entity)
        //{
        //    entity.UpstreamId = EnsureGuidProperty(element, nameof(Local.LocalDbEntity.UpstreamId), null);
        //    entity.LastSynchronizedOn = EnsureDateTimeProperty(element, nameof(Local.LocalDbEntity.LastSynchronizedOn), null);
        //    EnsureDbEntityProperties(element, entity);
        //}

        //internal static Task CheckLocalDbTestData()
        //{
        //    lock (_syncRoot)
        //    {
        //        if (_ensureLocalDbTestDataTask is null)
        //        {
        //            if (Services.ServiceProvider is null)
        //                throw new InvalidOperationException("Service provider not configured");
        //            _ensureLocalDbTestDataTask = Task.Factory.StartNew(() =>
        //            {
        //                try { EnsureLocalDbTestData(); }
        //                catch (AssertInconclusiveException) { throw; }
        //                catch (Exception exception)
        //                {
        //                    throw new AssertInconclusiveException("Error ensuring test data integrity", exception);
        //                }
        //            });
        //        }
        //    }
        //    return _ensureLocalDbTestDataTask;
        //}

        //private static void EnsureLocalDbTestData()
        //{
        //    using var dbContext = Services.ServiceProvider.GetService<Local.LocalDbContext>();
        //    XDocument document = XDocument.Parse(Properties.Resources.TestData);
        //    Guid[] ids = document.Root.Elements(nameof(Local.FileSystem)).Select(e => EnsureLocalFileSystem(dbContext, e).Id).ToArray();
        //    var fileSystems = dbContext.FileSystems.AsEnumerable().Where(f => !ids.Contains(f.Id)).ToArray();
        //    if (fileSystems.Length > 0)
        //    {
        //        foreach (var f in fileSystems)
        //            dbContext.ForceDeleteFileSystem(f);
        //        dbContext.SaveChanges();
        //    }
        //    ids = document.Root.Elements(nameof(Local.ContentInfo)).Select(e => EnsureLocalContentInfo(dbContext, e).Id).ToArray();
        //    var contentInfos = dbContext.ContentInfos.AsEnumerable().Where(c => !ids.Contains(c.Id)).ToArray();
        //    if (contentInfos.Length > 0)
        //    {
        //        foreach (var c in contentInfos)
        //            dbContext.ForceDeleteContentInfo(c);
        //        dbContext.SaveChanges();
        //    }
        //}

        //private static Local.ContentInfo EnsureLocalContentInfo(Local.LocalDbContext dbContext, XElement element)
        //{
        //    string n = nameof(Local.ContentInfo.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.ContentInfos.Find(id);
        //    throw new NotImplementedException();
        //}

        //private static Local.FileSystem EnsureLocalFileSystem(Local.LocalDbContext dbContext, XElement element)
        //{
        //    string n = nameof(Local.FileSystem.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.FileSystems.Find(id);
        //    if (target is null)
        //    {
        //        StringBuilder sql = new StringBuilder("INSERT INTO ").Append(nameof(Local.LocalDbContext.FileSystems)).Append(" (\"Id\"");
        //        var values = element.Attributes().Where(a => a.Name != n).Select((a, i) =>
        //        {
        //            switch (a.Name.LocalName)
        //            {
        //                case nameof(Local.FileSystem.CaseSensitiveSearch):
        //                case nameof(Local.FileSystem.ReadOnly):
        //                case nameof(Local.FileSystem.IsInactive):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = (object)XmlConvert.ToBoolean(a.Value) };
        //                case nameof(Local.FileSystem.MaxNameLength):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = (object)XmlConvert.ToInt32(a.Value) };
        //                case nameof(Local.FileSystem.DefaultDriveType):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = Enum.ToObject(typeof(DriveType), Enum.Parse(typeof(DriveType), a.Value)) };
        //                case nameof(Local.FileSystem.DisplayName):
        //                case nameof(Local.FileSystem.Notes):
        //                    return new { a.Name.LocalName, Index = i + 1, Value = (object)a.Value };
        //                default:
        //                    throw new NotSupportedException($"Attribute {a.Name} not supported for element {a.Parent.Name}");
        //            }
        //        }).ToArray();
        //        foreach (var v in values)
        //            sql.Append(", \"").Append(v.LocalName).Append("\"");
        //        sql.Append(") VALUES ({0}");
        //        foreach (var v in values)
        //            sql.Append(", {").Append(v.Index).Append('}');
        //        dbContext.Database.ExecuteSqlRaw(sql.Append(')').ToString(), values.Select(v => v.Value));
        //        if ((target = dbContext.Find<Local.FileSystem>(id)) is null)
        //            throw new AssertInconclusiveException($"Failed to insert new {nameof(Local.FileSystem)} with Id {id}");
        //    }
        //    else
        //    {
        //        EnsureStringProperty(element, nameof(Local.FileSystem.DisplayName), text => target.DisplayName = text);
        //        target.CaseSensitiveSearch = EnsureBooleanProperty(element, nameof(Local.FileSystem.CaseSensitiveSearch), false);
        //        target.ReadOnly = EnsureBooleanProperty(element, nameof(Local.FileSystem.ReadOnly), false);
        //        target.MaxNameLength = EnsureInt32Property(element, nameof(Local.FileSystem.MaxNameLength), DbConstants.DbColDefaultValue_MaxNameLength);
        //        target.Notes = EnsureStringProperty(element, nameof(Local.FileSystem.Notes), "");
        //        target.IsInactive = EnsureBooleanProperty(element, nameof(Local.FileSystem.IsInactive), false);
        //        EnsureLocalDbEntityProperties(element, target);
        //        Local.Volume[] localVolumes = element.Elements(nameof(Local.Volume)).Select(e => EnsureLocalVolume(dbContext, target, e)).ToArray();
        //    }
        //    return target;
        //}

        //private static Local.Volume EnsureLocalVolume(Local.LocalDbContext dbContext, Local.FileSystem fileSystem, XElement element)
        //{
        //    string n = nameof(Local.Volume.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.Volumes.Find(id);
        //    throw new NotImplementedException();
        //}

        //private static Local.Subdirectory EnsureLocalSubdirectory(Local.LocalDbContext dbContext, Local.Subdirectory parent, XElement element)
        //{
        //    string n = nameof(Local.Subdirectory.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.Subdirectories.Find(id);
        //    throw new NotImplementedException();
        //}

        //private static Local.DbFile EnsureLocalFile(Local.LocalDbContext dbContext, Local.Subdirectory parent, XElement element)
        //{
        //    string n = nameof(Local.DbFile.Id);
        //    Guid id = Guid.Parse(element.Attribute(n).Value);
        //    var target = dbContext.Files.Find(id);
        //    throw new NotImplementedException();
        //}
    }
}
