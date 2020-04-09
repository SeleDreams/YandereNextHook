using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Reflection;

namespace TestAssembly
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

        static bool instantiated = false;

        public static void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!instantiated)
            {
                instantiated = true;
                GameObject go = new GameObject("EntryPoint");
                EntryPoint point = go.AddComponent<EntryPoint>();
                DontDestroyOnLoad(go);
            }
        }

        public void Start()
        {
            
        }

    }
}
