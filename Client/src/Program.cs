namespace Client
{
	internal static class Program
	{
		private static int Main(string[] args)
		{
			using var game = new GameApp();
			game.Run();
			return 0;
		}
	}
}
