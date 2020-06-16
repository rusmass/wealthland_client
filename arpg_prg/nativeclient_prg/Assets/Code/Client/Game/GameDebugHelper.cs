using System;
using UnityEngine;
using Client.UI;
using Client;


public partial class GameDebugHelper
{
	private GameDebugHelper ()
	{
		
	}

	private void  _InputHelper()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			_isShowOptimizeTest = true;
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			_isShowDebugWindow = true;
		}

		if (Input.GetKeyUp (KeyCode.A))
		{
			_isAddMoney = true;
		}

		if (Input.GetKeyUp (KeyCode.S))
		{
			_isAddIncome = true;
		}

		if (Input.GetKeyUp (KeyCode.D))
		{
			_isAddQualituyScore = true;
		}

		if (Input.GetKeyUp (KeyCode.F))
		{
			_isAddTimeScore = true;
		}

		if (Input.GetKeyUp (KeyCode.Alpha1))
		{
			_debugRoll = 1;
			_isDebugRoll = true;
		}
		else if (Input.GetKeyUp (KeyCode.Alpha2))
		{
			_debugRoll = 2;
			_isDebugRoll = true;
		}
		else if (Input.GetKeyUp (KeyCode.Alpha3))
		{
			_debugRoll = 3;
			_isDebugRoll = true;
		}
		else if (Input.GetKeyUp (KeyCode.Alpha4))
		{
			_debugRoll = 4;
			_isDebugRoll = true;
		}
		else if (Input.GetKeyUp (KeyCode.Alpha5))
		{
			_debugRoll = 5;
			_isDebugRoll = true;
		}
		else if (Input.GetKeyUp (KeyCode.Alpha6))
		{
			_debugRoll = 6;
			_isDebugRoll = true;
		}


	}

	public void Tick(float deltaTime)
	{
		_InputHelper ();

		if(_isShowOptimizeTest==true)
		{
			_ShowUIOptimizeTest ();
			_isShowOptimizeTest = false;
		}

		if(_isShowDebugWindow==true)
		{
			_ShowDebugWindow ();
			_isShowDebugWindow = false;
		}

		if (_isAddMoney == true)
		{
			_DebugAddMoney ();
			_isAddMoney = false;
		}

		if(_isAddIncome==true)
		{
			_DebugAddInCome();
			_isAddIncome = false;
		}

		if (_isAddQualituyScore == true) 
		{
			_DebugAddQualityScore();
			_isAddQualituyScore = false;
		}

		if (_isAddTimeScore == true) {
			_DebugAddTimeSocre();
			_isAddTimeScore = false;
		}
	
	
	
	}

	private void _DebugAddMoney()
	{

		Client.CardOrderHandler.Instance.GetChanceCardId ();

		var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
		var heroInfro = PlayerManager.Instance.Players[0];
		if (null!=heroInfro)
		{
			heroInfro.totalMoney += 50000000;

//			if (heroInfro.totalMoney <= 0)
//			{
//				heroInfro.totalMoney = 0;
//			}

			controller.SetCashFlow((int)heroInfro.totalMoney);
//			controller.SetPersonInfor (heroInfro,1);
			heroInfro.PlayerIntegral+=1;
		}
	}

	private void _DebugAddInCome()
	{			
		var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
		//
		var heroInfro = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
		if (null!=heroInfro)
		{
			if (heroInfro.isEnterInner == false)
			{
				heroInfro.totalIncome += 500;
			} 
			else 
			{
				heroInfro.totalIncome += 8000;
			}
//			controller.SetPersonInfor (heroInfro, 1, false);
			controller.SetNonLaberIncome((int)heroInfro.CurrentIncome,heroInfro.TargetIncome);
		}
	}

	private void _DebugAddTimeSocre()
	{		
		var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
		var heroInfro = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
		if (null!=heroInfro)
		{
			heroInfro.timeScore += 100;
			controller.SetTimeScore((int)heroInfro.timeScore,1000);
//			controller.SetPersonInfor (heroInfro, 1, false);
		}
	}



	private void _DebugAddQualityScore()
	{
		var controller = UIControllerManager.Instance.GetController<UIBattleController> ();
		var heroInfro = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
		if (null != heroInfro) {
			heroInfro.qualityScore += 10;
			controller.SetQualityScore ((int)heroInfro.qualityScore,100);
//			controller.SetPersonInfor (heroInfro, 1, false);
		}		
	}


	private void _ShowUIOptimizeTest()
	{
		var control = UIControllerManager.Instance.GetController<UIOptimizeTestController>();
		if (control.getVisible())
		{
			control.setVisible(false);
		}
		else
		{
			control.setVisible(true);
		}
	}

	private void _ShowDebugWindow()
	{
//		CardManager.Instance.OpenCard ((int)SpecialCardType.StudyType);
		//UICheckOutController
		//var control = UIControllerManager.Instance.GetController<UICheckOutController>();

//		var _initWorldPosition = Room.Instance.GetCurrentPlayerByIndex(0).getPlayPos ().position;
//		var tmpPosition =Camera.main.WorldToScreenPoint(new Vector3(_initWorldPosition.x,_initWorldPosition.y+2,_initWorldPosition.z));
//		Console.WriteLine (string.Format("人物坐标点,{0},{1},{2}",tmpPosition.x.ToString(),tmpPosition.y.ToString(),tmpPosition.z.ToString()));
		//var tmpVec = new Vector3 (tmpPosition.x,tmpPosition.y,tmpPosition.z);
//		var tmpVec=new Vector3(0,0,0);
//		control.AddMoneyEffect (1,tmpVec);


//		if (control.getVisible())
//		{
//			control.setVisible(false);
//		}
//		else
//		{
//			control.setVisible(true);
//		}
		_TestPicPath();

	}

	private void _TestPicPath()
	{
		var control = UIControllerManager.Instance.GetController<UICheckOutController>();
		control.initCardPath ();
		if (control.getVisible())
		{
			control.setVisible(false);
		}
		else
		{
			control.setVisible(true);
		}		
	}

	// 只点数
	public bool IsDebugRoll
	{
		get
		{
			return _isDebugRoll;
		}

		set 
		{
			_isDebugRoll = value;
		}
	}

	public int DebugRollNumber
	{
		get
		{
			return _debugRoll;
		}
	}

	private bool _isShowOptimizeTest=false;
	private bool _isShowDebugWindow=false;


	// 增加游戏界面的金币数量
	private bool _isAddMoney=false;
	private bool _isAddIncome=false;
	private bool _isAddQualituyScore=false;
	private bool _isAddTimeScore=false;

	private bool _isDebugRoll=false;
	private int _debugRoll=0;

	public static readonly GameDebugHelper Instance=new GameDebugHelper();
}	


