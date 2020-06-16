using UnityEngine;
using Core;
using System.Collections;
using Client.UI;

namespace Client
{
#if UNITY_EDITOR
	public partial class UIUpdateSelectWindow
	{
		private void _ResetUISize()
		{
			_textWidth = Screen.width / 5;
			_textHeight = Screen.height / 10;
		}

		private void Start()
		{
			
		}

		private void OnGUI()
		{
			_ResetUISize();

			if (GUILayout.Button(_fromInternetText, GUILayout.Width(_textWidth), GUILayout.Height(_textHeight)))
			{
				DownloadFromInternet = true;
			}

			if (GUILayout.Button(_fromLocalText, GUILayout.Width(_textWidth), GUILayout.Height(_textHeight)))
			{
				UseLocalResource = true;
			}
		}
			
		public static bool DownloadFromInternet;
		public static bool UseLocalResource;

		private int _textWidth;
		private int _textHeight;
		private static readonly string _fromInternetText = "Internet";
		private static readonly string _fromLocalText = "Local";
	}
#endif
}
