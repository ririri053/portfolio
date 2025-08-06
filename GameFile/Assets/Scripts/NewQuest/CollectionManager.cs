using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    [SerializeField] public CollectionList collectionList;
    [SerializeField] public GameObject collectionItemPrefab;
    [SerializeField] public Transform ItemContent;
    [SerializeField] private CollectionResultUI resultUI; 
    [SerializeField] private PlayerCollection playerCollection;

    // すべてのCollectionItemをリストで管理
    public List<CollectionItem> itemList = new List<CollectionItem>();

    public void LoadDataAndRefreshUI(CollectionSaveData data)
    {
        if (data != null)
        {
            playerCollection.ApplySaveData(data, collectionList);
        }
        RefreshCollectionUI();
    }

    public void RefreshCollectionUI()
    {
        itemList.Clear();
        foreach (Transform child in ItemContent)
        {
            Destroy(child.gameObject);
        }

        //  ApplySaveData済みであれば、再初期化しない
        if (playerCollection.ownedCharacters == null || playerCollection.ownedCharacters.Count == 0)
        {
            playerCollection.Initialize(collectionList);
        }

        var sortedList = collectionList.characterDatas
            .OrderBy(data => int.Parse(data.characterId))
            .ToArray();

        foreach (var data in sortedList)
        {
            GameObject obj = Instantiate(collectionItemPrefab, ItemContent);
            CollectionItem item = obj.GetComponent<CollectionItem>();

            OwnedCharacterData ownedData = playerCollection.GetOwnedCharacter(data.characterId);

            Sprite raritySprite = GetRaritySprite(data.rarity);
            item.Setup(ownedData, raritySprite, resultUI, playerCollection);

            if (ownedData.count > 0)
                item.SetActive();
            else
                item.SetInactive();

            itemList.Add(item);
        }
    }

    private Sprite GetRaritySprite(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.N: return collectionList.Nicon;
            case Rarity.R: return collectionList.Ricon;
            case Rarity.SR: return collectionList.SRicon;
            case Rarity.SSR: return collectionList.SSRicon;
            default: return null;
        }
    }

    // キャラIDで該当アイテムだけカラーにする
    public void SetItemActive(string characterId)
    {
        var item = itemList.Find(i => i.characterData.characterId == characterId);
        if (item != null)
        {
            item.SetActive();
        }
    }

    public void CollectionLoadSave()
    {
        var collectionData = new CollectionSaveData();
        playerCollection.FillSaveData(collectionData);
        SaveManager.SaveCollection(collectionData);
    }
}