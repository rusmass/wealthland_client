using System;
using Metadata;
using System.Collections.Generic;


namespace Client.UI
{
    /// <summary>
    /// 咨询界面
    /// </summary>
	public class UIFeelingBaordController : UIController<UIFeelingBaordWindow, UIFeelingBaordController>
	{

        protected override string _windowResource {
			get {
				return "prefabs/ui/scene/uifeellist.ab";
			}
		}
        
		protected override void _OnLoad ()
		{
			
		}

		protected override void _OnShow ()
		{

        }

		protected override void _OnHide ()
		{
			
		}

		protected override void _Dispose ()
		{

        }
       
        private List<FeelingVo> _gameFeelList=new List<FeelingVo>();
        /// <summary>
        /// 游戏总的感悟列表
        /// </summary>
        public List<FeelingVo> GameFeeling
        {
            get
            {
                return _gameFeelList;
            }
            set
            {
                _gameFeelList = value;
            }
        }

        private List<FeelingVo> _selfFeelList=new List<FeelingVo>();

        /// <summary>
        /// 游戏总个人的感悟
        /// </summary>
        public List<FeelingVo> SelfFeelList
        {
            get
            {
                return _selfFeelList;
            }

            set
            {
                _selfFeelList = value;
            }
        }

        private int _GameFeelingPages=-1;

        private int _SelfFeelingPages=-1;

        /// <summary>
        /// 游戏感悟的总页数
        /// </summary>
        public int GameFeelPages
        {
            get
            {
                return _GameFeelingPages;
            }
            set
            {
                _GameFeelingPages = value;
            }
        }
        /// <summary>
        /// 玩家的自己感悟的总页数
        /// </summary>
        public int SelfFeelPages
        {
            get
            {
                return _SelfFeelingPages;
                  
            }

            set
            {
                _SelfFeelingPages = value;
            }
        }

        public int currentGameFeeling = 0;

        public int currentSelfFeeling = 0;

		public bool isShowBlackBg=false;      
        
        /// <summary>
        /// 刷新游戏感悟列表
        /// </summary>
        public void UpdateGameFeels()
        {
            if(null!=_window && getVisible())
            {
                (_window as UIFeelingBaordWindow).UpdateGameFeeling(currentGameFeeling,GameFeelPages);
            }
        }
        /// <summary>
        /// 刷新自己的感悟列表
        /// </summary>
        public void UpdateSelfFeels()
        {
            if (null != _window && getVisible())
            {
                (_window as UIFeelingBaordWindow).UpdateSelfFeeling(currentSelfFeeling, SelfFeelPages);
            }
        }

        /// <summary>
        /// 是否需要网络请求新的页数
        /// </summary>
        /// <param name="index"></param> 1   3
        /// <returns></returns>
        public bool IsRequestGameFeel(int index)
        {
            if(_isAllLoadGameFeel==true)
            {
                return false;
            }
            if(_GameFeelingPages<=0)
            {
                return true;
            }
            //0--9  ,10--19,
            return (index - 1) * 10 > (GameFeeling.Count - 1);///true;// 
        }

        /// <summary>
        /// 是否是需要加载游戏的感悟
        /// </summary>
        public bool IsAllLoadGameFeel
        {
            set
            {
                _isAllLoadGameFeel = value;
            }

        }
        /// <summary>
        /// 是否是需要个人的游戏感悟
        /// </summary>
        public bool IsAllLoadSelfFeel
        {
            set
            {
                _isAllLoadSelfFeel = value;
            }
        }

        private bool _isAllLoadGameFeel=false;
        private bool _isAllLoadSelfFeel=false;

        /// <summary>
        /// 是否需要在线请求个人的感悟列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsRequestSelfFeel(int index)
        {
            if(_isAllLoadSelfFeel==true)
            {
                return false;
            }

            if(_SelfFeelingPages<=0)
            {
                return true;
            }

            return (index - 1) * 10 > (SelfFeelList.Count - 1);

            //return index <= _SelfFeelingPages;
        }


        
	}
}

