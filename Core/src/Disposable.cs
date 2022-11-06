using System;

namespace Core
{
	public abstract class Disposable : IDisposable
	{
		private bool isDisposed;

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!isDisposed && disposing) {
				PerformDispose();
			}
			isDisposed = true;
		}

		protected virtual void PerformDispose()
		{
		}
	}
}
