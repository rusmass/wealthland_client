using UnityEngine;
using System.Collections.Generic;

/// <summary>
///  显示帧频测试的，可以显示游戏的性能
/// </summary>
public class FPSDisplay : MonoBehaviour
{
	private Queue<float> lastDeltaTimes;
	private float fps = 0;
	private float fixedFps = 0;
	private float deltaTime = 0;
	private float msec = 0;
	private GUIStyle style;
    private int lastFps = 0;
    private float lastFpsTime = 0;
    public RenderTexture temporary;

    void Awake()
	{
		style = new GUIStyle();
		style.normal.textColor = Color.green;
		style.alignment = TextAnchor.MiddleLeft;
		GUIStyle b = new GUIStyle();
		b.normal.textColor = Color.black;
		b.alignment = TextAnchor.MiddleLeft;
		style.fontSize = 24;
	}
	void Start()
	{
		lastDeltaTimes = new Queue<float>();
	}

	void Update()
	{
		deltaTime +=(Time.deltaTime-deltaTime) * 0.1f;
		lastDeltaTimes.Enqueue(Time.deltaTime/Time.timeScale);
		if(lastDeltaTimes.Count > 10)
		{
			lastDeltaTimes.Dequeue();
		}
		float allTime = 0;
		int count = lastDeltaTimes.Count;
		if(count == 0)
		return;
		foreach(var time in lastDeltaTimes)
		{
			allTime +=time;
		}
		fps = count/allTime;
	}
	private void FixedUpdate()
	{
		lastFps++;
		if(lastFpsTime + 1 < Time.realtimeSinceStartup)
		{
			fixedFps = lastFps;
			lastFpsTime = Time.realtimeSinceStartup;
			lastFps = 0;
		}
	}

	void OnGUI()
	{
		float textHeight = 30;
		msec = deltaTime * 1000.0f;
		string fpsText = string.Format("FPS:{0}({1:0.0}ms)",fps.ToString("f2"),msec);
		GUI.Label(new Rect(Screen.width/2-100,textHeight * 0,2000,textHeight),fpsText,style);

		string fixedFpsText = string.Format("FixedFPS:{0}",fixedFps.ToString("f2"));
		GUI.Label(new Rect(Screen.width/2-100,textHeight * 1,2000,textHeight),fixedFpsText,style);
	}
}