using System.Linq;

public class CureEffect : Effect
{
    public CureEffect(string name, IEffectLifeTime lifetime) : base(name, lifetime)
    {
    }

    public override void Affect(PlayerData playerData)
    {
        playerData.CureDesease(this.name);

        base.Affect(playerData);
    }
}
