using System.Collections.Generic;
using Core;

namespace Asteroids
{
	internal interface IDamageAcceptor : IBehavior
	{
		ISet<int> SensitiveTo { get; }
	}
}
