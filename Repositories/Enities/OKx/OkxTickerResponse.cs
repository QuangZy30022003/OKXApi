using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.OKx
{
    public class OkxTickerResponse
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public List<TickerData> Data { get; set; }
    }

    public class TickerData
    {
        public string InstId { get; set; }
        public string Last { get; set; }
        public string Open24h { get; set; }
    }
}
