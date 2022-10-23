using System;
using Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Presenters
{
	internal class TextPresenter<T> : IPresenter
	{
		private readonly SpriteFont font;
		private readonly Vector2 position;
		private readonly IDataProvider<T> provider;
		private readonly Func<T, string> dataConverter;

		public TextPresenter(
			SpriteFont spriteFont,
			Vector2 textPosition,
			IDataProvider<T> textProvider,
			Func<T, string> dataToString
		) {
			font = spriteFont;
			position = textPosition;
			provider = textProvider;
			dataConverter = dataToString;
		}

		public void Render(SpriteBatch spriteBatch)
		{
			var text = dataConverter?.Invoke(provider.Data) ?? provider.Data.ToString();
			spriteBatch.DrawString(font, text, position, Color.White);
		}
	}
}
