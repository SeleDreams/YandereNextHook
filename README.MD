# YandereNextHook

is a modloader for the game Yandere Simulator (even tough it can be used in virtually any unity game)
that aim to help modders to write code in order to modify the game.

it's mainly used to translate the game currently

# How to install ?

Download the latest version here : https://github.com/Corentynn/YandereNextHook/releases/latest

once downloaded, just move ModHook.dll and version.dll at the root folder of the game, where yanderesimulator.exe is.

if when you launch the game, blue text show up, then it's working!

## how do I install mods ?

you need to find a mod that use YandereNextHook.

once you find it, extract it in the Mods folder.

the mods hierarchy should look like that : 

```
├───Mods
│   ├───ModName
│   │   └───ModName.dll
│   │   └───active.txt
│   ├───ModName2
│   │   └───ModName2.dll
│   │   └───ModName2.dll
├───YandereSimulator_Data
├───ModHook.dll
├───version.dll
├───UnityPlayer.dll
├───YandereSimulator.exe
```

some mods can have more files depending on what they do.

## i have a problem !

you can ask help here : https://discord.gg/EVeBtYy

ask for Corentyn_ or SeleDreams

# How to create a mod ?

you'll need : 
	- .NET Framework 4.0
	- a c# Editor, like IntelJ Rider or Visual Studio
	- c# knowledge, as well as Unity's c# library knowledge
	- the latest version of the modloader
	
1) create a new project that use Class Library (.NetFramework)

2) Name your project as you want (exemple : MySuperCoolMod).
make sure to use the same name for the solution.

as for the version, it's recommanded to use .NET Framework 4. Any subversion should work.

3) delete Class1 and create a class named "Mod"

4) replace `class Mod` for `static class Mod`


add that inside the `Mod` class :
```c#

        public static void Start(string[] args)
        {
            MainClass.Launch();
            MainClass.Path = args;
        }

```

5) now we need to create the MainClass.

6) MainClass need to inherit MonoBehaviour.

add those files as a reference (you'll find them inside YandereSimulator_Data\Managed\) :
	
	- Unity.Mathematics.dll
    - Unity.TextMeshPro.dll
    - UnityEngine.dll
	- UnityEngine.CoreModule.dll
	- UnityEngine.IMGUIModule.dll
    - UnityEngine.InputLegacyModule.dll
	- UnityEngine.InputModule.dll
    - UnityEngine.UI.dll
	- UnityEngine.UnityWebRequestAssetBundleModule.dll
	- UnityEngine.UnityWebRequestAudioModule.dll
    - UnityEngine.UnityWebRequestModule.dll
    - UnityEngine.UnityWebRequestTextureModule.dll
    - UnityEngine.UnityWebRequestWWWModule.dll
    - XInputDotNetPure.dll
	
you might need to add more reference in the future but those are the strict minimum.
	
	
7) once done, you need a static string named path and a static Launch function : 

```c#
		// path of the mod
		public static string Path;

		// static function called from a static class
        public static void Launch()
        {
			// create a gameobject with "MySuperCoolMod" as object name.
            GameObject gameObject = new GameObject("MySuperCoolMod");
			
			// adding the MainClass on it
            gameObject.AddComponent<MainClass>();
			
			// this way it alway stay loaded
            DontDestroyOnLoad(gameObject);
        }
```

both the launch function and string Path are called / set-up when Mod.Start is called.

from there, you can do whatever you want, assuming you know how to do it.