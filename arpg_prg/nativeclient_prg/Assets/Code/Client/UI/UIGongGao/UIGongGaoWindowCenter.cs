using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



namespace Client.UI
{
	public partial class UIGongGaoWindow
	{
		private void _InitCenter(GameObject go)
		{
			btn_close = go.GetComponentEx<Button> (Layout.btn_close);
			btn_gonggao = go.GetComponentEx<Button> (Layout.btn_title);

			lb_gongtitle = go.GetComponentEx<Text> (Layout.txt_title);
			lb_gongcontent = go.GetComponentEx<Text> (Layout.txt_content);

            btn_wordTip = go.GetComponentEx<Button>(Layout.btn_join);

            img_gonggao = go.GetComponentEx<Image> (Layout.img_gonggao);
		}

		private void _ShowCenter()
		{
			img_gonggao.SetActiveEx (false);
			//btn_close.SetActiveEx (false);
			EventTriggerListener.Get (this.btn_close.gameObject).onClick += onCloseHandler;
            EventTriggerListener.Get(this.btn_wordTip.gameObject).onClick += _OnJumpToWeb;
            EventTriggerListener.Get(this.img_gonggao.gameObject).onClick += _OnJumpToWeb;

			lb_gongtitle.text = "";
			lb_gongcontent.text = "";

			for (var i = 0; i < _controller.inforList.Count; i++)
			{
				Button tmpBtn;
				if (i == 0)
				{
					tmpBtn = this.btn_gonggao;
					tmpBtn.name = "btn" + i;
				}
				else
				{
					tmpBtn = this.btn_gonggao;		
					tmpBtn =btn_gonggao.gameObject.CloneEx().GetComponent<Button>()  ;
					//tmpBtn =(Button) _btnServer.transform CloneEx(false)  ;
					tmpBtn.transform.SetParent (btn_gonggao.transform.parent);
					tmpBtn.transform.position = btn_gonggao.transform.position;
					tmpBtn.transform.localScale = Vector3.one;
					tmpBtn.transform.rotation =btn_gonggao.transform.rotation;
					tmpBtn.name = "btn"+i;
				}

				EventTriggerListener.Get (tmpBtn.gameObject).onClick += _OnSelectTitleHandler;
				var tmpStr = _controller.inforList [i].title;
				if(tmpStr.Length>6)
				{
					tmpStr = tmpStr.Substring (0, 6);
					tmpStr+="...";
				}
				tmpBtn.gameObject.GetComponentEx<Text> ("lb_txt").text =tmpStr;
				btnTitleList.Add (tmpBtn);
			}

			if (_controller.inforList.Count <= 0)
			{
				btn_gonggao.SetActiveEx (false);
				lb_gongtitle.SetActiveEx (false);
				lb_gongcontent.SetActiveEx (false);
			}
			else
			{
				_ShowTipByIndex (0);
			}
			GameModel.GetInstance.isFirstInGameHall = false;
		}

		private void _HideCenter()
		{
			EventTriggerListener.Get (this.btn_close.gameObject).onClick -= onCloseHandler;
            EventTriggerListener.Get(this.btn_wordTip.gameObject).onClick -= _OnJumpToWeb;
            EventTriggerListener.Get(this.img_gonggao.gameObject).onClick -= _OnJumpToWeb;
            for (var i = 0; i < btnTitleList.Count; i++)
			{
				var tmpBtn = btnTitleList[i];
				EventTriggerListener.Get (tmpBtn.gameObject).onClick -= _OnSelectTitleHandler;
			}
			GameModel.GetInstance.isFirstInGameHall = false;

		}

		private void _DisposeCenter()
		{
            Resources.UnloadUnusedAssets();
        }

		private void _OnSelectTitleHandler(GameObject go)
		{
			var tmpIndex =int.Parse(go.name.Substring(3));
			_ShowTipByIndex (tmpIndex);
		}
        /// <summary>
        /// 设置按钮列表选择状态
        /// </summary>
        /// <param name="index"></param>
		private void _SetSelectButtonByIndex(int index)
		{
			for (var i = 0; i < btnTitleList.Count; i++)
			{
				if (i == index)
				{
					_SetInitColor (btnTitleList[i].gameObject);
				}
				else
				{
					_SetGrayColor (btnTitleList[i].gameObject);
				}
			}
		}

