using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class UIWrapGridExampleWindow : UIWindow<UIWrapGridExampleWindow, UIWrapGridExampleController>
    {
        protected override void _Init(GameObject go)
        {
            var cellPrefab = go.DeepFindEx("item");

            _btnTest = go.GetComponentEx<Button>("Button");

            _CreateWrapGrid(cellPrefab.gameObject);

            EventTriggerListener.Get(_btnTest.gameObject).onClick += _OnBtnTestClick;
        }

        private void _CreateWrapGrid(GameObject go)
        {
            var items = _controller.GetDataList();
            _wrapGrid = new UIWrapGrid(go, items.Count);

            for (int i = 0; i < _wrapGrid.Cells.Length; ++i)
            {
                var cell = _wrapGrid.Cells[i];
                cell.DisplayObject = new ItemDisplay(cell.GetTransform().gameObject);
            }

            _wrapGrid.OnRefreshCell += _OnRefreshCell;
            _wrapGrid.Refresh();
        }

		private void _OnRefreshCell(UIWrapGridCell cell)
		{
			var index = cell.Index;
			var value = _controller.GetValueByIndex(index);
			var display = cell.DisplayObject as ItemDisplay;
			display.Refresh(value);
		}

        protected override void _OnShow()
        {
            
        }

        protected override void _OnHide()
        {
            
        }

        protected override void _Dispose()
        {
            if (null != _wrapGrid)
            {
                _wrapGrid.Dispose();
                _wrapGrid.OnRefreshCell -= _OnRefreshCell;
            }

            if (null != _btnTest)
            {
                EventTriggerListener.Get(_btnTest.gameObject).onClick -= _OnBtnTestClick;
            }
        }

        

        private void _OnBtnTestClick(GameObject go)
        {
            _wrapGrid.GridSize -= 1;
        }

        private Button _btnTest;
        private UIWrapGrid _wrapGrid;
    }
}
