using UnityEngine;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private GameObject DialogPanel;

    public void ShowDialogPanel() => DialogPanel.SetActive(true);
    public void HideDialogPanel() => DialogPanel.SetActive(false);
}
