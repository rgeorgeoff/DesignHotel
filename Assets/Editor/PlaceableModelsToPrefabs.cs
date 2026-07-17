using System.Collections;
using System.Collections.Generic;
using System.IO;
using BuildControllers;
using UnityEditor;
using UnityEngine;

public class PlaceableModelsToPrefabs
{
	private static string _modelsLoc = "Assets/Models/Placeables";
	private static string _prefabsLoc = "Assets/Prefabs/Placeables";
	
	[MenuItem("Assets/Make Placeable Prefabs From Models")]
	public static void BuildAllAssetBundles()
	{
		
		if(!Directory.Exists(_prefabsLoc))
		{
			Directory.CreateDirectory(_prefabsLoc);
		}
		DirectoryInfo dir = new DirectoryInfo(_modelsLoc);
		FileInfo[] info = dir.GetFiles("*.*");
		foreach (FileInfo f in info) 
		{
			if(!f.Name.EndsWith("blend"))
				continue;
			GameObject go = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine(_modelsLoc, f.Name)));
			go.AddComponent<Placeable>();
			MeshRenderer m = go.GetComponentInChildren<MeshRenderer>();
			if (m)
			{
				m.gameObject.AddComponent<BoxCollider>();
				m.gameObject.AddComponent<PlaceableChild>();
			}

			PrefabUtility.CreatePrefab(_prefabsLoc + "/" + f.Name.Split('.')[0] + ".prefab", go);
			GameObject.DestroyImmediate(go);
		}
	}
}
