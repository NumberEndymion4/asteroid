using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
	internal class SpriteAnimation
	{
		private static readonly TimeSpan FrameDuration = TimeSpan.FromSeconds(1d / 30);

		private readonly List<Rectangle> regions;

		private TimeSpan startTime;

		public Texture2D Texture { get; }

		public SpriteAnimation(Texture2D texture)
		{
			regions = new List<Rectangle>();
			Texture = texture;
		}

		public void AppendRegion(Rectangle region)
		{
			regions.Add(region);
		}

		public void AppendRegions(params Rectangle[] regionList)
		{
			regions.AddRange(regionList);
		}

		public void Start(GameTime gameTime)
		{
			startTime = gameTime.TotalGameTime;
		}

		public void Continue(GameTime gameTime, out Rectangle region)
		{
			int lastRegion = regions.Count - 1;

			if (lastRegion < 0) {
				region = Rectangle.Empty;
			} else {
				int frame = (int) ((gameTime.TotalGameTime - startTime) / FrameDuration);
				region = regions[Math.Min(lastRegion, frame)];
			}
		}
	}
}
