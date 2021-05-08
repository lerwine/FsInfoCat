using FsInfoCat.Desktop.Util;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public class ValidationAttributeList : Collection<ValidationAttribute>
    {
        private readonly object _syncRoot = new object();
        public ValidationAttributeList() { }

        private int Find(ValidationAttribute item)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                AttributeComparer<ValidationAttribute> comparer = AttributeComparer<ValidationAttribute>.Instance;
                int index = -1;
                foreach (ValidationAttribute attribute in Items)
                {
                    index++;
                    if (comparer.Equals(item, attribute))
                        return index;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return -1;
        }

        protected override void InsertItem(int index, ValidationAttribute item)
        {
            if (item is null)
                throw new ArgumentNullException();
            Monitor.Enter(_syncRoot);
            try
            {
                int oldIndex = Find(item);
                if (oldIndex < 0)
                    base.InsertItem(index, item);
                else
                {
                    base.InsertItem(index, item);
                    base.RemoveItem((oldIndex < index) ? oldIndex : oldIndex + 1);
                }
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override void SetItem(int index, ValidationAttribute item)
        {
            if (item is null)
                throw new ArgumentNullException();
            Monitor.Enter(_syncRoot);
            try
            {
                int oldIndex = IndexOf(item);
                base.SetItem(index, item);
                if (oldIndex >= 0 && index != oldIndex)
                    base.RemoveItem(oldIndex);
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override void ClearItems()
        {
            Monitor.Enter(_syncRoot);
            try { base.ClearItems(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override void RemoveItem(int index)
        {
            Monitor.Enter(_syncRoot);
            try { base.RemoveItem(index); }
            finally { Monitor.Exit(_syncRoot); }
        }
    }
}
