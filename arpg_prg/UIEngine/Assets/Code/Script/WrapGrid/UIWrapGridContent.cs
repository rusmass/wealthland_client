using Client.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class UIWrapGridContent : MonoBehaviour
{
    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }

    [Range(1, 50)]
    public int maxPerLine = 1;

    public float cellWidth = 200f;
    public float cellHeight = 200f;

    [Range(0, 50)]
    public float cellWidthSpace = 0f;

    [Range(0, 50)]
    public float cellHeightSpace = 0f;

    [Range(0, 30)]
    public int viewCount = 4;

    public Arrangement arrangement = Arrangement.Horizontal;

    public ScrollRect scrollRect;

    public RectTransform content;

    void Awake()
    {
        _listItem = new List<UIWrapGridCell>();
        _unUseItem = new Queue<UIWrapGridCell>();
    }

    void OnDestroy()
    {
		_listItem = null;
		_unUseItem = null;
    }

    internal void Init(UIWrapGrid grid)
    {
        if (scrollRect == null || content == null)
        {
            Debug.LogError("Check UIWarpGridContent ScrollRect");
            return;
        }

        _wrapGrid = grid;
        _SetUpdateContentSize();
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(_OnValueChanged);
        _SetUpdateRectItem(0);
    }

    public void RefreshCell()
    {
        for (int i = 0; i < _listItem.Count; ++i)
        {
            var item = _listItem[i];
            _wrapGrid.RefreshCell(item);
        }
    }

	internal void RefreshAll()
	{
		_SetUpdateContentSize();

		for (int i = 0; i <  _listItem.Count; ++i) 
		{
			var item = _listItem [i];
			item.SetGridIndex(-1);
			_unUseItem.Enqueue (item);    
		}

		_listItem.Clear();
		_SetUpdateRectItem(_GetCurScrollPerLineIndex());
	}

    public Vector3 GetLocalPositionByIndex(int index)
    {
        float x = 0f, y = 0f, z = 0f;
        switch (arrangement)
        {
            case Arrangement.Horizontal:
                x = (index / maxPerLine) * (cellWidth + cellWidthSpace);
                y = - (index % maxPerLine) * (cellHeight + cellHeightSpace);
                break;
            case Arrangement.Vertical:
                x = (index % maxPerLine) * (cellWidth + cellWidthSpace);
                y = -(index / maxPerLine) * (cellHeight + cellHeightSpace);
                break;
        }
        return new Vector3(x, y, z);
    }

    private void _OnValueChanged(Vector2 vt2)
    {
        switch (arrangement)
        {
            case Arrangement.Vertical:
                float y = vt2.y;
                if (y >= 1.0f || y <= 0.0f)
                {
                    return;
                }
                break;
            case Arrangement.Horizontal:
                float x = vt2.x;
                if (x <= 0.0f || x >= 1.0f)
                {
                    return;
                }
                break;
        }

        int curScrollPerLineIndex = _GetCurScrollPerLineIndex();
        if (curScrollPerLineIndex == _curScrollPerLineIndex)
        {
            return;
        }
        _SetUpdateRectItem(curScrollPerLineIndex);
    }

    private void _SetUpdateRectItem(int scrollPerLineIndex)
    {
        if (scrollPerLineIndex < 0)
        {
            return;
        }
        _curScrollPerLineIndex = scrollPerLineIndex;
        int startDataIndex = _curScrollPerLineIndex * maxPerLine;
        int endDataIndex = (_curScrollPerLineIndex + viewCount) * maxPerLine;

        for (int i = _listItem.Count - 1; i >= 0; i--)
        {
            UIWrapGridCell item = _listItem[i];
            int index = item.Index;
            if (index < startDataIndex || index >= endDataIndex)
            {
                item.SetGridIndex(-1);
                _listItem.Remove(item);
                _unUseItem.Enqueue(item);
            }
        }

        for (int dataIndex = startDataIndex; dataIndex < endDataIndex; dataIndex++)
        {
            if (dataIndex >= _wrapGrid.GridSize)
            {
                continue;
            }
            if (_IsExistDataByDataIndex(dataIndex))
            {
                continue;
            }
            _CreateItem(dataIndex);
        }
    }

    private void _CreateItem(int dataIndex)
    {
        UIWrapGridCell item;
        if (_unUseItem.Count > 0)
        {
            item = _unUseItem.Dequeue();
        }
        else
        {
            item = _wrapGrid.CreateItem(dataIndex);
        }

        item.SetGridIndex(dataIndex);
        _listItem.Add(item);
    }

    private bool _IsExistDataByDataIndex(int dataIndex)
    {
        if (_listItem == null || _listItem.Count <= 0)
        {
            return false;
        }
        for (int i = 0; i < _listItem.Count; i++)
        {
            if (_listItem[i].Index == dataIndex)
            {
                return true;
            }
        }
        return false;
    }

    private int _GetCurScrollPerLineIndex()
    {
        switch (arrangement)
        {
            case Arrangement.Horizontal: 
                return Mathf.FloorToInt(Mathf.Abs(content.anchoredPosition.x) / (cellWidth + cellWidthSpace));
            case Arrangement.Vertical:
                return Mathf.FloorToInt(Mathf.Abs(content.anchoredPosition.y) / (cellHeight + cellHeightSpace));
        }
        return 0;
    }

    private void _SetUpdateContentSize()
    {
        int lineCount = Mathf.CeilToInt((float)_wrapGrid.GridSize / maxPerLine);
        switch (arrangement)
        {
            case Arrangement.Horizontal:
                content.sizeDelta = new Vector2(cellWidth * lineCount + cellWidthSpace * (lineCount - 1), content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                content.sizeDelta = new Vector2(content.sizeDelta.x, cellHeight * lineCount + cellHeightSpace * (lineCount - 1));
                break;
        }
    }

    private UIWrapGrid _wrapGrid;
    private int _curScrollPerLineIndex = -1;

	private List<UIWrapGridCell> _listItem;
    private Queue<UIWrapGridCell> _unUseItem;
}
