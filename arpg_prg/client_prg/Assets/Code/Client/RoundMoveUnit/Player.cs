using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Web;
using Client.UnitFSM;
using Client.Scenes;
using Core;
using Metadata;
using Client.UI;
using DG.Tweening;

namespace Client.Unit
{
	public class Player : BattleUnit
	{
		public Player (string path)
		{
			_modelPath = path;
			CoroutineManager.StartCoroutine (_CoInitPlayer());
		}

		public string PlayerID {
			get;
			set;
		}

        /// <summary>
        /// 加载玩家数据
        /// </summary>
        /// <returns></returns>
		private IEnumerator _CoInitPlayer()
		{
			while (null == SceneManager.Instance.CurrentScene || null == SceneManager.Instance.CurrentScene.Root) 
			{
				yield return null;
			}


			var web = WebManager.Instance.LoadWebPrefab (_modelPath, prefab => {
				using(prefab)
				{
					_root = prefab.mainAsset.CloneEx() as GameObject;
					_root.name = PlayerID.ToString();

					//2016-11-20 zll fix
					_animator = _root.GetComponentInChildren<Animator>();
//					_animator.enabled = false;
					_animator.Play("gameIdle");
					_root.AddComponent<FaceToCamera>();
					isInited=true;
                    _InitPlayer();	

					if(_root.name == PlayerManager.Instance.HostPlayerInfo.playerID)
					{
						IsPlayer = true;
					}
//					_playerMat = _root.GetComponentEx<SkinnedMeshRenderer>("clothes").material;
//					var tmpcolor =  _playerMat.GetColor ("_Emission");
//					_originColor = new Color(tmpcolor.r,tmpcolor.g,tmpcolor.g);
				}
			});

			yield return web;
		}

        /// <summary>
        /// 初始化玩家信息
        /// </summary>
		private void _InitPlayer()
		{
			_currentScene = SceneManager.Instance.CurrentScene;
			var go = _currentScene.Root;

            _wayPoint = go.GetComponentEx<CurvySpline>("Waypoint_outer");
            _wayPointInner = go.GetComponentEx<CurvySpline>("Waypoint_inner");

            _dir = (_speed >= 0) ? 1 : -1;
			_speed = Mathf.Abs(_speed);

			//2016.10.21 zll add
			setPlayerParticleHide();
			setPlayerShaderAlpah();

			if (_isEnterInner == false)
			{
                if(null!=_root)
                {
                    _root.transform.localScale = Vector3.one;

                }
                _blockType = BlockType.Outer;
				_initPoint = outerInitPoint;
				_InitPosAndRot();
			}
			else
			{
				UpGradeToInner ();
			}

		}

		public void CalRoundWalkInfos(int distance)
		{
            //if (null != _wayPoint)
            {
                try
                {
//					Console.Error.WriteLine("当前的玩家id："+PlayerID + "当前的点数：：：："+_currentPos+"+++移动点数是："+distance.ToString());

					var curSeg = _wayPoint.DistanceToSegment(_currentPos);
                 
					CurrentPoint = curSeg.ControlPointIndex;
                    _lastPoint = CurrentPoint;
                    _roInseContion = CurrentPoint;

                    var count = _wayPoint.Count;
                    var moves = distance + CurrentPoint;

//					Console.Error.WriteLine("当前玩家的id:"+PlayerID+ "--------当前的点数:"+CurrentPoint.ToString()+"-------移动的位置："+distance.ToString());

                    _totalRound = (int)distance / count;
                    _targetPoint = moves % count;
                    _currentRound = 0;

					_animator.Play("gameJump");
					_animator.speed = 1.5f;
//					_animator.enabled = true;

//					_SetEmission(_bright);
					_lastMat = _playerMat;
					_lastColor = _originColor;
                }
                catch(Exception e)
                {
                    Console.WriteLine("~~~" + e);
                }
                
            }
        }

