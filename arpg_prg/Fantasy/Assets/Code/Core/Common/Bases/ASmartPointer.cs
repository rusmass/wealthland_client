
namespace Core
{
	public abstract class ASmartPointer : ADisposable, IIsDisposable
	{
		internal void AddReference()
		{
			++_referCount;
		}

		internal void RemoveReference()
		{
			--_referCount;
		}

		internal int GetReference()
		{
			return _referCount;
		}

		public bool IsDisposable()
		{
			return 0 == _referCount;
		}

		private int _referCount;
	}
}
