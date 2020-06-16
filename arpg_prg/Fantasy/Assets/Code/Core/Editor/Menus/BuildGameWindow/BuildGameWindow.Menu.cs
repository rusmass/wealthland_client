using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;
using Core.Xml;

namespace Core.Menus
{
    partial class BuildGameWindow
    {
        private void _InitMenuBar()
        {
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("Load Config"), false, _OnClickLoadConfig);
			menu.AddItem (new GUIContent ("Save Config"), false, _OnClickSaveConfig);
			_menuBar.AddMenu("File", menu);

			_BuildSwitchPlatformMenu();
        }

        private void _BuildSwitchPlatformMenu()
        {
            var menu = new GenericMenu();
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            
			menu.AddItem (new GUIContent ("iOS"), activeBuildTarget == BuildTarget.iOS, _OnClickSwitchToIPhone);
			menu.AddItem (new GUIContent ("Android"), activeBuildTarget == BuildTarget.Android, _OnClickSwitchToAndroid);

            _menuBar.AddMenu("Switch Platform", menu);
        }

        private void _OnClickLoadConfig ()
        {
            _LoadConfig();
        }

        private void _OnClickSaveConfig ()
        {
            if (null != _config)
            {
				var isSavable = _builder.GetActiveConfig().IsSavable();

				if (isSavable)
				{
					XmlTools.Serialize(_builderConfigPath, _config);
				}
				else 
				{
					Console.Error.WriteLine("[BuildGameWindow._OnClickSaveConfig()] Can not save an empty config.");
				}
            }
        }

        private void _OnClickSwitchToIPhone ()
        {
            _CheckSwitchToPlatform(BuildTarget.iOS);
        }

        private void _OnClickSwitchToAndroid ()
        {
            _CheckSwitchToPlatform(BuildTarget.Android);
        }

        private void _CheckSwitchToPlatform (BuildTarget target)
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeBuildTarget != target)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(target);
            }

            _BuildSwitchPlatformMenu();
        }
    }
}