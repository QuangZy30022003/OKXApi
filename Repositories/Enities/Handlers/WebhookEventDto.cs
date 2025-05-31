using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enities.Handlers
{
    public class WebhookEventDto
    {
        public string Secret { get; set; } = null!;
        public string Type { get; set; } = null!; // Enum dạng string
        public decimal Price { get; set; }
    }
}
