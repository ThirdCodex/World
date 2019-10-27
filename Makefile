all: world player

world: utilities script
	mono-csc \
		-r:bin/Utilities.dll -lib:bin/Utilities.dll \
		-r:bin/Script.dll    -lib:bin/Script.dll \
		-r:bin/NNanomsg.dll  -lib:bin/NNanomsg.dll \
		-out:bin/World.exe \
		-sdk:4 -main:Program src/World.cs

player: utilities script
	mono-csc \
		-r:bin/Utilities.dll -lib:bin/Utilities.dll \
		-r:bin/Script.dll    -lib:bin/Script.dll \
		-out:bin/Player.exe \
		-sdk:4 src/Player.cs

script: src/Script.cs
	mono-csc -sdk:4 -target:library -out:bin/Script.dll src/Script.cs

utilities: script src/Utilities.cs
	mono-csc \
		-r:bin/Script.dll   -lib:bin/Script.dll \
		-r:bin/NNanomsg.dll -lib:bin/NNanomsg.dll \
		-out:bin/Utilities.dll \
		-sdk:4 -target:library src/Utilities.cs
