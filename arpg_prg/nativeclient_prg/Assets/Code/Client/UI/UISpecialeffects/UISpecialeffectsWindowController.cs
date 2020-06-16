using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Metadata;
using System.Collections.Generic;

namespace Client.UI
{
    /// <summary>
    /// 显示卡牌类型的特效
    /// </summary>
	public class UISpecialeffectsWindowController :UIController<UISpecialeffectsWindow,UISpecialeffectsWindowController> 
	{
		protected override string _windowResource
		{
			get{
				return "prefabs/ui/scene/uispecialeffects.ab";
			}
		}

		protected override void _OnLoad()
		{

		}

		protected override void _OnShow()
		{

		}

		protected override void _OnHide()
		{

		}

		protected override void _Dispose ()
		{

		}

        /// <summary>
        /// 图片路径
        /// </summary>
        /// <param name="status"></param>
		private void ImagePath(Particle.StatusParticle status)
		{
			var window = _window as UISpecialeffectsWindow;
			if(status == Particle.StatusParticle.closing_date)
			{
				window.imagePath = "share/atlas/battle/headtitle/closing_date.ab";
			}
			else if(status == Particle.StatusParticle.inner_ring_fate)
			{
				window.imagePath = "share/atlas/battle/headtitle/fate.ab";
			}
			else if(status == Particle.StatusParticle.inner_ring_free_time)
			{
				window.imagePath = "share/atlas/battle/headtitle/rich_time.ab";
			}
			else if(status == Particle.StatusParticle.inner_ring_investment)
			{
				window.imagePath = "share/atlas/battle/headtitle/investment.ab";
			}
			else if(status == Particle.StatusParticle.inner_ring_quality_life)
			{
				window.imagePath = "share/atlas/battle/headtitle/improve_the_quality_of_life.ab";
			}
			else if(status == Particle.StatusParticle.outer_ring_fate)
			{
				window.imagePath = "share/atlas/battle/headtitle/fate.ab";
			}
			else if(status == Particle.StatusParticle.outer_ring_great_opportunity)
			{
				window.imagePath = "share/atlas/battle/headtitle/opportunity.ab";
			}
			else if(status == Particle.StatusParticle.outer_ring_little_chance)
			{
				window.imagePath = "share/atlas/battle/headtitle/opportunity.ab";
			}
			else if(status == Particle.StatusParticle.outer_ring_risk)
			{
				window.imagePath = "share/atlas/battle/headtitle/risk.ab";
			}
			else if(status == Particle.StatusParticle.specialcharitycard)
			{
				window.imagePath = "share/atlas/battle/headtitle/philanthropic_undertaking.ab";
			}
			else if(status == Particle.StatusParticle.specialchild)
			{
				window.imagePath = "share/atlas/battle/headtitle/give_birth_to_a_child.ab";
			}
			else if(status == Particle.StatusParticle.specialhealth)
			{
				window.imagePath = "share/atlas/battle/headtitle/health_management.ab";
			}
			else if(status == Particle.StatusParticle.specialstudy)
			{
				window.imagePath = "share/atlas/battle/headtitle/further_study.ab";
			}
		}

        /// <summary>
        /// 设置标题的信息
        /// </summary>
        /// <param name="tra"></param>
        /// <param name="status"></param>
		public void setHeadImage(Transform tra,Particle.StatusParticle status)
		{
			ImagePath(status);

			var window = _window as UISpecialeffectsWindow;

			if(null != window)
			{
				window.SetImagePos(tra);
				mUpdateImage = true;
			}
		}

        /// <summary>
        /// 释放资源
        /// </summary>
		public void setHeadImageDisappear()
		{

			var window = _window as UISpecialeffectsWindow;

			if(null != window && this.getVisible())
			{
				window.setHeadImageDisappear();
				mUpdateImage = false;
			}
		}

        /// <summary>
        /// 设置头顶飘字
        /// </summary>
        /// <param name="tra"></param>
        /// <param name="strNum"></param>
		public void SetHeadText(Transform tra,float strNum)
		{
			var window = _window as UISpecialeffectsWindow;

			if(null != window && this.getVisible())
			{
				window.SetTextPos(tra,strNum);
				mUpdateText = true;
			}
		}

        /// <summary>
        /// 未引用
        /// </summary>
		public void SetHeadTextDisappear()
		{
			var window = _window as UISpecialeffectsWindow;

			if(null != window && this.getVisible())
			{
				window.SetUpHeadTextDisappear();
				mUpdateText = false;
			}
		}

		public override void Tick (float deltaTime)
		{
			var window = _window as UISpecialeffectsWindow;
			if (null != window && getVisible ())
			{
				if(mStart == true)
				{
					mTimer += deltaTime;

					if(mTimer >= 0.5f)
					{
						window.SetUpHeadTextDisappear();
						mStart = false;
						mTimer = 0;
						mUpdateText = false;
					}
				}

				if(mUpdateImage == true)
				{
					window.updateImagePos(Room.Instance.getCurrentPlay().getPlayPos());
				}

				if(mUpdateText == true)
				{
					window.updateTextPos(Room.Instance.getCurrentPlay().getPlayPos());
				}
			}
		}

		public void ReInitConttoller()
		{
			mStart = false;
			mUpdateImage = false;
			mUpdateText = false;
		}

		private float mTimer = 0.0f;


		public bool mStart = false;
		public bool mUpdateImage = false;
		public bool mUpdateText = false;
	}
}
