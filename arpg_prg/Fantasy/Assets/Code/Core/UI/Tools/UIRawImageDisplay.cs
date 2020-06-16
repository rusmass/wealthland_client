using System;
using Core;
using Core.Web;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIRawImageDisplay : Disposable
	{
		public UIRawImageDisplay(RawImage rawImage)
		{
			if (null != rawImage)
			{
				_rawImage = rawImage;
				_rawImage.gameObject.SetActive(true);
			}
			else 
			{
				Console.Error.WriteLine("rawImage is null");
			}
		}

		protected override void _DoDispose (bool isDisposing)
		{
			_Clear ();
			base._DoDispose (isDisposing);
		}

		public IWebNode Load (string rawImagePath)
		{
			if (null == _rawImage)
			{
				return EmptyWebNode.Instance;
			}

			_rawImage.enabled = false;

			if (string.IsNullOrEmpty(rawImagePath))
			{
				return EmptyWebNode.Instance;
			}

			if (null != _webItem && _webItem.argument.localPath == rawImagePath) 
			{
				_OnLoadedWebItem (_webItem);
			} 
			else
			{
				os.dispose (ref _webItem);
				WebArgument arg = new WebArgument{localPath = rawImagePath, flags = WebFlags.HighPriority | WebFlags.DontCache | WebFlags.UnloadAllLoadedObjects};
				_webItem = WebManager.Instance.LoadWebItem (arg, _OnLoadedWebItem);
			}

			return _webItem;
		}

		private void _OnLoadedWebItem (WebItem web)
		{
			if (null == web.texture) 
			{
				Console.Error.WriteLine ("UIRawImageDisplay._OnLoadedWebItem error = {0}, localPath = {1}", web.error, web.argument.localPath);
			}

			_rawImage.texture = web.texture;
			_rawImage.enabled    = true;
		}

		private void _Clear ()
		{
			if (null != _rawImage)
			{
				_rawImage.texture = null;
				_rawImage.enabled = false;
			}

			os.dispose (ref _webItem);
		}

		private RawImage _rawImage;
		private WebItem _webItem;
	}
}

