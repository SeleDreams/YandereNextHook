using System;
using UnityEngine;

namespace YandereNext.SURVIVALPROJECT
{
	// Token: 0x02000019 RID: 25
	internal class LoadingScreenManager : MonoBehaviour
	{
		// Token: 0x0600008E RID: 142 RVA: 0x00004A89 File Offset: 0x00002C89
		private void Awake()
		{
			this.keyboard = GameObject.Find("Keyboard");
			this.gamepad = GameObject.Find("Gamepad");
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004AAC File Offset: 0x00002CAC
		private void Start()
		{
			UnityEngine.Object.Destroy(this.keyboard);
			UnityEngine.Object.Destroy(this.gamepad);
			this.SurvivalModLoadingText();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004AD0 File Offset: 0x00002CD0
		private void SurvivalModLoadingText()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameObject.Find("CrashLabel"));
			UILabel component = gameObject.GetComponent<UILabel>();
			component.transform.position = gameObject.transform.position + gameObject.transform.up * 1.5f;
		}

		// Token: 0x04000038 RID: 56
		private GameObject keyboard = null;

		// Token: 0x04000039 RID: 57
		private GameObject gamepad = null;
	}
}
