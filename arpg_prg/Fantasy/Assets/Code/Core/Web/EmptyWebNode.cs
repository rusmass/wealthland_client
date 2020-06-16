using System;

namespace Core.Web
{
	public class EmptyWebNode : IWebNode, IIsYieldable, IDisposable
    {
		private EmptyWebNode ()
		{

		}

		public void Dispose ()
		{

		}

		public void kill ()
		{

		}

		public bool isDone			{ get { return true; } }

		public bool isKilled		{ get { return false; } }

		bool IWebNode.isUseless   	{ get { return true; } }

		public float progress 		{ get { return 1.0f; } }

		public long size      		{ get { return 1; } }

		bool IIsYieldable.isYieldable	{ get { return true; } }

		public static readonly EmptyWebNode Instance = new EmptyWebNode();
    }
}