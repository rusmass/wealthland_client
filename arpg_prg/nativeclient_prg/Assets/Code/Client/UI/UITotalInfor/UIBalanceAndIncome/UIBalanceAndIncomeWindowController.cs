using System;
using System.Collections.Generic;
using Metadata;

namespace Client.UI
{
    /// <summary>
    /// 资产与收入的界面
    /// </summary>
	public class UIBalanceAndIncomeWindowController:UIController<UIBalanceAndIncomeWindow,UIBalanceAndIncomeWindowController>
	{
		public UIBalanceAndIncomeWindowController ()
		{
		}

		protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uiinforboards/inforbalanceincome.ab";
			}
		}

		protected override void _OnLoad ()
		{

		}

		protected override void _OnShow ()
		{

		}

		protected override void _OnHide ()
		{

		}

		protected override void _Dispose ()
		{

		}

		public List<ChanceShares> GetShareList()
		{
			return _shareList;
		}

		public List<InforRecordVo> GetIncomeList()
		{
			return _incomeList;
		}

		public InforRecordVo GetIncomeByIndex(int index)
		{
			var values = _incomeList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		public List<ChanceFixed> GetBalanceList()
		{
			return _balanceList;
		}

		public ChanceFixed GetBalanceByIndex(int index)
		{
			var values = _balanceList;
			if (null != values && index < values.Count)
			{
				return values[index];
			}
			return null;
		}


		public PlayerInfo playerInfor
		{
			get
			{
				return _playerInfor;
			}
			set
			{
				_playerInfor = value;
				_SetWindowData (_playerInfor);
			}
		}

		private void _SetWindowData(PlayerInfo player)
		{
			_shareList.Clear();
			_balanceList.Clear();
			_incomeList.Clear();

			for (var i = 0; i < player.chanceFixedCardList.Count; i++) 
			{
				var tmpCard=player.chanceFixedCardList[i];

				if (tmpCard.income != 0)
				{
					var tmpVo = new InforRecordVo ();
					tmpVo.index = _incomeList.Count + 1;
					tmpVo.title = tmpCard.title;
					tmpVo.num = tmpCard.income;
					_incomeList.Add (tmpVo);
				}
				_balanceList.Add (tmpCard);
			}

			for (var i = 0; i < player.opportCardList.Count; i++) 
			{
				var tmpCard=player.opportCardList[i];

				var tmpFixedInfor = new ChanceFixed();

				tmpFixedInfor.id = tmpCard.id;
				tmpFixedInfor.baseNumber = tmpCard.baseNumber;
				tmpFixedInfor.belongsTo = tmpCard.belongsTo;
				tmpFixedInfor.cardPath = tmpCard.cardPath;
				tmpFixedInfor.coast = tmpCard.cost;
				tmpFixedInfor.desc = tmpCard.desc;
				tmpFixedInfor.income = tmpCard.income;
				tmpFixedInfor.mortgage = tmpCard.mortgage;
				tmpFixedInfor.title=tmpCard.title;
				tmpFixedInfor.payment = tmpCard.payment;
				tmpFixedInfor.profit = tmpCard.profit;
				tmpFixedInfor.sale = tmpCard.sale;
				tmpFixedInfor.scoreNumber=0;
				tmpFixedInfor.scoreType=2;

				if (tmpCard.income != 0)
				{
					var tmpVo = new InforRecordVo ();
					tmpVo.index = _incomeList.Count + 1;
					tmpVo.title = tmpCard.title;
					tmpVo.num = tmpCard.income;
					_incomeList.Add (tmpVo);
				}
				_balanceList.Add (tmpFixedInfor);
			}


			for (var i = 0; i < player.shareCardList.Count; i++)
			{
				var tmpCard =  player.shareCardList [i];
				var tmpFixedInfor = new ChanceFixed ();
				tmpFixedInfor.title = tmpCard.title;
				tmpFixedInfor.id = tmpCard.id;
				tmpFixedInfor.cardPath = tmpCard.cardPath;
				tmpFixedInfor.baseNumber = tmpCard.shareNum;

				if (tmpCard.income != 0)
				{
					var tmpVo = new InforRecordVo ();
					tmpVo.index = _incomeList.Count + 1;
					tmpVo.title = tmpCard.title;
					tmpVo.num = tmpCard.income;
					_incomeList.Add (tmpVo);
				}
				_shareList.Add(tmpCard);
				_balanceList.Add (tmpFixedInfor);
			}

			if (GameModel.GetInstance.isPlayNet == true)
			{
				_incomeList = player.netInforBalanceAndIncome.nonIncomeList;
			}

			Console.WriteLine ("当前非劳务数据的长度,"+_incomeList.Count.ToString());

		}


		public void MoveOut()
		{
			var window = _window as UIBalanceAndIncomeWindow;
			if (null != window && getVisible ())
			{
				window.MoveOut ();
			}
		}

		public void MoveIn()
		{
			var window = _window as UIBalanceAndIncomeWindow;
			if (null != window && getVisible ())
			{
				window.MoveIn ();
			}
		}


		private PlayerInfo _playerInfor;

		private List<InforRecordVo> _incomeList=new List<InforRecordVo>();
		private List<ChanceFixed> _balanceList=new List<ChanceFixed>();
		private List<ChanceShares> _shareList=new List<ChanceShares>();
	}
}

