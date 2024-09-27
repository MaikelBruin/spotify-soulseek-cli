# Spotseek
Command line interface that integrates the spotify api with the soulseek api. Only intended for personal, non-commercial use. Support artists and do not use this code to download licensed music.

Needs the following credentials to run
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

## commands
To show the list of commands:
`spotseek --help`

To show the options of a command:
`spotseek download-playlist --help`

Example usage of command:
`spotseek save-playlist -i myspotifyuser -n House -u lala -p lala -g -t 30 -a MY_ACCESS_TOKEN`
