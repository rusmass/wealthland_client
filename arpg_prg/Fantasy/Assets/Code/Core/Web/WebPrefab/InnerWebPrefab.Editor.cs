using UnityEngine;
using System;
using Core.Reflection;

namespace Core.Web
{
	partial class InnerWebPrefab
	{
		private void _ProcessDependenciesInEditor (UnityEngine.Object goAsset)
		{
            if (!Application.isEditor || null == goAsset)
            {
                return;
            }

			if (localPath.Contains ("ui/scene")) 
			{
				return;
			}

            var dependenices = EditorUtility.CollectDependencies(new UnityEngine.Object[] { goAsset });
            for (int i = 0; i < dependenices.Length; ++i)
            {
                var component = dependenices[i];
                if (null == component)
                {
                    Console.Error.WriteLine("Missing component, localPath ={0}", localPath);
                    continue;
                }
					
				if (component is Renderer)
                {
					_ReassignShaders(component as Renderer);
                }
            }
        }

		private static void _ReassignShaders(Renderer renderer)
        {
            var sharedMaterials = renderer.sharedMaterials;

            if (sharedMaterials == null || sharedMaterials.Length == 0)
            {
                Console.Error.WriteLine("[InnerWebPrefab._ReassignShaders()] gameObject= {0} has not any shared material!");
                return;
            }

            var count = sharedMaterials.Length;

            for (int i = 0; i < count; ++i)
            {
                var material = sharedMaterials[i];

                if (null != material)
                {
                    var shaderName = material.shader.name;
					var shader = Shader.Find (shaderName);
					material.shader = shader;
                }
            }
        }
    }
}
