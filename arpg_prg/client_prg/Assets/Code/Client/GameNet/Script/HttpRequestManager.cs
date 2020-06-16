using UnityEngine;
using System.Collections;

using System;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Text;
//using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

using LitJson;
using GameNet;
using Client;

public class HttpRequestManager : MonoBehaviour
{
	public const string Type_FeedBack="feedback";

	public const string Type_LoginBack="loginback";

	public const string Type_RegistBack = "registback";

	public const string Type_ModifyBack = "modifyback";

	public const string Type_GetCodeBack="getcodeback";

	public const string Type_GetVersionBack="backversion";

	public const string Type_GetServerBack ="backserver";

	public const string Type_GetPlayerInforBack = "backplayerinfor";

	// Use this for initialization
	void Start ()
	{
	}

	void Awake()
	{
		_instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Get
	IEnumerator Get(string url,string getData , string type ,Action callback=null) 
	{		
//		Debug.Log (url+getData);

		var idfen = false;
		if (getData != "")
		{
			
			getData = "jsonString=" + WWW.EscapeURL (getData);//Encoding.UTF8.GetBytes();
			idfen = true;
		}
		WWW www;
		if(idfen==false)
		{
			www = new WWW (url+getData);
		}
		else
		{
			//getData
			Debug.Log (url+getData);
			www= new WWW (url+getData);
		}
		//www= new WWW ("http://59.110.29.41:8080/RichLife/account/player/getCode?jsonString=%7B%22phone%22%3A%2218618399063%22%7D");
		yield return www;

		if (www.error != null) 
		{
			Debug.Log("error is :"+ www.error);
		} 
		else 
		{
			//WWW.UnEscapeURL (Encoding.UTF8.GetString( www.bytes))
			Debug.Log("request result :" +www.text);
			var sstring = WWW.UnEscapeURL (www.text);
			switch (type)
			{
			case Type_GetVersionBack :
				_HandleCheckVerion (www.text);
				break;

			case Type_GetServerBack:
				_HandleServiceList (www.text, callback);
				break;
			case Type_GetCodeBack:
				_HandleCodeBack (sstring , callback);
				break;

			default:
				break;
			}

		}
	}

	IEnumerator Post(string url, string postData,string type,Action callback=null) 
	{
//		WWWForm form = new WWWForm();
//		foreach(KeyValuePair<string, string> postArg in postData) 
//		{
		//			form.AddField(postArg.Key, postArg.Value);WWW.EscapeURL(
//		}

		WWW www = new WWW(url,Encoding.UTF8.GetBytes(postData));

		Debug.Log (Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(postData)));

		yield return www;

		if (www.error != null) 
		{
			Debug.Log("error is :"+ www.error);
		} 
		else
		{
			Debug.Log("request result :" + www.text);

			//JsonData tmpback = JsonMapper.ToObject (www.text);

			var sstring = WWW.UnEscapeURL (www.text);

			switch (type)
			{
			case Type_FeedBack:
				_handlerFeedback (www.text, callback);
				break;
			case Type_ModifyBack:
				_handModifyBack (www.text, callback);
				break;
			case Type_RegistBack:
				_handRegistBack (www.text, callback);
				break;
			case Type_LoginBack:
				_handLoginBack(www.text, callback);
				break;
			case Type_GetPlayerInforBack:
				_handPlayerInforBack (www.text, callback);
				break;

			default:
				break;
			}
		}
	}

	private void _handPlayerInforBack(string value , Action callback)
	{
		//			Console.WriteLine (str);
		//request result :{"msg":"玩家角色创建成功",
		//"data":{"role":{"accountId":"25","gameNumber":"1","gender":"1","headImg":"","name":"12313213","uuid":"0007a98f-cf6f-42bf-b3a0-0f3aba2a055b"}},"status":0}
		var backdata =JsonMapper.ToObject(value);
		int status = int.Parse (backdata ["status"].ToString());
		string msg = backdata ["msg"].ToString ();

		if (status == 0)
		{

			NetWorkScript.getInstance ().init ();

			Debug.Log ("ssssssss------------"+status.ToString());
			MessageHint.Show (msg);

			var role =backdata["data"]["role"];

			var nickname = role ["name"].ToString ();
			var headimg = role["headImg"].ToString();
			var sex = int.Parse(role["gender"].ToString());
			var uuid = role["uuid"].ToString();

			GameModel.GetInstance.setPlayerInfor(nickname,sex,uuid,headimg);

			if (callback != null)
			{
				callback ();
			}
		}
		else if(status==1)
		{
			MessageHint.Show (msg);
		}



	}

