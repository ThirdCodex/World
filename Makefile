all: world

world: scriptengine
	mono-csc \
		-r:bin/ScriptEngine.dll -lib:bin/ScriptEngine.dll \
		-out:bin/World.exe \
		-sdk:4 -main:Program src/World.cs

scriptengine:
	mono-csc \
		-r:bin/MoonSharp.Interpreter.dll \
		-lib:bin/MoonSharp.Interpreter.dll \
		-out:bin/ScriptEngine.dll \
		-target:library \
		-sdk:4 src/ScriptEngine.cs
