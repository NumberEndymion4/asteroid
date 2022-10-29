using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
	internal interface IRenderRegion
	{
		Texture2D Texture { get; }
		Rectangle Region { get; }
		bool IsReady { get; }
	}
}
