using System;
using Microsoft.Xna.Framework;

namespace Core
{
	public interface IComponent : IDisposable
	{
		IGameObject Owner { get; }

		void Update(GameTime gameTime);
	}
}
