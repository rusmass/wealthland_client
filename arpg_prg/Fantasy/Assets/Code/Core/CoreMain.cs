using System;
using System.Collections;
using UnityEngine;
using Core.IO;
using Core.Web;
using Metadata;

namespace Core
{
	public sealed class CoreMain
	{
		static CoreMain()
		{

		}

		private CoreMain ()
		{

		}

		public void Init()
		{
			if (_isInited)
			{
				return ;
			}

			_isInited = true;

			os.mkdir (PathTools.DiskPath);

			Console.Init ();
			_Log.InitLogInfo ();

			Console.WriteLine("[UniqueMain.Init()]\n{0}\nplatform={1}\nos={2}\ndevice={3}\nprocessorCount={4}\nsystemMemorySize={5}\ngraphicsDevice={6}\ngraphicsMemorySize={7}\n" +
				"logPath={8}\ndataPath={9}\nstreamingAssetsPath={10}\npersistentDataPath={11}\ntemporaryCachePath={12}"
			                  , DateTime.Now.ToString("yyyy-M-d HH:mm ddd")
			                  , Application.platform.ToString()
			                  , SystemInfo.operatingSystem
			                  , SystemInfo.deviceModel
			                  , SystemInfo.processorCount
			                  , SystemInfo.systemMemorySize.ToString()
			                  , SystemInfo.graphicsDeviceName
			                  , SystemInfo.graphicsMemorySize.ToString()
			                  , Constants.LogPath
			                  , Application.dataPath
			                  , Application.streamingAssetsPath
			                  , Application.persistentDataPath
							  , Application.temporaryCachePath);

			_webManager.Init ();
		}

		public void Tick(float deltaTime)
		{
			if (_isInited) 
			{
				os.frameCount = Time.frameCount;
				os.time = Time.time;

				Console.Tick ();
				Loom.Tick();
				_Log.Tick();
				CoroutineManager.Tick();
				DisposableRecycler.Tick();
			}
		}

		public void Dispose()
		{
			if (_isInited) 
			{
				CoroutineManager.StopAllCoroutines ();
				_Log.Dispose ();

				_isInited = false;
				Console.WriteLine("[CoreMain.Dispose()]");

				WebPrefab._GetLruCache ().Clear ();
				PrefabPool._GetLruCache ().Clear ();
				_webManager.Dispose();
            }
		}

		public static readonly CoreMain Instance = new CoreMain();
		public bool IsInited { get { return _isInited; } }

		private bool _isInited = false;
		private static readonly LogCollector _Log = LogCollector.Instance;
		private static readonly WebManager _webManager = WebManager.Instance;
	}
}

