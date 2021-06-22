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

        [XmlElement(nameof(SummaryPropertySet))]
        public Collection<SummaryPropertySet> SummaryPropertySets { get; }

        [XmlElement(nameof(DocumentPropertySet))]
        public Collection<DocumentPropertySet> DocumentPropertySets { get; }

        [XmlElement(nameof(AudioPropertySet))]
        public Collection<AudioPropertySet> AudioPropertySets { get; }

        [XmlElement(nameof(DRMPropertySet))]
        public Collection<DRMPropertySet> DRMPropertySets { get; }

        [XmlElement(nameof(GPSPropertySet))]
        public Collection<GPSPropertySet> GPSPropertySets { get; }

        [XmlElement(nameof(ImagePropertySet))]
        public Collection<ImagePropertySet> ImagePropertySets { get; }

        [XmlElement(nameof(MediaPropertySet))]
        public Collection<MediaPropertySet> MediaPropertySets { get; }

        [XmlElement(nameof(MusicPropertySet))]
        public Collection<MusicPropertySet> MusicPropertySets { get; }

        [XmlElement(nameof(PhotoPropertySet))]
        public Collection<PhotoPropertySet> PhotoPropertySets { get; }

        [XmlElement(nameof(RecordedTVPropertySet))]
        public Collection<RecordedTVPropertySet> RecordedTVPropertySets { get; }

        [XmlElement(nameof(VideoPropertySet))]
        public Collection<VideoPropertySet> VideoPropertySets { get; }

        [XmlElement(nameof(BinaryPropertySet))]
        public Collection<BinaryPropertySet> BinaryPropertySets { get; set; }

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

        public abstract class ExtendedPropertySetBase : EntityExportElement
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
            BinaryPropertySet binaryProperties;
            SummaryPropertySet summaryProperties;
            DocumentPropertySet documentProperties;
            AudioPropertySet audioProperties;
            DRMPropertySet drmProperties;
            GPSPropertySet gpsProperties;
            ImagePropertySet imageProperties;
            MediaPropertySet mediaProperties;
            MusicPropertySet musicProperties;
            PhotoPropertySet photoProperties;
            RecordedTVPropertySet recordedTVProperties;
            VideoPropertySet videoProperties;
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
                SummaryPropertySet sp = Export.SummaryPropertySet.Create(fileInfo);
                DocumentPropertySet dp = Export.DocumentPropertySet.Create(fileInfo);
                AudioPropertySet ap = Export.AudioPropertySet.Create(fileInfo);
                DRMPropertySet drm = Export.DRMPropertySet.Create(fileInfo);
                GPSPropertySet gp = Export.GPSPropertySet.Create(fileInfo);
                ImagePropertySet ip = Export.ImagePropertySet.Create(fileInfo);
                MediaPropertySet mdp = Export.MediaPropertySet.Create(fileInfo);
                MusicPropertySet msp = Export.MusicPropertySet.Create(fileInfo);
                PhotoPropertySet pp = Export.PhotoPropertySet.Create(fileInfo);
                RecordedTVPropertySet rp = Export.RecordedTVPropertySet.Create(fileInfo);
                VideoPropertySet vp = Export.VideoPropertySet.Create(fileInfo);
                if (!summaryProperties.Equals(sp))
                {
                    if ((summaryProperties = SummaryPropertySets.FirstOrDefault(e => e.Equals(sp))) is null && sp is not null)
                    {
                        SummaryPropertySets.Add(sp);
                        summaryProperties = sp;
                    }
                }
                if (!documentProperties.Equals(dp))
                {
                    if ((documentProperties = DocumentPropertySets.FirstOrDefault(e => e.Equals(dp))) is null && dp is not null)
                    {
                        DocumentPropertySets.Add(dp);
                        documentProperties = dp;
                    }
                }
                if (!audioProperties.Equals(ap))
                {
                    if ((audioProperties = AudioPropertySets.FirstOrDefault(e => e.Equals(ap))) is null && ap is not null)
                    {
                        AudioPropertySets.Add(ap);
                        audioProperties = ap;
                    }
                }
                if (!drmProperties.Equals(drm))
                {
                    if ((drmProperties = DRMPropertySets.FirstOrDefault(e => e.Equals(drm))) is null && drm is not null)
                    {
                        DRMPropertySets.Add(drm);
                        drmProperties = drm;
                    }
                }
                if (!gpsProperties.Equals(gp))
                {
                    if ((gpsProperties = GPSPropertySets.FirstOrDefault(e => e.Equals(gp))) is null && gp is not null)
                    {
                        GPSPropertySets.Add(gp);
                        gpsProperties = gp;
                    }
                }
                if (!imageProperties.Equals(ip))
                {
                    if ((imageProperties = ImagePropertySets.FirstOrDefault(e => e.Equals(ip))) is null && ip is not null)
                    {
                        ImagePropertySets.Add(ip);
                        imageProperties = ip;
                    }
                }
                if (!mediaProperties.Equals(mdp))
                {
                    if ((mediaProperties = MediaPropertySets.FirstOrDefault(e => e.Equals(mdp))) is null && mdp is not null)
                    {
                        MediaPropertySets.Add(mdp);
                        mediaProperties = mdp;
                    }
                }
                if (!musicProperties.Equals(msp))
                {
                    if ((musicProperties = MusicPropertySets.FirstOrDefault(e => e.Equals(msp))) is null && msp is not null)
                    {
                        MusicPropertySets.Add(msp);
                        musicProperties = msp;
                    }
                }
                if (!photoProperties.Equals(pp))
                {
                    if ((photoProperties = PhotoPropertySets.FirstOrDefault(e => e.Equals(pp))) is null && pp is not null)
                    {
                        PhotoPropertySets.Add(pp);
                        photoProperties = pp;
                    }
                }
                if (!recordedTVProperties.Equals(rp))
                {
                    if ((recordedTVProperties = RecordedTVPropertySets.FirstOrDefault(e => e.Equals(rp))) is null && rp is not null)
                    {
                        RecordedTVPropertySets.Add(rp);
                        recordedTVProperties = rp;
                    }
                }
                if (!videoProperties.Equals(vp))
                {
                    if ((videoProperties = VideoPropertySets.FirstOrDefault(e => e.Equals(vp))) is null && vp is not null)
                    {
                        VideoPropertySets.Add(vp);
                        videoProperties = vp;
                    }
                }
            }
            else
            {
                long length = fileInfo.Length;
                binaryProperties = BinaryPropertySets.FirstOrDefault(c => !c.Hash.HasValue && c.Length == length);
                if (binaryProperties is null)
                {
                    (binaryProperties = new()
                    {
                        CreatedOn = DateTime.Now,
                        Length = length
                    }).ModifiedOn = binaryProperties.CreatedOn;
                    BinaryPropertySets.Add(binaryProperties);
                }
                //SummaryProperties summaryProperties;
                SummaryPropertySet sp = Export.SummaryPropertySet.Create(fileInfo);
                if ((summaryProperties = SummaryPropertySets.FirstOrDefault(e => e.Equals(sp))) is null && sp is not null)
                {
                    SummaryPropertySets.Add(sp);
                    summaryProperties = sp;
                }
                DocumentPropertySet dp = Export.DocumentPropertySet.Create(fileInfo);
                if ((documentProperties = DocumentPropertySets.FirstOrDefault(e => e.Equals(dp))) is null && dp is not null)
                {
                    DocumentPropertySets.Add(dp);
                    documentProperties = dp;
                }
                AudioPropertySet ap = Export.AudioPropertySet.Create(fileInfo);
                if ((audioProperties = AudioPropertySets.FirstOrDefault(e => e.Equals(ap))) is null && ap is not null)
                {
                    AudioPropertySets.Add(ap);
                    audioProperties = ap;
                }
                DRMPropertySet drm = Export.DRMPropertySet.Create(fileInfo);
                if ((drmProperties = DRMPropertySets.FirstOrDefault(e => e.Equals(drm))) is null && drm is not null)
                {
                    DRMPropertySets.Add(drm);
                    drmProperties = drm;
                }
                GPSPropertySet gp = Export.GPSPropertySet.Create(fileInfo);
                if ((gpsProperties = GPSPropertySets.FirstOrDefault(e => e.Equals(gp))) is null && gp is not null)
                {
                    GPSPropertySets.Add(gp);
                    gpsProperties = gp;
                }
                ImagePropertySet ip = Export.ImagePropertySet.Create(fileInfo);
                if ((imageProperties = ImagePropertySets.FirstOrDefault(e => e.Equals(ip))) is null && ip is not null)
                {
                    ImagePropertySets.Add(ip);
                    imageProperties = ip;
                }
                MediaPropertySet mdp = Export.MediaPropertySet.Create(fileInfo);
                if ((mediaProperties = MediaPropertySets.FirstOrDefault(e => e.Equals(mdp))) is null && mdp is not null)
                {
                    MediaPropertySets.Add(mdp);
                    mediaProperties = mdp;
                }
                MusicPropertySet msp = Export.MusicPropertySet.Create(fileInfo);
                if ((musicProperties = MusicPropertySets.FirstOrDefault(e => e.Equals(msp))) is null && msp is not null)
                {
                    MusicPropertySets.Add(msp);
                    musicProperties = msp;
                }
                PhotoPropertySet pp = Export.PhotoPropertySet.Create(fileInfo);
                if ((photoProperties = PhotoPropertySets.FirstOrDefault(e => e.Equals(pp))) is null && pp is not null)
                {
                        PhotoPropertySets.Add(pp);
                    photoProperties = pp;
                }
                RecordedTVPropertySet rp = Export.RecordedTVPropertySet.Create(fileInfo);
                if ((recordedTVProperties = RecordedTVPropertySets.FirstOrDefault(e => e.Equals(rp))) is null && rp is not null)
                {
                    RecordedTVPropertySets.Add(rp);
                    recordedTVProperties = rp;
                }
                VideoPropertySet vp = Export.VideoPropertySet.Create(fileInfo);
                if ((videoProperties = VideoPropertySets.FirstOrDefault(e => e.Equals(vp))) is null && vp is not null)
                {
                    VideoPropertySets.Add(vp);
                    videoProperties = vp;
                }
                file = new File
                {
                    Name = n,
                    CreatedOn = DateTime.Now,
                    CreationTime = fileInfo.CreationTime,
                    LastWriteTime = fileInfo.LastWriteTime,
                    //ExtendedPropertyId = extendedProperties?.Id,
                    BinaryPropertySetId = binaryProperties.Id,
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
