using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace Client.UI
{
    public partial class UIOtherCardWindow
    {
        /// <summary>
        /// 初始化提示界面
        /// </summary>
        /// <param name="go"></param>
        private void _InitKnowledge(GameObject go)
        {
            this.lb_knowledgeHead = go.GetComponentEx<Text>(Layout.lb_knowledgeHead);

            this.lb_knowledgeTitle = go.GetComponentEx<Text>(Layout.lb_knowledgeTitle);

            this.lb_knowledgeContent = go.GetComponentEx<Text>(Layout.lb_knowledgeContent);

            this.btn_konwledgeSure = go.GetComponentEx<Button>(Layout.btn_knowledgeSure);

            this._knowledge = go.DeepFindEx(Layout.obj_knowledge);
            
        }

        /// <summary>
        /// 显示窗口的时候
        /// </summary>
        private void _ShowKnowledge()
        {
            _knowledge.SetActiveEx(false);
            btn_konwledgeSure.SetActiveEx(false);
            EventTriggerListener.Get(btn_konwledgeSure.gameObject).onClick += _OnHandlerKnowledge;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _HideKnowledge()
        {
            EventTriggerListener.Get(btn_konwledgeSure.gameObject).onClick -= _OnHandlerKnowledge;
        }

        private void _OnHandlerKnowledge(GameObject go)
        {
            if (_playerManager.IsHostPlayerTurn())
            {
                //TODO HostPlayer Behaviour
                _handleSuccess = true;               
            }
            _HideBgImg();
            TweenTools.MoveAndScaleTo("uiothercard/Content", "uibattle/top/financementor", _CloseHandler);
        }

        /// <summary>
        /// 展示出先知识面板
        /// </summary>
        private void _StartShowKnowledge(bool isonlyshow=false)
        {
            _handleSuccess = true;

            var headStr = "";
            var titleStr = "";
            var contentStr = "";


            if (_controller.cardID == (int)SpecialCardType.HealthType || _controller.cardID == (int)SpecialCardType.InnerHealthType)
            {
                headStr = "健康小达人";
                var healthData = _controller.GetHealthKnowledge();
                titleStr = string.Format("节气知识普及:{0}", healthData.title); 
                contentStr = healthData.content;
            }
            else if (_controller.cardID == (int)SpecialCardType.StudyType || _controller.cardID == (int)SpecialCardType.InnerStudyType)
            {
                headStr = "嘉许您的用功";
                var studyData = _controller.GetStudyKnowledge();
                titleStr = studyData.title;
                //titleStr = string.Format("知识点学习:{0}", studyData.title);
                contentStr = studyData.content;

            }
            else if (_controller.cardID == (int)SpecialCardType.CharityType)
            {
                headStr = "感恩你的付出";
                var charityData = _controller.GetCharityKnowledge();
                titleStr = string.Format("{0}:", charityData.title);
                contentStr = charityData.content;
            }

            lb_knowledgeHead.text =headStr ;// _controller.KnowledgeHeadStr();
            //var tmpKnowledge = _controller.GetHealthKnowledge();
            lb_knowledgeTitle.text =titleStr ;// string.Format("知识点学习:{0}", tmpKnowledge.title);
            lb_knowledgeContent.text =contentStr ;// tmpKnowledge.content;
           

            if(isonlyshow==false)
            {
                var cardColor = _cardTransform.GetComponent<CanvasGroup>();
                _knowledge.SetActiveEx(true);
                btn_konwledgeSure.SetActiveEx(true);
                //_knowledge.localScale = Vector3.zero;
                var knowledgeColor = _knowledge.GetComponent<CanvasGroup>();
                //_knowledge.DOScale(1, 1);
                knowledgeColor.alpha = 0;
                var sequence = DOTween.Sequence();
                sequence.Append(cardColor.DOFade(0, 1));
                sequence.Append(knowledgeColor.DOFade(1, 1));
            }
            else
            {
                //Console.Error.WriteLine("开始展示学习细信息啦");
                _cardTransform.SetActiveEx(false);
                _knowledge.SetActiveEx(true);
                var knowledgeColor = _knowledge.GetComponent<CanvasGroup>();
                //_knowledge.DOScale(1, 1);
                btn_closeShow.transform.parent = _knowledge.transform;
                btn_closeShow.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                btn_closeShow.transform.localPosition = new Vector3(157, 190, 0);
                knowledgeColor.alpha = 0;
                var sequence = DOTween.Sequence();               
                sequence.Append(knowledgeColor.DOFade(1, 1));
            }

           


        }
    

        /// <summary>
        /// 确定按钮
        /// </summary>
        private Button btn_konwledgeSure;
        /// <summary>
        /// 显示类型的文本
        /// </summary>
        private Text lb_knowledgeHead;
        /// <summary>
        /// 显示标题内容的文本
        /// </summary>
        private Text lb_knowledgeTitle;
        /// <summary>
        /// 知识详情文本
        /// </summary>
        private Text lb_knowledgeContent;

        /// <summary>
        /// 滑动动画的容器
        /// </summary>
        private Transform _knowledge;
        

    }
}
