using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Core.AutoCode;
using Core;

namespace Metadata
{
    public class MetaManager
    {
        public MetaManager ()
        {
            _metaTypes = _GetAllMetaTypes();

            foreach (var type in _metaTypes)
            {
                if (type.IsTemplate)
                {
                    _templateTypes.Add(type);
                } 
                else if (type.IsConfig)
                {
                    _configTypes.Add(type);
                }
            }

			EditorMetaCommon.Init(this);
        }

        private MetaType[] _GetAllMetaTypes ()
        {
            var list = new List<MetaType>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
				try 
				{
					foreach (var type in assembly.GetTypes())
					{
						if (EditorMetaCommon.IsMetadata(type))
						{
							var metaType = new MetaType(type);
							list.Add(metaType);
						}
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("assembly={0}, ex={1}", assembly.FullName, ex.ToString());
				}
            }

            list.Sort((x, y) => x.FullName.CompareTo(y.FullName));
            var types = list.ToArray();

            return types;
        }
		
		public IEnumerable<MetaType> TemplateTypes	{ get { return _templateTypes;  } }
        public IEnumerable<MetaType> ConfigTypes	{ get { return _configTypes; } }
		public MetaType[]			 MetaTypes		{ get { return _metaTypes; } }

        private readonly List<MetaType> _configTypes = new List<MetaType>();
        private readonly List<MetaType> _templateTypes = new List<MetaType>();

        private MetaType[] _metaTypes;
    }
}