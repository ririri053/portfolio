using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    public string characterId;
    public int count;
}

[System.Serializable]
public class PlayerSaveData
{
    public int playerLevel;
    public int playerAT;
    public int playerMP;
    public int playerNextLevel;
    public int LevelUpEXP;
    public int playerHP;
}

[System.Serializable]
public class EXPSaveData
{
    public int EXP;
}

[System.Serializable]
public class QuestUnlockedSaveData
{
    public bool[] questUnlocked = new bool[3];
}

[System.Serializable]
public class GachaPointSaveData
{
    public int gachaPoint;
}

[System.Serializable]
public class SystemSaveData
{
    public float seVolume;
    public float bgmVolume;
}

[System.Serializable]
public class PlayerNameSaveData
{
    public string playerName;
}

[System.Serializable]
public class CollectionSaveData
{
    public List<string> ownedItemIds = new List<string>();
    public List<CharacterSaveData> ownedCharacters = new List<CharacterSaveData>();
}

public class SaveManager : MonoBehaviour
{
    // パス定義
    private static string PlayerPath => Path.Combine(Application.persistentDataPath, "player.json");
    private static string SystemPath => Path.Combine(Application.persistentDataPath, "system.json");
    private static string NamePath => Path.Combine(Application.persistentDataPath, "name.json");
    private static string CollectionPath => Path.Combine(Application.persistentDataPath, "collection.json");
    private static string GachaPointPath => Path.Combine(Application.persistentDataPath, "gachaPoint.json");
    private static string QuestUnlockedPath => Path.Combine(Application.persistentDataPath, "questUnlocked.json");
    public static string EXPPath => Path.Combine(Application.persistentDataPath, "exp.json");

    // ----------- 保存 -----------
    public static void SavePlayer(PlayerSaveData data)
    {
        File.WriteAllText(PlayerPath, JsonUtility.ToJson(data, true));
    }

    public static void SaveSystem(SystemSaveData data)
    {
        File.WriteAllText(SystemPath, JsonUtility.ToJson(data, true));
    }

    public static void SavePlayerName(PlayerNameSaveData data)
    {
        File.WriteAllText(NamePath, JsonUtility.ToJson(data, true));
    }

    public static void SaveCollection(CollectionSaveData data)
    {
        File.WriteAllText(CollectionPath, JsonUtility.ToJson(data, true));
    }
    public static void SaveGachaPoint(GachaPointSaveData data)
    {
        File.WriteAllText(GachaPointPath, JsonUtility.ToJson(data, true));
    }

    public static void SaveQuestUnlocked(QuestUnlockedSaveData data)
    {
        File.WriteAllText(QuestUnlockedPath, JsonUtility.ToJson(data, true));
    }

    public static void SaveEXP(EXPSaveData data)
    {
        File.WriteAllText(EXPPath, JsonUtility.ToJson(data, true));
    }

    // ----------- 読み込み -----------
    public static PlayerSaveData LoadPlayer()
    {
        return File.Exists(PlayerPath)
            ? JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(PlayerPath))
            : new PlayerSaveData();
    }

    public static SystemSaveData LoadSystem()
    {
        return File.Exists(SystemPath)
            ? JsonUtility.FromJson<SystemSaveData>(File.ReadAllText(SystemPath))
            : new SystemSaveData();
    }

    public static PlayerNameSaveData LoadPlayerName()
    {
        return File.Exists(NamePath)
            ? JsonUtility.FromJson<PlayerNameSaveData>(File.ReadAllText(NamePath))
            : new PlayerNameSaveData();
    }

    public static CollectionSaveData LoadCollection()
    {
        return File.Exists(CollectionPath)
            ? JsonUtility.FromJson<CollectionSaveData>(File.ReadAllText(CollectionPath))
            : new CollectionSaveData();
    }

    public static GachaPointSaveData LoadGachaPoint()
    {
        return File.Exists(GachaPointPath)
            ? JsonUtility.FromJson<GachaPointSaveData>(File.ReadAllText(GachaPointPath))
            : new GachaPointSaveData();
    }

    public static QuestUnlockedSaveData LoadQuestUnlocked()
    {
        return File.Exists(QuestUnlockedPath)
            ? JsonUtility.FromJson<QuestUnlockedSaveData>(File.ReadAllText(QuestUnlockedPath))
            : new QuestUnlockedSaveData();
    }

    public static EXPSaveData LoadEXP()
    {
        return File.Exists(EXPPath)
            ? JsonUtility.FromJson<EXPSaveData>(File.ReadAllText(EXPPath))
            : new EXPSaveData();
    }

    // ----------- リセット -----------
        public static void ResetAllSaveData()
    {
        string[] allSavePaths = new string[]
        {
            PlayerPath,
            SystemPath,
            NamePath,
            CollectionPath,
            GachaPointPath,
            QuestUnlockedPath,
            EXPPath
        };

        foreach (string path in allSavePaths)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[SaveManager] 削除: {Path.GetFileName(path)}");
            }
        }

        // PlayerPrefs も削除（音量などの一部データで使っている可能性がある場合）
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[SaveManager] PlayerPrefs をリセットしました。");
    }
}