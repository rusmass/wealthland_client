using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    /// <summary>
    /// 信用卡贷款界面
    /// </summary>
	public class UIBorrowWindowCard
	{
		public UIBorrowWindowCard (GameObject go, UIBorrowWindowController controller)
		{
			_selfObj = go;
			_controller = controller;
			_playerInfor = controller.playerInfor;
			lb_min = go.GetComponentEx<Text> (Layout.lb_min);
			lb_max = go.GetComponentEx<Text> (Layout.lb_max);
			lb_canborrow = go.GetComponentEx<Text> (Layout.lb_canmoney+Layout.number);
			lb_curborrow = go.GetComponentEx<Text> (Layout.lb_curborrw+Layout.number);
			lb_curdebt = go.GetComponentEx<Text> (Layout.lb_curdebt+Layout.number);
			lb_totalborrow = go.GetComponentEx<Text> (Layout.lb_totalborrow+Layout.number);
			lb_totaldebt = go.GetComponentEx<Text> (Layout.lb_totaldebt+Layout.number);
			_rangeSlider = go.GetComponentEx<Slider> (Layout.slider_range);

			_inputMoenyTxt = go.GetComponentEx<InputField> (Layout.lb_inputCard);
		}
        /// <summary>
        /// 初始化信用卡借贷界面
        /// </summary>
		public void OnShowBorrowCard()
		{

			var canBorrowMoney = 0f;

			if (GameModel.GetInstance.isPlayNet == false)
			{
				canborrow = _playerInfor.GetTotalBorrowCard ();
				totalborrow = _playerInfor.creditIncome;
				totaldebt = _playerInfor.creditDebt;

				canBorrowMoney = canborrow - totalborrow;
			}
			else
			{
				canborrow = _playerInfor.netBorrowBoardCardCanBorrow;
				totalborrow = _playerInfor.netBorrowBoardCardTotalBorrow;
				totaldebt = _playerInfor.netBorrowBoardCardTotalDebt;

				canBorrowMoney = canborrow;
			}


			_HandleNumLength (canborrow);

			lb_min.text = "0";
			lb_max.text = (canBorrowMoney).ToString ();
			lb_canborrow.text = (canBorrowMoney).ToString ();
			_rangeSlider.minValue = 0;
			_rangeSlider.maxValue = (canBorrowMoney)/_numLength;

			Console.WriteLine (string.Format("当前信用卡的步长是{0},滑竿最大的滑动值是{1}",_numLength.ToString(),_rangeSlider.maxValue.ToString()));

			_sliderCurrentValue=0;
			_rangeSlider.value = _sliderCurrentValue;
			_OnUpdateInfor ();
			_rangeSlider.onValueChanged.AddListener (_OnSliderValueChange);
			_inputMoenyTxt.onEndEdit.AddListener (_OnInputChange);
		}

        /// <summary>
        /// 更新人物信息
        /// </summary>
		private void _OnUpdateInfor()
		{
			lb_curborrow.text = curborrow.ToString ();
			lb_curdebt.text = curdebt.ToString ();
			lb_totalborrow.text = (curborrow + totalborrow).ToString ();
			lb_totaldebt.text = (curdebt + totaldebt).ToString ();

			_inputMoenyTxt.text = curborrow.ToString ();
		}

        /// <summary>
        /// 隐藏面板时，移除一些事件
        /// </summary>
		public void OnHideBorrowCard()
		{
			_rangeSlider.onValueChanged.RemoveListener (_OnSliderValueChange);
			_inputMoenyTxt.onEndEdit.RemoveListener (_OnInputChange);
		}

        /// <summary>
        /// 输入文本框
        /// </summary>
        /// <param name="value"></param>
		private void _OnInputChange(string value)
		{
			var _inputMoney = float.Parse (value);			

            if (GameModel.GetInstance.isPlayNet == false)
            {
                if (_inputMoney >= canborrow - totalborrow)
                {
                    _inputMoney = canborrow - totalborrow;
                    _inputMoenyTxt.text = _inputMoney.ToString();
                }
            }
            else
            {
                if (_inputMoney >= canborrow)
                {
                    _inputMoney = canborrow;
                    _inputMoenyTxt.text = _inputMoney.ToString();
                }
            }

            _rangeSlider.value = _inputMoney;


			if (null != IsSetVisible)
			{
				if (_inputMoney <= 0)
				{
					IsSetVisible (false);
				}
				else
				{
					IsSetVisible (true);
				}
			}
			curborrow =Mathf.FloorToInt((_inputMoney - _sliderCurrentValue) * _numLength);
			curdebt = Mathf.FloorToInt(curborrow * 0.03f);

			_controller.curborrowCard = curborrow;
			_controller.curcardDebt = curdebt;

			_OnUpdateInfor ();
		}

        /// <summary>
        /// 监听移动滑动条时的事件
        /// </summary>
        /// <param name="value"></param>
		private void _OnSliderValueChange(float value)
		{
			if (value <= _sliderCurrentValue)
			{
				value=_sliderCurrentValue;
				_rangeSlider.value = value;
			}

			if (null != IsSetVisible)
			{
				if (value <= 0)
				{
					IsSetVisible (false);
				}
				else
				{
					IsSetVisible (true);
				}
			}
			curborrow =Mathf.FloorToInt((value - _sliderCurrentValue) * _numLength);
			curdebt = Mathf.FloorToInt(curborrow * 0.03f);

			_controller.curborrowCard = curborrow;
			_controller.curcardDebt = curdebt;

			_OnUpdateInfor ();
		}
        /// <summary>
        /// 判断滑动条移动的步长，移动基本的长度代表的数值
        /// </summary>
        /// <param name="value"></param>
		private void _HandleNumLength(float value)
		{

			Console.WriteLine ("当前的信用卡借贷额是；"+value.ToString());

			if(value % 10000 ==0)
			{
				_numLength = 10000;
			}
			else if(value % 100==0)
			{
				_numLength = 100;
			}
			else if(value % 10==0)
			{
				_numLength = 10;
			}
			else 
			{
				_numLength = 1;
			}
			_numLength = 1;
			Console.WriteLine ("当前的信用卡借贷步长是；"+_numLength.ToString());
		}

        /// <summary>
        /// 设置面板是否可见
        /// </summary>
        /// <param name="value"></param>
		public void SetActiveEx(bool value)
		{
			_selfObj.SetActiveEx (value);
		}

		private GameObject _selfObj;
		private UIBorrowWindowController _controller;
		private Slider _rangeSlider;
		private Text lb_max;
		private Text lb_min;
		private Text lb_canborrow;
		private Text lb_curborrow;
		private Text lb_curdebt;
		private Text lb_totalborrow;
		private Text lb_totaldebt;

		private int curborrow=0;
		private int curdebt=0;

		private float canborrow;
		private float totalborrow;
		private float totaldebt;

		private float _sliderCurrentValue;
		private InputField _inputMoenyTxt;

		private int _numLength=1;

		private PlayerInfo _playerInfor;

		public IsSureVisible IsSetVisible;

		public delegate void IsSureVisible(bool value);

		class Layout
		{
			public static string lb_max="maxtxt";
			public static string lb_min="mintxt";
			public static string lb_canmoney="canmoney";
			public static string lb_curborrw="curborrow";
			public static string lb_curdebt="curdebet";
			public static string lb_totalborrow="totalborrow";
			public static string lb_totaldebt="totaldebet";
			public static string number="/number";
			public static string slider_range="bankrange";

			public static string lb_inputCard="inputcard";

		}

	}
}

