using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ScrollPage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
	ScrollRect rect;
	List<float> pages = new List<float>();
	int currentPageIndex = -1;
	public float smoothing = 4;

	float targethorizontal = 0;
	bool isDrag = false;

	public System.Action<int, int> OnPageChanged;

	float startime = 0f;
	float delay = 0.1f;

	void Start()
	{
		rect = transform.GetComponent<ScrollRect>();
		startime = Time.time;

		//UpdatePages();
	}

	void Update()
	{
		if (Time.time < startime + delay) return;
		UpdatePages();
		if (!isDrag && pages.Count > 0)
		{
			rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, Time.deltaTime * smoothing);

		}
	}

	/// <summary>
	/// The index is begin 0,1,2...
	/// </summary>
	/// <param name="pageIndex"></param>
	/// <param name="isSmoothing"></param>
	public void ChangeToPage(int pageIndex, bool isSmoothing = false)
	{
		if (pageIndex >= 0 && pageIndex < pages.Count)
		{
			targethorizontal = pages[pageIndex];
		}

		if (!isSmoothing)
		{
			rect.horizontalNormalizedPosition = targethorizontal;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDrag = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDrag = false;
		float posX = rect.horizontalNormalizedPosition;
		int index = 0;

		if (pages.Count <= 0)
		{
			return;
		}

		float offset = Mathf.Abs(pages[index] - posX);
		for (int i = 1; i < pages.Count; i++)
		{
			float temp = Mathf.Abs(pages[i] - posX);
			if (temp < offset)
			{
				index = i;
				offset = temp;
			}
		}

		if (index != currentPageIndex)
		{
			currentPageIndex = index;
			if (null != OnPageChanged)
			{
				OnPageChanged(pages.Count, currentPageIndex);
			}
		}

		targethorizontal = pages[index];
	}

	void UpdatePages()
	{
		int count = this.rect.content.childCount;
		int temp = 0;
		for (int i = 0; i < count; i++)
		{
			if (this.rect.content.GetChild(i).gameObject.activeSelf)
			{
				temp++;
			}
		}
		count = temp;

		if (pages.Count != count)
		{
			if (count != 0)
			{
				pages.Clear();
				for (int i = 0; i < count; i++)
				{
					if (count != 1)
					{
						pages.Add(i / ((float)(count - 1)));
					}
				}
			}
			OnEndDrag(null);
		}
	}
}
