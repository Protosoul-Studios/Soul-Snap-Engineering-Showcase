# Independent Soul Snap C# engineering examples

These are **two separately authored, engine-free .NET 10 review models** illustrating engineering contracts informed by the source-inspected Unity vertical slice. They are **not copies of the game scripts** and are **not** Unity/NGO components.

| Model | Demonstrates | Does **not** establish |
| --- | --- | --- |
| [PresenceAuthority](PresenceAuthority.cs) | Sender-bound movement, registration, clamping, snapshots and removal | Live NGO ownership, Unity views, scene wiring or account security |
| [ChallengeAuthority](ChallengeAuthority.cs) | Simplified firing/cooldown, invalid target rejection, one-shot opponent challenge, lifetime expiry | Physical projectile collisions, multiplayer networking, CardBattle sessions |

The [test runner](Program.cs) contains **20 independently executable contract scenarios** (9 presence, 11 challenge). The GitHub Actions workflow compiles and runs them on .NET 10, and checks repository layout and Markdown links. **20/20 independent checks passed** in [run #40209420007](https://github.com/Protosoul-Studios/Soul-Snap-Engineering-Showcase/actions/runs/40209420007) at `6db07af9b4c4ebbfd2dc5d3b11a1176f01a411b0`.

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

[CI status and execution](../../.github/workflows/showcase-checks.yml) · [Verification limits](../../docs/verification-matrix.md) · [Related case studies](../../case-studies/README.md)

A standalone test pass supports only the illustrated model's contract; do not transfer its safeguards or numbers to untested production Unity behavior. **All rights reserved; no general reuse license.**
