using SoulSnapShowcase;

var passed = 0;
void Check(string title, Action action)
{
    action();
    passed++;
    Console.WriteLine($"PASS {passed:D2}: {title}");
}

static void Require(bool condition, string description)
{
    if (!condition) throw new Exception(description);
}

Check("Register an identity", () => {
    var p = new PresenceAuthority();
    Require(p.Register(1) && p.IsRegistered(1), "registration failed");
});
Check("Reject duplicate registration", () => {
    var p = new PresenceAuthority();
    Require(p.Register(1) && !p.Register(1), "duplicate accepted");
});
Check("Reject unregistered sender movement", () => {
    var p = new PresenceAuthority();
    Require(!p.SubmitMovement(9, 1, 0), "unknown sender accepted");
});
Check("Clamp diagonal movement", () => {
    var p = new PresenceAuthority(); p.Register(1);
    Require(p.SubmitMovement(1, 1, 1), "valid intent rejected");
    p.Advance(1);
    var s = p.ReadSnapshot()[0];
    Require(Math.Abs(Math.Sqrt(s.X * s.X + s.Y * s.Y) - 4) < 0.0001, "speed not clamped");
});
Check("Reject non-finite movement", () => {
    var p = new PresenceAuthority(); p.Register(1);
    Require(!p.SubmitMovement(1, float.NaN, 0), "non-finite accepted");
});
Check("Separate movement per sender", () => {
    var p = new PresenceAuthority(); p.Register(1); p.Register(2);
    p.SubmitMovement(2, 0, 1); p.Advance(1);
    Require(p.ReadSnapshot()[0].X == 0 && p.ReadSnapshot()[0].Y == 0, "other identity moved");
    Require(p.ReadSnapshot()[1].Y == 4, "target identity did not move");
});
Check("Stable ID-ordered snapshots", () => {
    var p = new PresenceAuthority(); p.Register(9); p.Register(2);
    var s = p.ReadSnapshot();
    Require(s[0].ClientId == 2 && s[1].ClientId == 9, "snapshot order unstable");
});
Check("Disconnected identity disappears", () => {
    var p = new PresenceAuthority(); p.Register(1);
    Require(p.Remove(1) && p.ReadSnapshot().Count == 0, "removed identity retained");
});
Check("Reject invalid simulation time", () => {
    var p = new PresenceAuthority();
    try { p.Advance(-1); throw new Exception("negative step accepted"); }
    catch (ArgumentOutOfRangeException) { }
});
Check("Unregistered client cannot fire", () => {
    var p = new PresenceAuthority(); var a = new ChallengeAuthority(p.IsRegistered);
    Require(!a.TryFire(3, 0, 1, 0, out _), "unregistered shot accepted");
});
Check("Reject malformed aim", () => {
    var p = new PresenceAuthority(); p.Register(1); var a = new ChallengeAuthority(p.IsRegistered);
    Require(!a.TryFire(1, 0, float.NaN, 1, out _), "NaN shot accepted");
});
Check("Reject degenerate aim", () => {
    var p = new PresenceAuthority(); p.Register(1); var a = new ChallengeAuthority(p.IsRegistered);
    Require(!a.TryFire(1, 0, 0, 0, out _), "zero aim accepted");
});
Check("Enforce per-sender firing cooldown", () => {
    var p = new PresenceAuthority(); p.Register(1); var a = new ChallengeAuthority(p.IsRegistered);
    Require(a.TryFire(1, 0, 1, 0, out _), "initial shot rejected");
    Require(!a.TryFire(1, .1, 1, 0, out _), "early repeat accepted");
    Require(a.TryFire(1, .5, 1, 0, out _), "shot rejected after cooldown");
});
Check("World collision produces no challenge", () => {
    var p = new PresenceAuthority(); p.Register(1); var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var shot);
    Require(!a.TryResolveCollision(shot, null, out _) && a.ActiveShotCount == 0, "world impact failed");
});
Check("Reject shooter-to-self challenge", () => {
    var p = new PresenceAuthority(); p.Register(1); var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var shot);
    Require(!a.TryResolveCollision(shot, 1, out _), "self hit triggered challenge");
});
Check("Reject unregistered target", () => {
    var p = new PresenceAuthority(); p.Register(1); var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var shot);
    Require(!a.TryResolveCollision(shot, 99, out _), "unknown target accepted");
});
Check("Valid opponent gets correctly attributed challenge", () => {
    var p = new PresenceAuthority(); p.Register(1); p.Register(2);
    var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var shot);
    Require(a.TryResolveCollision(shot, 2, out var request), "challenge missing");
    Require(request.ChallengerClientId == 1 && request.TargetClientId == 2, "incorrect identities");
});
Check("Challenge consumes shot exactly once", () => {
    var p = new PresenceAuthority(); p.Register(1); p.Register(2);
    var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var shot);
    Require(a.TryResolveCollision(shot, 2, out _), "first impact missing");
    Require(!a.TryResolveCollision(shot, 2, out _), "duplicate challenge emitted");
});
Check("Expired projectile cannot challenge", () => {
    var p = new PresenceAuthority(); p.Register(1); p.Register(2);
    var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var shot);
    Require(a.Expire(3) == 1 && a.ActiveShotCount == 0, "expiry did not remove shot");
    Require(!a.TryResolveCollision(shot, 2, out _), "expired projectile challenged");
});
Check("Different senders retain distinct shooter identities", () => {
    var p = new PresenceAuthority(); p.Register(1); p.Register(2);
    var a = new ChallengeAuthority(p.IsRegistered);
    a.TryFire(1, 0, 1, 0, out var first); a.TryFire(2, 0, -1, 0, out var second);
    Require(a.TryResolveCollision(first, 2, out var challengeA) && challengeA.ChallengerClientId == 1, "A incorrect");
    Require(a.TryResolveCollision(second, 1, out var challengeB) && challengeB.ChallengerClientId == 2, "B incorrect");
});
Console.WriteLine($"ALL {passed} INDEPENDENT MODEL CHECKS PASSED");
