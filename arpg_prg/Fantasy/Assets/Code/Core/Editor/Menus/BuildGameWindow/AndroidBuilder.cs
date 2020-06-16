using UnityEngine;
using UnityEditor;
using Core;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace Core.Menus
{
    public class AndroidBuilder: BuilderBase
    {
		public override ConfigBase GetActiveConfig ()
		{
			return _config.android;
		}

		public override void _DoOnGUI (ref ProcessQueue processQueue)
		{
			var config = _config.android;
            _LeaveSpace();
            
			_DrawOpenFilePanel("apktool:", "Selection Location", ref config.apktoolPath);
			_DrawOpenFilePanel("adb:", "Selection Location", ref config.adbPath);
			if (os.isWindows) 
			{
				_DrawOpenFilePanel ("jarsigner", "Selection Location", ref config.jarsignerPath);	
			}

			_DrawSaveFilePanel("Apk Location:", "Selection Location", ref config.apkPath, "apk");
			_DrawAndroid_KeystoreName(config);
            _DrawAndroid_KeytorePass(config);

			config.builtinResources = GUILayout.Toggle(config.builtinResources, "Builtin Resources");

			config.autoInstall = GUILayout.Toggle(config.autoInstall, "Auto Install");

            var options = _DrawBuildOptions(config);
            
            _LeaveSpace();

			using (new LayoutHorizontalScope(0))
			{
            	_DrawBuild(config, options, ref processQueue);
				_DrawInstallApk(ref processQueue);
			}
        }

		private void _DrawAndroid_KeystoreName (AndroidConfig config)
		{
			var keystoreName = config.keystoreName;
			_DrawOpenFilePanel("Keystore Location:", "Selection Location", ref keystoreName, "keystore");
			config.keystoreName = keystoreName;
		}

        private void _DrawAndroid_KeytorePass (AndroidConfig config)
        {
            using (new LayoutHorizontalScope(0))
            {
                GUILayout.Label("Store Password:", _buttonWidth);
				config.keystorePass = GUILayout.TextField(config.keystorePass ?? string.Empty);
            }
        }

        private BuildOptions _DrawBuildOptions (AndroidConfig config)
        {
            BuildOptions options = BuildOptions.None;
            
            EditorGUILayout.Space();

			_DrawDevelopmentBuild(ref options);
			_DrawConnectProfiler(ref options);
			_DrawAllowDebugging(ref options);
            
            return options;
        }

		private void _DrawBuild (AndroidConfig config, BuildOptions options, ref ProcessQueue processQueue)
        {
			var isBuild = GUILayout.Button("Build Android", _buttonWidth);

            if (isBuild)
            {
                var apkPath = config.apkPath;
				if (string.IsNullOrEmpty(apkPath))
                {
                    EditorUtility.DisplayDialog("Warning", "Please choose a Project Location", "Ok");
                    return;
                }
                
				PlayerSettings.Android.keystoreName = config.keystoreName;
				PlayerSettings.Android.keystorePass = config.keystorePass;
				PlayerSettings.Android.keyaliasName = Path.GetFileName(config.keystoreName);
				PlayerSettings.Android.keyaliasPass = config.keystorePass;
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

                var result = BuildPipeline.BuildPlayer(_GetLevels(), apkPath, BuildTarget.Android, options);
				if (!string.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine(result);
					return;
                }

				processQueue = new ProcessQueue();

				if (config.builtinResources)
				{
					processQueue.Enqueue(_UnpackApk);
					processQueue.Enqueue(_PackApkResoucesAndRepackApk);
					processQueue.Enqueue(_JarsignerRepackedApk);
				}

				if (config.autoInstall)
				{
					processQueue.Enqueue(_InstallApk);
				}
            }
        }

		private void _DrawInstallApk (ref ProcessQueue processQueue)
		{
			var isInstall = GUILayout.Button("Install Apk", _buttonWidth);
			
			if (isInstall)
			{
				processQueue = new ProcessQueue();
				processQueue.Enqueue(_InstallApk);
			}
		}

		private Process _UnpackApk ()
		{
			var config = _config.android;
			var apkPath = config.apkPath;
			var apktoolPath = config.apktoolPath;
			var unpackedFolder = apkPath.Substring(0, apkPath.Length - 4);

			var process = new Process();
			var psi = process.StartInfo;

			psi.FileName = apktoolPath;
			psi.Arguments= string.Format("d -f {0} -o {1}", config.apkPath, unpackedFolder);
			psi.UseShellExecute = true;

			process.Start();
			return process;
		}

		private Process _PackApkResoucesAndRepackApk ()
		{
			var config = _config.android;
			var apkPath = config.apkPath;
			var apktoolPath = config.apktoolPath;
			var unpackedFolder = apkPath.Substring(0, apkPath.Length - 4);

			var rawFolder = os.path.join(unpackedFolder, Constants.LocalApkDirectory);
			var flags = PackFlags.SearchAllDirectories;

			PackTools.PackResourcesTo (rawFolder, flags, srcPath => srcPath.EndWithAnyEx(Constants.BuiltinFileExtensions));

			var process = new Process();
			var psi = process.StartInfo;

			psi.FileName = apktoolPath;
			psi.Arguments= string.Format("b {0}", unpackedFolder);
			psi.UseShellExecute = true;

			process.Start();
			return process;
		}

		private Process _JarsignerRepackedApk ()
		{
			var config = _config.android;
			var apkPath = config.apkPath;
			var jarsignePath = os.isWindows ? config.jarsignerPath : "jarsigner";
			var unpackedFolder = apkPath.Substring(0, apkPath.Length - 4);
			var distApkPath = string.Format("{0}/dist/{1}", unpackedFolder, Path.GetFileName(apkPath));

			var keystoreName = config.keystoreName;
			var keyaliasName = Path.GetFileName(keystoreName);

			var process = new Process();
			var psi = process.StartInfo;

			psi.FileName = jarsignePath;
			psi.Arguments= string.Format("-digestalg SHA1 -sigalg SHA1withRSA -storepass {0} -keystore {1} -signedjar {2}.repacked.apk {3} {4}"
			                             , config.keystorePass
			                             , keystoreName
			                             , unpackedFolder
			                             , distApkPath
			                             , keyaliasName);
			psi.UseShellExecute = true;

			process.Start();
			return process;
		}

		private Process _InstallApk ()
		{
			var config = _config.android;
			var apkPath = config.apkPath;
			var unpackedFolder = apkPath.Substring(0, apkPath.Length - 4);

			var apkName = config.builtinResources ? unpackedFolder + ".repacked.apk" : unpackedFolder + ".apk";

			var process = new Process();
			var psi = process.StartInfo;
			
			psi.FileName = config.adbPath;
			psi.Arguments= "install -r " + apkName;
			psi.UseShellExecute = true;
			
			process.Start();
			return process;
		}
    }
}