using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



namespace Client.UI
{
    public partial class UIPersonalWindow : UIWindow<UIPersonalWindow, UIPersonalController>
    {
    
        protected override void _Init(GameObject go)
        {
            Init(go);             
        }

        protected override void _OnShow()
        {
            InitButton();
            RushPersonal();
        }

        protected override void _OnHide()
        {
            RemoveButton();
        }

        protected override void _Dispose()
        {
            if(null!= _saleWrapGrid)
            {
                for (var i = 0; i < _saleWrapGrid.Cells.Length; i++)
                {
                    var cell = _saleWrapGrid.Cells[i];
                    (cell.DisplayObject as PersonalItem).Dispose();
                }
            }
            if (null != this.head)
            {
                this.head.sprite = null;
            }

            if (null != this.picHead)
            {
                this.picHead.sprite = null;
            }

            if (null != this.setHead)
            {
                this.setHead.sprite = null;
            }

            Resources.UnloadUnusedAssets();

        }


        private void RushPersonal()
        {
            //Console.Error.WriteLine("年龄:"+nianInt+"月:"+yueInt+"日："+riInt);            
            UpdateInforData(GameModel.GetInstance.myHandInfor);

            UpdateBattleData(_controller.TotalGameRecord);
        }

        /// <summary>
        ///  更新对战信息
        /// </summary>
        public void UpdateBattleData(TotalGameRecordVo value)
        {
            allCount.text = value.totalNums.ToString();
            allWin.text = value.winRate;
            allTime.text = value.avrageTime;
           // allIntegration.text = scores;
        }

        /// <summary>
        /// 更新人物数据
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="star"></param>
        /// <param name="birthday"></param>
        public void UpdateInforData(PlayerHeadInfor data)
        {
           // var tmpPlayerVo = GameModel.GetInstance.myHandInfor;
            var tmpBirthday = "1990.11.12";
            if(data.birthday!="")
            {
                tmpBirthday = data.birthday;
            }
            string[] arr = tmpBirthday.Split(new char[1] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);

            nianInt = int.Parse(arr[0]);
            yueInt = int.Parse(arr[1]);
            riInt = int.Parse(arr[2]);

            name.text = data.nickName;
            //sex.sprite = 
            if (data.sex == 0)
            {
                img_sexdisplay.Load(_herosexwomanPath);
            }
            else
            {
                img_sexdisplay.Load(_herosexmanPath);
            }

            this.LifeNumber(nianInt.ToString()+yueInt.ToString()+riInt.ToString());

            lb_star.text = RushStar();
            birsday.text = tmpBirthday;
            AsyncImageDownload.Instance.SetAsyncImage(data.headImg, head);
        }

        private void LifeNumber(string value)
        {
           // Console.Error.WriteLine("输入的生命数字是:"+value);
            int tmpNumber = 0;
            for(var i=0;i < value.Length;i++)
            {
                tmpNumber += int.Parse(value[i].ToString());
            }
            //Console.Error.WriteLine("当前的生命数字和:"+tmpNumber);
            if (tmpNumber > 9)
            {
                LifeNumber(tmpNumber.ToString());
                return;
            }
            _lifeNumber = tmpNumber;
            btn_lifeTime.GetComponentInChildren<Text>().text = string.Format("生命数字:{0}(点击产看详情)",_lifeNumber);

            //Console.Error.WriteLine("最终的生命数字是："+tmpNumber);
        }
        
        private void RushSet()
        {
            set.gameObject.SetActiveEx(true);

            var tmpHeadVo = GameModel.GetInstance.myHandInfor;

            setName.text = tmpHeadVo.nickName;
            //nianInt = 1993;
            //yueInt = 12;
            //riInt = 31;
            InitNianList();            
            InitYueList();
            InitRiList();
            tips.gameObject.SetActive(false);
            ChangeSex((tmpHeadVo.sex==0));

            AsyncImageDownload.Instance.SetAsyncImage(tmpHeadVo.headImg, this.setHead);

        }
    }



    public class ListData
    {
        /// <summary>
        /// 头像信息
        /// </summary>
        public string headPath;
        /// <summary>
        /// 游戏是日期
        /// </summary>
        public string date;
        /// <summary>
        /// 新增的流动现金
        /// </summary>
        public string money;
        /// <summary>
        /// 时间积分
        /// </summary>
        public string timeCount;
        /// <summary>
        /// 品质积分
        /// </summary>
        public string quality;
        /// <summary>
        /// 游戏时长
        /// </summary>
        public string time;
        /// <summary>
        /// 游戏的圈数的等级0外圈，1内圈，2核心圈
        /// </summary>
        public int levelState;
        /// <summary>
        /// 对战的唯一标识号
        /// </summary>
        public string roomCode;
    }
}