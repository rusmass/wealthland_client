using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Client.UI
{
	public partial class UIBattleWindow
	{
		private void _OnInitRuntip(GameObject go)
		{
			img_rolltip = go.GetComponentEx<Image> (Layout.img_rolltip);
			lb_runtip = go.GetComponentEx<Text> (Layout.lb_runtip);
		}

		private void _OnShowRuntip()
		{
			_maskWindth = 394f;
			var tmpPosition = lb_runtip.transform.localPosition;
			_initTextPosition = new Vector3 (_maskWindth+10,tmpPosition.y,tmpPosition.z);
			img_rolltip.SetActiveEx (false);
			_isShowTip = false;

		}

		private void _OnHideRuntip()
		{
			if (null != _runTipList)
			{
				_runTipList.Clear ();
				_runTipList = null;
			}
		}

		private void _OnTickRunning(float deltatime)
		{
			if (_isShowTip == true)
			{
				var tmpPosition = lb_runtip.transform.localPosition;

				lb_runtip.transform.localPosition = new Vector3 (tmpPosition.x-_runningSpeed*deltatime,tmpPosition.y,tmpPosition.z);

				if (lb_runtip.transform.localPosition.x < -_tmpTipWidth - 20)
				{
					_runTipList.RemoveAt (0);

					if (_runTipList.Count > 0)
					{
						_StartMoveTxt ();
					}
					else
					{						
						img_rolltip.SetActiveEx(false);
						_isShowTip = false;
					}

				}
			}

		}


		public void ShowRunTip (string value)
		{
			_runTipList.Add (value);

			if (_isShowTip == false)
			{
				_StartMoveTxt ();
			}

		}

		private void _StartMoveTxt()
		{
			img_rolltip.SetActiveEx(true);
			lb_runtip.text =_runTipList[0];
			_tmpTipWidth = lb_runtip.preferredWidth;

			lb_runtip.transform.localPosition = _initTextPosition;
			_isShowTip = true;
		}

		private bool _isShowTip=false;


		private float _runningSpeed=60;

		private List<string> _runTipList = new List<string> ();

		private Image img_rolltip;
		private Text lb_runtip;
		private float _maskWindth;

		private Vector3 _initTextPosition;

		private float _tmpTipWidth;
	}
}

