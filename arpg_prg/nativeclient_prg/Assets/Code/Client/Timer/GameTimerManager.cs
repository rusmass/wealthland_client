using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;



namespace Client.UI
{
    class GameTimerManager
    {
        //private float totalTime;//剩余时间        
        // Use this for initialization 
        private static GameTimerManager smInstance =null;
        public static GameTimerManager Instance
        {
            get
            {
                if(null==smInstance)
                {
                    smInstance = new GameTimerManager();
                }
                return smInstance;
            }
        }                
        
        /// <summary>
        /// 释放时间管理资源
        /// </summary>
        public void DisposeTimer()
        {          
            if(null !=_timerCount)
            {
                _timerCount.Stop();
                _timerCount = null;
            }

            _isCount = false;

            if (null != smInstance)
            {
                smInstance = null;
            }
        }

        /// <summary>
        /// 结束倒计时
        /// </summary>
        public void Stop()
        {
            if (null != _timerCount)
            {
                _timerCount.Stop();
                _timerCount = null;
            }
        }

        /// <summary>
        /// 刷新游戏时间
        /// </summary>
        /// <param name="value"></param>
        //public void UpdateGameTime()
        //{
        //    var tmpTime = _totalTime;
        //    if (tmpTime <= 0)
        //    {
        //        tmpTime = 0;
        //    }

        //    var tmpStr = this.GetMinute(tmpTime);
        //    if(null==_timerController)
        //    {
        //        _timerController = UIControllerManager.Instance.GetController<UIGameTimerWindowController>();
        //        _timerController.setVisible(true);
        //    }
        //    if(null!=_timerController)
        //    {
        //        _timerController.SetTime(tmpStr);
        //    }
        //}
                
        public float LeftTime
        {
            get
            {
                return _totalTime;
            }
           
        }

        /// <summary>
        /// 获取剩余时间的字符串
        /// </summary>
        public string LeftTimerStr
        {
            get
            {
                if(_totalTime<=0)
                {
                    return "0:00";

                }
                return GetTime(_totalTime);
            }
        }

        /// <summary>
        /// 开始倒计时
        /// </summary>
        public void Start()
        {
            if (null == _timerCount)
            {
                _timerCount = new System.Timers.Timer(1000);
                _timerCount.Elapsed += new System.Timers.ElapsedEventHandler(_CountHandler); //到达时间的时候执行事件；   
                _timerCount.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
                _timerCount.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
            }

            var tmpPlayers = PlayerManager.Instance.Players;
            for (var i = 0; i < tmpPlayers.Length; i++)
            {
                var tmpInfor = tmpPlayers[i];
                this._outerTime[tmpInfor.playerID] = 0f;
            }


            _timerCount.Start();
            _isCount = true;
        }

        /// <summary>
        /// 是否开始计时
        /// </summary>
        private static bool _isCount=false;

        public static bool IsCount
        {
            get
            {
                return _isCount;
            }
        }



        public void _CountHandler(object obj, System.Timers.ElapsedEventArgs e)
        {
            _totalTime--;                   
        }

        /// <summary>
        /// 记录游戏开始到进入内圈的时间
        /// </summary>
        public void EnterInnerTime(string playerId)
        {
            _outerTime[playerId]= _initTime - _totalTime;
            //_enterInnerTime = _initTime - totalTime;
        }

        /// <summary>
        /// 记录游戏开始到游戏胜利的时间
        /// </summary>
        public void SuceessTime()
        {
            _successTime = _initTime - _totalTime;
        }

        /// <summary>
        ///获取在外圈的时间
        /// </summary>
        /// <returns></returns>
        public string getOuterTime(string playerId)
        {
            return GetTime(_outerTime[playerId]);// GetTime(this._enterInnerTime);
        }

        /// <summary>
        /// 获取内圈游戏时间
        /// </summary>
        /// <returns></returns>
        public string getInnerTime(string playerId)
        {
            return GetTime(this._successTime - _outerTime[playerId]);
        }

        /// <summary>
        /// 获取游戏全过程的时间
        /// </summary>
        /// <returns></returns>
        public string getGameTime()
        {
            return GetTime(this._successTime);
        }

        /// <summary>
        /// 初始化的时间
        /// </summary>
        private float _initTime = 3600;

        /// <summary>
        /// 总的时间
        /// </summary>
        private float _totalTime = 3600;

        private float _enterInnerTime = 0;

        private float _successTime = 0;

        private Timer _timerCount = null;        

        /// <summary>
        /// 获取总的时间字符串
        /// </summary>
        public static string GetTime(float time)
        {
            return GetMinute(time) +":"+ GetSecond(time);
        }
        /// <summary>
        /// 获取小时
        /// </summary>
        public static string GetHour(float time)
        {
            int timer = (int)(time / 3600);
            string timerStr;
            if (timer < 10)
                timerStr = "0" + timer.ToString() + ":";
            else
                timerStr = timer.ToString() + ":";
            return timerStr;
        }
        /// <summary>
        ///获取分钟 
        /// </summary>
        public static string GetMinute(float time)
        {
            int timer = (int)((time % 3600) / 60);
            string timerStr;
            if (timer < 10)
                timerStr = "0" + timer.ToString();
            else
                timerStr = timer.ToString();
            return timerStr;
        }

        /// <summary>
        /// 获取秒
        /// </summary>
        public static string GetSecond(float time)
        {
            int timer = (int)((time % 3600) % 60);
            string timerStr;
            if (timer < 10)
                timerStr = "0" + timer.ToString();
            else
                timerStr = timer.ToString();

            return timerStr;
        }

        /// <summary>
        /// 四个玩家在外圈的游戏时间
        /// </summary>
        private Dictionary<string, float> _outerTime = new Dictionary<string, float>();        
    }
}
