using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Client.UI;



namespace Client.UI
{
    public partial class UIPersonalWindow : UIWindow<UIPersonalWindow, UIPersonalController>
    {
        List<ItemClass.PersonalItem> ltemList;

        Button backBtn;
        Image head;
        UIImageDisplay headDisplay;
        Text name;
        Image sex;
        Text birsday;
        Text lb_star;
        Button changeName;
        UIImageDisplay img_sexdisplay;
        Button weChat;
        Button logOut;

        Text allCount;
        Text allWin;
        Text allTime;
        Text allIntegration;

        private readonly string _herosexmanPath = "share/atlas/battle/gamehall/heroman.ab";
        private readonly string _herosexwomanPath = "share/atlas/battle/gamehall/herowoman.ab";

        private readonly string _winWordPath = "share/atlas/battle/PersonalCenter/44.ab";
        private readonly string _lossWordPath = "share/atlas/battle/PersonalCenter/45.ab";


        ScrollRect scrollView;
        GridLayoutGroup grid;
        Transform obj;

        GameObject set;
        Button setBack;
        Image setHead;
        Button setChange;
        InputField setName;
        Button grilButton;
        GameObject grilDot;
        Button manButton;
        GameObject manDot;
        Dropdown nian;
        Dropdown yue;
        Dropdown ri;
        Text tips;
        Text setStar;
        Button setSave;
        Button setCan;
        private bool isSexMan;


        Image picture;
        Image picHead;
        Button picCan;
        Button picSav;
        Button picPai;
        Button picXiang;

        string nameString;
        int nianInt;
        int yueInt;
        int riInt;

        private AndroidPhoneCallBack androidCallBack;
        private IOSAlbumCamera iphoneCallBack;


        private void Init(GameObject go)
        {

            backBtn = go.GetComponentEx<Button>(Layout.back);
            head = go.GetComponentEx<Image>(Layout.head);
            headDisplay = new UIImageDisplay(head);
            name = go.GetComponentEx<Text>(Layout.name);
            sex = go.GetComponentEx<Image>(Layout.sex);
            img_sexdisplay = new UIImageDisplay(sex);
            lb_star = go.GetComponentEx<Text>(Layout.star);
            birsday = go.GetComponentEx<Text>(Layout.birsday);
            changeName = go.GetComponentEx<Button>(Layout.changeName);
            weChat = go.GetComponentEx<Button>(Layout.weChat);
            logOut = go.GetComponentEx<Button>(Layout.logOut);
            allCount = go.GetComponentEx<Text>(Layout.allCount);
            allWin = go.GetComponentEx<Text>(Layout.allWin);
            allTime = go.GetComponentEx<Text>(Layout.allTime);
            allIntegration = go.GetComponentEx<Text>(Layout.allIntegration);

            scrollView = go.GetComponentEx<ScrollRect>(Layout.scrollView);
            grid = go.GetComponentEx<GridLayoutGroup>(Layout.grid);

            obj = grid.transform.Find(Layout.item);
            set = go.transform.Find(Layout.set).gameObject;
            set.gameObject.SetActive(false);
            setBack = go.GetComponentEx<Button>(Layout.setBack);
            setHead = go.GetComponentEx<Image>(Layout.setHead);
            setChange = go.GetComponentEx<Button>(Layout.setChange);

            setName = go.GetComponentEx<InputField>(Layout.setName);
            grilButton = go.GetComponentEx<Button>(Layout.grilButton);
            grilDot = grilButton.transform.parent.Find("Red").gameObject;
            manButton = go.GetComponentEx<Button>(Layout.manButton);
            manDot = manButton.transform.parent.Find("Red").gameObject;
            nian = go.GetComponentEx<Dropdown>(Layout.nian);
            yue = go.GetComponentEx<Dropdown>(Layout.yue);
            ri = go.GetComponentEx<Dropdown>(Layout.ri);
            tips = go.GetComponentEx<Text>(Layout.tips);
            setStar = go.GetComponentEx<Text>(Layout.setStar);
            setSave = go.GetComponentEx<Button>(Layout.setSave);
            setCan = go.GetComponentEx<Button>(Layout.setCan);
            ltemList = new List<ItemClass.PersonalItem>();
            ltemList.Add(new ItemClass.PersonalItem(obj));
            EventTriggerListener.Get(ltemList[0].detail.gameObject).onClick = DetailClick;

            picture = go.GetComponentEx<Image>(Layout.picture);
            picHead = go.GetComponentEx<Image>(Layout.picHead);
            picPai = go.GetComponentEx<Button>(Layout.picPai);
            picXiang = go.GetComponentEx<Button>(Layout.picXiang);
            picSav = go.GetComponentEx<Button>(Layout.picSave);
            picCan = go.GetComponentEx<Button>(Layout.picCan);

            addPickUpCompent(go);
        }

