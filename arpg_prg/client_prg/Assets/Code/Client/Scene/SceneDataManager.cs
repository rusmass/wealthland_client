using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Metadata;

namespace Client.Scenes
{
    /// <summary>
    /// Î´ÒýÓÃ
    /// </summary>
	public class SceneDataManager 
	{
		public void InitSceneDateDefault()
		{
			this._sceneDateTemp = new SceneDataTemplate();
		}

		public void Clear()
		{
			_sceneDateTemp = null;
		}

		public SceneDataTemplate SceneDateTemplate
		{
			get{return _sceneDateTemp;}
			private set{_sceneDateTemp = value;}
		}

		private SceneDataTemplate _sceneDateTemp;

		public static readonly SceneDataManager Instance = new SceneDataManager();

	}
}







