using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.OKx
{
    public class OkxResult
    {
        public bool IsSuccess { get; set; }
        public string? Content { get; set; }
        public string? ErrorMessage { get; set; }

        public static OkxResult Success(string content) => new OkxResult
        {
            IsSuccess = true,
            Content = content
        };

        public static OkxResult Failure(string error) => new OkxResult
        {
            IsSuccess = false,
            ErrorMessage = error
        };
    }
}
