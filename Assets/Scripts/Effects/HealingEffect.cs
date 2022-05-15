using System.Linq;

public class HealingEffect : Effect
{
    public float healing;

    public HealingEffect(string name, float healing, IEffectLifeTime lifetime) : base(name, lifetime)
    {
        this.healing = healing;
    }

    public override void Affect(PlayerData playerData)
    {
        playerData.timeRemaining += this.healing;

        base.Affect(playerData);
    }
}
