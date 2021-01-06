using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace YandereNext.SURVIVALPROJECT
{
	// Token: 0x0200001D RID: 29
	public class NemesisManager : MonoBehaviour
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x0000545C File Offset: 0x0000365C
		private void Awake()
		{
			base.StartCoroutine(this.InitializeNemesis());
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000546C File Offset: 0x0000366C
		private void Update()
		{
			bool flag = this.OriginalSurvivalNemesis != null && !this.SurvivalManager.Paused && Time.timeScale > 0.3f;
			if (flag)
			{
				bool flag2 = this.Killed != this.PreviousKilled;
				if (flag2)
				{
					this.PreviousKilled = this.Killed;
					this.SurvivalManager.Clock.PeriodLabel.text = this.Killed.ToString() + " Kills";
				}
				this.TimeBeforeNextSpawn -= Time.deltaTime;
				bool flag3 = this.TimeBeforeNextSpawn < 0f;
				if (flag3)
				{
					this.TimeBeforeNextSpawn = 60f / (float)this.SurvivalManager.DifficultyLevel;
					this.Spawn();
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000553F File Offset: 0x0000373F
		public IEnumerator InitializeNemesis()
		{
			NemesisScript originalNemesis = Resources.FindObjectsOfTypeAll<NemesisScript>().FirstOrDefault<NemesisScript>();
			bool flag = originalNemesis != null;
			if (flag)
			{
				originalNemesis.gameObject.SetActive(false);
				GameObject instance = UnityEngine.Object.Instantiate<GameObject>(originalNemesis.gameObject);
				SurvivalNemesis survivalNemesis = instance.AddComponent<SurvivalNemesis>();
				survivalNemesis.NemesisManager = this;
				this.OriginalSurvivalNemesis = survivalNemesis;
				int num;
				for (int i = 0; i < this.BeginningNemesisCount; i = num + 1)
				{
					this.Spawn();
					yield return null;
					num = i;
				}
				instance = null;
				survivalNemesis = null;
			}
			else
			{
				Debug.LogError("No Nemesis found, aborting.");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield break;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005550 File Offset: 0x00003750
		public void Spawn()
		{
			bool flag = this.OriginalSurvivalNemesis == null;
			if (flag)
			{
				Debug.Log("yep there's a problem");
			}
			bool flag2 = this.ScarySpawnCounter >= this.NumberOfSpawnsBeforeScarySpawn;
			if (flag2)
			{
				SurvivalNemesis survivalNemesis = UnityEngine.Object.Instantiate<SurvivalNemesis>(this.OriginalSurvivalNemesis);
				survivalNemesis.gameObject.SetActive(true);
				survivalNemesis.ScarySpawn = true;
				this.ScarySpawnCounter = 0;
			}
			else
			{
				UnityEngine.Object.Instantiate<SurvivalNemesis>(this.OriginalSurvivalNemesis).gameObject.SetActive(true);
				this.ScarySpawnCounter++;
			}
		}

		// Token: 0x04000051 RID: 81
		public SurvivalManager SurvivalManager = null;

		// Token: 0x04000052 RID: 82
		public SurvivalNemesis OriginalSurvivalNemesis = null;

		// Token: 0x04000053 RID: 83
		public float ReactionTime = 2f;

		// Token: 0x04000054 RID: 84
		public float TimeBeforeNextSpawn = 0f;

		// Token: 0x04000055 RID: 85
		public int BeginningNemesisCount = 15;

		// Token: 0x04000056 RID: 86
		public int Spawned = 0;

		// Token: 0x04000057 RID: 87
		public int Killed = 0;

		// Token: 0x04000058 RID: 88
		private int PreviousKilled = 0;

		// Token: 0x04000059 RID: 89
		private int NumberOfSpawnsBeforeScarySpawn = 20;

		// Token: 0x0400005A RID: 90
		private int ScarySpawnCounter = 0;
	}
}
