using Client.Presenters;
using Core;

namespace Client
{
	internal partial class GameApp
	{
		partial void ObtainBoundsPresenterPartial(ICollider collider, ref IPresenter presenter)
		{
			presenter = new NoPresent();
		}
	}
}