	private void _handLoginBack(string value ,Action callback=null)
	{
		//LoginBackVo data = Coding<LoginBackVo>.decode (value);

		var data = JsonMapper.ToObject (value);

		var status = int.Parse (data["status"].ToString());
		var msg = data["msg"].ToString();

		//登录成功
		if (status == 0)
		{
			var isneed = int.Parse (data ["data"] ["need"].ToString ());
			if(isneed==0)//如果需要弹提示框
			{
				_showPlayerInforBoard ();
			}
			else
			{
				NetWorkScript.getInstance ().init ();
				var role =data["data"]["role"];

				var nickname = role ["name"].ToString ();
				var headimg = role["headImg"].ToString();
				var sex = int.Parse(role["gender"].ToString());
				var uuid = role["uuid"].ToString();
				GameModel.GetInstance.setPlayerInfor(nickname,sex,uuid,headimg);
				if (null != callback)
				{
					callback ();
				}
			}



		}
		else
		{
			//失败
			MessageHint.Show(msg);
		}
	}

	private void _showPlayerInforBoard()
	{
		var inforController = UIControllerManager.Instance.GetController<Client.UI.UIPlayerInforController> ();
		inforController.windowType = 0;
		inforController.setVisible (true);
	}

	private void _handRegistBack(string value ,Action callback=null)
	{
		var backdatavo = new PlayerRegistBackVo (); //Coding<PlayerRegistBackVo>.decode (value);

		var jsonData = JsonMapper.ToObject (value);
		backdatavo.status = int.Parse(jsonData ["status"].ToString ());
		backdatavo.msg = jsonData ["msg"].ToString ();


		if(backdatavo.status==0)//chenggong
		{
			MessageHint.Show (backdatavo.msg);

			if (null != callback)
			{
				callback ();
			}

		}
		else 
		{
			MessageHint.Show (backdatavo.msg);
		}
	}

	private void _handModifyBack(string value ,Action callback=null)
	{
		var backdatavo = new PlayerRegistBackVo (); //Coding<PlayerRegistBackVo>.decode (value);

		var jsonData = JsonMapper.ToObject (value);
		backdatavo.status = int.Parse(jsonData ["status"].ToString ());
		backdatavo.msg = jsonData ["msg"].ToString ();


		if(backdatavo.status==0)//chenggong
		{
			MessageHint.Show (backdatavo.msg);

			if (null != callback)
			{
				callback ();
			}

		}
		else 
		{
			MessageHint.Show (backdatavo.msg);
		}
	}


	private void _handlerFeedback(string value , Action callback = null)
	{
		var backVo =new FankuiBackVo() ;//Coding<FankuiBackVo>.decode (value);
		var jsonData = JsonMapper.ToObject (value);
		backVo.status = int.Parse (jsonData ["status"].ToString ());
		backVo.msg = jsonData ["msg"].ToString ();


		////0成功，1失败 
		//public int status;
		/// <summary>
		/// The .：”反馈提示玩家”
		/// </summary>
		//public string msg;

		if (backVo.status == 0)//成功
		{				
			if (null != callback)
			{
				callback ();
			}
		}
		else
		{
			//失败
		}

		MessageHint.Show (backVo.msg);
	}

	/// <summary>
	/// The request UR. 请求urlhttp://localhost:8080/RichLife
	/// </summary>            http://59.110.29.41:8080/RichLife/account/player/getCode?
	private string targetURL= "http://118.144.74.82:8088/RichLife/account";
//	private string targetURL="http://192.168.1.153:8080/RichLife/account";

	/// <summary>
	/// The regist path. 注册请求的地址 post  
	/// </summary>
	private readonly string RegistPath ="/player/register?";

	/// <summary>
	/// The modify password. 找回密码 post
	/// </summary>
	private readonly string ModifyPasswordPath="/player/findPassword?";

	/// <summary>
	/// The login path.登录请求的接口  post?json=?json=player/login
	/// </summary>
	private readonly string LoginPath = "/player/login?";



	/// <summary>
	/// The get code path. 获取验证码 get
	/// </summary>
	private readonly string GetCodePath="/player/getCode?";

