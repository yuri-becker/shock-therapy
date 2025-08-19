[![AGPL-3.0 License](https://img.shields.io/github/license/yuri-becker/shock-therapy?style=for-the-badge&logo=gnu&logoColor=white&color=%23A42E2B )](https://github.com/yuri-becker/shock-therapy/blob/main/LICENSE.md)

<br />
<div align="center">

  
  <h1 align="center"><strong><img src="Assets/shock.png" alt="" aria-hidden="true"><br/>ShockTherapy</strong></h1>

  <p align="center">
    Make missing notes in <a href="https://rhythmdr.com/">Rhythm Doctor</a> hurt − using a <a href="https://pishock.com/#/">PiShock</a>.
  </p>
</div>
<br/>
<br/>

**Table of Contents**
1. [About this Mod](#about-this-mod)
2. [Installation](#installation)
3. [Configuration](#configuration)
4. [Development](#development)
    * [Prerequisites](#prerequisites)
    * [Setup](#setup)
    * [Code Structure](#code-structure)
5. [Contributions](#contributions)

# About this Mod

When I started playing Rhythm Doctor, I immediately saw the potential for a fun PiShock mod. After all, I strongly believe that everything should have a PiShock integration.

With this mod you can:
* 📳 Shock or vibrate your Shocker whenever you miss a note.
* 🪦 Configure individual lengths and intensities for completely off, very off and slightly missed notes.
* 🔢 Use multiple shockers at once.

Built using [BepInEx 5](https://github.com/BepInEx/BepInEx).


# Installation

1. **Install BepInEx into Rhythm Doctor** as per [the official instructions](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex).<br/>
   * Make sure to grab the latest **5.x** release.
   * You'll most likely want to go with the **x64** version.
   * If you are unsure what the game's directory is and have it on Steam, you can just right-click Rhythm Doctor in your library and click `Manage → Browse Local Files`.
   * <details>
        <summary>The game's folder should look something like this (on Linux, similar on Windows):</summary>
   
        ![Directory at \"steamapps/common/Rhythm Doctor\" containing: BepInEx (folder); libdoorstep.so; Rhythm Doctor; Rhythm Doctor_Data (folder); run_bepinex.sh; unity.lock; UnityPlayer.so; and User (folder).](Assets/Rhythm%20Doctor%20Folder.png)
      </details><br/>
2. Download the latest `li.yuri.rhythmdoctor.shocktherapy.dll` from [Releases](https://github.com/yuri-becker/shock-therapy/releases) and **drop it into `BepinEx/plugins/`**.<br/>&nbsp;
3. <details>
    <summary>Linux only − <strong>Make the game run with BepInEx</strong>:</summary>
   
    1. **Mark `run_bepinex.sh` as executable**. You can do that via the terminal or − in most file managers − by right-clicking it and opening the properties.
    2. If you are on Steam, set the game's **launch options** to `echo %command% && ./run_bepinex.sh "Rhythm Doctor"` (See https://github.com/BepInEx/BepInEx/issues/1143).<br/>
       If you are not on Steam, configure whatever you use to launch the game to run `./run_bepinex.sh "Rhythm Doctor"`.
    </details><br/>
4. **Optionally**, you can also install [ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) (pick the BepInEx5 version). This would give you an in-game overlay to configure ShockTherapy (and other BepInEx plugins). However, it did not work for me (let me know if you have better results).<br/>&nbsp;
5. **Launch the game once** to verify you set up everything correctly:
   * If BepInEx is installed correctly, the folder `BepInEx` should contain a file called `LogOutput.log`.
   * If ShockTherapy is installed correctly, there should be a file called `li.yuri.rhythmdoctor.shocktherapy.cfg` in `BepInEx/config`.<br/>&nbsp;
6. **[Proceed to configuration](#configuration)**

# Configuration

If you installed ConfigurationManager, you can configure everything in-game. A restart might be necessary after configuration changes.

Otherwise, after the game was run once with ShockTherapy installed, a configuration file exists at `BepInEx/config/li.yuri.rhythmdoctor.shocktherapy.cfg`.
The options are all explained in the configuration file itself.

<details>
<summary>Example Configuration</summary>

```ini
## Settings file was created by plugin ShockTherapy v0.1.0
## Plugin GUID: li.yuri.rhythmdoctor.shocktherapy

[PiShock]

## Your PiShock username (you can see this on https://pishock.com/#/account).
# Setting type: String
# Default value: 
Username = cool-pishock-user

## Your API key (also obtainable at https://pishock.com/#/account)
# Setting type: String
# Default value: 
ApiKey = abcdefg-hijkl-mnop-qrst-xyz12345678

## PiShock auth endpoint without trailing slash (usually not necessary to change).
# Setting type: String
# Default value: https://auth.pishock.com
AuthEndpoint = https://auth.pishock.com

## PiShock WebSocket (usually not necessary to change).
# Setting type: String
# Default value: wss://broker.pishock.com
WebSocketEndpoint = wss://broker.pishock.com

[Punishment.Missed]

## What the shocker should do.
# Setting type: PunishmentType
# Default value: None
# Acceptable values: None, Vibration, Shock
Type = Shock

## Duration in seconds (can be fractional).
# Setting type: Single
# Default value: 0.5
# Acceptable value range: From 0.1 to 15
Duration = 0.5

## Intensity of the vibration/shock.
# Setting type: Byte
# Default value: 30
# Acceptable value range: From 1 to 100
Intensity = 60

[Punishment.SlightlyOff]

## What the shocker should do.
# Setting type: PunishmentType
# Default value: None
# Acceptable values: None, Vibration, Shock
Type = None

## Duration in seconds (can be fractional).
# Setting type: Single
# Default value: 0.5
# Acceptable value range: From 0.1 to 15
Duration = 0

## Intensity of the vibration/shock.
# Setting type: Byte
# Default value: 30
# Acceptable value range: From 1 to 100
Intensity = 0

[Punishment.VeryOff]

## What the shocker should do.
# Setting type: PunishmentType
# Default value: None
# Acceptable values: None, Vibration, Shock
Type = Vibration

## Duration in seconds (can be fractional).
# Setting type: Single
# Default value: 0.5
# Acceptable value range: From 0.1 to 15
Duration = 0.5

## Intensity of the vibration/shock.
# Setting type: Byte
# Default value: 30
# Acceptable value range: From 1 to 100
Intensity = 30

[Shockers]

## ID of PiShock Hub. See at https://pishock.com/#/control by clicking on the ⋮.
# Setting type: UInt32
# Default value: 0
Device = 1234

## Optionally, define a ShareCode if this is someone else's device. Leave empty if this is your own device.
# Setting type: String
# Default value: 
ShareCode = 

## Comma-seperated IDs of Shockers to use (e.g. "11225, 11226"). You can see your shocker's ID by clicking on the Cog symbol at https://pishock.com/#/control.
# Setting type: String
# Default value: 
Shockers = 12345
```
</details>


# Development

## Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/en-us/)
* [just](https://just.systems./man/en/) for running the project's scripts.
  * Those scripts currently are only made for linux. 
* The game [Rhythm Doctor](https://rhythmdr.com/).

## Setup

* Follow most steps at [Installation](#installation) − You won't need to download ShockTherapy, but still install BepInEx. 
* Run `just init` to copy the necessary files from the game's folder.
  * If Rhythm Doctor is not installed at `~/.local/share/Steam/steamapps`, set `RD_GAME_DIRECTORY` in the .env file.
* Build and launch the game via `just launch` or `just launch-steam`.
* You might want to enable Debug output by setting `LogLevels = All` under `Logging.Console`/`Logging.Disk` in `BepInEx.cfg`.

## Code Structure

* The entry point is `src/ShockTherapy.cs`. This is also where the actual mod is contained and the lifecycle is managed.
* `src/PiShock/` is where the models and infrastructure for talking to PiShock's API are.
* `src/Config/` is the configuration models.

Some Caveats:
* `System.Text.Json` attributes do not work with the Newtonsoft.Json version shipped in Rhythm Doctor, but they are globally imported in the game's assembly.<br/>
  Therefore, all `JsonProperty` attributes must be explicitly set as `[Newtonsoft.Json.JsonProperty]`.
* `init` Properties are not available.


# Contributions

Bug Reports and feature suggestions are welcome. [Create an issue](https://github.com/yuri-becker/shock-therapy/issues/new).

I'll also happily look into any Pull Requests.