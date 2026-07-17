using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//This guy handles the in mem datastore AND will persist to disk when needed
namespace Managers
{
    public class DefaultDataStoreManger : IDataStoreManager
    {
        public Dictionary<string, string> PathStoreLocations = new Dictionary<string, string>();
        private Dictionary<string, object> memStore = new Dictionary<string, object>();
        HashSet<string> pathsToBeUpdated = new HashSet<string>();


        public DefaultDataStoreManger(bool clearMemOnStart = false)
        {
            if (clearMemOnStart)
            {
                Debug.Log("WIPING ALL MEMORY STORE! -- IF THIS IS NOT INTENDED PLEASE CHANGE INIT PARAMS");
                WipeAllMemortInPersistentDataPath();
            }
        }

        //T MUST BE SERIALIZABLE
        public void Save<T>(T obj, string savePath)
        {
            CommitToMemStore(savePath, obj);
            PushToDisk(savePath, obj);
        }

        public T Load<T>(string savePath)
        {
            object obj;
            if (TryLoadFromMemStore(savePath, out obj))
            {
                return (T) obj;
            }
            else
            {
                if (File.Exists(Path.Combine(Application.persistentDataPath, savePath)))
                {
                    BinaryFormatter bf = BinaryFormaterWithSurrogates.GetBFInstance();
                    FileStream fs = File.Open(Path.Combine(Application.persistentDataPath, savePath), FileMode.Open);
                    T loadedObj = (T) bf.Deserialize(fs);
                    fs.Close();
                    //found file on disk, add to mem store:
                    CommitToMemStore(savePath, loadedObj);
                    return loadedObj;
                }
                else
                {
                    CommitToMemStore(savePath, default(T));
                    return default(T);
                }
            }
        }

        private bool TryLoadFromMemStore(string savePath, out object obj)
        {
            if (memStore.ContainsKey(savePath))
            {
                obj = memStore[savePath];
                return true;
            }
            else
            {
                obj = null;
                return false;
            }
        }

        private void CommitToMemStore(string savePath, object obj)
        {
            if (memStore.ContainsKey(savePath))
            {
                memStore[savePath] = obj;
            }
            else
            {
                memStore.Add(savePath, obj);
            }
        }

        private void WipeAllMemortInPersistentDataPath()
        {
            DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
            dataDir.Delete(true);
        }

        public void UpdatedModels(string path)
        {
            pathsToBeUpdated.Add(path);
            Debug.Log("TBD --- make this work on a timer");
            PushToDisk(path, memStore[path]);
        }

        public void PushToDisk(String savePath, object obj)
        {
            BinaryFormatter bf = BinaryFormaterWithSurrogates.GetBFInstance();
            FileStream fs = File.Create(Path.Combine(Application.persistentDataPath, savePath));
            bf.Serialize(fs, obj);
            fs.Close();
        }

        public void Dispose()
        {
            
        }

        public void Init()
        {
        }
    }
}