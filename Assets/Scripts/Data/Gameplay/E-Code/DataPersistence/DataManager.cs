using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    //singleton Variable
    public static DataManager gameData;

    bool level0L;
    bool level1L;
    bool level2L;
    bool level3L;
    bool level4L;
    bool level5L;
    bool level6L;
    bool level7L;
    bool level8L;
    bool level9L;
    bool level10L;

    // Start is called before the first frame update
    void Awake()
    {
        SingletonizeDataManager();
    }
    
    //singletonize the Data manager
    private void SingletonizeDataManager()
    {
        if(gameData == null)
        {
            DontDestroyOnLoad(gameObject);
            gameData = this;
            Debug.Log("Data Manager created");
        }
        else if(gameData != this)
        {
            Destroy(gameObject);
        }
    }

    //Unlock a specific level
    public void UnlockLevel(int num)
    {
        switch (num)
        {
            case 0:
                level0L = true;
                break;
            case 1:
                level1L = true;
                break;
            case 2:
                level2L = true;
                break;
            case 3:
                level3L = true;
                break;
            case 4:
                level4L = true;
                break;
            case 5:
                level5L = true;
                break;
            case 6:
                level6L = true;
                break;
            case 7:
                level7L = true;
                break;
            case 8:
                level8L = true;
                break;
            case 9:
                level9L = true;
                break;
            case 10:
                level10L = true;
                break;
            default:
                level0L = true;
                break;
        }
    }

    //get the status of a level
    public bool GetLevelLockStatus(int levelNum)
    {
        switch (levelNum)
        {
            case 0:
                return level0L;
            case 1:
                return level1L;
            case 2:
                return level2L;
            case 3:
                return level3L;
            case 4:
                return level4L;
            case 5:
                return level5L;
            case 6:
                return level6L;
            case 7:
                return level7L;
            case 8:
                return level8L;
            case 9:
                return level9L;
            case 10:
                return level10L;
            default:
                return level0L;
        }
    }

    //save level data
    public void SaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +"/SaveData.dat");

        LevelData levelData = new LevelData
        {
            level_0_Access = level0L,
            level_1_Access = level1L,
            level_2_Access = level2L,
            level_3_Access = level3L,
            level_4_Access = level4L,
            level_5_Access = level5L,
            level_6_Access = level6L,
            level_7_Access = level7L,
            level_8_Access = level8L,
            level_9_Access = level9L,
            level_10_Access = level10L
        };

        bf.Serialize(file, levelData);
        file.Close();
    }

    //load level data
    public void LoadFile()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);

            LevelData unlockData = (LevelData)bf.Deserialize(file);
            file.Close();

            level0L = unlockData.level_0_Access;
            level1L = unlockData.level_1_Access;
            level2L = unlockData.level_2_Access;
            level3L = unlockData.level_3_Access;
            level4L = unlockData.level_4_Access;
            level5L = unlockData.level_5_Access;
            level6L = unlockData.level_6_Access;
            level7L = unlockData.level_7_Access;
            level8L = unlockData.level_2_Access;
            level9L = unlockData.level_9_Access;
            level10L = unlockData.level_10_Access;
        }
    }

    //Erase file data
    public void EraseFile()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
            File.Delete(Application.persistentDataPath + "SaveData.dat");
    }

    //reset all levels
    public void ResetLevelLocks()
    {
        level0L = false;
        level1L = false;
        level2L = false;
        level3L = false;
        level4L = false;
        level5L = false;
        level6L = false;
        level7L = false;
        level8L = false;
        level9L = false;
        level10L = false;
    }
}

//serializable data
[Serializable]
class LevelData
{
    public bool level_0_Access;
    public bool level_1_Access;
    public bool level_2_Access;
    public bool level_3_Access;
    public bool level_4_Access;
    public bool level_5_Access;
    public bool level_6_Access;
    public bool level_7_Access;
    public bool level_8_Access;
    public bool level_9_Access;
    public bool level_10_Access;
}