using Client;
using UnityEngine;
using Client.Cameras;
using Client.GameFSM;
using Client.Unit;
using Metadata;
using Client.UI;
using System.Diagnostics;
using UnityEngine.UI;
using DG.Tweening;
using Core.Web;
using Audio;

public partial class Game 
{
    /// <summary>
    ///  初始化游戏
    /// </summary>
	public Game()
	{
		_smartCamera.Init ();
		_uiCamera.Init ();
		_webManager.SetWebManagerParam ();
		_fsm = new FSM (this);
		_fsm.Start ();
		Caching.CleanCache ();
    }

    /// <summary>
    ///  帧频刷新
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Tick(float deltaTime)
    {

		_debugHelp.Tick (deltaTime);
        _fsm.Tick(deltaTime);
        _battleController.Tick(deltaTime);      	

		if (null != _socketManager)
		{
			_socketManager.tick (deltaTime);
		}
    }

    /// <summary>
    ///  切换到选择角色界面
    /// </summary>
	public void SwitchUISelectRoleState()
	{
		if (null != _fsm) 
		{
			_fsm.Input (new SelectRoleEvent());
		}
	}

	/// <summary>
	/// Switchs the login window.切换到登录界面
	/// </summary>
	public void SwitchLoginWindow()
	{
		if (null != _fsm) 
		{
			_fsm.Input (new LoginEvent());
		}
	}


    /// <summary>
    ///  切换到游戏大厅
    /// </summary>
	public void SwithUIGameHall()
	{
		if (null != _fsm) 
		{
			_fsm.Input (new GameHallEvent());
		}
	}

    /// <summary>
    ///  切换到网络游戏
    /// </summary>
	public void SwitchNetGame()
	{
		if (null != _fsm)
		{
			_fsm.Input (new LoadingEvent());
		}
	}

    /// <summary>
    ///  未引用
    /// </summary>
	public void SwitchNetSelectRole()
	{
		if (null != _fsm)
		{
			_fsm.Input (new LoadingEvent());
		}
	}

	public void LateTick(float deltaTime)
	{
		_smartCamera.Tick (deltaTime);	
		_uiControllerManager.Tick (deltaTime);
        _virtualServer.Tick(deltaTime);
    }

	public void OnDrawGizmos()
	{
		
	}

	public void OnApplicationFocus(bool focus)
	{
		
	}

	public void OnApplicationPause(bool focus)
	{
		
	}

	public void Dispose()
	{
        if (null != _metadataManager)
        {
            _metadataManager.Dispose();
        }
		_instance = null;
	}

	private static Game _instance;
	public static Game Instance
	{
		get 
		{
			if (null == _instance) 
			{
				_instance = new Game();
			}

			return _instance;
		}
	}

    /// <summary>
    ///  未引用
    /// </summary>
	public void StartSocketManager()
	{
		if (null != _socketManager)
		{
			_socketManager = new MessageManager ();
			_socketManager.init ();
		}
	}

    /// <summary>
    /// 未引用
    /// </summary>
	public void StopSocketManager()
	{
		
	}

    private readonly FSM _fsm;
	private readonly WebManager _webManager = WebManager.Instance;
    private readonly VirtualServer _virtualServer = VirtualServer.Instance;
	private readonly BattleController _battleController = BattleController.Instance;
	private readonly SmartCamera _smartCamera = SmartCamera.Instance;
	private readonly UICameraManager _uiCamera = UICameraManager.Instance;
	private readonly UIControllerManager _uiControllerManager = UIControllerManager.Instance;
    private readonly MetadataManager _metadataManager = MetadataManager.Instance;

	private readonly GameDebugHelper _debugHelp = GameDebugHelper.Instance;

	private MessageManager _socketManager=MessageManager.getInstance();

}
