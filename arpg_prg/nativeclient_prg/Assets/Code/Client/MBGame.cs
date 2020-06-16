using Core;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using cn.sharesdk.unity3d;

namespace Client
{
    /// <summary>
    /// 游戏主要控制器，进入游戏的接口
    /// </summary>
    public partial class MBGame : MonoBehaviour
    {
		private void Start()
		{
			_coreMain.Init ();
			_game = Game.Instance;
			Instance = this;
			Application.runInBackground = true;


			ssdk = gameObject.GetComponent<ShareSDK>();
			ssdk.authHandler = OnAuthResultHandler;
			ssdk.showUserHandler = OnGetUserInfoResultHandler;
        }

		private void Update()
        {
			try
			{
				var deltaTime = Time.deltaTime;
				_coreMain.Tick(deltaTime);
				if (null != _game)
				{
					_game.Tick(deltaTime);
				}
			}
			catch(Exception e)
			{
				Console.Error.WriteLine(e);
				Console.Error.WriteLine(e.StackTrace);
			}

			if(Application.platform==RuntimePlatform.Android)
			{
				if (Input.GetKeyUp (KeyCode.Escape))
				{
					if (isQuitGamed == true)
					{
						Application.Quit ();
					} 
					else
					{
						isQuitGamed = true;
						MessageHint.Show ("双击返回按钮，退出游戏");
					}				
				}

				if (isQuitGamed == true)
				{
					lefttime += Time.deltaTime;
					if (lefttime >= exitTime)
					{
						lefttime = 0;
						isQuitGamed = false;
					}
				}

			}
        }

		private float exitTime=2f;
		private float lefttime=0;

		private bool isQuitGamed=false;

        private void LateUpdate()
        {
			try
			{
				if (null != _game)
				{
					_game.LateTick(Time.deltaTime);
				}
			}
			catch(Exception e)
			{
				Console.Error.WriteLine(e);
				Console.Error.WriteLine(e.StackTrace);
			}
        }

        private void OnDrawGizmos()
        {
			if (null != _game) 
			{
				_game.OnDrawGizmos();	
			}
        }

        private void OnApplicationFocus(bool focus)
        {
			if (null != _game) 
			{
				_game.OnApplicationFocus(focus);

			}
        }

        private void OnApplicationPause(bool pause)
        {
			if (null != _game) 
			{
				_game.OnApplicationPause(pause);
//				OnApplicationFocus (false);
			}
           

        }
        /// <summary>
        ///  游戏重置，暂时未用到
        /// </summary>
        private void OnApplicationQuit()
        {
			if (null != _game) 
			{
				_game.Dispose();
			}
			_coreMain.Dispose();

            //Console.Error.WriteLine("ssssss");
            //NetWorkScript.getInstance().LogOutGame(GameModel.GetInstance.myHandInfor.uuid);
        }

		public void OnApplicationRestart()
		{
			OnApplicationQuit ();

			_coreMain.Init ();
			_game = Game.Instance;
		}

		public static MBGame Instance;

		private static Game _game;
		private static CoreMain _coreMain = CoreMain.Instance;



		public ShareSDK ssdk;
	
//		private void initSharesdk()
//		{
//			
//		}

		/// <summary>
		/// Logins the we chat. 微信登录
		/// </summary>
		public void LoginWeChat()
		{
			AuthorSdk (PlatformType.WeChatMoments);
			SaveTimeLocal ();
		}
        /// <summary>
        /// 微信登录注销,未引用
        /// </summary>
		public void LogoutWeChat()
		{
			LogOutSkd (PlatformType.WeChatMoments);

		}

		/// <summary>
		/// Shares the wei chat. 微信分享
		/// </summary>
		/// <param name="value">Value.</param>
		public void ShareWeiChat(ShareContent value)
		{
			ssdk.ShareContent (PlatformType.WeChat, value);
		}
        /// <summary>
        ///  微信朋友圈分享
        /// </summary>
        /// <param name="value"></param>
        public void ShareWeiChatMonment(ShareContent value)
		{
			ssdk.ShareContent (PlatformType.WeChatMoments, value);
		}

        /// <summary>
        ///  sharesdk，授权登录指定平台
        /// </summary>
        /// <param name="type"></param>
        private void AuthorSdk(PlatformType type)
		{			
			ssdk.Authorize(type);
		}

//		public void LoginSdk(PlatformType type)
//		{
//			ssdk.GetUserInfo(type);
//		}

		private void LogOutSkd(PlatformType type)
		{
			ssdk.CancelAuthorize (type);
		}

        /// <summary>
        ///  sharesdk登录认证平台后返回的数据
        /// </summary>
        /// <param name="reqID"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			if (state == ResponseState.Success)
			{
                GameModel.GetInstance.ShowNetLoading();

				print ("authorize success !" + "Platform :" + type);

				Hashtable res = ssdk.GetAuthInfo(type);

				print(MiniJSON.jsonEncode(res));

//				MessageHint.Show (MiniJSON.jsonEncode(res));
//				var openstr =;
				var openid = "null";
				#if UNITY_ANDROID
				openid=MiniJSON.jsonEncode (res["userID"]);
				#elif UNITY_IPHONE
				openid=MiniJSON.jsonEncode (res["uid"]);
				#endif

//				MessageHint.Show (openid);
				var tmpOpenid = openid.Substring (1, openid.Length - 2);

				if (openid != "" && openid != "null")
				{
					PlayerPrefs.SetString (GameModel.GetInstance.UserId, tmpOpenid);
				}

				var loginController = UIControllerManager.Instance.GetController<Client.UI.UILoginController> ();
				loginController.WeChatLogin (tmpOpenid);

			}
			else if (state == ResponseState.Fail)
			{
                GameModel.GetInstance.HideNetLoading();
                var str = "";
				#if UNITY_ANDROID
				print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
				str="fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"].ToString();
				#elif UNITY_IPHONE
				print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
				str="fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"].ToString();
				#endif
				MessageHint.Show (MiniJSON.jsonEncode(str));
			}
			else if (state == ResponseState.Cancel) 
			{
                GameModel.GetInstance.HideNetLoading();
                print ("cancel !");
			}else
            {
                GameModel.GetInstance.HideNetLoading();
            }
		}
        /// <summary>
        /// 获取玩家信息后返回的数据，微信登录暂未使用这个接口，登录认证的时候已经获取了人物信息
        /// </summary>
        /// <param name="reqID"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			if (state == ResponseState.Success)
			{
				print ("get user info result :");
				print (MiniJSON.jsonEncode(result));
				MessageHint.Show (MiniJSON.jsonEncode(result));
				print ("Get userInfo success !Platform :" + type );
			}
			else if (state == ResponseState.Fail)
			{
				#if UNITY_ANDROID
				print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
				#elif UNITY_IPHONE
				print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
				#endif
			}
			else if (state == ResponseState.Cancel) 
			{
				print ("cancel !");
			}
		}
        /// <summary>
        /// 判断微信登录的时间，当天登录后，再次登录会跳过认证直接登录，第二天会重置登录记录
        /// </summary>
        public void SaveTimeLocal()
		{
			PlayerPrefs.SetString (GameModel.GetInstance.WeChatLastLoginTime, System.DateTime.Now.Day.ToString());
		}
    }
}
