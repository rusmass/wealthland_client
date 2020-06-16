using UnityEngine;
using System.Collections;
using Client;
using UnityEngine.UI;

public class AndroidPhoneCallBack : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//
	public void HeadImage(string str="")
	{
		//			StartCoroutine(LoadTexture());
		MessageHint.Show ("dddddddd截图成功了吗啊 啊啊啊啊啊"+str);
		LoadTexture();

	}



	private void LoadTexture()
	{
		if (null != img_selecthead)
		{
			AsyncImageDownload.Instance.LoadLocalImage (img_selecthead);
		}

		//注解1
		//			string path = "file://" + Application.persistentDataPath + "/" + "image.jpg";

		//			AsyncImageDownload.Instance.SetAsyncImage (headpath,img_selecthead);
		//			AsyncImageDownload.Instance.

		//			WWW www = new WWW(path);
		//			while (!www.isDone)
		//			{
		//
		//			}
		//			yield return www;
		//			//为贴图赋值
		//			RawImage inma = transform.Find("RawImage").GetComponent<RawImage>();
		//			inma.texture = www.texture;
	}

	public Image img_selecthead;
}

