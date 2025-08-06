using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class GachaManager : MonoBehaviour
{
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] public CollectionList collectionList;
    [SerializeField] public GachaResultUI gachaResultUI;
    [SerializeField] public Button GachaButton;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Text ItemNameText;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private PlayerCollection playerCollection;
    [SerializeField] private CollectionManager collectionManager; // ←追加
    [SerializeField] private ConfirmUI confirmUI;
    [SerializeField] private FadeUI fadeUI;
    private OwnedCharacterData ownedCharacterData;

    public void SingleGacha()
    {
        if (playerManager.currentPlayer.GachaPoint <= 0)
        {
            SoundManager.instance.PlaySE(4); 
            Debug.Log("ガチャポイントが足りません。");
            return;
        }
        SoundManager.instance.PlaySE(2); 
        fadeUI.FadeOutAndHide();
        playerManager.currentPlayer.GachaPoint--;

        // ガチャ処理
        CharacterData drawn = DrawCharacter();

        if (drawn == null)
        {
            Debug.Log("ガチャでキャラクターが引けませんでした。");
            return;
        }

        ownedCharacterData = playerCollection.GetOwnedCharacter(drawn.characterId);

        if (ownedCharacterData == null)
        {
            ownedCharacterData = new OwnedCharacterData(drawn, 1);
            playerCollection.AddOwnedCharacter(ownedCharacterData);
        }
        else
        {
            ownedCharacterData.count++;
            Debug.Log($"[Gacha] {drawn.characterId} の新しい所持数: {ownedCharacterData.count}");
        }
        // UI更新
        confirmUI.HideGachaConfirmPanel();
        UpdateGachaUI(ownedCharacterData);
        menuUI.PlayerSettings();

        // --- セーブ処理 ---
        GachaLoadSave();
        collectionManager.CollectionLoadSave();
        collectionManager.SetItemActive(drawn.characterId);
    }

    private CharacterData DrawCharacter()
    {
        float rand = Random.value;

        Rarity chosenRarity;
        if (rand < 0.05f) chosenRarity = Rarity.SSR;
        else if (rand < 0.15f) chosenRarity = Rarity.SR;
        else if (rand < 0.3f) chosenRarity = Rarity.R;
        else chosenRarity = Rarity.N;

        var candidates = new List<OwnedCharacterData>();
        foreach (var ownedData in playerCollection.ownedCharacters)
        {
            if (ownedData.characterData.rarity == chosenRarity)
                candidates.Add(ownedData);
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"選ばれたレアリティ {chosenRarity} のキャラクターが所持キャラクターリストにありません。");
            return null;
        }

        var chosenOwnedCharacter = candidates[Random.Range(0, candidates.Count)];
        return chosenOwnedCharacter.characterData;
    }

    private void UpdateGachaUI(OwnedCharacterData data)
    {
        gachaResultUI.SetItemUI(data.characterData.characterImage, data.characterData.characterName);

        Sprite rarityIcon = null;
        switch (data.characterData.rarity)
        {
            case Rarity.N:
                rarityIcon = gachaResultUI.ItemRarityNImage;
                break;
            case Rarity.R:
                rarityIcon = gachaResultUI.ItemRarityRImage;
                break;
            case Rarity.SR:
                rarityIcon = gachaResultUI.ItemRaritySRImage;
                break;
            case Rarity.SSR:
                rarityIcon = gachaResultUI.ItemRaritySSRImage;
                break;
        }

        gachaResultUI.SetRarityImage(rarityIcon);
        gachaResultUI.ShowGachaResultPanel();
    }

    public void FillSaveData(GachaPointSaveData data)
    {
        data.gachaPoint = playerManager.currentPlayer.GachaPoint;
    }

    public void ApplySaveData(GachaPointSaveData data)
    {
        playerManager.currentPlayer.GachaPoint = data.gachaPoint;
    }

    public void GachaLoadSave()
    {
        var gachaData = SaveManager.LoadGachaPoint();
        FillSaveData(gachaData);
        SaveManager.SaveGachaPoint(gachaData);
    }
}