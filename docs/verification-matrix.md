# Verification matrix

[Portfolio](../README.md) · [Claims](coverage.md) · [Independent model runner](../samples/independent-models/Program.cs)

| Evidence layer | Method | Status and precise limit |
| --- | --- | --- |
| **Private Unity source** | Inspected at `33eb46745d1c594acf09d6726d748d14f557667d` | **Source inspected:** documents code responsibilities; no Unity runtime is proven by inspection alone |
| **Prior project validation** | Owner tested Unity 6000.2.14f1 Editor host + standalone Development Build client over local loopback | **Historical owner-reported pass:** presences, movement, Soul Bow hits, world impacts, expiry, reconnect and shutdown; not rerun for this portfolio |
| **Independent C# models** | .NET 10 Release, [20 contract checks](../samples/independent-models/Program.cs) | **20/20 passed** in [GitHub Actions run #40209420007](https://github.com/Protosoul-Studios/Soul-Snap-Engineering-Showcase/actions/runs/40209420007) at `6db07af9b4c4ebbfd2dc5d3b11a1176f01a411b0`; the workflow also passed its local tree/link audit. The run exercises only separate review examples, *not* production Unity |
| **Production automated Unity tests** | New EditMode/PlayMode test execution | **Not run** in this showcase workflow |
| **Production internet networking/security** | Account authorization, anti-cheat, lag, load and external connectivity | **Not verified** |
| **Game footage** | [Planned post-release captures](../media/videos/README.md) | **Not yet recorded/published** |
| **Hardware/performance** | Dedicated device and profiling measurements | **Not verified** |

## Reproduce the historical manual test

In the **private original Unity project** at the stated revision, open the `Card Battle` testing scene in Unity 6000.2.14f1. Run an Editor host and a desktop standalone Development Build client over loopback (`127.0.0.1:7777`). Observe both local/remote player presences; move each with WASD/arrow keys; turn aim with Q/E and fire with Space. Test an opposing-player hit and inspect the host's diagnostic; then try a world/backstop impact, projectile timeout, disconnect/reconnect and shutdown with active shots. Record any errors rather than inferring a pass from the procedure. **These steps document the prior owner validation method, not a new execution.**

## Reproduce the independent examples

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

Results from the portfolio's CI apply **only to these independent C# models**. They are distinct from the two-instance Unity development report. A planned gameplay clip is not a performance benchmark or a security audit.
