using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace Core.Web
{
	public partial class WebManager
	{
		public void AddMappingInfos(byte[] bytes, LoadType loadType)
		{
			if (null != _mappingDB) 
			{
				_mappingDB.AddMappingInfos(bytes, loadType);
			}
		}

		private void _LoadDefaultMappingInfo()
		{
			if (os.isAndroid) 
			{
				CoroutineManager.StartCoroutine(_LoadDefaultMappingInfo_Android());
			}
			else if (os.isIPhonePlayer) 
			{
				_LoadDefaultMappingInfo_IPhone();
			}
			else if(!os.isEditor)
			{
				Console.Error.WriteLine("[WebManager._LoadDefaultMappingInfo] platform is error.");
			}
			else
			{
				_LoadDefaultAssetBundleManifest ();
			}
		}

		private IEnumerator _LoadDefaultMappingInfo_Android()
		{
			var url = GetUrl (LoadType.Builtin, Constants.BuiltinMappingPath);
			var www = new WWW (url);

			while (!www.isDone) 
			{
				yield return null;
			}

			if (null != www.error) 
			{
				Console.Error.WriteLine("[WebManager._LoadDefaultMappingInfo_Android] www.error = {0}", www.error);
				yield break;
			}

			var bytes = www.bytes;
			os.dispose (ref www);
		
			_mappingDB.AddMappingInfos (bytes, LoadType.Builtin);

			_LoadDefaultAssetBundleManifest ();
		}

		private void _LoadDefaultMappingInfo_IPhone()
		{
			var builtinPath = GetFullPath (LoadType.Builtin, Constants.BuiltinMappingPath);
			if (File.Exists (builtinPath)) 
			{
				var bytes = File.ReadAllBytes(builtinPath);
				_mappingDB.AddMappingInfos(bytes, LoadType.Builtin);
			}

			_LoadDefaultAssetBundleManifest ();
		}

		public MappingInfo GetMappingInfo(string localPath)
		{
			localPath = localPath ?? string.Empty;
			MappingInfo info;
			
			if (!_mappingDB.TryGetMappingInfo(localPath, out info) && os.isEditor)
			{
				var localPathWithDigest = localPath;
				var fullPath = PathTools.GetExportPath(localPath);
				
				info = new MappingInfo()
				{
					localPath = localPath
					,localPathWithDigest = localPathWithDigest
					,selfSize = os.path.getsize(fullPath)
					,totalSize = 0
					,loadType = LoadType.Disk
				};
				
				_mappingDB.AddToDB(localPath, info);
			}
			
			return info;
		}

		internal string GetFullPath (LoadType loadType, string localPathWithDigest)
		{
			if (null != localPathWithDigest)
			{
				var index = (int) loadType;
				var url = os.path.join(_basePaths[index], localPathWithDigest);
				return url;
			}
			
			return string.Empty;
		}
		
		internal string GetUrl (LoadType loadType, string localPathWithDigest)
		{
			if (null != localPathWithDigest)
			{
				var index = (int) loadType;
				var url = os.path.join(_baseUrls[index], localPathWithDigest);
				return url;
			}
			
			return string.Empty;
		}

		private readonly MappingDB _mappingDB = new MappingDB();
	}
}