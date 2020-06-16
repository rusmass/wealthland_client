using UnityEditor;
using System.IO;

namespace Metadata.Menus
{
    class ClearAutoCode
    {
        [MenuItem(EditorMetaCommon.MenuRoot + "Clear Auto Code", false, 103)]
        public static void Clear()
        {
            var directories = new string[]
            {
                EditorMetaCommon.StandardAutoCodeDirectory,
                EditorMetaCommon.ClientAutoCodeDirectory,
                EditorMetaCommon.EditorAutoCodeDirectory,
            };

            for (int i = 0; i < directories.Length; ++i)
            {
                var directory = directories[i];
                if (!Directory.Exists(directory))
                {
                    continue;
                }

                var files = Directory.GetFiles(directories[i]);
                for (int j = 0; j < files.Length; ++j)
                {
                    var filename = files[j];
                    if (!filename.EndsWith(".meta"))
                    {
                        using (var fout = File.Create(filename))
                        {

                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }
}