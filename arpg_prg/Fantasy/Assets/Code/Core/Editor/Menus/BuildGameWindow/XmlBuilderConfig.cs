using System;

namespace Core.Menus
{
	[Serializable]
    public abstract class ConfigBase
    {
		public abstract bool IsSavable ();

		public bool builtinResources;
		public bool openLog;
    }

	[Serializable]
	public class AndroidConfig: ConfigBase
	{
		public override bool IsSavable ()
		{
			return !string.IsNullOrEmpty(apkPath);
		}
        
        public string apktoolPath;
		public string jarsignerPath;
		public string adbPath;
		public string apkPath;

		public string keystoreName;
		public string keystorePass;
		public bool	  autoInstall;
	}
	
	[Serializable]
	public class IPhoneConfig: ConfigBase
	{
		public override bool IsSavable ()
		{
			return !string.IsNullOrEmpty(projectPath);
        }

        public string projectPath;
		public bool ShowBuiltPlayer;
    }
	
	[Serializable]
	public class StandaloneConfig: ConfigBase
	{
		public override bool IsSavable ()
		{
			return !string.IsNullOrEmpty(gamePath);
        }

        public string gamePath;
		public bool ShowBuiltPlayer;
	}

	[Serializable]
	public class XmlBuilderConfig
	{
		public AndroidConfig android = new AndroidConfig();
		public IPhoneConfig iphone = new IPhoneConfig();
		public StandaloneConfig standalone = new StandaloneConfig();
	}
}