using System.Collections.Generic;
using UnityEngine;

public class PlayerCollection : MonoBehaviour
{

    public List<OwnedCharacterData> ownedCharacters = new List<OwnedCharacterData>();

    // 所持キャラクターの数を返すメソッド
    public int GetOwnedCharacterCount(CharacterData characterData)
    {
        OwnedCharacterData ownedCharacter = ownedCharacters.Find(c => c.characterData.characterId == characterData.characterId);
        return ownedCharacter != null ? ownedCharacter.count : 0; // 所持していなければ0を返す
    }

    // 所持キャラクターリストを初期化
    public void Initialize(CollectionList collectionList)
    {
        ownedCharacters.Clear();
        foreach (var character in collectionList.characterDatas)
        {
            ownedCharacters.Add(new OwnedCharacterData(character));
        }
    }

    // 所持キャラクターを取得
    public OwnedCharacterData GetOwnedCharacter(string characterId)
    {
        return ownedCharacters.Find(c => c.characterData.characterId == characterId);
    }

    // 所持キャラクターを追加するメソッド
    public void AddOwnedCharacter(OwnedCharacterData ownedCharacterData)
    {
        ownedCharacters.Add(ownedCharacterData);
    }

    // --- セーブデータ連携 ---
    public void ApplySaveData(CollectionSaveData data, CollectionList collectionList)
    {
        if (data == null || data.ownedCharacters == null) return;
        ownedCharacters.Clear();

        foreach (var savedCharacter in data.ownedCharacters)
        {
            var characterData = System.Array.Find(collectionList.characterDatas, c => c.characterId == savedCharacter.characterId);
            if (characterData != null)
            {
                ownedCharacters.Add(new OwnedCharacterData(characterData, savedCharacter.count));
            }
        }
    }

    public void FillSaveData(CollectionSaveData data)
    {
        data.ownedCharacters = new List<CharacterSaveData>();
        foreach (var owned in ownedCharacters)
        {
            data.ownedCharacters.Add(new CharacterSaveData()
            {
                characterId = owned.characterData.characterId,
                count = owned.count
            });
        }
    }
}