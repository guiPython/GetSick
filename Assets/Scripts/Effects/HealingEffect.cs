using System.Linq;

/// <summary>
/// Classe HealingEffect, representa o efeito de Regeneração
/// </summary>
public class HealingEffect : Effect
{
    public float healing; // Quantidade de regeneração

    /*
     * Construtor da classe
     */
    public HealingEffect(string name, string description, float healing, IEffectLifeTime lifetime) : base(name, description, lifetime)
    {
        this.healing = healing;
    }

    /*
     * Método que executa o efeito do efeito
     */
    public override void Affect()
    {
        this.target.timeRemaining += this.healing;

        base.Affect();
    }
}
