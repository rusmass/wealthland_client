using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEditor.SceneManagement;

namespace Core.Menus
{
    public abstract class BuilderBase
    {
		public static BuilderBase Create (BuildTarget buildTarget)
		{
			BuilderBase builder = null;

			if (buildTarget == BuildTarget.iOS)
			{
				builder = new IphoneBuilder();
			}
			else if (buildTarget == BuildTarget.Android)
			{
				builder = new AndroidBuilder();
			}
			else if (buildTarget == BuildTarget.StandaloneOSXIntel
			         || buildTarget == BuildTarget.StandaloneWindows)
			{
				Console.Error.WriteLine ("This is an unsupported option!");
			}

			if (null != builder)
			{
				builder._buildTarget = buildTarget;
			}

			return builder;
		}

		protected void _LeaveSpace ()
		{
			const float spacePixels = 10f;
			GUILayout.Space(spacePixels);
		}

		protected static string[] _GetLevels ()
		{
			var scene = EditorSceneManager.GetActiveScene ();
			if (string.IsNullOrEmpty(scene.name) || scene.name != "WinMain")
			{
				Console.Error.WriteLine("[BuildGameWindow._GetLevels()] You should load the WinMain scene for build game.");
			}
			
			string[] levels = { scene.path };
            return levels;
        }

		protected void _DrawOpenFilePanel (string label, string button, ref string filename, string extension= "")
		{
			using (new LayoutHorizontalScope(0))
			{
				GUILayout.Label(label, _buttonWidth);
				
				GUILayout.TextField(filename ?? string.Empty);
				
				if (GUILayout.Button(button, _buttonWidth))
				{
					var folder = string.Empty;
					if (string.IsNullOrEmpty(filename))
					{
						folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					}
					else 
					{
						folder = Path.GetDirectoryName(filename);
					}
						
					filename = EditorUtility.OpenFilePanel(button, folder, extension);
				}
			}
		}

		protected void _DrawSaveFilePanel (string label, string button, ref string filename, string extension="")
		{
			using (new LayoutHorizontalScope(0))
			{
				GUILayout.Label(label, _buttonWidth);

				filename = filename ?? string.Empty;
				GUILayout.TextField(filename);
				
				if (GUILayout.Button(button, _buttonWidth))
				{
					var folder = string.Empty;
					var name   = string.Empty;
					if (string.IsNullOrEmpty(filename))
					{
						folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					}
					else 
					{
						folder = Path.GetDirectoryName(filename);
						if (Directory.Exists(folder))
						{
							name = Path.GetFileNameWithoutExtension(filename);
						}
						else
						{
							folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
						}
					}
					
					filename = EditorUtility.SaveFilePanel(button, folder, name, extension);
                }
            }
        }
        
        protected void _DrawOpenFolderPanel (string label, string button, ref string folder)
        {
            using (new LayoutHorizontalScope(0))
            {
                GUILayout.Label(label, _buttonWidth);
                GUILayout.TextField(folder ?? string.Empty);
                
                if (GUILayout.Button(button, _buttonWidth))
                {
                    var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    folder = EditorUtility.OpenFolderPanel(button, desktopFolder, string.Empty);
				}
			}
		}

		protected void _DrawDevelopmentBuild (ref BuildOptions options)
		{
			var development = GUILayout.Toggle(EditorUserBuildSettings.development, "Development Build");

			if (EditorUserBuildSettings.development != development)
			{
				EditorUserBuildSettings.development = development;
			}
			
			if (development)
			{
				options |= BuildOptions.Development;
			}
		}

		protected void _DrawConnectProfiler (ref BuildOptions options)
		{
			var connectProfiler = GUILayout.Toggle(EditorUserBuildSettings.connectProfiler, "Auto Connect Profiler");
			
			if (EditorUserBuildSettings.connectProfiler != connectProfiler)
			{
				EditorUserBuildSettings.connectProfiler = connectProfiler;
			}

			if (connectProfiler)
			{
				options |= BuildOptions.ConnectWithProfiler;
			}
		}

		protected void _DrawAllowDebugging (ref BuildOptions options)
		{
			var allowDebugging = GUILayout.Toggle(EditorUserBuildSettings.allowDebugging, "Script Debugging");
			
			if (EditorUserBuildSettings.allowDebugging != allowDebugging)
			{
				EditorUserBuildSettings.allowDebugging = allowDebugging;
			}

			if (allowDebugging)
			{
				options |= BuildOptions.AllowDebugging;
			}
		}
		
        public void OnGUI (XmlBuilderConfig config, ref ProcessQueue processQueue)
        {
            _config = config;
            
            _buttonWidth = GUILayout.Width(128.0f);
			_DoOnGUI(ref processQueue);
        }
        
        public BuildTarget GetBuildTarget ()
		{
			return _buildTarget;
		}

		public abstract ConfigBase GetActiveConfig ();
		public abstract void _DoOnGUI (ref ProcessQueue processQueue);
        
		protected XmlBuilderConfig  _config;
		private BuildTarget      	_buildTarget;
		protected GUILayoutOption	_buttonWidth;
    }
}