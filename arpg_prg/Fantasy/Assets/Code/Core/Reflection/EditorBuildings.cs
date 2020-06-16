using System;

namespace Core.Reflection
{
    public enum TargetPlatform
    {
        StandaloneOSXIntel = 4,
        StandaloneWindows = 5,
        WebPlayer = 6,
        iPhone = 9,
        Android = 13,
    }

    public static class EditorBuildings
    {
        private static System.Reflection.PropertyInfo _activeBuildTarget;

        public static TargetPlatform activeBuildTarget
        {
            get
            {
                if (null == _activeBuildTarget)
                {
                    _activeBuildTarget = MyType.GetProperty("activeBuildTarget");
                }

                return (TargetPlatform)_activeBuildTarget.GetValue(null, null);
            }
        }

        private static Type _myType;

        public static Type MyType
        {
            get
            {
                if (null == _myType)
                {
                    _myType = System.Type.GetType("UnityEditor.EditorUserBuildSettings,UnityEditor");
                }

                return _myType;
            }
        }
    }
}
