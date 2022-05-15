using System.Linq;

public class CureEffect : Effect
{
    public CureEffect(string name, string description, IEffectLifeTime lifetime) : base(name, description, lifetime)
    {
    }

    public override void Affect(PlayerData playerData)
    {
        playerData.CureDesease(this.name);

        base.Affect(playerData);
    }
}
