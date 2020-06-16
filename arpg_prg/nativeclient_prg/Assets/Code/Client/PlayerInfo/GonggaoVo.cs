using System;

/// <summary>
///  公告数据单元
/// </summary>
public class GonggaoVo
{
    public GonggaoVo()
    {
    }
    /// <summary>
    /// 标题
    /// </summary>
	public string title;

    /// <summary>
    ///  内容
    /// </summary>
	public string content;

    /// <summary>
    /// The type. 公告的类型，0是文字  ， 1是图片
    /// </summary>
    public int type = 0;

    /// <summary>
    /// 跳转的网页链接
    /// </summary>
    public string targetUrl = "";

    /// <summary>
    /// 时是否有链接跳转
    /// </summary>
    public bool isUrl = false;
}