		public bool RoundWalk(float deltaTime)
		{
			if (null != _wayPoint) 
			{
				float tf = 0.0f;
				if (!Mathf.Approximately (_currentPos, _wayPoint.Length)) 
				{
					tf = _wayPoint.DistanceToTF (_currentPos);
				} 

				mTransform.position = _wayPoint.MoveByFast (ref tf, ref _dir, _speed * deltaTime, CurvyClamping.Loop);

				_currentPos = _wayPoint.TFToDistance (tf);

//				var tmpInitPostion = Convert.ToInt32(_currentPos);
//				if (tmpInitPostion >= _wayPoint.Count)
//				{
//					tmpInitPostion = tmpInitPostion % (_wayPoint.Count);
//				}
//				var segment = _wayPoint.Segments[tmpInitPostion];
//				float tf =_wayPoint.SegmentToTF (segment);

                var rotation = _wayPoint.GetOrientationFast(tf);
                var mEulerAngles = mTransform.eulerAngles;
                mTransform.eulerAngles = new Vector3(mEulerAngles.x, rotation.eulerAngles.y, mEulerAngles.z);

                CurrentPoint = _wayPoint.TFToSegment(tf).ControlPointIndex;

                if (CurrentPoint == _roInseContion && CurrentPoint != _lastPoint)
                {
                    _currentRound++;
                }

                if (_currentRound == _totalRound && CurrentPoint == _targetPoint)
                {
	
					_currentScene.BrightBlocks (_blockType, CurrentPoint);
//					_animator.enabled = false;
					_animator.Play("gameIdle");
                    return true;
                }

				if (_lastPoint != CurrentPoint) {
					
					var currentPlayer = PlayerManager.Instance.Players[(Room.Instance.CurrentPlayerIndex)];
//					Console.WriteLine ("zouzouzzouzo"+currentPlayer.ToString()+"zzzzzzzzzz"+Room.Instance.CurrentPlayerIndex.ToString());
					if (null != currentPlayer)
					{
						var isCheck = false;
						if (currentPlayer.isEnterInner == false)
						{
							var template = MetadataManager.Instance.GetTemplate<StageOuterPoint>(CurrentPoint);
							if (template.IsCheckDay ())
							{
								isCheck = true;
							}
						}
						else
						{
							var template = MetadataManager.Instance.GetTemplate<StageInnerPoint>(CurrentPoint);
							if (template.IsCheckDay())
							{
								isCheck = true;
							}
						}


						if (isCheck==true)
						{
							GameEventManager.Instance.FireEvent(GameEvents.CheckDay);
						}

					}

					_currentScene.BrightBlocks (_blockType, CurrentPoint);
                }

                _lastPoint = CurrentPoint;
			}
			
			return false;
		}

        /// <summary>
        /// 初始化位置
        /// </summary>
		private void _InitPosAndRot()
		{
			if (null == _wayPoint) 
			{
				return;
			}
			_currentPos = _initPoint;
//			Console.Error.WriteLine ("当前玩家的点数位置"+_currentPos.ToString()+"当前总的点数是：+++++"+_wayPoint.Count+"-------当前的id号："+_modelPath);

			var tmpInitPostion = Convert.ToInt32(_currentPos);
			if (tmpInitPostion >= _wayPoint.Count)
			{
				tmpInitPostion = tmpInitPostion % (_wayPoint.Count);
			}

			var segment = _wayPoint.Segments[tmpInitPostion];
			float tf =_wayPoint.SegmentToTF (segment);

			if (segment.IsFirstSegment == true )
			{
				_currentPos = segment.Distance;
			}
			else
			{
				_currentPos = segment.Distance+segment.Length/2;
			}

//			_currentPos = _wayPoint.TFToDistance (tf);
//			Console.Error.WriteLine ("转换后晚间的位置："+_wayPoint.DistanceToSegment(_currentPos)+"----当前转换后的砖块数"+_wayPoint.DistanceToSegment(_currentPos).ControlPointIndex.ToString()+"----当前的_currentPos:"+_currentPos.ToString());
			//float tf = _wayPoint.DistanceToTF (_currentPos);

			mTransform.position = _wayPoint.Interpolate (tf);
			var rotation = _wayPoint.GetOrientationFast (tf);
			var mEulerAngles = mTransform.eulerAngles;
			mTransform.eulerAngles = new Vector3(mEulerAngles.x, rotation.eulerAngles.y, mEulerAngles.z);

//			for (var i = 0; i < _wayPoint.Count; i++)
//			{
//				segment = _wayPoint.Segments[i];
//				tf =_wayPoint.SegmentToTF (segment);
//				if (segment.IsFirstSegment == true )
//				{
//					_currentPos = segment.Distance;
//				}
//				else
//				{
//					_currentPos = segment.Distance+segment.Length/2;
//				}
//
//				Console.Error.WriteLine ("当前的砖块数"+i.ToString()+"-----当前的砖块："+segment.Length.ToString()+"----------转换后晚间的位置："+_wayPoint.DistanceToSegment(_currentPos)+"----当前转换后的砖块数"+_wayPoint.DistanceToSegment(_currentPos).ControlPointIndex.ToString()+"----当前的_currentPos:"+_currentPos.ToString()+"----segmment的距离"+segment.Distance.ToString());
//			}

		}

