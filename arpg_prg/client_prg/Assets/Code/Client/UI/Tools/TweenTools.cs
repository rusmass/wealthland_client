using System;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class TweenTools
    {
        public static void MoveAndScaleTo(string startObj, string endObj, TweenCallback callBack)
        {
            if (!string.IsNullOrEmpty(startObj) && !string.IsNullOrEmpty(endObj))
            {
                var start = GameObject.Find(startObj);
                var end = GameObject.Find(endObj);

                if (null != start && null != end)
                {
                    Sequence mySequence = DOTween.Sequence();
                    Tweener move = start.transform.DOMove(end.transform.position, _moveDuration);
                    Tweener scale = start.transform.DOScale(_scaleEnd, _moveDuration);

                    mySequence.Append(move);
                    mySequence.Join(scale);
                    mySequence.AppendCallback(callBack);
                }
            }
        }

        public static float MoveDuration
        {
            get
            {
                return _moveDuration;
            }

            set
            {
                _moveDuration = value;
            }
        }

        public static float ScaleEnd
        {
            get
            {
                return _scaleEnd;
            }

            set
            {
                _scaleEnd = value;
            }
        }

        private static float _moveDuration = 0.5f;
        private static float _scaleEnd = 0f;
    }
}
