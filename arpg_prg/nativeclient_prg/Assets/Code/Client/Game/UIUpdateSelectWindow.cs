using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace Client 
{
#if UNITY_EDITOR
	public partial class UIUpdateSelectWindow : MonoBehaviour
	{
		private void Awake()
		{
			_instance = this;
		}

		public static void Toggle()
		{
			if(Instance.gameObject.activeSelf)
			{
				Instance._Hide();
			}
			else
			{
				Instance._Show();
			}
		}
		
		private void _Hide()
		{
			gameObject.SetActive(false);
			Dispose ();
		}
		
		private void _Show()
		{
			gameObject.SetActive(true);
		}

        public void Dispose()
        {
            GameObject.Destroy(gameObject);
        }
		
        private static UIUpdateSelectWindow _instance;
		public static UIUpdateSelectWindow Instance 
        {
            get 
            {
                if (null == _instance) 
                {
                    _instance = new GameObject("UIUpdateSelect").AddComponent<UIUpdateSelectWindow>();
                    _instance.gameObject.SetActive(false);
                }
                return _instance;
            }
        }
		
		public static bool FirstOpen;
	}
#endif
}
