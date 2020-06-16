using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIConsultingWindow
    {
        #region Const
        private const float TopHigh = 155;
        #endregion
        #region Componment
        private Transform _contentTrans;
        private Transform _groupPanel;

        private Transform _questionPrefab;
        private Transform _questionGroupPrefab;


		private Image img_bg;

        private Button _btnExit;
        #endregion
        #region Normal
        private Counter _timer = null;
        private Dictionary<string, Dictionary<string, string>> consultingDataDict = null;
        private Dictionary<string, string> currentDataDict = null;
        private UIConsultingQuestionGroupInfo[] groupInfo = null;
        private List<UIConsultingQuestionInfo> questionInfoList = null;
        private EventTriggerListener.VoidDelegate QuestionAction = null;
        #endregion


        private void _OnInitCenter(GameObject go)
        {
            consultingDataDict = new Dictionary<string, Dictionary<string, string>>();
            currentDataDict = new Dictionary<string, string>();
            questionInfoList = new List<UIConsultingQuestionInfo>();
            InitData();
            _contentTrans = go.GetComponentEx<Transform>(Layout.content);
            _groupPanel = go.GetComponentEx<Transform>(Layout.groupPanel);

            _btnExit = go.GetComponentEx<Button>(Layout.btn_Exit);
            EventTriggerListener.Get(_btnExit.gameObject).onClick = (obj)=> { Audio.AudioManager.Instance.BtnMusic(); _ClosePanel(); };

            _questionPrefab = go.GetComponentEx<Transform>(Layout.content + Layout.btn_Question);
            _questionPrefab.SetActiveEx(false);

            InitQuestionGroup(_groupPanel.gameObject);

			img_bg = go.GetComponentEx<Image> (Layout.img_bgimg);

            if(groupInfo != null && groupInfo.Length > 0)
                _RefreshQuestionList(groupInfo[0].TitleName);
        }
        private void InitData()
        {
            var template = MetadataManager.Instance.GetTemplateTable<ConsultingTemplate>();
            var it = template.GetEnumerator();
            while (it.MoveNext())
            {
                var value = it.Current.Value as ConsultingTemplate;
                if (consultingDataDict.ContainsKey(value.questionGroupName))
                {
                    if (consultingDataDict[value.questionGroupName].ContainsKey(value.questionName))
                    {
                        string.Concat(consultingDataDict[value.questionGroupName][value.questionName], "\n" + value.questionDescribe);
                        Debug.Log("咨询问题有重复的。");
                    }
                    else
                    {
                        consultingDataDict[value.questionGroupName].Add(value.questionName, value.questionDescribe);
                    }
                }
                else
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add(value.questionName, value.questionDescribe);
                    consultingDataDict.Add(value.questionGroupName, data);
                }
            }
            //TODO 测试数据使用
            //foreach (var item in consultingDataDict)
            //{
            //    foreach (var context in item.Value)
            //    {
            //        Debug.Log(item.Key + "," + context.Key + "," + context.Value);
            //    }
            //}
        }
        private void InitQuestionGroup(GameObject go)
        {
            _questionGroupPrefab = go.GetComponentEx<Transform>(Layout.btn_QuestionGroup);

            groupInfo = new UIConsultingQuestionGroupInfo[consultingDataDict.Count];
            int flag = 0;
            foreach (string item in consultingDataDict.Keys)
            {
                groupInfo[flag] = new UIConsultingQuestionGroupInfo();
                if (flag == 0)
                {
                    groupInfo[flag].Init(_questionGroupPrefab.gameObject, item, _OnSelectGroupCallBack);
                }
                else
                {
                    GameObject clone = UICommonTool.Clone(_questionGroupPrefab.gameObject, go.transform);
                    groupInfo[flag].Init(clone, item, _OnSelectGroupCallBack);
					groupInfo [flag].SetBtnGray ();
                }
                flag++;
            }
        }
        private void _OnSelectGroupCallBack(string groupName)
        {
            Audio.AudioManager.Instance.BtnMusic();

			for (var i = 0; i < groupInfo.Length; i++)
			{
				groupInfo [i].SetBtnGray ();
			}


            UICommonTool.SetTransformY(ref _contentTrans, TopHigh);
            _RefreshQuestionList(groupName);
        }
        private void _RefreshQuestionList(string groupName)
        {
            currentDataDict = consultingDataDict[groupName];
            int flag = currentDataDict.Count - questionInfoList.Count;
            if (flag > 0)
            {
                UIConsultingQuestionInfo info = null;
                GameObject clone = null;
                for (int i = 0; i < flag; i++)
                {
                    clone = UICommonTool.Clone(_questionPrefab.gameObject, _contentTrans);
                    info = new UIConsultingQuestionInfo();
                    info.Init(clone,_OnClickQuestionCallBack);
                    QuestionAction += info.MovePanel;
                    questionInfoList.Add(info);
                }
            }
            else if(flag < 0)
            {
                for (int i = 0; i < questionInfoList.Count; i++)
                    questionInfoList[i].SetEnable(currentDataDict.Count > i);
            }
            flag = 0;
            foreach (KeyValuePair<string,string> item in currentDataDict)
            {
                questionInfoList[flag].SetEnable(true);
                questionInfoList[flag].SetData(item.Key,item.Value);
                flag++;
            }
        }
        private void _OnClickQuestionCallBack(GameObject go)
        {
            Audio.AudioManager.Instance.BtnMusic();
            QuestionAction(go);
        }
        private void _OnShowCenter()
		{
			if (_controller.isShowBlackBg == true)
			{
				img_bg.color = new Color (0,0,0,1);
			}
        }
        
        private void _OnTick(float deltaTime)
        {
            if (null != _timer && _timer.Increase(deltaTime))
            {
                _timer = null;
            }
        }

		private void _OnHideCenter()
		{
			
		}
        

		private void _OnDisposeCenter()
		{
            QuestionAction = null;
            _timer = null;
            consultingDataDict = null;
            currentDataDict = null;
            groupInfo = null;
            questionInfoList = null;
        }
        private void _ClosePanel()
        {
			_controller.isShowBlackBg = false;
            _controller.setVisible(false);
        }
        public class UIConsultingQuestionGroupInfo
        {
            private GameObject _obj;

            private Button _btnQuestionGroup;

            private Text _lbQuestionGroupName;

            private string titleName;
            public string TitleName { get { return titleName; } }

            public void Init(GameObject obj,string name,Action<string> clickCallBack)
            {
                _obj = obj;
                titleName = name;

                _btnQuestionGroup = obj.GetComponent<Button>();
                EventTriggerListener.Get(_btnQuestionGroup.gameObject).onClick = (go)=> {
					clickCallBack(titleName);
					SetBtnBright();
				};

                _lbQuestionGroupName = obj.GetComponentEx<Text>(Layout.lb_QuestionGroupName);
                UICommonTool.SetTextContext(ref _lbQuestionGroupName, name);
            }


			public void SetBtnBright()
			{
				_btnQuestionGroup.GetComponent<Image> ().color = initColor;
				_btnQuestionGroup.GetComponentInChildren<Text> ().color = initColor;

			}

			public void SetBtnGray()
			{
				_btnQuestionGroup.GetComponent<Image> ().color = btnbgColor;
				_btnQuestionGroup.GetComponentInChildren<Text> ().color = imgbgColor;
			}

			private Color btnbgColor = new Color (116f/255,116f/255,116f/255,1f);
			private Color imgbgColor = new Color (180f/255,180f/255,180f/255,1f);
			private Color initColor = new Color (255f/255,255f/255,255f/255,1f);
        }
        public class UIConsultingQuestionInfo
        {
            private bool isOpen = false;
            public bool IsOpen { get { return isOpen; } }

            private string _questionName;
            public string QuestionName { get { return _questionName; } }
            private string _describe;
            public string Describe { get { return _describe; } }

            private GameObject _obj;

            private Button _btnQuestion;

            private Text _lbQuestionName;
            private Text _lbDescribe;

            private Image _imgBG;

            private LayoutElement _layoutEle;

            private float minHeight = 50;
            private float heightOffset = 10;
            private float minY = -15;

            public void Init(GameObject obj,EventTriggerListener.VoidDelegate clickCallBack)
            {
                _obj = obj;

                _btnQuestion = obj.GetComponent<Button>();
                _layoutEle = obj.GetComponent<LayoutElement>();
                _imgBG = obj.GetComponentEx<Image>(Layout.img_QuestionBG);
                _lbQuestionName = obj.GetComponentEx<Text>(Layout.lb_QuestionName);
                _lbDescribe = obj.GetComponentEx<Text>(Layout.lb_Describe);

                _btnQuestion.onClick.AddListener(()=> { clickCallBack(_obj); });
            }
            public void SetData(string questionName,string describe)
            {
                _questionName = questionName;
                _describe = describe;

                UICommonTool.SetTextContext(ref _lbQuestionName, questionName);
                UICommonTool.SetTextContext(ref _lbDescribe, describe);
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
                        OpenPanel();
                }
            }
            private void ClosePanel()
            {
                isOpen = false;
                UICommonTool.SetComponentState(ref _lbDescribe, false);
                SetLayOutElementHeight(minHeight);
                UICommonTool.SetComponentHeight(ref _imgBG,minHeight);
                UICommonTool.SetComponentY(ref _imgBG,minY);
            }
            private void OpenPanel()
            {
                isOpen = true;
                UICommonTool.SetComponentState(ref _lbDescribe, true);
                float height = GetContextHeight();
                SetLayOutElementHeight(height);
                UICommonTool.SetComponentHeight(ref _imgBG, height);
                UICommonTool.SetComponentY(ref _imgBG, heightOffset - height / 2);
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

