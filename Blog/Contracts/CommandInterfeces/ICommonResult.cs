using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Contracts.CommandInterfeces
{
    public interface ICommonResult
    {
        int TotalResults { get; }
        string FlashMessage { get; }
        bool Success { get; }
    }
    public class CommonResult : ICommonResult
    {
        public int TotalResults { get; }

        public string FlashMessage { get; }

        public bool Success { get; }
        public CommonResult(int TotalResults, string FlashMessage, bool Success)
        {
            this.TotalResults = TotalResults;
            this.FlashMessage = FlashMessage;
            this.Success = Success;
        }
    }
}
