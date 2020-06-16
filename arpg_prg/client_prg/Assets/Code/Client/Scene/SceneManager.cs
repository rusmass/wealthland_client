using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Metadata;
using Core.Web;
using Core;
using Client.Cameras;

namespace Client.Scenes
{
    /// <summary>
    /// ≥°æ∞π‹¿Ì∆˜
    /// </summary>
	public class SceneManager : Disposable
	{
		private SceneManager()
		{

		}

		public void Send_RequestEnterScene(int sceneDataId, Action<Scene> onSceneLoaded)
		{
			_onSceneLoaded = onSceneLoaded;
			VirtualServer.Instance.Handle_RequestEnterScene(sceneDataId);
		}

		public void Re_RequestEnterScene(int sceneDataId)
		{
			_CreateScene (sceneDataId);
		}

		private List<IWebNode> _CreateScene(int sceneDataId)
		{
			SceneDataTemplate temp = new SceneDataTemplate ();
			if(temp != null)
			{
				Scene scene = new Scene();
				CurrentScene = scene;
				SmartCamera.Instance.SetPosition (temp.cameraPos);
				SmartCamera.Instance.SetEulerAngles (temp.cameraRotation);
				return scene.Load(temp, _onSceneLoaded);
			}
			else
			{
				Console.Error.WriteLine("ERROR: CreateScene() Can't Find SceneData: " + sceneDataId);
			}

			return new List<IWebNode>();
		}

		public void Tick(float deltaTime)
		{

		}

		protected override void _DoDispose (bool isDisposing)
		{
			_onSceneLoaded = null;
			base._DoDispose (isDisposing);
		}

		private void _SetCameraBackground(Color bgcolor)
		{
			if(Camera.main != null)
			{
				Camera.main.backgroundColor = bgcolor;
			}
		}



		public Scene CurrentScene { get; set; }

		private Action<Scene> _onSceneLoaded;
		public static readonly SceneManager Instance = new SceneManager();

	}
}

