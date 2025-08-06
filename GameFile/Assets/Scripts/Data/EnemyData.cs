using UnityEngine;
using System.Collections.Generic;
using Battle;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/EnemyData")]
    public class EnemyData : ScriptableObject
{
    [SerializeField] public string Name; // 名前を修正
    [SerializeField] public int HP;      // HP を修正
    [SerializeField] public int Attack;
    [SerializeField] public int Defense;
    [SerializeField] public int EXP;
    [SerializeField] public int GachaPoint;
    [SerializeField] public Sprite EnemyImage; // 敵の画像を修正
    [SerializeField] public SkillType FirstSkill;    // 最初に使うスキル（null可）
    [SerializeField] public SkillType TriggerSkill;  // HPトリガーで使うスキル（null可）
    [SerializeField] public float TriggerHPRate = 0.5f; // 例: 0.5fでHP50%以下
    [SerializeField] public SkillType NormalAttackSkill = SkillType.NormalAttack;
    [SerializeField] public string EnemyStartText; // スキルテキストを追加
    [SerializeField] public string EnemyEndWinText; // スキルテキストを追加
    [SerializeField] public string EnemyEndLoseText; // スキルテキストを追加
    [SerializeField] public string EnemyTalkText; // スキルテキストを追加
}