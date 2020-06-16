using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Web;
using Client.UnitFSM;
using Client.Scenes;
using Core;
using Metadata;
using Client;
using Client.UI;

/// <summary>
/// 玩家掷筛子走到目标后显示卡牌小图标
/// </summary>
public class Particle : MonoBehaviour
{

	public enum StatusParticle
	{
		closing_date, //结账日
		inner_ring_fate, // 内圈命运
		inner_ring_free_time, //内圈有钱有闲
		inner_ring_investment, //内圈投资
		inner_ring_quality_life,//内圈品质生活
		outer_ring_fate,//外圈命运
		outer_ring_great_opportunity,//外圈大机会
		outer_ring_little_chance,//外圈小机会
		outer_ring_risk,//外圈风险
		specialcharitycard,//慈善事业
		specialchild,	//生孩子
		specialhealth,	//健康管理
		specialstudy  	//进修学习
	}

	public static Particle smInstance;
	public static Particle Instance
	{
		get{return smInstance;}
	}

	void Awake()
	{
		smInstance = this;
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
//		if(Input.GetKey(KeyCode.A))
//		{
//			AddHeadParticle(gameObject.transform, StatusParticle.closing_date);
//		}
	}

	/// <summary>
	/// 添加卡牌粒子效果
	/// </summary>
	public void AddCardParticle()
	{
//		_modelPath = "prefabs/particle/fxcomichit1largezap.ab";
//		var web = WebManager.Instance.LoadWebPrefab (_modelPath, prefab => {
//			using(prefab)
//			{
//				_root = prefab.mainAsset.CloneEx() as GameObject;
//			}
//		});
	}

	/// <summary>
	/// 销毁卡牌粒子效果
	/// </summary>
	public void DestroyCardParticle()
	{
		_root.DestroyEx();
		_root = null;
	}

	/**
	public void ParticlePath(StatusParticle status)
	{
		
		if(status == StatusParticle.closing_date)
		{
			_modelPath = "prefabs/particle/closing_date.ab";
		}
		else if(status == StatusParticle.inner_ring_fate)
		{
			_modelPath = "prefabs/particle/inner_ring_fate.ab";
		}
		else if(status == StatusParticle.inner_ring_free_time)
		{
			_modelPath = "prefabs/particle/inner_ring_free_time.ab";
		}
		else if(status == StatusParticle.inner_ring_investment)
		{
			_modelPath = "prefabs/particle/inner_ring_investment.ab";
		}
		else if(status == StatusParticle.inner_ring_quality_life)
		{
			_modelPath = "prefabs/particle/inner_ring_quality_life.ab";
		}
		else if(status == StatusParticle.outer_ring_fate)
		{
			_modelPath = "prefabs/particle/outer_ring_fate.ab";
		}
		else if(status == StatusParticle.outer_ring_great_opportunity)
		{
			_modelPath = "prefabs/particle/outer_ring_great_opportunity.ab";
		}
		else if(status == StatusParticle.outer_ring_little_chance)
		{
			_modelPath = "prefabs/particle/outer_ring_little_chance.ab";
		}
		else if(status == StatusParticle.outer_ring_risk)
		{
			_modelPath = "prefabs/particle/outer_ring_risk.ab";
		}
		else if(status == StatusParticle.specialcharitycard)
		{
			_modelPath = "prefabs/particle/specialcharitycard.ab";
		}
		else if(status == StatusParticle.specialchild)
		{
			_modelPath = "prefabs/particle/specialchild.ab";
		}
		else if(status == StatusParticle.specialhealth)
		{
			_modelPath = "prefabs/particle/specialhealth.ab";
		}

	}
	**/

	public void ParticlePath(StatusParticle status)
	{
		
		if(status == StatusParticle.closing_date)
		{
			_modelPath = "prefabs/particle/closing_date.ab";
		}
		else if(status == StatusParticle.inner_ring_fate)
		{
			_modelPath = "prefabs/particle/inner_ring_fate.ab";
		}
		else if(status == StatusParticle.inner_ring_free_time)
		{
			_modelPath = "prefabs/particle/inner_ring_free_time.ab";
		}
		else if(status == StatusParticle.inner_ring_investment)
		{
			_modelPath = "prefabs/particle/inner_ring_investment.ab";
		}
		else if(status == StatusParticle.inner_ring_quality_life)
		{
			_modelPath = "prefabs/particle/inner_ring_quality_life.ab";
		}
		else if(status == StatusParticle.outer_ring_fate)
		{
			_modelPath = "prefabs/particle/outer_ring_fate.ab";
		}
		else if(status == StatusParticle.outer_ring_great_opportunity)
		{
			_modelPath = "prefabs/particle/outer_ring_great_opportunity.ab";
		}
		else if(status == StatusParticle.outer_ring_little_chance)
		{
			_modelPath = "prefabs/particle/outer_ring_little_chance.ab";
		}
		else if(status == StatusParticle.outer_ring_risk)
		{
			_modelPath = "prefabs/particle/outer_ring_risk.ab";
		}
		else if(status == StatusParticle.specialcharitycard)
		{
			_modelPath = "prefabs/particle/specialcharitycard.ab";
		}
		else if(status == StatusParticle.specialchild)
		{
			_modelPath = "prefabs/particle/specialchild.ab";
		}
		else if(status == StatusParticle.specialhealth)
		{
			_modelPath = "prefabs/particle/specialhealth.ab";
		}

	}

	/// <summary>
	/// 添加粒子效果
	/// </summary>
	public void AddHeadParticle(Transform tra,StatusParticle status)
	{
		ParticlePath(status);

		var web = WebManager.Instance.LoadWebPrefab (_modelPath, prefab => {
			using(prefab)
			{
				_root = prefab.mainAsset.CloneEx() as GameObject;

				Vector3 locaPos = tra.position;
				locaPos.x = tra.position.x;
				locaPos.y = tra.position.y + 2.5f;
				locaPos.z = tra.position.z;
				_root.transform.position = locaPos;
			}
		});
			
	}

	/// <summary>
	/// 销毁粒子效果
	/// </summary>
	public void DestroyHeadParticle()
	{
		if (null != _root)
		{
			_root.DestroyEx();
			_root = null;
		}
	}

	private string _modelPath;
	private GameObject _root;
	private WebItem _webItem;


}

