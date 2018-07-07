using System;

namespace TcpGameServer.Contracts
{
    public class AuthenticationResponse
    {
        public bool Success { get; set; }
        public Guid SessionId { get; set; }
    }
}