using System;
using System.Collections.Generic;
using UnityEngine;

namespace YandereNext.Debugging
{
	// Token: 0x02000015 RID: 21
	internal class LogManager
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00004203 File Offset: 0x00002403
		public static string Log
		{
			get
			{
				return LogManager.logContent;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000420A File Offset: 0x0000240A
		public static void Clear()
		{
			LogManager.logMessages.Clear();
			LogManager.logContent = "";
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004222 File Offset: 0x00002422
		public static void SetupLog()
		{
			Application.logMessageReceived += new Application.LogCallback(LogManager.Register);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004238 File Offset: 0x00002438
		private static void Register(string message, string stackTrace, LogType logtype)
		{
			bool flag = LogManager.logMessages.Count == LogManager._maxLines;
			if (flag)
			{
				LogManager.logMessages.RemoveAt(0);
			}
			LogManager.logMessages.Add(message);
			string[] value = logMessages.ToArray();
			LogManager.logContent = string.Join("\n", value);
		}

		// Token: 0x04000021 RID: 33
		private static List<string> logMessages = new List<string>();

		// Token: 0x04000022 RID: 34
		private static int _maxLines = 20;

		// Token: 0x04000023 RID: 35
		private static string logContent = "";
	}
}