        /// <summary>
        /// 升级进入内圈
        /// </summary>
        public void UpGradeToInner()
        {
			_initPoint = innerInitPoint;
			_currentPos = _initPoint;
            _wayPoint = _wayPointInner;
			_blockType = BlockType.Inner;
            _root.transform.localScale = Vector3.one * 1.4f;
            _InitPosAndRot();
        }

        /// <summary>
        /// 网络版游戏中初始化玩家的信息
        /// </summary>
        /// <param name="position"></param>
        /// <param name="isEnterInner"></param>
		public void NetSetPlayerInitData(int position,bool isEnterInner)
		{			
			_isEnterInner = isEnterInner;

			if (_isEnterInner == false)
			{
				_blockType = BlockType.Outer;
				outerInitPoint = position;
				_initPoint = outerInitPoint;
				if (isInited == true)
				{
					_InitPosAndRot();
				}
			}
			else
			{
				innerInitPoint = position;
				if (isInited == true)
				{
					UpGradeToInner ();
				}
			}
		}

		private int outerInitPoint = 0;
		private int innerInitPoint = 0;

		public override void Tick (float deltaTime)
		{
			
		}

		private void _SetEmission(Color color)
		{
			if (null != _lastMat) 
			{
				_lastMat.SetColor ("_Emission", _lastColor);
			}
			_playerMat.SetColor ("_Emission", color);
		}

		/// <summary>
		/// 设置当前角色踩下的格子还原
		/// </summary>
		/// <param name="blockType">Block type.</param>
		public void SetUpGridReduction(BlockType blockType)
		{
			_currentScene.SetUpGridReduction (_blockType, CurrentPoint);
		}

		/// <summary>
		/// 返回当前角色的 transform
		/// </summary>
		/// <returns>The play position.</returns>
		public Transform getPlayPos()
		{
			return mTransform;
		}
	
		/// <summary>
		/// 显示脚下的效果
		/// </summary>
		public void setPlayerParticleShow()
		{
			ParticleSystem ps = _root.GetComponentEx<ParticleSystem>("fx_Summoner_b");
			ps.SetActiveEx(true);
		}

		/// <summary>
		/// 隐藏脚下的效果
		/// </summary>
		public void setPlayerParticleHide()
		{
			ParticleSystem ps2 = _root.GetComponentEx<ParticleSystem>("fx_Summoner_b");
			ps2.SetActiveEx(false);
		}

		/// <summary>
		/// 设置人物半透明状态
		/// </summary>
		public void setPlayerShaderAlpah()
		{
			SkinnedMeshRenderer sr =  _root.GetComponentEx<SkinnedMeshRenderer>("clothes");

			for(int i = 0;i<sr.materials.Length;i++)
			{
				_playerMat = sr.materials[i];
				_playerMat.shader =  Shader.Find("Custom/Alpha/NormalAlpha");
				Color color = _playerMat.color;
				_playerMat.color = new Color(color.r,color.g,color.b,0.5f);
			}

		}

