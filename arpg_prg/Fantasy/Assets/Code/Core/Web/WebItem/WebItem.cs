using System;
using UnityEngine;
using System.Collections;
using Core.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Core.Web
{
	public partial class WebItem : ADisposable, IWebNode, IIsYieldable
	{
		internal WebItem(WebArgument argument, Action<WebItem> handler)
		{
			this.argument = argument;
			this._handler = handler;

			if (os.isEditor) 
			{
				_AssertLocalPath ();	
			}

			var localPath = argument.localPath;
			var info = WebManager.Instance.GetMappingInfo (localPath);
			if (null == info.localPathWithDigest) 
			{
				Console.Warning.WriteLine("[WebItem.ctor] localPathWithDigest == null, localPath = {0}", localPath);
			}

			size = info.totalSize;
			_IntendWebItem ();
		}

		protected override void _DoDispose (bool isDisposing)
		{
			if (null != _cacheItem) 
			{
				WebTools.RemoveFromCache(_cacheItems, ref _cacheItem);
			}
		}

		internal void HoldBabies(ref IWebNode[] nodes)
		{
			if (null != _cacheItem) 
			{
				_cacheItem.HoldBabies (ref nodes);
			}
			else 
			{
				Console.Error.WriteLine("[WebItem.HoldBabies] _cacheItem is null, localPath = {0}", argument.localPath);
			}
		}

		public void kill() 
		{
			_nodeState.isKilled = true;
			_handler = null;
		}

		public override string ToString ()
		{
			return string.Format("[WebItem: localPath={0}, isDone={1}, progress={2}, isKilled={3}, _cacheItem={4}, _handler={5}]"
			                     , argument.localPath
			                     , isDone.ToString()
			                     , progress.ToString()
			                     , isKilled.ToString()
			                     , _cacheItem
			                     , _handler);
		}

		private void _IntendWebItem()
		{
			ACacheItem cacheItem;
			var localPath = argument.localPath;
			if (_cacheItems.TryGetValue (localPath, out cacheItem) && cacheItem.isDone) 
			{
				_SetCacheItem (ref cacheItem);
				_HandCallBack ();
			}
			else 
			{
				_Loading(this);
			}
		}

		private void _HandCallBack()
		{
			//这里判断_nodeState.isLoaded是因为有可能多个prefab引用了多份相同的资源，当异步加载时，由于某一个prefab已经开始加载此
			//资源，但是还未加载完成，此时另外一份加载此资源时，由于已经缓存此资源到_cacheItems中，所以另一份资源会直接调用_HandCallBack
			//这样是不对的，应该等待第一份加载完成就可以了
			if (_nodeState.isLoaded && (null == _cacheItem || !_cacheItem.isDone || IsDisposed()))
			{
				Console.Error.WriteLine("_cacheItem={0}, IsDisposed()={1}, this={2}", _cacheItem, IsDisposed(), this);
				return;
			}

			//不能通过回调将WebItem Dispose, 否则会引起系统错误
			CallbackTools.Handle (ref _handler, this, "[WebItem:_HandleCallBack]");

			_nodeState.isHandled = true;
		}

		private void _SetCacheItem(ref ACacheItem cacheItem)
		{
            _cacheItem = cacheItem;
            _cacheItem.AddReference();
        }

		private void _AssertLocalPath()
		{
			var localPath = argument.localPath;
			if (localPath != localPath.ToLower ()) 
			{
				Console.Error.WriteLine ("[WebItem.AssertPath()] localPath contains up char, localPath = {0}", localPath);
			}

			if (localPath.Contains ("\\")) 
			{
				Console.Error.WriteLine ("[WebItem.AssertPath()] localPath contains \\, please replace \\ by /, localPath = {0}", localPath);
			}
		}

		public float progress
		{
			get
			{
				if (null != _cacheItem)
				{
					return _cacheItem.progress;
				}

				if (_nodeState.isLoaded)
				{
					return 1.0f;
				}

				return 0.0f;
			}
		}

		public AssetBundle assetBundle { get { return null != _cacheItem ? _cacheItem.assetbundle : null; } }
		public Texture2D texture { get { return null != _cacheItem ? _cacheItem.texture : null; } }
		public Sprite sprite { get { return null != _cacheItem ? _cacheItem.GetSprite(argument.assetName) : null; } }
		public AudioClip audioClip { get {return null != _cacheItem ? _cacheItem.audioClip : null;}}

		public byte[] bytes { get { return null != _cacheItem ? _cacheItem.bytes : Constants.EmptyBytes; } }
		public string text { get { return null != _cacheItem ? _cacheItem.text : string.Empty; } }
		public string error { get { return null != _cacheItem ?  _cacheItem.error : string.Empty; } }
		public bool isKilled { get { return _nodeState.isKilled; } }
		public bool isDone { get { return ( null != _cacheItem && _cacheItem.isDone ) || _nodeState.isLoaded; } }
		public long size { get; private set; }
		public WebArgument argument { get; private set; }
		bool IIsYieldable.isYieldable { get { return isDone || isKilled || IsDisposed(); } }
		bool IWebNode.isUseless { get { return _nodeState.isHandled || isKilled || IsDisposed(); } }

		private ACacheItem _cacheItem;
		private Action<WebItem> _handler;
		private WebNodeState _nodeState;

		private static readonly LruCache<string, ACacheItem> _cacheItems = new LruCache<string, ACacheItem>(512);
	}
}