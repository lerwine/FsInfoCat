using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FsInfoCat.ComponentSupport
{
    public class ValidationMessageCollection : DerivedListBase<ValidationResultCollection.Item, string>
    {
        internal ValidationMessageCollection(ValidationResultCollection source) : base(source, StringComparer.InvariantCultureIgnoreCase) { }

        protected override string GetDerivedValue(ValidationResultCollection.Item item) => item.ToString();
    }
}
