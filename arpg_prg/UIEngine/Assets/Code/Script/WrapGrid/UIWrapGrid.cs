using Core;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Client.UI
{
	public class UIWrapGrid : Disposable
    {
        public UIWrapGrid(GameObject cellPrefab, int dataCount)
        {
            if (null == cellPrefab)
            {
                Console.Error.WriteLine("[UIWrapGrid Error] cellPrefab = Null!");
            }

            var content = cellPrefab.transform.parent;
            if (null == content)
            {
                Console.Error.WriteLine("[UIWrapGrid Error] cellPrefab.parent = Null!");
            }

            var viewPort = content.transform.parent;
            if (null == viewPort)
            {
                Console.Error.WriteLine("[UIWrapGrid Error] cellPrefab.parent.parent = Null!");
            }

            GridContent = viewPort.GetComponent<UIWrapGridContent>();
            if (GridContent == null)
            {
                Console.Error.WriteLine("[UIWrapGrid Error] No has UIWrapGridContent");
            }

            _CreateItemPools(cellPrefab);
            _gridSize = dataCount;
            GridContent.Init(this);
        }

        public void Refresh()
        {
            if (null != GridContent)
            {
                GridContent.RefreshCell();
            }
        }

        internal void RefreshCell(UIWrapGridCell cell)
        {
            if (null != OnRefreshCell)
            {
                OnRefreshCell(cell);
            }
        }

        internal UIWrapGridCell CreateItem(int dataIndex)
        {
            if (dataIndex > _cells.Length)
            {
                Console.Error.WriteLine("[UIWrapGrid.CreateItem] Error dataIndex > _cells.Length");
            }

            return _cells[dataIndex];
        }

        private void _CreateItemPools(GameObject go)
        {
            _cells = new UIWrapGridCell[GridContent.viewCount];
            _cells[0] = new UIWrapGridCell(this, go);

            for (int i = 1; i < GridContent.viewCount; ++i)
            {
                var cloned = GameObject.Instantiate(go);
                cloned.transform.SetParent(go.transform.parent);
                cloned.transform.localScale = Vector3.one;
				cloned.SetActiveEx (false);
                _cells[i] = new UIWrapGridCell(this, cloned);
            }
        }

		protected override void _DoDispose (bool isDisposing)
		{
			for (int i = 0; i < Cells.Length; ++i) 
			{
				var cell = Cells [i];
				os.dispose (ref cell);
			}

			_cells = null;
		}

        public UIWrapGridCell[] Cells { get { return _cells; } }

        private int _gridSize;
        public int GridSize 
        {
            get { return _gridSize;}
            set 
            {
                 _gridSize = value;
                 GridContent.RefreshAll();
            }
        }
        public event Action<UIWrapGridCell> OnRefreshCell;
        internal UIWrapGridContent GridContent;

        private UIWrapGridCell[] _cells;
    }
}