using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

internal class CreateDefaultFolders
{
	[MenuItem("Sticky Balls/Create Default Folders")]
	private static void CreateFolders()
	{
		CreateDirectory("Animations");
		CreateDirectory("Animations/Player");
		CreateDirectory("Animations/Misc");
		CreateDirectory("Materials");
		CreateDirectory("Models");
		CreateDirectory("Models/2D");
		CreateDirectory("Models/3D");
		CreateDirectory("Prefabs");
		CreateDirectory("Scenes");
		CreateDirectory("Scenes/Main");
		CreateDirectory("Scenes/Sandbox");
		CreateDirectory("Scenes/Sandbox/Sam");
		CreateDirectory("Scenes/Sandbox/Luca");
		CreateDirectory("Scenes/Sandbox/Martin");
		CreateDirectory("Scenes/Sandbox/Thomas");
		CreateDirectory("Scenes/Sandbox/Lars");
		CreateDirectory("Scripts");
		CreateDirectory("Scripts/Managers");
		CreateDirectory("Shaders");
		CreateDirectory("Audio");
		CreateDirectory("Audio/SFX");
		CreateDirectory("Audio/Music");
		CreateDirectory("Textures");
		CreateDirectory("Textures/UV");
		CreateDirectory("Textures/2DSprite");
		CreateDirectory("Fonts");
		AssetDatabase.Refresh();
	}

	private static void CreateDirectory(string name)
	{
		string path = Path.Combine(Application.dataPath, name);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}
}