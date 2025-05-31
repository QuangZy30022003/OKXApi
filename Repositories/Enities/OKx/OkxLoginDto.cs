using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.OKx
{
    public class OkxLoginDto
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string Passphrase { get; set; } // OKX cần passphrase
    }

}
