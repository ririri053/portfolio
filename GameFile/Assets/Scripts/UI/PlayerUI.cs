using UnityEngine;
using UnityEngine.UI;

    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] public Text hpText;
        [SerializeField] public Text mpText;
        [SerializeField] public Text levelText;
        [SerializeField] public Text expText;
        [SerializeField] public GameObject PlayerPanel;
        [SerializeField] public PlayerCollection playerCollection;

        public void ShowPlayerPanel() => PlayerPanel.SetActive(true);
        public void HidePlayerPanel() => PlayerPanel.SetActive(false);

    private string FormatWithTransparentZeros(int value)
    {
        return value.ToString().PadLeft(3, '_').Replace("_", "<color=#00000000>0</color>");
    }

    public void UpdateHP(int hp) => hpText.text = "HP : " + FormatWithTransparentZeros(hp);
    public void UpdateMP(int mp) => mpText.text = "MP : " + FormatWithTransparentZeros(mp);
}