using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace Hook
{
    public class EntryPoint : MonoBehaviour
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);


        static bool hooked = false;
        public static void Hooked()
        {
            if (!hooked)
            {
                hooked = true;
                SceneManager.sceneLoaded += SceneLoaded;
            }
        }


        public static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!string.IsNullOrEmpty(scene.name))
            {
                SceneManager.sceneLoaded -= SceneLoaded;
                GameObject go = new GameObject("EntryPoint");
                EntryPoint point = go.AddComponent<EntryPoint>();
                DontDestroyOnLoad(go);
            }
        }

        public void Awake()
        {
            try
            {
                Directory.CreateDirectory("Mods");
            }
            catch
            {

            }

            // Loads all the mods when the game starts, it will load the static void function "YandereMod.StartMod"
            if (!Directory.Exists("Mods")) {
                MessageBox(IntPtr.Zero, "The Mods directory does not exist, no mods will be loaded","Mods directory not found",0);
                return;
            }
            string[] dirs = Directory.GetDirectories("Mods");
            foreach (string dir in dirs)
            {
                int active;
                if (int.TryParse(File.ReadAllText(dir + "/active.txt"), out active))
                {
                    if (active > 0)
                    {
                        try
                        {
                            Assembly asm = Assembly.LoadFile(dir + "\\" + Path.GetFileName(dir) + ".dll");
                           
                            Type t = asm.GetType(Path.GetFileName(dir) + ".Mod");
                            t.GetMethod("Start").Invoke(t, new object[] { Path.GetFullPath(dir) });
                        }
                        catch (Exception ex)
                        {
                            MessageBox(IntPtr.Zero, "There was an issue while loading the " + Path.GetFileName(dir) + " mod : " + ex.InnerException.Message, ex.GetType().ToString(), 0);
                        }
                    }
                }
            }
        }

    }
}
