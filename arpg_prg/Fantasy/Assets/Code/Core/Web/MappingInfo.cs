using System;
using System.IO;

namespace Core.Web
{
	public enum LoadType : ushort
	{
		Disk,
		Builtin,
		Count,
	}

	public struct MappingInfo
	{
		public MappingInfo(string fullpath, LoadType loadType = LoadType.Disk) : this()
		{
			if (!string.IsNullOrEmpty (fullpath) && File.Exists (fullpath)) 
			{
				localPath = os.path.normpath(PathTools.GetLocalPath(fullpath));

				var bytes = File.ReadAllBytes(fullpath);
				localPathWithDigest = localPath + "." + Md5sum.Instance.GetAssetDigest(bytes);

				this.selfSize = os.path.getsize(fullpath);
				this.totalSize = 0;
				this.loadType = loadType;
			}
		}

		//WWW or LoadFromCacheOrDownload use
		public string GetUrl()
		{
			return WebManager.Instance.GetUrl (loadType, localPathWithDigest);
		}

		//CreateFromFile use
		public string GetFullPath()
		{
			return WebManager.Instance.GetFullPath (loadType, localPathWithDigest);
		}

		public string 	localPath;
		public string 	localPathWithDigest;

		public long 	selfSize { get; set;}
		public long 	totalSize { get; set;}
		public LoadType loadType { get; internal set;}
	}
}