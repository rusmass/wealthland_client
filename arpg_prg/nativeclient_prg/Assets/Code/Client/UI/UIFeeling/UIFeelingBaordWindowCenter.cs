using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;


namespace Client.UI
{
	public partial class UIFeelingBaordWindow
    {
        #region Const
        private const float TopHigh = 200;
        #endregion
        #region Componment    
        /// <summary>
        /// 展示感悟的组件
        /// </summary>
        private Button btn_item;
        /// <summary>
        /// 背景图
        /// </summary>
		private Image img_bg;
        /// <summary>
        /// 退出按钮
        /// </summary>
        private Button _btnExit;

        /// <summary>
        /// 玩家感恩列表
        /// </summary>
        private Button btn_feelshare;
        /// <summary>
        /// 玩家自己的列表
        /// </summary>
        private Button btn_feelself;
        /// <summary>
        /// 发表感悟
        /// </summary>
        private Button btn_feelsend;

        /// <summary>
        /// 显示感悟的view
        /// </summary>
        private ScrollRect _scrollRect;

        #endregion
        #region Normal       
        private FeelingItem[] _btnList = null;
        private List<FeelingItem> questionInfoList = null;
        private EventTriggerListener.VoidDelegate QuestionAction = null;

        private List<FeelingVo> _dataList = new List<FeelingVo>();

        #endregion

        private void _OnInitCenter(GameObject go)
        {

            GameModel.GetInstance.MathWidthOrHeightByCondition(go,0);

            questionInfoList = new List<FeelingItem>();
          
            _btnExit = go.GetComponentEx<Button>(Layout.btn_Exit);
            EventTriggerListener.Get(_btnExit.gameObject).onClick = (obj)=> { Audio.AudioManager.Instance.BtnMusic(); _ClosePanel(); };
                       
			img_bg = go.GetComponentEx<Image> (Layout.img_bgimg);

            this.btn_feelshare = go.GetComponentEx<Button>(Layout.btn_feelshare);
            this.btn_feelself = go.GetComponentEx<Button>(Layout.btn_feelself);
            this.btn_feelsend = go.GetComponentEx<Button>(Layout.btn_feelsend);

            this.btn_item = go.GetComponentEx<Button>(Layout.btn_item);
            this._scrollRect = go.GetComponentEx<ScrollRect>(Layout.contentView);            
        }

        private void _OnShowCenter()
        {
            if (_controller.isShowBlackBg == true)
            {
                img_bg.color = new Color(0, 0, 0, 1);
            }

           

            EventTriggerListener.Get(btn_feelshare.gameObject).onClick += _FeelingShareHandler;
            EventTriggerListener.Get(btn_feelself.gameObject).onClick += _FeelSelfHandler;
            EventTriggerListener.Get(btn_feelsend.gameObject).onClick += _FeelSendHandler;

            _btnList = new FeelingItem[10];
            for(var i=0;i<10;i++)
            {
                if(i==0)
                {
                    _btnList[0] =new FeelingItem();
                    _btnList[0].Init(this.btn_item.gameObject, _OnClickQuestionCallBack);
                    QuestionAction += _btnList[0].MovePanel;
                }
                else
                {
                    var transform = this.btn_item.transform.parent;
                    var tmpObj = GameObject.Instantiate<Button>(this.btn_item);
                    tmpObj.transform.SetParent(transform);
                    tmpObj.transform.position = Vector3.one;
                    tmpObj.transform.localScale = Vector3.one;

                    var tmpItem = new FeelingItem();
                    tmpItem.Init(tmpObj.gameObject, _OnClickQuestionCallBack);
                    QuestionAction += tmpItem.MovePanel;
                    _btnList[i] = tmpItem;
                }
                //_btnList[i].name = "item" + i;
                //EventTriggerListener.Get(_btnList[i].gameObject).onClick += _ClickItemHandler;
                //_btnList[i].onClick.AddListener(_ClickItemHandler);
            }

            this._scrollRect.SetActiveEx(false);
            _FeelingShareHandler(null);
        }

        /// <summary>
        /// 刷新感恩列表的信息
        /// </summary>
        public void UpdateGameFeeling(int _currentIndex,int totalPage)
        {
            this._pageIndex = _currentIndex;
            var startIndex = (_currentIndex - 1)*_pageLength;
            this._totalPage = totalPage;

            _UpdatePage();

            if(this._scrollRect.IsActive()==false)
            {
                this._scrollRect.SetActiveEx(true);
            }
            
            for(var i=0;i<10;i++)
            {
                var tmpItem = _btnList[i];
                //var tmpData = _controller.GameFeeling[startIndex+i];
                var tmpData = (startIndex + i) < _controller.GameFeeling.Count ? _controller.GameFeeling[startIndex + i] : null;
               // Console.Error.WriteLine("sssss:"+(startIndex+i)+"ssssss___"+i);
                if (null!=tmpData)
                {
                    tmpItem.SetEnable(true);
                    tmpItem.SetData(tmpData);
                }
                else
                {
                    tmpItem.SetEnable(false);
                }
            }

        }

