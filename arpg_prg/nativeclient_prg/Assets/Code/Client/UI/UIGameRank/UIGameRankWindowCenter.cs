using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIGameRankWindow
	{
		private Transform _contentTrans;
		private Transform _groupPanel;

//		private Transform _questionPrefab;
//		private Transform _questionGroupPrefab;

		private Image img_bg;
		private Button _btnExit;

		private void _OnInitCenter(GameObject go)
		{
			_contentTrans = go.GetComponentEx<Transform>(Layout.content);
			_groupPanel = go.GetComponentEx<Transform>(Layout.groupPanel);

			_btnExit = go.GetComponentEx<Button>(Layout.btn_Exit);

//			_questionPrefab = go.GetComponentEx<Transform>(Layout.btn_Question);
//			_questionPrefab.SetActiveEx(false);

			rankItem = go.GetComponentEx<Button> (Layout.btn_Question);
			rankItem.SetActiveEx (false);

			rankItem = go.GetComponentEx<Button> (Layout.btn_Question);

			btn_activeRank = go.GetComponentEx<Button> (Layout.btn_QuestionGroup);

			img_bg = go.GetComponentEx<Image> (Layout.img_bgimg);

		}
		private void _OnClickQuestionCallBack(GameObject go)
		{
			Audio.AudioManager.Instance.BtnMusic();
		}
		private void _OnShowCenter()
		{
			if (_controller.isShowBlackBg == true)
			{
				img_bg.color = new Color (0,0,0,1);
			}

			EventTriggerListener.Get(_btnExit.gameObject).onClick = (obj)=> { Audio.AudioManager.Instance.BtnMusic(); _ClosePanel(); };

			_setBtnType (btn_activeRank,"活跃榜");

			for (var i = 0; i < 11; i++)
			{
				RankItem tmpRankItem;
				if (i == 0)
				{
					tmpRankItem = new RankItem ();
					tmpRankItem.InitItem (rankItem.gameObject);

					rankItemList.Add (tmpRankItem);
				}
				else
				{						
					var tmpBtn =rankItem.gameObject.CloneEx().GetComponent<Button>()  ;
					tmpBtn.transform.SetParent (rankItem.transform.parent);
					tmpBtn.transform.position = rankItem.transform.position;
					tmpBtn.transform.localScale = Vector3.one;
					tmpBtn.transform.rotation =rankItem.transform.rotation;

					tmpRankItem = new RankItem ();
					tmpRankItem.InitItem (tmpBtn.gameObject);

					rankItemList.Add (tmpRankItem);
				}
			}

			updateRank (RankType.ActiveType);
		
		}

		private void _OnHideCenter()
		{
			for (var i = 0; i < rankItemList.Count; i++)
			{
				if (null != rankItemList [i])
				{
					rankItemList [i].Dispose ();
				}
			}
		}


		public void updateRank(int rankType)
		{
			var dataLen = _controller.activeRankList.Count;
			for (var i = 0; i < rankItemList.Count; i++)
			{
				if (null != rankItemList [i])
				{
					if (i < dataLen)
					{
						rankItemList [i].UpateData(_controller.activeRankList[i],rankType);
					}
					else
					{
						rankItemList [i].UpateData (null,rankType);
					}
				}
			}
		}


		private void _OnDisposeCenter()
		{
		}

		private void _OnTick(float deltaTime)
		{
			
		}


		private void _ClosePanel()
		{
			_controller.isShowBlackBg = false;
			_controller.setVisible(false);
		}

		/// <summary>
		/// Sets the type of the button.设置左上排行拍的类型
		/// </summary>
		/// <param name="tar">Tar.</param>
		/// <param name="typestr">Typestr.</param>
		private void _setBtnType(Button tar,string typestr)
		{
			var typetext = tar.gameObject.GetComponentEx<Text> ("lb_text");
			typetext.text = typestr;
		}


		/// <summary>
		/// Sets the rank infor.  需要填充数据的btn ，  排行的数据 ， 排行榜的类型0 是活跃排行榜  1等级排行榜  2.资产排行榜
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="tmpvo">Tmpvo.</param>
		/// <param name="rankType">Rank type.</param>
		private void _setRankInfor(RankItem target , GameRankVo tmpvo , int rankType)
		{
			target.UpateData (tmpvo, rankType);
		}

		private List<RankItem> rankItemList = new List<RankItem> ();
		private Button rankItem;
		private Button btn_activeRank;
	}

	class RankType
	{
		public static int ActiveType=0;
		public static int LevelType=1;
		public static int balanceType=2;
	}

	public class RankItem
	{
		public RankItem()
		{
			
		}

		public void InitItem(GameObject go)
		{
			gameobj = go.gameObject;

			bgimg = gameobj.GetComponentEx<Image> (Layout.img_itemBg);
			img_itembg = new UIImageDisplay (bgimg);

			var rankIcon = gameobj.GetComponentEx<Image> (Layout.img_itemRank);
			rankImg = new UIImageDisplay (rankIcon);

			var imghead = gameobj.GetComponentEx<Image> (Layout.img_head);
			img_head = new UIImageDisplay (imghead);

			lb_rank = gameobj.GetComponentEx<Text> (Layout.lb_rank);

			lb_name = gameobj.GetComponentEx<Text> (Layout.lb_playerName);
			lb_tip = gameobj.GetComponentEx<Text> (Layout.lb_tip);


		}

		/// <summary>
		/// Upates the data  刷新组件信息.  gamevo数据  ， 所在排行榜的类型排行榜的类型0 是活跃排行榜  1等级排行榜  2.资产排行榜
		/// </summary>
		/// <param name="gameVo">Game vo.</param>
		/// <param name="rankType">Rank type.</param>
		public void UpateData(GameRankVo gameVo , int rankType)
		{
			if (null == gameVo)
			{
				gameobj.SetActiveEx (false);
				return;
			}

			gameobj.SetActiveEx(true);

			var index = gameVo.rankIndex;

			var isNormal = true;
			var tmpbgPath = "";
			var tmpIconPath = "";
			var tipStr = gameVo.rankTip;

			if (index == 1) 
			{
				isNormal = false;
				tmpbgPath=brightbgPath;
				tmpIconPath = rankFirstPath;
			}
			else if(index==2)
			{
				isNormal = false;
				tmpbgPath=brightbgPath;
				tmpIconPath = rankTwoPath;
			}
			else if(index==3)
			{
				isNormal = false;
				tmpbgPath=brightbgPath;
				tmpIconPath = rankThreePath;
			}
			else
			{
				tmpbgPath = normalbgPath;
			}

			if (isNormal == false)
			{
				rankImg.SetActive (true);
				lb_rank.SetActiveEx(false);
				rankImg.Load (tmpIconPath);

				lb_name.color = brightColor;
				lb_tip.color = brightColor;
				bgimg.rectTransform.sizeDelta =new Vector2 (327,62);

			}
			else
			{
				rankImg.SetActive (false);
				lb_rank.SetActiveEx (true);
				lb_rank.text=index.ToString();

				lb_name.color = normalColor;
				lb_tip.color = normalColor;
				bgimg.rectTransform.sizeDelta =new Vector2 (340,67);
			}


			if (rankType == 0)
			{
				tipStr = "游戏次数:" + gameVo.rankTip;
			}
			else if(rankType==1)
			{
				tipStr = "当前等级:"+gameVo.rankTip;
			}
			else if(rankType==2)
			{ 
				tipStr = "当前财富:" + gameVo.rankTip;
			}

			lb_name.text = gameVo.playerName;
			lb_tip.text = tipStr;
			//gameVo.headPath
			img_head.Load ("share/atlas/battle/playerhead/head3.ab");
			img_itembg.Load (tmpbgPath);

		}

		public void Dispose()
		{
			if (null != img_head)
			{
				img_head.Dispose ();
			}

			if (null != img_itembg)
			{
				img_itembg.Dispose ();
			}

			if (null != rankImg)
			{
				rankImg.Dispose ();
			}
		}


		private UIImageDisplay img_itembg;
		private UIImageDisplay img_head;
		private UIImageDisplay rankImg;

		private Image bgimg;

		private Text lb_rank;
		private Text lb_name;
		private Text lb_tip;

		private GameObject gameobj;

		private Color normalColor = new Color (51f/255,51f/255,51f/255,1f);
		private Color brightColor = new Color (255f/255,255f/255,255f/255,1f);

		private string rankFirstPath="share/atlas/battle/gamerank/paihang1.ab";
		private string rankTwoPath="share/atlas/battle/gamerank/paihang2.ab";
		private string rankThreePath="share/atlas/battle/gamerank/paihang3.ab";

		private string normalbgPath="share/atlas/battle/consulting/advisory_function_bg_1.ab";
		private string brightbgPath="share/atlas/battle/gamerank/bg1.ab";
	}


	class Layout
	{
		public static string img_itemBg = "img_itembg";
		public static string lb_rank="lb_rank";
		public static string img_itemRank="img_Icon";
		public static string lb_playerName = "lb_name";
		public static string lb_tip = "lb_tip";
		public static string img_head="headImg";
	}
}