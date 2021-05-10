using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FsInfoCat.ComponentSupport
{
    public class ValidationAttributeCollection : GeneralizableListBase<ValidationAttribute>
    {
        public override ValidationAttribute this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int Count => throw new NotImplementedException();

        public override void Add(ValidationAttribute item)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(ValidationAttribute item)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(ValidationAttribute[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<ValidationAttribute> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(ValidationAttribute item)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, ValidationAttribute item)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(ValidationAttribute item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        protected override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator GetGenericEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
