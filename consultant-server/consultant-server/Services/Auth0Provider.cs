using consultant_server.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace consultant_server.Services
{
    public class Auth0Provider : IAuthProvider
    {
        public string GetUserIdFromToken(HttpContext http)
        {
            Claim c = http.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (c == null)
                return "";

            return c.Value;
        }
    }
}
