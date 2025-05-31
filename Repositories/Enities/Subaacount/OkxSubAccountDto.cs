using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.Subaacount
{
    public class OkxSubAccountDto
    {
        [JsonProperty("subAcct")]
        public string SubAccount { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("ts")]
        public string Timestamp { get; set; }
    }
    }
