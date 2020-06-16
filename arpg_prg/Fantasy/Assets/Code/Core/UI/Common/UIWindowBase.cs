using UnityEngine;
using System;
using System.Collections.Generic;
using Client.Cameras;
using UnityEngine.UI;
using Core;

namespace Client.UI
{
	public abstract class UIWindowBase : Disposable
	{
		public virtual void Initialise(GameObject go)
		{
			_gameObject = go;
			_layer = (UILayer)_gameObject.layer;
			if (_layer <= UILayer.UIMin || _layer >= UILayer.UIMax) 
			{
				Console.Error.WriteLine ("Error: The UILayer must between {0} and {1}!", UILayer.UIMin + 1, UILayer.UIMax - 1);
				_layer = (UILayer)Mathf.Clamp (_gameObject.layer, (int)(UILayer.UIMin + 1), (int)(UILayer.UIMax - 1));
			}

			_InitCanvas ();
			_Init (_gameObject);
		}

		protected override void _DoDispose (bool isDisposing)
		{
			_sortingOrder--;
			Console.WriteLine ("sortingOrder = {0}", _sortingOrder);
			_Dispose ();
			_gameObject.DestroyEx ();
            base._DoDispose (isDisposing);
		}

		protected abstract void _Init (GameObject go);
		protected abstract void _OnShow ();
		protected abstract void _OnHide ();
		protected abstract void _Dispose ();

		public bool Visible
		{
			get 
			{ 
				return _gameObject.activeSelf; 
			}

			internal set
			{
				if (value) 
				{
					if (_canvas.sortingOrder <= _sortingOrder) 
					{
						_canvas.sortingOrder = ++_sortingOrder;
					}
					Console.WriteLine ("sortingOrder = {0}", _sortingOrder);
					_OnShow ();
				}
				else
				{
					_OnHide ();
				}

				_gameObject.SetActiveEx(value);
			}
		}

		private void _InitCanvas()
		{
			_canvas = _gameObject.GetComponent<Canvas>();
			if (null == _canvas) 
			{
				_canvas = _gameObject.AddComponent<Canvas> ();
			}

			_canvas.renderMode = RenderMode.ScreenSpaceCamera;
			_canvas.worldCamera = UICameraManager.Instance.GetCamera (_layer);

			CanvasScaler scaler = _gameObject.GetComponent<CanvasScaler> ();
			if (null == scaler) 
			{
				scaler = _gameObject.AddComponent<CanvasScaler> ();
			}
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.referenceResolution = Constants.UiResolution;
			scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			scaler.matchWidthOrHeight = 1.0f;
		}

		private UILayer _layer;
		private Canvas _canvas;
		private GameObject _gameObject;
		public UILayer Layer { get{ return _layer; } }
		public string Name { get { return _gameObject.name; } }

		private static int _sortingOrder = 0;
	}
}

