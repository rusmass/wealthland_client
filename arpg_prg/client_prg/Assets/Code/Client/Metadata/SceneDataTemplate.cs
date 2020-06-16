using UnityEngine;

namespace Metadata
{
    /// <summary>
    ///  场景配置，3d模型场景和摄像机的配置
    /// </summary>
	public partial class SceneDataTemplate
	{
		public int[] playerPos = new int[2]{0, 0};
//		public Vector3 cameraPos = new Vector3(9.15f,10.06f,-0.34f);
//		public Vector3 cameraRotation = new Vector3(48.432f, -89.759f,0.943f);
		public Vector3 cameraPos = new Vector3(10.55f,13.23f,-0.23f);
		public Vector3 cameraRotation = new Vector3(50.176f, -89.75001f,0.9540001f);
		public string resourceName = "prefabs/level/battle_scene.ab";
		public string backgroundAudio = "share/audio/bgm.ab";
		public string lightmapName = string.Empty;
	}
}