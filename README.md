# Spotseek
Command line interface that integrates the spotify api with the soulseek api. Only intended for non-commercial use.

Needs the following credentials to run 
- a spotify access token which you have to obtain yourself using [any of the available methods](https://developer.spotify.com/documentation/web-api/tutorials/getting-started#request-an-access-token)
- soulseek credentials

## Requirements
- dotnet runtime environment (link will pop up when running)

## Debugging
- in visual studio:
    - set cli project as startup project
    - right click cli project
    - select run with: custom configuration
    - put `save-playlist` or another command with optional parameters in the arguments field
    - select run or debug in actions
    
## Creating a runnable CLI executable from the project code
- make sure to update version number in `.cli` project file
- navigate to cli project folder
- to publish for mac, use command `dotnet publish  -c Release -p:PublishDir=<someDir>/build --self-contained --runtime osx-x64`
- to publish for windows, use command `dotnet publish  -c Release -p:PublishDir=<someDir>\build-windows --self-contained --runtime win-x64`

## commands
To show the list of commands:
`./spotseek --help`

To show the options of a command:
`./spotseek download-playlist --help`

Example usage of command:
`spotseek save-playlist -i myspotifyuser -n House -u lala -p lala -g -t 30 -a MY_ACCESS_TOKEN`
