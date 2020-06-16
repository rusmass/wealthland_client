using System;
using cn.sharesdk.unity3d;


/// <summary>
/// shareSdk分享信息
/// </summary>
public class ShareContentInfor
{
	private static ShareContentInfor _instance;

	public static ShareContentInfor Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = new ShareContentInfor ();
			}

			return _instance;
		}
	}

	private ShareContentInfor ()
	{
		
	}

	public void SetShareContent(string title,string sharetxt,string imgUrl , string weburl)
	{
		ShareContent content = new ShareContent();
		content.SetTitle(title);
		content.SetText(sharetxt);
		content.SetImageUrl(imgUrl);
		content.SetTitleUrl(weburl);
		content.SetSite("智富人生");
		content.SetSiteUrl(weburl);
		content.SetUrl(weburl);
//		content.SetContentType (ContentType.Webpage);
		content.SetShareType (ContentType.Webpage);

		normalTitleContent = content;
	}


	public void SetShareRoomContent(string title,string sharetxt,string imgUrl , string weburl)
	{
		roomShareTxt = sharetxt;
		ShareContent content = new ShareContent();
		content.SetTitle(title);
		content.SetText(sharetxt);
		content.SetImageUrl(imgUrl);
		content.SetTitleUrl(weburl);
		content.SetSite("智富人生");
		content.SetSiteUrl(weburl);
		content.SetUrl(weburl);        

		content.SetShareType (ContentType.Webpage);
		roomFightContent = content;
	}

	public void setShareRoomTxt(string roomId)
	{
//		var tmpstr = string.Format (roomShareTxt, roomId);
//		Console.WriteLine("储存的信息是--------："+roomShareTxt);
		var tmpstr =roomShareTxt.Replace("-",roomId);
		roomFightContent.SetText (tmpstr);
	}

    /// <summary>
    /// 设置提交梦想板content
    /// </summary>
    /// <param name="title"></param>
    /// <param name="sharetxt"></param>
    /// <param name="imgUrl"></param>
    /// <param name="weburl"></param>
    public void SetShareDream(string title, string sharetxt, string imgUrl, string weburl)
    {
        ShareContent content = new ShareContent();
        content.SetTitle(title);
        content.SetText(sharetxt);
        content.SetImageUrl(imgUrl);
        content.SetTitleUrl(weburl);
        content.SetSite("智富人生");
        content.SetSiteUrl(weburl);
        content.SetUrl(weburl);
        //content.SetContentType (ContentType.Webpage);
        content.SetShareType(ContentType.Webpage);
        dreamShareContent = content;
    }

    /// <summary>
    /// 设置分享的按钮
    /// </summary>
    /// <param name="weburl"></param>
    public void SetShareDreamUrl(string weburl)
    {
        dreamShareContent.SetTitleUrl(weburl);
        dreamShareContent.SetSite("智富人生");
        dreamShareContent.SetSiteUrl(weburl);
        dreamShareContent.SetUrl(weburl);
    }


	private string roomShareTxt="";

	public ShareContent normalTitleContent;
	public ShareContent roomFightContent;

    /// <summary>
    /// 分享梦想板content
    /// </summary>
    public ShareContent dreamShareContent;
}

