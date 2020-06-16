using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    /// <summary>
    /// 新手引导管理器
    /// </summary>
    class GameGuidManager
    {
        private static GameGuidManager _instance=null;

        private string _wordGameHall = "doneGameHall";
        private string _wordRoom = "doneGameRoom";
        private string _wordNetSelect = "doneNetSelect";
        private string _wordSelect = "doneSelect";
        private string _wordGame = "doneGame";
        private string _wordBorrow = "doneBorrow";
        private string _wordPayback = "donePayback";

        private bool _guidGameHall;
        private bool _guidRoom;
        private bool _guidNetSelect;
        private bool _guidSelect;
        private bool _guidGame;
        private bool _guidBorrow;
        private bool _guidPayback;



        public GameGuidManager GetInstance
        {
            get
            {
                if(null==_instance)
                {
                    _instance = new GameGuidManager();
                }

                return _instance;
            }
        }


        private GameGuidManager()
        {
            _localConfig = new LocalConfigManager();
            _guidGameHall=bool.Parse(_localConfig.LoadValue(_wordGameHall, "false"));
            _guidRoom = bool.Parse(_localConfig.LoadValue(_wordRoom,"false"));
            _guidNetSelect = bool.Parse(_localConfig.LoadValue(_wordNetSelect, "false"));
            _guidSelect = bool.Parse(_localConfig.LoadValue(_wordSelect, "false"));
            _guidBorrow = bool.Parse(_localConfig.LoadValue(_wordBorrow, "false"));
            _guidPayback = bool.Parse(_localConfig.LoadValue(_wordPayback, "false"));
            _guidGame = bool.Parse(_localConfig.LoadValue(_wordGame, "false"));

        }

        /// <summary>
        ///  是否完成游戏大厅的新手引导？每次初始化的时候都会去读取临时变量或者（每次游戏初始化的时候，读取一个变量，如果变量不符合条件，改变变量的值）
        /// </summary>
        public bool DoneGameHall
        {
            get
            {
                return _guidGameHall;
            }

            set
            {
                _guidGameHall = value;
                _localConfig.SaveValue(_wordGameHall, value.ToString());
            }
        }

        /// <summary>
        /// 是否房完成房间新手引导
        /// </summary>
        public bool DoneRoom
        {
            get
            {
                return _guidRoom;
            }

            set
            {
                _guidRoom = value;
                _localConfig.SaveValue(_wordRoom, value.ToString());
            }
        }
        

        /// <summary>
        /// 是否完成网络版选择角色的功能
        /// </summary>
        public bool DoneGameNetSelect
        {
            get
            {
                return _guidNetSelect;
            }

            set
            {
                _guidNetSelect = value;
                _localConfig.SaveValue(_wordNetSelect, value.ToString());
            }
        }

        /// <summary>
        ///  是否是完成了单机版选择角色的新手引导
        /// </summary>
        public bool DoneGameSelect
        {
            get
            {
                return _guidSelect;
            }

            set
            {
                _guidSelect = value;
                _localConfig.SaveValue(_wordSelect,value.ToString());
            }
        }


        /// <summary>
        /// 是否是完成了游戏界面的新手引导
        /// </summary>
        public bool DoneGameWindow
        {
            get
            {
                return _guidGame;
            }

            set
            {
                _guidGame = value;
                _localConfig.SaveValue(_wordGame, value.ToString());
            }
        }

        /// <summary>
        ///  是否完成借款界面的新手引导
        /// </summary>
        public bool DoneGameBorrow
        {
            get
            {
                return _guidBorrow;
            }

            set
            {
                _guidBorrow = value;
                _localConfig.SaveValue(_wordBorrow, value.ToString());
            }
        }

        /// <summary>
        /// 是否完成还款界面的新手引导
        /// </summary>
        public bool DownGamePayback
        {
            get
            {
                return _guidPayback;
            }

            set
            {
                _guidPayback = value;

            }
        }

        private LocalConfigManager _localConfig;
    }
}
