using UnityEngine;

namespace Core
{
	public interface ITransformObject
	{
		void Destroy();
		void Tick(float deltaTime);

		void AttachChild(Transform child);
		void AttachChild(TransformObject child);
		void AttachChild(Transform child, Vector3 position);
		void AttachChild(TransformObject child, Vector3 position);
		void AttachChild(Transform child, Vector3 position, Quaternion rotation);
		void AttachChild(TransformObject child, Vector3 position, Quaternion rotation);
		void DetachChild(Transform child);
		void DetachChild(TransformObject child);
		int GetChildCount();
		Vector3 GetEulerAngles(Space relativeTo = Space.World);
		Vector3 GetForward();
		int GetInstanceID();
		Matrix4x4 GetLocalToWorldMatrix();
		Vector3 GetPosition(Space relativeTo = Space.World);
		Vector3 GetRight();
		Quaternion GetRotation(Space relativeTo = Space.World);
		Vector3 GetScale(Space relativeTo = Space.Self);
		Vector3 GetUp();
		Matrix4x4 GetWorldToLocalMatrix();
		void ResetTransformTimer();
		void RotateTo(Quaternion rotation, float time);
		void RotateTo(Vector3 dir, Vector3 up, float time);
		void ScaleTo(float scale, float time);
		void ScaleTo(Vector3 scale, float time);
		void SetEulerAngles(Vector3 eulerAngles, Space relativeTo = Space.World);
		void SetPosition(Vector3 position, Space relativeTo = Space.World);
		void SetRotation(Quaternion rotation, Space relativeTo = Space.World);
		void SetRotation(Vector3 dir, Vector3 up);
		void SetScale(float scale, Space relativeTo = Space.Self);
		void SetScale(Vector3 scale, Space relativeTo = Space.Self);
		void TranslateTo(Vector3 position, float time);
	}

	public class TransformObject : ITransformObject
	{
		public TransformObject(GameObject go=null)
		{
			if (go == null)
			{
                if (os.isEditor)
                {
                    _gameObject = new GameObject(this.GetType().Name);
                }
                else
                {
                    _gameObject = new GameObject();
                }
			}
			else
			{
				_gameObject = go;
			}
			_transform = _UnityGameObject.transform;

			_translateTimer = new Counter(1.0f);
			_rotateTimer = new Counter(1.0f);
			_scaleTimer = new Counter(1.0f);
			ResetTransformTimer();
		}

		public void ResetTranslateTimer()
		{
			_translateTimer.Exceed();
		}

		public void ResetRotateTimer()
		{
			_rotateTimer.Exceed();
		}

		public void ResetScaleTimer()
		{
			_scaleTimer.Exceed();
		}

		public void ResetTransformTimer()
		{
			ResetTranslateTimer();
			ResetRotateTimer();
			ResetScaleTimer();
		}

