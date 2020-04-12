using System.IO;
using System.Reflection;
using UnityEngine;

namespace YandereNext.Debugging
{
	public class LogConsole : MonoBehaviour
	{
		static public bool log;

		public void Clear()
		{
			LogManager.Clear();
		}

		void Awake()
		{
			LogManager.SetupLog();
			pos = new Vector2(2, 2);
			size = new Vector2(200, 400);
			logStyle = new GUIStyle();
			logStyle.normal.textColor = Color.blue;
			log = true;
			
		}

		void OnGUI()
		{
			if (log)
			{
				var textRect = new Rect(pos, size);
				GUI.Label(textRect, LogManager.Log, logStyle);
			}
		}

		// Private members
		private Vector2 pos, size;
		private GUIStyle logStyle;
	}
}
