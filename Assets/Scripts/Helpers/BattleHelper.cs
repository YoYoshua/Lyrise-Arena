using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleHelper
{
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
