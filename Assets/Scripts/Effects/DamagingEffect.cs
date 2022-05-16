using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Classe DamagingEffect, representa o efeito de Dano
/// </summary>
public class DamagingEffect : Effect
{
    public float damage; // Quantidade de dano
    public List<string> curedBy; // Lista de rem�dios que curam esse efeito

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
     * M�todo que diz se o efeito � cur�vel por algum rem�dio
     */
    public bool IsCuredBy(string remedyName)
    {
        return this.curedBy.Contains(remedyName);
    }

    /*
     * M�todo que executa o efeito do efeito
     */
    public override void Affect()
    {
        this.target.timeRemaining -= this.damage;

        base.Affect();
    }
}
