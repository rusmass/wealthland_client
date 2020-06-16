using System;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar:MonoBehaviour
{
	public Image targetObj;
	public Image progressBar;

	public Direction direction;

	private Vector3 _barPosition;

	private float _tmpPercent=0;

	void Start()
	{
		_barPosition = progressBar.transform.localPosition;

		var tmpRect = targetObj.GetComponent<RectTransform>().sizeDelta;

		if(direction==Direction.Horizontal)
		{			
			progressBar.transform.localPosition = new Vector3 (_barPosition.x,_barPosition.y,_barPosition.x);
		}
		else
		{
			progressBar.transform.localPosition = new Vector3 (_barPosition.x,_barPosition.y,_barPosition.z);
		}

		_barPosition = progressBar.transform.localPosition;

		if(_tmpPercent !=0)
		{
			SetPersent (_tmpPercent);
		}
	}

	public void SetPersent(float per)
	{
		if (per > 1f)
		{
			per = 1;
		}
		if (per < 0)
		{
			per = 0;
		}

		_tmpPercent = per;

		if(direction==Direction.Horizontal)
		{
			
		}
		else
		{
			
		}

	}

	public enum Direction
	{
		Horizontal,
		Vertical
	}
}
	


