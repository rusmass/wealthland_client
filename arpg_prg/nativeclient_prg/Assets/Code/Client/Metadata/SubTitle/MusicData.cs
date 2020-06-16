using System;

namespace Metadata
{
    /// <summary>
    ///  背景音乐
    /// </summary>
	[ExportAttribute(ExportFlags.ExportRaw)]
	public partial class MusicData:Config
	{
        /// <summary>
        /// 背景音乐第一阶段舒缓 bgm.mp3
        /// </summary>
        public string backGroundAndio;

		public string clickBtn;

		public string login;

		public string readyMusic;

		public string goMusic;

		public string gameWin;

		public string gameLose;

		public string throwCraps;

		public string move;

      
		public string bgMusic1;
       
		public string bgMusic2;
       
		public string bgMusic3;

		public string slectRole;

		public string startBgSound;
        /// <summary>
        /// 游戏第三阶段的音乐 bg01 
        /// </summary>
        public string bgNewAdd1;  
        /// <summary>
        /// 游戏第四阶段的音乐 bg02
        /// </summary>
        public string bgNewAdd2;
        /// <summary>
        /// 游戏中第二阶段的音乐  bg03
        /// </summary>
        public string bgNewAdd3;

        /// <summary>
        /// 生孩子大笑的声音
        /// </summary>
        public string getBaby;


	}
}

