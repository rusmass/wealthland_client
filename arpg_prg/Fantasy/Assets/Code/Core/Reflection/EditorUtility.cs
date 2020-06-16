using System;

using Object = UnityEngine.Object;

namespace Core.Reflection
{
    public class EditorUtility
    {
        public static Object[] CollectDependencies(Object[] roots)
        {
            _CheckCreateStaticDelegate("CollectDependencies", ref _lpfnCollectDependencies);
            var result = _lpfnCollectDependencies(roots);
            return result;
        }

        private static void _CheckCreateStaticDelegate<T>(string name, ref T lpfnMethod) where T : class
        {
            if (null == lpfnMethod)
            {
                var method = MyType.GetMethod(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                TypeTools.CreateDelegate(method, out lpfnMethod);
            }
        }

        private static Type _myType;

        public static Type MyType
        {
            get
            {
                if (null == _myType)
                {
                    _myType = System.Type.GetType("UnityEditor.EditorUtility,UnityEditor");
                }

                return _myType;
            }
        }

        private static Func<Object[], Object[]> _lpfnCollectDependencies;
    }
}
