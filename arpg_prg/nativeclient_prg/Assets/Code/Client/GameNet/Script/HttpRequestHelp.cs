namespace GameNet
{
	using System;
	using System.Net;
	using System.Net.Security;
	using System.IO;
	using System.Text;
	//using System.IO.Compression;
	using System.Security.Cryptography.X509Certificates;
	using System.Collections.Generic;

	using LitJson;

	
	public class HttpRequestHelp
	{
		private readonly string REQUEST_TYPE_GET="GET";

		private readonly string REQUEST_TYPE_POET="POST";
		/// <summary>
		/// The request UR. 请求urlhttp://localhost:8080/RichLife
		/// </summary>
		private string targetURL="http://59.110.29.41:8060/RichLife/account";

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
		private readonly string LoginPath = "/player/login";



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
		private readonly string FeedBackPath="/game/feedback?";

		/// <summary>
		/// The server list path.获取服务器列表
		/// </summary>
		private readonly string ServerListPath="/game/server/list";
		//http://59.110.29.41:8060/RichLife/account/game/server/list  http://59.110.29.41:8060/RichLife/account/game/server/list

		private readonly string PlayerInforPath="/player/perfectInfo";


		private HttpWebRequest request;

		private static HttpRequestHelp _instance;

		public static HttpRequestHelp GetInstance()
		{
			if (null == _instance)
			{
				_instance = new HttpRequestHelp ();
			}

			return _instance;
		}

		public HttpRequestHelp ()
		{
			//request = HttpWebRequest.Create (targetURL) as HttpWebRequest;
			//request.Method = REQUEST_TYPE_GET;
			//HttpWebResponse 

			System.Net.ServicePointManager.DefaultConnectionLimit = 200;
		}


		public string GetPlayerInfor(string data)
		{
			var value = "jsonString=" + data;
			return HttpPost (targetURL+PlayerInforPath,value);
		}

		/// <summary>
		/// Gets the regist data. 发送注册请求，获得反馈 post
		/// </summary>
		/// <returns>The regist data.</returns>
		/// <param name="data">Data.</param>
		public string GetRegistData(string data)
		{
			var value = "jsonString=" + data;
			return HttpPost (targetURL+RegistPath,value);
		}

		/// <summary>
		/// Gets the login data. 发送登录请求，获得反馈数据 post
		/// </summary>
		/// <returns>The login data.</returns>
		/// <param name="data">Data.</param>
		public string GetLoginData(string data)
		{
			var value = "jsonString=" + data;			
			return HttpPost (targetURL+LoginPath,value);
		}

		/// <summary>
		/// Gets the modify data. 或得修改密码的数据
		/// </summary>
		/// <returns>The modify data.</returns>
		/// <param name="data">Data.</param>
		public string GetModifyData(string data)
		{
			var value = "jsonString=" + data;
			return HttpPost (targetURL+ModifyPasswordPath,value);
		}

		/// <summary>
		/// Gets the check code data.获取验证码的数据
		/// </summary>
		/// <returns>The check code data.</returns>
		/// <param name="data">Data.</param>
		public string GetCheckCodeData(string data)
		{
			//JsonData value = new JsonData();
			//value ["jsonString"] = data;

			//JSONObject test = new JSONObject ();

			//test ["jsonString"] = data;

			//Console.WriteLine ("sssssss"+value.ToJson());

			return HttpGet (targetURL+GetCodePath,data);
		}

		/// <summary>
		/// Gets the check version data. 获取检测版本号的数据
		/// </summary>
		/// <returns>The check version data.</returns>
		public string GetCheckVersionData()
		{
			return HttpGet (targetURL+CheckVersionPath,"");
		}

		/// <summary>
		/// Gets the feed back data.发送反馈信息，并且返回
		/// </summary>
		/// <returns>The feed back data.</returns>
		/// <param name="data">Data.</param>
		public string GetFeedBackData(string data)
		{			
			var value = "jsonString=" + data;

			return HttpPost (targetURL+FeedBackPath,value);
		}

		/// <summary>
		/// Gets the check server data.
		/// </summary>
		/// <returns>The check server data.</returns>
		/// <param name="data">Data.</param>
		public string GetServerListData(string data="")
		{
			return HttpGet (targetURL+ServerListPath,"");
		}


		public string HttpGet(string url, string postDataStr)
		{
			if (postDataStr != "")
			{
				postDataStr="jsonString="+postDataStr;
			}


			//Encoding myEncoding = Encoding.GetEncoding("gb2312");
			//string address = "http://www.jb51.net/?" + HttpUtility.UrlEncode("参数一", myEncoding) + "=" + HttpUtility.UrlEncode("值一", myEncoding);
			//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
			//req.Method = "GET";
			//using (WebResponse wr = req.GetResponse())
			//{
				//在这里对接收到的页面内容进行处理
			//}


			//HttpWebRequest request =(HttpWebRequest)(WebRequest.Create(url+(postDataStr==""?"":"?")+postDataStr));

			//Encoding myEncoding = Encoding.GetEncoding("gb2312");

			System.GC.Collect();

			HttpWebRequest request =(HttpWebRequest)(WebRequest.Create(url+postDataStr));
			request.Method = REQUEST_TYPE_GET;

			request.ContentType = "text/html";

			//request.ContentType = "gb2312    application/x-www-form-urlencoded";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			Stream myResponseStream = response.GetResponseStream ();
			StreamReader myStreamReader = new StreamReader (myResponseStream,Encoding.GetEncoding("utf-8"));

			string retString = myStreamReader.ReadToEnd ();

			myStreamReader.Close ();
			myResponseStream.Close ();


			if (response != null) 
			{ 
				response.Close(); 
				response = null;
			} 
			if (request != null) 
			{ 
				request.Abort(); 

				request = null;
			}

			return retString;
		}


		public string HttpPost(string url,string postDataStr)
		{
			/*if (postDataStr != "")
			{
				postDataStr="jsonString="+postDataStr;
			}*/

			//Console.Write ();

			//UnityEngine.Debug.LogWarning (url + postDataStr);

			//return url + postDataStr;

			System.GC.Collect();

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
			request.Method = REQUEST_TYPE_POET;
			request.KeepAlive = false;
			//request.ContentType = "charset=gb2312";application/json;
			request.ContentType="application/x-www-form-urlencoded;charset=UTF-8";
			//request.ContentType="text/html";
			request.ProtocolVersion = HttpVersion.Version10;  
			request.ContentLength =Encoding.UTF8.GetByteCount(postDataStr);
			CookieContainer cookie = new CookieContainer ();

			request.CookieContainer = cookie;

			Stream myRequestStream = request.GetRequestStream ();//gb2312
			StreamWriter myStreamWriter = new StreamWriter (myRequestStream,Encoding.GetEncoding("gb2312"));
			myStreamWriter.Write (postDataStr);
			myStreamWriter.Close ();

			HttpWebResponse respose=(HttpWebResponse)request.GetResponse();
			respose.Cookies = cookie.GetCookies (respose.ResponseUri);

			Stream myResposeStream = respose.GetResponseStream ();

			StreamReader myStreamReader = new StreamReader (myResposeStream,Encoding.GetEncoding("utf-8"));
			string retstring = myStreamReader.ReadToEnd ();
			myStreamReader.Close ();
			myResposeStream.Close ();

			if (respose != null) 
			{ 
				respose.Close(); 
				respose = null;
			} 
			if (request != null) 
			{ 
				request.Abort(); 

				request = null;
			}


			return retstring;
		}

	}
}






