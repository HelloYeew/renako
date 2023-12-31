<img src="Renako.Resources/Textures/Logo/renako-logo.png" width="200" height="200">

# Renako

A cross-platform community rhythm game design to be not just a rhythm game, but will be a rhythm game that has a story and some other gameplay elements that you not found in other rhythm games.

## Project status

The game is currently under heavy development, most gameplay elements have been designed but still need polishing but aren't nowhere usable for now.

Current development plans are available here:

- [Rough plan of development](https://github.com/users/HelloYeew/projects/5)
- [Design task](https://github.com/users/HelloYeew/projects/7)
- [Future roadmap](https://github.com/users/HelloYeew/projects/6)

## Design Documentation

Here is the list of design documentation for Renako (each Figma file have many pages in them):

- [Figma UI file](https://www.figma.com/file/slfKBAdlVhJXxCgGKEmNfa/Renako-Design?type=design&node-id=269%3A214&mode=design&t=Mo1yI0tcytDeBn2k-1)
- [Gameplay design](https://www.figma.com/file/slfKBAdlVhJXxCgGKEmNfa/Renako-Design?type=design&node-id=358-3&mode=design)
- [Styling assets and design documentation](https://www.figma.com/file/BX3qXUFYNsWAJPIid0Cfng/Renako-Assets?type=design&node-id=0%3A1&mode=design&t=nMCULqG0f4mZXN1b-1)

Note : In styling assets and design documentation, there are some "guidelines" that need to be followed when designing on both UX, UI and assets. Include the game's start lore too.

## Developing Renako

The following items are required to be installed on your computer in order to develop Renako:

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)
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

Contributing in the form of pull request, suggestions or bug reports via issues are welcome. If interested to touch on the game's design as most things are already designed, please contact HelloYeew (me@helloyeew.dev) beforehand working on designing.

## License

This project is licensed under the MIT license. Please see [the licence file](LICENSE) for more information. tl;dr you can do whatever you want as long as you include the original copyright and license notice in any copy of the software/source.
