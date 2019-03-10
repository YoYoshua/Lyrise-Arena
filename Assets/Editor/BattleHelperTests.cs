using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHelperTests
{
    private Skill offensiveSkill;
    private Skill defensiveSkill;
    private Creature testCreature;

    [SetUp]
    public void Init()
    {
        offensiveSkill = new Skill
        {
            APCost = 5,
            Power = 10,
            SuccessChance = 0.7f,
            SkillType = SkillType.Offensive,
            SkillTarget = SkillTarget.Creature,
            AttachedOrbs = new List<Orb>
            {
                new Orb { Aspect = Aspect.Fire, Level = 1 },
                new Orb { Aspect = Aspect.Earth, Level = 1 }
            }
        };

        defensiveSkill = new Skill
        {
            APCost = 3,
            Power = 12,
            SuccessChance = 0.9f,
            SkillType = SkillType.Defensive,
            SkillTarget = SkillTarget.Caster,
            AttachedOrbs = new List<Orb>
            {
                new Orb { Aspect = Aspect.Life, Level = 1 },
                new Orb { Aspect = Aspect.Light, Level = 1 }
            }
        };

        testCreature = new Creature
        {
            HealthPoints = 100,
            ActionPoints = 7,
            OffensivePower = 12,
            DefensivePower = 20,
            ElementalAffinity = new Dictionary<Aspect, float>
            {
                { Aspect.Fire, 0.8f },
                { Aspect.Earth, 0.5f },
                { Aspect.Light, 0.3f },
                { Aspect.Wind, 0.2f },
                { Aspect.Life, 0.2f },
                { Aspect.Shadow, 0.2f },
                { Aspect.Void, 0.2f },
                { Aspect.Water, 0.2f }
            }
        };
    }

    [Test]
    public void CalculateSkillPower_OffensiveSkill_Test()
    {
        // Arrange
        Creature creature = testCreature;
        Skill skill = offensiveSkill;

        // Act
        int result = BattleHelper.CalculateSkillPower(skill, creature);

        // Assert
        Assert.AreEqual(result, 18);
    }

    [Test]
    public void CalculateSkillPower_DefensiveSkill_Test()
    {
        // Arrange
        Creature creature = testCreature;
        Skill skill = defensiveSkill;

        // Act
        int result = BattleHelper.CalculateSkillPower(skill, creature);

        // Assert
        Assert.AreEqual(result, 6);
    }
}
