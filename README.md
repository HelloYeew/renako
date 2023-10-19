(game icon)

# Renako

(a game description with gameplay description here, you can write it as you understand)

## Project status

(under constant development, gameplay finalized, styling is mostly finalized but still need to be polished, under working on the game's foundation like base styling, story and codebase)

(with project management resource here)

- [Rough plan of development](https://github.com/users/HelloYeew/projects/5)
- [Design task](https://github.com/users/HelloYeew/projects/7)
- [Future roadmap](https://github.com/users/HelloYeew/projects/6)

## Design Documentation

Here is the list of design documentation for Renako (each Figma file have many pages in them):

- [Figma UI file](https://www.figma.com/file/slfKBAdlVhJXxCgGKEmNfa/Renako-Design?type=design&node-id=269%3A214&mode=design&t=Mo1yI0tcytDeBn2k-1)
- [Gameplay design](https://www.figma.com/file/slfKBAdlVhJXxCgGKEmNfa/Renako-Design?type=design&node-id=358-3&mode=design)
- [Styling assets and design documentation](https://www.figma.com/file/BX3qXUFYNsWAJPIid0Cfng/Renako-Assets?type=design&node-id=0%3A1&mode=design&t=nMCULqG0f4mZXN1b-1)

## Developing Renako

The following items are required to be installed on your computer in order to develop Renako:

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)
- The use of a C# IDE (Rider, Visual Studio) is highly recommended to browse through the codebase.

For mobile platform, you will need to run `sudo dotnet workload restore` to install Android and iOS workloads to build the project in mobile platforms.

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

(contribution on code via pull request, suggestion or bug report via issue is welcome. If want to contribute to the game's design, contact HelloYeew (me@helloyeew.dev))

## License

This project is licensed under the MIT license. Please see [the licence file](LICENSE) for more information. tl;dr you can do whatever you want as long as you include the original copyright and license notice in any copy of the software/source.