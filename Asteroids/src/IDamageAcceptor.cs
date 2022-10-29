using System.Collections.Generic;
using Core;

namespace Asteroids
{
	internal interface IDamageAcceptor : IComponent
	{
		ISet<int> SensitiveTo { get; }
	}
}
