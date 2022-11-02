using Core;

namespace Asteroids
{
	internal delegate void SpawnHandler(IComponent sender);

	internal interface ISpawner
	{
		event SpawnHandler Spawn;
	}
}
