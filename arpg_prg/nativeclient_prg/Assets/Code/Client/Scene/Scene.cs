#pragma warning disable 0414
using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Web;
using Metadata;
using UnityEngine;
using Client.Cameras;
using DG.Tweening;


namespace Client.Scenes
{
    /// <summary>
    /// 游戏场景类
    /// </summary>
    public class Scene
	{
		public List<IWebNode> Load (SceneDataTemplate template, Action<Scene> onSceneLoaded = null)
		{
			_sceneData = template;
			_onSceneLoaded = onSceneLoaded;

			List<IWebNode> webNodeList = new List<IWebNode>();	
			if(!string.IsNullOrEmpty(_sceneData.resourceName))
			{
				var argument = new WebArgument
				{
					localPath = _sceneData.resourceName
						, flags = WebFlags.UnloadAllLoadedObjects
				};

				var webScene = WebManager.Instance.LoadWebPrefab (argument, null);

				// here we must convert the list to an array, because the list will be returned to outside,
				// and may become a 700+ list.
				var waitingWebNodes = webNodeList.ToArray();
				_loadingSceneRoutine = CoroutineManager.StartCoroutine(_CoOnSceneLoaded(webScene, waitingWebNodes));
				webNodeList.Add(webScene);
			}

			return webNodeList;
		}

		private IEnumerator _CoOnSceneLoaded (WebPrefab webScene, IWebNode[] waitingWebNodes)
		{
			yield return webScene;

			using (webScene) 
			{
				_root = webScene.mainAsset.CloneEx(true) as GameObject;
				_LoadSceneMap();
			}

			yield return null;

			var count = waitingWebNodes.Length;
			for (int i = 0; i< count; ++i)
			{
				var node = waitingWebNodes[i];
				yield return node;
			}

			_OnSceneResLoaded();
			_loadingSceneRoutine = null;
		}

		private void _OnSceneResLoaded()
		{
			if (_onSceneLoaded != null)
			{
				_onSceneLoaded(this);
				_onSceneLoaded = null;
			}

			_LoadSceneBlocks ();
			//2016-09-22 zll fix 播放音乐
			//Audio.AudioManager.Instance.Play (_sceneData.backgroundAudio);
		}
        /// <summary>
        /// unload场景
        /// </summary>
		public void Unload()
		{
			_sceneData = null;
			_onSceneLoaded = null;
			_root.DestroyEx ();
			_root = null;

			_loadingSceneRoutine = null;
		}

		public GameObject Root
		{
			get{ return _root; }
		}

		private void _LoadSceneMap()
		{
			_wayPoint = _root.GetComponentEx<CurvySpline>("Waypoint_outer");
			_wayPointInner = _root.GetComponentEx<CurvySpline>("Waypoint_inner");
		}

		private void _LoadSceneBlocks()
		{
			const string _blockPrefx = "Object0";

			var inner = _root.DeepFindEx ("neiquan").gameObject;
			var outer = _root.DeepFindEx ("waiquan").gameObject;
			for (int i = 0; i < _outerCount; ++i) 
			{
				var name = _blockPrefx + string.Format ("{0:00}", i);

				_outerMats [i] = outer.GetComponentEx<MeshRenderer> (name).material;
				_outerObjs [i] = outer.GetComponentEx<Transform> (name).transform;
			}

			for (int i = 0; i < _innerCount; ++i) 
			{
				var name = _blockPrefx + string.Format ("{0:00}", i);
				_innerMats [i] = inner.GetComponentEx<MeshRenderer> (name).material;
				_innerObjs [i] = inner.GetComponentEx<Transform> (name).transform;
			}
		}

		public void BrightBlocks(BlockType blockType, int index)
		{
			//更换材质球颜色
//			if (null != _lastMat) 
//			{
//				//_SetEmission (_lastMat, _lastColor);
//				setUpMatTexInit(_lastMat,_lastTexture);		
//			}
//			var blocks = blockType == BlockType.Inner ? _innerMats : _outerMats;
//			//2016.10.21 zll fix
////			_SetEmission (blocks [index], _bright);
//			setUpMatTex(blocks [index]);

			var blocks2 = blockType == BlockType.Inner ? _innerObjs : _outerObjs;

			if(null != _lastTra)
			{
				SetUpObjReduction(_lastTra,blockType);
			}
				
			SetUpObjDown(blocks2 [index],blockType);
		}

