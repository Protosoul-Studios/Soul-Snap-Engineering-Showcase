# Server-authoritative Soul Bow projectile

**Production claim IDs:** ENG-03, ENG-04 · **Evidence:** source inspected plus historical owner-reported Unity runtime observations.

## Problem

A meaningful challenge cannot rely on a client declaring which opponent was hit. The server must decide whether a shot can spawn, simulate its collision, and determine whether the target is another registered player.

## Source-inspected design

In the private Unity prototype, `SoulBowSession` receives shot *intentions* associated with actual transport sender IDs. The server checks player registration, finite/useful direction and a firing cooldown. It derives the muzzle position from authoritative player position and creates a `SoulBowProjectile` with a Rigidbody and trigger collider.

The server owns projectile movement, collision, lifetime and cleanup. Remote instances get projectile position/facing snapshots and create collider-free visual copies, not authoritative hit reports. A registered opposing-player hit emits a typed challenge request; a world impact or expired projectile does not create a duel request. Code also includes a self-hit guard; its separate controlled runtime demonstration was not confirmed.

## Engineering decisions and limits

Server-side collision reduces dependence on untrusted hit claims, but the slice is **unpredicted** and does not include latency compensation, production bow tuning, pooling, visual polish or verified scalable networking. Aim intention checking is deliberately limited, not anti-cheat certification.

## Separate code example and verification

[**ChallengeAuthority.cs**](../samples/independent-models/ChallengeAuthority.cs) illustrates an engine-free shot-admission and one-shot resolution contract with **11 checks**. It **does not run Unity physics or NGO**; its results cannot establish game hit detection.

Earlier developer validation reports shots from both host and client, correct opposing-player diagnostics, backstop/world impacts without challenges and shot expiry, with reconnect and shutdown smoke checks.

[Evidence coverage](../docs/coverage.md) · [Verification matrix](../docs/verification-matrix.md) · [Video gallery](../media/videos/README.md)
