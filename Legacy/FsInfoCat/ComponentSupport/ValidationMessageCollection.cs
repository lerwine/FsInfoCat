using FsInfoCat.Collections;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Wraps a list of <see cref="System.ComponentModel.DataAnnotations.ValidationResult"/> objects, presenting them as a list of string messages.
    /// </summary>
    /// <seealso cref="DerivedListBase{ValidationResultCollection, string}" />
    public class ValidationMessageCollection : DerivedListBase<ValidationResult, string>
    {
        internal ValidationMessageCollection(ValidationResultCollection source) : base(source, StringComparer.InvariantCultureIgnoreCase) { }

        protected override string GetDerivedValue(ValidationResult item) => item.ToString();
    }
}
