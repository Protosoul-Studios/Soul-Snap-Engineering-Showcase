namespace SoulSnapShowcase;

/// <summary>
/// Independently written, engine-free model of one server-side presence contract.
/// It illustrates sender-bound intent, clamping and snapshot ownership;
/// it is NOT the game's NetworkPresenceSession implementation.
/// </summary>
public sealed class PresenceAuthority
{
    private const float Speed = 4f;
    private readonly Dictionary<ulong, Presence> presences = new();

    private sealed class Presence
    {
        public float X;
        public float Y;
        public float InputX;
        public float InputY;
    }

    public readonly record struct Snapshot(ulong ClientId, float X, float Y);

    public bool Register(ulong clientId) => presences.TryAdd(clientId, new Presence());

    public bool IsRegistered(ulong clientId) => presences.ContainsKey(clientId);

    public bool Remove(ulong clientId) => presences.Remove(clientId);

    public bool SubmitMovement(ulong senderClientId, float horizontal, float vertical)
    {
        if (!presences.TryGetValue(senderClientId, out var state)
            || !float.IsFinite(horizontal) || !float.IsFinite(vertical))
            return false;

        // Do the length calculation as double so huge finite inputs cannot overflow to infinity.
        var length = Math.Sqrt((double)horizontal * horizontal + (double)vertical * vertical);
        if (length > 1d)
        {
            horizontal = (float)(horizontal / length);
            vertical = (float)(vertical / length);
        }

        state.InputX = horizontal;
        state.InputY = vertical;
        return true;
    }

    public void Advance(float seconds)
    {
        if (!float.IsFinite(seconds) || seconds < 0f)
            throw new ArgumentOutOfRangeException(nameof(seconds));

        foreach (var state in presences.Values)
        {
            state.X += state.InputX * Speed * seconds;
            state.Y += state.InputY * Speed * seconds;
        }
    }

    public IReadOnlyList<Snapshot> ReadSnapshot() =>
        presences.OrderBy(pair => pair.Key)
            .Select(pair => new Snapshot(pair.Key, pair.Value.X, pair.Value.Y))
            .ToArray();
}
