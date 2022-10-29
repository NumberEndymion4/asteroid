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
		public bool IsPlaying { get; private set; }

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
			IsPlaying = true;
		}

		public void Continue(GameTime gameTime, out Rectangle region)
		{
			int lastFrame = regions.Count - 1;
			if (lastFrame < 0) {
				region = Rectangle.Empty;
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
