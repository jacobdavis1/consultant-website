using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consultant_server.ServiceInterfaces
{
    public interface IAuthProvider
    {
        string GetUserIdFromToken(HttpContext http);
    }
}
