using Core;

namespace Asteroids.Broadcast
{
	internal class ScoreMessage : IBroadcastMessage
	{
		public string Tag => "scores";
		public int Scores { get; }

		public ScoreMessage(int scores)
		{
			Scores = scores;
		}
	}
}
