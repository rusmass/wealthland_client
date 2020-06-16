using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Core.IO;

namespace Core.Web
{
	internal class MappingDB
	{
		public void AddMappingInfos(byte[] bytes, LoadType loadType)
		{
			if (bytes.IsNullOrEmptyEx ()) 
			{
				Console.Error.WriteLine("[MappingDB.AddMappingInfos() bytes = null or empty, loadType = {0}]", loadType);
			}

			using (var reader = new CsvReader(new MemoryStream(bytes))) 
			{
				var it = reader.GetEnumerator();
				while(it.MoveNext())
				{
					var element = it.Current;
					_AddRowItem(element, loadType);
				}
			}
		}

		public bool TryGetMappingInfo(string localPath, out MappingInfo info)
		{
			localPath = localPath ?? string.Empty;
			return _mappingInfos.TryGetValue (localPath, out info);
		}

		//临时用一下dictionary, 后面自己设计合适的数据结构存储, 太费内存了
		public void AddToDB(string localPath, MappingInfo info)
		{
			_mappingInfos.Remove (localPath);
			_mappingInfos.Add (localPath, info);
		}

		public void Dispose()
		{
			if (null != _mappingInfos) 
			{
				_mappingInfos.Clear ();
			}
		}

		private void _AddRowItem(List<string> row, LoadType loadType)
		{
			if (row.Count != 3) 
			{
				Console.Error.WriteLine("[MappingDB._AddRowItem()] row.Count != 3, row = {0}", row.ToString());
				return;
			}

			var localPathWithDigest = row [0];
			var localPath = PathTools.ExtractLocalPath (localPathWithDigest);

			var selfSize = System.Convert.ToInt64 (row [1]);
			var totalSize = System.Convert.ToInt64 (row [2]);

			var info = new MappingInfo () 
			{
				localPath = localPath
				,localPathWithDigest = localPathWithDigest
				,selfSize = selfSize
				,totalSize = totalSize
				,loadType = loadType
			};

			AddToDB (localPath, info);
		}

		//临时用一下dictionary, 后面自己设计合适的数据结构存储, 太费内存了
		private Dictionary<string, MappingInfo> _mappingInfos = new Dictionary<string, MappingInfo>();
	}
}