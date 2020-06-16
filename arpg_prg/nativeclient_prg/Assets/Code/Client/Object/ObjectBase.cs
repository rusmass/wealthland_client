using Client.Scenes;
using UnityEngine;

namespace Client
{
    public class GUIDGenerator
    {
        private static uint _guid = 0;

        public static uint GenGUID()
        {
            return _guid++;
        }
    }

    public class ObjectInitParam
    {
        public virtual ObjectType GetInitType()
        {
            return ObjectType.Invalid;
        }
        public Vector3 init_pos = Vector3.zero;
        public float init_dir = 0.0f;
    }

    public abstract class ObjectBase
    {
        public ObjectBase()
        {

        }

        public virtual bool Init(ObjectInitParam param)
        {
            _guid = GUIDGenerator.GenGUID();
            _disappear = false;

            return true;
        }

        public virtual void OnEnterScene(Scene scene, uint instanceID)
        {
            _scene = scene;
            _instanceID = instanceID;
        }

        public virtual bool Update(float deltaTime)
        {
            if (_disappear)
            {
                return false;
            }

            return true;
        }

        public virtual void SetPosition(Vector3 pos)
        {
            if (_position.x != pos.x || _position.y != pos.y || _position.z != pos.z)
            {
                Vector3 oldPos = new Vector3(_position.x, _position.y, _position.z);

                _position.x = pos.x;
                _position.y = pos.y;
                _position.z = pos.z;

                OnChangePosition(oldPos, _position);
            }
        }

        protected virtual void OnChangePosition(Vector3 oldPos, Vector3 curPos)
        {
            
        }

        public virtual void Destroy()
        {
            _scene = null;
            _disappear = false;
            //_initDir = 0.0f;
            _guid = uint.MaxValue;
            _instanceID = uint.MaxValue;
            //_initPos = Vector3.zero;
        }

        public void Disappear()
        {
            _disappear = true;
        }

        private uint _instanceID = uint.MaxValue;
        public uint InstanceID { get { return _instanceID; } }

        private uint _guid = uint.MaxValue;
        public uint GUID { get { return _guid; } }

        private Vector3 _position = Vector3.zero;
        //private Vector3 _initPos = Vector3.zero;
        //private float _initDir = 0.0f;

        private Scene _scene;
        public Scene Scene { get { return _scene; } }

        private bool _disappear;

        public abstract ObjectType Type { get; }
    }
}
