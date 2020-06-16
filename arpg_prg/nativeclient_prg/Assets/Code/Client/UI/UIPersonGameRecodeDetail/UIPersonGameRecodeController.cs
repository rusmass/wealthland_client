using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Client.UI
{
    /// <summary>
    /// 个人信息，游戏信息详情面板
    /// </summary>
    public class UIPersonGameRecodeController : UIController<UIPersonalGameRecodeWindow, UIPersonGameRecodeController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/gamerecorddetail.ab";
            }
        }

        protected override void _OnLoad()
        {
            base._OnLoad();
        }

        protected override void _OnShow()
        {
            base._OnShow();
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }

        protected override void _Dispose()
        {

            base._Dispose();
        }

        /// <summary>
        /// 详情列表
        /// </summary>
        public List<DetailVo> detailList=new List<DetailVo>();

    }
}
