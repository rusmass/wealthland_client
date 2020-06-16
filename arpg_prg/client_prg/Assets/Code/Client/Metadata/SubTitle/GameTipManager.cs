using System;
using Metadata;
namespace Client
{
	public class GameTipManager
	{
        /// <summary>
        /// 话术管理
        /// </summary>
		private GameTipManager()
		{
			_gameTips = ConfigManager.Instance.GetConfig<GameTips> ();

			_starTips = ConfigManager.Instance.GetConfig<ConclusionStarTip> ();

			_InitInnerArr ();
			_InitOldArr ();
			_InitYoungArr ();
			_InitMoreOldArr ();
		}

        /// <summary>
        /// 返回游戏中提示的话术
        /// </summary>
        public GameTips gameTips
        {
            get
            {
                return this._gameTips;
            }
        }

		/// <summary>
		/// Gets the young outer tip. 31岁之前的提示
		/// </summary>
		/// <returns>The young outer tip.</returns>
		public string GetYoungOuterTip()
		{
			// 还有两个时就是遍历选择 ， 否则是随机 ， 随机的时候，如果和最后的一个数一样，再重新随机

			int value = 0;

			if (_youngTipNum - _currentYoungIndex <= 2)
			{
				for (var i = _youngTipArr.Length-1; i >=0 ; i--)
				{
					if (_youngTipArr [i] != 1)
					{
						_youngTipArr [i] = 1;
						value = i+1;
						break;
					}
				}
			}
			else
			{
				value = MathUtility.Random (1,_youngTipNum);
				if (_currentYoungIndex == 0)
				{
					// 如果是第一次随机  ， 跟最后一个随机数一样的话，重新选择
					if (_lastYoungIndex != -1)
					{
						while (value == _lastYoungIndex)
						{
							value = MathUtility.Random (1,_youngTipNum);
						}
					}
				}
				else
				{
					// _youngTipArr [value] == 1  说明被使用过了
					while (_youngTipArr [value-1] == 1)
					{
						value = MathUtility.Random (1,_youngTipNum);
					}
				}
			}

			_youngTipArr [value-1] = 1;

			_currentYoungIndex++;	

			if (_currentYoungIndex == _youngTipArr.Length)
			{
				_lastYoungIndex = value;
				_InitYoungArr ();
				_currentYoungIndex = 0;			
			}

			var tmpStr = "";
//			tmpVlaue=this.GetType ().GetField ("tip3").GetValue (this).ToString ();
			tmpStr = _gameTips.GetType ().GetField ("newTip"+value).GetValue(_gameTips).ToString();
			return tmpStr;
		}

		/// <summary>
		/// Gets the old outer tip.31-45岁之间的提示
		/// </summary>
		/// <returns>The old outer tip.</returns>
		public string GetOldOuterTip()
		{
			int value = 0;

			if (_oldTipNum - _currentOldIndex <= 2)
			{
				for (var i = _oldTipArr.Length-1; i >=0 ; i--)
				{
					if (_oldTipArr [i] != 1)
					{
						_oldTipArr [i] = 1;
						value = i+1;
						break;
					}
				}
			}
			else
			{
				value = MathUtility.Random (1,_oldTipNum);
				if (_currentOldIndex == 0)
				{
					// 如果是第一次随机  ， 跟最后一个随机数一样的话，重新选择
					if (_lastOldIndex != -1)
					{
						while (value == _lastOldIndex)
						{
							value = MathUtility.Random (1,_oldTipNum);
						}
					}
				}
				else
				{
					// _youngTipArr [value] == 1  说明被使用过了
					while (_oldTipArr [value-1] == 1)
					{
						value = MathUtility.Random (1,_oldTipNum);
					}
				}
			}

			_oldTipArr [value - 1] = 1;

			_currentOldIndex++;

			if (_currentOldIndex == _oldTipArr.Length)
			{
				_lastOldIndex = value;
				_InitOldArr ();
				_currentOldIndex = 0;			
			}
			var tmpStr = "";
			//			tmpVlaue=this.GetType ().GetField ("tip3").GetValue (this).ToString ();
			tmpStr = _gameTips.GetType ().GetField ("oldTip"+value).GetValue(_gameTips).ToString();
			return tmpStr;
		}

		/// <summary>
		/// Gets the more old tip.大于45岁之后的提示
		/// </summary>
		/// <returns>The more old tip.</returns>
		public string GetMoreOldTip()
		{
			int value = 0;

			if (_moreOldTipNum - _currentMoreOldIndex <= 2)
			{
				for (var i = _moreTipArr.Length-1; i >=0 ; i--)
				{
					if (_moreTipArr [i] != 1)
					{
						_moreTipArr [i] = 1;
						value = i+1;
						break;
					}
				}
			}
			else
			{
				value = MathUtility.Random (1,_moreOldTipNum);
				if (_currentMoreOldIndex == 0)
				{
					// 如果是第一次随机  ， 跟最后一个随机数一样的话，重新选择
					if (_lastMoreOldIndex != -1)
					{
						while (value == _lastMoreOldIndex)
						{
							value = MathUtility.Random (1,_moreOldTipNum);
						}
					}
				}
				else
				{
					// _youngTipArr [value] == 1  说明被使用过了
					while (_moreTipArr [value-1] == 1)
					{
						value = MathUtility.Random (1,_moreOldTipNum);
					}
				}
			}

			_moreTipArr [value - 1] = 1;

			_currentMoreOldIndex++;

			if (_currentMoreOldIndex == _moreTipArr.Length)
			{
				_lastMoreOldIndex= value;
				_InitMoreOldArr ();
				_currentMoreOldIndex = 0;
			}
			var tmpStr = "";
			//			tmpVlaue=this.GetType ().GetField ("tip3").GetValue (this).ToString ();
			tmpStr = _gameTips.GetType ().GetField ("oldMoreTip"+value).GetValue(_gameTips).ToString();
			return tmpStr;
		}

