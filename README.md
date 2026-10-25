# Soul Snap: Card Extraction

**Unity/C# multiplayer gameplay engineering — Protosoul Studios**

[![Independent C# contract checks](https://github.com/Protosoul-Studios/Soul-Snap-Engineering-Showcase/actions/workflows/showcase-checks.yml/badge.svg?branch=main)](https://github.com/Protosoul-Studios/Soul-Snap-Engineering-Showcase/actions/workflows/showcase-checks.yml)

*The automated checks cover portfolio-only C# models and local links—not the Unity production game.*

Soul Snap: Card Extraction is an in-development game combining multiplayer world encounters, a planned tactical card-duel system, and extraction stakes. This **code-first engineering showcase** examines the implemented early multiplayer vertical slice: two-player presence, server-controlled Soul Bow projectiles, and a validated hit signal designed to connect world gameplay to a future card duel.

**For prospective clients:** review network authority, defensive decisions, and executable code. **For collaborators:** explore system responsibilities and integration boundaries. **For players and fans:** see how a distinctive game mechanic is being developed without revealing the private Unity project.

> **Portfolio, not a playable build.** The current proof emits a challenge signal; it does **not** implement Card Battle rules, extraction, inventory, persistence, matchmaking, or production online services.

## Explore the engineering

| System | Engineering challenge | Case study | Independent C# illustration |
| --- | --- | --- | --- |
| **Multiplayer presence and movement** | Associate client intentions with server-owned player state | [Presence and movement](case-studies/multiplayer-presence.md) | [PresenceAuthority](samples/independent-models/PresenceAuthority.cs) |
| **Server-authoritative Soul Bow** | Validate firing and decide hits without trusting a client's claimed target | [Projectile authority](case-studies/soul-bow-authority.md) | [ChallengeAuthority](samples/independent-models/ChallengeAuthority.cs) |
| **World-hit to card-duel boundary** | Separate physics, challenge signaling, and later Match/CardBattle orchestration | [Challenge boundary](case-studies/challenge-boundary.md) | [Challenge request model](samples/independent-models/ChallengeAuthority.cs) |

The independent models illustrate selected contracts; **they are not copied Unity production code, drop-in systems, or proof that the game's code has exactly the same safeguards**.

## Run the independent C# examples

Two engine-free **.NET 10** models contain **20 self-checking scenarios** covering identity registration, input clamps, snapshots, firing guards, invalid hits, one-shot challenges and expiry. GitHub Actions runs the checks and a repository link/layout validator.

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

[Model guide](samples/independent-models/README.md) · [Test runner](samples/independent-models/Program.cs) · [CI workflow](.github/workflows/showcase-checks.yml) · [Verification matrix](docs/verification-matrix.md)

**Verified result:** [GitHub Actions run #40209420007](https://github.com/Protosoul-Studios/Soul-Snap-Engineering-Showcase/actions/runs/40209420007) passed **20/20 independent model checks** and the repository structure/link audit at commit `6db07af9b4c4ebbfd2dc5d3b11a1176f01a411b0`. These results establish **only the behavior of the independent examples**. They do not demonstrate Unity physics, multiplayer reliability, or production testing.

## Actual development context

Inspected development source revision: `33eb46745d1c594acf09d6726d748d14f557667d` · Unity **6000.2.14f1** · **Netcode for GameObjects 2.7.0** · **Unity Transport 2.6.0**.

The actual prototype uses local input intentions, transport-provided connection IDs, server-owned player positions and physical projectiles, and a typed `SoulChallengeRequest` after a valid hit on another registered player. It was manually tested by the developer with an Editor host and a standalone Development Build connected over loopback. Those are **historical owner-reported observations**, not fresh independently rerun game tests. Connection IDs do not provide account authentication or a production security guarantee.

[Architecture](docs/architecture.md) · [Decisions](docs/decisions.md) · [Evidence coverage](docs/coverage.md) · [Verification methods and limits](docs/verification-matrix.md)

## Gameplay video evidence

Videos will be added **after the initial public code-first release**, as in the other Protosoul Studios engineering showcases. There is no footage claimed today. The [video gallery and capture plan](media/videos/README.md) identify the demonstrations to add.

## Reviewing and reusing this work

**© 2026 Protosoul Studios and applicable rights holders. All rights reserved.** This is a professional review portfolio, **not** a public Unity project or open-source software distribution. No general reuse license is granted; see the [code review and reuse policy](docs/code-review-and-reuse.md).
