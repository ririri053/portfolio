using UnityEngine;

public class CollectionUI : MonoBehaviour
{
    [SerializeField] private GameObject CollectionPanel;

    public void ShowCollectionPanel() => CollectionPanel.SetActive(true);
    public void HideCollectionPanel() => CollectionPanel.SetActive(false);
}