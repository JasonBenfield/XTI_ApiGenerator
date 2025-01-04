namespace XTI_ApiGeneratorTask
{
    public sealed class ApiGeneratorTaskError
    {
        public ApiGeneratorTaskError(string message, string errorCode, string filePath)
        {
            Message = message;
            ErrorCode = errorCode;
            FilePath = filePath;
        }

        public string Message { get; }
        public string ErrorCode { get; }
        public string FilePath { get; }
    }
}
