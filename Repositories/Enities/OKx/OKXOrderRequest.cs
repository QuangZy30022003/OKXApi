using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.OKx
{
    public class OKXOrderRequest
    {
     //   public string Secret { get; set; }
        public string InstId { get; set; } = null!;
        public string TdMode { get; set; } = null!;
        public string ClOrdId { get; set; } = null!;
        public string Side { get; set; } = null!;
        public string OrdType { get; set; } = null!;
        public string Px { get; set; } = null!;
        public string Sz { get; set; } = null!;
    }
}
