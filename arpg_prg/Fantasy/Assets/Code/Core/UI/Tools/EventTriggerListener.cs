using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;

	private Button _button;

	public static EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) 
		{
			listener = go.AddComponent<EventTriggerListener>();
		}

		listener._button = go.GetComponent<Button> ();
		return listener;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null && (null == _button || _button.enabled)) 
		{
			onClick(gameObject);
		}
	}

	public override void OnPointerDown (PointerEventData eventData)
	{
		if(onDown != null && (null == _button || _button.enabled)) onDown(gameObject);
	}

	public override void OnPointerEnter (PointerEventData eventData)
	{
		if (onEnter != null && (null == _button || _button.enabled)) 
		{
			onEnter(gameObject);
		}
	}

	public override void OnPointerExit (PointerEventData eventData)
	{
		if (onExit != null && (null == _button || _button.enabled)) 
		{
			onExit(gameObject);
		}
	}

	public override void OnPointerUp (PointerEventData eventData)
	{
		if (onUp != null && (null == _button || _button.enabled)) 
		{
			onUp(gameObject);
		}
	}

	public override void OnSelect (BaseEventData eventData)
	{
		if (onSelect != null && (null == _button || _button.enabled)) 
		{
			onSelect(gameObject);
		}
	}

	public override void OnUpdateSelected (BaseEventData eventData)
	{
		if (onUpdateSelect != null && (null == _button || _button.enabled)) 
		{
			onUpdateSelect(gameObject);
		}
	}
}