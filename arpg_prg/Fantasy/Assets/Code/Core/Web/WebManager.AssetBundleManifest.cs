using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace Core.Web
{
	public partial class WebManager
	{
		private void _LoadDefaultAssetBundleManifest()
		{
			CoroutineManager.StartCoroutine (_LoadDefaultAssetBundleManifest_All ());
		}

		private IEnumerator _LoadDefaultAssetBundleManifest_All()
		{
			var url = GetUrl (LoadType.Builtin, PathTools.PlatformResFolder);
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

			var assetBundle = www.assetBundle;
			UnityEngine.Object obj = assetBundle.LoadAsset("AssetBundleManifest");
			assetBundle.Unload(false);
			Manifest = obj as AssetBundleManifest;	

			_isInited = true;
		}
	}
}