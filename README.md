# Spotseek
Command line interface that integrates the spotify api with the soulseek api. Only intended for personal, non-commercial use. Support artists and do not use this code to download licensed music.

Depending on the command, you may need the following credentials to run
- a spotify access token which you can obtain [using any of the available methods](https://developer.spotify.com/documentation/web-api/tutorials/getting-started#request-an-access-token)
- soulseek credentials

## Requirements
- dotnet runtime environment (link will pop up when running)

## Debugging
- in visual studio (for Mac):
    - set cli project as startup project
    - run cli project with: custom configuration
    - put `save-playlist` or another command with optional parameters in the arguments field
    - select run or debug in actions
- in visual studio (for Windows):
    - use one of the launch profiles to run or debug

## Creating a runnable CLI executable from the project code
- make sure to update version number in `.cli` project file
- navigate to cli project folder
- to publish for mac, use command `dotnet publish  -c Release -p:PublishDir=<someDir>/build --self-contained --runtime osx-x64`
- to publish for windows, use command `dotnet publish  -c Release -p:PublishDir=<someDir>\build-windows --self-contained --runtime win-x64`

## Usage
- Download a release from the releases tab
- Extract it
- Open folder in a terminal
- (On Mac) Run `./spotseek --help` to see a list of the available commands
- (On Windows) Run `.\spotseek.exe --help` to see a list of the available commands

## Commands
To show the list of commands:
`./spotseek --help`

To show the options of a specific command, e.g.:
`./spotseek download-playlist --help`

### download-playlist command
Attempts to download all songs from a playlist if a match is found in soulseek with certain filters to ensure the right song is downloaded with a bitrate of at least 320kbps.

Requires:
- spotify access token
- soulseek credentials

Example usage of command:
`./spotseek download-playlist -i myspotifyuser -n "My Awesome playlist" -u soulseekUser -p soulseekPass -g -t 30 -a MY_ACCESS_TOKEN`

### save-playlist command
Attempts to download all songs from a playlist that are not yet in your "Liked Songs" playlist with certain filters to ensure the right song is downloaded with a bitrate of at least 320kbps. Will add the song to your "Liked Songs" if a song is succesfully downloaded.

Requires:
- spotify access token
- soulseek credentials

Example usage of command:
`./spotseek save-playlist -i myspotifyuser -n "My Awesome playlist" -u soulseekUser -p soulseekPass -g -t 30 -a MY_ACCESS_TOKEN`

### download-track command
Attempts to download a song from soulseek based on the provided query, again with all filters for quality but with less filters for determining if the right song is downloaded.

Requires:
- soulseek credentials

Example usage of command:
`./spotseek download-track -u soulseekUser -p soulseekPass -q someQuery`


### translate-to-open-key command
Translates the "InitialKey" Id3Tag of a song to [OpenKey format](https://blog.faderpro.com/music-theory/camelot-wheel-dj-harmonic-mixing/).

Requires:
- no credentials

Example usage of command:
`./spotseek translate-to-open-key -f someFolder`

## ToDos
- let user specify output directory instead of using current directory
- use dependency injection
- rearrange arguments and make some static
