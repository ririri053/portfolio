using UnityEngine;

[CreateAssetMenu(fileName = "CollectionList", menuName = "Create/CollectionList")]
public class CollectionList : ScriptableObject
{
    [SerializeField] public CharacterData[] characterDatas;
    [SerializeField] public Sprite Nicon;
    [SerializeField] public Sprite Ricon;
    [SerializeField] public Sprite SRicon;
    [SerializeField] public Sprite SSRicon;
}
