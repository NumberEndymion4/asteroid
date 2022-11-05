using System;

namespace Core
{
	[Flags]
	public enum Orientation : int
	{
		None = 0,
		Horizontal = 1,
		Vertical = 1 << 1,

		All = Horizontal | Vertical
	}
}
