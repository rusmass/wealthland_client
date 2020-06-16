using Core;
using System;
using Core.IO;
using Core.Web;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Core.Web
{
	internal partial class InnerWebPrefab: ASmartPointer, IIsYieldable, ILocalPath
    {
		internal InnerWebPrefab (WebArgument argument)
		{
			argument.flags |= WebFlags.UnloadAllLoadedObjects;
			this.localPath = argument.localPath;			
			this.argument = argument;
			var mainArgument = new WebArgument{ localPath= localPath, flags = argument.flags };
			var mainWeb = WebManager.Instance.LoadWebItem(mainArgument, _OnLoadMainWeb);
			_InitCallback_MainWeb(mainWeb);
		}

        protected override void _DoDispose (bool isDisposing)
        {
			_RestoreMainAssetBundle ();

			os.dispose(ref _mainWeb);
			_partGroutp = null;
		}
        
        private void _InitCallback_MainWeb (WebItem mainWeb)
        {
            _mainWeb  = mainWeb;
            size  = mainWeb.size;
        }
        
        private void _OnLoadMainWeb (WebItem web)
        {
            _InitCallback_MainWeb(web);
            
            if (null != web.error)
            {
				error = string.Format ("[InnerWebPrefab._OnLoadMainAsset()] {0}, web.error={1}, web={2}"
					, Path.GetFileName (web.argument.localPath), web.error, web);
                _OnLoadAll(web);
                return;
            }
            
			var bundle = web.assetBundle;
            if (null == bundle)
            {
                error = string.Format("[InnerWebPrefab._OnLoadMainAsset()] bundle is null, web={0}", web);
                _OnLoadAll(web);
                return;
            }
            
			var manif = WebManager.Instance.Manifest;
			string[] dependences = manif.GetAllDependencies(web.argument.localPath);
			if (null == dependences)
            {
				error = string.Format("[InnerWebPrefab._OnLoadMainAsset()] dependences is null, web={0}", web);
                _OnLoadAll(web);
                return;
            }

			var nodes = new IWebNode[dependences.Length];
			for (int i = 0; i < dependences.Length; ++i) 
			{
				var arg = new WebArgument { localPath = dependences [i], flags = WebFlags.UnloadAllLoadedObjects };
				var sharedPart = WebManager.Instance.LoadWebItem (arg, _OnLoadSimplePart);
				nodes[i] = sharedPart;
			}

			_partGroutp = new WebGroup (nodes, _OnLoadAll);
			web.HoldBabies (ref nodes);
        }

        private static void _OnLoadSimplePart (WebItem web)
        {
            var bundle = web.assetBundle;

			if (null != web.error || null == bundle) 
			{
				return;
			}
            
			bundle.LoadAllAssets ();
        }
        
        private void _OnLoadAll (IWebNode web)
        {
            isDone = true;
            if (null != error)
            {
                Console.Error.WriteLine(error);
            }
        }
        
        public override string ToString ()
        {
			return string.Format("[InnerWebPrefab: id={0}, localPath={1}, argument={2}, progress={3}, isDone={4}" +
                                 ", IsDisposed={5}, referCount={6}, _partGroup={7}, mainAsset={8}]"
								 , argument.localPath
			                     , argument.ToString()
                                 , progress.ToString()
                                 , isDone.ToString()
                                 , IsDisposed().ToString()
                                 , GetReference().ToString()
								 , null != _partGroutp ? _partGroutp.ToString() : string.Empty
                                 , _mainAsset);
        }
        
        public float progress
        {
            get
            {
                if (isDone)
                {
                    return 1.0f;
                }
                
                float total = 0.0f;
                
                if (null != _mainWeb)
                {
                    total += _mainWeb.progress;
                }
                
				if (null != _partGroutp)
                {
					total += _partGroutp.progress * _partGroutp.size;
                }
                
                var current = size > 0 ? total / size : 0.0f;
                return current;
            }
        }

		public GameObject mainAsset
		{
			get
			{
				if (null == _mainAsset)
				{
					_BuildMainAsset ();

					if (os.isEditor)
                    {
                        _ProcessDependenciesInEditor(_mainAsset);
                    }
				}

				return _mainAsset;
			}
		}

		private void _BuildMainAsset ()
		{
			if (null != _mainWeb)
			{
				var bundle = _mainWeb.assetBundle;

				if (null != bundle)
				{
					_mainAsset = bundle.LoadAsset (bundle.GetAllAssetNames () [0]) as GameObject;
					var script = _mainAsset.AddComponent<MBPrefabAid>();
					script.localPath = localPath;
				}
			}
		}

		private void _RestoreMainAssetBundle ()
		{
			if (null != _mainAsset)
			{
				var script = _mainAsset.GetComponent<MBPrefabAid>();
				GameObject.DestroyImmediate(script, false);
				_mainAsset = null;
			}
		}

		public string localPath 			{ get; set;}

        public WebArgument argument         { get; private set; }
        
        public bool isDone                  { get; private set; }

		bool IIsYieldable.isYieldable		{ get { return isDone; } }

        public string error                 { get; private set; }
        
        public long size                    { get; private set; }

		private WebGroup 	_partGroutp;
		private GameObject	_mainAsset;
        private WebItem     _mainWeb;
    }
}
