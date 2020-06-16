using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Client.UI
{
	public partial class UIStartGuildWindow
    {
        #region Const
        private const int MaxImageCount = 3;
        #endregion
        #region Componment
        private ScrollRect _scrollRect = null;
        private Transform _parent = null;
		private Image[] _rimgShows;
		private UIImageDisplay[] _rimgDisplays = null;

        private Button _btnJump;

        private ToggleGroup _tgParent;
        private Toggle[] _toggles;
        #endregion
        #region Normal
		private string[] _imagePaths = new string[MaxImageCount]  { "share/texture/uistartguid/indexa.ab", "share/texture/uistartguid/indexb.ab" , "share/texture/uistartguid/indexc.ab" };
        //private string[] _context = { "上一步","下一步", "完成" };
        private int _curIndex = 0;
        private float duration = 0.5f;
        private Vector2 pos;
        private float dragDistance = 200;
        #endregion
        private void _OnInitCenter(GameObject go)
		{
            GameModel.GetInstance.MathWidthOrHeightDierctor(go, 0);

            _scrollRect = go.GetComponentEx<ScrollRect>(Layout.sr_ScrollView);
            _parent = go.GetComponentEx<Transform>(Layout.trans_Parent);

			_selfObj = go;

            if (_parent != null)
            {
				_rimgShows = new Image[MaxImageCount];
				_rimgDisplays = new UIImageDisplay[MaxImageCount];
                for (int i = 0; i < MaxImageCount; i++)
                {
                    if (i == 0)
                    {
						_rimgShows[0] = _parent.gameObject.GetComponentEx<Image>(Layout.rimg_Show);
                    }
                    else
                    {
                        GameObject clone = (GameObject)_rimgShows[0].gameObject.InstantiateEx();
                        clone.transform.SetParent(_parent);
                        clone.transform.localScale = Vector3.one;
                        Vector3 v3 = clone.transform.localPosition;
                        v3.z = 0;
                        clone.transform.localPosition = v3;
						_rimgShows[i] = clone.GetComponent<Image>();
                    }
					_rimgDisplays[i] = new UIImageDisplay(_rimgShows[i]);
                    SetRawImageProTex(_rimgDisplays[i], _imagePaths[i]);
                }

                this.particleCircle = go.GetComponentEx<ParticleSystem>(Layout.particleCircle);
                this.particleXingyun = go.GetComponentEx<ParticleSystem>(Layout.particleXingyun);

            }

            _tgParent = go.GetComponentEx<ToggleGroup>(Layout.tg_Parent);

            _btnJump = go.GetComponentEx<Button>(Layout.btn_Jump);
            EventTriggerListener.Get(_btnJump.gameObject).onClick = _OnJumpHandler;
            UIEventListener.Get(_scrollRect.gameObject).PointerBeginDrag += _OnBeginDragHandler;
            UIEventListener.Get(_scrollRect.gameObject).PointerEndDrag += _OnEndDragHandler;

            if (_tgParent != null)
            {
                _toggles = new Toggle[MaxImageCount];
                for (int i = 0; i < MaxImageCount; i++)
                {
                    if (i == 0)
                    {
                        _toggles[i] = _tgParent.gameObject.GetComponentEx<Toggle>(Layout.toggle_1);
                    }
                    else
                    {
                        if (_toggles[0] == null)
                            break;
                        GameObject clone = (GameObject)_toggles[0].gameObject.InstantiateEx();
                        clone.transform.SetParent(_tgParent.transform);
                        clone.transform.localScale = Vector3.one;
                        Vector3 v3 = clone.transform.localPosition;
                        v3.z = 0;
                        clone.transform.localPosition = v3;
                        _toggles[i] = clone.GetComponent<Toggle>();
                        _toggles[i].isOn = false;
                    }
                    int flag = i;
                    EventTriggerListener.Get(_toggles[i].gameObject).onClick = (GameObject obj)=> { _OnClickHandler(flag); };
                }
            }
            _curIndex = 0;
        }
        private void SetTextContext(Text text, string context)
        {
            if (text != null)
                text.text = context;
        }
		private void SetRawImageProTex(UIImageDisplay rawImage,string path)
        {
            if (rawImage != null)
            {
                rawImage.Load(path);
            }
        }
        private void SetComponentActive<T>(T t,bool enable) where T : Component
        {
            if (t != null)
            {
                t.gameObject.SetActive(enable);
            }
        }
        private void _OnBeginDragHandler(GameObject obj, PointerEventData eventData)
        {
            pos = eventData.position;
        }

        private void _OnEndDragHandler(GameObject obj, PointerEventData eventData)
        {
            //int flag = _curIndex;

            if (eventData.position.x - pos.x > dragDistance)
            {
                _Before();
            }
            else if (eventData.position.x - pos.x < -dragDistance)
            {
                _Next();
            }
            else
            {
                _Move();
            }

        }
        private void _OnClickHandler(int index)
        {
            _curIndex = index;
        }
        private void _OnJumpHandler(GameObject go)
        {
            Audio.AudioManager.Instance.BtnMusic();
            _ClosePanel();
        }
        private void _MoveBack(Vector2 v2)
        {
            if(_scrollRect != null)
                _scrollRect.DONormalizedPos(v2, duration);
        }
        private void _Move()
        {
            if (_curIndex == 0)
            {
                _MoveBack(new Vector2(0, 0));
            }
            else if (_curIndex == MaxImageCount - 1)
            {
                _MoveBack(new Vector2(1f, 0));
            }
            else
            {
                float gridLength = 1f / (MaxImageCount - 1);
                _MoveBack(new Vector2(_curIndex * gridLength, 0));
            }
        }
        private void _Next()
        {
            _curIndex++;
            if (_curIndex >= MaxImageCount)
            {
                _curIndex = MaxImageCount - 1;
                _ClosePanel();
                return;
            }
            _Move();
            _toggles[_curIndex].OnSubmit(null);
        }
        private void _Before()
        {
            _curIndex--;
            if (_curIndex < 0)
            {
                _curIndex = 0;
                return;
            }
            _Move();
            _toggles[_curIndex].OnSubmit(null);
        }

        private void _ClosePanel()
        {
            _controller.setVisible(false);
           
			var control = Client.UIControllerManager.Instance.GetController<Client.UI.UILoginController>();
            control.setVisible(true);

			//2016-9-21 zll fix
			//var controller = Client.UIControllerManager.Instance.GetController<UILoadingWindowController>();
			//controller.setVisible (true);
			//controller.LoadSeletRoleUI();

        }
        private void _OnShowCenter()
		{

            //particleXingyun.Stop();
            //particleCircle.Stop();

            //particleXingyun.GetComponent<Renderer>().material.shader=Shader.Find("Mobile/Particles/Additive");
            //particleCircle.GetComponent<Renderer>().material.shader = Shader.Find("Mobile/Particles/Additive");

            //particleXingyun.Play();
            //particleCircle.Play();


            var scaler =  _selfObj.GetComponent<CanvasScaler> ();
			scaler.referenceResolution = new Vector2 (1334,750);

            Audio.AudioManager.Instance.Play("share/audio/story.ab", true);
            //Audio.AudioManager.Instance.StartMusic();
        }
        
        private void _OnTick(float deltaTime)
        {

        }

		private void _OnHideCenter()
		{
            Audio.AudioManager.Instance.Stop();

        }

        private GameObject _selfObj;


        private ParticleSystem particleXingyun;

        private ParticleSystem particleCircle;


		private void _OnDisposeCenter()
		{
            UIEventListener.Get(_scrollRect.gameObject).PointerEndDrag -= _OnEndDragHandler;
        }
    }
}

