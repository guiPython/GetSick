using System.Linq;

/// <summary>
/// Classe CureEffect, representa o efeito de Cura
/// </summary>
public class CureEffect : Effect
{
    /*
     * Construtor da classe
     */
    public CureEffect(string name, string description, IEffectLifeTime lifetime) : base(name, description, lifetime)
    {
    }

    /*
     * Método que executa o efeito do efeito
     */
    public override void Affect()
    {
        this.target.CureDesease(this.name);

        base.Affect();
    }
}
