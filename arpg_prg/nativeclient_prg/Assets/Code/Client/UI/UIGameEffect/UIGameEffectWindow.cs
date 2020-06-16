using System;
using UnityEngine;
using DG.Tweening;
using Core.Web;

namespace Client.UI
{
	public class UIGameEffectWindow:UIWindow<UIGameEffectWindow,UIGameEffectController>
	{
		public UIGameEffectWindow ()
		{
		}

		protected override void _Init (GameObject go)
		{
			_gameObj = go;
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

        /// <summary>
        /// 添加金币的特效
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="initPostion"></param>
		public void AddMoneyEffect(int playerIndex,Vector3 initPostion)
		{
			
			var _initPosition=new Vector3(initPostion.x,initPostion.y,initPostion.z);
			var tmpPath = "prefabs/ui/scene/moneyperfab.ab";

			Vector3 _targetPosition;

			switch (playerIndex)
			{
			case 0:
				_targetPosition = player1Position;
				break;
			case 1:
				_targetPosition = player2Position;
				break;
			case 2:
				_targetPosition = player3Position;
				break;
			case 3:
				_targetPosition = player4Position;
				break;
			default:
				_targetPosition = Vector3.zero;
				break;

			}

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
						var tmpx=UnityEngine.Random.Range(-40f,40f);
						var tmpy=UnityEngine.Random.Range(-40f,40f);
						transform.localPosition=new Vector3(_initPosition.x+tmpx,_initPosition.y + tmpy,_initPosition.z);

						var suqence=DOTween.Sequence();

						var tmpDuration=UnityEngine.Random.Range(0.2f,1f);

						suqence.Append(transform.DOLocalMove(_targetPosition,tmpDuration));
						suqence.AppendCallback(()=>{
							GameObject.Destroy(transform.gameObject);
						});
						suqence.SetDelay(1f);
						suqence.SetAutoKill(true);
					}
					GameObject.Destroy(_obj);
					_obj=null;
				}
			});
		}

		private GameObject _gameObj;

		private Vector3 player1Position=new Vector3(-330,-160,0);
		private Vector3 player2Position=new Vector3(-330,256,0);
		private Vector3 player3Position=new Vector3(346,256,0);
		private Vector3 player4Position=new Vector3(346,-165,0);
	}
}

