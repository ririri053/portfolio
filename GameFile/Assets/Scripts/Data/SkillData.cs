using UnityEngine;
using Battle;

[CreateAssetMenu(fileName = "SkillData", menuName = "Battle/SkillData")]
public class SkillData : ScriptableObject
{
    public SkillType Type;
    public string Name;
    public string Description;
    public int MpCost;
    // public Sprite Icon;
    public GameObject EffectPrefab;
}
