using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace DevUtil
{
    public static class ExtensionMethods
    {
        public static Exception GetCausalException(this Exception source)
        {
            if (source is null) return null;
            if ((source is MethodInvocationException || source is GetValueInvocationException || source is SetValueInvocationException) && source.InnerException is not null)
                source = source.InnerException;
            return (source is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1) ? aggregateException.InnerException : source;
        }
    }
}
