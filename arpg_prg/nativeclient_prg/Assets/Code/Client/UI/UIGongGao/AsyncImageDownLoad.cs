using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

/// <summary>
/// 异步下载网络图片
/// </summary>
public class AsyncImageDownload : MonoBehaviour
{
    public Sprite placeholder;

    private static AsyncImageDownload _instance = null;
    //public static AsyncImageDownload GetInstance() { return Instance; }  
    public static AsyncImageDownload Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("AsyncImageDownload");
                _instance = obj.AddComponent<AsyncImageDownload>();
                DontDestroyOnLoad(obj);
                _instance.Init();
            }
            return _instance;
        }
    }

    public bool Init()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/ImageCache/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/ImageCache/");
        }
        if (placeholder == null)
        {
            placeholder = Resources.Load("placeholder") as Sprite;
        }
        return true;

    }

    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="image"></param>
    public void SetAsyncImage(string url, Image image)
    {
        //开始下载图片前，将UITexture的主图片设置为占位图  
        //image.sprite = placeholder;
        //image.SetActiveEx(false);
        //判断是否是第一次加载这张图片  

        //StartCoroutine(DownloadImage(url, image));
        //return;

       
        if (!File.Exists(path + url.GetHashCode()))
        {
        //    //如果之前不存在缓存文件  
           StartCoroutine(DownloadImage(url, image));
        }
        else
        {
            StartCoroutine(LoadLocalImage(url, image));
        }

    }

    IEnumerator DownloadImage(string url, Image image)
    {
        Debug.Log("downloading new image:" + path + url.GetHashCode());//url转换HD5作为名字  
        WWW www = new WWW(url);
        yield return www;
        print("-------------xxx-----:" + null != www.error);
        Texture2D tex2d = www.texture;
       
        byte[] pngData = tex2d.EncodeToPNG();      
        if (null == www.error || www.error == "")
        {
            //将图片保存至缓存路径 
            File.WriteAllBytes(path + url.GetHashCode(), pngData);
        }
        Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
        if (null != image)
        {
            image.sprite = m_sprite;
            image.SetActiveEx(true);
        }       
    }

    IEnumerator LoadLocalImage(string url, Image image)
    {
        string filePath = "file:///" + path + url.GetHashCode();

        Debug.Log("getting local image:" + filePath);
        WWW www = new WWW(filePath);
        yield return www;

        Texture2D texture = www.texture;
        Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

        

        if (null!=image)
        {            
            image.sprite = m_sprite;
            image.SetActiveEx(true);
        }       
    }

    /// <summary>
	/// Loads the local image. 下载本地照片
	/// </summary>
	/// <param name="img">Image.</param>
	public void LoadLocalImage(Image img)
    {
        StartCoroutine(LoadLocalImg(img));
    }

    IEnumerator LoadLocalImg(Image ssimg)
    {
        string path = "file:///" + Application.persistentDataPath + "/" + "image.jpg";
        WWW www = new WWW(path);
        yield return www;
        //为贴图赋值
        //		RawImage inma = transform.Find("RawImage").GetComponent<RawImage>();
        //		inma.texture = www.texture;
        Texture2D texture = www.texture;

        if(null!=ssimg)
        {
            Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            ssimg.sprite = m_sprite;
        }

        //HttpRequestManager.GetInstance().UpLoadImage(texture);
    }

    public string path
    {
        get
        {
            //pc,ios //android :jar:file//  
            return Application.persistentDataPath + "/ImageCache/";
        }
    }



    public void HeadImage(Image str)
    {
        if (str != null)
            StartCoroutine(LoadTexture(str));
    }
    IEnumerator LoadTexture(Image str)
    {
        //注解1
        string path = "file://" + Application.persistentDataPath + "/" + "image.jpg";

        WWW www = new WWW(path);
        while (!www.isDone)
        {

        }
        yield return www;
        //为贴图赋值         
        str.sprite = Sprite.Create(www.texture, new Rect(0, 0, 200, 200), new Vector2(0, 0)); ;
    }
    public void ShowGameObjectFalse(GameObject obj,int tim)
    {
        StartCoroutine(ObjFalse(obj, tim));
    }
    
    IEnumerator ObjFalse(GameObject obj, int tim)
    {
        yield return new WaitForSeconds(tim);
        obj.SetActive(false);
    }
}

