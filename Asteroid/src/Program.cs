namespace Asteroid
{
	public static class Program
	{
		static int Main(string[] args)
		{
			using var game = new GameApp();
			game.Run();
			return 0;
		}
	}
}