        /// <summary>
        /// 刷新自己的感恩列表
        /// </summary>
        public void UpdateSelfFeeling(int _currentIndex, int totalPage)
        {
            this._pageIndex = _currentIndex;
            var startIndex = (_currentIndex - 1) * _pageLength;
            this._totalPage = totalPage;

            if (this._scrollRect.IsActive() == false)
            {
                this._scrollRect.SetActiveEx(true);
            }

            _UpdatePage();

            for (var i = 0; i < 10; i++)
            {
                var tmpItem = _btnList[i];
                var tmpData = (startIndex + i) < _controller.SelfFeelList.Count? _controller.SelfFeelList[startIndex + i] : null;

                if (null != tmpData)
                {
                    tmpItem.SetEnable(true);
                    tmpItem.SetData(tmpData);
                }
                else
                {
                    tmpItem.SetEnable(false);
                }
            }

        }

        private void _OnClickQuestionCallBack(GameObject go)
        {
            Audio.AudioManager.Instance.BtnMusic();
            QuestionAction(go);
        }

        private void _OnHideCenter()
        {
            EventTriggerListener.Get(btn_feelshare.gameObject).onClick -= _FeelingShareHandler;
            EventTriggerListener.Get(btn_feelself.gameObject).onClick -= _FeelSelfHandler;
            EventTriggerListener.Get(btn_feelsend.gameObject).onClick -= _FeelSendHandler;

            for (var i = 0; i < 10; i++)
            {
                var tmpItem = _btnList[i];
                tmpItem.Dispose();
            }
        }        

        /// <summary>
        /// 处理游戏感悟列表
        /// </summary>
        /// <param name="go"></param>
        private void _FeelingShareHandler(GameObject go)
        {
            this._pageIndex = 1;
            _feelTypes = 0;
            _ShowGameFeelByIndex(_pageIndex);
        }

        private void _ShowGameFeelByIndex(int index)
        {
            if (_controller.IsRequestGameFeel(index))
            {
                var tmpData = new JsonData();
                tmpData["page"] = index;
                HttpRequestManager.GetInstance().GetFeelingShareData(tmpData.ToJson(), _controller.UpdateGameFeels);

            }
            else
            {
                UpdateGameFeeling(index, _controller.GameFeelPages);
            }
        }



        /// <summary>
        /// 处理自己的感恩列表
        /// </summary>
        /// <param name="go"></param>
        private void _FeelSelfHandler(GameObject go)
        {
            this._pageIndex = 1;
            _feelTypes = 1;
            _ShowSelfShareByIndex(_pageIndex);
        }

        private void _ShowSelfShareByIndex(int index)
        {
            if (_controller.IsRequestSelfFeel(index))
            {
                var tmpData = new JsonData();
                tmpData["page"] = index;
                tmpData["playerId"] = GameModel.GetInstance.myHandInfor.uuid;
                HttpRequestManager.GetInstance().GetFeelSelfData(tmpData.ToJson(), _controller.UpdateSelfFeels);
            }
            else
            {
                UpdateSelfFeeling(index, _controller.SelfFeelPages);
            }
        }


        /// <summary>
        /// 添加感悟
        /// </summary>
        /// <param name="go"></param>
        private void _FeelSendHandler(GameObject go)
        {
            var controll = UIControllerManager.Instance.GetController<UIFellingWindowControll>();
            controll.setVisible(true);
        }             

		private void _OnDisposeCenter()
		{           
            _btnList = null;
            questionInfoList = null;
            QuestionAction = null;
        }
        private void _ClosePanel()
        {
			_controller.isShowBlackBg = false;
            _controller.setVisible(false);
        }

        private Color btnbgcolor = new Color(116f / 255, 116f / 255, 116f / 255, 1f);
        private Color imgbgcolor = new Color(180f / 255, 180f / 255, 180f / 255, 1f);
        private Color initcolor = new Color(255f / 255, 255f / 255, 255f / 255, 1f);
        
        public class FeelingItem
        {
            /// <summary>
            /// 临时选择的一个item组件
            /// </summary>
            public static FeelingItem tmpClickItem = null;

            private bool isOpen = false;
            public bool IsOpen
            {
                get
                {
                    return isOpen;
                }
            }

            private FeelingVo _feelVo;

            /// <summary>
            /// 玩家的排行数据
            /// </summary>
            public FeelingVo FeelVo
            {
                get
                {
                    return _feelVo;
                }

                set
                {
                    _feelVo = value;
                }
            }           

            /// <summary>
            /// 传入的obj
            /// </summary>
            private GameObject _obj;

            /// <summary>
            /// 按钮
            /// </summary>
            private Button _btnQuestion;
            /// <summary>
            /// 游戏感悟的标题
            /// </summary>
            private Text _lbQuestionName;
            /// <summary>
            /// 游戏感悟的内容
            /// </summary>
            private Text _lbDescribe;

            /// <summary>
            /// 按钮的背景图片
            /// </summary>
            private Image _imgBG;

            /// <summary>
            /// 按钮缩放的组件
            /// </summary>
            private LayoutElement _layoutEle;

