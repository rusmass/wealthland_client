using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
	public class ItemDisplay : IDisposable
    {
        public ItemDisplay(GameObject go)
        {
            _text = go.GetComponentEx<Text>("Text");
        }

        public void Refresh(string textValue)
        {
            _text.SetTextEx(textValue);
        }

		private void _btnClick(GameObject go)
		{
			//_selected = go.transform;
		}

		public void Dispose()
		{
			
		}

        private Text _text; 

		//private static Transform _selected;
    }
}
