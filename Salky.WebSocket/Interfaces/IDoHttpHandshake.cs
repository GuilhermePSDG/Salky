﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IDoHttpHandshake
    {
        /// <summary>
        /// Any <see cref="Exception"/> when want reject the connection
        /// <para></para>
        /// The User Claims <paramref name="Claims"/>
        /// <para></para>
        /// <paramref name="SocketKey"/> Usualy will be the UserId, must be unique
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="Claims"></param>
        /// <param name="SocketKey"></param>
        /// <exception cref="Exception"></exception>
        public void MakeOrThrow(HttpContext httpContext,out List<Claim> Claims,out string SocketKey);
    }
}
