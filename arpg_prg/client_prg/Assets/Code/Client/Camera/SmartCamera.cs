using System;
using Core;
using UnityEngine;
using Client.CamerasFSM;
using DG.Tweening;

namespace Client.Cameras
{
    /// <summary>
    ///  摄像机
    /// </summary>
	public class SmartCamera : TransformObject
	{
		private SmartCamera()
			: base()
		{
			_fsm = new FSM(this);
			_fsm.Start ();


			_dampingTimer = new Counter();
			_dampingTimer.Exceed();

			DampingFactor = 16.0f;
			DampingAmplitude = 0.2f;
			DampingFrequency = 8.0f;
			DampingDirection = Vector3.up;

			FollowDistance = 10.0f;
			FollowRelativeTargetPosition = true;
			FollowRelativeTargetRotation = false;
			FollowRelativePosition = Vector3.up;
			FollowRelativeRotation = Quaternion.identity;
			RelativeTargetRotation = Quaternion.identity;
			FollowSmoothTime = 0.1f;

			AudioListenerOffset = 0.75f;
		}

		public void Init()
		{
			int cullingMask = 0; 
			for(int i = (int)UILayer.UIMin + 1; i < (int)UILayer.UIMax; ++i)
			{
				cullingMask |= (1 << i);
			}

			cullingMask |= 1 << 5;
			Camera.main.cullingMask = ~cullingMask;	

			Camera.main.cullingMask = ~cullingMask;
			Instance.SetCamera(Camera.main);
			Instance.Visible = true;

            FOV = 37.1f;

			//2016-11-10 zll add TouchControl
			GetCamera().gameObject.AddComponent<TouchControl>();
		}

		public override void Tick(float deltaTime)
		{
			_fsm.Tick(deltaTime);
			base.Tick(deltaTime);

			if (IsDamping)
			{
				_TickDamping(deltaTime);
			}
		}

		public void SetCamera(UnityEngine.Camera camera)
		{
			if (_camera != null)
			{
				DetachChild(_camera.transform);
			}
			_camera = camera;
			if (_camera != null)
			{
				AttachChild(_camera.transform);
			}
		}

		public Camera GetCamera()
		{
			return _camera;
		}

		public void Damping()
		{
			Damping(16.0f, 0.2f, 8.0f, Vector3.up);
		}

		public void Damping(float factor, float amplitude, float frequency, Vector3 direction)
		{
			DampingFactor = factor;
			DampingAmplitude = amplitude;
			DampingFrequency = frequency;
			DampingDirection = direction;
			_dampingTimer.Reset();
		}

		public bool IsDamping { get { return _dampingTimer.IsNotExceed(); } }

		public bool Stay()
		{
			_followTarget = null;
			return _fsm.Input(new StayEvent());
		}

		public void _TickDamping(float deltaTime)
		{
			if (_camera != null)
			{
				_dampingTimer.Increase(deltaTime);
				float damping = DampingAmplitude * (float)Math.Exp(-DampingFactor * _dampingTimer.Current);
				float d = damping * (float)Math.Cos(2.0f * Math.PI * DampingFrequency * _dampingTimer.Current);
				_camera.transform.localPosition = DampingDirection * d;
				if (damping < float.Epsilon)
				{
					_camera.transform.localPosition = Vector3.zero;
					_dampingTimer.Exceed();
				}
			}
		}

		public bool Visible
		{
			get
			{
				if (_camera != null)
				{
					return _camera.enabled;
				}
				return false;
			}

			set
			{
				if (_camera != null)
				{
					_camera.enabled = value;
				}
			}
		}

		public TransformObject FollowTarget
		{
			get { return _followTarget; }

			set
			{
				_followTarget = value;
				if (_followTarget != null)
				{
					_fsm.Input(new FollowEvent());
				}
			}
		}

		public void FollowHostPlayer()
		{
//			FollowTarget = HostPlayerController.GetHostPlayer ();
		}

		public void SideScrolling(TransformObject target = null)
		{
			_followTarget = target;
			if (_followTarget != null)
			{
				_fsm.Input(new SideScrollingEvent());
			}
		}

		/// <summary>
		/// 设置相机滑动后归位
		/// </summary>
		public void SetCameraPos()
		{
			Tweener	tweener = GetCamera().transform.DOLocalMove(Vector3.zero,0.2f);

			//设置这个Tween不受Time.scale影响
			tweener.SetUpdate(true);
			//设置移动类型
			tweener.SetEase(Ease.Linear);
		}

		public FSMStateType CameraStateType
		{
			get
			{
				return _fsm.CurrentStateType;
			}
		}

		public float FOV
		{
			get
			{
				if ( _camera != null )
					return _camera.fieldOfView;
				return 0;
			}
			set
			{
				if ( _camera != null )
					_camera.fieldOfView = value;
			}
		}
		public float Near
		{
			get
			{
				if (_camera != null)
					return _camera.nearClipPlane;
				return 0;
			}
			set
			{
				if (_camera != null)
					_camera.nearClipPlane = value;
			}
		}
		public float Far
		{
			get
			{
				if (_camera != null)
					return _camera.farClipPlane;
				return 0;
			}
			set
			{
				if (_camera != null)
					_camera.farClipPlane = value;
			}
		}
		public Color BackgroundColor
		{
			get
			{
				if (_camera != null)
					return _camera.backgroundColor;
				return Color.white;
			}
			set
			{
				if (_camera != null)
					_camera.backgroundColor = value;
			}
		}

		public float DampingFactor { get; set; }
		public float DampingAmplitude { get; set; }
		public float DampingFrequency { get; set; }
		public Vector3 DampingDirection { get; set; }

		public float FollowDistance { get; set; }
		public bool FollowRelativeTargetPosition { get; set; }
		public bool FollowRelativeTargetRotation { get; set; }
		public Vector3 FollowRelativePosition { get; set; }
		public Quaternion FollowRelativeRotation { get; set; }
		public Quaternion RelativeTargetRotation { get; set; }
		public float FollowSmoothTime { get; set; }

		public float AudioListenerOffset { get; set; }

		private FSM _fsm;
		private UnityEngine.Camera _camera;
		private Counter _dampingTimer;
		private TransformObject _followTarget;

		public static readonly SmartCamera Instance = new SmartCamera();
	}
}
