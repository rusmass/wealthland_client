using System;
using UnityEngine;
using UnityEngine.UI;
using Metadata;
using DG.Tweening;
namespace Client.UI
{
	public partial class UISelectRoleWindow
	{
		private void _OnInitCenter(GameObject go)
		{
			var herimg = go.GetComponentEx<Image> (Layout.img_role);
			_heroImg = new UIImageDisplay (herimg);

			var roundImg = go.GetComponentEx<Image> (Layout.img_rotate);
			_rotateImg = new UIImageDisplay (roundImg);

			animtor_rotate = go.DeepFindEx (Layout.anim_rotation);

			_imgHeroInfor = go.GetComponentEx<Image> (Layout.img_infor);
			_lbName = go.GetComponentEx<Text> (Layout.lb_name);
			_lbAge = go.GetComponentEx<Text> (Layout.lb_age);
			_lbCareer = go.GetComponentEx<Text> (Layout.lb_career);
			_lbCashflow = go.GetComponentEx<Text> (Layout.lb_cashFlow);
			_lbPayment = go.GetComponentEx<Text> (Layout.lb_payment);
			_lbIncome = go.GetComponentEx<Text> (Layout.lb_income);
			_lbInfor = go.GetComponentEx<Text> (Layout.lb_infor);
			_lbInfor.SetActiveEx (false);
		}

		private void _OnShowCenter()
		{
			_heroImg.SetActive (false);
			_HideAnimaitorRotation ();
			_imgHeroInfor.SetActiveEx (false);
		}

		public void _OnShowHeroInfor(PlayerInitData value)
		{
			_imgHeroInfor.SetActiveEx (true);

			var tf = _imgHeroInfor.transform;

			var tmpPosition = tf.localPosition;

			var size = _imgHeroInfor.rectTransform.sizeDelta;
			_imgHeroInfor.transform.localPosition = new Vector3 (tmpPosition.x+size.x,tmpPosition.y,tmpPosition.z);

			_lbName.text =string.Format("玩家名称: {0}",value.playName);
			_lbAge.text =string.Format("年龄: {0}",value.initAge.ToString());
			_lbCareer.text = string.Format ("职业: {0}", value.careers);
			_lbCashflow.text =string.Format("总收入: {0}" ,value.cashFlow.ToString ());
			var totalPay=value.cardPay + value.carPay + value.educationPay + value.housePay + value.nessPay + value.additionalPay + value.taxPay;;

			_lbPayment.text =string.Format("总支出: {0}" ,totalPay.ToString());
			var income = value.cashFlow - totalPay;
			_lbIncome.text =string.Format("流动现金: {0}",income.ToString());

//			_lbInfor.text =string.Format("个人简介: {0}" ,value.infor);

			var squence = DOTween.Sequence ();
			squence.Append (tf.DOLocalMoveX(tf.localPosition.x,0.5f));
			squence.Append (tf.DOLocalMoveX(tmpPosition.x,0.5f));

		}

		private void _ShowAnimatorRotation()
		{
			animtor_rotate.SetActiveEx (true);
		}

		private void _HideAnimaitorRotation()
		{
			animtor_rotate.SetActiveEx (false);
		}

		private void _OnHideCenter()
		{
			
		}

		private void _OnLoadHeroImg(string str)
		{
			if ("" != str)
			{
				_heroImg.Load (str);
			}

			_heroImg.SetActive (true);
		}

		private void _OnLoadRoteImg (string str)
		{
			if ("" != str)
			{
				_rotateImg.Load (str);
			}
			_rotateImg.SetActive (true);
		}



		private void _OnDisposeCenter()
		{
			if (null != _heroImg)
			{
				_heroImg.Dispose ();
			}
			if(null != _rotateImg)
			{
				_rotateImg.Dispose ();
			}
		}



		private UIImageDisplay _heroImg;
		private UIImageDisplay _rotateImg;
		private Transform animtor_rotate;

		// 默认顺序  医生 司机 销售经理  律师 it技术员 保安
//		private string[] _imgArrs=new string[]{
//			"select_role_turntable_1",
//			"select_role_turntable_3",
//			"select_role_turntable_5",
//			"select_role_turntable_7",
//			"select_role_turntable_9",
//			"select_role_turntable_11"
//		};

		private Image _imgHeroInfor;
		private Text _lbName;
		private Text _lbAge;
		private Text _lbCareer;
		private Text _lbCashflow;
		private Text _lbPayment;
		private Text _lbIncome;
		private Text _lbInfor;

		private string[] _imgArrs=new string[]{
			"select_role_turntable_1",
			"select_role_turntable_7",
			"select_role_turntable_5",
			"select_role_turntable_9",
			"select_role_turntable_3",
			"select_role_turntable_11"
		};
	
		private string loadImgPath="share/atlas/animator/zhuanpan/idle/{0}.ab";

	}
}

