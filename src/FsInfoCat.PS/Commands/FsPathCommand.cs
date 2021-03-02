using FsInfoCat.Models.Crawl;
using FsInfoCat.Util;
using System;
using System.Management.Automation;

namespace FsInfoCat.PS.Commands
{
    public class FsPathCommand : PSCmdlet
    {
        private string _conversionErrorMessage;
        private string _conversionErrorId;

        protected void SetConversionMessageId(MessageId messageId)
        {
            _conversionErrorMessage = MessageId.InvalidPath.GetDescription(out string errorId);
            _conversionErrorId = errorId;
        }

        protected bool ValidateItem(object source, FileUri fileUri, string paramName)
        {
            try
            {
                if (fileUri is null || !fileUri.IsAbsolute)
                    throw new PSArgumentOutOfRangeException(paramName, source, _conversionErrorMessage);
                return true;
            }
            catch (PSArgumentOutOfRangeException exc)
            {
                if (paramName.Equals(exc.ParamName) && ReferenceEquals(source, exc.ActualValue))
                    WriteError(new ErrorRecord(exc, _conversionErrorId, ErrorCategory.InvalidArgument, source));
                else
                {
                    ErrorRecord errorRecord = exc.ErrorRecord;
                    if (errorRecord is null)
                        WriteError(new ErrorRecord(exc, MessageId.UnexpectedError.ToString("F"), ErrorCategory.InvalidArgument, exc.ActualValue));
                    else
                        WriteError(new ErrorRecord(exc, errorRecord.FullyQualifiedErrorId, errorRecord.CategoryInfo.Category, errorRecord.TargetObject));
                }
            }
            catch (Exception exc)
            {
                ErrorRecord errorRecord = (exc is IContainsErrorRecord ce) ? ce.ErrorRecord : null;
                if (errorRecord is null)
                    WriteError(new ErrorRecord(exc, MessageId.UnexpectedError.ToString("F"), ErrorCategory.InvalidArgument, source));
                else
                    WriteError(new ErrorRecord(exc, errorRecord.FullyQualifiedErrorId, errorRecord.CategoryInfo.Category, errorRecord.TargetObject));
            }
            return false;
        }
    }
}
