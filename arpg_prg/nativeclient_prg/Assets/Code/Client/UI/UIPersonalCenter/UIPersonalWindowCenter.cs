using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Client.UI;

namespace Client.UI
{
    public partial class UIPersonalWindow : UIWindow<UIPersonalWindow, UIPersonalController>
    {       
        /// <summary>
        /// 返回按钮
        /// </summary>
        Button backBtn;
        /// <summary>
        /// 头像信息
        /// </summary>
        Image head;
        
        /// <summary>
        /// 名字昵称
        /// </summary>
        Text name;
        /// <summary>
        /// 显示性别的小图标
        /// </summary>
        Image sex;
        /// <summary>
        /// 年龄
        /// </summary>
        Text birsday;
        /// <summary>
        /// 星座文本
        /// </summary>
        Text lb_star;

        /// <summary>
        /// 编辑信息
        /// </summary>
        Button changeName;
        /// <summary>
        /// 显示玩家的性别
        /// </summary>
        UIImageDisplay img_sexdisplay;
        /// <summary>
        /// 微信绑定按钮
        /// </summary>
        Button weChat;
        /// <summary>
        /// 退出登录按钮
        /// </summary>
        Button logOut;

        /// <summary>
        /// 生命数字
        /// </summary>
        Button btn_lifeTime;

        int _lifeNumber = 1;

        /// <summary>
        /// 总的场次
        /// </summary>
        Text allCount;
        /// <summary>
        /// 总的胜率
        /// </summary>
        Text allWin;
        /// <summary>
        /// 平均时长
        /// </summary>
        Text allTime;
        /// <summary>
        /// 暂时未使用
        /// </summary>
        Text allIntegration;

        private readonly string _herosexmanPath = "share/atlas/battle/gamehall/heroman.ab";
        private readonly string _herosexwomanPath = "share/atlas/battle/gamehall/herowoman.ab";       
        
        /// <summary>
        /// 展示游戏记录的容器
        /// </summary>
        //ScrollRect scrollView;
        /// <summary>
        /// 容器存放item的容器
        /// </summary>
        //GridLayoutGroup grid;
        /// <summary>
        /// 容器组件
        /// </summary>
        //Transform obj;

        private GameObject content_item;
        private UIWrapGrid _saleWrapGrid;

        /// <summary>
        /// 没有信息记录的文本提示
        /// </summary>
        private Text textNoRecord;
        /// <summary>
        /// 对战记录的scrollView
        /// </summary>
        private ScrollRect scrollView;

        /// <summary>
        /// 设置界面面板
        /// </summary>
        GameObject set;
        /// <summary>
        /// 设置返回按钮
        /// </summary>
        Button setBack;
        /// <summary>
        /// 设置界面头像图片
        /// </summary>
        Image setHead;
        /// <summary>
        /// 更改头像按钮
        /// </summary>
        Button setChange;
        /// <summary>
        /// 修改名字
        /// </summary>
        InputField setName;

        /// <summary>
        /// 女孩选择
        /// </summary>
        Button grilButton;
        /// <summary>
        /// 女孩点
        /// </summary>
        GameObject grilDot;
        /// <summary>
        /// 男孩选择
        /// </summary>
        Button manButton;
        /// <summary>
        /// 男孩点
        /// </summary>
        GameObject manDot;
        /// <summary>
        /// 年份下拉框
        /// </summary>
        Dropdown nian;
        /// <summary>
        /// 月份下拉框
        /// </summary>
        Dropdown yue;
        /// <summary>
        /// 日期下拉框
        /// </summary>
        Dropdown ri;

        Text tips;

        /// <summary>
        /// 设置界面星座文本
        /// </summary>
        Text setStar;
        /// <summary>
        /// 设置界面保存按钮
        /// </summary>
        Button setSave;
        /// <summary>
        /// 设置界面取消按钮
        /// </summary>
        Button setCan;

        private bool isSexMan;

        
        /// <summary>
        /// 选择头像面板
        /// </summary>
        Image picture;
        /// <summary>
        /// 选择头像面板头像图片
        /// </summary>
        Image picHead;
        /// <summary>
        /// 选择头像面板取消
        /// </summary>
        Button picCan;
        /// <summary>
        /// 选择头像面板保存按钮
        /// </summary>
        Button picSav;
        /// <summary>
        /// 选择头像面板拍照按钮
        /// </summary>
        Button picPai;
        /// <summary>
        /// 选择头像面板相册按钮
        /// </summary>
        Button picXiang;

        /// <summary>
        /// 是否是更新最新了玩家信息
        /// </summary>
        private bool _isInforUpdated = false;
       
