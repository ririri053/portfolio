using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private GameObject TitlePanel;
    [SerializeField] private GameMainController gameMainController;
    [SerializeField] private ConfirmUI confirmUI;


    public void ShowTitlePanel() => TitlePanel.SetActive(true);
    public void HideTitlePanel() => TitlePanel.SetActive(false);

    public void Reset()
    {
        SaveManager.ResetAllSaveData();
        gameMainController.StartSetting();
        confirmUI.HideDelConfirmPanel();
    }
}