﻿using System;
using Core.Collisions;
using Microsoft.Xna.Framework;

namespace Core
{
	public interface IGameEnvironment
	{
		IPresenter GetSpaceshipPresenter(IGameObject spaceship);
		IPresenter GetAsteroidPresenter(IGameObject asteroid);
		IPresenter GetBulletPresenter(IGameObject bullet);
		IPresenter GetLaserPresenter(IGameObject laser);
		IPresenter GetCircleColliderPresenter(CircleCollider collider);

		IPresenter GetSpaceshipPositionToHudPresenter(IDataProvider<Vector2> positionProvider);
		IPresenter GetSpaceshipAngleToHudPresenter(IDataProvider<float> angleProvider);
		IPresenter GetLaserCooldownToHudPresenter(IDataProvider<TimeSpan> cooldownProvider);
	}
}
