using Client;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Cameras
{
	public class UICameraManager
	{
		private static UICameraManager _instance;
		public static UICameraManager Instance { get { return _instance; } }
		static UICameraManager()
		{
			_instance = new UICameraManager();
		}

		private UICameraManager()
		{

		}

		public void Init()
		{
			if (_uiCameraRoot == null)
			{
				_uiCameraRoot = new GameObject("UICameraRoot");
				PreActiviteUILay(UILayer.UI0);
			}
		}

		public void PreActiviteUILay(UILayer layer)
		{
			if (!_uicameras.ContainsKey(layer))
			{
				Camera camera = _CreateUICamera(layer);
				camera.gameObject.AddComponent<CameraScale> ();
				camera.orthographicSize = 3.2f;
				_uicameras.Add(layer, camera);
				_activitedCount.Add(layer, 0);
				_uicameras[layer].SetActiveEx(true);
			}
		}

		public Camera GetCamera(UILayer layer)
		{
			Camera camera = null;
			if (!_uicameras.TryGetValue (layer, out camera)) 
			{
				Console.Error.WriteLine ("There is no Camera of this layer. layer = {0}", layer);
			}
			return camera;
		}

		private Camera _CreateUICamera(UILayer layer)
		{
			GameObject newCamera = new GameObject("UICamera_"+layer);
			newCamera.transform.parent = _uiCameraRoot.transform;
			Camera camera = newCamera.AddComponent<Camera>();

			int depth = (int)layer - (int)UILayer.UIMin;

			newCamera.layer = (int)layer;
			newCamera.SetActive(false);

			camera.clearFlags = CameraClearFlags.Depth;
			camera.cullingMask = 1<<(int)layer;
			camera.orthographic = true;
			camera.orthographicSize = 3.2f;
			camera.nearClipPlane = 0;
			camera.farClipPlane = 1000;
			camera.rect = new Rect(0, 0, 1, 1);
			camera.depth = depth;
			camera.renderingPath = RenderingPath.VertexLit;
			camera.targetTexture = null;
			camera.useOcclusionCulling = true;
			camera.hdr = false;

			return camera;
		}

		private GameObject _uiCameraRoot;
		private Dictionary<UILayer, int> _activitedCount = new Dictionary<UILayer, int>();
		private readonly Dictionary<UILayer, Camera> _uicameras = new Dictionary<UILayer, Camera>();
	}
}