            private float minHeight = 70;
            private float heightOffset = 0;
            private float minY =0;

            /// <summary>
            /// 初始化组件
            /// </summary>
            /// <param name="obj"></param>
            public void Init(GameObject obj,EventTriggerListener.VoidDelegate clickCallBack)
            {
                _obj = obj;

                _btnQuestion = obj.GetComponent<Button>();
                _layoutEle = obj.GetComponent<LayoutElement>();
                _imgBG = obj.GetComponentEx<Image>(Layout.img_QuestionBG);
                _lbQuestionName = obj.GetComponentEx<Text>(Layout.lb_itemTitle);
                _lbDescribe = obj.GetComponentEx<Text>(Layout.lb_describe);

                _btnQuestion.onClick.AddListener(() => { clickCallBack(_obj); });
            }

            /// <summary>
            /// 点击事件
            /// </summary>
            private void _OnClickHandler()
            {
                this.MovePanel(_obj);
            }

            /// <summary>
            /// 释放监听的函数
            /// </summary>
            public void Dispose()
            {
                _btnQuestion.onClick.RemoveAllListeners();
            }

            /// <summary>
            /// 设置组件存储的数据
            /// </summary>
            /// <param name="questionName"></param>
            /// <param name="describe"></param>
            public void SetData(FeelingVo value)
            {
                var tmpStri = value.content;
                var titleStr = "";
                if(tmpStri.Length>22)
                {
                    titleStr = tmpStri.Substring(0, 2);
                }else
                {
                    titleStr = tmpStri;
                }

                UICommonTool.SetTextContext(ref _lbQuestionName, titleStr);
                UICommonTool.SetTextContext(ref _lbDescribe, tmpStri);
                UICommonTool.SetComponentState(ref _lbDescribe, false);
                ClosePanel();
            }
            public void MovePanel(GameObject go)
            {
                if (isOpen)
                {
                    ClosePanel();
                }
                else
                {
                    if (_obj.Equals(go))
                    {
                        OpenPanel();
                    }                      
                }
            }
            private void ClosePanel()
            {
                isOpen = false;
                UICommonTool.SetComponentState(ref _lbDescribe, false);
                SetLayOutElementHeight(minHeight);
                UICommonTool.SetComponentHeight(ref _imgBG,minHeight);
                UICommonTool.SetComponentY(ref _imgBG,minY);
                this._lbDescribe.SetActiveEx(false);
                this._lbQuestionName.SetActiveEx(true);
            }
            private void OpenPanel()
            {
                isOpen = true;
                UICommonTool.SetComponentState(ref _lbDescribe, true);
                float height = GetContextHeight();
                SetLayOutElementHeight(height);
                UICommonTool.SetComponentHeight(ref _imgBG, height);
                //UICommonTool.SetComponentY(ref _imgBG, heightOffset - height / 2);
                this._lbDescribe.SetActiveEx(true);
                this._lbQuestionName.SetActiveEx(false);
            }
            public void SetEnable(bool enable)
            {
                if (_obj != null)
                    _obj.SetActive(enable);
            }
            private void SetLayOutElementHeight(float height)
            {
                if (_layoutEle != null)
                {
                    _layoutEle.preferredHeight = height;
                }
            }
            private float GetContextHeight()
            {
                if (_lbDescribe == null)
                {
                    return 0;
                }
                return minHeight + heightOffset + _lbDescribe.preferredHeight;
            }
        }
        public class UICommonTool
        {
            public static void SetTextContext(ref Text text, string context)
            {
                if (text != null)
                    text.text = context;
            }
            public static void SetComponentState<T>(ref T t, bool enable) where T : Component
            {
                if (t != null)
                    t.SetActiveEx(enable);
            }
            public static GameObject Clone(GameObject prefab,Transform parent,string objName = null)
            {
                GameObject clone = (GameObject)prefab.InstantiateEx();
                clone.SetActive(true);
                clone.name = string.IsNullOrEmpty(objName) ? (prefab.name + clone.GetHashCode()) : objName;
                clone.transform.SetParent(parent);
                clone.transform.localPosition = prefab.transform.localPosition;
                clone.transform.localScale = prefab.transform.localScale;
                return clone;
            }
            public static void SetComponentY<T>(ref T t,float high) where T : Graphic
            {
                if (t != null)
                {
                    Vector3 v3 = t.rectTransform.anchoredPosition3D;
                    v3.y = high;
                    t.rectTransform.anchoredPosition3D = v3;
                }
            }
            public static void SetTransformY(ref Transform trans,float high)
            {
                if (trans != null)
                {
                    Vector3 v3 = (trans as RectTransform).anchoredPosition3D;
                    v3.y = high;
                    (trans as RectTransform).anchoredPosition3D = v3;
                }
            }
            public static void SetComponentHeight<T>(ref T t,float height) where T : Graphic
            {
                if (t != null)
                {
                    Vector2 v2 = t.rectTransform.sizeDelta;
                    v2.y = height;
                    t.rectTransform.sizeDelta = v2;
                }
            }
        }
    }
}