        int nianInt;
        int yueInt;
        int riInt;

       

        private AndroidPhoneCallBack androidCallBack;
        private IOSAlbumCamera iphoneCallBack;

        private void Init(GameObject go)
        {
            backBtn = go.GetComponentEx<Button>(Layout.back);
            head = go.GetComponentEx<Image>(Layout.head);           
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
            
            content_item = go.DeepFindEx("Item").gameObject;
            
            scrollView = go.GetComponentEx<ScrollRect>(Layout.scrollView);
            this.textNoRecord = go.GetComponentEx<Text>(Layout.txtNoRecord);

            //grid = go.GetComponentEx<GridLayoutGroup>(Layout.grid);
            //obj = grid.transform.Find(Layout.item);

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

            picture = go.GetComponentEx<Image>(Layout.picture);
            picHead = go.GetComponentEx<Image>(Layout.picHead);
            picPai = go.GetComponentEx<Button>(Layout.picPai);
            picXiang = go.GetComponentEx<Button>(Layout.picXiang);
            picSav = go.GetComponentEx<Button>(Layout.picSave);
            picCan = go.GetComponentEx<Button>(Layout.picCan);

            btn_lifeTime = go.GetComponentEx<Button>(Layout.btn_lifeNum);

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

            EventTriggerListener.Get(btn_lifeTime.gameObject).onClick +=_CheckLifeNum;

            setName.onValueChanged.AddListener((string s) => { NameChange(s); });
            nian.onValueChanged.AddListener((int s) => { NianChange(s); });
            yue.onValueChanged.AddListener((int s) => { YueChange(s); });
            ri.onValueChanged.AddListener((int s) => { RiChange(s); });

            setStar.text = RushStar();

            _CreateWrapGridForSale(content_item);

        }


        private void _CreateWrapGridForSale(GameObject go)
        {
            if (_controller.TotalGameRecord.totalNums <= 0 ||_controller.GetGameDataList.Count<=0)
            {
                this.scrollView.SetActiveEx(false);
                this.textNoRecord.SetActiveEx(true);
                return;
            }


            var items = _controller.GetGameDataList;
            if (items.Count <= 0)
            {
                go.SetActive(false);
                return;
            }
            else
            {
                go.SetActive(true);
            }

            _saleWrapGrid = new UIWrapGrid(go, items.Count);
            for (var i = 0; i < _saleWrapGrid.Cells.Length; i++)
            {
                var cell = _saleWrapGrid.Cells[i];
                cell.DisplayObject = new PersonalItem(cell.GetTransform().gameObject);
            }
            _saleWrapGrid.OnRefreshCell += _OnRefreshCell;
            _saleWrapGrid.Refresh();
        }

        private void _OnRefreshCell(UIWrapGridCell cell)
        {
            var index = cell.Index;
            var value = _controller.GetGameDataByIndex(index);
            var display = cell.DisplayObject as PersonalItem;
            display.Refresh(value);
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

            EventTriggerListener.Get(btn_lifeTime.gameObject).onClick += _CheckLifeNum;

            setName.onValueChanged.RemoveListener((string s) => { NameChange(s); });
            nian.onValueChanged.RemoveListener((int s) => { NianChange(s); });
            yue.onValueChanged.RemoveListener((int s) => { YueChange(s); });
            ri.onValueChanged.RemoveListener((int s) => { RiChange(s); });
        }

