using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Create/CharacterData")]
public class CharacterData : ScriptableObject //ScriptableObjectを継承
{
    [SerializeField] public string characterId; // 追加：キャラクターの一意のID
    [SerializeField] public string characterName; // キャラクター名
    [SerializeField] public Sprite characterImage; // キャラクター画像
    [SerializeField] public Rarity rarity; // レアリティ
    [SerializeField] public int count;
}

public enum Rarity
{
    N,
    R,
    SR,
    SSR
}