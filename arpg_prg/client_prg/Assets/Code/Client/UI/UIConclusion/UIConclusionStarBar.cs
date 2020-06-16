using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class UIConclusionStarBarItem
	{
		public UIConclusionStarBarItem (GameObject go)
		{
			lb_score = go.GetComponentEx<Text> (Layout.lb_score);
			for (var i = 0; i < _starNum; i++)
			{
				var img = go.GetComponentEx<Image> (string.Format(Layout.img_start,(i+1).ToString()));
				var tmpImgDisplay = new UIImageDisplay (img);
				_starArr [i] = tmpImgDisplay;
			}
		}

		/// <summary>
		/// Sets the scores. 设置评分和星级
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="stars">Stars.</param>
		public void SetScores(int value , int stars)
		{
			lb_score.text = value.ToString ();

			for (var i = 0; i < stars; i++)
			{
				var tmpItem = _starArr[i];
				tmpItem.Load (_starPath);
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Client.UI.UIConclusionStarBarItem"/> object.释放资源
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Client.UI.UIConclusionStarBarItem"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Client.UI.UIConclusionStarBarItem"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="Client.UI.UIConclusionStarBarItem"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Client.UI.UIConclusionStarBarItem"/> was occupying.</remarks>
		public void Dispose()
		{
			var tmplen = _starArr.Length;
			for (var i = 0; i < tmplen;i++)
			{
				var tmpItem = _starArr[i];
				tmpItem.Dispose ();
			}
		}


		private UIImageDisplay[] _starArr=new UIImageDisplay[5];
		private const int _starNum=5;

		private const string _starPath = "share/atlas/battle/conclusion/starnormal.ab";

		private Text lb_score;

		class Layout
		{
			public static string lb_score="score";
			public static string img_start = "star{0}";
		}

	}


}

