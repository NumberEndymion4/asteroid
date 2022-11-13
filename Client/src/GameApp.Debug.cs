using Client.Presenters;
using Core;
using Core.Collisions;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
	internal partial class GameApp
	{
		private Texture2D circleTexture;

		partial void LoadPartial()
		{
			circleTexture = Content.Load<Texture2D>("circle");
		}

		partial void ObtainCircleColliderPresenterPartial(CircleCollider collider, ref IPresenter presenter)
		{
			presenter = new CircleColliderPresenter(collider, circleTexture);
		}
	}
}
