# 🔧 The Poor Man's Garage - MSC Mod Suite

A modular utility mod for **My Summer Car** that provides quality-of-life features via hotkeys. Built with **MSCLoader v1.4.2**.

## 🎮 Features

### 1. **Satsuma Jumpstarter** (Ctrl + J)
Instantly fully charge your Satsuma's battery and fix a stalled engine. Never get stranded in the middle of nowhere again!

- 🔋 Fully recharges battery to 100%
- ⚡ Fixes stalled engines instantly
- 📍 Works from anywhere in the game world

### 2. **Sledgehammer Summoner** (Ctrl + H)
Instantly summon the sledgehammer to your location. No more hunting through the map for tools!

- 🔨 Teleports sledgehammer to player
- ⏱️ Instant activation
- 📦 No item duplication

## 📦 Installation

1. Download the latest **SamentorMods.dll** from the [Releases](../../releases) page
2. Place it in your MSCLoader mods folder:
   ```
   My Summer Car/Mods/SamentorMods.dll
   ```
3. Launch the game and enjoy!

## 🎯 Hotkeys

| Action | Hotkey |
|--------|--------|
| Jumpstart Satsuma | **Ctrl + J** |
| Summon Sledgehammer | **Ctrl + H** |

## 📁 Project Structure

```
The-Poor-Man-s-Garage/
├── .github/workflows/
│   └── build.yml              # Automated cloud compilation
├── lib/                       # Game DLL references (add manually)
│   ├── Assembly-CSharp.dll
│   ├── MSCLoader.dll
│   └── UnityEngine.dll
├── src/
│   └── Main.cs                # Main mod manager with all features
├── SamentorMods.csproj        # Project configuration
├── .gitignore
└── README.md
```

## 🛠️ Development

### Prerequisites
- Visual Studio 2019+ or MSBuild
- My Summer Car game files
- MSCLoader v1.4.2

### Building Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/Samentor/The-Poor-Man-s-Garage.git
   cd The-Poor-Man-s-Garage
   ```

2. Add game DLL references to `lib/`:
   - Copy from `My Summer Car/Mods/` or your game installation:
     - `Assembly-CSharp.dll`
     - `MSCLoader.dll`
     - `UnityEngine.dll`

3. Build the project:
   ```bash
   msbuild SamentorMods.csproj /p:Configuration=Release
   ```

4. The compiled DLL will be in `bin/Release/SamentorMods.dll`

### Auto-Build with GitHub Actions
Every push to the `main` branch automatically compiles the mod. Download the artifact from the workflow run!

## 🔄 How It Works

### Satsuma Jumpstart
- **PlayMaker Integration**: Uses `PlayMakerFSM` to access the game's electrical systems
- **Battery Charge**: Sets the battery charge variable to 130.0f (fully charged)
- **Engine Reset**: Sends the `START_ENGINE` event to wake up the electrical system

### Sledgehammer Summoner
- **GameObject Lookup**: Finds both the player and sledgehammer in the game world
- **Position Override**: Teleports the sledgehammer in front of the player
- **Physics Reset**: Clears velocity/angular velocity to prevent flying objects

## 📝 License

This project is provided as-is for personal use and modding community contributions.

## 🤝 Contributing

Found a bug? Have an idea? Feel free to:
- Open an [Issue](../../issues)
- Submit a [Pull Request](../../pulls)
- Join the discussion!

---

**Created by:** [samentor](https://github.com/Samentor)  
**Repository:** [The-Poor-Man's-Garage](https://github.com/Samentor/The-Poor-Man-s-Garage)  
**Last Updated:** July 5, 2026  
**Mod Loader Version:** v1.4.2+
