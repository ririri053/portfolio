using UnityEngine;
using UnityEngine.UI;

public class GachaUI : MonoBehaviour
{
    [SerializeField] private GameObject GachaPanel;

    public void ShowachaPanel() => GachaPanel.SetActive(true);
    public void HideachaPanel() => GachaPanel.SetActive(false);
}
