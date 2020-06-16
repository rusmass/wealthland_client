using System;

public class LocalConfigManager : ILocalConfigManamger
{
	public void SaveValue(string key, float value)
	{
		_SaveValue(key, value);
	}

	public void SaveValue(string key, string value)
	{
		_SaveValue(key, value);
	}

	public void SaveValue(string key, int value)
	{
		_SaveValue(key, value);
	}

	public float LoadValue(string key, float defaultValue)
	{
		return _LoadValue(key, defaultValue);
	}

	public string LoadValue(string key, string defaultValue)
	{
		return _LoadValue(key, defaultValue);
	}

	public int LoadValue(string key, int defaultValue)
	{
		return _LoadValue(key, defaultValue);
	}

	public void DeleteValue(string key)
	{
		_DeleteValue(key);
	}

	public void SaveImmediately()
	{
		UnityEngine.PlayerPrefs.Save ();
	}

	private void _SaveValue(string key, float value)
	{
		if (value.Equals(UnityEngine.PlayerPrefs.GetFloat(key))) { return; }

		IsModify = true;
		UnityEngine.PlayerPrefs.SetFloat(key, value);
	}

	private void _SaveValue(string key, string value)
	{
		if (value.Equals(UnityEngine.PlayerPrefs.GetString(key))) { return; }

		IsModify = true;
		UnityEngine.PlayerPrefs.SetString(key, value);
	}

	private void _SaveValue(string key, int value)
	{
		if (value.Equals(UnityEngine.PlayerPrefs.GetInt(key))) { return; }

		IsModify = true;
		UnityEngine.PlayerPrefs.SetInt(key, value);
	}

	private float _LoadValue(string key, float defaultValue)
	{
		return UnityEngine.PlayerPrefs.GetFloat(key, defaultValue);
	}

	private string _LoadValue(string key, string defaultValue)
	{
		return UnityEngine.PlayerPrefs.GetString(key, defaultValue);
	}

	private int _LoadValue(string key, int defaultValue)
	{
		return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
	}

	private void _DeleteValue(string key)
	{
		UnityEngine.PlayerPrefs.DeleteKey(key);
	}

	public bool IsInited { get { return true; } }
	public bool IsModify { set { bModify = value; } get { return bModify; }}
	private bool bModify = false;
}