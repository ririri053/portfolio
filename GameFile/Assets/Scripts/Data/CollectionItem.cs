using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UI;

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Image rarityIcon;
    [SerializeField] private Button button; // ←追加
    public CharacterData characterData;
    private PlayerCollection playerCollection;
    private CollectionResultUI resultUI;
    private FadeUI fadeUI;

    public void Setup(OwnedCharacterData ownedData, Sprite raritySprite, CollectionResultUI ui, PlayerCollection collection)
    {
        characterData = ownedData.characterData;
        resultUI = ui;
        playerCollection = collection;

        characterImage.sprite = characterData.characterImage;
        rarityIcon.sprite = raritySprite;

        AddClickEvent();
        SetInactive(); // 初期状態で非アクティブ
    }

    private void AddClickEvent()
    {
        if (button == null) button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        FadeUI.Instance.FadeOutAndHide();
        var latestOwned = playerCollection.GetOwnedCharacter(characterData.characterId);
        resultUI.ShowCharacter(latestOwned);
    }

    public void SetInactive()
    {
        characterImage.color = new Color(0, 0, 0, 1);
        rarityIcon.color = new Color(0, 0, 0, 1);
        if (button == null) button = GetComponent<Button>();
        if (button != null) button.interactable = false; // クリック不可
    }

    public void SetActive()
    {
        characterImage.color = Color.white;
        rarityIcon.color = Color.white;
        if (button == null) button = GetComponent<Button>();
        if (button != null) button.interactable = true; // クリック可
    }
}