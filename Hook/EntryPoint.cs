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
                GameObject go = new GameObject("ModLoader");
                ModLoader point = go.AddComponent<ModLoader>();
                DontDestroyOnLoad(go);
            }
        }

    }
}
