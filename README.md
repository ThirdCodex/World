# World Server

Welcome, this is the World server for the Unity client. To start, make sure that
you have the [latest build of
mono](https://www.mono-project.com/download/stable/) for your system. 

Next, clone this repository and run `./configure`. This will make the `bin/` and
`lib/` directories and also download `nuget.exe`. Nuget is the package manager
for .Net which is used to install dependencies.

Finally, run `make` and the binaries will be put inside of `bin/`. Run these
like `mono bin/World.exe`. Enjoy!
