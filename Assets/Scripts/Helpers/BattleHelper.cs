using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleHelper
{
    /// <summary>
    /// Calculates skill power based on its type and caster's Elemental Affinity
    /// </summary>
    /// <param name="skill">Skill which power is being calculated</param>
    /// <param name="caster">Caster of the skill</param>
    /// <returns>Calculated skill power</returns>
    public static int CalculateSkillPower(Skill skill, Creature caster)
    {
        // Skill aspect is based on first attached orb
        Aspect skillAspect = skill.AttachedOrbs[0].Aspect;
        float casterAffinity = caster.ElementalAffinity[skillAspect];

        if (skill.SkillType == SkillType.Offensive)
        {
            return Mathf.RoundToInt((skill.Power + caster.OffensivePower) * casterAffinity); 
        }
        else if(skill.SkillType == SkillType.Defensive)
        {
            return Mathf.RoundToInt((skill.Power + caster.DefensivePower) * casterAffinity);
        }
        else
        {
            Debug.Log("Incorrect skill!");
            return 0;
        }
    }
}
