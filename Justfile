set dotenv-load

game := env("RD_GAME_DIRECTORY", x"~/.local/share/Steam/steamapps/common/Rhythm Doctor")
game_assemblies := game + "/Rhythm Doctor_Data/Managed/"
game_assembly := "Assembly-CSharp.dll"
rdtools_assembly := "RDTools.dll"
mod_assembly := "li.yuri.rhythmdoctor.shocktherapy.dll"

[private]
default:
    @just --list --list-heading $'{{BOLD}}{{UNDERLINE}}Shock Therapy{{NORMAL}} \n' --unsorted

[private]
check-config:
   @if test ! -d "{{game}}"; then \
    echo "{{style("error")}} ⛔  Game directory does not exist at {{game}}. Please set RD_GAME_DIRECTORY to your Rhtyhm Doctor path in a .env file or as an environment variable."; \
    exit 1; \
   fi

[doc("Imports required files from Rhythm Doctor's directory.")]
init:
    @cp -f "{{game_assemblies}}/{{game_assembly}}" "./lib/{{game_assembly}}"
    @cp -f "{{game_assemblies}}/{{rdtools_assembly}}" "./lib/{{rdtools_assembly}}"
    @echo "{{BOLD}}{{GREEN}}✅  Imported game's assembly."
alias i := init

[doc('Builds the mod')]
build:
    dotnet build
    @echo "{{BOLD}}{{GREEN}}✅  Build succeeded."
alias b := build

[doc("Builds the mod and copies the build output into the game's BepinEx directory.")]
deploy: check-config build
    @cp -f "./bin/Debug/netstandard2.1/{{mod_assembly}}" "{{game}}/BepInEx/plugins/{{mod_assembly}}"
    @echo "{{BOLD}}{{GREEN}}✅  Copied build output to game."
alias d := deploy


[doc("Launches Rhythm Doctor directly without steam, after building and deploying the mod.")]
launch: deploy
    cd "{{game}}" && ./run_bepinex.sh "Rhythm Doctor"
alias r := launch

[doc("Launches Rhythm Doctor via steam, after building and deploying the mod.")]
launch-steam: deploy
    steam steam://rungameid/774181
alias s := launch-steam

[doc("Watch BepInEx logs")]
logs:
    @tail -f "{{game}}/BepInEx/LogOutput.log"
alias l:=logs

[group("config")]
[doc("Edit ShockTherapy's configuration")]
config-shock-therapy:
    $EDITOR "{{game}}/BepInEx/config/li.yuri.rhythmdoctor.shocktherapy.cfg"

[group("config")]
[doc("Edit BepInEx's configuration")]
config-bepinex:
    $EDITOR "{{game}}/BepInEx/config/BepInEx.cfg"

