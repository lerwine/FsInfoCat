using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FsInfoCat.Models.DB
{
    [DisplayColumn("HostDeviceID", "DisplayName", false)]
    public class HostDevice : IHostDevice
    {
        #region Properties

        public string Name
        {
            get
            {
                string n = _displayName;
                if (string.IsNullOrWhiteSpace(n))
                {
                    n = _machineName;
                    if (string.IsNullOrWhiteSpace(n))
                        n = _machineIdentifer;
                }
                return n;
            }
        }

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid HostDeviceID { get; set; }

        #region DisplayName

        public const int Max_Length_DisplayName = 128;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        private string _displayName = "";

        [MaxLength(DB.HostDevice.Max_Length_DisplayName, ErrorMessage = DB.HostDevice.Error_Message_DisplayName)]
        [Display(Name = DB.HostDevice.DisplayName_DisplayName)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        #endregion

        #region MachineIdentifer

        public const string DisplayName_MachineIdentifer = "Machine Unique Identifier";
        public const string PropertyName_MachineIdentifer = "MachineIdentifer";
        public const int Max_Length_MachineIdentifer = 128;
        public const string Error_Message_MachineIdentifer = "Machine identifier too long.";
        private string _machineIdentifer = "";

        // TODO: Add this to db script and view
        [MaxLength(DB.HostDevice.Max_Length_MachineIdentifer, ErrorMessage = DB.HostDevice.Error_Message_MachineIdentifer)]
        [Display(Name = DB.HostDevice.DisplayName_MachineIdentifer)]
        public string MachineIdentifer
        {
            get { return _machineIdentifer; }
            set { _machineIdentifer = (null == value) ? "" : value; }
        }

        #endregion

        #region MachineName

        public const string DisplayName_MachineName = "Machine name";
        public const string PropertyName_MachineName = "MachineName";
        public const int Max_Length_MachineName = 128;
        public const string Error_Message_MachineName_Empty = "Machine name cannot be empty";
        public const string Error_Message_MachineName_Length = "Machine name too long.";
        public const string Error_Message_MachineName_Invalid = "Invalid machine name";
        private string _machineName = "";

        [Required()]
        [MinLength(1)]
        [MaxLength(DB.HostDevice.Max_Length_MachineName, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Length)]
        [Display(Name = DB.HostDevice.DisplayName_MachineName)]
        [RegularExpression(ModelHelper.PATTERN_MACHINE_NAME, ErrorMessage = DB.HostDevice.Error_Message_MachineName_Invalid)]
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (null == value) ? "" : value; }
        }

        #endregion

        [Display(Name = "Is Windows OS")]
        public bool IsWindows { get; set; }

        [Display(Name = "Allow Local Crawl")]
        public bool AllowCrawl { get; set; }

        [Display(Name = "Is Inactive")]
        public bool IsInactive { get; set; }

        #region Notes

        private string _notes = "";

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        #endregion

        #region Audit

        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        public Account Creator { get; set; }

        public string CreatorName
        {
            get
            {
                Account account = Creator;
                return (null == account) ? "" : account.Name;
            }
        }

        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Editable(false)]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public Account Modifier { get; set; }

        public string ModifierName
        {
            get
            {
                Account account = Modifier;
                return (null == account) ? "" : account.Name;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public HostDevice()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        protected HostDevice(HostDevice host)
        {
            if (null == host)
                throw new ArgumentNullException("host");
            HostDeviceID = host.HostDeviceID;
            DisplayName = host.DisplayName;
            MachineIdentifer = host.MachineIdentifer;
            MachineName = host.MachineName;
            AllowCrawl = host.AllowCrawl;
            IsWindows = host.IsWindows;
            CreatedOn = host.CreatedOn;
            CreatedBy = host.CreatedBy;
            ModifiedOn = host.ModifiedOn;
            ModifiedBy = host.ModifiedBy;
        }

        public HostDevice(HostDeviceRegRequest request, Guid createdBy) : this()
        {
            if (null == request)
                throw new ArgumentNullException("request");
            HostDeviceID = Guid.NewGuid();
            MachineIdentifer = request.MachineIdentifer;
            MachineName = request.MachineName;
            AllowCrawl = request.AllowCrawl;
            IsWindows = request.IsWindows;
            CreatedBy = ModifiedBy = createdBy;
        }

        public HostDevice(HostDeviceRegRequest request, Account createdBy) : this()
        {
            if (null == request)
                throw new ArgumentNullException("request");
            HostDeviceID = Guid.NewGuid();
            MachineIdentifer = request.MachineIdentifer;
            MachineName = request.MachineName;
            AllowCrawl = request.AllowCrawl;
            IsWindows = request.IsWindows;
            Creator = Modifier = createdBy;
            CreatedBy = ModifiedBy = createdBy.AccountID;
        }

        #endregion

        public void Normalize()
        {
            if (HostDeviceID.Equals(Guid.Empty))
                HostDeviceID = Guid.NewGuid();
            _machineIdentifer = ModelHelper.CoerceAsWsNormalized(_machineIdentifer);
            _machineName = _machineName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _machineName;
            _notes = _notes.Trim();
            CreatedOn = ModelHelper.CoerceAsLocalTime(CreatedOn);
            ModifiedOn = ModelHelper.CoerceAsLocalTime(ModifiedOn);
            if (null != Creator)
                CreatedBy = Creator.AccountID;
            if (null != Modifier)
                ModifiedBy = Modifier.AccountID;
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_DisplayName);
            Validate(result, PropertyName_MachineIdentifer);
            Validate(result, PropertyName_MachineName);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_DisplayName:
                case DisplayName_DisplayName:
                    if (_displayName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_DisplayName, new string[] { PropertyName_DisplayName }));
                    break;
                case PropertyName_MachineIdentifer:
                case DisplayName_MachineIdentifer:
                    if (_machineIdentifer.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_MachineIdentifer, new string[] { PropertyName_MachineIdentifer }));
                    break;
                case PropertyName_MachineName:
                case DisplayName_MachineName:
                    if (_machineName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_MachineName_Empty, new string[] { PropertyName_MachineName }));
                    else if (_machineName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_MachineName_Length, new string[] { PropertyName_MachineName }));
                    else if (!ModelHelper.MachineNameRegex.IsMatch(_machineName))
                        result.Add(new ValidationResult(Error_Message_MachineName_Invalid, new string[] { PropertyName_MachineName }));
                    break;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            if (string.IsNullOrEmpty(validationContext.DisplayName))
                OnValidateAll(result);
            else
                Validate(result, validationContext.DisplayName);
            return result;
        }

        public static async Task<HostDevice> LookUp(DbSet<HostDevice> dbSet, string machineName, string machineIdentifer)
        {
            if (string.IsNullOrWhiteSpace(machineName))
                return null;
            IQueryable<HostDevice> hostDevices = from d in dbSet select d;
            if (string.IsNullOrWhiteSpace(machineIdentifer))
                hostDevices = hostDevices.Where(h => string.Equals(machineName, h.MachineName, StringComparison.InvariantCulture));
            else
                hostDevices = hostDevices.Where(h => string.Equals(machineName, h.MachineName, StringComparison.InvariantCulture) &&
                    string.Equals(machineIdentifer, h.MachineIdentifer, StringComparison.InvariantCulture));
            return (await hostDevices.AsNoTracking().ToListAsync()).FirstOrDefault();
        }
    }
}