        /// <summary>
        /// Adds the pick up compent.添加手机截图的组建
        /// </summary>
        /// <param name="go">Go.</param>
        private void addPickUpCompent(GameObject go)
        {
#if UNITY_ANDROID
            Console.WriteLine("当前是安卓平台啊");
            if (null == androidCallBack)
            {
                androidCallBack = go.AddComponent<AndroidPhoneCallBack>();
            }
            androidCallBack.img_selecthead = picHead;
#elif UNITY_IPHONE
			Console.WriteLine ("当前是ios平台啊");
			if (null == iphoneCallBack)
			{
			iphoneCallBack = go.AddComponent<IOSAlbumCamera> ();
			}
			iphoneCallBack.img_selecthead = picHead;
#endif
        }

        public void InitButton()
        {
            EventTriggerListener.Get(backBtn.gameObject).onClick += Back;
            EventTriggerListener.Get(changeName.gameObject).onClick += ChangeName;
            EventTriggerListener.Get(weChat.gameObject).onClick += WeChat;
            EventTriggerListener.Get(logOut.gameObject).onClick += LogOut;
            EventTriggerListener.Get(setBack.gameObject).onClick += SetBack;
            EventTriggerListener.Get(setChange.gameObject).onClick += ChangeHead;
            EventTriggerListener.Get(grilButton.gameObject).onClick += GrilButton;
            EventTriggerListener.Get(manButton.gameObject).onClick += ManButton;
            EventTriggerListener.Get(setSave.gameObject).onClick += SaveInforHandler;
            EventTriggerListener.Get(setCan.gameObject).onClick += SetCan;
            EventTriggerListener.Get(picPai.gameObject).onClick += PaiZhao;
            EventTriggerListener.Get(picXiang.gameObject).onClick += XiangCe;
            EventTriggerListener.Get(picSav.gameObject).onClick += PicSave;
            EventTriggerListener.Get(picCan.gameObject).onClick += PicCan;

            setName.onValueChanged.AddListener((string s) => { NameChange(s); });
            nian.onValueChanged.AddListener((int s) => { NianChange(s); });
            yue.onValueChanged.AddListener((int s) => { YueChange(s); });
            ri.onValueChanged.AddListener((int s) => { RiChange(s); });

            setStar.text = RushStar();
        }

