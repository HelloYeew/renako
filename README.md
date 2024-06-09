<img src="Renako.Resources/Textures/Logo/renako-logo.png" width="200" height="200">

# Renako

A cross-platform community rhythm game design to be not just a rhythm game, but will be a rhythm game that have the visual novel elements in it.

## Game Overview

Renako try to be a rhythm game that's not just only a rhythm game, but try to be a rhythm game that include the story so the balance between PvP and PvE will be there.

The game will have the rhythm game part with the editor, so you can create your own beatmap and share it with the community and the visual novel part that you can use our visual novel editor to create your own story and try to mixing your story with the rhythm game part to create a unique storytelling experience. With the goal of cross-platform design in mind.

About the rhythm game concept you can check the gameplay design part in the design documentation section that we try to make the game to be compatible with the storytelling part and the difficulty of the game.

## Project status

The game is currently under heavy development that sometime will be slow due to my software engineer jobs, the gameplay are partially implemented but not yet fully implemented but you can try the game by running the game in the desktop project using the test beatmap provided in the game.

You can follow the development by checking [Renako development blog](https://story.helloyeew.dev/tag/renako/) that I will try to update the blog post every week or follow this repository to get the latest update.

Current development plans are available here:

- [Rough plan of development](https://github.com/users/HelloYeew/projects/5)
- [Design task](https://github.com/users/HelloYeew/projects/7)
- [Future roadmap](https://github.com/users/HelloYeew/projects/6)

## Design Documentation

Here is the list of design documentation for Renako (each Figma file have many pages in them):

- [Figma UI file](https://www.figma.com/file/slfKBAdlVhJXxCgGKEmNfa/Renako-Design?type=design&node-id=269%3A214&mode=design&t=Mo1yI0tcytDeBn2k-1)
- [Gameplay design](https://www.figma.com/file/slfKBAdlVhJXxCgGKEmNfa/Renako-Design?type=design&node-id=358-3&mode=design)
- [Styling assets and design documentation](https://www.figma.com/file/BX3qXUFYNsWAJPIid0Cfng/Renako-Assets?type=design&node-id=0%3A1&mode=design&t=nMCULqG0f4mZXN1b-1)

Note : In styling assets and design documentation, there are some "guidelines" that need to be followed when designing on both UX, UI and assets.

## Developing Renako

The following items are required to be installed on your computer in order to develop Renako:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- The use of a C# IDE (Rider, Visual Studio) is highly recommended to browse through the codebase.

For mobile platforms, you will need to run `sudo dotnet workload restore` to install Android and iOS workloads to build the project in mobile platforms.

Getting started with developing Renako is as follows:


### Grabbing the code from the repository

```sh
git clone https://github.com/HelloYeew/renako
cd renako
```

### Running the game

```sh
cd Renako.Desktop

# restore nuget packages for the solution
dotnet restore

# run the game with the debug profile
dotnet run 

# if you ever want to run the game with the release profile
# for performance testing, using the release profile
dotnet run -c:Release
```

### Running visual tests

```sh
cd Renako.Game.Tests

# restore nuget packages for the solution
dotnet restore

# run the visual tests with the debug profile
dotnet run

# run unit tests
dotnet test
```

## Contributing

Contributing in the form of pull request, suggestions or bug reports via issues are welcome. If interested to touch on the game's design, please contact me beforehand working on designing. Idea or suggestions via issues are welcome too.

Also, if you want to contact me directly, you can contact me via the contact in [my GitHub profile](https://github.com/HelloYeew). Don't hesitate to contact me if you have any questions or want to contribute to the project.

## License

This project is licensed under the MIT license. Please see [the licence file](LICENSE) for more information. tl;dr you can do whatever you want as long as you include the original copyright and license notice in any copy of the software/source.
