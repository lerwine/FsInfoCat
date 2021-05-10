using FsInfoCat.Collections;
using System;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Wraps a list of <see cref="System.ComponentModel.DataAnnotations.ValidationResult"/> objects, presenting them as a list of string messages.
    /// </summary>
    /// <seealso cref="DerivedListBase{ValidationResultCollection.Item, string}" />
    public class ValidationMessageCollection : DerivedListBase<ValidationResultCollection.Item, string>
    {
        internal ValidationMessageCollection(ValidationResultCollection source) : base(source, StringComparer.InvariantCultureIgnoreCase) { }

        protected override string GetDerivedValue(ValidationResultCollection.Item item) => item.ToString();
    }
}
