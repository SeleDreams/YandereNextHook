using System;
using UnityEngine;

namespace YandereNext.Debugging
{
	// Token: 0x02000016 RID: 22
	public class LogConsole : MonoBehaviour
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000041FA File Offset: 0x000023FA
		private void Clear()
		{
			LogManager.Clear();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000042B0 File Offset: 0x000024B0
		private void Start()
		{
			this.pos = new Vector2(2f, 2f);
			this.size = new Vector2(200f, 400f);
			this.logStyle = new GUIStyle();
			this.logStyle.normal.textColor = Color.blue;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004309 File Offset: 0x00002509
		private void OnGUI()
		{
			
		}

		// Token: 0x04000024 RID: 36
		public static bool log = true;

		// Token: 0x04000025 RID: 37
		private Vector2 pos;

		// Token: 0x04000026 RID: 38
		private Vector2 size;

		// Token: 0x04000027 RID: 39
		private GUIStyle logStyle;
	}
}
