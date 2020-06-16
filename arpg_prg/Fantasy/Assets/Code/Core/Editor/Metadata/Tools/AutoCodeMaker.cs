using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Core.AutoCode;
using Core;

namespace Metadata.Raw
{
    class AutoCodeMaker
    {
		enum AutoCodeType
		{
			ClientCode,
			StandardCode,
			EditorCode,
		}

        public void WriteAll (MetaManager metaManager)
        {
			_InitRootTypes(metaManager);
			_CheckCodeDistribution(metaManager);

            _WriteMetadataManager(metaManager);
            _WriteAllMetadataClasses(metaManager);
            _rootTypes.Clear();
        }

        private void _InitRootTypes (MetaManager metaManager)
        {
            foreach (var type in metaManager.MetaTypes)
            {
                var rootType = EditorMetaCommon.GetRootMetadata(type.RawType);
                if (type.RawType != rootType)
                {
                    _rootTypes.Add(rootType);
                }
            }
        }

        private void _WriteMetadataManager(MetaManager metaManager)
        {
            var _templates = new List<string>();
            var _configs = new List<string>();

            foreach (var type in metaManager.MetaTypes)
            {
                if (!type.IsAbstract && type.IsTemplate)
                {
                    _templates.Add(type.Name);
                }
                else if (!type.IsAbstract && type.IsConfig)
                {
                    _configs.Add(type.Name);
                }
            }

            _WriteMetadataManagerTemplates(_templates);
            _WriterMetadataManagerConfigs(_configs);
        }

        private void _WriteMetadataManagerTemplates(List<string> templates)
        {
            using (_writer = new CodeWriter(_GetTemplateManagerPath()))
            {
                _WriteFileHead();
                using (CodeScope.Create(_writer, "{\n", "}"))
                {
                    _writer.WriteLine("public partial class TemplateManager");
                    using (CodeScope.CreateCSharpScope(_writer))
                    {
                        _writer.WriteLine("partial void _LoadTemplates()");
                        using (CodeScope.CreateCSharpScope(_writer))
                        {
                            foreach (var template in templates)
                            {
                                _writer.WriteLine("_AddLoaderTemplate<{0}>();", template);
                            }
                        }
                    }
                }
            }
        }

        private void _WriterMetadataManagerConfigs(List<string> configs)
        {
            using (_writer = new CodeWriter(_GetConfigManagerPath()))
            {
                _WriteFileHead();
                using (CodeScope.Create(_writer, "{\n", "}"))
                {
                    _writer.WriteLine("public partial class ConfigManager");
                    using (CodeScope.CreateCSharpScope(_writer))
                    {
                        _writer.WriteLine("partial void _LoadConfigs()");
                        using (CodeScope.CreateCSharpScope(_writer))
                        {
                            foreach (var config in configs)
                            {
                                _writer.WriteLine("_AddLoaderConfig<{0}>();", config);
                            }
                        }
                    }
                }
            }
        }

        private void _WriteAllMetadataClasses (MetaManager metaManager)
        {
            foreach (var type in metaManager.MetaTypes)
            {
                if (type.IsNested
                    || type.IsSerializable
                    || !_rootTypes.Contains(type.RawType) && type.IsAbstract)
                {
                    continue;
                }

                using (_writer = new CodeWriter(type.GetAutoCodePath()))
                {
                    _WriteFileHead();

					using (CodeScope.Create(_writer, "{\n", "}"))
                    {
                        _WriteOneClassOrStruct(type.RawType, type.GetExportFlags());
                    }
                }
            }
        }

		private CodeAssembly _CheckCodeDistribution (MetaManager metaManager)
		{
			var distribution = CodeAssembly.None;

			foreach (var type in metaManager.MetaTypes)
			{
				var codeAssembly = type.GetCodeAssembly();
				distribution |= codeAssembly;
				if (distribution == CodeAssembly.All)
				{
					break;
				}
			}

			if ((distribution & Metadata.CodeAssembly.StandardAssembly) != 0)
			{
                Directory.CreateDirectory(EditorMetaCommon.StandardAutoCodeDirectory);
			}
			
			if ((distribution & Metadata.CodeAssembly.ClientAssembly) != 0)
			{
                Directory.CreateDirectory(EditorMetaCommon.ClientAutoCodeDirectory);
			}
			
			if ((distribution & Metadata.CodeAssembly.EditorAssembly) != 0)
			{
                Directory.CreateDirectory(EditorMetaCommon.EditorAutoCodeDirectory);
			}

			return distribution;
		}

