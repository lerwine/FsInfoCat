# Entity Models

## Inheritance Diagram

```mermaid
classDiagram
    class LocalDbEntity
    class PropertiesRow
    PropertiesRow --|> LocalDbEntity

    class AudioPropertiesRow
    AudioPropertiesRow --|> PropertiesRow

    class AudioPropertiesListItem
    AudioPropertiesListItem --|> AudioPropertiesRow

    class AudioPropertySet
    AudioPropertySet --|> AudioPropertiesRow

    class BinaryPropertySet
    BinaryPropertySet --|> LocalDbEntity

    class CrawlConfigurationRow
    CrawlConfigurationRow --|> LocalDbEntity

    class CrawlConfigListItemBase
    CrawlConfigListItemBase --|> CrawlConfigurationRow

    class CrawlConfigListItem
    CrawlConfigListItem --|> CrawlConfigListItemBase

    class CrawlConfigReportItem
    CrawlConfigReportItem --|> CrawlConfigListItemBase

    class CrawlConfiguration
    CrawlConfiguration --|> CrawlConfigurationRow

    class CrawlJobLogRow
    CrawlJobLogRow --|> LocalDbEntity

    class CrawlJobLog
    CrawlJobLog --|> CrawlJobLogRow

    class CrawlJobLogListItem
    CrawlJobLogListItem --|> CrawlJobLogRow

    class DbFileRow
    DbFileRow --|> LocalDbEntity

    class DbFile
    DbFile --|> DbFileRow

    class FileWithAncestorNames
    FileWithAncestorNames --|> DbFileRow

    class FileWithBinaryProperties
    FileWithBinaryProperties --|> DbFileRow

    class FileWithBinaryPropertiesAndAncestorNames
    FileWithBinaryPropertiesAndAncestorNames --|> FileWithBinaryProperties

    class DocumentPropertiesRow
    DocumentPropertiesRow --|> PropertiesRow

    class DocumentPropertiesListItem
    DocumentPropertiesListItem --|> DocumentPropertiesRow

    class DocumentPropertySet
    DocumentPropertySet --|> DocumentPropertiesRow
```

__________________________________________________________________________
References

- [Class Diagrams in Mermaid](https://mermaid.js.org/syntax/classDiagram.html)
- [Entity Diagrams in Mermaid](https://mermaid.js.org/syntax/entityRelationshipDiagram.html)
