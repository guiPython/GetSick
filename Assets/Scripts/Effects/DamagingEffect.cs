using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Classe DamagingEffect, representa o efeito de Dano
/// </summary>
public class DamagingEffect : Effect
{
    public float damage; // Quantidade de dano
    public List<string> curedBy; // Lista de remédios que curam esse efeito

    /*
     * Construtor da classe
     */
    public DamagingEffect(string name, string description, float damage, IEffectLifeTime lifetime, List<string> curedBy)
        : base(name, description, lifetime)
    {
        this.damage = damage;
        this.curedBy = curedBy ?? new List<string>();
    }

    /*
     * Método que diz se o efeito é curável por algum remédio
     */
    public bool IsCuredBy(string remedyName)
    {
        return this.curedBy.Contains(remedyName);
    }

    /*
     * Método que executa o efeito do efeito
     */
    public override void Affect()
    {
        this.target.timeRemaining -= this.damage;

        base.Affect();
    }
}
