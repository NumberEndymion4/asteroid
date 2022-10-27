using System.Collections.Generic;
using Core;

namespace Asteroids
{
	internal interface IDamageAcceptor : IBehavior
	{
		int Health { get; }
		ISet<int> SensitiveTo { get; }
	}
}
