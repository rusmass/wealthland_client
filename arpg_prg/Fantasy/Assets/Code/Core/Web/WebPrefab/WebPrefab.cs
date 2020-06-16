using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Collections;
using Object = UnityEngine.Object;

namespace Core.Web
{
	public class WebPrefab : ADisposable, IWebNode, IIsYieldable
	{
		internal WebPrefab (WebArgument argument, Action<WebPrefab> handler)
		{
			this.argument = argument;
			var localPath = argument.localPath;

			InnerWebPrefab inner;
			if (!_innerPrefabs.TryGetValue(localPath, out inner))
			{
				inner	= new InnerWebPrefab(argument);
				_innerPrefabs.Add(localPath, inner);
			}

			_size		= inner.size;
			_inner      = inner;
			_inner.AddReference();

			_loadingInnerRoutine = CoroutineManager.StartCoroutine(_CoLoadInnerWebPrefab(handler));
		}

		protected override void _DoDispose (bool isDisposing)
		{
			WebTools.RemoveFromCache (_innerPrefabs, ref _inner);
		}

		private IEnumerator _CoLoadInnerWebPrefab (Action<WebPrefab> handler)
		{
			var inner = _inner;
			if (!inner.isDone) 
			{
				yield return inner;	
			}

			_loadingInnerRoutine = null;
			_nodeState.isLoaded = true;
			error		= inner.error;

			CallbackTools.Handle(ref handler, this, "[WebPrefab:_CoLoadInnerWebPrefab()]");
			_nodeState.isHandled = true;
		}

		public void kill() 
		{
			if (!_nodeState.isKilled)
			{
				_nodeState.isKilled = true;
				CoroutineManager.KillCoroutine(ref _loadingInnerRoutine);
			}
		}

		internal static LruCache<string, InnerWebPrefab> _GetLruCache ()
		{
			return _innerPrefabs;
		}

		private WebNodeState _nodeState;

		public WebArgument argument     { get; private set; }

		public GameObject mainAsset     	{ get { return _inner.mainAsset; } }

		public string error             { get; private set; }

		public bool isDone	            { get { return _nodeState.isLoaded; } }

		public bool isKilled			{ get { return _nodeState.isKilled; } }

		bool IIsYieldable.isYieldable   { get { return _nodeState.isLoaded || _nodeState.isKilled || IsDisposed(); } }

		bool IWebNode.isUseless     	{ get { return _nodeState.isHandled || _nodeState.isKilled || IsDisposed(); } }

		public float progress           { get { return isDone || null == _inner ? 1.0f : _inner.progress ; } }

		long IWebNode.size       		{ get { return _size; } }

		public object userdata;

		internal InnerWebPrefab  _inner;
		
		private long			_size;

		private IEnumerator _loadingInnerRoutine;

		private static readonly LruCache<string, InnerWebPrefab> _innerPrefabs = new LruCache<string, InnerWebPrefab>(32);
	}
}