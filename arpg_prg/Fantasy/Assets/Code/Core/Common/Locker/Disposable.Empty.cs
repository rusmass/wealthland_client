using System;

namespace Core
{
	public class Disposable : ADisposable
	{
		protected override void _DoDispose (bool isDisposing)
		{

		}

		public static readonly Disposable Empty = new Disposable();
	}
}