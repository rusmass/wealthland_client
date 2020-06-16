using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    /// <summary>
    /// 银行贷款面板
    /// </summary>
	public class UIBorrowWindowBank
	{
		
		public UIBorrowWindowBank (GameObject go, UIBorrowWindowController controller)
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
			lb_banktip = go.GetComponentEx<Text> (Layout.lb_tip);

			_inputMoenyTxt = go.GetComponentEx<InputField> (Layout.lb_inputBank);
		}

        /// <summary>
        /// 在显示银行贷款时，初始化的信息
        /// </summary>
		public void OnShowBorrowBank()
		{
            if (GameGuidManager.GetInstance.DoneGameBorrow == false)
            {
                GameGuidManager.GetInstance.ShowBorrowGuid();
            }

            if (GameModel.GetInstance.isPlayNet == false)
			{
				canborrow = _playerInfor.GetTotalBorrowBank ();
				totalborrow = _playerInfor.bankIncome;
				totaldebt = _playerInfor.bankLoans;

			}
			else
			{
				canborrow = _playerInfor.netBorrowBoardBankCanBorrow;
				totalborrow = _playerInfor.netBorrowBoardBankTotalBorrow;
				totaldebt = _playerInfor.netBorrowBoradBankTotalDebt;
			}

			_HandlerNumLength (canborrow);

			lb_min.text = "0";

			var canBorrowMoney = 0f;
			if (GameModel.GetInstance.isPlayNet == false)
			{
				canBorrowMoney = canborrow - totalborrow;
			}
			else
			{
				canBorrowMoney = canborrow;
			}
			lb_max.text =HandleStringTool.HandleMoneyTostring(canBorrowMoney);
			lb_canborrow.text =HandleStringTool.HandleMoneyTostring(canBorrowMoney);
			_rangeSlider.minValue = 0;
			_rangeSlider.maxValue = (canBorrowMoney)/_numLength;
			_sliderCurrentValue=0;
			_rangeSlider.value = _sliderCurrentValue;
			_OnUpdateInfor ();
			_rangeSlider.onValueChanged.AddListener (_OnSliderValueChange);
			_inputMoenyTxt.onEndEdit.AddListener (_OnInputChange);
		}

		public void MoveDownY(float value)
		{
			var tmpVec = _selfObj.transform.localPosition;
			_selfObj.transform.localPosition = new Vector3 (tmpVec.x,tmpVec.y-value,tmpVec.z);

			if (null != _playerInfor)
			{
				if (_playerInfor.isEnterInner == true)
				{
					lb_banktip.text ="贷款上限为结算金额的100倍，利率为1%，利息在结账日扣除";
				}
			}
		}

        /// <summary>
        /// 增加玩家的借贷信息
        /// </summary>
		private void _OnUpdateInfor()
		{
			lb_curborrow.text = HandleStringTool.HandleMoneyTostring (curborrow);
			lb_curdebt.text = HandleStringTool.HandleMoneyTostring (curdebt);
			lb_totalborrow.text = HandleStringTool.HandleMoneyTostring (curborrow + totalborrow);
			lb_totaldebt.text = HandleStringTool.HandleMoneyTostring (curdebt + totaldebt);

			_inputMoenyTxt.text = curborrow.ToString ();
		}

        /// <summary>
        /// 隐藏银行贷款面板时移除事件
        /// </summary>
		public void OnHideBorrowBank()
		{
			_rangeSlider.onValueChanged.RemoveListener (_OnSliderValueChange);
			_inputMoenyTxt.onEndEdit.RemoveListener (_OnInputChange);
		}

        /// <summary>
        /// 监听输入文本框事件
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


			if (null != IsVisible)
			{
				if (_rangeSlider.value <= 0)
				{
					IsVisible (false);
				}
				else
				{
					IsVisible (true);
				}
			}

			curborrow =Mathf.FloorToInt((_rangeSlider.value - _sliderCurrentValue) * _numLength );

			var tmpRate = 0.1f;

			if (_playerInfor.isEnterInner == true)
			{
				tmpRate = 0.01f;
			}

			curdebt = Mathf.FloorToInt (curborrow * tmpRate);

			_controller.curborrowBank = curborrow;
			_controller.curbankDebt = curdebt;

			_OnUpdateInfor ();
		}

        /// <summary>
        /// 监听移动拖动条
        /// </summary>
        /// <param name="value"></param>
		private void _OnSliderValueChange(float value)
		{
			Debug.LogWarning ("当前的变化值是多少,,,"+value.ToString());

			if (value <= _sliderCurrentValue)
			{
				value=_sliderCurrentValue;
				_rangeSlider.value = value;
			}

			if (null != IsVisible)
			{
				if (value <= 0)
				{
					IsVisible (false);
				}
				else
				{
					IsVisible (true);
				}
			}

			curborrow =Mathf.FloorToInt((_rangeSlider.value - _sliderCurrentValue) * _numLength );

			var tmpRate = 0.1f;

			if (_playerInfor.isEnterInner == true)
			{
				tmpRate = 0.01f;
			}

			curdebt = Mathf.FloorToInt (curborrow * tmpRate);

			_controller.curborrowBank = curborrow;
			_controller.curbankDebt = curdebt;

			_OnUpdateInfor ();

		}

        /// <summary>
        /// 计算滑竿每次移动的步长，移动一个点代表多少钱
        /// </summary>
        /// <param name="value"></param>
        private void _HandlerNumLength(float value)
		{
			if(value % 100000 ==0)
			{
				_numLength = 100000;
				
			}else if(value % 10000 ==0)
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

			_numLength=1;
		}

		/// <summary>
		/// Sets the active ex.设置可见不可见
		/// </summary>
		/// <param name="value">If set to <c>true</c> value.</param>
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
		private Text lb_banktip;

		private int curborrow=0;
		private int curdebt=0;

		private float canborrow;
		private float totalborrow;
		private float totaldebt;

		private float _sliderCurrentValue;

		// 每次滑竿滑动的步长
		private int _numLength=1;

		private InputField _inputMoenyTxt;

		private PlayerInfo _playerInfor;

		public IsVisibleCallBack IsVisible;

		public delegate void IsVisibleCallBack(bool value);


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
			public static string lb_tip="banktip";

			public static string lb_inputBank="inputbank";
				
		}
	}
}