        private void _CheckLifeNum(GameObject go)
        {
            var controller = UIControllerManager.Instance.GetController<UINativeWebController>();
            Console.WriteLine("http://www.selfwealth.com.cn/WEWC0200/WEWA0205/" + _lifeNumber);
            controller.SetTargetUrl("http://www.selfwealth.com.cn/WEWC0200/WEWA0205/"+_lifeNumber);

            controller.setVisible(true);

        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        /// <param name="obj"></param>
        private void Back(GameObject obj)
        {
            if (null != _controller)
            {
                _controller.setVisible(false);
            }

            if(this._isInforUpdated==true)
            {
                var controller = UIControllerManager.Instance.GetController<UIGameHallWindowController>();
                controller.UpdatePlayerHeadInfor();
            }
        }
        /// <summary>
        /// 修改名字
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeName(GameObject obj)
        {
            RushSet();
        }
        private void WeChat(GameObject obj)
        {

        }
        /// <summary>
        /// 登出游戏
        /// </summary>
        /// <param name="obj"></param>
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
        /// <summary>
        /// 关闭设置面板
        /// </summary>
        /// <param name="obj"></param>
        private void SetBack(GameObject obj)
        {
            set.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置玩家信息
        /// </summary>
        /// <param name="go"></param>
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


            var tarBirthday = string.Format("{0}.{1}.{2}", nianInt, yueInt, riInt);
            var tarName= setName.text;

            var myInfor = GameModel.GetInstance.myHandInfor;

            if(myInfor.nickName==tarName && tarBirthday==myInfor.birthday&& tmpsex==myInfor.sex)
            {
                //Console.Error.WriteLine("当前的信息是一致的");
                set.gameObject.SetActive(false);
                return;
            }


            var tmpdata = new LitJson.JsonData();
            tmpdata["name"] = tarName;
            tmpdata["gender"] = tmpsex;
            tmpdata["birthday"] = tarBirthday;
            tmpdata["constellation"] = this.RushStar();
            tmpdata["playerId"] = myInfor.uuid;

            HttpRequestManager.GetInstance().UpdatePlayerInfor(tmpdata.ToJson(), _UpdateInforBack);           
            set.gameObject.SetActive(false);           
        }

        /// <summary>
        /// 玩家更新消息成功的回调函数
        /// </summary>
        private void _UpdateInforBack()
        {
            this._isInforUpdated = true;
            UpdateInforData(GameModel.GetInstance.myHandInfor);
        }

        /// <summary>
        /// 改变头像
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeHead(GameObject obj)
        {
            picture.SetActiveEx(true);
            picHead.sprite = null;
        }
        /// <summary>
        /// 选择女性
        /// </summary>
        /// <param name="obj"></param>
        private void GrilButton(GameObject obj)
        {
            ChangeSex(true);
        }
        /// <summary>
        /// 选择男性别
        /// </summary>
        /// <param name="obj"></param>
        private void ManButton(GameObject obj)
        {
            ChangeSex(false);
        }
        /// <summary>
        /// 未引用
        /// </summary>
        /// <param name="obj"></param>
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
            Console.Error.WriteLine("掉漆当前的具体的activity----"+ jc);
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
            //Console.Error.WriteLine("调起activity----"+jc);
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            //Console.Error.WriteLine("获取当前的activity----" + jo);
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
                //setHead.sprite = picHead.sprite;                
                HttpRequestManager.GetInstance().UpLoadImage(picHead.sprite.texture, UploadImgSuccess);

            }
            
        }

        /// <summary>
        /// 上传头像成功后返回的消息
        /// </summary>
        private void UploadImgSuccess()
        {
            setHead.sprite = picHead.sprite;
            _isInforUpdated = true;
            picture.SetActiveEx(false);
        }

        private void NameChange(string s)
        {

        }
        /// <summary>
        /// 修改年
        /// </summary>
        /// <param name="s"></param>
        private void NianChange(int s)
        {
            nianInt = int.Parse(nian.options[s].text);
            InitRiList();
            setStar.text =  RushStar();
        }
        /// <summary>
        /// 修改月
        /// </summary>
        /// <param name="s"></param>
        private void YueChange(int s)
        {
            yueInt = int.Parse(yue.options[s].text);
            InitRiList();           
            setStar.text = RushStar();
        }
        /// <summary>
        /// 修改日期
        /// </summary>
        /// <param name="s"></param>
        private void RiChange(int s)
        {
            riInt = int.Parse(ri.options[s].text);
            setStar.text =  RushStar();
        }

        /// <summary>
        /// 刷新年份列表
        /// </summary>
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

        /// <summary>
        /// 刷新月份信息
        /// </summary>
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
        /// <summary>
        /// 刷新日期列表
        /// </summary>
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

        /// <summary>
        /// 设置星座
        /// </summary>
        /// <returns></returns>
        private string RushStar()
        {
            switch (yueInt)
            {
                case 1:
                    return riInt >= 20 ? "水瓶座" : "摩羯座";
                case 2:
                    return riInt >= 19 ? "双鱼座" : "水瓶座";
                case 3:
                    return riInt >= 21 ? "白羊座" : "双鱼座";
                case 4:
                    return riInt >= 20 ? "金牛座" : "白羊座";
                case 5:
                    return riInt >= 21 ? "双子座" : "金牛座";
                case 6:
                    return riInt >= 22 ? "巨蟹座" : "双子座";
                case 7:
                    return riInt >= 23 ? "狮子座" : "巨蟹座";
                case 8:
                    return riInt >= 23 ? "处女座" : "狮子座";
                case 9:
                    return riInt >= 23 ? "天秤座" : "处女座";
                case 10:
                    return riInt >= 24 ? "天蝎座" : "天秤座";
                case 11:
                    return riInt >= 23 ? "射手座" : "天蝎座";
                case 12:
                    return riInt >= 22 ? "摩羯座" : "射手座";
                default:
                    return "";
            }
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
