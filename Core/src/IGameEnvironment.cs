﻿using Microsoft.Xna.Framework;

namespace Core
{
	public interface IGameEnvironment
	{
		IKeyStateProvider GetKeyStateProvider();

		IPresenter GetSpaceshipPresenter(IGameObject spaceship);
		IPresenter GetAsteroidPresenter(IGameObject asteroid);
		IPresenter GetBoundsPresenter(ICollider collider);

		IPresenter GetSpaceshipPositionToHudPresenter(IDataProvider<Vector2> positionProvider);
		IPresenter GetSpaceshipAngleToHudPresenter(IDataProvider<float> angleProvider);
	}
}