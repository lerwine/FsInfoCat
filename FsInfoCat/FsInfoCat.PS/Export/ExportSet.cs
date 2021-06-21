using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public sealed class ExportSet
    {
        internal void SetAllProcessedFlags(bool value)
        {
            foreach (FileSystem fileSystem in FileSystems)
                fileSystem.SetAllProcessedFlags(value);
        }

        [XmlElement(nameof(FileSystem))]
        public Collection<FileSystem> FileSystems { get; set; }

        [Obsolete]
        public Collection<ExtendedProperties> ExtendedProperties { get; set; }

        [XmlElement(nameof(SummaryProperties))]
        public Collection<SummaryProperties> SummaryProperties { get; }

        [XmlElement(nameof(DocumentProperties))]
        public Collection<DocumentProperties> DocumentProperties { get; }

        [XmlElement(nameof(AudioProperties))]
        public Collection<AudioProperties> AudioProperties { get; }

        [XmlElement(nameof(DRMProperties))]
        public Collection<DRMProperties> DRMProperties { get; }

        [XmlElement(nameof(GPSProperties))]
        public Collection<GPSProperties> GPSProperties { get; }

        [XmlElement(nameof(ImageProperties))]
        public Collection<ImageProperties> ImageProperties { get; }

        [XmlElement(nameof(MediaProperties))]
        public Collection<MediaProperties> MediaProperties { get; }

        [XmlElement(nameof(MusicProperties))]
        public Collection<MusicProperties> MusicProperties { get; }

        [XmlElement(nameof(PhotoProperties))]
        public Collection<PhotoProperties> PhotoProperties { get; }

        [XmlElement(nameof(RecordedTVProperties))]
        public Collection<RecordedTVProperties> RecordedTVProperties { get; }

        [XmlElement(nameof(VideoProperties))]
        public Collection<VideoProperties> VideoProperties { get; }

        [XmlElement(nameof(Export.BinaryProperties))]
        public Collection<BinaryProperties> BinaryProperties { get; set; }

        private static XmlWriterSettings GetDefaultXmlWriterSettings() => new()
        {
            Indent = true,
            Encoding = new UTF8Encoding(false, true)
        };

        public static ExportSet Load(string path)
        {
            using XmlReader xmlReader = XmlReader.Create(path);
            return Load(xmlReader);
        }

        public static ExportSet Load(TextReader input)
        {
            using XmlReader xmlReader = XmlReader.Create(input);
            return Load(xmlReader);
        }

        public static ExportSet LoadXml(string xml)
        {
            using StringReader reader = new(xml);
            return Load(reader);
        }

        public static ExportSet Load(Stream input)
        {
            using XmlReader xmlReader = XmlReader.Create(input);
            return Load(xmlReader);
        }

        public static ExportSet Load(XmlReader xmlReader)
        {
            XmlSerializer serializer = new(typeof(ExportSet));
            return (ExportSet)serializer.Deserialize(xmlReader);
        }

        public void WriteTo(string path, XmlWriterSettings settings = null)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(path, settings ?? GetDefaultXmlWriterSettings());
            WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        public void WriteTo(Stream output, XmlWriterSettings settings = null)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(output, settings ?? GetDefaultXmlWriterSettings());
            WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        public void WriteTo(TextWriter output, XmlWriterSettings settings = null)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(output, settings ?? GetDefaultXmlWriterSettings());
            WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        public string ToXmlString(XmlWriterSettings settings = null)
        {
            StringWriter writer = new();
            WriteTo(writer, settings);
            return writer.ToString();
        }

        public void WriteTo(XmlWriter xmlWriter)
        {
            XmlSerializer serializer = new(typeof(ExportSet));
            serializer.Serialize(xmlWriter, this);
        }

        public abstract class FileSystemBase : EntityExportElement
        {
            private string _displayName = "";

            [XmlAttribute]
            public string DisplayName { get => _displayName; set => _displayName = value.TrimmedNotNull(); }

            [XmlElement(nameof(SymbolicName))]
            public Collection<SymbolicName> SymbolicNames { get; set; }

            [XmlIgnore]
            public ExportSet ExportSet { get; private set; }
        }

        public abstract class ExtendedPropertiesBase : EntityExportElement
        {
            [XmlIgnore]
            public ExportSet ExportSet { get; private set; }
        }

        public abstract class BinaryPropertiesBase : EntityExportElement
        {
            [XmlAttribute]
            public long Length { get; set; }

            [XmlAttribute]
            public MD5Hash? Hash { get; set; }

            [XmlIgnore]
            public ExportSet ExportSet { get; private set; }
        }

        internal File Import(FileInfo fileInfo)
        {
            Subdirectory parent = Import(fileInfo.Directory);
            string n = fileInfo.Name;
            File file = parent.Files.FirstOrDefault(f => n.Equals(f.Name, StringComparison.InvariantCultureIgnoreCase));
            BinaryProperties binaryProperties;
            SummaryProperties summaryProperties;
            DocumentProperties documentProperties;
            AudioProperties audioProperties;
            DRMProperties drmProperties;
            GPSProperties gpsProperties;
            ImageProperties imageProperties;
            MediaProperties mediaProperties;
            MusicProperties musicProperties;
            PhotoProperties photoProperties;
            RecordedTVProperties recordedTVProperties;
            VideoProperties videoProperties;
            if (file is not null)
            {
                if (file.IsProcessed)
                    return file;
                binaryProperties = file.GetBinaryProperties();
                summaryProperties = file.GetSummaryProperties();
                documentProperties = file.GetDocumentProperties();
                audioProperties = file.GetAudioProperties();
                drmProperties = file.GetDRMProperties();
                gpsProperties = file.GetGPSProperties();
                imageProperties = file.GetImageProperties();
                mediaProperties = file.GetMediaProperties();
                musicProperties = file.GetMusicProperties();
                photoProperties = file.GetPhotoProperties();
                recordedTVProperties = file.GetRecordedTVProperties();
                videoProperties = file.GetVideoProperties();
                SummaryProperties sp = Export.SummaryProperties.Create(fileInfo);
                DocumentProperties dp = Export.DocumentProperties.Create(fileInfo);
                AudioProperties ap = Export.AudioProperties.Create(fileInfo);
                DRMProperties drm = Export.DRMProperties.Create(fileInfo);
                GPSProperties gp = Export.GPSProperties.Create(fileInfo);
                ImageProperties ip = Export.ImageProperties.Create(fileInfo);
                MediaProperties mdp = Export.MediaProperties.Create(fileInfo);
                MusicProperties msp = Export.MusicProperties.Create(fileInfo);
                PhotoProperties pp = Export.PhotoProperties.Create(fileInfo);
                RecordedTVProperties rp = Export.RecordedTVProperties.Create(fileInfo);
                VideoProperties vp = Export.VideoProperties.Create(fileInfo);
                if (!summaryProperties.Equals(sp))
                {
                    if ((summaryProperties = SummaryProperties.FirstOrDefault(e => e.Equals(sp))) is null && sp is not null)
                    {
                        SummaryProperties.Add(sp);
                        summaryProperties = sp;
                    }
                }
                if (!documentProperties.Equals(dp))
                {
                    if ((documentProperties = DocumentProperties.FirstOrDefault(e => e.Equals(dp))) is null && dp is not null)
                    {
                        DocumentProperties.Add(dp);
                        documentProperties = dp;
                    }
                }
                if (!audioProperties.Equals(ap))
                {
                    if ((audioProperties = AudioProperties.FirstOrDefault(e => e.Equals(ap))) is null && ap is not null)
                    {
                        AudioProperties.Add(ap);
                        audioProperties = ap;
                    }
                }
                if (!drmProperties.Equals(drm))
                {
                    if ((drmProperties = DRMProperties.FirstOrDefault(e => e.Equals(drm))) is null && drm is not null)
                    {
                        DRMProperties.Add(drm);
                        drmProperties = drm;
                    }
                }
                if (!gpsProperties.Equals(gp))
                {
                    if ((gpsProperties = GPSProperties.FirstOrDefault(e => e.Equals(gp))) is null && gp is not null)
                    {
                        GPSProperties.Add(gp);
                        gpsProperties = gp;
                    }
                }
                if (!imageProperties.Equals(ip))
                {
                    if ((imageProperties = ImageProperties.FirstOrDefault(e => e.Equals(ip))) is null && ip is not null)
                    {
                        ImageProperties.Add(ip);
                        imageProperties = ip;
                    }
                }
                if (!mediaProperties.Equals(mdp))
                {
                    if ((mediaProperties = MediaProperties.FirstOrDefault(e => e.Equals(mdp))) is null && mdp is not null)
                    {
                        MediaProperties.Add(mdp);
                        mediaProperties = mdp;
                    }
                }
                if (!musicProperties.Equals(msp))
                {
                    if ((musicProperties = MusicProperties.FirstOrDefault(e => e.Equals(msp))) is null && msp is not null)
                    {
                        MusicProperties.Add(msp);
                        musicProperties = msp;
                    }
                }
                if (!photoProperties.Equals(pp))
                {
                    if ((photoProperties = PhotoProperties.FirstOrDefault(e => e.Equals(pp))) is null && pp is not null)
                    {
                        PhotoProperties.Add(pp);
                        photoProperties = pp;
                    }
                }
                if (!recordedTVProperties.Equals(rp))
                {
                    if ((recordedTVProperties = RecordedTVProperties.FirstOrDefault(e => e.Equals(rp))) is null && rp is not null)
                    {
                        RecordedTVProperties.Add(rp);
                        recordedTVProperties = rp;
                    }
                }
                if (!videoProperties.Equals(vp))
                {
                    if ((videoProperties = VideoProperties.FirstOrDefault(e => e.Equals(vp))) is null && vp is not null)
                    {
                        VideoProperties.Add(vp);
                        videoProperties = vp;
                    }
                }
            }
            else
            {
                long length = fileInfo.Length;
                binaryProperties = BinaryProperties.FirstOrDefault(c => !c.Hash.HasValue && c.Length == length);
                if (binaryProperties is null)
                {
                    (binaryProperties = new()
                    {
                        CreatedOn = DateTime.Now,
                        Length = length
                    }).ModifiedOn = binaryProperties.CreatedOn;
                    BinaryProperties.Add(binaryProperties);
                }
                //SummaryProperties summaryProperties;
                SummaryProperties sp = Export.SummaryProperties.Create(fileInfo);
                if ((summaryProperties = SummaryProperties.FirstOrDefault(e => e.Equals(sp))) is null && sp is not null)
                {
                    SummaryProperties.Add(sp);
                    summaryProperties = sp;
                }
                DocumentProperties dp = Export.DocumentProperties.Create(fileInfo);
                if ((documentProperties = DocumentProperties.FirstOrDefault(e => e.Equals(dp))) is null && dp is not null)
                {
                    DocumentProperties.Add(dp);
                    documentProperties = dp;
                }
                AudioProperties ap = Export.AudioProperties.Create(fileInfo);
                if ((audioProperties = AudioProperties.FirstOrDefault(e => e.Equals(ap))) is null && ap is not null)
                {
                    AudioProperties.Add(ap);
                    audioProperties = ap;
                }
                DRMProperties drm = Export.DRMProperties.Create(fileInfo);
                if ((drmProperties = DRMProperties.FirstOrDefault(e => e.Equals(drm))) is null && drm is not null)
                {
                    DRMProperties.Add(drm);
                    drmProperties = drm;
                }
                GPSProperties gp = Export.GPSProperties.Create(fileInfo);
                if ((gpsProperties = GPSProperties.FirstOrDefault(e => e.Equals(gp))) is null && gp is not null)
                {
                    GPSProperties.Add(gp);
                    gpsProperties = gp;
                }
                ImageProperties ip = Export.ImageProperties.Create(fileInfo);
                if ((imageProperties = ImageProperties.FirstOrDefault(e => e.Equals(ip))) is null && ip is not null)
                {
                    ImageProperties.Add(ip);
                    imageProperties = ip;
                }
                MediaProperties mdp = Export.MediaProperties.Create(fileInfo);
                if ((mediaProperties = MediaProperties.FirstOrDefault(e => e.Equals(mdp))) is null && mdp is not null)
                {
                    MediaProperties.Add(mdp);
                    mediaProperties = mdp;
                }
                MusicProperties msp = Export.MusicProperties.Create(fileInfo);
                if ((musicProperties = MusicProperties.FirstOrDefault(e => e.Equals(msp))) is null && msp is not null)
                {
                    MusicProperties.Add(msp);
                    musicProperties = msp;
                }
                PhotoProperties pp = Export.PhotoProperties.Create(fileInfo);
                if ((photoProperties = PhotoProperties.FirstOrDefault(e => e.Equals(pp))) is null && pp is not null)
                {
                        PhotoProperties.Add(pp);
                    photoProperties = pp;
                }
                RecordedTVProperties rp = Export.RecordedTVProperties.Create(fileInfo);
                if ((recordedTVProperties = RecordedTVProperties.FirstOrDefault(e => e.Equals(rp))) is null && rp is not null)
                {
                    RecordedTVProperties.Add(rp);
                    recordedTVProperties = rp;
                }
                VideoProperties vp = Export.VideoProperties.Create(fileInfo);
                if ((videoProperties = VideoProperties.FirstOrDefault(e => e.Equals(vp))) is null && vp is not null)
                {
                    VideoProperties.Add(vp);
                    videoProperties = vp;
                }
                file = new File
                {
                    Name = n,
                    CreatedOn = DateTime.Now,
                    CreationTime = fileInfo.CreationTime,
                    LastWriteTime = fileInfo.LastWriteTime,
                    //ExtendedPropertyId = extendedProperties?.Id,
                    BinaryPropertiesId = binaryProperties.Id,
                    IsProcessed = true
                };
                file.LastAccessed = file.ModifiedOn = file.CreatedOn;
                parent.Files.Add(file);
            }
            return file;
        }

        internal Subdirectory Import(DirectoryInfo directoryInfo)
        {
            throw new NotImplementedException();
        }
    }
}
