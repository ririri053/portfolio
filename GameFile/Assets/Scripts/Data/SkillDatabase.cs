using UnityEngine;
using System.Collections.Generic;
using Battle;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Battle/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    public List<SkillData> skills;

    private Dictionary<SkillType, SkillData> skillDict;

    public void Init()
    {
        skillDict = new Dictionary<SkillType, SkillData>();
        foreach (var skill in skills)
        {
            skillDict[skill.Type] = skill;
        }
    }

    public SkillData Get(SkillType type)
    {
        if (skillDict == null) Init();
        return skillDict.TryGetValue(type, out var data) ? data : null;
    }
}

