using System;
using Core;
using Core.Web;
using UnityEngine;
using Metadata;

namespace Audio
{
	/// <summary>
    ///  声音控制器
    /// </summary>
	public partial class AudioManager
	{
        /// <summary>
        /// 初始化的时候，获取关于声音的一些设置
        /// </summary>
		protected AudioManager ()
		{
			_localConfig = new LocalConfigManager ();
			_audioObject = new GameObject ("AudioManager");
			_audio = _audioObject.AddComponent<AudioSource> ();
			_audioSound = _audioObject.AddComponent<AudioSource>();

			_audioToggle = System.Convert.ToBoolean(_localConfig.LoadValue ("audioToggle", "true"));
			_volume = _localConfig.LoadValue ("audioVolume", 1.0f);
			_soundVolume = _localConfig.LoadValue ("audioSoundVolume",1.0f);
		}

        /// <summary>
        ///  获取单例
        /// </summary>
		public static AudioManager Instance
		{
			get
			{
				if (null == _instance) 
				{
					_instance = new AudioManager ();
				}

				return _instance;
			}
		}

		public void ToggleAudio(bool value)
		{
			_localConfig.SaveValue ("audioToggle", value.ToString());
			_audioToggle = value;

			if (_audioToggle && null != _audio) 
			{
				_audio.Play ();
			}
		}

        /// <summary>
        ///  设置背景音乐大小
        /// </summary>
        /// <param name="volume"></param>
		private void SetAudioVolume(float volume)
		{
			_volume = volume;
			_localConfig.SaveValue ("audioVolume", volume);

			if (null != _audio) 
			{
				_audio.volume = volume;
			}
		}

        /// <summary>
        ///  设置音效声音大小
        /// </summary>
        /// <param name="volume"></param>
		private void SetSoundVolume(float volume)
		{
			_soundVolume = volume;
			_localConfig.SaveValue ("audioSoundVolume", volume);

			if (null != _audioSound) 
			{
				_audioSound.volume = volume;
			}
		}

		#region 播放音乐
		public void Play(string audioPath ,bool mLoop)
		{
			if (!_audioToggle) 
			{
				return;
			}

			if (null != _audio && _audio.name == audioPath && !_audio.isPlaying) 
			{
				_audio.Play ();
				return;
			}

			if (!string.IsNullOrEmpty (audioPath)) 
			{
				WebManager.Instance.LoadWebItem (audioPath, item => {
					using (item)
					{
						_audio.clip = (AudioClip)item.audioClip;
						_audio.clip.name = audioPath;
						_audio.volume = _volume;
						_audio.loop = mLoop;
						_audio.Play();	

					}
				});	
			}
		}
			
		public void Stop()
		{
			_isTick = false;
			_audio.Stop ();
		}

		public void Pause()
		{
			_isTick = false;
			_audio.Pause ();
		}

		public void Tick(float deltatime)
		{
			if(_isTick==false)
			{
				return;
			}

			if(isDelayed==false)
			{
				delayTime += deltatime;

				if (delayTime <= delayMax)
				{					
					return;
				}

				isDelayed = true;
			}

			if (null != _audio)
			{
				if (_audio.isPlaying == false)
				{
					_isTick = false;
					GameBgMusic ();
				}
			}
		}

		#endregion

		#region 播放音效
		public void PlaySound(string audioPath ,bool mLoop=false)
		{
			if (!_audioToggle) 
			{
				return;
			}

			if (null != _audioSound && _audioSound.name == audioPath && !_audioSound.isPlaying) 
			{
				_audioSound.Play ();
				return;
			}

			if (!string.IsNullOrEmpty (audioPath)) 
			{
				WebManager.Instance.LoadWebItem (audioPath, item => {
					using (item)
					{
						_audioSound.clip = (AudioClip)item.audioClip;
						_audioSound.clip.name = audioPath;
						_audioSound.volume = _soundVolume;
						_audioSound.loop = mLoop;
						_audioSound.Play();	
					}
				});	
			}
		}

		public void StopSound()
		{
			_audioSound.Stop ();
		}


		public void PauseSound()
		{
			_audioSound.Pause ();
		}
        #endregion

