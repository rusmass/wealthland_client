using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client.UI
{
    public class UIMessageTipsWindow : UIWindow<UIMessageTipsWindow,UIMessageTipsController>
    {
        protected override void _Init(GameObject go)
        {
            Text text;

			Image img;

            var childCount = go.transform.childCount;
            for (int i = 1; i <= childCount; ++i)
            {
                text = go.GetComponentEx<Text>("message" + i);
                text.SetActiveEx(false);
                _textQueue.Enqueue(text);

				img = go.GetComponentEx<Image> ("image" + i);
				img.SetActiveEx (false);
				_imgQueue.Enqueue (img);
            }

            _timer = new Counter(_stayTime);
        }

        protected override void _OnShow()
        {

        }

        protected override void _OnHide()
        {

        }

        protected override void _Dispose()
        {

        }

        public void Tick(float deltaTime)
        {
            if (null != _timer)
            {
                _timer.Increase(deltaTime);
            }
        }

        public bool ShowFlyText(string text, TweenCallback callBack = null)
        {
            if (null == _textQueue || _textQueue.Count <= 0 || !_timer.IsExceed())
            {
                return false;
            }

            _timer.Reset();

            var graphic = _textQueue.Dequeue();
            var rt = graphic.rectTransform;
           
            var c = graphic.color;
            c.a = 0;

			var img = rt.parent.gameObject.GetComponent<Image> ();

//			img.color = new Color (0f,0f,0f,0);
			var imgRt = img.transform;
			var imgColor = img.color;
			var lp = imgRt.localPosition;

            graphic.color = c;
			rt.SetActiveEx(true);
            graphic.SetTextEx(text);

			if ("" != text)
			{
				img.SetActiveEx(true);
			}

			var tmpSize = graphic.rectTransform.sizeDelta;
			tmpSize.y = graphic.preferredHeight+20;

			var tmpWidth = graphic.preferredWidth;
			if (tmpWidth >graphic.rectTransform.sizeDelta.x )
			{
				tmpWidth = graphic.rectTransform.sizeDelta.x;
			}
			tmpSize.x =tmpWidth + 10; //graphic.rectTransform.sizeDelta.x+10;
			img.rectTransform.sizeDelta =tmpSize;
			img.color = new Color (img.color.r,img.color.g,img.color.b,1f);
//			Console.WriteLine ("当前文本框的宽度和高度"+graphic.rectTransform.sizeDelta.ToString()+"ssss"+graphic.rectTransform.sizeDelta.y);

            Sequence mySequence = DOTween.Sequence();
			mySequence.Append(imgRt.DOLocalMoveY(imgRt.localPosition.y, _stayTime));
            mySequence.Join(graphic.DOColor(new Color(c.r, c.g, c.b, 1.5f), _stayTime));
			mySequence.Append(imgRt.DOLocalMoveY(imgRt.localPosition.y + _moveLength, _moveDuration));
   //         mySequence.Join(graphic.DOColor(new Color(c.r, c.g, c.b, 0), _moveDuration));
			//mySequence.Join (img.DOColor(new Color(imgColor.r,imgColor.g,imgColor.b,0),_moveDuration));

			_tmpSequence = mySequence.AppendCallback(() => {
				img.SetActiveEx(false);
				img.transform.localPosition = lp;
                _textQueue.Enqueue(graphic);
            });

            if (null != callBack)
            {
                mySequence.AppendCallback(callBack);
            }

            return true;
        }

		public void Dispose()
		{
			if (null != _tmpSequence)
			{
				_tmpSequence.Kill (false);
			}
		}

		private Sequence _tmpSequence;

		private const float _stayTime = 3f ;// 1.5f;
        private const float _moveLength = 150;
        private const float _moveDuration = 1f;

        private Counter _timer;
        private Queue<Text> _textQueue = new Queue<Text>();
		private Queue<Image> _imgQueue = new Queue<Image> ();
    }
}