        private void RemoveButton()
        {
            EventTriggerListener.Get(backBtn.gameObject).onClick -= Back;
            EventTriggerListener.Get(changeName.gameObject).onClick -= ChangeName;
            EventTriggerListener.Get(weChat.gameObject).onClick -= WeChat;
            EventTriggerListener.Get(logOut.gameObject).onClick -= LogOut;
            EventTriggerListener.Get(setBack.gameObject).onClick -= SetBack;
            EventTriggerListener.Get(setChange.gameObject).onClick -= ChangeHead;
            EventTriggerListener.Get(grilButton.gameObject).onClick -= GrilButton;
            EventTriggerListener.Get(manButton.gameObject).onClick -= ManButton;
            EventTriggerListener.Get(setSave.gameObject).onClick -= SaveInforHandler;
            EventTriggerListener.Get(setCan.gameObject).onClick -= SetCan;
            EventTriggerListener.Get(picPai.gameObject).onClick -= PaiZhao;
            EventTriggerListener.Get(picXiang.gameObject).onClick -= XiangCe;
            EventTriggerListener.Get(picSav.gameObject).onClick -= PicSave;
            EventTriggerListener.Get(picCan.gameObject).onClick -= PicCan;
            setName.onValueChanged.RemoveListener((string s) => { NameChange(s); });
            nian.onValueChanged.RemoveListener((int s) => { NianChange(s); });
            yue.onValueChanged.RemoveListener((int s) => { YueChange(s); });
            ri.onValueChanged.RemoveListener((int s) => { RiChange(s); });
        }

        private void Back(GameObject obj)
        {
            if (null != _controller)
            {
                _controller.setVisible(false);
            }
        }
        private void ChangeName(GameObject obj)
        {
            RushSet();
        }
        private void WeChat(GameObject obj)
        {

        }
        private void LogOut(GameObject obj)
        {
            Audio.AudioManager.Instance.BtnMusic();
            Console.WriteLine("登出游戏，，，，");

            NetWorkScript.getInstance().CloseNet();
            PlayerPrefs.SetString(GameModel.GetInstance.UserId, "");
            PlayerPrefs.SetString(GameModel.GetInstance.WeChatLastLoginTime, "-1");
            var gameHall = UIControllerManager.Instance.GetController<UIGameHallWindowController>();
            gameHall.setVisible(false);
            _controller.setVisible(false);
            Game.Instance.SwitchLoginWindow();
        }
        private void SetBack(GameObject obj)
        {
            set.gameObject.SetActive(false);
        }

        private void SaveInforHandler(GameObject go)
        {
            if (setName.text == "")
            {
                MessageHint.Show("姓名不能为空");
                return;
            }

            if (nian.options[nian.value].text=="")
            {
                MessageHint.Show("年份不能为空");
                return;
            }

            if (yue.options[yue.value].text == "")
            {
                MessageHint.Show("月份不能为空");
                return;
            }

            if (ri.options[ri.value].text == "")
            {
                MessageHint.Show("日期不能为空");
                return;
            }

            var dataStr = string.Format("{0}.{1}.{2}", nian.options[nian.value].text, yue.options[yue.value].text, ri.options[ri.value].text);

            int tmpsex = 1;
            if (isSexMan == false)
            {
                tmpsex = 0;
            }


            UpdateInforData(setName.text, setStar.text, dataStr, tmpsex);

            set.gameObject.SetActive(false);

        }

        private void ChangeHead(GameObject obj)
        {
            picture.SetActiveEx(true);
            picHead.sprite = null;
        }
        private void GrilButton(GameObject obj)
        {
            ChangeSex(true);
        }
        private void ManButton(GameObject obj)
        {
            ChangeSex(false);
        }
        private void SetSave(GameObject obj)
        {
            Debug.Log("像服务器发送");
        }
        private void SetCan(GameObject obj)
        {
            set.gameObject.SetActive(false);
        }
        private void PaiZhao(GameObject obj)
        {
           // AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
           // AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
           // jo.Call("TakePhoto", 0, 200, 200);

#if UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("TakePhoto", 0, 200, 200, "personalcenter", "HeadImage");
#elif UNITY_IPHONE
			IOSAlbumCamera.iosOpenCamera (true);
#endif
        }
        private void XiangCe(GameObject obj)
        {

#if UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("TakePhoto", 1, 200, 200, "personalcenter", "HeadImage");
#elif UNITY_IPHONE
			IOSAlbumCamera.iosOpenPhotoAlbums (true);
#endif
           // AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
           // AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
           // jo.Call("TakePhoto", 1, 200, 200);
        }
        private void PicCan(GameObject obj)
        {
            picture.SetActiveEx(false);
        }
        private void PicSave(GameObject obj)
        {
            if (picHead.sprite != null)
            {
               // head.sprite = picHead.sprite;
                setHead.sprite = picHead.sprite;
            }
            picture.SetActiveEx(false);
        }

