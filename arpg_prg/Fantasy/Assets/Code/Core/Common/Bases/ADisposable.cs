using System;

namespace Core
{
	public abstract class ADisposable : IDisposable
	{
		~ADisposable()
		{
			DisposableRecycler.Add (this);
		}

		public void Dispose()
		{
			if (!_isDisposed) 
			{
				try
				{
					_DoDispose(true);
				}
				finally
				{
					_isDisposed = true;
					GC.SuppressFinalize(this);
				}
			}
		}

		public bool IsDisposed()
		{
			return _isDisposed;
		}

		protected abstract void _DoDispose(bool isDisposing);

		private bool _isDisposed;
	}
}

