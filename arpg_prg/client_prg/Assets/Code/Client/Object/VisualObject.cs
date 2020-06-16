using System;
using Metadata;
using Core.Web;
using UnityEngine;
using Client.Scenes;
using System.Collections;
using System.Collections.Generic;

namespace Client
{
    // 可视化对象
    public class VisualObjectInitParam : ObjectInitParam
    {

    }

    /// <summary>
    /// 可挂接的物体
    /// </summary>
    public class AttachMent
    {
        public Transform parent;
        public string bindername;//绑点名称
    }

    public class VisualObject : ObjectBase
    {
        public override void OnEnterScene(Scene scene, uint instanceid)
        {
            base.OnEnterScene(scene, instanceid);

            LoadModel();
        }

        public void LoadModel()
        {
            var template = MetadataManager.Instance.GetTemplate<ModelTemplate>(modelResID);
            if (null != template && !string.IsNullOrEmpty(template.modelPath))
            {
                WebManager.Instance.LoadWebPrefab("prefabs/character/" + template.modelPath, prefab =>
                {
                    using (prefab)
                    {
						_model = prefab.mainAsset.CloneEx() as GameObject;
                        _model.name = InstanceID.ToString();
                    }
                });
            }
            else
            {
                Console.Error.WriteLine("[VisualObject.LoadModel] please check modeltemplate.xlsx. modelResID = {0}", modelResID);
            }
        }

        // 挂件列表
        protected List<AttachMent> attachments = new List<AttachMent>();
        protected int modelResID = -1;

        //private bool _hasShadow = false;
        private GameObject _shadow;
        private GameObject _model;
        public override ObjectType Type { get { return ObjectType.Invalid; } }
    }
}
