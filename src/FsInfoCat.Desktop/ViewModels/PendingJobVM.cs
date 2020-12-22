using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Schema;
using FsInfoCat.Desktop.Commands;

namespace FsInfoCat.Desktop.ViewModels
{
    public class PendingJobVM : DependencyObject
    {
        #region Subject Property Members

        /// <summary>
        /// Defines the name for the <see cref="Subject" /> dependency property.
        /// </summary>
        public const string PropertyName_Subject = "Subject";

        private static readonly DependencyPropertyKey SubjectPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_Subject, typeof(string), typeof(PendingJobVM), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Subject" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubjectProperty = SubjectPropertyKey.DependencyProperty;

        /// <summary>
        /// Subject
        /// </summary>
        public string Subject
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(SubjectProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(SubjectProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(SubjectPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(SubjectPropertyKey, value));
            }
        }

        #endregion

        #region "Job type" (JobType) Property Members

        /// <summary>
        /// Defines the name for the <see cref="JobType" /> dependency property.
        /// </summary>
        public const string PropertyName_JobType = "JobType";

        private static readonly DependencyPropertyKey JobTypePropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_JobType, typeof(PendingJobType), typeof(PendingJobVM),
                new PropertyMetadata(default(PendingJobType), null,
                    (d, baseValue) => CoerceJobTypeValue(baseValue as PendingJobType?)
            )
        );

        /// <summary>
        /// Identifies the <see cref="JobType" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty JobTypeProperty = JobTypePropertyKey.DependencyProperty;

        public static PendingJobType CoerceJobTypeValue(PendingJobType? value)
        {
            if (value.HasValue)
                return value.Value;
            return default(PendingJobType);
        }

        /// <summary>
        /// Job type
        /// </summary>
        public PendingJobType JobType
        {
            get
            {
                if (CheckAccess())
                    return (PendingJobType)(GetValue(JobTypeProperty));
                return Dispatcher.Invoke(() => (PendingJobType)(GetValue(JobTypeProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(JobTypePropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(JobTypePropertyKey, value));
            }
        }

        #endregion

    }
}
