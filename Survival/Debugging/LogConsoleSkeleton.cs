using System;

namespace YandereNext.Debugging
{
	// Token: 0x02000014 RID: 20
	public class LogConsoleSkeleton
	{
		// Token: 0x06000074 RID: 116 RVA: 0x000041C8 File Offset: 0x000023C8
		public static LogConsoleSkeleton CreateInstance()
		{
			return new LogConsoleSkeleton();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000041DF File Offset: 0x000023DF
		public void ToggleLog()
		{
			this.DisplayLog(!LogConsole.log);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000041F1 File Offset: 0x000023F1
		public void DisplayLog(bool command)
		{
			LogConsole.log = command;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000041FA File Offset: 0x000023FA
		public void Clear()
		{
			LogManager.Clear();
		}
	}
}
