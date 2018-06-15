namespace TcpGameServer.Players
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GameState GameState { get; set; }
    }
}