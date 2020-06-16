using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using Core.Web;
using System.IO;

namespace Client.UI
{
	public partial class UISpecialeffectsWindow  
	{
		private void _OnInitEffects(GameObject go)
		{
			_headImagePerfab = go.GetComponentEx<Image>(Layout.HeadImagePerfab);
			_headTextPerfab  = go.GetComponentEx<Text>(Layout.HeadTextPerfab); 

			canvasScaler = go.GetComponent<CanvasScaler>();

			_headImagePerfab.SetActiveEx(false);
			_headTextPerfab.SetActiveEx(false);

			_headImagePerfab.sprite = null;

		}

		public void SetImagePos(Transform tra)
		{

			Vector3 pos = new Vector3 (WorldToUI(tra.position).x,WorldToUI(tra.position).y + 105 ,WorldToUI(tra.position).z);

			_headImagePerfab.rectTransform.localPosition = pos;

//			WebManager.Instance.LoadWebItem (imagePath, item => {
//				using (item)
//				{
//					_headImagePerfab.sprite = item.sprite;
//				}
//			});	
				
//			_headImagePerfab.SetActiveEx(true);
			LoadHead(imagePath);
		}

		private IWebNode LoadHead(string imagePath)
		{
			if (null == _headImagePerfab)
			{
				return EmptyWebNode.Instance;
			}

			_headImagePerfab.enabled = false;

			if (string.IsNullOrEmpty(imagePath))
			{
				return EmptyWebNode.Instance;
			}

			WebArgument arg = WebManager.Instance.GetWebArgument(imagePath);

			if (null != _webItemHead && _webItemHead.argument.assetName.Equals(arg.assetName))
			{
				Console.WriteLine("!!!!!!!!!!!!!!");
				_OnLoadedWebItemHead(_webItemHead);
			}
			else
			{
				_webItemHead = null;
				arg.flags = WebFlags.HighPriority | WebFlags.DontCache | WebFlags.UnloadAllLoadedObjects;
				_webItemHead = WebManager.Instance.LoadWebItem(arg, _OnLoadedWebItemHead);
			}

			return _webItemHead;
		}

		private void _OnLoadedWebItemHead (WebItem web)
		{
			if (null == web.sprite) 
			{
				Console.Error.WriteLine ("UIImageDisplay._OnLoadedWebItem error = {0}, localPath = {1}", web.error, web.argument.localPath);
			}

			_headImagePerfab.sprite = web.sprite;
			_headImagePerfab.enabled= true;
			_headImagePerfab.SetActiveEx(true);
		}

		private WebItem _webItemHead;

		public void updateImagePos(Transform tra)
		{

			Vector3 pos = new Vector3 (WorldToUI(tra.position).x,WorldToUI(tra.position).y + 105 ,WorldToUI(tra.position).z);
			_headImagePerfab.rectTransform.localPosition = pos;
		}


		/// <summary>
		/// 设置头顶上的文字出现位置
		/// </summary>
		/// <param name="tra">Tra.</param>
		public void SetTextPos(Transform tra,float strNum)
		{
			Vector3 pos = new Vector3 (WorldToUI(tra.position).x,WorldToUI(tra.position).y + 110 ,WorldToUI(tra.position).z);

			_headTextPerfab.rectTransform.localPosition = pos;

			_headTextPerfab.text = strNum.ToString();

			_headTextPerfab.SetActiveEx(true);
		}

		/// <summary>
		/// Updates the text position.
		/// </summary>
		/// <param name="tra">Tra.</param>
		public void updateTextPos(Transform tra)
		{
			Vector3 pos = new Vector3 (WorldToUI(tra.position).x,WorldToUI(tra.position).y + 110 ,WorldToUI(tra.position).z);

			_headTextPerfab.rectTransform.localPosition = pos;

			_headTextPerfab.SetActiveEx(true);
		}

		/// <summary>
		/// 设置头顶上的文字消失
		/// </summary>
		public void SetUpHeadTextDisappear()
		{
			_headTextPerfab.SetActiveEx(false);
			_controller.mUpdateImage = false;
		}

		/// <summary>
		/// 设置头顶上的图片消失
		/// </summary>
		public void setHeadImageDisappear()
		{
			_headImagePerfab.SetActiveEx(false);
		}

		/// <summary>
		/// 世界坐标转化屏幕坐标
		/// </summary>
		/// <returns>The to U.</returns>
		/// <param name="pos">Position.</param>
		private  Vector3 WorldToUI(Vector3 pos){  

			float resolutionX = canvasScaler.referenceResolution.x;  
			float resolutionY = canvasScaler.referenceResolution.y;  

			Vector3 viewportPos = Camera.main.WorldToViewportPoint(pos);  

			Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f,  
				viewportPos.y * resolutionY - resolutionY * 0.5f,0);  

			return uiPos;  
		}  

		private Image _headImagePerfab;
		private Text _headTextPerfab;
		public string imagePath;

		private CanvasScaler  canvasScaler;
	}
}
