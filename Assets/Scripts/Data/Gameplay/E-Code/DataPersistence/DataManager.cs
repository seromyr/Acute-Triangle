using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager data;

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
    
    private void SingletonizeDataManager()
    {
        if(data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
            Debug.Log("Data Manager created");
        }
        else if(data != this)
        {
            Destroy(gameObject);
        }
    }

    public void UnlockLevel()
    {

    }

    public void SaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +"/SaveData.dat");

        Data levelData = new Data
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

    public void LoadFile()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);

            Data unlockData = (Data)bf.Deserialize(file);
        
            file.Close();
        }

    }

    public void EraseFile()
    {

    }

}

[Serializable]
class Data
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