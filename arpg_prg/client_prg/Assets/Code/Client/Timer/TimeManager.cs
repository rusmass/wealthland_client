
using UnityEngine;
using System.Collections;
/// <summary>
/// 描述：这个类用来处理计时功能，包括计时开始，暂停计函数
/// </summary>
public class TimeManager : MonoBehaviour {

	private float time_All = 300;//计时的总时间（单位秒）
	private float totalTime;//剩余时间
	private bool  isPauseTime = false;


	// Use this for initialization
	void Start () {
		totalTime = 0;
		PauseTimer();

        
	}

	// Update is called once per frame
	void Update () {
		if(!isPauseTime)
		{
			StartTimer();
		}

	}

	public static TimeManager smInstance;
	public static TimeManager Instance
	{
		get{return smInstance;}
	}

	void Awake()
	{
		smInstance = this;
	}

	/// <summary>
	/// 开始计时 
	/// </summary>
	void StartTimer(){
		totalTime += Time.deltaTime;
	}

	/// <summary>
	/// 获取游戏结束时分钟数
	/// </summary>
	/// <returns>The time minute.</returns>
	public string GetEndTime()
	{
		return GetMinute(totalTime);
	}

	/// <summary>
	/// 获取进入内圈时分钟数
	/// </summary>
	/// <returns>The time minute.</returns>
	public string GetEnterInnerTime()
	{
		return GetMinute(totalTime);
	}


	/// <summary>
	///继续游戏，这个暂时加在这里，后期代码重构时加在UIControl中 
	/// </summary>
	public void ContinueGame()
	{
		isPauseTime = false;
//		Time.timeScale  = 1;
	}

	/// <summary>
	/// 暂停计时
	/// </summary>
	public void PauseTimer()
	{
		isPauseTime = true; 
//		Time.timeScale = 0;
	}

	/// <summary>
	/// 获取总的时间字符串
	/// </summary>
	string GetTime(float time){
		
		return GetMinute (time) + GetSecond (time);
	}

	/// <summary>
	/// 获取小时
	/// </summary>
	string GetHour(float time){
		int timer = (int)(time / 3600);
		string timerStr;
		if (timer < 10)
			timerStr = "0" + timer.ToString () + ":";
		else 
			timerStr = timer.ToString () + ":";
		return timerStr;
	}

	/// <summary>
	///获取分钟 
	/// </summary>
	string GetMinute(float time){
		int timer = (int)((time % 3600)/60);
		string timerStr;
		if (timer < 10)
			timerStr = "0" + timer.ToString ();
		else 
			timerStr = timer.ToString ();
		return timerStr;
	}

	/// <summary>
	/// 获取秒
	/// </summary>
	string GetSecond(float time){
		int timer = (int)((time % 3600)%60);
		string timerStr;
		if (timer < 10)
			timerStr = "0" + timer.ToString ();
		else 
			timerStr = timer.ToString ();

		return timerStr;
	}
}