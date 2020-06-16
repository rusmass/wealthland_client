using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.UI
{
    /// <summary>
    /// 收到好友借贷的提示面板
    /// </summary>
    class UIBorrowFriendTipController:UIController<UIBorrowFriendTipWindow, UIBorrowFriendTipController>
    {
        protected override string _windowResource
        {
            get
            {
                //uifriendborrowinfor
                return "prefabs/ui/scene/uifriendborrowinfor.ab";
            }
        }

        protected override void _OnShow()
        {
            base._OnShow();
        }

        protected override void _OnHide()
        {
            base._OnHide();
        }

        protected override void _Dispose()
        {
            base._Dispose();
        }

        /// <summary>
        /// 倒计时刷新
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Tick(float deltaTime)
        {
            if(null!=_window && this.getVisible())
            {
                (_window as UIBorrowFriendTipWindow).Tick(deltaTime);
            }
        }

        /// <summary>
        /// 设置需要借款的人的id
        /// </summary>
        public string TargetPlayerID
        {
            get
            {
                return _playerId;
            }

            set
            {
                _playerId = value;
            }
        }

        /// <summary>
        /// 需要借钱的额度
        /// </summary>
        public int TargetMoney
        {
            get
            {
                return _targetMoney;
            }

            set
            {
                _targetMoney = value;
            }
        }
         

        /// <summary>
        /// 贷款的利率，暂时不使用
        /// </summary>
        public float Rate
        {
            get
            {
                return rate;
            }

            set
            {
                rate = value;
            }
        }

        /// <summary>
        /// 玩家的名字
        /// </summary>
        public String PlayerName
        {
            get
            {
                return _playerName;
            }

            set
            {
                _playerName = value;
            }
        }

        /// <summary>
        /// 加载的头像的路径
        /// </summary>
        public string HeadPath
        {
            get
            {
                return _headPath;
            }
            set
            {
                _headPath = value;
            }
        }

        private string _playerId;
        private int _targetMoney;

        private string _playerName;

        private string _headPath;

        private float rate=1f;
    }
}
