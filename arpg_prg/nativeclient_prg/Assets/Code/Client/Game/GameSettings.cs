using Core;
using System;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 游戏设置，未引用
    /// </summary>
    public class GameSettings
	{
		private GameSettings ()
		{
			
		}

		public static readonly GameSettings Instance = new GameSettings();
	}
}

