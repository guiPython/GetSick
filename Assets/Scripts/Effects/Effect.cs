/// <summary>
/// Classe abstrata Effect, representa um efeito qualquer
/// </summary>
public abstract class Effect : IEffect
{
    public string name; // Nome do efeito

    public string description; // Descrição do efeito
    public PlayerData? target; // Alvo do efeito
    public int turnApplied; // Turno que esse efeito foi aplicado
    public IEffectLifeTime lifetime; // Tipo de duração do efeito

    /*
     * Construtor da classe
     */
    public Effect(string name, string description, IEffectLifeTime lifetime)
    {
        this.name = name;
        this.description = description;
        this.lifetime = lifetime;
     }

    /*
     * Método que executa o efeito do efeito
     */
    public virtual void Affect() { }

    /*
     * Método que faz o override do ToString()
     */
    public override string ToString()
    {
        string str = this.name;

        if (this.lifetime is TemporaryLifetime temporaryLt)
        {
            str += " (" + temporaryLt.duration + " turnos restantes)";
        }
        else if (this.lifetime is PermanentLifetime)
        {
            str += " (Permanente)";
        }

        return str;
    }
}
