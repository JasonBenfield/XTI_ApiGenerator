using System;
using System.Linq;

namespace XTI_CoreApiGeneratorTask
{
    public sealed class ApiGeneratorTaskException : Exception
    {
        public ApiGeneratorTaskException(ApiGeneratorTaskError[] errors)
            : base(string.Join("\r\n", errors.Select(e => e.Message)))
        {
            Errors = errors;
        }

        public ApiGeneratorTaskError[] Errors { get; }
    }
}
