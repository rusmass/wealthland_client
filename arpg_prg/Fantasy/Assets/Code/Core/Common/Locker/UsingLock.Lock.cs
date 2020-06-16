using System;
using System.Threading;

namespace Core
{
	public enum LockModel
	{
		Read,
		Write,
		UpgradeableRead,
	}
	
	public partial class UsingLock
	{
		private struct Lock : IDisposable
		{
			public Lock (ReaderWriterLockSlim rwl, LockModel model)
			{
				_locker = rwl;
				_model = model;
			}
			
			public override string ToString ()
			{
				return string.Format ("[UsingLock.Lock]");
			}
			
			public void Dispose ()
			{
				switch (_model) 
				{
				case LockModel.Read :
					if (_locker.IsReadLockHeld)
					{
						_locker.ExitReadLock();
					}
					break;
				case LockModel.Write :
					if (_locker.IsWriteLockHeld)
					{
						_locker.ExitWriteLock();
					}
					break;
				case LockModel.UpgradeableRead :
					if (_locker.IsUpgradeableReadLockHeld)
					{
						_locker.ExitUpgradeableReadLock();
					}
					break;
				}
			}
			
			private ReaderWriterLockSlim _locker;
			private LockModel _model;
		}
	}
}
