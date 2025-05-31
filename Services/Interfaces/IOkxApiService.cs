using Repositories.Enities.OKx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOkxApiService
    {
        Task<bool> PlaceOrderAsync(OKXOrderRequest requestData);

    }
}
