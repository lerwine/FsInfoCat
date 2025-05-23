using FsInfoCat.Model;
using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model;

/// <summary>
/// List item DB entity for a file system.
/// </summary>
/// <seealso cref="FileSystem" />
/// <seealso cref="LocalDbContext.FileSystemListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
// CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public class FileSystemListItem : FileSystemRow, ILocalFileSystemListItem, IEquatable<FileSystemListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    private const string VIEW_NAME = "vFileSystemListing";

    /// <summary>
    /// Gets the primary symbolic name identifier.
    /// </summary>
    /// <value>The primary symbolic name identifier.</value>
    public Guid? PrimarySymbolicNameId { get; set; }

    /// <summary>
    /// Gets the name of the primary symbolic.
    /// </summary>
    /// <value>The name of the primary symbolic.</value>
    [NotNull]
    public string PrimarySymbolicName { get; set; }

    /// <summary>
    /// Gets the symbolic name count.
    /// </summary>
    /// <value>The symbolic name count.</value>
    public long SymbolicNameCount { get; set; }

    /// <summary>
    /// Gets the volume count.
    /// </summary>
    /// <value>The volume count.</value>
    public long VolumeCount { get; set; }

    internal static void OnBuildEntity(EntityTypeBuilder<FileSystemListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(FileSystemListItem other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(IFileSystemListItem other)
    {
        if (other is null) return false;
        if (other is FileSystemListItem listItem) return Equals(listItem);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalFileSystemListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is FileSystemListItem listItem) return Equals(listItem);
        if (obj is IFileSystemListItem other)
        {
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileSystemListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }
        return false;
    }

    protected override string PropertiesToString() => $@"{base.PropertiesToString()},
    SymbolicNameCount={SymbolicNameCount}, VolumeCount={VolumeCount}, PrimarySymbolicName={PrimarySymbolicName}, PrimarySymbolicNameId={PrimarySymbolicNameId}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Asynchronously load related database records.
    /// </summary>
    /// <returns>A tuple that returns the related symbolic names and volumes.</returns>
    public async Task<(SymbolicName[], VolumeListItem[])> LoadRelatedItemsAsync(IActivityProgress progress)
    {
        Guid id = Id;
        using IServiceScope scope = Hosting.ServiceProvider.CreateScope();
        using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
        SymbolicName[] symbolicNames = await dbContext.SymbolicNames.Where(n => n.FileSystemId == id).ToArrayAsync(progress.Token);
        VolumeListItem[] volumes = await dbContext.VolumeListing.Where(v => v.FileSystemId == id).ToArrayAsync(progress.Token);
        SymbolicNameCount = symbolicNames.LongLength;
        VolumeCount = volumes.LongLength;
        return (symbolicNames, volumes);
    }
}
