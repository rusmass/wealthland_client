using Core;
using Core.Web;
using UnityEngine.UI;
using System.IO;

namespace Client.UI
{
	public class UIImageDisplay : Disposable
	{
		public UIImageDisplay (Image image)
		{
			if (null != image)
			{
				_image = image;
				_image.gameObject.SetActive(true);
			}
			else 
			{
				Console.Error.WriteLine("image is null");
			}
		}

		protected override void _DoDispose (bool isDisposing)
		{
			_Clear();
			base._DoDispose (isDisposing);
		}

        //public IWebNode Load (string imagePath)
        //{
        //	if (null == _image)
        //	{
        //		return EmptyWebNode.Instance;
        //	}

        //	_image.enabled = false;

        //	if (string.IsNullOrEmpty(imagePath))
        //	{
        //		return EmptyWebNode.Instance;
        //	}

        //	if (null != _webItem && _webItem.argument.localPath == imagePath) 
        //	{
        //		Console.WriteLine ("!!!!!!!!!!!!!!");
        //		_OnLoadedWebItem (_webItem);
        //	} 
        //	else
        //	{
        //		os.dispose (ref _webItem);
        //		WebArgument arg = new WebArgument{localPath = imagePath, flags = WebFlags.HighPriority | WebFlags.DontCache | WebFlags.UnloadAllLoadedObjects};
        //		_webItem = WebManager.Instance.LoadWebItem (arg, _OnLoadedWebItem);
        //	}

        //	return _webItem;
        //}
        public IWebNode Load(string imagePath)
        {
            if (null == _image)
            {
                return EmptyWebNode.Instance;
            }

            _image.enabled = false;

            if (string.IsNullOrEmpty(imagePath))
            {
                return EmptyWebNode.Instance;
            }

            WebArgument arg = WebManager.Instance.GetWebArgument(imagePath);

            if (null != _webItem && _webItem.argument.assetName.Equals(arg.assetName))
            {
                Console.WriteLine("!!!!!!!!!!!!!!");
                _OnLoadedWebItem(_webItem);
            }
            else
            {
                os.dispose(ref _webItem);
			    arg.flags = WebFlags.HighPriority | WebFlags.DontCache | WebFlags.UnloadAllLoadedObjects;
                _webItem = WebManager.Instance.LoadWebItem(arg, _OnLoadedWebItem);
            }

            return _webItem;
        }

        private void _OnLoadedWebItem (WebItem web)
		{
			if (null == web.sprite) 
			{
				Console.Error.WriteLine ("UIImageDisplay._OnLoadedWebItem error = {0}, localPath = {1}", web.error, web.argument.localPath);
			}

			_image.sprite = web.sprite;
			_image.enabled    = true;
		}

		private void _Clear ()
		{
			if (null != _image)
			{
				_image.sprite = null;
				_image.enabled = false;
			}

			os.dispose (ref _webItem);
		}

		public void SetActive(bool active)
		{
			if (null != _image && null != _image.gameObject) 
			{
				_image.gameObject.SetActiveEx (active);
			}
		}

		private Image _image;
		private WebItem _webItem;
	}
}

