using System;

/// <summary>
/// Interface para os tipo de duração Temporária
/// </summary>
public class TemporaryLifetime : IEffectLifeTime
{
    public ushort duration; // Duração em turnos

    /*
     * Construtor da classe
     */
    public TemporaryLifetime(ushort duration)
    {
        this.duration = duration;
    }

    /*
     * Método para criar uma instância dessa classe a partir de uma duração anulável
     */
    public static TemporaryLifetime From(ushort? duration)
    {
        if (!duration.HasValue)
            throw new InvalidOperationException("Duration is needed when using Temporary Lifetime");

        return new TemporaryLifetime(duration.Value);
    }
}
