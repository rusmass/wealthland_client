using System;
using Client.UI;
namespace Client
{
    /// <summary>
    /// 单机游戏中出牌的数据
    /// </summary>
	public class CardOrderHandler
	{
		private CardOrderHandler ()
		{
			
		}

		#region 外圈卡牌随机

		public int GetChanceCardId()
		{
			if (null == _chanceCardOrder)
			{
				var list2 = CardManager.Instance.outerChanceList;
				_chanceCardOrder = new CardOrder (list2.ToArray());
			}

			return _chanceCardOrder.GetCardId ();
		}

		public int GetOpportunityCardId()
		{
			if (null == _opportunityCardOrder)
			{
				var list1 = CardManager.Instance.outerOpportunityList;
				_opportunityCardOrder = new CardOrder (list1.ToArray());
			}
			return _opportunityCardOrder.GetCardId();
		}

		public int GetOuterFateCardId()
		{
			if (null == _outerfateCardOrder)
			{
				var list = CardManager.Instance.outerFateList;
				_outerfateCardOrder = new CardOrder (list.ToArray());
			}

			return _outerfateCardOrder.GetCardId();
		}

		public int GetRiskChanceCardId()
		{
			if (null == _riskCardOrder)
			{
				var list = CardManager.Instance.outerRiskList;
				_riskCardOrder = new CardOrder (list.ToArray());
			}
			return _riskCardOrder.GetCardId();
		}
		#endregion


		#region 内圈卡牌随机

		public int GetRelaxCardId()
		{
			if (null == _relaxCardOrder)
			{
				var list = CardManager.Instance.innerRelaxList;
				_relaxCardOrder = new CardOrder (list.ToArray ());
			}
			return _relaxCardOrder.GetCardId();
		}

		public int GetInvestmentCardId()
		{
			if (null == _investmnetCardOrder)
			{
				var list = CardManager.Instance.innerInvestmentList;
				_investmnetCardOrder =new CardOrder(list.ToArray());
			}
			return _investmnetCardOrder.GetCardId ();
		}

		public int GetQualityCardId()
		{
			if (null == _qualityCardOrder)
			{
				var list = CardManager.Instance.innerQualtyList;
				_qualityCardOrder=new CardOrder((list.ToArray()));
			}
			return _qualityCardOrder.GetCardId();
		}

		public int GetInnerFateCardId()
		{
			if (null == _innerfateCardOrder)
			{
				var list = CardManager.Instance.innerFateList;
				_innerfateCardOrder =new CardOrder(list.ToArray());
			}

			return _innerfateCardOrder.GetCardId ();			
		}

		#endregion


		private CardOrder _riskCardOrder;
		private CardOrder _chanceCardOrder;
		private CardOrder _opportunityCardOrder;
		private CardOrder _outerfateCardOrder;

		private CardOrder _innerfateCardOrder;
		private CardOrder _relaxCardOrder;
		private CardOrder _qualityCardOrder;
		private CardOrder _investmnetCardOrder;

		public static readonly CardOrderHandler Instance =new CardOrderHandler();

	}


	class CardOrder
	{
		private int[] _cardArr;
		private int _index=0;
		private int _maxLen;

		public CardOrder(int[] value)
		{
			_cardArr= value;
			_maxLen = _cardArr.Length;
			_ReSortOn();
		}


		/// <summary>
		/// 卡牌重新排序
		/// </summary>
		private void _ReSortOn()
		{
			for (int i = _maxLen - 1; i > 0; i--)
			{
				Random rand = new Random();
				int p = rand.Next(i);
				int temp = _cardArr[p];
				_cardArr[p] = _cardArr[i];
				_cardArr[i] = temp;
			}

			for (int i = _maxLen - 1; i > 0; i--)
			{
				Random rand = new Random();
				int p = rand.Next(i);
				int temp = _cardArr[p];
				_cardArr[p] = _cardArr[i];
				_cardArr[i] = temp;
			}

//			for (int i = 0; i < _maxLen; i++)
//			{
//				Console.WriteLine (_cardArr[i]);
//			}

		}

		public int GetCardId()
		{
			var tmpValue = _cardArr [_index];

			_index++;
			if (_index >= _maxLen)
			{
				_index = 0;
				Console.WriteLine ("卡牌循环完了呢");
			}

			Console.WriteLine ("卡牌id"+tmpValue.ToString());		

			return tmpValue;
		}
	}
}

