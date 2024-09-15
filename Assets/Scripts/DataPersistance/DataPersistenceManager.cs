using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistance.Data;
using Events;
using UnityEngine;

namespace DataPersistance
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        [SerializeField] private bool useEncryption;
        [SerializeField] private IDataPersistence[] dataPersistenceObjects;

        private GameData _gameData;
        private FileDataHandler _dataHandler;

        public static DataPersistenceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Data Persistence Manager in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            EventManager.StartListening(EventStrings.SceneLoaded, OnSceneLoaded);
            EventManager.StartListening(EventStrings.SceneLoaded, OnSceneChanged);
        }

        private void Start()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        // Beginning of the scene
        private void OnSceneLoaded()
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        // End of the scene
        private void OnSceneChanged()
        {
            SaveGame();
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load();

            if (_gameData == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                if (dataPersistenceObj.IsLoaded) { continue; }
                
                dataPersistenceObj.LoadData(_gameData);
                dataPersistenceObj.IsLoaded = true;
            }
        }

        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(_gameData);
            }

            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private void OnDestroy()
        {
            EventManager.StopListening(EventStrings.SceneLoaded, OnSceneLoaded);
        }

        private IDataPersistence[] FindAllDataPersistenceObjects()
        {
            // IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            //     .OfType<IDataPersistence>();
            //
            // return new List<IDataPersistence>(dataPersistenceObjects);
            var dataObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>().ToArray();
            
            return dataObjects;
        }
    }
}
