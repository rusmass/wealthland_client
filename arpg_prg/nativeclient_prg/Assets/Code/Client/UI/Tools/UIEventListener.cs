using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventListener : EventTrigger
{
    private UnityEngine.UI.ScrollRect scrollRect;

    public event BaseEventDelegate BaseCancel;

    public event BaseEventDelegate BaseDeselect;

    public event BaseEventDelegate BaseSelect;

    public event BaseEventDelegate BaseSubmit;

    public event BaseEventDelegate BaseUpdateSelected;

    public event PointerEventDelegate PointerBeginDrag;

    public event PointerEventDelegate PointerClick;

    public event PointerEventDelegate PointerDown;

    public event PointerEventDelegate PointerDrag;

    public event PointerEventDelegate PointerDrop;

    public event PointerEventDelegate PointerEndDrag;

    public event PointerEventDelegate PointerEnter;

    public event PointerEventDelegate PointerExit;

    public event PointerEventDelegate PointerInitializePotentialDrag;

    public event PointerEventDelegate PointerScroll;

    public event PointerEventDelegate PointerUp;

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener component = go.GetComponent<UIEventListener>();
        if (component == null)
        {
            component = go.AddComponent<UIEventListener>();
        }
        return component;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (this.ScrollRect != null)
        {
            this.ScrollRect.OnBeginDrag(eventData);
        }
        if (this.PointerBeginDrag != null)
        {
            this.PointerBeginDrag(base.gameObject, eventData);
        }
    }

    public override void OnCancel(BaseEventData eventData)
    {
        if (this.BaseCancel != null)
        {
            this.BaseCancel(base.gameObject, eventData);
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (this.BaseDeselect != null)
        {
            this.BaseDeselect(base.gameObject, eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (this.ScrollRect != null)
        {
            this.ScrollRect.OnDrag(eventData);
        }
        if (this.PointerDrag != null)
        {
            this.PointerDrag(base.gameObject, eventData);
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (this.PointerDrop != null)
        {
            this.PointerDrop(base.gameObject, eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (this.ScrollRect != null)
        {
            this.ScrollRect.OnEndDrag(eventData);
        }
        if (this.PointerEndDrag != null)
        {
            this.PointerEndDrag(base.gameObject, eventData);
        }
    }

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (this.PointerInitializePotentialDrag != null)
        {
            this.PointerInitializePotentialDrag(base.gameObject, eventData);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.PointerClick != null)
        {
            this.PointerClick(base.gameObject, eventData);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.PointerDown != null)
        {
            this.PointerDown(base.gameObject, eventData);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this.PointerEnter != null)
        {
            this.PointerEnter(base.gameObject, eventData);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this.PointerExit != null)
        {
            this.PointerExit(base.gameObject, eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (this.PointerUp != null)
        {
            this.PointerUp(base.gameObject, eventData);
        }
    }

    public override void OnScroll(PointerEventData eventData)
    {
        if (this.PointerScroll != null)
        {
            this.PointerScroll(base.gameObject, eventData);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (this.BaseSelect != null)
        {
            this.BaseSelect(base.gameObject, eventData);
        }
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (this.BaseSubmit != null)
        {
            this.BaseSubmit(base.gameObject, eventData);
        }
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (this.BaseUpdateSelected != null)
        {
            this.BaseUpdateSelected(base.gameObject, eventData);
        }
    }

    public static UIEventListener TryGet(GameObject go)
    {
        if (go != null)
        {
            return go.GetComponent<UIEventListener>();
        }
        return null;
    }

    public UnityEngine.UI.ScrollRect ScrollRect
    {
        get
        {
            return this.scrollRect;
        }
        set
        {
            this.scrollRect = value;
        }
    }

    public delegate void BaseEventDelegate(GameObject obj, BaseEventData eventData);

    public delegate void PointerEventDelegate(GameObject obj, PointerEventData eventData);
}