		/// <summary>
		/// Gets the enter tip. 进入内圈的提示语
		/// </summary>
		/// <returns>The enter tip.</returns>
		public string GetEnterTip()
		{
			var tmpStr = "";

			var tmpNum = UnityEngine.Random.Range (0,100);

			if (tmpNum > 75)
			{
				tmpStr = _gameTips.enterTip;
			}
			else if(tmpNum>50)
			{
				tmpStr = _gameTips.enterResult1;
			}
			else if(tmpNum>25)
			{
				tmpStr = _gameTips.enterResult2;
			}
			else
			{
				tmpStr = _gameTips.enterResult3;
			}

			return tmpStr;
		}

		/// <summary>
		/// Gets the inner tip. 内圈提示语
		/// </summary>
		/// <returns>The inner tip.</returns>
		public string GetInnerTip()
		{
			int value = 0;

			if (_innerTipNum - _currentInnerIndex <= 2)
			{
				for (var i = _innerTipArr.Length-1; i >=0 ; i--)
				{
					if (_innerTipArr [i] != 1)
					{
						_innerTipArr [i] = 1;
						value = i + 1;
						break;
					}
				}
			}
			else
			{
				value = MathUtility.Random (1,_innerTipNum);
				if (_currentInnerIndex == 0)
				{
					// 如果是第一次随机  ， 跟最后一个随机数一样的话，重新选择
					if (_lastInnerIndex != -1)
					{
						while (value == _lastInnerIndex)
						{
							value = MathUtility.Random (1,_innerTipNum);
						}
					}
				}
				else
				{
					// _innerTipArr [value] == 1  说明被使用过了
					while (_innerTipArr [value-1] == 1)
					{
						value = MathUtility.Random (1,_innerTipNum);
					}
				}
			}

			_innerTipArr [value - 1] = 1;

			_currentInnerIndex++;

			if (_currentInnerIndex == _innerTipArr.Length)
			{
				_lastOldIndex = value;
				_InitInnerArr ();
				_currentInnerIndex = 0;			
			}

			var tmpStr = "";
			tmpStr = _gameTips.GetType ().GetField ("innerTip"+value).GetValue(_gameTips).ToString();
			return tmpStr;
		}


		private void _InitYoungArr()
		{
			for (var i = 0; i < _youngTipArr.Length; i++)
			{
				_youngTipArr [i] = 0;
			}
		}

		private void _InitOldArr()
		{
			for(var i=0;i<_oldTipArr.Length;i++)
			{
				_oldTipArr [i] = 0;
			}
		}

		private void _InitInnerArr()
		{
			for (var i = 0; i < _innerTipArr.Length; i++)
			{
				_innerTipArr [i] = 0;
			}
		}

		private void _InitMoreOldArr()
		{
			for (var i = 0; i < _moreTipArr.Length; i++)
			{
				_moreTipArr [i] = 0;
			}
		}

		/// <summary>
		/// Gets the star tip for pinzhi. 品质指数的评价话术 ， 用过星级获得
		/// </summary>
		/// <param name="index">Index.</param>
		public string GetStarTipForPinzhi(int index)
		{
			var tmpStr = "";

			tmpStr = _starTips.GetType ().GetField ("tipPinzhi"+index).GetValue (_starTips).ToString ();

			return tmpStr;
		}


		/// <summary>
		/// Gets the star tip for chengzhang. 成长指数的评价话术 ， 用过星级获得
		/// </summary>
		/// <param name="index">Index.</param>
		public string GetStarTipForChengzhang(int index)
		{
			var tmpStr = "";
			tmpStr = _starTips.GetType ().GetField ("tipChengzhang"+index).GetValue (_starTips).ToString ();
			return tmpStr;
		}


		/// <summary>
		/// Gets the star tip for caishang. 财商指数的评价话术 ， 用过星级获得
		/// </summary>
		/// <param name="index">Index.</param>
		public string GetStarTipForCaishang(int index)
		{
			var tmpStr = "";
			tmpStr = _starTips.GetType ().GetField ("tipCaishang"+index).GetValue (_starTips).ToString ();
			return tmpStr;
		}


		/// <summary>
		/// Gets the star tip for fengxian. 抗风险指数的评价话术 ， 用过星级获得
		/// </summary>
		/// <param name="index">Index.</param>
		public string GetStarTipForFengxian(int index)
		{
			var tmpStr = "";
			tmpStr = _starTips.GetType ().GetField ("tipFengxian"+index).GetValue (_starTips).ToString ();
			return tmpStr;
		}


		private const int _youngTipNum = 6;
		private const int _oldTipNum = 8;
		private const int _moreOldTipNum = 6;
//		private const int _enterTipNum = 10;
		private const int _innerTipNum = 15;

		private int _currentYoungIndex=0;
		private int _currentOldIndex = 0;
		private int _currentMoreOldIndex = 0;
		private int _currentInnerIndex=0;

		private int _lastYoungIndex = -1;
		private int _lastOldIndex = -1;
		private int _lastInnerIndex = -1;
		private int _lastMoreOldIndex = -1;

		private int[] _youngTipArr=new int[6];
		private int[] _oldTipArr=new int[8];
		private int[] _moreTipArr=new int[6];
//		private int[] _enterTipArr=new int[_enterTipNum];
		private int[] _innerTipArr=new int[15];

		private GameTips _gameTips;

		private ConclusionStarTip _starTips;

		public static readonly GameTipManager Instance=new GameTipManager();
	}
}

