using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;
using Core.Xml;

namespace Core.Menus
{
    internal partial class BuildGameWindow: EditorWindow
    {
		[MenuItem("*Resource/Build Game Window %#g", false, 203)]
        private static void _Execute ()
        {
            EditorWindow.GetWindow<BuildGameWindow>(false, "Build Game Window");
        }

        private void OnEnable ()
        {
            _InitMenuBar();
        }

        private void OnDisable ()
        {

        }

        private void _LoadConfig ()
        {
			_config = XmlTools.OpenOrCreate<XmlBuilderConfig>(_builderConfigPath);
        }

        private void OnInspectorUpdate ()
        {
            Repaint();

			if(_builder != null && _processQueue != null)
			{
				_processQueue.Tick(_builder);
			}
			else
			{
				EditorUtility.ClearProgressBar ();
			}
        }

        private void OnGUI ()
        {
            if (null == _config)
            {
                _LoadConfig();
            }

			if (null == _builder || _builder.GetBuildTarget() != EditorUserBuildSettings.activeBuildTarget)
			{
				var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
				_builder = BuilderBase.Create(activeBuildTarget);

                if (null == _builder)
                {
                    this.Close();
                    EditorUtility.DisplayDialog("Title", "This is an unsupported option!", "Close");
                }
				_InitOpenLog ();
			}

            _menuBar.OnGUI();

			if (null != _builder)
			{
				_builder.OnGUI(_config, ref _processQueue);
			}
        }

		private void _InitOpenLog ()
		{
			if (_builder.GetBuildTarget() == BuildTarget.iOS)
			{
				PublishTools.DefineGlobalMacro (BuildTargetGroup.iOS, "OPEN_LOG_OUTPUT");
			}
			else if (_builder.GetBuildTarget() == BuildTarget.Android)
			{
				PublishTools.DefineGlobalMacro (BuildTargetGroup.Android, "OPEN_LOG_OUTPUT");
			}
		}

		private XmlBuilderConfig _config;
		private BuilderBase	_builder;
		private ProcessQueue _processQueue;

        private readonly MenuBar _menuBar = new MenuBar();
		private readonly string _builderConfigPath = os.path.join(Application.persistentDataPath, "builder-config.xml");
    }
}
