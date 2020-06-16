using System;
using System.Threading;

namespace Core
{
	public partial class UsingLock
	{
		public IDisposable Read()
		{
			if (_lockSlim.IsReadLockHeld || _lockSlim.IsWriteLockHeld) 
			{
				return Disposable.Empty;
			}
			else 
			{
				_lockSlim.EnterReadLock();
				return new Lock(_lockSlim, LockModel.Read);
			}
		}

		public IDisposable UpgradeableRead()
		{
			if (_lockSlim.IsUpgradeableReadLockHeld) 
			{
				return Disposable.Empty;
			}
			else 
			{
				_lockSlim.EnterUpgradeableReadLock();
				return new Lock(_lockSlim, LockModel.UpgradeableRead);
			}
		}

		public IDisposable Write()
		{
			if (_lockSlim.IsWriteLockHeld) 
			{
				return Disposable.Empty;
			}
			else if (_lockSlim.IsReadLockHeld) 
			{
				throw new NotImplementedException ("[UsingLock.Write() -- Read Mode can not Write]");
			}
			else 
			{
				_lockSlim.EnterWriteLock();
				return new Lock(_lockSlim, LockModel.Write);
			}
		}

		private ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
	}
}
