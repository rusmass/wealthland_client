using System;
using UnityEngine;

namespace Core.Web
{
	internal class RemoteBundleCacheItem : ABundleCacheItem
	{
		public RemoteBundleCacheItem (string localPath, WebType webType) : base (localPath)
		{
			_www = _CreateWWW (webType);
			if (null != _www) 
			{
				_stage = ProgressStage.Loading;
			}
		}

		protected override void _DoDispose (bool isDisposing)
		{
			os.dispose (ref _www);
			base._DoDispose (isDisposing);
		}

		public override string url 
		{
			get 
			{
				var info = WebManager.Instance.GetMappingInfo(localPath);
				return info.GetUrl();
			}
		}

		public override AssetBundle assetbundle 
		{
			get 
			{
				//这里有个问题，当assetbundle已经LoadAllAssets后，若后面继续执行此操作
				//则在手机上会导致显示问题（贴图等泛白等...）
				if (null == _assetbundle && null != _www && _www.isDone)
				{
					_assetbundle = _www.assetBundle;

					if (null == _assetbundle)
					{
						Console.Error.WriteLine("[RemoteBundlCacheItem.assetbundle is null, url = {0}]", _www.url);
					}

					_stage = ProgressStage.Done;
					//we tend to decrease the count of www objects, to decrease the crash possibilty on android platform.
					os.dispose(ref _www);
				}

				return _assetbundle;
			}
		}

		public override bool isDone 
		{
			get 
			{
				return _www == null ? true : _www.isDone;
			}
		}

		public override float  progress
		{
			get
			{
				if (_stage == ProgressStage.None)
				{
					return 0.0f;
				}
				else if (_stage == ProgressStage.Loading)
				{
					return _www.progress;
				}
				else
				{
					return 1.0f;
				}
			}
		}

		public override string error
		{
			get 
			{ 
				if (_stage == ProgressStage.None || _stage == ProgressStage.Done) 
				{
					_error = "_www is null";
				} else if (_stage == ProgressStage.Loading) 
				{
					_error = _www.error;
				}
				return _error;
			}
		}

		private WWW _www;
		private string _error;
		private ProgressStage _stage;
	}
}
