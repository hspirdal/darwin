using System;

namespace GameLib.Identities
{
    public class AuthentificationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConnectionId { get; set; }

        public AuthentificationRequest()
        {
            UserName = String.Empty;
            Password = String.Empty;
            ConnectionId = string.Empty;
        }

    }
}