		private void _SetEmission(Material mat, Color color)
		{
			_lastMat = mat;
			_lastColor = mat.GetColor ("_Emission");
			mat.SetColor ("_Emission", color);
		}

		private void setUpMatTexInit(Material mat,Texture tex)
		{
			mat.mainTexture = tex;
			mat.shader = Shader.Find("Custom/Alpha/NormalAlpha");
			Color color = mat.color;
			mat.color = new Color(color.r,color.g,color.b,1.0f);
		}

		private void setUpMatTex(Material mat)
		{
			_lastMat = mat;
			_lastTexture = mat.GetTexture("_MainTex");

			if(Room.Instance.getCurrentPlay().PlayerID == PlayerManager.Instance.HostPlayerInfo.playerID)
			{
				mImagePath = "share/texture/zhujiaojiaodisekuai.ab";
			}else
			{
				mImagePath = "share/texture/npcjiaodisekuai.ab";
			}


			WebManager.Instance.LoadWebItem (mImagePath, item => {
				using (item)
				{
					mat.mainTexture = item.texture;
					mat.shader = Shader.Find("Custom/Alpha/NormalAlpha");
					Color color = mat.color;
					mat.color = new Color(color.r,color.g,color.b,0.5f);
				}
			});	
		}
			
		/// <summary>
		/// 设置当前角色踩下的格子还原
		/// </summary>
		/// <param name="blockType">Block type.</param>
		public void SetUpGridReduction(BlockType blockType, int index)
		{
			var blocks2 = blockType == BlockType.Inner ? _innerObjs : _outerObjs;
			Transform tra = blocks2 [index];

			Tweener tweener;

			if(blockType == BlockType.Outer)
			{
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.2351589f - 0.1f,tra.localPosition.z),0.2f);
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.2351589f,tra.localPosition.z),1.5f);
			}else
			{
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.1535432f - 0.1f,tra.localPosition.z),0.2f);
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.1535432f,tra.localPosition.z),1.5f);
			}
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);

			_lastTra = null;
		}

		private void SetUpObjDown(Transform tra,BlockType blockType)
		{
			Tweener tweener;

			_lastTra = tra;
			_blockType = blockType;

			if(blockType == BlockType.Outer)
			{
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.2351589f - 0.1f,tra.localPosition.z),1.0f);
			}
			else
			{
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.1535432f - 0.1f,tra.localPosition.z),1.0f);
			}
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
//			tweener.SetEase(Ease.Linear);
		}
			
		public void SetUpObjReduction(Transform tra,BlockType blockType)
		{
			Tweener tweener;

			if(blockType == BlockType.Outer)
			{
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.2351589f,tra.localPosition.z),1f);
			}else
			{
				tweener = tra.DOLocalMove(new Vector3(tra.localPosition.x,0.1535432f,tra.localPosition.z),1f);
			}
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
			//			tweener.SetEase(Ease.Linear);
		}

		public void setParticleSystem(bool mState)
		{
			particleSystem = _root.GetComponentEx<ParticleSystem>("fx_Summoner_s");
			particleSystem.SetActiveEx(mState);
			particleSystem2 = _root.GetComponentEx<ParticleSystem>("fx_Summoner_r");
			particleSystem2.SetActiveEx(mState);
		}

		public void RestartGame()
		{
			if (null != _lastTra)
			{
				SetUpObjReduction (_lastTra, _blockType);
			}
		}


		private const int _outerCount = 25;
		private const int _innerCount = 19;

		private CurvySpline _wayPoint;
		private CurvySpline _wayPointInner;

		private SceneDataTemplate _sceneData;
		private Action<Scene> _onSceneLoaded;
		private GameObject _root;
		private IEnumerator _loadingSceneRoutine;


		private Material _lastMat;
		private Color _lastColor;
		private Texture _lastTexture;

		private static readonly Color _bright = new Color(40f/ 255,40f / 255, 40f/ 255);
		private Material[] _outerMats = new Material[_outerCount];
		private Material[] _innerMats = new Material[_innerCount];

		public Transform _lastTra;
		private BlockType _blockType=BlockType.Outer;
		private Transform[] _outerObjs = new Transform[_outerCount];
		private Transform[] _innerObjs = new Transform[_innerCount];

		private string mImagePath;
		private string _tempImagePath = "share/texture/ditu/{0}.ab";

		private ParticleSystem particleSystem;
		private ParticleSystem particleSystem2;

	}


		
}

public enum BlockType
{
	None,
	Outer,
	Inner,
}
