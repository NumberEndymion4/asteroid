using Client.Presenters;
using Core;
using Core.Collisions;

namespace Client
{
	internal partial class GameApp
	{
		partial void ObtainCircleColliderPresenterPartial(
			CircleCollider collider, ref IPresenter presenter
		) {
			presenter = new NoPresent();
		}
	}
}
