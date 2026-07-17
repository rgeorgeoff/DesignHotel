using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// https://docs.unity3d.com/Manual/AssetBundles-Workflow.html

//I guess this is the way to do it...
//    string AssetBundlePath = Application.streamingAssetsPath + "/AssetBundles/Windows/";
//    AssetBundle theBundle = AssetBundle.LoadFromFile(AssetBundlePath + "objectsbundle1");
//    GameObject[] allObjectsBundle1Prefabs = theBundle.LoadAllAssets<GameObject>();

//    4.2.1.1. Streaming Assets
//    The easiest way to include any type of content, including AssetBundles, within a Unity application at install time is to build the content into the /Assets/StreamingAssets/ folder, prior to building the project. Anything contained in the StreamingAssets folder at build time will be copied into the final application.
//
//    The full path to the StreamingAssets folder on local storage is accessible via the property Application.streamingAssetsPath at runtime. The AssetBundles can then be loaded with via AssetBundle.LoadFromFile on most platforms.
//
//    Android Developers: On Android, assets in the StreamingAssets folders are stored into the APK and may take more time to load if they are compressed, as files stored in an APK can use different storage algorithms. The algorithm used may vary from one Unity version to another. You can use an archiver such as 7-zip to open the APK to determine if the files are compressed or not. If they are, you can expect AssetBundle.LoadFromFile() to perform more slowly. In this case, you can retrieve the cached version by using UnityWebRequest.GetAssetBundle as a workaround. By using UnityWebRequest, the AssetBundle will be uncompressed and cached during the first run, allowing for following executions to be faster. Note that this will will take more storage space, as the AssetBundle will be copied to the cache. Alternatively, you can export your Gradle project and add an extension to your AssetBundles at build time. You can then edit the build.gradle file and add that extension to the noCompress section. Once done, you should be able to use AssetBundle.LoadFromFile() without having to pay the decompression performance cost.
//
//    Note: Streaming Assets is not a writable location on some platforms. If a project's AssetBundles need to be updated after installation, either use WWW.LoadFromCacheOrDownload or write a custom downloader.

// To ensure that sprite atlases are not duplicated, check that all sprites tagged into the same sprite atlas are assigned to the same AssetBundle.

// I think the trick here is to come up with a solution that would work to load up everything at start (a single bundle?) or if assets get to big, split it up and load as needed...it does seem like everything could just be loaded once though... like a single asset bundle and be done, editable OTA

public class LoadFromFileExample{
    void LoadAssetBundle() {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myassetBundle"));
        if (myLoadedAssetBundle == null) {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        GameObject.Instantiate(prefab);
    }
    
    //from web
    IEnumerator InstantiateObject()
    {
        string assetBundleName = "bundleName";
        string uri = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;        
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        GameObject cube = bundle.LoadAsset<GameObject>("Cube");
        GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        GameObject.Instantiate(cube);
        GameObject.Instantiate(sprite);
    }
}