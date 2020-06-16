using Client.UI;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class TestWrapGrid : MonoBehaviour
{
    void Start()
    {
		var go = transform.DeepFindEx ("sahres");

        _wrapGrid = new UIWrapGrid(go.gameObject, 6);
        _wrapGrid.OnRefreshCell += RefreshCell;
        for (int i = 0; i < _wrapGrid.Cells.Length; ++i)
        {
            var item = _wrapGrid.Cells[i];
            item.DisplayObject = new ItemTest(item.GetTransform());
        }

        _wrapGrid.Refresh();
    }

    public void OnBtnClick()
    {
        if (_wrapGrid.GridSize > 0 && !_flag)
        {
            _wrapGrid.GridSize -= 1;
        }
        else
        {
            _flag = true;
            _wrapGrid.GridSize += 1;
        }

        _wrapGrid.Refresh();
    }

    void Destroy()
    {
        _wrapGrid.OnRefreshCell -= RefreshCell;
    }

    public void RefreshCell(UIWrapGridCell cell)
    {
        
    }

    private UIWrapGrid _wrapGrid;
    private bool _flag;
}

public class ItemTest
{
    public ItemTest(Transform tf)
    {
        this.tf = tf;
    }

    public Transform tf;
}
#endif