using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    /// <summary>
    /// 显示星级评分的话术
    /// </summary>
	public class UIConclusionStarTip
	{
		public UIConclusionStarTip (GameObject go)
		{
			_tipTxt = go.GetComponentEx<Text> (Layout.lb_tip);

			for (var i = 0; i < _maxTitleNum; i++)
			{
				var tmpTxt = go.GetComponentEx<Text> (string.Format(Layout.lb_title,(i+1).ToString()));
				_titleArr [i] = tmpTxt;
			}

			for (var j = 0; j < _maxTitleNum; j++)
			{
				var tmpNumTxt = go.GetComponentEx<Text> (string.Format(Layout.lb_num,(j+1).ToString()));
				_numArr [j] = tmpNumTxt;
			}

			var tmpImg = go.GetComponentEx<Image> (Layout.img_title);
			_titleImg = new UIImageDisplay (tmpImg);

			_selfObj = go;
		}


		public void ShowBoardTip(string[] _titltString,int[] _numberString,string _tip,string loadPath)
		{
			_selfObj.SetActiveEx (true);
			_tipTxt.text = _tip;

			var tmpLen = _titltString.Length;

			for (var i = 0; i < _maxTitleNum; i++)
			{
				if (i < tmpLen)
				{
					var tmptxt = _titleArr [i];

					tmptxt.SetActiveEx (true);
					tmptxt.text = _titltString [i];

					var tmpx = tmptxt.preferredWidth;

					if (tmpx >= _maxTxtWidth)
					{
						tmpx = _maxTxtWidth;
					}

					var numTxt = _numArr [i];

					var tmpPosition=numTxt.transform.localPosition;
					numTxt.transform.localPosition=new Vector3(tmpx+5,tmpPosition.y,tmpPosition.z);
					numTxt.text=_numberString[i].ToString();
				}
				else
				{
					_titleArr [i].SetActiveEx (false);
				}
			}

			if (null != _titleImg)
			{
				_titleImg.Load (loadPath);
			}

		}

		public void HideBoardTip()
		{
			_selfObj.SetActiveEx (false);
		}


		public void Dispose()
		{
			if(null !=_titleImg)
			{
				_titleImg.Dispose ();
			}
		}

		private const int _maxTitleNum= 6;
		private const float _maxTxtWidth=114f;
		private Text[] _titleArr=new Text[6];
		private Text[] _numArr=new Text[6];
		private Text _tipTxt;
		private UIImageDisplay _titleImg;
		private GameObject _selfObj;

		class Layout
		{
			public static string lb_title="scoretxt{0}";
			public static string lb_num="numtxt{0}";
			public static string lb_tip="txt_rated";
			public static string img_title="titleimg";
		}

	}


}

