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
    /// 管理多个列，多个数据类型的表
    /// </summary>
	public partial class TemplateManager: IDisposable
    {
        protected TemplateManager()
        {

        }

        public void LoadTemplates()
        {
            _LoadTemplates();
        }

        public T GetTemplate<T>(int id) where T : Template
        {
            TemplateTable table = GetTemplateTable<T>();
            Template template;
            if (!table.TryGetValue(id, out template))
            {
                Console.Error.WriteLine("No has id = {0} in {1} Template.", id, typeof(T).Name);
            }

            return template as T;
        }

        public TemplateTable GetTemplateTable<T>() where T : Template
        {
            TemplateTable table;
            var type = typeof(T).Name;
            if (!_mTemplateTables.TryGetValue(type, out table))
            {
                Console.Error.WriteLine("No has {0} template!", type);
            }

            return table;
        }

        private void _AddLoaderTemplate<T>(string path = null) where T : Template, new()
        {
            path = path ?? "metadata/" + typeof(T).Name.ToLower() + ".gd";
            _LoadMetadataTemplate<T>(path);
        }

        private void _LoadMetadataTemplate<T>(string path) where T : Template, new()
        {
            TemplateTable table = new TemplateTable();
            _mTemplateTables.Add(typeof(T).Name, table);

            var argument = new WebArgument()
            {
                localPath = path,
                flags = WebFlags.NewWWW
            };

            WebManager.Instance.LoadWebItem(argument, item => {
                using (MemoryStream ms = new MemoryStream(item.bytes))
                using (OctetsReader br = new OctetsReader(ms))
                {
                    var count = br.ReadInt32();
                    for (int i = 0; i < count; ++i)
                    {
                        var template = new T();
                        template.Load(br);
                        table.Add(template.id, template);
                    }
                }
            });
        }

        public void Dispose()
        {
            if (null != _mTemplateTables)
            {
                _mTemplateTables.Clear();
            }
        }

        partial void _LoadTemplates();

        private Dictionary<string, TemplateTable> _mTemplateTables = new Dictionary<string, TemplateTable>();

        public static readonly TemplateManager Instance = new TemplateManager();
    }
}