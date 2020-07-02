using consultant_server.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consultant_server.Services
{
    public class Auth0Provider : IAuthProvider
    {
        public string GetUserIdFromToken(HttpContext http)
        {
            return http.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
    }
}
