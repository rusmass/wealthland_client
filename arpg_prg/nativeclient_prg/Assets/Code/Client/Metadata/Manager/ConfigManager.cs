using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using Core;
using Core.Web;
using Core.IO;

namespace Metadata
{
    /// <summary>
    ///  管理数据单列，单一数据类型的表
    /// </summary>
    public partial class ConfigManager : IDisposable
    {
        protected ConfigManager()
        {

        }

        public void LoadConfigs()
        {
            _LoadConfigs();
        }

        public T GetConfig<T>() where T : Config
        {
            Config config;
            if (!_mConfigs.TryGetValue(typeof(T).Name, out config))
            {
                Console.Error.WriteLine("No has {0}!", typeof(T).Name);
            }

            return config as T;
        }

        private void _AddLoaderConfig<T>(string path = null) where T : Config, new()
        {
            path = path ?? "metadata/" + typeof(T).Name.ToLower() + ".gd";
            _LoadMetadataConfig<T>(path);
        }

        private void _LoadMetadataConfig<T>(string path) where T : Config, new()
        {
            var argument = new WebArgument()
            {
                localPath = path,
                flags = WebFlags.NewWWW
            };

            WebManager.Instance.LoadWebItem(argument, item => {
                using (MemoryStream ms = new MemoryStream(item.bytes))
                using (OctetsReader br = new OctetsReader(ms))
                {
                    var config = new T();
                    config.Load(br);
                    _mConfigs.Add(typeof(T).Name, config);
                }
            });
        }

        public void Dispose()
        {
            if (null != _mConfigs)
            {
                _mConfigs.Clear();
            }
        }

        partial void _LoadConfigs();

        private Dictionary<string, Config> _mConfigs = new Dictionary<string, Config>();

        public static readonly ConfigManager Instance = new ConfigManager();
    }
}