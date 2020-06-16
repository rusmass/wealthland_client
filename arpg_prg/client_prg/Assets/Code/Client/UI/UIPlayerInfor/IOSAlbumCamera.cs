using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

using UnityEngine.UI;


public class IOSAlbumCamera : MonoBehaviour
{
	[DllImport ("__Internal")]
	private static extern void _iosOpenPhotoLibrary();
	[DllImport ("__Internal")]
	private static extern void _iosOpenPhotoAlbums();
	[DllImport ("__Internal")]
	private static extern void _iosOpenCamera();
	[DllImport ("__Internal")]
	private static extern void _iosOpenPhotoLibrary_allowsEditing();	
	[DllImport ("__Internal")]
	private static extern void _iosOpenPhotoAlbums_allowsEditing();	
	[DllImport ("__Internal")]
	private static extern void _iosOpenCamera_allowsEditing();
	[DllImport ("__Internal")]
	private static extern void _iosSaveImageToPhotosAlbum(string readAddr);

    /// <summary>
    /// 打开照片
    /// </summary>
    /// <param name="allowsEditing"></param>
	public static void iosOpenPhotoLibrary(bool allowsEditing=false)
	{

		if (allowsEditing)
			_iosOpenPhotoLibrary_allowsEditing ();
		else
			_iosOpenPhotoLibrary ();
	}

    /// <summary>
    /// 打开相册
    /// </summary>
    /// <param name="allowsEditing"></param>
	public static void iosOpenPhotoAlbums(bool allowsEditing=false)
	{
		if (allowsEditing)
			_iosOpenPhotoAlbums_allowsEditing ();
		else
			_iosOpenPhotoAlbums ();
	}



    /// <summary>
    /// 打开相机
    /// </summary>
    /// <param name="allowsEditing"></param>
	public static void iosOpenCamera(bool allowsEditing=false)
	{
		if (allowsEditing)
			_iosOpenCamera_allowsEditing ();
		else
			_iosOpenCamera ();
	}

    /// <summary>
    /// 保存图片到相册
    /// </summary>
    /// <param name="readAddr"></param>
	public static void iosSaveImageToPhotosAlbum(string readAddr)
	{
		_iosSaveImageToPhotosAlbum (readAddr);
	}

    /// <summary>
    /// 将ios传过的string转成u3d中的texture
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
	public static Texture2D Base64StringToTexture2D(string base64)
	{
		Texture2D tex = new Texture2D (4, 4, TextureFormat.ARGB32, false);
		try
		{
			byte[] bytes = System.Convert.FromBase64String(base64);
			tex.LoadImage(bytes);
		}
		catch(System.Exception ex)
		{
            Debug.LogError(ex.Message);
			Client.MessageHint.Show ("获取图片失败了，失败信息；；"+ex.Message);
		}
		return tex;
	}    


	public Image img_selecthead;

	private static IOSAlbumCamera _instance;
	public static IOSAlbumCamera Instance{get{ return _instance; }}

	public System.Action<string> CallBack_PickImage_With_Base64;
	public System.Action<string> CallBack_ImageSavedToAlbum;

	void Awake()
	{
		if (_instance != null)
		{
			DestroyImmediate(this);
			return;
		}
		_instance = this;
//		GameObject go = new GameObject ("IOSAlbumCamera");
//		_instance = go.AddComponent<IOSAlbumCamera> ();
	}

	/// <summary>
	/// 打开相册相机后的从ios回调到unity的方法
	/// </summary>
	/// <param name="base64">Base64.</param>
	void PickImageCallBack_Base64(string base64)
	{
//		if(CallBack_PickImage_With_Base64!=null)
//		{
//			CallBack_PickImage_With_Base64(base64);
//		}
		Texture2D tex = IOSAlbumCamera.Base64StringToTexture2D (base64);

//		string warningStr = "当前相册截图是否存在~~~" + (tex ！= null).ToString () + "纹理输出：" + tex.ToString ();
//		Client.MessageHint.Show (warningStr);
		//Console.Error.WriteLine ("当前相册截图的纹理集合："+(tex==null).ToString()+"纹理输出："+tex.ToString());

		if(null!=tex)
		{
			//HttpRequestManager.GetInstance ().UpLoadImage (tex);	

			if (null != img_selecthead)
			{
				Sprite m_sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));  
				img_selecthead.sprite = m_sprite; 
			}
		}
		else
		{
			Client.MessageHint.Show("获取纹理集失败");
		}


//		img_selecthead.texture = tex;
	}

    /// <summary>
    /// 保存图片到相册后，从ios回调到unity的方法
    /// </summary>
    /// <param name="msg">Message.</param>
    void SaveImageToPhotosAlbumCallBack(string msg)
	{
		Debug.Log (" -- msg: " + msg);
		if(CallBack_ImageSavedToAlbum!=null)
		{
			CallBack_ImageSavedToAlbum(msg);
		}
	}
}
