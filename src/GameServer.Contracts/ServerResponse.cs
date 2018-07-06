namespace TcpGameServer.Contracts
{
    public class ServerResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Payload { get; set; }
    }
}