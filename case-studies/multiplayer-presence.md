# Multiplayer presence and movement

**Production claim IDs:** ENG-01, ENG-02 · **Evidence:** source inspected plus historical owner-reported runtime checks.

## Problem

Before a Soul Bow hit could lead to any multiplayer interaction, two players needed stable connection-scoped world presences. An untrusted client must not name another player as its movement owner or directly set the authoritative world position.

## Source-inspected design

In the reviewed Unity development slice, `NetworkPresenceSession` maintains a host/server registry of player presence, input and position by Netcode connection ID. Remote clients send movement intention through NGO named messages. The server uses the transport-provided **sender ID**, ignores unregistered senders, clamps input length, advances world positions and distributes complete snapshots. Each client creates disposable local/remote capsule views and interpolates toward server positions.

A disconnect removes that ID and its state; the remaining host can continue. Cleanup unregisters message handlers before network service teardown. Connection identity is **not** persistent account identity or secure login.

## Trade-offs

The simple registry and complete snapshots are easy to inspect for a two-process proof but are not a tuned production multiplayer replication design. There is no prediction, reconciliation, full session restoration, verified anti-cheat, production controller, or internet matchmaking.

## Separate code example and verification

[**PresenceAuthority.cs**](../samples/independent-models/PresenceAuthority.cs) demonstrates registration, sender-bound movement, clamping, ordered snapshots and removal in standalone .NET code. Its **nine contract checks** verify that model, *not* Unity/NGO.

The developer's recorded Editor-host/standalone-client tests report two local/remote presences, input ownership, movement in both directions, disconnect/reconnect and shutdown. These game observations were **not rerun** for this public portfolio.

[Evidence coverage](../docs/coverage.md) · [Verification matrix](../docs/verification-matrix.md) · [Video gallery](../media/videos/README.md)