        /// <summary>
        /// 刷新显示公告内容
        /// </summary>
        /// <param name="value"></param>
		private void _ShowTipByIndex(int value)
		{
			_SetSelectButtonByIndex (value);			
			//var gonggaovalue =_controller.inforList[value];
            this._tmpVo = _controller.inforList[value];
			bool isUrl = false;
			if (_tmpVo.type == 1)
			{
				isUrl = true;
			}

			if (isUrl == false)
			{
				_SetGonggaoTip (_tmpVo.title, _tmpVo.content);
			}
			else
			{
				_SetGonggaoImg (_tmpVo.content);
			}
		}

        /// <summary>
        /// 判断是否可以跳转链接
        /// </summary>
        /// <returns></returns>
        private bool _IsUrl()
        {
            if(null!=_tmpVo)
            {
                if(_tmpVo.isUrl==true &&_tmpVo.targetUrl!=""&&_tmpVo.targetUrl!=null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 显示文字公告内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
		private void _SetGonggaoTip(string  title , string content)
		{
			lb_gongtitle.text = title;
			lb_gongcontent.text = content;
			lb_gongtitle.SetActiveEx (true);
			lb_gongcontent.SetActiveEx (true);
			img_gonggao.SetActiveEx (false);

            btn_wordTip.SetActiveEx(_IsUrl());
		}

        /// <summary>
        /// 打开网页链接
        /// </summary>
        /// <param name="value"></param>
        private void _ShowNativeWeb(string value)
        {
            ///"http://t.asdyf.com/"
            var webController = UIControllerManager.Instance.GetController<UINativeWebController>();
            webController.SetTargetUrl(value);
            webController.setVisible(true);
        }

        /// <summary>
        /// 显示图片公告的界面
        /// </summary>
        /// <param name="loadPath"></param>
		private void _SetGonggaoImg (string loadPath)
		{
            btn_wordTip.SetActiveEx(false);

			img_gonggao.SetActiveEx (true);
			AsyncImageDownload.Instance.SetAsyncImage (loadPath,img_gonggao);
			//img_gonggao.SetNativeSize ();
			//img_gonggao.texture= LoadRawImg.CUTPicture2 (loadPath);
			lb_gongtitle.SetActiveEx (false);
			lb_gongcontent.SetActiveEx (false);
		}

        /// <summary>
        /// 点击跳转网页功能
        /// </summary>
        /// <param name="go"></param>
        private void _OnJumpToWeb(GameObject go)
        {
            if(null!=_tmpVo)
            {
                if(_IsUrl())
                {
                    _ShowNativeWeb(_tmpVo.targetUrl);
                }
            }
        }


        private void onCloseHandler(GameObject go)
		{
			Console.WriteLine ("sssssssssssssssssssssssss");
			_controller.setVisible (false);
		}

		/// <summary>
		/// Sets the color of the gray.设置灰色未选中状态
		/// </summary>
		/// <param name="go">Go.</param>
		private void _SetGrayColor(GameObject go)
		{
			go.GetComponent<Image> ().color = btnbgColor;
			go.GetComponentEx<Text> ("lb_txt").color = imgbgColor;
		}

		/// <summary>
		/// Sets the color of the init. 设置选中状态
		/// </summary>
		/// <param name="go">Go.</param>
		private void _SetInitColor(GameObject go)
		{
			go.GetComponent<Image> ().color = initColor;
			go.GetComponentEx<Text> ("lb_txt").color = initColor;
		}

        /// <summary>
        /// 关闭按钮
        /// </summary>
		private Button btn_close;
        /// <summary>
        /// 左侧列表按钮
        /// </summary>
		private Button btn_gonggao;

        /// <summary>
        /// 公告标题
        /// </summary>
		private Text lb_gongtitle;
        /// <summary>
        /// 公告的文字内容
        /// </summary>
		private Text lb_gongcontent;

        /// <summary>
        /// 图片公告内容
        /// </summary>
		private Image img_gonggao;
        /// <summary>
        /// 当前的公告数据
        /// </summary>
        private GonggaoVo _tmpVo = null;
        /// <summary>
        /// 文字公告中跳转链接
        /// </summary>
        private Button btn_wordTip;

        private Color btnbgColor = new Color (116f/255,116f/255,116f/255,1f);
		private Color imgbgColor = new Color (180f/255,180f/255,180f/255,1f);
		private Color initColor = new Color (255f/255,255f/255,255f/255,1f);
        
        /// <summary>
        /// 左侧公告按钮列表
        /// </summary>
		private List<Button> btnTitleList = new List<Button> ();

       
	}
}

