<div align="center">

  <img src="assets/icon.svg" alt="Matchzy Enhanced" width="140" height="140">

# Matchzy Enhanced

⚡ **Enhanced CS2 match management plugin tailored for tournament automation**

  <p>Enhanced fork of MatchZy tailored for the automatic tournament platform. Adds more events and enables external tools to setup, control, and track matches in real-time.</p>

> 🔥 **Fork lineage:** [Original MatchZy](https://github.com/shobhit-pathak/MatchZy) → [sivert-io MatchZy Enhanced](https://github.com/sivert-io/MatchZy-Enhanced) → **BebraLand MatchZy Enhanced**.
>
> 🧠 **What BebraLand adds:** the missing production glue between the CS2 server, the tournament admin panel, and the broadcast stack — reliable server handoffs, operator-controlled series flow, authoritative live state, comprehensive statistics, and roster enforcement. It is designed to work together with [MatchZy Auto Tournament](https://github.com/BebraLand/matchzy-auto-tournament) and [JTs Hud](https://github.com/BebraLand/JTs-Hud).

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)

**🔗 [MatchZy Auto Tournament](https://github.com/BebraLand/matchzy-auto-tournament)** • **[CS2 Server Manager](https://github.com/sivert-io/cs2-server-manager)**

</div>

---

## 🚀 Quick Start

**Use [CS2 Server Manager](https://github.com/sivert-io/cs2-server-manager)** for automated setup with MatchZy Enhanced pre-configured:

👉 **[Get Started with CS2 Server Manager](https://github.com/sivert-io/cs2-server-manager)**

### Manual Installation

1. Download the [latest release](https://github.com/BebraLand/MatchZy-Enhanced/releases)
2. Extract to `game/csgo/` directory
3. Restart your server

📖 **[Documentation](https://docs.sivert.io/docs/me)**

---

## ✨ What's Enhanced

Built for **[MatchZy Auto Tournament](https://github.com/BebraLand/matchzy-auto-tournament)** with extended APIs and events for tournament automation. The original enhanced feature set is kept below; BebraLand-specific additions are listed separately:

### Tournament Features
- 📡 **Extended event system** for real-time match tracking
- 🔧 **Match report API** with structured JSON state
- 🔄 **Thread-safe operations** for reliable automation
- 🤖 **Simulation mode** for testing and demos
- 🔁 **Event retry system** with automatic queue and recovery
- 📊 **Server tracking** with health monitoring and status events
- 💾 **Pull API** for direct match stats retrieval

### Player Features
- 🚀 **Auto-ready system** — Instant match starts (optional)
- ⏸️ **Enhanced pauses** — Team limits, timeouts, dual unpause
- ⏱️ **Side selection timer** — Auto-decide after knife round
- 🏳️ **`.gg` command** — Team vote to forfeit early
- 🚫 **FFW system** — Handle full team disconnects
- ⚡ **Smart demo delays** — 10s restart when demos disabled
- 📺 **Center notifications** — Important events shown center-screen with countdown timers

### BebraLand Fork Features

These are the BebraLand-specific additions on top of the original MatchZy feature set:

- 🎛️ **Operator-controlled series transitions** — admins can decide when a series moves to the next map instead of fighting automatic intermission timing.
- 🔁 **Live match reallocation** — a live match can be handed to another server through explicit checkpoints, preserving the tournament state during infrastructure changes.
- 📊 **Comprehensive match statistics** — the plugin sends the round, map, series, and player data needed by MAT and JTs Hud to build real history and broadcast views.
- 🧩 **Roster enforcement across transitions** — configured players stay on their assigned sides and non-playing admins are routed to spectators after connects and map loads.
- 🧪 **Stable simulation behavior** — bot rosters remain predictable during warmup and go-live, making tournament demos and integration testing repeatable.

---

## 📖 Documentation (docs.sivert.io)

- 📋 **[Configuration Guide](https://docs.sivert.io/docs/me/user/configuration)** — All ConVars and examples
- 🎮 **[Commands Reference](https://docs.sivert.io/docs/me/user/commands)** — Player and admin commands
- 🔗 **[Integration Guide](https://docs.sivert.io/docs/me/advanced/integration)** — API endpoints and events
- 📝 **[Changelog](https://docs.sivert.io/docs/me/advanced/changelog)** — Release history

---

## 🔗 Related Projects

- **[MatchZy Auto Tournament](https://github.com/BebraLand/matchzy-auto-tournament)** — Automated tournament platform
- **[JTs Hud](https://github.com/BebraLand/JTs-Hud)** — Broadcast HUD manager connected to the same live match state
- **[CS2 Server Manager](https://github.com/sivert-io/cs2-server-manager)** — Multi-server deployment tool

## 🙏 Credits

**Original MatchZy:** [shobhit-pathak/MatchZy](https://github.com/shobhit-pathak/MatchZy) by WD-  
**Enhanced Fork:** Maintained by [BebraLand](https://github.com/BebraLand) for [MatchZy Auto Tournament](https://github.com/BebraLand/matchzy-auto-tournament)

Built with [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/) • Inspired by [Get5](https://github.com/splewis/get5)

---

<div align="center">

<strong>Made with ❤️ for the CS2 community</strong>

</div>
