using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
	internal interface IRenderRegion
	{
		Texture2D Texture { get; }
		Rectangle Bounds { get; }
		Vector2 Origin { get; }
		Vector2 Scale { get; }
		bool IsReady { get; }
	}
}
