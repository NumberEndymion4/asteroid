using System;

namespace Asteroids
{
	internal interface ITrigger
	{
		event Action Triggered;
	}
}
