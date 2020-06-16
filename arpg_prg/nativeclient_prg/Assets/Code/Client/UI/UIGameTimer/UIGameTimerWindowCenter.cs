using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    partial class UIGameTimerWindow
    {
        private void _InitCenter(GameObject go)
        {
            this.lb_time = go.GetComponentEx<Text>(Layout.lb_time);
        }

        private void _ShowCenter()
        {
           
        }

        private void _HideCenter()
        {
           
        }

        private void _DisposeCenter()
        {

        }

        /// <summary>
        /// 设置游戏时间
        /// </summary>
        /// <param name="value"></param>
        public void SetTime(float value)
        {
            var tmpStr = "";

            if (value >= 0)
            {
                if(value>300)
                {
                    tmpStr = GameTimerManager.GetTime(value);                   
                }
                else
                {
                    tmpStr = String.Format("<color=#FF0000>{0}</color>", GameTimerManager.GetTime(value));
                }
               
            }
            else
            {
                tmpStr = "您超越时间极限";
            }

            if(null!=lb_time)
            {
                lb_time.text = tmpStr;
            }
        }

        //private 
        private Text lb_time;

    }
}
