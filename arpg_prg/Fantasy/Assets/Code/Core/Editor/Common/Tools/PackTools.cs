using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Core;
using Core.Web;
using Core.IO;

namespace Core
{
	public enum PackFlags: ushort
	{
		None,
		SearchAllDirectories	= 1,
		OnlyPackExpansion0		= 2,
	}

	public static class PackTools
	{
		public static void PackResourcesTo (string destDirectory, PackFlags flags, Predicate<string> validateFileFunc = null)
		{
			if (string.IsNullOrEmpty(destDirectory))
			{
				Console.Error.WriteLine("Invalid empty destDirectory={0}", destDirectory);
				return;
			}

			os.mkdir(destDirectory);

			var searchOption = (flags & PackFlags.SearchAllDirectories) != 0 ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly; 

			Console.WriteLine (PathTools.ExportResourceRoot);

			var paths = Directory.GetFiles(PathTools.ExportResourceRoot, "*.*", searchOption);
			var isAssetBundleOutline = false;

			var knownDirectories = new HashSet<string>();
			var mappingInfos = new List<MappingInfo>(2048);

			ScanTools.ScanAll("Packing builtin resources...", paths, srcPath => 
				{
					var isValid = null == validateFileFunc || validateFileFunc(srcPath);

					if (isValid)
					{
						var bytes = File.ReadAllBytes(srcPath);
						var localPathWithDigest = string.Empty;
						var localPath = os.path.normpath(PathTools.GetLocalPath(srcPath));

						if (localPath.Equals(PathTools.PlatformResFolder))
						{
							isAssetBundleOutline = true;
							localPathWithDigest = localPath;
						}
						else
						{
							isAssetBundleOutline = false;
							var digest = Md5sum.Instance.GetAssetDigest(bytes);
							localPathWithDigest = PathTools.GetLocalPathWithDigest(localPath, digest);
						}

						var destPath  = os.path.join(destDirectory, localPathWithDigest);

						_CheckCreateDirectory(knownDirectories, destPath);

						if (!File.Exists(destPath))
						{
							File.WriteAllBytes(destPath, bytes);
						}

						var info = new MappingInfo
						{
							localPath = localPath
							, localPathWithDigest = localPathWithDigest
							, selfSize  = os.path.getsize(srcPath)
							, totalSize = isAssetBundleOutline ? os.path.getsize(srcPath) : BundleTools.GetTotalSize(srcPath)
						};

						mappingInfos.Add(info);
					}
				});

			var mappingPath = os.path.join(destDirectory, Constants.BuiltinMappingPath);
			WriteMappingFile(mappingPath, mappingInfos);
		}

		private static void _CheckCreateDirectory (HashSet<string> knownDirectories, string filepath)
		{
			var directory = Path.GetDirectoryName(filepath);
			if (!knownDirectories.Contains(directory))
			{
				os.mkdir(filepath);
				knownDirectories.Add(directory);
			}
		}

		public static void WriteMappingFile (string path, IList<MappingInfo> infos, Predicate<MappingInfo> filter= null)
		{
			if (string.IsNullOrEmpty(path) || null == infos)
			{
				return;
			}

			using (var writer = new CsvWriter(path))
			{
				var rowItems = new List<string>();

				var count = infos.Count;
				for (int i= 0; i< count; ++i)
				{
					var info = infos[i];
					if (null != filter && !filter(info))
					{
						continue;
					}
					rowItems.Clear ();

					rowItems.Add(info.localPathWithDigest);
					rowItems.Add(info.selfSize.ToString());
					rowItems.Add(info.totalSize.ToString());

					writer.WriteRow (rowItems);
				}
			}
		}
	}
}