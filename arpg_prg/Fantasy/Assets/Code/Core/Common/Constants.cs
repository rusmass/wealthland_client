using UnityEngine;

namespace Core
{
	public static class Constants
	{
		public static readonly string LogPath 				= Application.persistentDataPath + "/game.log";
		public static readonly string LastLogPath 			= Application.persistentDataPath + "/last-game.log";

		public const string LocalApkDirectory				= "assets/Raw/";

		public static readonly string BuiltinMappingPath	= "mapping";

		public static readonly byte[] EmptyBytes 			= new byte[0];

		public static readonly string[] BuiltinFileExtensions = new string[]{BundleExtension, MetadataExtension, PathTools.PlatformResFolder};

		public const string BundleExtension        			= ".ab";    // file extension for AssetBundle files
		public const string MetadataExtension 				= ".gd";
        public const string TextureExtension                = ".png";

		public static readonly Vector2 UiResolution			= new Vector2(960, 540);
//		public static readonly Vector2 UiResolution			= new Vector2(960, 640);

		public const string AssetBundleFoldersPath 			= "Assets/assetBundleFolderNames.xml";
	}
}
