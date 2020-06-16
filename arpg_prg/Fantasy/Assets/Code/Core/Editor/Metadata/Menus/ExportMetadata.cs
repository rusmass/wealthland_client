using UnityEngine;
using Excel;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Core;
using Core.IO;
using Metadata;
using UnityEditor;

namespace Core.Menus
{
    public class ExportMetadata
    {
        [MenuItem("*Metadata/ExportMetadata", false, 3)]
        public static void Export()
        {
            var templatesPath = os.path.join(PathTools.MetadataResourceRoot, "Templates");
            var configsPath = os.path.join(PathTools.MetadataResourceRoot, "Configs");
            var files = Directory.GetFiles(templatesPath, "*.xlsx", SearchOption.AllDirectories);
            ScanTools.ScanAll("ExportMetadata", files, path =>
            {
                if (Directory.Exists(PathTools.ExportMetadataRoot) == false)
                {
                    Directory.CreateDirectory(PathTools.ExportMetadataRoot);
                }
				ExcelExport.WriteTemplates2File(path);
            });

            files = Directory.GetFiles(configsPath, "*.xlsx", SearchOption.AllDirectories);
            ScanTools.ScanAll("ExportMetadata", files, path =>
            {
                if (Directory.Exists(PathTools.ExportMetadataRoot) == false)
                {
                    Directory.CreateDirectory(PathTools.ExportMetadataRoot);
                }
                ExcelExport.WriteConfigs2File(path);
            });
        }
    }
}

public class ExcelExport
{
    public static void WriteTemplates2File(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs))
        {
            var result = excelReader.AsDataSet();
            var table = result.Tables[0];
            var columns = table.Columns.Count;
            var rows = table.Rows.Count;

            int currentRow = 0, currentCol = 0;
            try
            {
                using (var fsWrite = new FileStream(_GetExportName(path), FileMode.Create, FileAccess.Write))
                using (var bw = new OctetsWriter(fsWrite))
                {
                    if (rows == 0)
                    {
                        Debug.Log(path + "为空，请检查文档内容");
                        return;
                    }

                    var types = _GetTypes(table.Rows[1], columns);
                    var noteCount = _GetNoteCount(rows, table);

                    bw.Seek(0, SeekOrigin.Begin);
                    bw.Write(rows - noteCount);

                    for (currentRow = 2; currentRow < rows; ++currentRow)
                    {
                        var rowTable = table.Rows[currentRow];
                        if (!_IsNoteRow(rowTable[0]))
                        {
                            for (currentCol = 0; currentCol < columns; ++currentCol)
                            {
                                _WriteByType(bw, types[currentCol], rowTable[currentCol]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var error = string.Format(" Error in Row:{0}, Col:{1} ", currentRow + 1, currentCol + 1);
                Console.Error.WriteLine(_fileName + error + ex.ToStringEx());
            }
        }
    }

	public static void WriteConfigs2File(string path)
	{
		using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
		using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs))
		{
			var result = excelReader.AsDataSet();
			var table = result.Tables[0];
			var rows = table.Rows.Count;

			int currentRow = 0;
			try
			{
				using (var fsWrite = new FileStream(_GetExportName(path), FileMode.Create, FileAccess.Write))
				using (var bw = new OctetsWriter(fsWrite))
				{
					if (rows == 0)
					{
						Debug.Log(path + "为空，请检查文档内容");
						return;
					}

					bw.Seek(0, SeekOrigin.Begin);

					for (currentRow = 0; currentRow < rows; ++currentRow)
					{
						var rowTable = table.Rows[currentRow];
						_WriteByType(bw, rowTable[1].ToString(), rowTable[2]);
					}
				}
			}
			catch (Exception ex)
			{
				var error = string.Format(" Error in Row:{0}", currentRow + 1);
				Console.Error.WriteLine(_fileName + error + ex.ToStringEx());
			}
		}
	}

    private static string _GetExportName(string rootPath)
    {
        _fileName = Path.GetFileName(rootPath);
        var fileExtension = Path.GetExtension(rootPath);
        var finalName = _fileName.Replace(fileExtension, ".gd");

        return os.path.join(PathTools.ExportMetadataRoot, finalName.ToLower());
    }

    private static int _GetNoteCount(int rows, DataTable table)
    {
        int noteCount = 2; //the first line is chinese notes, the second line is types(such as int, long etc..)
        for (int i = 2; i < rows; ++i)
        {
            var rowTable = table.Rows[i];
            if (_IsNoteRow(rowTable[0]))
            {
                noteCount++;
            }
        }

        return noteCount;
    }

    private static bool _IsNoteRow(object row)
    {
        var unit = row.ToString();
        if (string.IsNullOrEmpty(unit) || unit[0] == '#')
        {
            return true;
        }

        return false;
    }

    private static string[] _GetTypes(DataRow row, int columns)
    {
        var types = new string[columns];
        for (int i = 0; i < columns; ++i)
        {
            types[i] = row[i].ToString();
        }
        return types;
    }

    public static void _WriteByType(OctetsWriter writer, string value, object cell)
    {
        switch (value)
        {
            case "byte":
                writer.Write(System.Convert.ToByte(cell));
                break;
            case "short":
                writer.Write(System.Convert.ToInt16(cell));
                break;
            case "int":
                writer.Write(System.Convert.ToInt32(cell));
                break;
            case "float":
                writer.Write(Convert.ToSingle(cell));
                break;
		case "string":
			var str = cell.ToString ();
			str = str.Replace ("\\n", "\n");
			str = str.Replace ("\\t", "\t");
			str = str.Replace ("\\u3000", "\u3000");
			writer.Write(str);
                break;
            case "bool":
                writer.Write(System.Convert.ToInt32(cell) == 1 ? true : false);
                break;
            case "long":
                writer.Write(System.Convert.ToInt64(cell));
                break;
            case "Vector2":
                var vec2 = cell.ToString().Split(',');
                writer.Write(new Vector2(Convert.ToSingle(vec2[0]), Convert.ToSingle(vec2[1])));
                break;
            case "Vector3":
                var vec3 = cell.ToString().Split(',');
                writer.Write(new Vector3(Convert.ToSingle(vec3[0]), Convert.ToSingle(vec3[1]),
                    Convert.ToSingle(vec3[2])));
                break;
            case "Vector4":
                var vec4 = cell.ToString().Split(',');
                writer.Write(new Vector4(Convert.ToSingle(vec4[0]), Convert.ToSingle(vec4[1]),
                    Convert.ToSingle(vec4[2]),
                    Convert.ToSingle(vec4[3])));
                break;
            case "Color":
                var color = cell.ToString().Split(',');
                var aa = new Color(Convert.ToSingle(color[0]), Convert.ToSingle(color[1]), Convert.ToSingle(color[2]));
                writer.Write(aa);
                break;
		default:

			if (string.IsNullOrEmpty (value)) 
			{
				value = "string.Empty";
			}
			Console.Error.WriteLine("Excel {0} 类型 {1} 有误", _fileName,  value);
                break;
        }
    }

    private static string _fileName;
}
