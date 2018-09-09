namespace Client.Contracts
{
	public enum ResponseType
	{
		InvalidType,
		GameStatus,
		GameState,
		GameMessage,
		NotAuthenticated,
		RequestAccepted,
		RequestDeclined,
		RequestMalformed
	}
}