		// TODO: Position
		public Vector3 GetPosition(Space relativeTo=Space.World)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					return _transform.position;
				}
				else
				{
					return _transform.localPosition;
				}
			}
			return Vector3.zero;
		}

		public void SetPosition(Vector3 position)
		{
			SetPosition(position, Space.World);
		}

		public virtual void SetPosition(Vector3 position, Space relativeTo)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					_transform.position = position;
				}
				else
				{
					_transform.localPosition = position;
				}
				ResetTranslateTimer();
			}
		}

		// TODO: Rotation
		public Quaternion GetRotation(Space relativeTo=Space.World)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					return _transform.rotation;
				}
				else
				{
					return _transform.localRotation;
				}
			}
			return Quaternion.identity;
		}

		public Vector3 GetRight()
		{
			if (!IsDestroyed)
			{
				return _transform.right;
			}
			return Vector3.right;
		}

		public Vector3 GetForward()
		{
			if (!IsDestroyed)
			{
				return _transform.forward;
			}
			return Vector3.forward;
		}

		public Vector3 GetUp()
		{
			if (!IsDestroyed)
			{
				return _transform.up;
			}
			return Vector3.up;
		}

		public void SetRotationH(Vector3 dir)
		{
			dir.y = 0.0f;
			if (dir.sqrMagnitude > 0.000001f)
			{
				dir.Normalize();
				SetRotation(Quaternion.LookRotation(dir, Vector3.up));
			}
		}

		public void SetRotation(Vector3 dir, Vector3 up)
		{
			if (dir.sqrMagnitude > 0.000001f && up.sqrMagnitude > 0.000001f)
			{
				SetRotation(Quaternion.LookRotation(dir, up));
			}
		}

		public void SetRotation(Quaternion rotation,
			Space relativeTo=Space.World)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					_transform.rotation = rotation;
				}
				else
				{
					_transform.localRotation = rotation;
				}
				ResetRotateTimer();
			}
		}

		public Vector3 GetEulerAngles(Space relativeTo=Space.World)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					return _transform.eulerAngles;
				}
				else
				{
					return _transform.localEulerAngles;
				}
			}
			return Vector3.zero;
		}

		public void SetEulerAngles(Vector3 eulerAngles,
			Space relativeTo=Space.World)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					_transform.eulerAngles = eulerAngles;
				}
				else
				{
					_transform.localEulerAngles = eulerAngles;
				}
				ResetRotateTimer();
			}
		}

		// TODO: Scale
		public Vector3 GetScale(Space relativeTo=Space.Self)
		{
			if (!IsDestroyed)
			{
				if (relativeTo == Space.World)
				{
					return _transform.lossyScale;
				}
				else
				{
					return _transform.localScale;
				}
			}
			return Vector3.one;
		}

		public void SetScale(float scale,
			Space relativeTo=Space.Self)
		{
			if (scale > 0.0f)
			{
				SetScale(Vector3.one * scale, relativeTo);
			}
		}

		public void SetScale(Vector3 scale,
			Space relativeTo=Space.Self)
		{
			if (!IsDestroyed)
			{
				if (scale.x > 0.0f && scale.y > 0.0f && scale.z > 0.0f)
				{
					if (relativeTo == Space.World)
					{
						if (_transform.parent == null)
						{
							_transform.localScale = scale;
						}
						else
						{
							Vector3 parentScale = _transform.parent.lossyScale;
							if (parentScale.x > 0.0f &&
								parentScale.y > 0.0f &&
								parentScale.z > 0.0f)
							{
								_transform.localScale = new Vector3(
									scale.x / parentScale.x,
									scale.y / parentScale.y,
									scale.z / parentScale.z);
							}
						}
					}
					else
					{
						_transform.localScale = scale;
					}
					ResetScaleTimer();
				}
			}
		}

		public Matrix4x4 GetLocalToWorldMatrix()
		{
			if (!IsDestroyed)
			{
				return _transform.localToWorldMatrix;
			}
			return Matrix4x4.identity;
		}

		public Matrix4x4 GetWorldToLocalMatrix()
		{
			if (!IsDestroyed)
			{
				return _transform.worldToLocalMatrix;
			}
			return Matrix4x4.identity;
		}

		public Transform GetParent()
		{
			if (!IsDestroyed)
			{
				return _transform.parent;
			}
			return null;
		}

		public void SetParent(Transform parent)
		{
			if (!IsDestroyed)
			{
				_transform.parent = parent;
				SetPosition(Vector3.zero, Space.Self);
				SetRotation(Quaternion.identity, Space.Self);
				SetScale(Vector3.one);
			}
		}

		public int GetChildCount()
		{
			if (!IsDestroyed)
			{
				return _transform.childCount;
			}
			return 0;
		}

		public void AttachChild(TransformObject child)
		{
			if (child != null && !child.IsDestroyed)
			{
				AttachChild(child._transform, Vector3.zero);
			}
		}

		public void AttachChild(TransformObject child,
			Vector3 position)
		{
			if (child != null && !child.IsDestroyed)
			{
				AttachChild(child._transform, position, Quaternion.identity);
			}
		}

		public void AttachChild(TransformObject child,
			Vector3 position,
			Quaternion rotation)
		{
			if (child != null && !child.IsDestroyed)
			{
				AttachChild(child._transform, position, rotation);
			}
		}

		public void DetachChild(TransformObject child)
		{
			if (child != null && !child.IsDestroyed)
			{
				DetachChild(child._transform);
			}
		}

		public void AttachChild(Transform child)
		{
			AttachChild(child, Vector3.zero);
		}

		public void AttachChild(Transform child,
			Vector3 position)
		{
			AttachChild(child, position, Quaternion.identity);
		}

		public void AttachChild(Transform child,
			Vector3 position,
			Quaternion rotation)
		{
			if (!IsDestroyed)
			{
				if (child != null)
				{
					child.parent = _transform;
					child.localPosition = position;
					child.localRotation = rotation;
					child.localScale = Vector3.one;
				}
			}
		}

		// Do not DetachChild before MonoBehaviour.OnDestroy
		public void DetachChild(Transform child)
		{
			if (child != null)
			{
				child.parent = null;
			}
		}

		public virtual void TranslateTo(Vector3 position, float time)
		{
			if (time > 0)
			{
				_srcPosition = GetPosition();
				_dstPosition = position;
				_translateTimer.Redefine(time);
			}
			else
			{
				SetPosition(position);
			}
		}

		public void RotateToH(Vector3 dir, float time)
		{
			dir.y = 0.0f;
			if (dir.sqrMagnitude > 0.000001f)
			{
				dir.Normalize();
				RotateTo(Quaternion.LookRotation(dir, Vector3.up), time);
			}
		}

		public void RotateTo(Vector3 dir, Vector3 up, float time)
		{
			if (dir.sqrMagnitude > 0.000001f && up.sqrMagnitude > 0.000001f)
			{
				RotateTo(Quaternion.LookRotation(dir, up), time);
			}
		}

		public void RotateTo(Quaternion rotation, float time)
		{
			if (time > 0)
			{
				_srcRotation = GetRotation();
				_dstRotation = rotation;
				_rotateTimer.Redefine(time);
			}
			else
			{
				SetRotation(rotation);
			}
		}

		public void ScaleTo(float scale, float time)
		{
			if (scale > 0.0f)
			{
				ScaleTo(Vector3.one * scale, time);
			}
		}

		public void ScaleTo(Vector3 scale, float time)
		{
			if (!IsDestroyed)
			{
				if (scale.x > 0.0f && scale.y > 0.0f && scale.z > 0.0f)
				{
					if (time > 0)
					{
						_srcLocalScale = GetScale();
						_dstLocalScale = scale;
						_scaleTimer.Redefine(time);
					}
					else
					{
						SetScale(scale);
					}
				}
			}
		}

		public virtual void Tick(float deltaTime)
		{
			if (!IsDestroyed)
			{
				if (_translateTimer.IsNotExceed())
				{
					if (_translateTimer.Increase(deltaTime))
					{
						_transform.position = _dstPosition;
					}
					else
					{
						_transform.position = Vector3.Lerp(_srcPosition, _dstPosition,
							_translateTimer.CurrentNormalized);
					}
				}
				if (_rotateTimer.IsNotExceed())
				{
					if (_rotateTimer.Increase(deltaTime))
					{
						_transform.rotation = _dstRotation;
					}
					else
					{
						_transform.rotation = Quaternion.Slerp(_srcRotation, _dstRotation,
							_rotateTimer.CurrentNormalized);
					}
				}
				if (_scaleTimer.IsNotExceed())
				{
					if (_scaleTimer.Increase(deltaTime))
					{
						_transform.localScale = _dstLocalScale;
					}
					else
					{
						_transform.localScale = Vector3.Lerp(_srcLocalScale, _dstLocalScale,
							_scaleTimer.CurrentNormalized);
					}
				}
			}
		}

		public int GetInstanceID()
		{
			if (!IsDestroyed)
			{
				return _gameObject.GetInstanceID();
			}
			return 0;
		}

		public virtual void Destroy()
		{
			if (!IsDestroyed)
			{
				if (_gameObject != null)
				{
					_transform = null;
					GameObject.Destroy(_gameObject);
					_gameObject = null;
				}
			}
		}

		public bool Active
		{
			get
			{
				if (!IsDestroyed)
				{
					return _gameObject.activeSelf;
				}
				return false;
			}

			set
			{
				if (!IsDestroyed)
				{
					_gameObject.SetActive(value);
				}
			}
		}
        
		public GameObject _UnityGameObject
		{
			get { return _gameObject; }
		}

		public bool IsDestroyed { get { return _gameObject == null; } }

		private GameObject _gameObject;

		private Transform _transform;

		private Vector3 _srcPosition;
		private Vector3 _dstPosition;
		private Counter _translateTimer;

		private Quaternion _srcRotation;
		private Quaternion _dstRotation;
		private Counter _rotateTimer;

		private Vector3 _srcLocalScale;
		private Vector3 _dstLocalScale;
		private Counter _scaleTimer;
	}
}
