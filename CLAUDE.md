# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

UODiablo is a ServUO-based Ultima Online server emulator written in C# (.NET Framework 4.8). ServUO is a community-driven project that provides a complete MMO server implementation with extensive scripting capabilities for customizing game content.

- **Website:** https://www.servuo.com
- **Discord:** https://discord.gg/0cQjvnFUN26nRt7y
- **License:** MIT

## Build Commands

### Windows
```batch
# Development build with debug symbols
Compile.WIN - Debug.bat

# Production build (optimized)
Compile.WIN - Release.bat

# Run the server
ServUO.exe
```

### Linux/macOS
```bash
# Install Mono (one-time setup)
apt-get install mono-complete  # Ubuntu
brew install mono              # macOS

# Build and run (default target)
make

# Build debug version
make debug

# Clean build artifacts
make clean

# Run after building
./ServUO.sh
```

## Solution Architecture

The project consists of three C# projects with specific build dependencies:

```
ServUO.sln
├── Ultima/           → Ultima.dll (library)
├── Server/           → ServUO.exe (executable, depends on Ultima)
└── Scripts/          → Scripts.CS.dll (library, depends on Server)
```

**Build Order:** Ultima → Server → Scripts

**Key Characteristics:**
- Target Framework: .NET 4.8
- Unsafe code blocks enabled (for performance-critical sections)
- Default server port: 2593 (TCP)
- Runtime script compilation via Microsoft.CodeDom.Providers.DotNetCompilerPlatform

## Core Architecture

### Server/ - Core Engine
The Server project (`ServUO.exe`) is the main executable providing MMO server infrastructure:

- **Entry Point:** `Server.Core` initializes configuration, networking, world state, and dynamically compiles Scripts
- **Network Layer:** `Network/` directory contains packet handling, client state management (`NetState.cs`), compression, encoding, and throttling
- **Persistence System:** `Persistence/` provides multiple save strategies (StandardSaveStrategy, ParallelSaveStrategy, etc.) with binary serialization
- **Game Objects:** `Mobile.cs` and `Item.cs` are foundational entity types
- **World Management:** `World.cs` and `Map.cs` handle world state and map data
- **UI System:** `Gumps/` implements the client dialog/UI system with various entry types (buttons, text fields, images, etc.)
- **Customs Framework:** `Customs/` provides an extensible system with modules and services for custom content

### Scripts/ - Game Content
The Scripts project is a dynamically compiled library that extends server functionality:

- **Extensibility Model:** Scripts add game content without modifying the core Server project
- **Content Types:** Abilities, items, mobiles (NPCs/monsters), quests, spells, skills, commands, gumps (UI), services
- **Compilation:** Scripts are compiled at runtime during server startup
- **Output:** Compiled to `Scripts/Output/Scripts.CS.dll`

**Key Directories:**
- `Abilities/` - Combat abilities and special moves (30+ files)
- `Commands/` - In-game admin and player commands
- `Items/`, `Mobiles/` - Game object definitions
- `Services/` - Game systems and services
- `Custom/` - Custom modifications and experimental features

### Ultima/ - Client Data Handler
The Ultima project (`Ultima.dll`) provides read-only access to Ultima Online client data files:

- Handles UO client file formats: art, gumps, hues, animations, multis (buildings/ships), tiles, sounds, lighting
- Core files: `Art.cs`, `Gumps.cs`, `Hues.cs`, `Animations.cs`, `TileData.cs`, `TileMatrix.cs`
- Provides data indexing via `FileIndex.cs` and `Files.cs`

## VitaNex Core Framework

Scripts include VitaNex Core (VNc), a modular extension framework located at `Scripts/VitaNex/Core/`:

- **Design Philosophy:** Plug & play, 100% extensible without source modifications
- **Configuration:** `VNC.cfg` file
- **Console Commands:** `[VNC`, `[VNC SRV`, `[VNC MOD` (requires admin login)
- **Custom Extensions:** Place in `Scripts/VitaNex/Custom/` rather than modifying Core directory
- **License:** MIT (Copyright 2022, Vita-Nex)

**Important:** Don't modify VitaNex Core directory structure - use the extension points provided.

## Configuration System

Configuration files in `/Config/` control server behavior (20+ .cfg files):

**Format:**
- Lines starting with `#` are option descriptions
- Blank lines terminate the current option
- Syntax: `Key=Value`
- Empty keys use null or default values
- Force defaults with: `@Key=Value`

**Essential Files:**
- `Server.cfg` - Core settings (name, address, port)
- `DataPath.cfg` - Path to UO client files (required on non-Windows)
- `General.cfg` - Global settings (metrics, item decay, PvP)
- `Expansion.cfg` - Game expansion settings
- `Accounts.cfg` - Account management

## Development Patterns

### Adding Custom Content
1. **Custom Scripts:** Add to `Scripts/Custom/` or appropriate subdirectory
2. **Custom VitaNex Extensions:** Add to `Scripts/VitaNex/Custom/`
3. **Configuration:** Add/modify `.cfg` files in `Config/`
4. **Game Data:** Add XML data to `Data/` directory

### Modifying vs Extending
- **Prefer:** Extending via Scripts project and VitaNex framework
- **Avoid:** Modifying core Server project files unless necessary
- **Never:** Modify VitaNex Core directory structure

### Network Packet Handling
- Packet handlers are defined in `Server/Network/PacketHandlers.cs`
- Packet definitions in `Server/Network/Packets.cs`
- Client state tracked via `NetState.cs`

### Persistence/Save System
- Multiple save strategies available in `Server/Persistence/`
- Binary serialization via `GenericWriter` and `GenericReader`
- Save metrics and file operations managed by strategy classes

## Initial Setup

1. Edit `/Config/Server.cfg` for essential settings (name, address, port)
2. Configure remaining `*.cfg` files as needed
3. Build using appropriate method (Windows: `.bat` files, Unix: `make`)
4. Run `ServUO.exe` (Windows) or `./ServUO.sh` (Linux/macOS)
5. Verify server starts and listens on configured port (default 2593)
6. Launch Ultima Online client and connect to server

## Key Dependencies

- **DSharpPlus 3.2.3** - Discord bot integration
- **Newtonsoft.Json** - JSON serialization
- **Microsoft.CodeDom.Providers.DotNetCompilerPlatform** - Runtime script compilation
- **WebSocket libraries** - Network communication
- **zlib** - Compression (32/64-bit DLLs included)

## CI/CD

Travis CI configuration (`.travis.yml`) provides automated builds using latest Mono compiler.
- Make sure to add files in script to the csproj to compile