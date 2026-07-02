# GeForceNow WindowMover
[![GitHub tag](https://img.shields.io/github/tag/TH3C0D3R/GeForceNowWindowMover?style=flat-square&include_prereleases=&sort=semver&color=blue)](https://github.com/TH3C0D3R/GeForceNowWindowMover/releases/)
![OS - windows](https://img.shields.io/badge/OS-windows-2ea44f?logo=windows&logoColor=%230078D6)
[![Build status](https://ci.appveyor.com/api/projects/status/rlok08jsyvfrs4or/branch/main?svg=true&style=flat-square)](https://ci.appveyor.com/project/Th3C0D3R/geforcenowwindowmover/branch/main)
[![GitHub issues](https://img.shields.io/github/issues/Th3C0D3R/GeForceNowWindowMover?style=flat-square)](https://github.com/Th3C0D3R/GeForceNowWindowMover/issues)
[![GitHub license](https://img.shields.io/github/license/Th3C0D3R/GeForceNowWindowMover?style=flat-square)](https://github.com/Th3C0D3R/GeForceNowWindowMover/blob/main/LICENSE)


This Application exists because I could not find a working programm to resize/move the game window which 
GeForce NOW shows when inside a game. CAN BE USED WITH ANY OTHER PROGRAM. BUT MAY HAVE SOME UNEXPECTED BEHAVIOR.
Will test with different games and programs in the future. If you have any suggestions, please create an issue.

## HOW TO USE (precompiled)
1. Start GeForce NOW and start the game you want
2. When the game is fullscreen run the `GFNWindowMover.exe`
3. Choose your Window by finding the Process via the "Choose Process" button
4. Choose your desired Mode (Fixed, Wrapped)
4.1 If you choose Fixed, you can set the desired position and size of the window via the "Open Fixed Region Editor" button
4.2 If you choose Wrapped, the window should automatically resize to the size of the wrapper window. You can resize the wrapper window as you like.


The Program does need to set the game/programs window parameters to display it above all other windows.
This may be problems with some anticheat software. If you have problems with the program, please create an issue.

## Compile your own
Just clone the repo and compile, it should work out of the box (I compiled it with Visual Studio 2022)

## Screenshots
![Fixed Example](https://raw.githubusercontent.com/Th3C0D3R/GeForceNowWindowMover/main/image1.png)
![Process List](https://raw.githubusercontent.com/Th3C0D3R/GeForceNowWindowMover/main/image2.png)
![Wrapper Example](https://raw.githubusercontent.com/Th3C0D3R/GeForceNowWindowMover/main/image3.png)
![Docked Wrapper Example](https://raw.githubusercontent.com/Th3C0D3R/GeForceNowWindowMover/main/image4.png)
