using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YandereNext.SURVIVALPROJECT
{
	// Token: 0x0200001A RID: 26
	public class MainMenuManager : MonoBehaviour
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00004B3D File Offset: 0x00002D3D
		public void Awake()
		{
			this.TitleMenuScript = UnityEngine.Object.FindObjectOfType<TitleMenuScript>();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004B4B File Offset: 0x00002D4B
		private void Start()
		{
			base.StartCoroutine(this.Initialize());
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004B5C File Offset: 0x00002D5C
		private void Update()
		{
			bool flag = this.TitleMenuScript.Selected == this.SurvivalModeID && Input.GetButtonDown("A");
			if (flag)
			{
				this.TitleMenuScript.Darkness.color = new Color(0f, 0f, 0f, this.TitleMenuScript.Darkness.color.a);
				this.TitleMenuScript.InputTimer = -10f;
				this.TitleMenuScript.FadeOut = true;
				this.TitleMenuScript.Fading = true;
			}
			bool flag2 = this.TitleMenuScript.Selected == this.SurvivalModeID && this.TitleMenuScript.Fading && this.TitleMenuScript.FadeOut && this.TitleMenuScript.Darkness.color.a >= 1f;
			if (flag2)
			{
				SurvivalModeGlobals.Survival = true;
				GameGlobals.LoveSick = true;
				SceneManager.LoadScene("LoadingScene");
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004C5F File Offset: 0x00002E5F
		private IEnumerator Initialize()
		{
			yield return null;
			List<UILabel> labels = this.TitleMenuScript.ColoredLabels.ToList<UILabel>();
			UILabel missionModeLabel = (from m in labels
			where m.text.Contains("Exit")
			select m).FirstOrDefault<UILabel>();
			bool flag = missionModeLabel != null;
			if (flag)
			{
				this.SurvivalModeID = labels.IndexOf(missionModeLabel) + 1;
				GameObject exitClone = UnityEngine.Object.Instantiate<GameObject>(missionModeLabel.gameObject);
				UILabel SurvivalLabel = exitClone.GetComponent<UILabel>();
				SurvivalLabel.text = "Survival";
				SurvivalLabel.name = "9 Survival Mode";
				SurvivalLabel.transform.rotation = missionModeLabel.transform.rotation;
				SurvivalLabel.transform.position = missionModeLabel.transform.position - missionModeLabel.transform.up * 0.075f;
				SurvivalLabel.UpdateNGUIText();
				labels.Add(SurvivalLabel);
				this.TitleMenuScript.ColoredLabels = labels.ToArray();
				this.TitleMenuScript.SelectionCount++;
				exitClone = null;
				SurvivalLabel = null;
			}
			else
			{
				Debug.Log("It was found");
			}
			yield break;
		}

		// Token: 0x0400003A RID: 58
		private TitleMenuScript TitleMenuScript = null;

		// Token: 0x0400003B RID: 59
		private int SurvivalModeID = 0;
	}
}
