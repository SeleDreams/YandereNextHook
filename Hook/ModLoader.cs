using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using YandereNext.Debugging;

namespace Hook
{
    public class ModLoader : MonoBehaviour
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        public void Awake()
        {
            gameObject.AddComponent<LogConsole>();
            try { Directory.CreateDirectory("Mods"); } catch { }

            // Loads all the mods when the game starts, it will load the static void function "YandereMod.StartMod"
            if (!Directory.Exists("Mods"))
            {
                MessageBox(IntPtr.Zero, "The Mods directory does not exist, The game will play normally.", "Mods directory not found", 0);
                Destroy(this);
                return;
            }

            string[] dirs = Directory.GetDirectories("Mods");
            foreach (string dir in dirs)
            {
                StartMod(dir);
            }
        }

        private void StartMod(string dir)
        {
            int active;
            if (int.TryParse(File.ReadAllText(dir + "/active.txt"), out active) && active > 0)
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(dir + "\\" + Path.GetFileName(dir) + ".dll");
                    Type t = asm.GetType(Path.GetFileName(dir) + ".Mod");
                    t.GetMethod("Start").Invoke(t, new object[] { Path.GetFullPath(dir) });
                }
                catch (Exception ex)
                {
                    MessageBox( IntPtr.Zero, 
                        $"There was an issue while loading the  {Path.GetFileName(dir)} mod : {ex.InnerException.Message}.\n This mod won't be loaded !", ex.GetType().ToString(), 0);
                }
            }

            
        }
    }
}
