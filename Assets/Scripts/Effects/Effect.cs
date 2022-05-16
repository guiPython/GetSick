public abstract class Effect : IEffect
{
    public string name;

    public string description;
    public PlayerData? target;
    public int turnApplied;
    public IEffectLifeTime lifetime;

    public Effect(string name, string description, IEffectLifeTime lifetime)
    {
        this.name = name;
        this.description = description;
        this.lifetime = lifetime;
     }

    public virtual void Affect() { }

    public Effect? DyingWish()
    {
        return null;
    }

    public override string ToString()
    {
        string str = this.name;

        if (this.lifetime is TemporaryLifetime temporaryLt)
        {
            str += " (" + temporaryLt.duration + " turns left)";
        }
        else if (this.lifetime is PermanentLifetime)
        {
            str += " (Permanent)";
        }

        return str;
    }

    public Effect Clone()
    {
        return (Effect) this.MemberwiseClone();
    }
}