    /// <summary>
    /// The check version path. 检查游戏客户端版本
    /// </summary>
    private readonly string CheckVersionPath="/game/checkVersion?json=";

	/// <summary>
	/// The feed back path. 游戏客户端反馈/game/feedback?json=
	/// </summary>
	private readonly string FeedBackPath="/feedback?";

	/// <summary>
	/// The server list path.获取服务器列表
	/// </summary>
	private readonly string ServerListPath="/game/server/list";
	//http://59.110.29.41:8060/RichLife/account/game/server/list  http://59.110.29.41:8060/RichLife/account/game/server/list

	private readonly string PlayerInforPath="/player/perfectInfo?";


	private HttpWebRequest request;

	private static HttpRequestManager _instance;

	public List<string> serviceList=new List<string>();

	public string identyCode="dddd";

	public static HttpRequestManager GetInstance()
	{
		return _instance;
	}

	public HttpRequestManager ()
	{
		//request = HttpWebRequest.Create (targetURL) as HttpWebRequest;
		//request.Method = REQUEST_TYPE_GET;
		//HttpWebResponse 
	}

	private void _HandleCheckVerion(string value)
	{
		var jsonData = JsonMapper.ToObject (value);

		var versionNum = float.Parse (jsonData ["data"] ["versionCode"].ToString ());
		if (versionNum > GameModel.CurVersion)
		{
			showTipNewVersion ();
			GameModel.GetInstance.isNeedNewVersion = true;
		}
	}

	int times=0;
	private void showTipNewVersion()
	{
		StartCoroutine(showGameTip());
	}

	IEnumerator showGameTip()
	{
		yield return new WaitForSeconds (0.5f);
		times++;
		var gameTip = UIControllerManager.Instance.GetController<Client.UI.UIGameSimpleTipController> ();
		gameTip.SetSureTip ("当前版本过低，请更新最新版本"+times.ToString(),showTipNewVersion);
		gameTip.setVisible (true);
	}


	private void _HandleServiceList(string value , Action callback)
	{
		serviceList.Clear ();
		//request result :{"msg":"查询成功","data":[{"serverName":"电信"},{"serverName":"联通"}],"status":0}
		JsonData data = JsonMapper.ToObject (value);

		var state = int.Parse (data ["status"].ToString ());

		if (state == 0)
		{
			var jsonlist = data["data"];

			if (jsonlist.IsArray == true)
			{
				for (int i = 0; i < jsonlist.Count; i++)
				{
					var serverdata = jsonlist [i];
					this.serviceList.Add (serverdata["serverName"].ToString());
				}
			}

		}
		else
		{
			MessageHint.Show ("获取服务器失败");
		}

		if (null != callback)
		{
			callback ();
		}
	}

	private void _HandleCodeBack(string value , Action callback)
	{
		//{"msg":"获取验证码成功","data":{"code":"7491"},"status":0}
		Debug.Log ("sssssss"+value.ToString());

		var getcodeback = new PlayerGetCodeBackVo (); //Coding<PlayerGetCodeBackVo>.decode (value);
		JsonData data = JsonMapper.ToObject (value);

		getcodeback.status = int.Parse (data ["status"].ToString ());
		getcodeback.msg = data ["msg"].ToString ();
		getcodeback.code = data ["data"] ["code"].ToString ();
//		public int status;   
//		public string msg;
//
//		public GetCodeBackData data;

		if(getcodeback.status==0)//成功
		{
			identyCode = getcodeback.code;
			MessageHint.Show ("已经发验证码，注意接收");

			var registController = UIControllerManager.Instance.GetController<Client.UI.UIRegistController> ();
			if (registController.getVisible ())
			{
				registController.StartCountTime ();
			}


			var modifyController = UIControllerManager.Instance.GetController<Client.UI.UIModifyWindowController> ();
			if (modifyController.getVisible ())
			{
				modifyController.StartCountTime ();
			}

		}
		else
		{
			Console.WriteLine (getcodeback.msg);
		}
	}

	public void GetPlayerInfor(string data,Action callback=null)
	{
		var value = "jsonString=" + data;
		StartCoroutine(Post (targetURL+PlayerInforPath,value,Type_GetPlayerInforBack,callback));
	}

	/// <summary>
	/// Gets the regist data. 发送注册请求，获得反馈 post
	/// </summary>
	/// <returns>The regist data.</returns>
	/// <param name="data">Data.</param>
	public  void GetRegistData(string data , Action callback=null)
	{
		var value = "jsonString=" + data;
		StartCoroutine(Post (targetURL+RegistPath,value,Type_RegistBack,callback));
	}

