using System;

public interface ILocalConfigManamger
{
	void SaveValue(string key, float value);
	void SaveValue(string key, string value);
	void SaveValue(string key, int value);

	float LoadValue(string key, float defaultValue);
	string LoadValue(string key, string defaultValue);
	int LoadValue(string key, int defaultValue);

	void DeleteValue(string key);
	void SaveImmediately();

	bool IsInited { get; }
	bool IsModify { set; get; }
}
