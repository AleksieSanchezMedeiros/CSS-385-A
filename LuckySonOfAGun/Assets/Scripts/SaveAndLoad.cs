using UnityEngine;
using System.IO;

public static class SaveAndLoad
{
    public static void SaveGame(PlayerMovement pm, PlayerShooting ps)
    {
        //the data to save
        SaveData currentData = new SaveData
        {
            playerX = pm.transform.position.x,
            playerY = pm.transform.position.y,
            playerAmmoCount = ps.ammoCount
        };

        //the data as a json
        string json = JsonUtility.ToJson(currentData);

        Debug.Log("Saving game to " + Application.persistentDataPath + "/LSOAGSave.json");
        //the place its being saved to
        File.WriteAllText(Application.persistentDataPath + "/LSOAGSave.json", json);

    }

    //load the saved data
    public static SaveData LoadGame()
    {
        //where the file should be
        string path = Application.persistentDataPath + "/LSOAGSave.json";

        //if there is a file load it
        if (File.Exists(path))
        {
            //convert json back to data
            string json = File.ReadAllText(path);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
            return loadedData;
        }
        // if no file, log error and return null for now, probably best to like reset the game or something
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
