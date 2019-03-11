using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature
{
    public int HealthPoints { get; set; }
    public int ActionPoints { get; set; }
    public int OffensivePower { get; set; }
    public int DefensivePower { get; set; }
    public Dictionary<Aspect, float> ElementalAffinity { get; set; }

    public Creature()
    {
        ElementalAffinity = new Dictionary<Aspect, float>
        {
                { Aspect.Fire, 0.2f },
                { Aspect.Earth, 0.2f },
                { Aspect.Light, 0.2f },
                { Aspect.Wind, 0.2f },
                { Aspect.Life, 0.2f },
                { Aspect.Shadow, 0.2f },
                { Aspect.Void, 0.2f },
                { Aspect.Water, 0.2f }
        };
    }

    public Aspect GetDominatingAspect()
    {
        float strongestElementalAffinity = Mathf.Max(ElementalAffinity.Values.ToArray());
        return ElementalAffinity.Where(e => e.Value == strongestElementalAffinity).FirstOrDefault().Key;
    }
}
