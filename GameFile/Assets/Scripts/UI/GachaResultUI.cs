using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultUI : MonoBehaviour
{
    [SerializeField] private GameObject GachaResultPanel;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Image ItemImageBG;

    [SerializeField] private Text ItemNameText;
    [SerializeField] public Image ItemRarityImageBG;
    [SerializeField] public Sprite ItemRarityNImage;
    [SerializeField] public Sprite ItemRarityRImage;
    [SerializeField] public Sprite ItemRaritySRImage;
    [SerializeField] public Sprite ItemRaritySSRImage;
    [SerializeField] private Image RarityDisplayImage;

    [SerializeField] private GameObject GachaResultCanvas;

    /// <summary>
    /// ガチャ結果パネルを表示し、Canvasを一番前に持ってくる
    /// </summary>
    public void ShowGachaResultPanel()
    {
        SoundManager.instance.PlaySE(2); 
        GachaResultPanel.SetActive(true);
    }

    public void HideGachaResultPanel()
    {
        GachaResultPanel.SetActive(false);
    }

    /// <summary>
    /// レアリティに応じた画像だけ表示する
    /// </summary>
    public void SetRarityImage(Sprite rarityIcon)
    {
        RarityDisplayImage.gameObject.SetActive(true);
        ItemRarityImageBG.gameObject.SetActive(true);
        RarityDisplayImage.sprite = rarityIcon;
        ItemRarityImageBG.sprite = rarityIcon;
    }

    /// <summary>
    /// アイテム画像と名前をUIに設定
    /// </summary>
    public void SetItemUI(Sprite image, string name)
    {
        ItemImage.sprite = image;
        ItemImageBG.sprite = image;
        ItemNameText.text = name;
    }
}