namespace SoulSnapShowcase;

/// <summary>
/// A separate, engine-free exercise in challenge admission and one-shot resolution.
/// There is NO physics, projectile motion, netcode, authentication service or CardBattle.
/// This model is NOT copied from SoulBowSession.
/// </summary>
public sealed class ChallengeAuthority
{
    public readonly record struct ChallengeRequest(ulong ChallengerClientId, ulong TargetClientId);
    private readonly record struct Shot(ulong ShooterClientId, double ExpiresAt);

    private readonly Func<ulong, bool> isRegistered;
    private readonly Dictionary<uint, Shot> shots = new();
    private readonly Dictionary<ulong, double> nextAllowedFire = new();
    private uint nextId = 1;

    public ChallengeAuthority(Func<ulong, bool> isRegistered)
    {
        this.isRegistered = isRegistered ?? throw new ArgumentNullException(nameof(isRegistered));
    }

    public int ActiveShotCount => shots.Count;

    public bool TryFire(ulong senderClientId, double timeSeconds, float aimX, float aimY, out uint shotId)
    {
        shotId = 0;
        if (!isRegistered(senderClientId)
            || !double.IsFinite(timeSeconds)
            || !float.IsFinite(aimX) || !float.IsFinite(aimY)
            || Math.Sqrt((double)aimX * aimX + (double)aimY * aimY) < 0.5d
            || (nextAllowedFire.TryGetValue(senderClientId, out var next) && timeSeconds < next))
            return false;

        shotId = nextId++;
        shots.Add(shotId, new Shot(senderClientId, timeSeconds + 3d));
        nextAllowedFire[senderClientId] = timeSeconds + 0.5d;
        return true;
    }

    /// <summary>
    /// Simulates a collision outcome reported BY THE AUTHORITY.
    /// It does not let a remote client submit a hit or simulate Unity physics.
    /// </summary>
    public bool TryResolveCollision(uint shotId, ulong? targetClientId, out ChallengeRequest challenge)
    {
        challenge = default;
        if (!shots.Remove(shotId, out var shot)
            || !targetClientId.HasValue
            || targetClientId.Value == shot.ShooterClientId
            || !isRegistered(targetClientId.Value))
            return false;

        challenge = new ChallengeRequest(shot.ShooterClientId, targetClientId.Value);
        return true;
    }

    public int Expire(double timeSeconds)
    {
        if (!double.IsFinite(timeSeconds))
            throw new ArgumentOutOfRangeException(nameof(timeSeconds));

        var expired = shots.Where(item => item.Value.ExpiresAt <= timeSeconds)
            .Select(item => item.Key).ToArray();
        foreach (var id in expired)
            shots.Remove(id);
        return expired.Length;
    }
}
