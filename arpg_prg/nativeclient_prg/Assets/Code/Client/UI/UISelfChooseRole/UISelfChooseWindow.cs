using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Client.UI
{
    public partial class UISelfChooseWindow:UIWindow<UISelfChooseWindow,UISelfChooseController>
    {
        public UISelfChooseWindow()
        {
            
        }

        protected override void _Init(GameObject go)
        {
            _OnInitCenter(go);
            _OnInitIncome(go);
            _OnInitDebt(go);
        }

        protected override void _OnShow()
        {
            _OnShowCenter();
            _OnShowDebt();
            _OnShowIncome();
        }

        protected override void _OnHide()
        {
            _OnHideCenter();
            _OnHideIncome();
            _OnHideDebt();
        }

        protected override void _Dispose()
        {
          
        }
    }
}
