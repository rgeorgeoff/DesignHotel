using System;
using BuildControllers;
using Managers.SceneManagers;
using Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameMainManager : MonoBehaviour
    {
        public static GameMainManager Instance;

        public IDataStoreManager DataStoreManager { get; private set; }

        public EventManager EventManager { get; private set; }

        public RemoteConfig RemoteConfig { get; private set; }

        public RoomSelectionManager RoomSelectionManager { get; private set; }

        public SceneManagerBase CurrentSceneManager { get; private set; }

        public AssetBundleManager AssetBundleManager { get; private set; }
        
        public ObjectPoolManager ObjectPoolManager { get; private set; }

        public bool ClearAllDataOnStart = false;

        //This is more or less a test class, should require nothing else to be init already TBD
        public DefaultConfigs DefaultConfigs = new DefaultConfigs();

        private GameState _gameState;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeComponents();
                RegisterEvents(); // must register after EventManager is init
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeComponents()
        {
            //first we load store reading module so we can get states for components.
            DataStoreManager = new DefaultDataStoreManger(ClearAllDataOnStart);
            EventManager = new EventManager();
            AssetBundleManager = new AssetBundleManager();
            ObjectPoolManager = new ObjectPoolManager();
            RemoteConfig = new RemoteConfig();
            RoomSelectionManager = new RoomSelectionManager();
        }

        //This does fire the first time a scene is loaded too since we subscribe to this on awake
        void OnSceneChange(Scene s, LoadSceneMode lsm)
        {
            CurrentSceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagerBase>();
            if(CurrentSceneManager != null)
                CurrentSceneManager.Init();
        }

        void RegisterEvents()
        {
            SceneManager.sceneLoaded += OnSceneChange;
            //GameMainManager.instance.eventManager.StartListening(EventManager.EventName.NEW_SCENE.ToString(), OnChangeSceneComplete);
        }

        void OnChangeSceneComplete(object sender, EventArgs e)
        {
        }
    }
}