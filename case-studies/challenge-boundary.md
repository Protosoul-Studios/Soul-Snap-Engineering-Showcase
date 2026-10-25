# World-hit to card-duel challenge boundary

**Production claim IDs:** ENG-04, ENG-05 · **Implementation status:** hit-to-signal implemented; Match-owned CardBattle orchestration **not implemented**.

## Integration problem

Soul Snap's intended hybrid gameplay connects a physical world encounter to a tactical card duel. The projectile implementation must not also own card rules, participant locking, inventory consequences, or UI lifecycle.

## Source-inspected contract

When a server-owned projectile hits another registered player, the prototype constructs a readonly `SoulChallengeRequest` with challenger and target connection IDs and publishes a `SoulChallengeSignals.Requested` event.

**That is where the implemented flow ends.** Future Match orchestration would need to decide duel eligibility, map network IDs to session identities, resolve duplicate/concurrent challenges, create and dispose a CardBattle session, and handle its result. Neither the Unity source nor this portfolio currently proves those downstream steps.

## Lifecycle and design trade-offs

The prototype removes NGO named-message handlers before shutdown and clears retained presence/projectile views. Historical owner reports describe ordinary disconnect/reconnect and shutdown without avoidable errors, but no automated long-running teardown stress test exists. The current static event requires explicit subscription management as future consumers are added; it is not a durable multiplayer message bus.

## Independent illustration

The [**ChallengeAuthority**](../samples/independent-models/ChallengeAuthority.cs) standalone model enforces a deliberately simplified one-time opposing-player request decision. Its checks are independent model tests, **not** Unity CardBattle, network-service or physics tests.

[Architecture](../docs/architecture.md) · [Evidence matrix](../docs/coverage.md) · [Video plan](../media/videos/README.md)
