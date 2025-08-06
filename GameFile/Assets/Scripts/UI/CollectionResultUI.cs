using UnityEngine;
using UnityEngine.UI;

public class CollectionResultUI : MonoBehaviour
{
    [SerializeField] public GameObject CollectionResultPanel;
    [SerializeField] public Text ItemNameText;
    [SerializeField] public Text ItemCountText; 
    [SerializeField] public Text ItemCountBGText; 
    [SerializeField] public Sprite NImage;
    [SerializeField] public Sprite RImage;
    [SerializeField] public Sprite SRImage;
    [SerializeField] public Sprite SSRImage;
    [SerializeField] public Image ItemImage;
    [SerializeField] public Image ItemImageBG;
    [SerializeField] public Image ItemRarityImage;
    [SerializeField] public Image ItemRarityImageBG;
    [SerializeField] public GameObject CollectionResultCanvas;
    
    public void ShowCollectionResultPanel()
    {
        SoundManager.instance.PlaySE(0);
        CollectionResultPanel.SetActive(true);
        // CollectionResultCanvas.transform.SetAsLastSibling();
    }

    public void HideCollectionResultPanel()
    {
        CollectionResultPanel.SetActive(false);
    }

    public void ShowCharacter(OwnedCharacterData ownedCharacterData)
    {
        // キャラ名と画像を設定
        ItemNameText.text = ownedCharacterData.characterData.characterName;
        ItemImage.sprite = ownedCharacterData.characterData.characterImage;
        ItemImageBG.sprite = ownedCharacterData.characterData.characterImage;

        // レアリティ枠を設定
        Sprite raritySprite = GetRaritySprite(ownedCharacterData.characterData.rarity);
        ItemRarityImage.sprite = raritySprite;
        ItemRarityImageBG.sprite = raritySprite;
        Debug.Log($"現在の所持数: {ownedCharacterData.count}");
        ItemCountText.text = $"所持数：{ownedCharacterData.count}";
        ItemCountBGText.text = $"所持数：{ownedCharacterData.count}";


        // パネルを表示
        ShowCollectionResultPanel();
    }

    private Sprite GetRaritySprite(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.N: return NImage;
            case Rarity.R: return RImage;
            case Rarity.SR: return SRImage;
            case Rarity.SSR: return SSRImage;
            default: return null;
        }
    }
}
