namespace FsInfoCat.Models
{
    public class RequestResponse<T>
    {
        private string _message = "";

        public RequestResponse(T result, string failureMessage = null)
        {
            Success = string.IsNullOrWhiteSpace(failureMessage);
            Message = failureMessage;
            Result = result;
        }

        public T Result { get; set; }

        public bool Success { get; set; }

        public string Message
        {
            get { return _message; }
            set { _message = (value is null) ? "" : value; }
        }
    }
}
