using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlManagerService
    {
        public record Context
        {
            public DirectoryInfo DirectoryInfo { get; init; }

            public Subdirectory Subdirectory { get; init; }

            public FileInfo FileInfo { get; init; }

            public DbFile DbFile { get; init; }

            public Context Parent { get; init; }

            public ushort Depth { get; init; }

            public string Name => FileInfo?.Name ?? DirectoryInfo.Name;

            public string FullName => FileInfo?.FullName ?? DirectoryInfo.FullName;

            public string GetCrawlRelativeName() => Depth switch
            {
                0 => null,
                1 => Name,
                _ => Path.Combine(Parent.GetCrawlRelativeName(), Name)
            };

            public string GetCrawlRelativeParentPath() => Parent?.GetCrawlRelativeName() ?? "";

            public string GetParentPath() => Parent?.FullName ?? "";

            public IEnumerable<Context> GetDirectories(LocalDbContext dbContext)
            {
                if (FileInfo is not null || DbFile is not null)
                    throw new InvalidOperationException("Not a subdirectory context");
                if (DirectoryInfo is null)
                    throw new InvalidOperationException("DirectoryInfo is null");
                ushort nestedDepth = (ushort)(Depth + 1);
                return DirectoryInfo.GetDirectories().Select(d => new Context
                {
                    DirectoryInfo = d,
                    Subdirectory = dbContext.Subdirectories.Add(new()
                    {
                        CreationTime = d.CreationTime,
                        LastWriteTime = d.LastWriteTime,
                        Name = d.Name,
                        Parent = Subdirectory
                    }).Entity,
                    FileInfo = null,
                    DbFile = null,
                    Parent = this,
                    Depth = nestedDepth
                });
            }

            public async Task<List<Context>> GetDirectoriesAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
            {
                if (FileInfo is not null || DbFile is not null)
                    throw new InvalidOperationException("Not a subdirectory context");
                DirectoryInfo[] directoryInfos = (DirectoryInfo ?? throw new InvalidOperationException("DirectoryInfo is null")).GetDirectories();
                Subdirectory[] subdirectories = (await dbContext.Entry(Subdirectory).GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken)).ToArray();

                ushort nestedDepth = (ushort)(Depth + 1);
                return subdirectories.ToMatchedPairs(directoryInfos).Select(t => new Context
                {
                    DirectoryInfo = t.DirectoryInfo,
                    Subdirectory = t.Subdirectory,
                    FileInfo = null,
                    DbFile = null,
                    Parent = this,
                    Depth = nestedDepth
                }).ToList();
            }

            public IEnumerable<Context> GetFiles()
            {
                if (FileInfo is not null || DbFile is not null)
                    throw new InvalidOperationException("Not a subdirectory context");
                return (DirectoryInfo ?? throw new InvalidOperationException("DirectoryInfo is null")).GetFiles().Select(f => this with
                {
                    FileInfo = f,
                    Parent = this
                });
            }

            public async Task<List<Context>> GetFilesAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
            {
                if (FileInfo is not null || DbFile is not null)
                    throw new InvalidOperationException("Not a subdirectory context");
                FileInfo[] fileInfos = (DirectoryInfo ?? throw new InvalidOperationException("DirectoryInfo is null")).GetFiles();
                DbFile[] dbFiles = (await dbContext.Entry(Subdirectory).GetRelatedCollectionAsync(d => d.Files, cancellationToken)).ToArray();
                return dbFiles.ToMatchedPairs(fileInfos).Select(t => this with { DbFile = t.DbFile, FileInfo = t.FileInfo, Parent = this }).ToList();
            }

            public static async Task<Context> CreateAsync(LocalDbContext dbContext, Subdirectory subdirectory, IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken)
            {
                EntityEntry<Subdirectory> entry = dbContext.Entry(subdirectory);
                Subdirectory parent = await entry.GetRelatedReferenceAsync(d => d.Parent, cancellationToken);
                if (parent is null)
                {
                    Volume volume = await entry.GetRelatedReferenceAsync(d => d.Volume, cancellationToken);
                    if (volume is null)
                        throw new InvalidOperationException("Subdirectory has no parent or volume");
                    ILogicalDiskInfo[] logicalDiskInfos = await fileSystemDetailService.GetLogicalDisksAsync(cancellationToken);
                    ILogicalDiskInfo matchingDiskInfo = logicalDiskInfos.FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volume.Identifier));
                    if (matchingDiskInfo is not null)
                        return new Context()
                        {
                            Subdirectory = subdirectory,
                            DirectoryInfo = new DirectoryInfo(matchingDiskInfo.Name),
                            Depth = 0,
                            FileInfo = null,
                            DbFile = null,
                            Parent = null
                        };
                }
                else
                {
                    Context p = await CreateAsync(dbContext, parent, fileSystemDetailService, cancellationToken);
                    if (p is not null)
                        return new Context()
                        {
                            Subdirectory = subdirectory,
                            DirectoryInfo = new DirectoryInfo(Path.Combine(p.FullName, subdirectory.Name)),
                            Depth = 0,
                            FileInfo = null,
                            DbFile = null
                        };
                }
                return null;
            }
        }
    }
}
