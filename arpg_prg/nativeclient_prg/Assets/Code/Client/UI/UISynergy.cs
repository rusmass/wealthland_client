using UnityEngine;
using System.Collections;
using Client.Scenes;

namespace Client.UI
{
	public class UISynergy : MonoBehaviour
	{
		public static UISynergy smInstance;

		public static UISynergy Instance
		{
			get{return smInstance;}
		}

		void Awake()
		{
			smInstance = this;
		}

		private AsyncOperation async;

		// Use this for initialization
		void Start ()
		{

		}
		
		// Update is called once per frame
		void Update ()
		{

		}

		public void loadSelectScene()
		{
			StartCoroutine(loadSelectRoleScene());  
		}

		/// <summary>
		/// Loads the game hall scene. 加载游戏大厅
		/// </summary>
		public void loadGameHallScene()
		{
			StartCoroutine (loadGameHall ());
		}

		private IEnumerator loadSelectRoleScene()
		{
			yield return new WaitForSeconds(0.5f);
			Game.Instance.SwitchUISelectRoleState();
		}

		private IEnumerator loadGameHall()
		{
			yield return new WaitForSeconds (0.5f);
			Game.Instance.SwithUIGameHall ();
		}



		public void loadBattleScene()
		{
			StartCoroutine(loadBattleSceneUI());  
		}

		private IEnumerator loadBattleSceneUI()
		{
			yield return new WaitForSeconds(0.5f);

			var controller = Client.UIControllerManager.Instance.GetController<UIChooseRoleWindowController>();
			controller.setSelectedImage();
		}

		public void loadNetGameScene()
		{
			StartCoroutine(loadNetGameSceneUI());  
		}

		private IEnumerator loadNetGameSceneUI()
		{
			yield return new WaitForSeconds(0.5f);
			Game.Instance.SwitchNetGame ();
//			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UIBattleController>();
//			control.setVisible(true);
//			VirtualServer.Instance.Handle_RequestBuildRoom(133);
//			SceneManager.Instance.Send_RequestEnterScene(0, null);
//			Console.WriteLine ("游戏开始是了");
		}

        /// <summary>
        /// 隐藏恭喜胜利界面，进入结束界面
        /// </summary>
        public void ChanceToGameOver()
        {
            StartCoroutine(AddGameOver());
        }

        private IEnumerator AddGameOver()
        {
            yield return new WaitForSeconds(9f);

            var entersuccess = Client.UIControllerManager.Instance.GetController<UIEnterSuccessWindowController>();
            if (null != entersuccess && entersuccess.getVisible())
            {
                entersuccess.setVisible(false);
            }

            var overHandler = Client.UIControllerManager.Instance.GetController<UIGameOverWindowController>();
            overHandler.setVisible(true);            
        }
	}
}

