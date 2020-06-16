using UnityEditor;
using System.IO;
using Metadata.Raw;

namespace Metadata.Menus
{
    public static class MakeAutoCode
    {
        [MenuItem(EditorMetaCommon.MenuRoot + "Make Auto Code", false, 102)]
        public static void Make()
        {
            var metaManager = new MetaManager();

            new AutoCodeMaker().WriteAll(metaManager);

            _RemoveEmptyFiles(EditorMetaCommon.StandardAutoCodeDirectory);
            _RemoveEmptyFiles(EditorMetaCommon.ClientAutoCodeDirectory);
            _RemoveEmptyFiles(EditorMetaCommon.EditorAutoCodeDirectory);
            AssetDatabase.Refresh();
        }

        private static void _RemoveEmptyFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            foreach (var filename in Directory.GetFiles(directory))
            {
                if (filename.EndsWith(".cs"))
                {
                    var info = new FileInfo(filename);
                    if (info.Length == 0)
                    {
                        File.Delete(filename);
                    }
                }
            }
        }
    }
}