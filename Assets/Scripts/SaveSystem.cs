using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    public SaveData currentSaveData;
    private int currentSaveIndex;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
        InitializeNewSave();
    }


    #region Utility Functions
        public static void SaveToJson(int _saveIndex)
        {
            string saveData = JsonUtility.ToJson(instance.currentSaveData);
            string filePath = Application.persistentDataPath + $"/SaveData{_saveIndex}.json";
            System.IO.File.WriteAllText(filePath, saveData);
            Debug.Log($"Saved Game As: {filePath}");
        }

        public static void LoadFromJson(int _saveIndex)
        {
            string filePath = Application.persistentDataPath + $"/SaveData{_saveIndex}.json";
            if (System.IO.File.Exists(filePath))
            {
                string saveData = System.IO.File.ReadAllText(filePath);

                instance.currentSaveData = JsonUtility.FromJson<SaveData>(saveData);
                instance.currentSaveIndex = _saveIndex;
                Debug.Log("Save Data Loaded.");
            }
            else
            {
                Debug.Log($"Save Data Failed To Be Loaded At: {filePath}. Does this save file exist?");
            }

        }

        public static void InitializeNewSave()
        {
            instance.currentSaveIndex = FetchNextAvailableSaveIndex();
            instance.currentSaveData = new SaveData();
            SaveToJson(instance.currentSaveIndex);
        }

        public static SaveData FetchSaveData(int _saveIndex)
        {
            string filePath = Application.persistentDataPath + $"/SaveData{_saveIndex}.json";
            string saveData = System.IO.File.ReadAllText(filePath);

            return JsonUtility.FromJson<SaveData>(saveData);
        }
        public static int FetchNextAvailableSaveIndex()
        {
            bool noFileFound = true;
            int testedIndex = 0;
            while (noFileFound)
            {
                if (System.IO.File.Exists(Application.persistentDataPath + $"/SaveData{testedIndex}.json"))
                {
                    testedIndex++;
                }
                else
                {
                    noFileFound = false;
                }
            }
            return testedIndex;
        }
    #endregion
    #region Update Events
    private void OnEnable()
    {
        RelationshipManager.RelationshipChangeEvent += UpdateRelationshipStats;
    }
    private void OnDisable()
    {
        RelationshipManager.RelationshipChangeEvent -= UpdateRelationshipStats;
    }
    private void UpdateRelationshipStats(eCharacters _character, int _friendshipChange)
    {
        currentSaveData.relationshipData[(int)_character].friendship += _friendshipChange;
        for (int i = 0; i < (int)eCharacters.Terminator; i++)
        {
            Debug.Log($"{(eCharacters)i}, friendship: {currentSaveData.relationshipData[i].friendship}");
        }
    }
    #endregion

}

[System.Serializable]
public class SaveData
{
    [NamedArray(typeof(eCharacters))]public RelationshipInfromation[] relationshipData;

    public SaveData()
    {
        this.relationshipData = new RelationshipInfromation[(int)eCharacters.Terminator];
        for (int i = 0; i < (int)eCharacters.Terminator; i++)
        {
            this.relationshipData[i] = new RelationshipInfromation((eCharacters)i);
        }
    }
}
