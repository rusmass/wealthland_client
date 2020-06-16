using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Core.Web;
using Core.IO;

namespace Metadata
{
    /// <summary>
    ///  管理导出表的数据
    /// </summary>
	public class MetadataManager : IDisposable
	{
        protected MetadataManager()
        {

        }

        public T GetTemplate<T>(int id) where T : Template
        {
            return _templateManager.GetTemplate<T>(id);
        }

        public TemplateTable GetTemplateTable<T>() where T : Template
        {
            return _templateManager.GetTemplateTable<T>();
        }

        public T GetConfig<T>() where T : Config
        {
            return _configManager.GetConfig<T>();
        }

        public void LoadMetadata()
        {
            _configManager.LoadConfigs();
            _templateManager.LoadTemplates();
        }

        public void Dispose()
        {
            _configManager.Dispose();
            _templateManager.Dispose();
        }

        public static readonly MetadataManager Instance = new MetadataManager();

        private static readonly ConfigManager _configManager = ConfigManager.Instance;
        private static readonly TemplateManager _templateManager = TemplateManager.Instance;
    }
}

