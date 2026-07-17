using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AssetBundleManager
{

    private List<AssetBundle> _assetBundles = new List<AssetBundle>();
    private List<string> assetBundlePaths = new List<string>();

    public enum KnownBundleNames
    {
        placeables
    }
    
    public AssetBundleManager()
    {
        Init();
    }

    public void Init()
    {
        assetBundlePaths.Add("Assets/AssetBundles/placeables");
        assetBundlePaths.Add("Assets/AssetBundles/rooms");
        LoadAllAssetBundles();
    }

    private void LoadAllAssetBundles()
    {
        //need a record of all asset bundles to load on start
        assetBundlePaths.ForEach(assetBundlePath => 
            _assetBundles.Add(AssetBundle.LoadFromFile(assetBundlePath)));
        //_assetBundles[0].GetAllAssetNames().ToList().ForEach(x => Debug.Log(x));
    }
    
    //here we have no idea what we are loading up, it could be a gameobject, a texture, no idea.
    private object LoadAssetFromAssetBundle(string assetBundlePath)
    {
        AssetBundle assetBundle = _assetBundles.FirstOrDefault(ab => ab.Contains(assetBundlePath));
        if (assetBundle == null) {
            Debug.Log("Failed to find asset " + assetBundlePath);
            return null;
        }
        object a = assetBundle.LoadAsset(assetBundlePath);
        return a;
    }

    //here we are expecting an object from the LoadAssetFromAssetBundleGuy, but it doesnt know what it is, so how can we? .. somewhere this should be inforced. (client?) 
    public object LoadObject(string path) 
    {
        return LoadAssetFromAssetBundle(path);
    }

    public IEnumerable<string> GetAllAssetsInBundle(string bundleName)
    {
        return _assetBundles.First(ab => ab.name == bundleName).GetAllAssetNames();
    }
    
    public IEnumerable<object> GetAllObjectsInBundle(string bundleName)
    {
        var t = _assetBundles.FirstOrDefault(ab => ab.name == bundleName);
        if (t)
            return t.LoadAllAssets();
        else
            return null;
    }
  
}