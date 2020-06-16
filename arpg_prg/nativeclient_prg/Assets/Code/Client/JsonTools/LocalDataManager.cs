using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Client
{
    public class LocalDataManager
    {
        private static LocalDataManager m_instance;
        public static LocalDataManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new LocalDataManager();
                return m_instance;
            }
        }

		private void LoacalDataManager()
		{			
			_localConfig = new LocalConfigManager ();
//			Debug ("ssssssssssssskkkkkkkkkkkk");
		}

        /// <summary>
        /// 自己单独处理读取数据接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T ReadFile<T>(string fileName)
        {
            return JsonHelper.DeserializeFile<T>(fileName);
        }
        /// <summary>
        /// 自己单独处理保存数据接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fileName"></param>
        public void WriteFile<T>(T t, string fileName)
        {			
            JsonHelper.SerializeFile(fileName, t);
        }

        /// <summary>
        /// 此处统一处理存储数据，把要存储的数据都放进去
        /// </summary>
        public void SaveData()
        {
            WriteFile(PlayerManager.Instance.Players, FileNames.Players); 
        }
        /// <summary>
        /// 此处统一处理读取数据，把要读取的数据都赋值
        /// </summary>
        public void ReadData()
        {
            PlayerInfo[] playerInfos = ReadFile<PlayerInfo[]>(FileNames.Players);
            //PlayerManager.Instance.Players不允许set，得换个方式
        }

		public bool IsNormalEnded
		{
			get
			{
				if (null == _localConfig) 
				{
					_localConfig = new LocalConfigManager ();
				}
				_isNormalEnd = System.Convert.ToBoolean(_localConfig.LoadValue ("_isNormalEnd", "true"));
				return _isNormalEnd; 
			}

			set
			{
				if (null == _localConfig) 
				{
					_localConfig = new LocalConfigManager ();
				}
				_isNormalEnd = value;
				_localConfig.SaveValue ("_isNormalEnd",_isNormalEnd.ToString());
			}
		}

		private bool _isNormalEnd;
		private LocalConfigManager _localConfig;
    }

    public static class FileNames
    {
        public readonly static string Players = "Players";
		public readonly static string VirtualPlayer = "ServivePlayer";
        public readonly static string Test = "Test";
    }
}

