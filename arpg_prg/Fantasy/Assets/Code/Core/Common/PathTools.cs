using System;
using UnityEngine;
using Core.Reflection;

namespace Core
{
	public static class PathTools
	{
		public static string EditorResourceRoot
		{
			get
			{
				return System.IO.Path.GetFullPath("../../arpg_res/resources");
			}
		}

		public static string ExportResourceRoot
		{
			get
			{
				return os.path.join(EditorResourceRoot, PlatformResFolder);
			}
		}

        public static string MetadataResourceRoot
        {
            get
            {
                return System.IO.Path.GetFullPath("../../arpg_des/Metadata");
            }
        }

        public static string ExportMetadataRoot
        {
            get
            {
                return os.path.join(ExportResourceRoot, "metadata");
            }
        }

		public static string PlatformResFolder
		{
			get
            {
                if (os.isIPhonePlayer)
                {
                    return "ios";
                }
                else if (os.isAndroid)
                {
                    return "android";
                }
                else
                {
                    var targe = EditorBuildings.activeBuildTarget;
                    if (targe == TargetPlatform.Android)
                    {
                        return "android";
                    }
                    else if (targe == TargetPlatform.iPhone)
                    {
                        return "ios";
                    }
                    return "pc";
                }
			}
		}

		public static string DiskPath
		{
			get
			{
				switch(Application.platform)
				{
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.OSXEditor:
					return ExportResourceRoot;
				case RuntimePlatform.IPhonePlayer:
					return Application.temporaryCachePath + "/res";
				case RuntimePlatform.Android:
					return Application.persistentDataPath + "/res";

				default:
					throw new NotImplementedException("[PathTools.DiskPath] invalid platform type!");
				}
			}
		}

		public static string DiskUrl
		{
			get
			{
				return FileProtocolHead + DiskPath;
			}
		}

		public static string BuiltinPath
		{
			get 
			{
				switch(Application.platform)
				{
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.OSXEditor:
					return ExportResourceRoot;
				case RuntimePlatform.IPhonePlayer:
					return Application.streamingAssetsPath;
				case RuntimePlatform.Android: // android is special(builtIn can only read by www or loadfromcacheordownload)
					return DiskPath; 		  // so we uncompress some builtIn res to disk for use createfromfile

				default:
					throw new NotImplementedException("[PathTools.DiskPath] invalid platform type!");
				}
			}
		}

		public static string BuiltinUrl
		{
			get
			{
				switch(Application.platform)
				{
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.OSXEditor:
					return DiskUrl;
				case RuntimePlatform.IPhonePlayer:
					return FileProtocolHead + Application.streamingAssetsPath;
				case RuntimePlatform.Android:
					return Application.streamingAssetsPath + "/Raw";

				default:
					throw new NotImplementedException("[PathTools.BuiltInUrl] invalid platform type!");
				}
			}
		}

		public static string FileProtocolHead
		{
			get
			{
				switch(Application.platform)
				{
				case RuntimePlatform.WindowsEditor:
				case RuntimePlatform.OSXEditor:
					return "file:///";

				default:
					return "file://";
				}
			}
		}

		public static string GetLocalPath(string fullpath)
		{
			var head = ExportResourceRoot;

			if (string.IsNullOrEmpty (fullpath) || !fullpath.StartsWith(head)) 
			{
				return fullpath;
			}

			return fullpath.Substring (head.Length + 1);
		}

		public static string GetExportPath(string localPath)
		{
			return os.path.join(EditorResourceRoot ,PlatformResFolder, localPath);
		}

		internal static int LastIndexOfExtensionDot (string path)
		{
			if (null == path)
			{
				return -1;
			}

			var length = path.Length;
			if (length == 0)
			{
				return -1;
			}

			for (int i= length - 1; i>= 0; i--)
			{
				var c = path[i];
				if (c == '.')
				{
					return i;
				}
				else if (c == '/' || c =='\\')
				{
					return -1;
				}
			}

			return -1;
		}

		public static string GetLocalPathWithDigest (string localPath, string digest)
		{
			var lastDotIndex = LastIndexOfExtensionDot(localPath);
			if (lastDotIndex > 0)
			{
				var localPathWithoutExtension = localPath.Substring(0, lastDotIndex);
				var extension = localPath.Substring(lastDotIndex);

				var localPathWithDigest = localPathWithoutExtension + "." + digest + extension;
				return localPathWithDigest;
			}
			else
			{
				var localPathWithDigest = localPath + "." + digest;
				return localPathWithDigest;
			}
		}

		internal static string ExtractLocalPath (string localPathWithDigest)
		{
			var endDotIndex = LastIndexOfExtensionDot(localPathWithDigest);
			if (endDotIndex == -1)
			{
				return localPathWithDigest;
			}

			var startDotIndex = localPathWithDigest.LastIndexOf('.', endDotIndex - 1);
			var digestLength = endDotIndex - startDotIndex -1 ;

			if (digestLength != Md5sum.AssetDigestLength)
			{
				return localPathWithDigest;
			}

			var localPath = localPathWithDigest.Substring(0, startDotIndex) + localPathWithDigest.Substring(endDotIndex);
			return localPath;
		}

		internal static string ExtractAssetDigest (string localPathWithDigest)
		{
			var endDotIndex = LastIndexOfExtensionDot(localPathWithDigest);
			if (endDotIndex == -1)
			{
				return string.Empty;
			}

			var startDigestIndex = localPathWithDigest.LastIndexOf('.', endDotIndex - 1) + 1;
			var digestLength = endDotIndex - startDigestIndex;

			if (digestLength != Md5sum.AssetDigestLength)
			{
				return string.Empty;
			}

			var digest = localPathWithDigest.Substring(startDigestIndex, digestLength);
			return digest;
		}
	}
}