		/// <summary>
		/// 设置人物不透明状态
		/// </summary>
		public void setPlayerShaderInitial()
		{
			SkinnedMeshRenderer sr =  _root.GetComponentEx<SkinnedMeshRenderer>("clothes");

			for(int i = 0;i<sr.materials.Length;i++)
			{
				_playerMat = sr.materials[i];
				_playerMat.shader =  Shader.Find("Legacy Shaders/VertexLit");
			}
				
		}
	
		/// <summary>
		/// 展示流光图
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void ShowLightImage()
		{
			var battleControlelr = UIControllerManager.Instance.GetController<UIBattleController> ();
			if (battleControlelr.getVisible ())
			{
				battleControlelr.ShowLightImage (int.Parse(_root.name));
			}
		}

		/// <summary>
		/// 显示人物从外圈进入内圈的效果
		/// </summary>
		public void enterInnerShow()
		{
//			ParticleSystem ps = _root.GetComponentEx<ParticleSystem>("fx_Summoner_body_b");
			GameObject ps = _root.DeepFindEx("texiao").gameObject;
			ps.SetActiveEx(true);

            MessageHint.Show(string.Format("玩家{0}传送进入内圈", PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex].playerName));

            Tweener	tweener = _root.transform.DOScale(Vector3.zero,2f);
	
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
			tweener.SetEase(Ease.Linear);
			tweener.OnComplete(enterInnerHide);
		}

		/// <summary>
		/// 隐藏人物从外圈进入内圈的效果
		/// </summary>
		private void enterInnerHide()
		{
//			ParticleSystem ps = _root.GetComponentEx<ParticleSystem>("fx_Summoner_body_b");
			GameObject ps = _root.DeepFindEx("texiao").gameObject;
			ps.SetActiveEx(false);

			Room.Instance.Re_UpPlayerState();
			setPlayerParticleHide();
			setPlayerScale();
		}
			
		/// <summary>
		/// 还原玩家大小
		/// </summary>
		private void setPlayerScale()
		{
			SceneManager.Instance.CurrentScene.setParticleSystem(true);
            _root.transform.localScale = Vector3.zero;
            Tweener	tweener = _root.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f),2f);
			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
			tweener.SetEase(Ease.Linear);

			tweener.OnComplete(InnerEvent);
		}

		/// <summary>
		/// 进入内圈的事件
		/// </summary>
		void InnerEvent()
		{
			SceneManager.Instance.CurrentScene.setParticleSystem(false);
            var player = PlayerManager.Instance.Players[Client.Unit.BattleController.Instance.CurrentPlayerIndex];
            var controller = Client.UIControllerManager.Instance.GetController<UIEnterInnerWindowController>();
            controller.playerInfor = player;
            controller.setVisible(true);          
            

			//var couclusionController = Client.UIControllerManager.Instance.GetController<UIConclusionController>();
			//couclusionController.setTiletText(false);
			//couclusionController.setUpBaseText(player);
			//couclusionController.setVisible (true);
		}
			
		public void Dispose()
		{
			if (null!=_root)
			{
				_root.DestroyEx();
			}

			if (null != _animator)
			{
				_animator.DestroyEx ();
			}
		}

		public void ReStartGame()
		{
			_blockType = BlockType.Outer;
			_animator.Play("gameIdle");
			_InitPlayer ();
		}


		private int _initPoint = 0;
		private bool isInited = false;
		private bool _isEnterInner=false;


		private GameObject _root;

		private int _dir;
		private float _speed = 4;//可配置
		private float _currentPos; //可配置
		public int CurrentPoint;
		private int _lastPoint;
		private int _targetPoint;
		private int _roInseContion;
		private int _totalRound;
		private int _currentRound;
		private CurvySpline _wayPoint;
        private CurvySpline _wayPointInner;
		private Transform mTransform
		{
			get
			{
				return _root.transform;
			} 
		}

		private Animator _animator;

		private string _modelPath;

		public BlockType _blockType = BlockType.Outer;
		private Scene _currentScene;

		private Material _playerMat;
		private Color _originColor;
		private static Color _lastColor;
		private static Material _lastMat;
		private static readonly Color _bright = new Color(0.25f, 0.25f, 0.25f);
	
		private Material _headMat;

		private bool IsPlayer = false;
	}
}