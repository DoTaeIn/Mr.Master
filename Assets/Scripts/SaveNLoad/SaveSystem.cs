    using System;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Text.Json;
    
    
    [Serializable]
    public class PlayerData
    {
        public Vector2 playerPos;
        public Quaternion playerRot;
        public float playerMoney;
    }

    [Serializable]
    public class DrinkManagerData
    {
        public List<Drink> drinks = new List<Drink>();
        public List<Cocktail> Cocktails = new List<Cocktail>();
    }

    [Serializable]
    public class StorageManagerData
    {
        public int dataSaveIndex;
        public Dictionary<DrinkSO, List<DrinkSO>> drinks = new Dictionary<DrinkSO, List<DrinkSO>>();
    }

    [Serializable]
    public class GameManagerData
    {
        public int dayofWeekIndex;
        public int month;
        public int day;
        public float currTime;
        public bool isAfternoon;
        public string currQuestName;
        public string currQuestDescription;
    }

    [Serializable]
    public class saveData
    {
        public int dataSaveIndex;
        public PlayerData playerData = new PlayerData();
        public GameManagerData gameManagerData = new GameManagerData();
        public DrinkManagerData drinkManagerData = new DrinkManagerData();
    }

    [Serializable]
    public class CombinedSaveData
    {
        public saveData mainData;  // Data from JsonUtility
        public StorageManagerData storageData;  // Data from Newtonsoft.Json
    }

    
    public class SaveSystem : MonoBehaviour
    {
        PlayerCTRL playerCTRL;
        DrinkManager drinkManager;
        Gamemanager gamemanager;
        StorageManager storageManager;
        
        public string dataPath = Application.dataPath + "/saveData.json";


        private void Awake()
        {
            setManagers();
        }

        private void setManagers()
        {
            playerCTRL = FindFirstObjectByType<PlayerCTRL>();
            drinkManager = FindFirstObjectByType<DrinkManager>();
            gamemanager = FindFirstObjectByType<Gamemanager>();
            storageManager = FindFirstObjectByType<StorageManager>();
        }


        private saveData createSaveData(int a)
        {
            saveData data = new saveData();

            data.dataSaveIndex = a;
            
            // Player Data
            data.playerData.playerPos = playerCTRL.transform.position;
            data.playerData.playerRot = playerCTRL.transform.rotation;
            data.playerData.playerMoney = playerCTRL.money;

            // Game Manager Data
            data.gameManagerData.dayofWeekIndex = gamemanager.dayofWeekIndex;
            data.gameManagerData.month = gamemanager.month;
            data.gameManagerData.day = gamemanager.day;
            data.gameManagerData.currTime = gamemanager.currentTime;
            data.gameManagerData.isAfternoon = gamemanager.isAfternoon;
            data.gameManagerData.currQuestName = gamemanager.questTitle;
            data.gameManagerData.currQuestDescription = gamemanager.questDescription;

            // Storage Manager Data
            drinkManager.drinks.Add(new Drink(1, "Test Drink", 10f, 5f, 100f, Color.white, new List<string> { "Sweet", "Fruity" })); ;
            data.drinkManagerData.drinks = drinkManager.drinks;
            data.drinkManagerData.Cocktails = drinkManager.cocktails;

            return data;
        }

        private StorageManagerData createStorageManagerData(int a)
        {
            StorageManagerData data = new StorageManagerData();
            data.dataSaveIndex = a;
            data.drinks = storageManager.drinks;
            
            return data;
        }


        public void saveDataToFile(int saveIndex)
        {
            setManagers();
            saveData JUdata = createSaveData(saveIndex);
            StorageManagerData storageManagerData = createStorageManagerData(saveIndex);
            string JUjson = JsonUtility.ToJson(JUdata, true);
            string NJjson = JsonConvert.SerializeObject(storageManagerData, Formatting.Indented);
            JUjson += "\n" + "|" + "\n" + NJjson;
            
            File.WriteAllText(Application.dataPath + $"/saveData{saveIndex}.json", JUjson);
            
        }

        public void loadSaveDataFromFile(int saveIndex)
        {
            if (File.Exists(Application.dataPath + $"/saveData{saveIndex}.json"))
            {
                string json = File.ReadAllText(Application.dataPath + $"/saveData{saveIndex}.json"); // Read JSON from file
                string[] dataSplit = json.Split("|");
                saveData data = JsonUtility.FromJson<saveData>(dataSplit[0]); // Convert back to object
                StorageManagerData storageManagerData = JsonConvert.DeserializeObject<StorageManagerData>(dataSplit[1]);
                Debug.Log("Game loaded from " + dataPath);
                setManagers();

                playerCTRL.transform.position = data.playerData.playerPos;
                playerCTRL.transform.rotation = data.playerData.playerRot;
                playerCTRL.money = data.playerData.playerMoney;

                // Game Manager Data
                gamemanager.dayofWeekIndex = data.gameManagerData.dayofWeekIndex;
                gamemanager.month = data.gameManagerData.month;
                gamemanager.day = data.gameManagerData.day;
                gamemanager.currentTime = data.gameManagerData.currTime;
                gamemanager.questTitle = data.gameManagerData.currQuestName;
                gamemanager.questDescription = data.gameManagerData.currQuestDescription;
                gamemanager.isAfternoon = data.gameManagerData.isAfternoon;
                
                drinkManager.drinks = data.drinkManagerData.drinks;
                drinkManager.cocktails = data.drinkManagerData.Cocktails;
                
                
            }
            else
            {
                Debug.LogWarning("No save file found!");
            }
        }
        
        
        
    }