        private void _WriteFileHead ()
        {
			_writer.WriteLine("using UnityEngine;");
            _writer.WriteLine("using System;");
            _writer.WriteLine("using System.Collections.Generic;");
            _writer.WriteLine("using Core.IO;");
            
            _writer.WriteLine();
            _writer.WriteLine("namespace Metadata");
        }

        private void _WriteOneClassOrStruct (Type type, ExportFlags flags)
        {
            var classOrStruct = type.IsClass ? "class" : "struct";
			var sealedClass = type.IsClass && EditorMetaCommon.IsFinalType(type) ? "sealed " : string.Empty;

            _writer.WriteLine("[Serializable]");
			_writer.WriteLine("{0}partial {1} {2} : {3}", sealedClass, classOrStruct, type.Name, typeof(ILoadable).Name);

            using (MacroScope.CreateEditorScope(_writer.BaseWriter))
            {
                _writer.WriteLine(", {0}", typeof(ISavable).Name);
            }

            using (CodeScope.CreateCSharpScope(_writer))
            {
				var nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Public);
                _WriteNestedTypes(nestedTypes, flags);

                var members = new List<MemberBase>();
                _CollectSerializableMembers(type, members);

                _WriteSaveMethod(type, members);
                _WriteLoadMethod(type, members);
                _WriteToStringMethod(type, members);
                _WriteGetMetadataTypeMethod(type);
            }
        }

        private void _WriteNestedTypes (Type[] nestedTypes, ExportFlags flags)
        {
            if (nestedTypes.Length > 0)
            {
                //Array.Sort(nestedTypes, (a, b) => a.Name.CompareTo(b.Name));
                //foreach (var nestedType in nestedTypes)
                //{
                //    if (typeof(ILoadable).IsAssignableFrom(nestedType))
                //    {
                //        continue;
                //    }
				//
                //    _WriteOneClassOrStruct(nestedType, flags);
                //}

				Console.Error.WriteLine ("Can not use nested type. nestedTypes = {0}", nestedTypes.ToString());
            }
        }

        private void _WriteLoadMethod (Type type, List<MemberBase> members)
        {
			_writer.WriteLine("[Export(ExportFlags.AutoCode)]");

            if (_rootTypes.Contains(type))
            {
				_writer.WriteLine("public virtual void Load (IOctetsReader reader)");
                using(CodeScope.CreateCSharpScope(_writer))
                {
                    _writer.WriteLine("throw new NotImplementedException(\"This method should be override~\");");
                }
            }
            else
            {
                var overrideText = _rootTypes.Contains(EditorMetaCommon.GetRootMetadata(type)) ? "override " : string.Empty;
                _writer.WriteLine("public {0}void Load (IOctetsReader reader)", overrideText);
                using (CodeScope.CreateCSharpScope(_writer))
                {
                    for (int index= 0; index < members.Count; ++index)
                    {
                        var member = members[index];
                        member.WriteLoad(_writer);
                    }
                }
            }
        }

        private void _WriteSaveMethod(Type type, List<MemberBase> members)
        {
            if (_rootTypes.Contains(type))
            {
                _writer.WriteLine("[Export(ExportFlags.AutoCode)]");
                _writer.WriteLine("public virtual void Save (IOctetsWriter writer)");
                using (CodeScope.CreateCSharpScope(_writer))
                {
                    _writer.WriteLine("throw new NotImplementedException(\"This method should be override~\");");
                }
            }
            else
            {
                using (MacroScope.CreateEditorScope(_writer.BaseWriter))
                {
                    _writer.WriteLine("[Export(ExportFlags.AutoCode)]");

                    var overrideText = _rootTypes.Contains(EditorMetaCommon.GetRootMetadata(type)) ? "override " : string.Empty;
                    _writer.WriteLine("public {0}void Save (IOctetsWriter writer)", overrideText);
                    using (CodeScope.Create(_writer, "{\n", "}\n"))
                    {
                        for (int index = 0; index < members.Count; ++index)
                        {
                            var member = members[index];
                            member.WriteSave(_writer);
                        }
                    }
                }
            }
        }

        private void _WriteGetMetadataTypeMethod(Type type)
        {
            _writer.WriteLine("[Export(ExportFlags.AutoCode)]");

            if (_rootTypes.Contains(type))
            {
                _writer.WriteLine("public virtual ushort GetMetadataType ()");
                using (CodeScope.CreateCSharpScope(_writer))
                {
                    _writer.WriteLine("throw new NotImplementedException(\"This method should be override~\");");
                }
            }
            else
            {
                var overrideText = _rootTypes.Contains(EditorMetaCommon.GetRootMetadata(type)) ? "override " : string.Empty;
                _writer.WriteLine("public {0}ushort GetMetadataType ()", overrideText);

                using (CodeScope.CreateCSharpScope(_writer))
                {
                    //var typeName = EditorMetaCommon.GetMetaTypeName(type);
                    //_writer.WriteLine("return (ushort) MetadataType.{0};", typeName);
                    _writer.WriteLine("throw new NotImplementedException(\"This method should be override~\");");
                }
            }
        }

