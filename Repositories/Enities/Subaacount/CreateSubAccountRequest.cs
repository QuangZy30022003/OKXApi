using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.Subaacount
{
    public class CreateSubAccountRequest
    {
        public string SubAccountName { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }

        public string ApiKey { get; set; } = null!;

    }
}
