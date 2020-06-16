using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using LitJson;
using GameNet;
using Client;

using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.UI;

/// <summary>
/// http登录模块
/// </summary>
public class HttpRequestManager : MonoBehaviour
{
    /// <summary>
    /// 修改人物信息
    /// </summary>
    public const string Type_UpdateInforBack = "updateInforback";

    /// <summary>
    /// 自己的分享感悟列表
    /// </summary>
    public const string Type_FeelingSelfBack = "feelselfback";

    /// <summary>
    /// 全部游戏感悟的分享返回
    /// </summary>
    public const string Type_FeelingShareBack = "feelshareback";

    /// <summary>
    /// 游戏反馈后返回信息的
    /// </summary>
	public const string Type_FeelingBack="feelingback";

    /// <summary>
    /// 处理登录返回消息
    /// </summary>
	public const string Type_LoginBack="loginback";
    /// <summary>
    /// 处理注册返回消息
    /// </summary>
	public const string Type_RegistBack = "registback";
    /// <summary>
    /// 注册修改密码返回的消息
    /// </summary>
	public const string Type_ModifyBack = "modifyback";
    /// <summary>
    /// 修改获取验证码返回的消息
    /// </summary>
	public const string Type_GetCodeBack="getcodeback";
    /// <summary>
    /// 请求游戏版本号返回的消息--旧版本，不带请求参数
    /// </summary>
	public const string Type_GetVersionBack="backversion";

    /// <summary>
    /// 请求游戏版本号---新版本，带请求参数
    /// </summary>
    public const string Type_GetVersionBackNewest = "versionNew";

    /// <summary>
    /// 查看游戏服务器后返回的消息，暂时屏蔽服务器功能
    /// </summary>
	public const string Type_GetServerBack ="backserver";
    /// <summary>
    /// 创建角色后返回的消息
    /// </summary>
	public const string Type_GetPlayerInforBack = "backplayerinfor";
    /// <summary>
    /// 上传图像后返回的数据
    /// </summary>
	public const string Type_UploadImgBack = "backuploadimg";
    /// <summary>
    /// 绑定手机号后返回的消息
    /// </summary>
	public const string Type_blindPhone = "blindPhone";
    /// <summary>
    /// 检测手机号是否可绑定返回的消息
    /// </summary>
	public const string Type_CheckPhone = "chenckphonenumber";
    
    /// <summary>
    /// 处理获取游戏公告后返回的消息
    /// </summary>
	public const string Type_NoticeBack = "NoticeBack";

    /// <summary>
    /// 游戏感悟返回的消息
    /// </summary>
    //public const string Type_FellingBack = "FeelingBack";
	
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

