using System;
using UnityEngine;

namespace Client.UI
{
    public class UIWrapGridCell : IDisposable
    {
        public UIWrapGridCell(UIWrapGrid wrapGrid, GameObject cellPrefab)
        {
            _wrapGrid = wrapGrid;
            _transform = cellPrefab.transform;
        }

        internal void SetGridIndex(int index)
        {
            _index = index;
            if (index == -1)
            {
                _transform.name = "-1";
            }
            else
            {
                _transform.localPosition = _wrapGrid.GridContent.GetLocalPositionByIndex(index);
                _transform.name = (index < 10) ? ("0" + index) : ("" + index);
                _wrapGrid.RefreshCell(this);
            }

            var obj = _transform.gameObject;
            obj.SetActive(index != -1);
        }

        public Transform GetTransform()
        {
            return _transform;
        }

        public void Dispose()
        {
			DisplayObject = null;
			if (null != _transform) 
			{
				GameObject.Destroy (_transform.gameObject);
			}
        }

        public object DisplayObject;
        public int Index { get { return _index; } }

        private int _index = -1;
        private UIWrapGrid _wrapGrid;
        private Transform _transform;
    }
}
