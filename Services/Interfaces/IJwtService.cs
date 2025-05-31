using Microsoft.Extensions.Configuration;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user, IConfiguration config);
        string GenerateToken(SubAccount subAccount, IConfiguration config);

        string GenerateToken(int userId, string email, string role);
    }
}
