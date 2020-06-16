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
        private static GameGuidManager _instance =null;

        private string _wordGameHall = "doneGameHallz";
        private string _wordRoom = "doneGameRoomz";
        private string _wordNetSelect = "doneNetSelectz";
        private string _wordSelect = "doneSelectz";
        private string _wordGame = "doneGamez";
        private string _wordBorrow = "doneBorrowz";
        private string _wordPayback = "donePaybackz";

        private bool _guidGameHall;
        private bool _guidRoom;
        private bool _guidNetSelect;
        private bool _guidSelect;
        private bool _guidGame;
        private bool _guidBorrow;
        private bool _guidPayback;

        public static GameGuidManager GetInstance
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


            //_guidGameHall = false;
            //_guidRoom = false;
            //_guidNetSelect = false;
            //_guidSelect = false;
            //_guidBorrow = false;
            //_guidPayback = false;
            //_guidGame = false;

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

                if(_guidRoom==true)
                {
                    var tmpController = UIControllerManager.Instance.GetController<UIGuidRoomController>();
                    if(tmpController.getVisible())
                    {
                        tmpController.setVisible(false);
                    }
                }

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
                if(value==true)
                {
                    var tmpController = UIControllerManager.Instance.GetController<UIGuidSelectController>();
                    if(tmpController.getVisible())
                    {
                        tmpController.setVisible(false);
                    }
                }
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
        public bool DoneGamePayback
        {
            get
            {
                return _guidPayback;
            }

            set
            {
                _guidPayback = value;
                _localConfig.SaveValue(_wordPayback, value.ToString());

                if(value==true)
                {
                    var tmpController = UIControllerManager.Instance.GetController<UIGuidPaybackController>();
                    if(tmpController.getVisible())
                    {
                        tmpController.setVisible(false);
                    }
                }

            }
        }

        /// <summary>
        /// 显示游戏大厅的新手引导
        /// </summary>
        public void ShowGameHallGuid()
        {
            var tmpController = UIControllerManager.Instance.GetController<UIGuidGameHallController>();
            tmpController.setVisible(true);
        }

       /// <summary>
       /// 显示游戏房间的新手引导
       /// </summary>
        public void ShowFightRoomGuid()
        {
            var tmpController = UIControllerManager.Instance.GetController<UIGuidRoomController>();
            tmpController.setVisible(true);

        }

        /// <summary>
        /// 显示房间准备的好的引导
        /// </summary>
        public void ShowRoomReadyGuid()
        {

        }

        /// <summary>
        /// 显示单机版模式，选着角色新手引导
        /// </summary>
        public void ShowSingleSelectGuid()
        {
            var tmpController = UIControllerManager.Instance.GetController<UIGuidSelectController>();
            tmpController.setVisible(true);
        }

        /// <summary>
        /// 显示游戏界面新手引导
        /// </summary>
        public void ShowGameGuid()
        {
            var tmpController = UIControllerManager.Instance.GetController<UIGuidGameController>();
            tmpController.setVisible(true);

        }

        /// <summary>
        /// 显示借款新手引导
        /// </summary>
        public void ShowBorrowGuid()
        {
            var tmpController = UIControllerManager.Instance.GetController<UIGuidBorrowController>();
            tmpController.setVisible(true);
        }

        /// <summary>
        /// 显示还款新手引导
        /// </summary>
        public void ShowPayBackGuid()
        {
            var tmpController = UIControllerManager.Instance.GetController<UIGuidPaybackController>();
            tmpController.setVisible(true);
        }

        private LocalConfigManager _localConfig;
    }
}
