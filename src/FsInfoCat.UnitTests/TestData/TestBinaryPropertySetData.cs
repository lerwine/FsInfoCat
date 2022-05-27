using System;
using FsInfoCat.Local.Model;
using FsInfoCat.Model;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestBinaryPropertySetData
    {
        public Guid UpstreamId { get; init; }

        public DateTime LastSynchronizedOn { get; init; }

        public long Length { get; init; }

        public MD5Hash Hash { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        public Guid Id { get; init; }

        public static readonly TestBinaryPropertySetData Item1 = new()
        {
            Id = new("47e72cd7-ebd4-433b-bf64-9d69213ba098"),
            UpstreamId = new("2435a215-17d0-45f9-a14e-6d55198f70f5"),
            CreatedOn = new(637884082875014428L), // 2022-05-17T18:18:07.5014428
            ModifiedOn = new(637884082875014428L), // 2022-05-17T18:18:07.5014428
            LastSynchronizedOn = new(637884082875014428L), // 2022-05-17T18:18:07.5014428
            Length = 725984962L,
            Hash = new(Convert.FromBase64String("JHKSK4I8aid2KAi0+satWg=="))
        };

        public static readonly TestBinaryPropertySetData Item2 = new()
        {
            Id = new("8be3ace4-8582-445b-b440-84e923903032"),
            UpstreamId = new("a4043dbf-73ab-48b5-944b-b9a47f759ebb"),
            CreatedOn = new(637884727403644428L), // 2022-05-18T12:12:20.3644428
            ModifiedOn = new(637884727403644428L), // 2022-05-18T12:12:20.3644428
            LastSynchronizedOn = new(637884727403644428L), // 2022-05-18T12:12:20.3644428
            Length = 205052L,
            Hash = new(Convert.FromBase64String("lJ+PRmeVDep5IoN7Wn6/6g=="))
        };

        internal static BinaryPropertySet CreateClone(BinaryPropertySet source)
        {
            BinaryPropertySet result = new()
            {
                Hash = source.Hash,
                Length = source.Length,
                CreatedOn = source.CreatedOn,
                ModifiedOn = source.ModifiedOn,
                UpstreamId = source.UpstreamId,
                LastSynchronizedOn = source.LastSynchronizedOn,
            };
            if (source.TryGetId(out Guid id)) result.Id = id;
            foreach (DbFile file in source.Files)
                result.Files.Add(file);
            foreach (RedundantSet redundantSet in source.RedundantSets)
                result.RedundantSets.Add(redundantSet);
            return result;
        }
    }
}