        private void NameChange(string s)
        {

        }
        private void NianChange(int s)
        {
            nianInt = int.Parse(nian.options[s].text);
            InitRiList();
            setStar.text =  RushStar();
        }
        private void YueChange(int s)
        {
            yueInt = int.Parse(yue.options[s].text);
            InitRiList();
            //RushStar();
            setStar.text = RushStar();
        }
        private void RiChange(int s)
        {
            riInt = int.Parse(ri.options[s].text);
            setStar.text =  RushStar();
        }

        private void InitNianList()
        {
            if (nian != null)
            {
                nian.options.Clear();
                for (int i = 1950; i <= System.DateTime.Now.Year; i++)
                {
                    nian.options.Add(new Dropdown.OptionData(i.ToString()));
                    if (i == nianInt)
                        nian.value = nian.options.Count - 1;
                }
            }
        }

        private void InitYueList()
        {
            if (yue != null)
            {
                yue.options.Clear();
                for (int i = 1; i <= 12; i++)
                {
                    yue.options.Add(new Dropdown.OptionData(i.ToString()));
                    if (i == yueInt)
                        yue.value = yue.options.Count - 1;
                }
            }
        }

        private void InitRiList()
        {
            if (ri != null)
            {
                if (riInt > System.DateTime.DaysInMonth(nianInt, yueInt))
                {
                    riInt = 1;
                    ri.value = 0;
                }
                ri.options.Clear();
                for (int i = 1; i <= System.DateTime.DaysInMonth(nianInt, yueInt); i++)
                {
                    ri.options.Add(new Dropdown.OptionData(i.ToString()));
                    if(i==riInt)
                        ri.value = ri.options.Count - 1;
                }
            }
        }



        private void ChangeSex(bool isGril)
        {
            grilDot.SetActive(isGril);
            manDot.SetActive(!isGril);
            isSexMan = !isGril;
        }

        private string RushStar()
        {
            switch (yueInt)
            {
                case 1:
                    return riInt >= 20 ? "水瓶座" : "摩羯座";
                case 2:
                    return riInt >= 20 ? "双鱼座" : "水瓶座";
                case 3:
                    return riInt >= 20 ? "白羊座" : "双鱼座";
                case 4:
                    return riInt >= 20 ? "金牛座" : "白羊座";
                case 5:
                    return riInt >= 20 ? "双子座" : "金牛座";
                case 6:
                    return riInt >= 20 ? "巨蟹座" : "双子座";
                case 7:
                    return riInt >= 20 ? "狮子座" : "巨蟹座";
                case 8:
                    return riInt >= 20 ? "处女座" : "狮子座";
                case 9:
                    return riInt >= 20 ? "天秤座" : "处女座";
                case 10:
                    return riInt >= 20 ? "天蝎座" : "天秤座";
                case 11:
                    return riInt >= 20 ? "射手座" : "天蝎座";
                case 12:
                    return riInt >= 20 ? "摩羯座" : "射手座";
                default:
                    return "";
            }
        }

        private void DetailClick(GameObject obj)
        {
            var hallset = UIControllerManager.Instance.GetController<UIPersonGameRecodeController>();
            hallset.setVisible(true);
        }
        public Image ReturnIm()
        {
            return picHead;
        }


        private void ShowTips(string ti)
        {
            tips.gameObject.SetActive(true);
            tips.text = ti;
            AsyncImageDownload.Instance.ShowGameObjectFalse(tips.gameObject, 2);
        }
    }
}
