using System.Linq;

public class CureEffect : Effect
{
    public CureEffect(string name, string description, IEffectLifeTime lifetime) : base(name, description, lifetime)
    {
    }

    public override void Affect()
    {
        this.target.CureDesease(this.name);

        base.Affect();
    }
}
