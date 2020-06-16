using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public partial class UIEnterSuccessWindow
    {
        private void _OnInitCenter(GameObject go)
        {
            lb_income = go.GetComponentEx<Text>(Layout.lb_money);

            lb_timetxt = go.GetComponentEx<Text>(Layout.lb_time);

            lb_qualitytxt = go.GetComponentEx<Text>(Layout.lb_quality);

            lb_name = go.GetComponentEx<Text>(Layout.lb_name);

            var imgs = go.GetComponentEx<Image>(Layout.img_hero);
            imgRole = new UIImageDisplay(imgs);


            lb_income.text = "";

            lb_timetxt.text = "";

            lb_qualitytxt.text = "";

            lb_name.text = "";

        }

        private void _OnDisposeCenter()
        {
            if(null!=imgRole)
            {
                imgRole.Dispose();
            }
        }

        private void _OnShowCenter()
        {
            if (null != _controller.playerInfor)
            {
                SetCardData(_controller.playerInfor);
            }
        }

        /// <summary>
        /// 设置显示人物信息
        /// </summary>
        /// <param name="player"></param>
        public void SetCardData(PlayerInfo player)
        {
            lb_income.text = HandleStringTool.HandleMoneyTostring(player.totalMoney);
            lb_timetxt.text =string.Format("{0}/{1}",player.timeScore.ToString(),"1000");
            lb_qualitytxt.text =string.Format("{0}/{1}", player.qualityScore.ToString(),"100");

            lb_name.text = player.playerName;

            if(null!= imgRole)
            {
                imgRole.Load(player.playerImgPath);
            }
        }

        /// <summary>
        /// 当前现金
        /// </summary>
        private Text lb_income;
        /// <summary>
        /// 时间积分
        /// </summary>
        private Text lb_timetxt;

        /// <summary>
        /// 品质积分
        /// </summary>
        private Text lb_qualitytxt;

        /// <summary>
        /// 玩家姓名
        /// </summary>
        private Text lb_name;

        /// <summary>
        /// 加载角色图片
        /// </summary>
        private UIImageDisplay imgRole;
    }
}
