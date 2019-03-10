using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public int APCost { get; set; }
    public int Power { get; set; }
    public float SuccessChance { get; set; }
    public SkillType SkillType { get; set; }
    public SkillTarget SkillTarget { get; set; }
    public List<Orb> AttachedOrbs { get; set; }
}

public enum SkillType
{
    Offensive = 1,
    Defensive = 2
}

public enum SkillTarget
{
    Creature = 1,
    Caster = 2,
    Area = 3
}
