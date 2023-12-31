﻿using System.IdentityModel.Tokens.Jwt;

namespace Maui.Authentication.Core
{
    internal class JwtService
    {
        public JwtSecurityToken? DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            return jsonToken as JwtSecurityToken;
        }
    }
}
