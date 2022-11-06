using Core;
using Microsoft.Xna.Framework;

namespace Asteroids
{
	internal delegate void SpawnHandler(IComponent sender, Vector2 position, float rotation);

	internal interface ISpawner
	{
		event SpawnHandler Spawn;
	}
}
