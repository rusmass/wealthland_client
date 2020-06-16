using System;
using System.Collections;
using System.Collections.Generic;
using DataManager;

namespace Client.UI
{
    /// <summary>
    /// 测试模块的未引用
    /// </summary>
    public class UIWrapGridExampleController : UIController<UIWrapGridExampleWindow, UIWrapGridExampleController>
    {
        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/uitest.ab";
            }
        }
        protected override void _OnShow()
        {
            
        }

        protected override void _OnHide()
        {
            
        }

        public List<string> GetDataList()
        {
            return ExampleDataManager.Instance.TextValues;
        }

        public string GetValueByIndex(int index)
        {
            var values = ExampleDataManager.Instance.TextValues;
            if (null != values && index < values.Count)
            {
                return values[index];
            }

            return string.Empty;
        }
    }
}
