using UnityEngine;
using System.Collections;



namespace Client.UI
{
    /// <summary>
    /// 游戏历史信息列表窗口
    /// </summary>
    public class UIPersonGameRecodeListController : UIController<UIPersonalGameRecodeListWindow, UIPersonGameRecodeListController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/gamerecordlist.ab";
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

    }
}
