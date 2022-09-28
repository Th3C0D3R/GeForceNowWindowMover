# GeForceNow WindowMover
[![Build status](https://ci.appveyor.com/api/projects/status/rlok08jsyvfrs4or/branch/main?svg=true)](https://ci.appveyor.com/project/Th3C0D3R/geforcenowwindowmover/branch/main)

This Application exists because I could not find a working programm to resize/move the game window which 
GeForce NOW shows when inside a game.

## HOW TO USE (precompiled)
#### The program currently must be run everytime you enter a new game
1) Start GeForce NOW and start the game you want
2) When the game is fullscreen run the `GeForceNowWindowMover.exe`
3) You will get asked which method you want:
3.1) if Wrapper Window is chosen:
3.1.1) You will directly asked to select the process it should wrap around
3.2) if fixed position and size is chosen:
3.2.1) You can resize and move the dummy window to the location the GeForceNow window should be
3.2.2) With "Save" the program will save the location and size for the next run
4) If done correctly, the GeForceNow window should be either at the fixed position/size or within the Wrapper Window

## Compile your own
Just clone the repo and compile, it should work out of the box (I compiled it with Visual Studio 2022 and .NET 4.8)
