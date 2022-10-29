using Core;

namespace Asteroids
{
	internal interface IDamageProvider : IComponent
	{
		int DamageGroup { get; }
		int Damage { get; }
	}
}