        private void _CollectSerializableMembers (Type type, List<MemberBase> members)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public;

			var idDecalringTemplate = 0;
            foreach (var field in type.GetFields(flags))
            {
                if (!EditorMetaCommon.IsAutoCodeIgnore(field))
                {
                    var member = MemberBase.Create(field.FieldType, field.Name);
					if (field.DeclaringType == typeof(Template)) 
					{
						members.Insert (idDecalringTemplate++, member);
					}
					else
					{
						members.Add (member);
					}
                }
            }
        }

        private void _WriteEqualsToMethod(Type type, List<MemberBase> members)
        {
            _writer.WriteLine("[Export(ExportFlags.AutoCode)]");
            if (_rootTypes.Contains(type))
            {
                _writer.WriteLine("public virtual bool EqualsTo (IMetadata other)");
                using (CodeScope.CreateCSharpScope(_writer))
                {
                    _writer.WriteLine("throw new NotImplementedException(\"This method should be override~\");");
                }
            }
            else
            {
                var overrideText = _rootTypes.Contains(EditorMetaCommon.GetRootMetadata(type)) ? "override " : string.Empty;
                _writer.WriteLine("public {0}bool EqualsTo (IMetadata other)", overrideText);

                using (CodeScope.CreateCSharpScope(_writer))
                {
                    var typeName = EditorMetaCommon.GetNestedClassName(type);
                    _writer.WriteLine("var that = ({0}) other;", typeName);

                    if (type.IsClass)
                    {
                        _writer.WriteLine("if (null == that)");
                        using (CodeScope.CreateCSharpScope(_writer))
                        {
                            _writer.WriteLine("return false;");
                        }
                    }

                    var memberCount = members.Count;

                    for (int index = 0; index < memberCount; ++index)
                    {
                        var member = members[index];
                        member.WriteNotEqualsReturn(_writer);
                    }

                    _writer.WriteLine("return true;");
                }
            }
        }

        private void _WriteToStringMethod(Type type, List<MemberBase> members)
        {
            //			_writer.WriteLine("[Export(ExportFlags.AutoCode)]");
            _writer.WriteLine("public override string ToString ()");
            using (CodeScope.Create(_writer, "{\n", "}\n"))
            {
                var memberCount = members.Count;

                if (memberCount > 0)
                {
                    _sbText.Append("return string.Format(\"[");
                    _sbText.Append(type.Name);
                    _sbText.Append(":ToString()] ");

                    for (int index = 0; index < memberCount; ++index)
                    {
                        _sbText.Append(members[index].GetMemberName());
                        _sbText.Append("={");
                        _sbText.Append(index);
                        _sbText.Append("}");

                        if (index < memberCount - 1)
                        {
                            _sbText.Append(", ");
                        }
                    }

                    _sbText.Append("\", ");

                    for (int index = 0; index < memberCount; ++index)
                    {
                        _sbText.Append(members[index].GetMemberName());

                        if (index < memberCount - 1)
                        {
                            _sbText.Append(", ");
                        }
                    }

                    _sbText.Append(");");
                }
                else
                {
                    _sbText.Append("return \"[");
                    _sbText.Append(type.Name);
                    _sbText.Append(":ToString()]\";");
                }

                _writer.WriteLine(_sbText.ToString());
                _sbText.Length = 0;
            }
        }

        private string _GetTemplateManagerPath()
        {
            //这里暂时不考虑LuaMetadata,因为LuaMetadata加载是在Lua进行
            //但是代码是在C#生成的
            var path = EditorMetaCommon.ClientAutoCodeDirectory + "TemplateManager.AutoCode.cs";
            return path;
        }

        private string _GetConfigManagerPath()
        {
            //这里暂时不考虑LuaMetadata,因为LuaMetadata加载是在Lua进行
            //但是代码是在C#生成的
            var path = EditorMetaCommon.ClientAutoCodeDirectory + "ConfigManager.AutoCode.cs";
            return path;
        }

        public CodeWriter GetCodeWriter ()
        {
            return _writer;
        }

        private CodeWriter _writer;
        private StringBuilder _sbText = new StringBuilder(256);
        private HashSet<Type> _rootTypes = new HashSet<Type>();
    }
}