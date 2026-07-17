using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Managers
{
    public class ObjectPoolManager
    {
        //private Dictionary<string, object> cachedObjectDict;
        private Dictionary<string, List<GameObject>> recycledGameObjectDict;
        
        public ObjectPoolManager()
        {
            Init();
        }
        
        public void Init()
        {
            //cachedObjectDict = new Dictionary<string, object>();
            recycledGameObjectDict = new Dictionary<string, List<GameObject>>();
        }
        
//        public GameObject LoadObject(string path)
//        {
//            //first check to see if one in pool!
//            if (recycledGameObjectDict.ContainsKey(path) && recycledGameObjectDict[path].Count > 0)
//            {
//                GameObject go = recycledGameObjectDict[path][0];
//                recycledGameObjectDict[path].RemoveAt(0);
//                return go;
//            }
//            return null;
//        }
        
        public GameObject LoadObject(GameObjectAssetBundleInfo rso)
        {
            //first check to see if one in pool!
            if (rso.AssetBundlePath == null ||
                !recycledGameObjectDict.ContainsKey(rso.AssetBundlePath) ||
                recycledGameObjectDict[rso.AssetBundlePath].Count <= 0){
                    return GameObject.Instantiate(rso.Prefab);
            }
            GameObject go = recycledGameObjectDict[rso.AssetBundlePath][0];
            recycledGameObjectDict[rso.AssetBundlePath].RemoveAt(0);
            return go;
        }
        
        public void RecycleThisGameObject(GameObject go, string path)
        {
            if (!recycledGameObjectDict.ContainsKey(path))
                recycledGameObjectDict.Add(path, new List<GameObject>());
            recycledGameObjectDict[path].Add(go);
        }

    }
}