using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;



namespace Client.UI
{
    /// <summary>
    /// 游戏个人中心面板
    /// </summary>
    public class UIPersonalController : UIController<UIPersonalWindow, UIPersonalController>
    {

        protected override string _windowResource
        {
            get
            {
                return "prefabs/ui/scene/personalcenter.ab";
            }
        }

        protected override void _OnLoad()
        {
            base._OnLoad();
        }

        protected override void _OnShow()
        {
            base._OnShow();
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }

        protected override void _Dispose()
        {
            base._Dispose();
        }


        private TotalGameRecordVo _totalRecord;

        /// <summary>
        /// 综合的任务信息的记录
        /// </summary>
        public TotalGameRecordVo TotalGameRecord
        {
            get
            {
                return _totalRecord;
            }

            set
            {
                _totalRecord = value;
            }
        }


        public List<ListData> GetGameDataList
        {
            get
            {
                //_dataList = new List<ListData>();
                //for (int i = 0; i < 6; i++)
                //{
                //    ListData mm = new ListData();
                   
                //    mm.headPath = "share/texture/head/baoan.ab";
                //    mm.date = UnityEngine.Random.Range(2, 5).ToString();
                //    mm.money = UnityEngine.Random.Range(3, 5).ToString();
                //    mm.timeCount = UnityEngine.Random.Range(4, 8).ToString();
                //    mm.quality = UnityEngine.Random.Range(5, 8).ToString();
                //    mm.time = UnityEngine.Random.Range(6, 9).ToString();
                //    _dataList.Add(mm);
                //}

                return _dataList;
            }

            set
            {
                _dataList = value;
            }

            //if (null != playerInfor)
            //{
            //    return playerInfor.saleRecordList;
            //}           
        }

       

        public ListData GetGameDataByIndex(int index)
        {
            var items = _dataList;

            if (null != items && index < items.Count)
            {
                return items[index];
            }
            return null;
        }

        /// <summary>
        /// 玩家游戏数据data
        /// </summary>
        private List<ListData> _dataList=new List<ListData>();

               

        /// <summary>
        /// 加载网络头像图片
        /// </summary>
        /// <param name="str"></param>
        public void HeadImage(string str)
        {
            UIPersonalWindow win = _window as UIPersonalWindow;     
            AsyncImageDownload.Instance.HeadImage(win.ReturnIm());
        }

    }
}

