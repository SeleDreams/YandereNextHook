using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModHook
{
    public class Logger : MonoBehaviour
    {
		// when awaken
		public void Awake()
		{
			messages = new List<string>();
			Application.logMessageReceived += new Application.LogCallback(LogMessageReceived);
			pos = new Vector2(2f, 2f);
			size = new Vector2(Screen.width / 4, Screen.height / 2);
			logStyle = new GUIStyle();

			// @TODO : maybe make a config file
			logStyle.normal.textColor = Color.blue;
			Debug.Log("Succesfully initialized !");

			currentlyInUse = true;
		}

		void Update()
        {
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.P))
            {
				currentlyInUse = !currentlyInUse;
            }
        }

		// somehow  if the variable is local it don't work
		Rect rect;

		// Something to draw on screen
		public void OnGUI()
		{
			if (currentlyInUse)
			{
				rect.Set(0, 0, 0, 0);

				GUI.Label(rect, text, logStyle);
			}
		}

		public void LogMessageReceived(string message, string stackTrace, LogType type)
		{
			bool flag = (long)messages.Count >= 25L;
			if (flag)
			{
				messages.RemoveAt(0);
			}
			messages.Add(Enum.GetName(typeof(LogType), type) + " : " + message);
			text = string.Empty;
			foreach (string str in messages)
			{
				text = text + str + "\n";
			}
		}

		//@TODO : maybe make a config file for those kind of stuff
		private const uint MAX_MESSAGES = 60u;

		private List<string> messages;

		private string text;

		private Vector2 pos;

		private Vector2 size;

		private GUIStyle logStyle;

		private bool currentlyInUse;
	}
}