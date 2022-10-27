using Core;

namespace Asteroids
{
	internal interface IDamageProvider : IBehavior
	{
		int DamageGroup { get; }
		int Damage { get; }
	}
}