	/// <summary>
    /// Http Get方法，通过请求地址，和请求参数，确定请求的数据。通过type类型，和callBack，确定对的处理函数和应用的回调函数
    /// </summary>
    /// <param name="url"></param>
    /// <param name="getData"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
	IEnumerator Get(string url,string getData , string type ,Action callback=null) 
	{		
		Debug.Log (url+getData);
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
				_HandleCheckVerionOld (www.text);
				break;
                case Type_GetVersionBackNewest:
                    _HandleNewVerion(www.text);
                    break;


			case Type_GetServerBack:
				_HandleServiceList (www.text, callback);
				break;
			case Type_GetCodeBack:
				_HandleCodeBack (sstring , callback);
				break;
			case Type_NoticeBack:
				_HandlerNoticeBack (www.text,callback);
				break;
			default:
				break;
			}
		}
	}
    
    /// <summary>
    /// 上传纹理集图片到服务器，然后显示上传的图片。此处单独作为新建角色信息时接口，后需要单独摘出
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
	IEnumerator UploadImageData(Texture2D data,Action callBack)
	{
        //MessageHint.Show("上传图片的宽度："+data.width+",高度是："+data.height);
        //data.Resize(200, 200);
        //MessageHint.Show("上传图片的宽度：" + data.width + ",高度是：" + data.height);
        byte[] bs =ScaleTexture(data,180,180).EncodeToPNG ();
		//Encoding.UTF8.GetBytes(postData)
		WWWForm form = new WWWForm();//
		form.AddBinaryData ("picture", bs, "filename.png", "image/png");

        var playerId = GameModel.GetInstance.myHandInfor.uuid;

        form.AddField("playerId", playerId);

		WWW www = new WWW(targetURL+"/user/image",form);

		yield return www;
		if (www.error != null)
		{
			Debug.Log ("error is :" + www.error);
		}
		else
		{		
			var backdata =JsonMapper.ToObject( WWW.UnEscapeURL (www.text));
			int status = int.Parse (backdata ["status"].ToString());
			//string msg = backdata ["msg"].ToString ();
			if (status == 0)
			{
               // Console.Warning.WriteLine("返回的卡牌数据:"+ backdata["data"]["path"].ToString());
				var tmpPath = backdata ["data"] ["path"].ToString ();
                //var playerController = UIControllerManager.Instance.GetController<Client.UI.UIPlayerInforController> ();
                //            if(playerController.getVisible())
                //            {
                //                playerController.SetHeadPath(tmpPath);
                //            }
                //var personController = UIControllerManager.Instance.GetController<Client.UI.UIPersonalController>();
                //if(personController.getVisible())
                //{
                //    GameModel.GetInstance.myHandInfor.headImg = tmpPath;
                //}
                Console.Error.WriteLine("头像下载路径是："+tmpPath);
                GameModel.GetInstance.myHandInfor.headImg = tmpPath;
                if (null!=callBack)
                {
                    callBack();
                }
                MessageHint.Show("上传图片成功");               
			}
            else
            {
                MessageHint.Show("上传图片失败");
            }
		}
	}


    Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        //float incX = (1.0f / (float)targetWidth);
        //float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }

    /// <summary>
    /// Http post 形式通过地址和请求参数，确定数据请求。通过type执行对应的方法，通过callBack，传入回调函数
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postData"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
	IEnumerator Post(string url, string postData,string type,Action callback=null) 
	{
        Console.WriteLine("请求的地址："+url+"______请求的参数："+postData);
		WWW www = new WWW(url,Encoding.UTF8.GetBytes(postData));
		Debug.Log (Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(postData)));
		yield return www;
		if (www.error != null) 
		{
			Debug.Log("error is :"+ www.error);
            if(type== Type_LoginBack)
            {
                GameModel.GetInstance.HideNetLoading();
            }
		} 
		else
		{
			Debug.Log("request result :" + www.text);			
			switch (type)
			{
                case Type_UpdateInforBack:
                    _HandlerUpdateInfor(www.text, callback);
                     break;

                case Type_FeelingSelfBack:
                    _handlerFeelSelfShare(www.text, callback);
                    break;

                case Type_FeelingShareBack:
                    _handlerFeelingShare(www.text, callback);
                    break;
                case Type_FeelingBack:
				_handlerFeelBack (www.text, callback);
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
			case Type_blindPhone:
				_blindPhoneBackData (www.text, callback);
				break;
			case Type_CheckPhone:
				_CheckPhoneNumBackData (www.text, callback);
				break;
			default:
				break;
			}
		}
	}

	/// <summary>
	/// Checks the phone number back data.校验手机号
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="callback">Callback.</param>
	private void _CheckPhoneNumBackData(string value, Action callback=null)
	{
		var backdata =JsonMapper.ToObject(value);
		int status = int.Parse (backdata ["status"].ToString());
		
		var tmpstate = 0;
		var checkController = UIControllerManager.Instance.GetController<Client.UI.UIGameBindPhoneWindowController> ();

		if (status == 0)//不能绑定
		{
			tmpstate = 0;
		}
		else if (status == 1)//已经有手机号，可以绑定
		{
			tmpstate = 1;
		}
		else if (status == 2)//该手机号没有注册，可以绑定
		{
			tmpstate = 2;
		}
		checkController.CheckPhoneState (tmpstate);
	}

	/// <summary>
	/// Blinds the phone back data.绑定手机data
	/// </summary>
	private void _blindPhoneBackData(string value , Action callback)
	{
		var backdata =JsonMapper.ToObject(value);
		int status = int.Parse (backdata ["status"].ToString());
		string msg = backdata ["msg"].ToString ();

		var bindController = UIControllerManager.Instance.GetController<Client.UI.UIGameBindPhoneWindowController> ();

		if (status == 0)
		{
			_showPlayerInforBoard ();
			bindController.setVisible (false);
			MessageHint.Show ("绑定手机号成功");

		}
		else if(status==1)
		{
			NetWorkScript.getInstance ().init ();
			var role = backdata ["data"] ["player"];

			var nickname = role ["name"].ToString ();
			var headimg = role ["avatar"].ToString ();
			var sex = int.Parse (role ["gender"].ToString ());
			var uuid = role ["id"].ToString ();
            var birth = "";
            if (((IDictionary)role).Contains("birthday"))
            {
                birth = role["birthday"].ToString();
            }
            GameModel.GetInstance.setPlayerInfor (nickname, sex, uuid, headimg,birth);
			if (null != callback) {
				callback ();
			}
			MessageHint.Show ("绑定手机号成功");
			bindController.setVisible (false);
		}
		else
		{
			MessageHint.Show (msg);
		}
	}

    /// <summary>
    /// 处理创建玩家信息后的信息
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _handPlayerInforBack(string value , Action callback)
	{		
		//request result :{"msg":"玩家角色创建成功","data":{"role":{"accountId":"25","gameNumber":"1","gender":"1","headImg":"","name":"12313213","uuid":"0007a98f-cf6f-42bf-b3a0-0f3aba2a055b"}},"status":0}
		var backdata =JsonMapper.ToObject(value);
		int status = int.Parse (backdata ["status"].ToString());
		string msg = backdata ["msg"].ToString ();

		if (status == 0)
		{
			NetWorkScript.getInstance ().init ();			
			MessageHint.Show (msg);

			var role =backdata["data"]["player"];

			var nickname = role ["name"].ToString ();
			var headimg = role["avatar"].ToString();
			var sex = int.Parse(role["gender"].ToString());
			var uuid = role["id"].ToString();
            var birth = "";
            if(((IDictionary)role).Contains("birthday"))
            {
                birth = role["birthday"].ToString();
            }

			GameModel.GetInstance.setPlayerInfor(nickname,sex,uuid,headimg,birth);

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

    /// <summary>
    /// 处理登录后返回的数据
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _handLoginBack(string value ,Action callback=null)
	{
        GameModel.GetInstance.HideNetLoading();

        var data = JsonMapper.ToObject (value);
		var status = int.Parse (data["status"].ToString());
		var msg = data["msg"].ToString();
		//登录成功
		if (status == 0)
		{				
			var isShowBoard = false;
			if (((IDictionary)data ["data"]).Contains ("bind"))
			{
				var isBind = int.Parse (data["data"]["bind"].ToString());
				if (isBind == 0)
				{
					_showBindInforBoard ();
				}
				else if(isBind==1)				
				{
					isShowBoard = true;
                }
			}
			else
			{
				isShowBoard = true;
			}

			if (isShowBoard == true)
			{
				if (((IDictionary)data ["data"]).Contains ("need"))
				{
					var isneed = int.Parse (data ["data"] ["need"].ToString ());
					if (isneed == 0)
					{						
						_showPlayerInforBoard ();
					}
					else
					{
						NetWorkScript.getInstance ().init ();
						var role = data ["data"] ["player"];

						var nickname = role ["name"].ToString ();
						var headimg = role ["avatar"].ToString ();
						var sex = int.Parse (role ["gender"].ToString ());
						var uuid = role ["id"].ToString ();
                        var birth = "";
                        if (((IDictionary)role).Contains("birthday"))
                        {
                            birth = role["birthday"].ToString();
                        }

                        GameModel.GetInstance.setPlayerInfor (nickname, sex, uuid, headimg, birth);
						if (null != callback) {
							callback ();
						}
					}
				}		
			}
		}
		else
		{
			//失败
			MessageHint.Show(msg);
		}
	}

    /// <summary>
    /// 显示创建玩家信息的面板
    /// </summary>
	private void _showPlayerInforBoard()
	{
		var inforController = UIControllerManager.Instance.GetController<Client.UI.UIPlayerInforController> ();
		inforController.windowType = 0;
		inforController.setVisible (true);
	}

    /// <summary>
    /// 显示绑定手机号的界面
    /// </summary>
	private void _showBindInforBoard()
	{
		Console.WriteLine ("出现了绑定手机号");
		var controller = UIControllerManager.Instance.GetController<Client.UI.UIGameBindPhoneWindowController> ();
		controller.setVisible (true);
	}

    /// <summary>
    /// 处理注册后返回的数据
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _handRegistBack(string value ,Action callback=null)
	{
		var backdatavo = new PlayerRegistBackVo (); //Coding<PlayerRegistBackVo>.decode (value);

		var jsonData = JsonMapper.ToObject (value);
		backdatavo.status = int.Parse(jsonData ["status"].ToString ());
		backdatavo.msg = jsonData ["msg"].ToString ();

		if(backdatavo.status==0)//chenggong
		{
			MessageHint.Show (backdatavo.msg);
            if(null!=callback)
            {
                callback();
            }
		}
		else 
		{
			MessageHint.Show (backdatavo.msg);
		}
	}

    /// <summary>
    /// 处理修改密码后返回的数据
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _handModifyBack(string value ,Action callback=null)
	{
		var backdatavo = new PlayerRegistBackVo (); //Coding<PlayerRegistBackVo>.decode (value);
    	var jsonData = JsonMapper.ToObject (value);
		backdatavo.status = int.Parse(jsonData ["status"].ToString ());
		backdatavo.msg = jsonData ["msg"].ToString ();

		if(backdatavo.status==0)//chenggong
		{
			MessageHint.Show (backdatavo.msg);
            if(null!=callback)
            {
                callback();
            }
		}
		else 
		{
			MessageHint.Show (backdatavo.msg);
		}
	}

    /// <summary>
    /// 处理提交反馈后返回的数据
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _handlerFeelBack(string value , Action callback = null)
	{
		var backVo =new FankuiBackVo() ;//Coding<FankuiBackVo>.decode (value);
		var jsonData = JsonMapper.ToObject (value);
		backVo.status = int.Parse (jsonData ["status"].ToString ());
		backVo.msg = jsonData ["msg"].ToString ();
        
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
    /// 显示所有玩家的分享
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
    private void _handlerFeelingShare(string value,Action callback=null)
    {
        var controll = UIControllerManager.Instance.GetController<Client.UI.UIFeelingBaordController>();
        var jsonData = JsonMapper.ToObject(value);
       

        var state = int.Parse(jsonData["status"].ToString());

        if(state==1)
        {
            MessageHint.Show("");
            return;
        }

        var dataList = jsonData["data"];
        if (((IDictionary)dataList).Contains("inspirations"))
        {
            var tmpList = dataList["inspirations"];
            
            if (tmpList.IsArray)
            {
                for(var i=0;i<tmpList.Count;i++)
                {
                    var tmpVo = new FeelingVo();
                    var tmpValue = tmpList[i];
                    tmpVo.content= tmpValue["content"].ToString();
                    //Console.Error.WriteLine("lllllllllllllll" + i);
                    controll.GameFeeling.Add(tmpVo);
                }
            }
            //Console.Error.WriteLine("当前数组的长度--"+controll.GameFeeling.Count);
        }

        var _current = int.Parse(dataList["page"].ToString());
        var _total = int.Parse(dataList["count"].ToString());

        if (_current == _total)
        {
            controll.IsAllLoadGameFeel = true;
        }

        controll.currentGameFeeling = _current;
        controll.GameFeelPages = _total;

        if(null!=callback)
        {
            callback();
        }
        
    }

    /// <summary>
    /// 返回玩家自己的游戏感悟列表
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
    private void _handlerFeelSelfShare(string value,Action callback=null)
    {
        var controll = UIControllerManager.Instance.GetController<Client.UI.UIFeelingBaordController>();
        var jsonData = JsonMapper.ToObject(value);
        

        var state = int.Parse(jsonData["status"].ToString());

        if(state==1)
        {
            MessageHint.Show("您暂时没有发表感悟");
            return;
        }
        var dataList = jsonData["data"];
        if (((IDictionary)dataList).Contains("inspirations"))
        {
            var tmpList = dataList["inspirations"];
            if (tmpList.IsArray)
            {
                for (var i = 0; i < tmpList.Count; i++)
                {
                    var tmpVo = new FeelingVo();
                    var tmpValue = tmpList[i];
                    tmpVo.content = tmpValue["content"].ToString();

                    controll.SelfFeelList.Add(tmpVo);
                }
            }
        }

        var _current = int.Parse(dataList["page"].ToString());
        var _total = int.Parse(dataList["count"].ToString());

        if(_current==_total)
        {
            controll.IsAllLoadSelfFeel = true;
        }

        controll.currentSelfFeeling = _current;
        controll.SelfFeelPages = _total;

        if (null != callback)
        {
            callback();
        }
    }
    
    /// <summary>
    /// 处理更新人物信息返回的数据
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
    private void _HandlerUpdateInfor(string value , Action callback)
    {
        var jsonData = JsonMapper.ToObject(value);
        var tipStr = jsonData["msg"].ToString();
        var state = int.Parse(jsonData["status"].ToString());

        //    "msg": "添加信息成功",
        //"data": {
        //        "birthday": "1990.11.12",
        //    "gender": "1",
        //    "constellation": "天蝎座",
        //    "name": "55555"
        //},
        //"status": 0

        if(state==0)
        {
            var data = jsonData["data"];
            var birthday = data["birthday"].ToString();
            var gender = int.Parse(data["gender"].ToString());
            var constellation = data["constellation"].ToString();
            var name = data["name"].ToString();

            var tmpInfor = GameModel.GetInstance.myHandInfor;
            tmpInfor.nickName = name;
            tmpInfor.sex = gender;
            tmpInfor.birthday = birthday;
            tmpInfor.constellation = constellation;
            if(null!=callback)
            {
                callback();
            }
        }
        MessageHint.Show(tipStr);
    }

	public static HttpRequestManager GetInstance()
	{
		return _instance;
	}

	public HttpRequestManager ()
	{
		
	}
    /// <summary>
    /// 处理检查游戏版本后返回的数据---旧版本
    /// </summary>
    /// <param name="value"></param>
	private void _HandleCheckVerionOld(string value)
	{
		var jsonData = JsonMapper.ToObject (value);
		var versionNum = float.Parse (jsonData ["data"] ["versionCode"].ToString ());
        if (((IDictionary)jsonData["data"]).Contains("androidDownload"))
        {
            var androidDownPath = jsonData["data"]["androidDownload"].ToString();
            GameModel.GetInstance.androidDownPath = androidDownPath;
        }
        if (((IDictionary)jsonData["data"]).Contains("iosDownload"))
        {
            var iosDownPath = jsonData["data"]["iosDownload"].ToString();
            GameModel.GetInstance.iosDownPath = iosDownPath;
        }  
        if (versionNum > GameModel.CurVersion)
		{
			showTipNewVersion ();
			GameModel.GetInstance.isNeedNewVersion = true;
		}
	}


    private void _HandleNewVerion(string value)
    {

 //       {
 //           "msg": "版本号",
	//"data": {
 //               "androidDownload": "http://wealthlife.cfwh.com.cn/",
	//	"iosDownload": "http://wealthlife.cfwh.com.cn/",
	//	"isUpdate": 0,
	//	"serverIP": "192.168.1.31",
	//	"serverPort": "8067",
	//	"status": 0,
	//	"versionCode": "2.2.27",
	//	"version_id": 0

 //   },
	//"status": 0
//}
        

        var jsonData = JsonMapper.ToObject(value);
        var tmpData = jsonData["data"];
      
        if (((IDictionary)tmpData).Contains("androidDownload"))
        {
            var androidDownPath = tmpData["androidDownload"].ToString();
            GameModel.GetInstance.androidDownPath = androidDownPath;
        }
        if (((IDictionary)tmpData).Contains("iosDownload"))
        {
            var iosDownPath = tmpData["iosDownload"].ToString();
            GameModel.GetInstance.iosDownPath = iosDownPath;
        }
        //"serverIP": "192.168.1.31",
        //"serverPort": "8067",
        var connectIP = tmpData["serverIP"].ToString();
        var connectPort = int.Parse(tmpData["serverPort"].ToString());
        NetWorkScript.hostUrl = connectIP;
        NetWorkScript.port = connectPort;

        //var versionNum = float.Parse(tmpData["versionCode"].ToString());
        //判断是否是需要更新 0不需更新 ， 1需要更新
        var isUpdate =int.Parse(tmpData["isUpdate"].ToString());        
        if (isUpdate==1)
        {
            showTipNewVersion();
            GameModel.GetInstance.isNeedNewVersion = true;
        }

    }

    private int times=0;
    /// <summary>
    /// 显示版本更新的请求
    /// </summary>
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

    /// <summary>
    /// 处理请求服务器列表后返回的信息
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _HandleServiceList(string value , Action callback)
	{
		serviceList.Clear ();       
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

	/// <summary>
	/// Handlers the notice back. 获取到公告的返回信息
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="callback">Callback.</param>
	private void _HandlerNoticeBack (string value, Action callback)
	{
		var backData = JsonMapper.ToObject (value);
		var status = int.Parse (backData["status"].ToString());

		if (status == 0)
		{
			if (((IDictionary)backData).Contains ("data"))
			{
				var noticeList = backData ["data"];
				if (noticeList.IsArray)
				{
                    
					if (noticeList.Count > 0)
					{
                        GameModel.GetInstance.gonggaoList.Clear();
                        GameModel.GetInstance.gonggaoList.TrimExcess();


                        for (var i = 0; i < noticeList.Count; i++)
						{
							var tip = noticeList[i];

							var title = tip["title"].ToString();
							var content = tip ["content"].ToString ();
							var type =int.Parse(tip ["type"].ToString ());
                            var targetUrl = "";
                            if(((IDictionary)tip).Contains("target_url"))
                            {
                                targetUrl = tip["target_url"].ToString();
                            }

                            var gonggaovo = new GonggaoVo ();
							gonggaovo.title = title;
							gonggaovo.content = content;
							gonggaovo.type = type;
                            gonggaovo.targetUrl = targetUrl;
                            if(targetUrl!="")
                            {
                                gonggaovo.isUrl = true;
                            }


							Debug.Log ("~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + gonggaovo.title);
							GameModel.GetInstance.gonggaoList.Add (gonggaovo);
						}

                        //var gonggaovo1 = new GonggaoVo();
                        //gonggaovo1.title = "llallaa";
                        //gonggaovo1.content = "http://192.168.1.31:8080/gameNotice/getImg";
                        //gonggaovo1.type = 1;
                        //gonggaovo1.targetUrl = "http://www.asdyf.com/video_mobile/index.php";
                        //gonggaovo1.isUrl = true;                        
                        //GameModel.GetInstance.gonggaoList.Add(gonggaovo1);

                        if (GameModel.GetInstance.isFirstInGameHall == true)
						{
							_ShowGonggao ();
						}
					}
				}
			}			
		}
	}

	/// <summary>
	/// Shows the gonggao.游戏公告
	/// </summary>
	private void _ShowGonggao()
	{
        if(Client.UI.GameGuidManager.GetInstance.DoneGameHall==false)
        {
            return;
        }

		if (GameModel.GetInstance.gonggaoList.Count > 0)
		{
            var gonggaocontroller = UIControllerManager.Instance.GetController<Client.UI.UIGongGaoController>();
            gonggaocontroller.inforList = GameModel.GetInstance.gonggaoList;
            gonggaocontroller.setVisible(true);
        }
	}

    /// <summary>
    /// 处理获取验证码后返回的消息
    /// </summary>
    /// <param name="value"></param>
    /// <param name="callback"></param>
	private void _HandleCodeBack(string value , Action callback)
	{		
		Debug.Log ("sssssss"+value.ToString());
		var getcodeback = new PlayerGetCodeBackVo (); //Coding<PlayerGetCodeBackVo>.decode (value);
		JsonData data = JsonMapper.ToObject (value);
		getcodeback.status = int.Parse (data ["status"].ToString ());
		getcodeback.msg = data ["msg"].ToString ();
		getcodeback.code = data ["data"] ["code"].ToString ();
		if(getcodeback.status==0)//成功
		{
			identyCode = getcodeback.code;
			MessageHint.Show ("已经发验证码，注意接收");

            var registController = UIControllerManager.Instance.GetController<Client.UI.UIRegistController>();
            if (registController.getVisible())
            {
                registController.StartCountTime();
            }


            var modifyController = UIControllerManager.Instance.GetController<Client.UI.UIModifyWindowController>();
            if (modifyController.getVisible())
            {
                modifyController.StartCountTime();
            }
        }
		else
		{
			Console.WriteLine (getcodeback.msg);
		}
	}

    /// <summary>
    /// 把2d纹理集合转成图片，上传到服务器
    /// </summary>
    /// <param name="data"></param>
	public void UpLoadImage(Texture2D data,Action callBack=null)
	{
		StartCoroutine (UploadImageData (data, callBack));
	}
    /// <summary>
    /// 新建玩家角色信息
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
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
    /// 绑定手机号
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
	public void BindPhone(string data, Action callback=null)
	{
		var value = "jsonString=" + data;
		StartCoroutine(Post (targetURL+BlindPhonePath,value,Type_blindPhone,callback));
	}

	/// <summary>
	/// Checks the phone number. 校验手机号
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="callback">Callback.</param>
	public void CheckPhoneNum(string data , Action callback=null)
	{
		var value = "jsonString=" + data;
		Console.WriteLine (CheckPhoneNumPath+value);
		StartCoroutine(Post (targetURL+CheckPhoneNumPath,value,Type_CheckPhone,callback));
	}

	/// <summary>
	/// Gets the check code data.获取验证码的数据
	/// </summary>
	/// <returns>The check code data.</returns>
	/// <param name="data">Data.</param>
	public void GetCheckCodeData(string data,Action callback=null)
	{
		var value = "jsonString=" + data;
		Console.WriteLine ("sssssss--------------"+value);
		StartCoroutine( Get (targetURL+GetCodePath,data,Type_GetCodeBack,callback));		
	}

	/// <summary>
	/// Gets the check version data. 获取检测版本号的数据，旧版维护用
	/// </summary>
	/// <returns>The check version data.</returns>
	public void GetCheckVersionOld()
	{
		StartCoroutine (Get(targetURL+CheckVersionOld, "", Type_GetVersionBack));
	}

    /// <summary>
    ///  获取游戏版本号，域名，和更新信息等
    /// </summary>
    /// <param name="data"></param>
    public void GetCheckVersionNew(string data)
    {
        StartCoroutine(Get(targetURL+CheckNewVersionPath,data,Type_GetVersionBackNewest));
    }



	/// <summary>
	/// Gets the game notice.获取游戏公告
	/// </summary>
	public void GetGameNotice()
	{
		StartCoroutine (Get(targetURL+GameNoticePath,"",Type_NoticeBack));
	}

	/// <summary>
	/// Gets the feed back data.发送反馈信息，并且返回
	/// </summary>
	/// <returns>The feed back data.</returns>
	/// <param name="data">Data.</param>
	public void GetFeelingBackData(string data, Action callback=null)
	{			
		var value = "jsonString=" + data;
		StartCoroutine(Post(targetURL+FeelingBackPath,value,Type_FeelingBack,callback));
	}

    /// <summary>
    /// 更新玩家信息
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    public void UpdatePlayerInfor(string data,Action callback=null)
    {
        var value = "jsonString=" + data;
        StartCoroutine(Post(targetURL + GameUpdateInfor, value, Type_UpdateInforBack, callback));
    }

    /// <summary>
    /// 返回游戏的分享列表
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callback"></param>
    public void GetFeelingShareData(string data,Action callback=null)
    {
        var value = "jsonString=" + data;
        StartCoroutine(Post(targetURL + GameFeelingShare, value, Type_FeelingShareBack, callback));
    }

    public void GetFeelSelfData(string data,Action callback=null)
    {
        var value = "jsonString=" + data;
        StartCoroutine(Post(targetURL + GameFeelingSelf, value, Type_FeelingSelfBack, callback));
    }

	/// <summary>
	/// Gets the check server data.
	/// </summary>
	/// <returns>The check server data.</returns>
	/// <param name="data">Data.</param>
	public void GetServerListData(string data="",Action callback=null)
	{				
		StartCoroutine( Get (targetURL + ServerListPath, data, Type_GetServerBack, callback));
	}


    /// <summary>
    /// The request UR. 请求urlhttp://localhost:8080/RichLife 
    /// </summary>   
    private string targetURL = "http://ocalhost:8100:8066/game";//game
    //private string targetURL = "http://localhost:8100/game-login";

    /// <summary>
    /// The regist path.注册请求的地址post
    /// </summary>
    private readonly string RegistPath = "/user/register?";

    /// <summary>
    /// The modify password. 找回密码 post
    /// </summary>
    private readonly string ModifyPasswordPath = "/user/findPassword?";

    /// <summary>
    /// The login path.登录请求的接口  post?json=?json=player/login
    /// </summary>
    private readonly string LoginPath = "/user/login?";

    /// <summary>
    /// The get code path. 获取验证码 get
    /// </summary>
    private readonly string GetCodePath = "/user/getCode?";
    /// <summary>
    /// The check version path. 检查游戏客户端版本---老版本维护接口
    /// </summary>
    private readonly string CheckVersionOld = "/game/checkVersion?";

    /// <summary>
    ///  检查游戏最新版本---新版本
    /// </summary>
    private readonly string CheckNewVersionPath = "/game/version?";

    /// <summary>
    /// 游戏感悟的接口
    /// </summary>
    private readonly string FeelingBackPath = "/inspiration/add?";

    /// <summary>
    /// The server list path.获取服务器列表
    /// </summary>
    private readonly string ServerListPath = "/game/server/list";

    //创建角色，绑定手机
    private readonly string PlayerInforPath = "/user/player/perfectInfo?";
    /// <summary>
    /// The blind phone path.绑定手机号
    /// </summary>
    private readonly string BlindPhonePath = "/user/bindPhone";
    /// <summary>
    /// The check phone number path. 验证手机号
    /// </summary>
    private readonly string CheckPhoneNumPath = "/user/authPhone";

    /// <summary>
    /// The game notice path.获取游戏公告
    /// </summary>
    private readonly string GameNoticePath = "/gameNotice/find";
    /// <summary>
    /// 游戏全部感恩的分享
    /// </summary>
    private readonly string GameFeelingShare = "/inspiration/page";

    /// <summary>
    /// 自己的分享列表
    /// </summary>
    private readonly string GameFeelingSelf = "/inspiration/myself";

    /// <summary>
    /// 更新玩家信息的数据
    /// </summary>
    private readonly string GameUpdateInfor = "/user/updatePlayer";



    private static HttpRequestManager _instance;
    public List<string> serviceList = new List<string>();

    public string identyCode = "";
}