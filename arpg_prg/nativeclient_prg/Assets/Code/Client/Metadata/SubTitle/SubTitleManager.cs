using System;

namespace Metadata
{
    /// <summary>
    /// 管理游戏中的配置的话术
    /// </summary>
	public class SubTitleManager
	{
		private SubTitleManager()
		{
            subtitle = ConfigManager.Instance.GetConfig<SubtitleCount>();
			musicData = ConfigManager.Instance.GetConfig<MusicData> ();
        }

		public SubtitleCount subtitle;
		public MusicData musicData;

		public static readonly SubTitleManager Instance = new SubTitleManager();
	}
}

