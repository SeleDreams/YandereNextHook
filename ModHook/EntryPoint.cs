using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEngine.SceneManagement;
namespace ModHook
{
    public class EntryPoint : MonoBehaviour
    {
        /*
            this class is where dll are loaded into the game.
         */


        static bool hooked = false;

        // this function is called from version.dll
        public static void Hooked()
        {
            if (!hooked)
            {
                hooked = true;
                try
                {
                    Directory.CreateDirectory("Mods");
                    SceneManager.sceneLoaded += SceneLoaded;
                }
                catch
                {

                }
            }
        }

        static bool instantiated = false;

        // when a scene is loaded
        public static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // if the hook is not loaded, and a scene is loaded
            if (!instantiated && scene.name != string.Empty)
            {
                instantiated = true;
                GameObject go = new GameObject("EntryPoint");
                EntryPoint point = go.AddComponent<EntryPoint>();
                DontDestroyOnLoad(go);
            }

        }
        int num;
        public void Awake()
        {
            // add the logger
            gameObject.AddComponent<Logger>();

            // Replaced to match the current Mods\ModName\ModName.dll::ModName.Mod::Start() stuff

            // get directory into an array
            string[] dirs = Directory.GetDirectories("Mods");

            // for each of them
            foreach (string dir in dirs)
            {
                // we try something...
                try
                {
                    // load the assembly
                    Assembly asm = Assembly.LoadFile(dir + "\\" + Path.GetFileName(dir) + ".dll");

                    // num is set to the number of active.txt inside of the mod folder if it exist
                    if (File.Exists(dir + "/active.txt"))
                    {
                        int.TryParse(File.ReadAllText(dir + "/active.txt"), out num);
                    }
                    else
                    {
                        // the loader won't recalculate active.txt if it didn't exist on purpose.
                        File.Create(dir + "/active.txt");
                        File.WriteAllText(dir + "/active.txt", "1");
                        Debug.LogWarning("Since " + Path.GetFileName(dir) + " don't have an active.txt, i created one and set it to 1.\n" +
                            "you will need to relaunch the game to load the mod.");
                        
                    }

                    // if active.txt have "1" inside it...
                    if (num == 1)
                    {
                        // looking for a class named "Mod" in the namespace "ModName"
                        Type type = asm.GetType(Path.GetFileName(Path.GetFileName(dir)) + ".Mod");

                        // now, we execute that method with the path of the mod as an argument, since it's required
                        type.GetMethod("Start").Invoke(type, new object[] {
                        Path.GetFullPath(dir)
                    });
                    }
                }
                // we return the error if something goes wrong
                catch(Exception ex)
                {
                    Debug.LogError("issue while loading" + Path.GetFileName(dir) + ".dll : " + ex);
                }

            }
        }

    }
}

