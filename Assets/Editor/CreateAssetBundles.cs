using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
	private static Dictionary<string, string> bundleNameAndPaths = new Dictionary<string, string>()
	{
		{"placeables","Assets/ScriptibleObjects/Placeables"},
		{"rooms","Assets/ScriptibleObjects/Rooms"}
		//...
	};
	
	private static Dictionary<string, string> prefabLocations = new Dictionary<string, string>()
	{
		{"placeables","Assets/Prefabs/Placeables"},
		{"rooms","Assets/Prefabs/Rooms"}
		//...
	};

	//[MenuItem("Assets/Build AssetBundles")]
	public static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/AssetBundles";
		if(!Directory.Exists(assetBundleDirectory))
		{
			Directory.CreateDirectory(assetBundleDirectory);
		}
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}
	
	[MenuItem("Assets/Build AssetBundles Map")]
	public static void BuildAssetBundleMaps()
	{
		// Create the array of bundle build details.
		List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
		bundleNameAndPaths.ToList().ForEach(kvp => AssignBundlePathsToScriptitbleObjects(kvp.Value, prefabLocations[kvp.Key]));
		bundleNameAndPaths.ToList().ForEach(kvp => buildMap.Add(CreateAssetBundleBuild(kvp.Key, kvp.Value)));
		
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles", buildMap.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}

	//[MenuItem("Assets/AssignBundlePathsToScriptitbleObjects")]
	private static void AssignBundlePathsToScriptitbleObjects(string path, string prefabPath)
	{ 
		DirectoryInfo dir = new DirectoryInfo(path);
		FileInfo[] info = dir.GetFiles("*.asset");
		foreach (FileInfo f in info) 
		{
			PlaceableDefaultInfo so = AssetDatabase.LoadAssetAtPath<PlaceableDefaultInfo>(Path.Combine(path, f.Name));
			PlaceableDefaultInfo newSO = ScriptableObject.CreateInstance<PlaceableDefaultInfo> ();
			AssetDatabase.DeleteAsset(path + "/run-replace-" + f.Name);
			if(so == null)
				Debug.Log("NULL?!?");
			else
			{

				newSO.name = so.name;
				newSO.Cost = so.Cost;
				newSO.ShopSprite = so.ShopSprite;
				newSO.DefaultPlaceableData = so.DefaultPlaceableData;
				newSO.Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + f.Name.Replace(".asset", ".prefab"));
				newSO.AssetBundlePath = Path.Combine(path, f.Name);
				AssetDatabase.CreateAsset(newSO, path + "/run-replace-" + f.Name);
				AssetDatabase.DeleteAsset(path + "/" + f.Name);
				AssetDatabase.CopyAsset(path + "/run-replace-" + f.Name, path + "/" + f.Name);
				AssetDatabase.DeleteAsset(path + "/run-replace-" + f.Name);
			}
		}
		AssetDatabase.SaveAssets();
	}
	
	[MenuItem("Assets/ClearAllData")]
	public static void WipeAllMemortInPersistentDataPath()
	{
		DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
		dataDir.Delete(true);
	}

	private static AssetBundleBuild CreateAssetBundleBuild(string name, string path)
	{
		AssetBundleBuild abb = new AssetBundleBuild();
		DirectoryInfo info = new DirectoryInfo(path);
		string[] fileNames = info.GetFiles().Select(x => Path.Combine(path, x.Name)).Where(x => !x.EndsWith(".meta")).ToArray();

		abb.assetNames = fileNames;
		abb.assetBundleName = name;
		return abb;
	}
}