/*class Program  
{  

	public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters,Encoding charset)  
	{  
		HttpWebRequest request = null;  
		//HTTPSQ请求  
		ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);  
		request = WebRequest.Create(url) as HttpWebRequest;  
		request.ProtocolVersion = HttpVersion.Version10;  
		request.Method = "POST";  
		request.ContentType = "application/x-www-form-urlencoded";  
		//request.UserAgent = DefaultUserAgent;  
		//如果需要POST数据     

		if (!(parameters == null || parameters.Count == 0))  
		{  			
			using (Stream stream = request.GetRequestStream())  
			{  
				stream.Write(data, 0, data.Length);  
			}  
		}  
		return request.GetResponse() as HttpWebResponse;  
	}     

	static void Main(string[] args)  
	{  
		string url = "https://192.168.1.101/httpOrg/create";  
		Encoding encoding = Encoding.GetEncoding("utf-8");  
		IDictionary<string, string> parameters = new Dictionary<string, string>();  
		parameters.Add("orgkey","*****");  
		parameters.Add("orgname", "*****");  
		HttpWebResponse response = Program.CreatePostHttpResponse(url,parameters,encoding);  
		//打印返回值  
		Stream stream = response.GetResponseStream();   //获取响应的字符串流  
		StreamReader sr = new StreamReader(stream); //创建一个stream读取流  
		string html = sr.ReadToEnd();   //从头读到尾，放到字符串html  
		Console.WriteLine(html);   
	}  
}  
}  
*/


/*

Encoding myEncoding = Encoding.GetEncoding("gb2312");
string address = "http://www.jb51.net/?" + HttpUtility.UrlEncode("参数一", myEncoding) + "=" + HttpUtility.UrlEncode("值一", myEncoding);
HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
req.Method = "GET";
using (WebResponse wr = req.GetResponse())
{
	//在这里对接收到的页面内容进行处理
}


string param = "hl=zh-CN&newwindow=1";
byte[] bs = Encoding.ASCII.GetBytes(param);
HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create( "http://www.jb51.net/" );
req.Method = "POST";
req.ContentType = "application/x-www-form-urlencoded";
req.ContentLength = bs.Length;
using (Stream reqStream = req.GetRequestStream())
{
	reqStream.Write(bs, 0, bs.Length);
}
using (WebResponse wr = req.GetResponse())
{
	//在这里对接收到的页面内容进行处理
}


Encoding myEncoding = Encoding.GetEncoding("gb2312");
string param = HttpUtility.UrlEncode("参数一", myEncoding) + "=" + HttpUtility.UrlEncode("值一", myEncoding) + "&" + HttpUtility.UrlEncode("参数二", myEncoding) + "=" + HttpUtility.UrlEncode("值二", myEncoding);
byte[] postBytes = Encoding.ASCII.GetBytes(param);
HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create( "http://www.jb51.net/" );
req.Method = "POST";
req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
req.ContentLength = postBytes.Length;
using (Stream reqStream = req.GetRequestStream())
{
	reqStream.Write(bs, 0, bs.Length);
}
using (WebResponse wr = req.GetResponse())
{
	//在这里对接收到的页面内容进行处理
}


*/