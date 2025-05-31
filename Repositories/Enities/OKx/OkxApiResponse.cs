using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.OKx
{
    public class OkxApiResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }

    public class OkxApiResponse<T> : OkxApiResponse
    {
        public T Data { get; set; }
    }
}
