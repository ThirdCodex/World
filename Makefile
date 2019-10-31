all: world

client: utilities
	mono-csc \
		-r:bin/NNanomsg.dll -lib:bin/bin/NNanomsg.dll  \
		-r:bin/Utilities.dll -lib:bin/Utilities.dll \
		-out:bin/Client.dll \
		-sdk:4 src/Client.cs

world: utilities serverentity scriptengine
	mono-csc \
		-r:bin/NNanomsg.dll -lib:bin/bin/NNanomsg.dll  \
		-r:bin/ServerEntity.dll -lib:bin/ServerEntity.dll \
		-r:bin/ScriptEngine.dll -lib:bin/ScriptEngine.dll \
		-r:bin/Utilities.dll -lib:bin/Utilities.dll \
		-out:bin/World.exe \
		-sdk:4 -main:Program src/World.cs

scriptengine:
	mono-csc \
		-r:bin/MoonSharp.Interpreter.dll -lib:bin/MoonSharp.Interpreter.dll \
		-out:bin/ScriptEngine.dll \
		-target:library \
		-sdk:4 src/ScriptEngine.cs

serverentity:
	mono-csc \
		-r:bin/MoonSharp.Interpreter.dll -lib:bin/MoonSharp.Interpreter.dll \
		-target:library \
		-out:bin/ServerEntity.dll \
		-sdk:4 src/ServerEntity.cs

utilities:
	mono-csc \
		-r:bin/NNanomsg.dll -lib:bin/bin/NNanomsg.dll  \
		-target:library \
		-out:bin/Utilities.dll \
		-sdk:4 src/Utilities.cs
