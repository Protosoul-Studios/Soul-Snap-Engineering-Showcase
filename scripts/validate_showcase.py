#!/usr/bin/env python3
"""Code-first engineering portfolio tree audit; not a Git-history or rights audit."""
from __future__ import annotations
import re
import sys
from pathlib import Path
from urllib.parse import unquote

ROOT = Path(__file__).resolve().parent.parent
issues: list[str] = []
blocked_roots = {"Assets", "Packages", "ProjectSettings", "Library", "Temp", "Obj", "Builds", "Build", "ThirdParty", "source-samples", "templates"}
internal_files = {"AGENTS.md", "PROJECT_STATE.md", "docs/PUBLICATION_CHECKLIST.md"}
blocked_ext = {".unity", ".prefab", ".asset", ".fbx", ".dll", ".exe", ".apk", ".aab", ".pem", ".key", ".p12", ".pfx", ".keystore"}
media_ext = {".mp4", ".webm", ".png", ".jpg", ".jpeg", ".webp"}
private_url = re.compile(r"https?://(?:www\.)?github\.com/Protosoul-Studios/(?!Soul-Snap-Engineering-Showcase(?:/|$))[\w.-]+", re.I)
sensitive = re.compile(r"(?:gh[pousr]_[A-Za-z0-9]{12,}|github_pat_[A-Za-z0-9_]{12,}|sk-[A-Za-z0-9]{18,}|AIza[A-Za-z0-9_-]{30,})")
placeholder = re.compile(r"\[(?:GAME TITLE|FILE\.cs|APPROVED PATH|PRECISE CLAIM|INSERT LINK|YOUR NAME)\]|\b(?:TODO|FIXME|TBD):")
link_re = re.compile(r"!?\[[^\]]+\]\(([^)]+)\)")

for path in ROOT.rglob("*"):
    if not path.is_file() or ".git" in path.parts:
        continue
    rel = path.relative_to(ROOT)
    name = rel.as_posix()
    if rel.parts[0] in blocked_roots or name in internal_files:
        issues.append(f"private/template path: {name}")
    if path.suffix.lower() in blocked_ext:
        issues.append(f"blocked project/binary/key type: {name}")
    if path.name == ".env" or path.name.startswith(".env."):
        issues.append(f"environment configuration: {name}")
    if path.suffix.lower() in media_ext and rel.parts[:2] != ("media", "videos"):
        issues.append(f"unreviewed media location: {name}")
    if path.suffix == ".cs" and rel.parts[:2] != ("samples", "independent-models"):
        issues.append(f"unexpected C# source: {name}")
    if path.suffix.lower() in {".md", ".py", ".yml", ".yaml", ".cs"}:
        content = path.read_text(encoding="utf-8")
        if sensitive.search(content) or private_url.search(content):
            issues.append(f"sensitive token/private source URL pattern: {name}")
        if path.suffix.lower() == ".md":
            if placeholder.search(content):
                issues.append(f"unresolved placeholder: {name}")
            for found in link_re.finditer(content):
                target = found.group(1).split(" ", 1)[0].strip("<>")
                if not target or target.startswith(("http:", "https:", "mailto:", "#")):
                    continue
                suffixless = unquote(target.split("#", 1)[0].split("?", 1)[0])
                resolved = (path.parent / suffixless).resolve()
                if not resolved.is_relative_to(ROOT) or not resolved.exists():
                    issues.append(f"invalid relative link: {name} -> {target}")
if issues:
    print("SHOWCASE TREE AUDIT FAILED")
    for issue in issues: print(" - " + issue)
    sys.exit(1)
print("SHOWCASE TREE AUDIT PASSED")
print("Scope: current files/layout/local Markdown links; not historical commits, media rights, production game or security certification.")