        /// <summary>
        /// 按钮音乐
        /// </summary>
        public void BtnMusic()
		{
			PlaySound(subTitleManager.musicData.clickBtn,false);
		}

		///<summary>
		/// 开始音乐
		/// </summary>
		public void StartMusic() 
		{
			_isTick = false;
			Play(subTitleManager.musicData.startBgSound,true);
		}



		///<summary>
		/// 登录音乐
		/// </summary>
		public void LoginMusic() 
		{
			_isTick = false;
			Play(subTitleManager.musicData.login,true);
		}

		///<summary>
		/// 骰子音效
		/// </summary>
		public void DiceMusic() 
		{
			PlaySound(subTitleManager.musicData.throwCraps,false);
		}

		///<summary>
		/// 游戏背景音乐
		/// </summary>
		public void GameBgMusic() 
		{
			//ytf1010 更换成原来的背景音乐
//			Play(subTitleManager.musicData.backGroundAndio,true);
//			return;

			int random = UnityEngine.Random.Range(0,100);	

			var tmpPath = "";
			var tmpAudioIndex = 0;

			if (random >= 75)
			{
				tmpPath=subTitleManager.musicData.bgNewAdd1;
				tmpAudioIndex = 1;
			}
			else if(random>=50)
			{
				tmpPath=subTitleManager.musicData.bgNewAdd2;
				tmpAudioIndex = 2;
			}
			else if(random>=25)
			{
				tmpPath=subTitleManager.musicData.bgNewAdd3;
				tmpAudioIndex = 3;
			}
			else 
			{
				tmpPath=subTitleManager.musicData.backGroundAndio;
				tmpAudioIndex =4;
			}

			if(_tmpGameAudio==tmpAudioIndex)
			{
				GameBgMusic ();
				return;
			}
			else
			{
				_tmpGameAudio = tmpAudioIndex;
			}

			Play (tmpPath,false);
			_isTick = true;
			delayTime = 0;
			isDelayed = false;
		}

		///<summary>
		/// 移动音效，暂时屏蔽
		/// </summary>
		public void MoveMusic() 
		{
			PlaySound(subTitleManager.musicData.move,true);
		}

		///<summary>
		/// 胜利音效
		/// </summary>
		public void VictoryMusic() 
		{
			PlaySound(subTitleManager.musicData.gameWin,false);
		}

		///<summary>
		/// 失败音效
		/// </summary>
		public void FailureMusic() 
		{
			PlaySound(subTitleManager.musicData.gameLose,false);
		}

		///<summary>
		/// 选角色音乐
		/// </summary>
		public void SelectedRoleMusic() 
		{
			PlaySound(subTitleManager.musicData.slectRole,false);
		}

        /// <summary>
        ///  开启音效
        /// </summary>
		public void OpenSound()
		{
			SetSoundVolume (1);
		}

        /// <summary>
        ///  关闭音效
        /// </summary>
		public void CloseSound()
		{
			SetSoundVolume (0);
		}

        /// <summary>
        ///  开启音乐
        /// </summary>
		public void OpenMusic()
		{
			SetAudioVolume (1f);
		}

        /// <summary>
        ///  关闭音乐
        /// </summary>
		public void CloseMusic()
		{
			SetAudioVolume (0);
		}

        /// <summary>
        /// 当前音效是否是静音
        /// </summary>
		public bool IsOpenSound
		{
			get
			{
				return (_soundVolume > 0);
			}
		}

        /// <summary>
        /// 当前音乐是否是静音
        /// </summary>
		public bool IsOpenMusic
		{
			get
			{
				return (_volume > 0);
			}
		}

		/// <summary>
		/// The is tick. 是否是tick 检测
		/// </summary>
		private bool _isTick=false;
		private float delayTime = 0f;
		private float delayMax=6f;
		private bool isDelayed=false;

		private int _tmpGameAudio=0;

		private float _volume;
		private float _soundVolume;

		private bool _audioToggle;

		private AudioSource _audio;
		private AudioSource _audioSound;

		private GameObject _audioObject;
		private LocalConfigManager _localConfig;
		private static AudioManager _instance;
		private readonly SubTitleManager subTitleManager = SubTitleManager.Instance;
	}
}

