using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class SkillEffectManager : MonoBehaviour
    {
        private Dictionary<SkillType, SkillEffect> activeEffects;

        private void Awake()
        {
            activeEffects = new Dictionary<SkillType, SkillEffect>();
        }

        public void AddEffect(SkillType type, SkillEffect effect)
        {
            if (activeEffects.ContainsKey(type))
            {
                activeEffects[type] = effect;
            }
            else
            {
                activeEffects.Add(type, effect);
            }
        }

        public bool HasEffect(SkillType type)
        {
            return activeEffects.ContainsKey(type);
        }

        public SkillEffect GetEffect(SkillType type)
        {
            return activeEffects.TryGetValue(type, out var effect) ? effect : null;
        }

        public void ClearEffects()
        {
            activeEffects.Clear();
        }

        public class EffectProcessResult
        {
            public int TotalDamage;
            public List<SkillType> RemovedTypes;
        }

        public EffectProcessResult ProcessEffects()
        {
            List<SkillType> effectsToRemove = new List<SkillType>();
            int totalDamage = 0;

            foreach (var effect in activeEffects)
            {
                int damage = effect.Value.ApplyEffect();
                totalDamage += damage;

                effect.Value.DecreaseDuration();
                if (effect.Value.Duration <= 0)
                {   
                    effectsToRemove.Add(effect.Key);
                }
            }

            foreach (var type in effectsToRemove)
            {
                activeEffects.Remove(type);
            }

            return new EffectProcessResult
            {
                TotalDamage = totalDamage,
                RemovedTypes = effectsToRemove
            };
        }
    }
}