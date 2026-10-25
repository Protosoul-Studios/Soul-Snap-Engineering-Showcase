# Architecture and system boundaries

[Portfolio](../README.md) · [Decisions](decisions.md) · [Evidence](coverage.md)

**Basis:** Source inspection of the private Unity development project at `33eb46745d1c594acf09d6726d748d14f557667d`. This is a conceptual responsibility map, not an observed complete runtime call graph.

```text
Local movement / Soul Bow input
                 |
                 v
   Netcode connection-scoped sender ID
         |                  |
         v                  v
NetworkPresenceSession  SoulBowSession
  server registration    validate fire intention
  authoritative position derive shooter and muzzle
  movement / snapshots   physical server projectile
         |                  |
         v                  v
 interpolated views   server collision validation
                            |
                            v
                 SoulChallengeRequest
                            |
                     [NOT IMPLEMENTED]
                Match eligibility / CardBattle
                 participant mapping / results
```

## Production responsibilities

- **Networking:** registered client presence, sender-bound movement intentions, host simulation, state snapshots and teardown.
- **SoulWeapons:** authoritative firing and physical projectiles, world/registered-player collision, expiry and challenge publication.
- **Player presentation:** development-only capsule instances and local/remote views.
- **Match/CardBattle:** *designed future responsibilities*, not running code at the inspected revision.
- **Persistence/extraction:** *future work*, not demonstrated or released here.

The inspected prototype uses Unity 6000.2.14f1, NGO 2.7.0 and Transport 2.6.0. Its `Card Battle` testing scene is a loopback development sandbox, **not** a finished game mode. Many domain folders in the underlying project are organizational scaffolding rather than implementations.

The [independent C# models](../samples/independent-models/README.md) are newly authored abstractions for review; they do not reproduce NGO transport, Unity collisions, scene bindings or production source.
