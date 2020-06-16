using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Core.Xml
{
	public static class XmlTools
	{
		public static void Serialize<T> (string path, T item)
		{
			using (var writer = new StreamWriter(path))
			{
				writer.NewLine = os.linesep;
				var serializer = new XmlSerializer (typeof(T));
				serializer.Serialize(writer, item);
			}
		}

		public static T Deserialize<T> (string path)
		{
			using (var reader = new StreamReader(path))
			{
				var serializer = new XmlSerializer (typeof(T));
				return (T)serializer.Deserialize(reader);
			}
		}

		public static T OpenOrCreate<T> (string path) where T : new()
		{
			if (File.Exists(path))
			{
				var xml = Deserialize<T>(path);

				if (null != xml)
				{
					return xml;
				}
			}

			return new T();
		}
	}
}