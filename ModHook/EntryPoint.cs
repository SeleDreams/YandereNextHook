using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEngine.SceneManagement;
namespace ModHook
{
    public class EntryPoint : MonoBehaviour
    {
        static bool hooked = false;
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

        public static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!instantiated && scene.name != string.Empty)
            {
                instantiated = true;
                GameObject go = new GameObject("EntryPoint");
                EntryPoint point = go.AddComponent<EntryPoint>();
                DontDestroyOnLoad(go);
            }

        }

        public void Awake()
        {
            // Loads all the mods when the game starts, it will load the static void function "YandereMod.StartMod"
            string[] dirs = Directory.GetDirectories("Mods");
            foreach (string dir in dirs)
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(dir + "\\Mod.dll");
                    Type[] entryTypes = asm.GetExportedTypes();
                    foreach (Type type in entryTypes)
                    {
                        if (type.Name == "YandereMod")
                        {

                            type.GetMethod("StartMod").Invoke(this, null);
                        }
                    }
                }
                catch
                {
                }

            }
        }

    }
}

