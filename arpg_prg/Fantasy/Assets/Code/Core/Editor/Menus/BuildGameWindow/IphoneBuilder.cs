using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Core.Menus
{
    public class IphoneBuilder: BuilderBase
    {
		public IphoneBuilder ()
		{
			_il2cpp = PlayerSettings.GetPropertyInt("ScriptingBackend", BuildTargetGroup.iOS) == (int) ScriptingImplementation.IL2CPP;
		}

		public override ConfigBase GetActiveConfig ()
		{
			return _config.iphone;
        }

		public override void _DoOnGUI (ref ProcessQueue processQueue)
		{
			var config = _config.iphone;
			_LeaveSpace();
            
			_DrawOpenFolderPanel("Project Location:", "Selection Location", ref config.projectPath);

			config.builtinResources = GUILayout.Toggle(config.builtinResources, "Builtin Resources");

			var options = _DrawBuildOptions(config);
            
            _LeaveSpace();
            _DrawBuild(config, options);
		}

		private void _DrawSymlinkLibraries (ref BuildOptions options)
		{
			var symlinkLibraries = GUILayout.Toggle(EditorUserBuildSettings.symlinkLibraries, "Symlink Unity Libraries");

			if (EditorUserBuildSettings.symlinkLibraries != symlinkLibraries)
			{
				EditorUserBuildSettings.symlinkLibraries = symlinkLibraries;
			}
			
			if (symlinkLibraries)
			{
				options |= BuildOptions.SymlinkLibraries;
			}
		}

		private void _DrawIl2CPP ()
		{
			var il2cpp = GUILayout.Toggle(_il2cpp, "Il2CPP");
			if (_il2cpp != il2cpp)
			{
				_il2cpp = il2cpp;
				PlayerSettings.SetPropertyInt("ScriptingBackend", (int) ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
				PlayerSettings.SetPropertyInt("Architecture", 2, BuildTargetGroup.iOS);
			}
		}

		private BuildOptions _DrawBuildOptions (IPhoneConfig config)
		{
			BuildOptions options = BuildOptions.None;
			
			_LeaveSpace();
			_DrawIl2CPP();
			_DrawSymlinkLibraries(ref options);
			_DrawDevelopmentBuild(ref options);
			_DrawConnectProfiler(ref options);
			_DrawAllowDebugging(ref options);

			_LeaveSpace();
			
			config.ShowBuiltPlayer = GUILayout.Toggle(config.ShowBuiltPlayer, "Show Built Player Folder");
			if (config.ShowBuiltPlayer)
			{
				options |= BuildOptions.ShowBuiltPlayer;
			}
			
			return options;
		}
		
		private void _DrawBuild (IPhoneConfig config, BuildOptions options)
		{
			if (GUILayout.Button("Build iPhone", _buttonWidth))
			{
				if (string.IsNullOrEmpty(config.projectPath))
				{
					EditorUtility.DisplayDialog("Warning", "Please choose a Project Location", "Ok");
					return;
				}
				
				var projectPath = os.path.join(config.projectPath, "Unity-iPhone.xcodeproj");
				if (Directory.Exists(projectPath))
				{
					var title   = "Warning";
					var message = "Build folder already exists. Would you like to append or replace it?";
					var option	= EditorUtility.DisplayDialogComplex(title, message, "Cancel", "Append", "Replace");
					
					if (option == 0)
					{
						return;
					}
					else if (option == 1)
					{
						options |= BuildOptions.AcceptExternalModificationsToPlayer;
					}
				}
				
				var error = BuildPipeline.BuildPlayer(_GetLevels(), config.projectPath, BuildTarget.iOS, options);
				if (!string.IsNullOrEmpty(error))
				{
					Console.Error.WriteLine("BuildPipeline.BuildPlayer() error={0}", error);
					return;
				}
				
				if (config.builtinResources)
				{
					var rawFolder = os.path.join(config.projectPath, "Data/Raw");
					var flags = PackFlags.SearchAllDirectories;
					PackTools.PackResourcesTo(rawFolder, flags);
				}
                
                os.startfile(projectPath, null, true);
            }
        }

		private bool _il2cpp;
    }
}