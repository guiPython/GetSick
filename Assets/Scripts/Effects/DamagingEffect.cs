using System.Collections.Generic;
using System.Linq;

public class DamagingEffect : Effect
{
    public float damage;
    public List<string> curedBy;

    public DamagingEffect(string name, float damage, IEffectLifeTime lifetime, List<string> curedBy)
        : base(name, lifetime)
    {
        this.damage = damage;
        this.curedBy = curedBy ?? new List<string>();
    }

    public bool IsCuredBy(string remedyName)
    {
        return this.curedBy.Contains(remedyName);
    }

    public override void Affect(PlayerData playerData)
    {
        this.target.timeRemaining -= this.damage;

        base.Affect(playerData);
    }
}