	/// <summary>
	/// Gets the login data. 发送登录请求，获得反馈数据 post
	/// </summary>
	/// <returns>The login data.</returns>
	/// <param name="data">Data.</param>
	public void GetLoginData(string data,Action callback=null)
	{
		var value = "jsonString=" + data;			
		StartCoroutine(Post(targetURL+LoginPath,value,Type_LoginBack,callback));


	}



	/// <summary>
	/// Gets the modify data. 或得修改密码的数据
	/// </summary>
	/// <returns>The modify data.</returns>
	/// <param name="data">Data.</param>
	public void GetModifyData(string data,Action callback=null)
	{
		var value = "jsonString=" + data;
		StartCoroutine(Post (targetURL+ModifyPasswordPath,value,Type_ModifyBack,callback));
	}

	/// <summary>
	/// Gets the check code data.获取验证码的数据
	/// </summary>
	/// <returns>The check code data.</returns>
	/// <param name="data">Data.</param>
	public void GetCheckCodeData(string data,Action callback=null)
	{
		//JsonData value = new JsonData();
		//value ["jsonString"] = data;
		//JSONObject test = new JSONObject ();
		//test ["jsonString"] = data;

		var value = "jsonString=" + data;
		Console.WriteLine ("sssssss--------------"+value);
		StartCoroutine( Get (targetURL+GetCodePath,data,Type_GetCodeBack,callback));
		//return HttpGet (targetURL+GetCodePath,data);
	}

	/// <summary>
	/// Gets the check version data. 获取检测版本号的数据
	/// </summary>
	/// <returns>The check version data.</returns>
	public void GetCheckVersionData()
	{
		StartCoroutine (Get(targetURL+CheckVersionPath,"",Type_GetVersionBack));
	}

	/// <summary>
	/// Gets the feed back data.发送反馈信息，并且返回
	/// </summary>
	/// <returns>The feed back data.</returns>
	/// <param name="data">Data.</param>
	public void GetFeedBackData(string data, Action callback=null)
	{			
		var value = "jsonString=" + data;
		StartCoroutine(Post(targetURL+FeedBackPath,value,Type_FeedBack,callback));
	}

	/// <summary>
	/// Gets the check server data.
	/// </summary>
	/// <returns>The check server data.</returns>
	/// <param name="data">Data.</param>
	public void GetServerListData(string data="",Action callback=null)
	{		
		//return HttpGet (targetURL+ServerListPath,"");
		StartCoroutine( Get (targetURL + ServerListPath, data, Type_GetServerBack, callback));
	}

//	public string HttpPost(string url,string postDataStr)
//	{
//		/*if (postDataStr != "")
//			{
//				postDataStr="jsonString="+postDataStr;
//			}*/
//
//		//Console.Write ();
//
//		//UnityEngine.Debug.LogWarning (url + postDataStr);
//
//		//return url + postDataStr;
//
//		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
//		//request.Method = REQUEST_TYPE_POET;
//		//request.ContentType = "charset=gb2312";application/json;
//		request.ContentType="application/x-www-form-urlencoded;charset=UTF-8";
//		//request.ContentType="text/html";
//		request.ProtocolVersion = HttpVersion.Version10;  
//		request.ContentLength =Encoding.UTF8.GetByteCount(postDataStr);
//		CookieContainer cookie = new CookieContainer ();
//
//		request.CookieContainer = cookie;
//
//		Stream myRequestStream = request.GetRequestStream ();//gb2312
//		StreamWriter myStreamWriter = new StreamWriter (myRequestStream,Encoding.GetEncoding("gb2312"));
//		myStreamWriter.Write (postDataStr);
//		myStreamWriter.Close ();
//
//		HttpWebResponse respose=(HttpWebResponse)request.GetResponse();
//		respose.Cookies = cookie.GetCookies (respose.ResponseUri);
//
//		Stream myResposeStream = respose.GetResponseStream ();
//
//		StreamReader myStreamReader = new StreamReader (myResposeStream,Encoding.GetEncoding("utf-8"));
//		string retstring = myStreamReader.ReadToEnd ();
//		myStreamReader.Close ();
//		myResposeStream.Close ();
//		return retstring;
//	}
//}

}