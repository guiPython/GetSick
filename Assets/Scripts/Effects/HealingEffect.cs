using System.Linq;

/// <summary>
/// Classe HealingEffect, representa o efeito de Regenera��o
/// </summary>
public class HealingEffect : Effect
{
    public float healing; // Quantidade de regenera��o

    /*
     * Construtor da classe
     */
    public HealingEffect(string name, string description, float healing, IEffectLifeTime lifetime) : base(name, description, lifetime)
    {
        this.healing = healing;
    }

    /*
     * M�todo que executa o efeito do efeito
     */
    public override void Affect()
    {
        this.target.timeRemaining += this.healing;

        base.Affect();
    }
}
