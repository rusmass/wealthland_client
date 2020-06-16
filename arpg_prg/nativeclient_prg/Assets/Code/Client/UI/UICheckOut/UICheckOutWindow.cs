using System;
using UnityEngine;
using Core.Web;
using DG.Tweening;

namespace Client.UI
{
	public partial class UICheckOutWindow:UIWindow<UICheckOutWindow,UICheckOutController>
	{
		protected override void _Init (GameObject go)
		{
			_InitTop (go);
			_InitCenter (go);
			_InitBottom (go);
			_gameObj = go;
		}

		protected override void _OnShow ()
		{
			_OnShowTop ();
			_OnShowCenter ();
			_OnShowBottom ();
//			AddMoneyEffect ();
		}

		protected override void _OnHide ()
		{
			_OnHideTop ();
			_OnHideCenter ();
			_OnHideBottom ();
		}

		protected override void _Dispose ()
		{
			_OnDisposeCenter ();
		}


		public void AddMoneyEffect()
		{
			Console.WriteLine ("wuli金币显示啊");
			var tmpPath = "prefabs/ui/scene/moneyperfab.ab";
			var pfb = WebManager.Instance.LoadWebPrefab (tmpPath,perfab=>{
				using(perfab)
				{
					var _obj=perfab.mainAsset.CloneEx() as GameObject;			

					var tmpNum=UnityEngine.Random.Range(5,8);

					for(var i=0;i<tmpNum;i++)
					{
						var transform=_obj.CloneEx().transform;
						transform.SetParent(_gameObj.transform);
						transform.localScale=Vector3.one;
						var tmpx=UnityEngine.Random.Range(-50f,50f);
						var tmpy=UnityEngine.Random.Range(-50f,50f);
						transform.localPosition=new Vector3(tmpx,tmpy,0);

						var suqence=DOTween.Sequence();
						suqence.Append(transform.DOLocalMove(new Vector3(-400,200,0),UnityEngine.Random.Range(0.2f,0.9f)));
						suqence.AppendCallback(()=>{
							GameObject.Destroy(transform.gameObject);
						});
						suqence.SetDelay(2f);
						suqence.SetAutoKill(true);
					}
					GameObject.Destroy(_obj);
					_obj=null;


				}
			});

//			var web = WebManager.Instance.LoadWebPrefab (tmpPath, prefab => {
//				using(prefab)
//				{
//					_root = prefab.mainAsset.CloneEx() as GameObject;
//					_root.name = PlayerID.ToString();
//
//					_animator = _root.GetComponentInChildren<Animator>();
//					_animator.enabled = false;
//
//					_InitPlayer();				
//
//					_playerMat = _root.GetComponentEx<SkinnedMeshRenderer>("clothes").material;
//					var tmpcolor =  _playerMat.GetColor ("_Emission");
//					_originColor = new Color(tmpcolor.r,tmpcolor.g,tmpcolor.g);
//				}
//			});
		}

		private GameObject _gameObj;


	}
}

