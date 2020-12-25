Param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^[A-Z][A-Z\da-z]*([A-Z\d][A-Z\da-z]*)*$')]
    [string]$Name
)

$FilePath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat\Models\$Name.cs";
@"
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class $Name
    {
        private string _displayName = "";
        private string _notes = "";

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid $($Name)ID { get; set; }

        [MaxLength(256)]
        [Display(Name = "Display Name")]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public $($Name)() { }

        public $($Name)(string displayName, Guid createdBy)
        {
            $($Name)ID = Guid.NewGuid();
            DisplayName = displayName;
            CreatedOn = ModifiedOn = DateTime.Now;
            CreatedBy = ModifiedBy = createdBy;
        }

    }
}
"@ | Out-File -FilePath $FilePath;
$FilePath = (Get-Command -Name 'dotnet').Path;
Push-Location;
try {
    Set-Location -Path ($PSScriptRoot | Join-Path -ChildPath 'src\FsInfoCat.Web');
    . dotnet build
    . dotnet aspnet-codegenerator controller -name "$($Name)Controller" -m $Name -dc FsInfoDataContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
} finally {
    Pop-Location;
}
