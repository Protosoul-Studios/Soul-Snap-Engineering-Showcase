# Evidence and coverage

[Portfolio](../README.md) · [Verification methods](verification-matrix.md) · [Case studies](../case-studies/README.md)

**Production baseline inspected:** `33eb46745d1c594acf09d6726d748d14f557667d`. No Unity production code or game source-history is included in the intended public file tree.

| ID | Source-inspected game responsibility | Public case study | Evidence limit |
| --- | --- | --- | --- |
| **ENG-01** | Server registers/removes presence using connection IDs and sends snapshots | [Presence](../case-studies/multiplayer-presence.md) | Previous owner-reported two-instance manual validation; local loopback only |
| **ENG-02** | Server receives and clamps sender-bound movement intentions | [Presence](../case-studies/multiplayer-presence.md) | Historical owner-reported two-direction movement; no latency/fault tests |
| **ENG-03** | Server simulates Soul Bow projectiles, collisions, expiry and presentation snapshots | [Soul Bow](../case-studies/soul-bow-authority.md) | Owner-reported firing, world impacts and expiry; no performance metrics |
| **ENG-04** | A registered opposing-player hit produces a typed challenge signal, not a CardBattle session | [Challenge](../case-studies/challenge-boundary.md) | Owner-reported challenge logs in both directions; self-hit guard source-inspected only |
| **ENG-05** | Networking and projectile handlers/state are cleaned during shutdown | [Challenge](../case-studies/challenge-boundary.md) | Owner-reported disconnect/reconnect/shutdown smoke, not a stress test |

## Separately verified portfolio-only code

| ID | Model | Contract checks | Verification |
| --- | --- | ---: | --- |
| **MOD-01** | [PresenceAuthority](../samples/independent-models/PresenceAuthority.cs) | 9 | Independent .NET 10 contract model; [CI](../.github/workflows/showcase-checks.yml) |
| **MOD-02** | [ChallengeAuthority](../samples/independent-models/ChallengeAuthority.cs) | 11 | Independent .NET 10 contract model; [CI](../.github/workflows/showcase-checks.yml) |

**20/20 standalone model checks passed** in [GitHub Actions run #40209420007](https://github.com/Protosoul-Studios/Soul-Snap-Engineering-Showcase/actions/runs/40209420007) at `6db07af9b4c4ebbfd2dc5d3b11a1176f01a411b0`. These are **not** Unity tests. See the [verification matrix](verification-matrix.md) for exact run scope and untested behavior.

**Future video evidence:** [recording gallery](../media/videos/README.md) — additions after initial code-first public release. No footage currently claimed.
