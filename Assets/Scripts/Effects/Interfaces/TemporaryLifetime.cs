using System;

public class TemporaryLifetime : IEffectLifeTime
{
    public ushort duration; // Lifetime in turns
    public TemporaryLifetime(ushort duration)
    {
        this.duration = duration;
    }

    public static TemporaryLifetime From(ushort? duration)
    {
        if (!duration.HasValue)
            throw new InvalidOperationException("Duration is needed when using Temporary Lifetime");

        return new TemporaryLifetime(duration.Value);
    }
}
