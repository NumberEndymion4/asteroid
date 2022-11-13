using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
	internal class SpriteAnimation
	{
		public class Region
		{
			public readonly Rectangle Bounds;
			public readonly Vector2 Origin;
			public readonly Vector2 Scale;

			public Region(Rectangle bounds, Vector2 origin, Vector2 scale)
			{
				Bounds = bounds;
				Origin = origin;
				Scale = scale;
			}
		}

		private static readonly TimeSpan FrameDuration = TimeSpan.FromSeconds(1d / 30);

		private readonly List<Region> regions;

		private TimeSpan startTime;

		public Texture2D Texture { get; }
		public bool IsPlaying { get; private set; }

		public SpriteAnimation(Texture2D texture)
		{
			regions = new List<Region>();
			Texture = texture;
		}

		public void AppendRegion(Rectangle bound)
		{
			AppendRegion(bound, new Vector2(0.5f), Vector2.One);
		}

		public void AppendRegion(Rectangle bound, Vector2 origin, Vector2 scale)
		{
			regions.Add(new Region(bound, origin, scale));
		}

		public void Start(GameTime gameTime)
		{
			startTime = gameTime.TotalGameTime;
			IsPlaying = true;
		}

		public void Continue(GameTime gameTime, out Region region)
		{
			int lastFrame = regions.Count - 1;
			if (lastFrame < 0) {
				region = null;
				IsPlaying = false;
				return;
			}

			if (!IsPlaying) {
				region = regions[lastFrame];
				return;
			}

			int frame = (int) ((gameTime.TotalGameTime - startTime) / FrameDuration);
			if (frame > lastFrame) {
				region = regions[lastFrame];
				IsPlaying = false;
			} else {
				region = regions[frame];
			}
		}
	}
}
