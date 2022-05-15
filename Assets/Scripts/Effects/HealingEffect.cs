using System.Linq;

public class HealingEffect : Effect
{
    public float healing;

    public HealingEffect(string name, string description, float healing, IEffectLifeTime lifetime) : base(name, description, lifetime)
    {
        this.healing = healing;
    }

    public override void Affect()
    {
        this.target.timeRemaining += this.healing;

        base.Affect();
    }
}
