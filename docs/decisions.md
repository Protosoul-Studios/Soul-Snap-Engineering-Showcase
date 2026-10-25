# Engineering decisions and trade-offs

**Basis:** Private Unity development source inspected at `33eb46745d1c594acf09d6726d748d14f557667d`.

| Recorded choice | Benefit | Cost or unverified concern |
| --- | --- | --- |
| Start with a small, domain-organized Unity runtime assembly | Avoid premature assembly/service complexity | Many folders represent planned systems, not implementations |
| NGO 2.7.0 with Unity Transport 2.6.0 for the initial proof | Immediate two-player sender identity and local connectivity | No Relay, Lobby, matchmaking or dedicated hosting |
| Server-owned movement and projectile hits | Clients submit intentions, not arbitrary world outcomes | Snapshots are basic; latency, scaling and adversarial behavior untested |
| Typed `SoulChallengeRequest` from SoulWeapons | World-hit logic need not own the unbuilt tactical card rules | Match eligibility, stable participant mapping and duel lifecycle still future |
| Explicit handler cleanup before network teardown | Handles a previously observed shutdown ordering issue | Smoke-tested manually by the owner, not under prolonged automated load |
| Isolated `Card Battle` test scene | Narrow reproducible integration checkpoint | Not production gameplay, UI or final art |

The historical development environment also recorded a Package Manager NGO signature warning. It did not prevent the reported tests, but no production release/security conclusion follows from those tests.

[Verification matrix](verification-matrix.md)
