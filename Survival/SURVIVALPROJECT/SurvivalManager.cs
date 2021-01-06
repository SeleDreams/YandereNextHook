using System;
using System.Collections;
using UnityEngine;

namespace YandereNext.SURVIVALPROJECT
{
	// Token: 0x0200001B RID: 27
	public class SurvivalManager : MonoBehaviour
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00004C88 File Offset: 0x00002E88
		private void Awake()
		{
			MissionModeGlobals.MissionMode = true;
			MissionModeGlobals.NemesisDifficulty = 4;
			this.StudentManager = UnityEngine.Object.FindObjectOfType<StudentManagerScript>();
			this.MissionModeScript = UnityEngine.Object.FindObjectOfType<MissionModeScript>();
			this.YandereChan = UnityEngine.Object.FindObjectOfType<YandereScript>();
			this.NotificationManager = UnityEngine.Object.FindObjectOfType<NotificationManagerScript>();
			this.Subtitles = UnityEngine.Object.FindObjectOfType<SubtitleScript>();
			this.Clock = UnityEngine.Object.FindObjectOfType<ClockScript>();
			this.RichPresenceHelper = UnityEngine.Object.FindObjectOfType<RichPresenceHelper>();
			this.DiscordController = UnityEngine.Object.FindObjectOfType<DiscordController>();
			this.RichPresenceHelper.enabled = false;
			this.Clock.enabled = false;
			base.StartCoroutine(this.RemoveStudents());
			base.StartCoroutine(this.LoadMusic());
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004D48 File Offset: 0x00002F48
		private IEnumerator LoadMusic()
		{
			/*WWW fileLoader = new WWW("file://" + YandereNextManager.ModDir + "/music/dreams.ogg");
			while (!fileLoader.isDone)
			{
				yield return null;
			}
			AudioClip audio = WWWAudioExtensions.GetAudioClip(fileLoader);
			this.MissionModeScript.Jukebox.MissionMode.clip = audio;
			this.MissionModeScript.Jukebox.MissionMode.Play();*/
			yield break;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004D58 File Offset: 0x00002F58
		private void OnDisable()
		{
			this.RichPresenceHelper.enabled = true;
			bool flag = this.MissionModeScript.Destination != 1;
			if (flag)
			{
				SurvivalModeGlobals.Survival = false;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004D90 File Offset: 0x00002F90
		private void Start()
		{
			this.NemesisManager = base.gameObject.AddComponent<NemesisManager>();
			this.NemesisManager.SurvivalManager = this;
			this.EasterEggManager = base.gameObject.AddComponent<EasterEggManager>();
			this.EasterEggManager.SurvivalManager = this;
			this.MissionModeScript.Watermark.text = "SURVIVAL MOD REVIVAL (BETA) BY SELEDREAMS (AKA Pikachuk)";
			this.Clock.DayLabel.gameObject.SetActive(true);
			this.Clock.DayLabel.text = "";
			this.Clock.TimeLabel.text = "TIME : 00:00";
			this.Clock.PeriodLabel.text = "0 Kills";
			this.MissionModeScript.MoneyLabel.enabled = false;
			this.MissionModeScript.ReputationLabel.text = "DIFFICULTY";
			this.MissionModeScript.ExitPortal.SetActive(false);
			UnityEngine.Object.Destroy(this.MissionModeScript.Headmaster);
			this.MissionModeScript.HeartbeatCamera.SetActive(false);
			this.MissionModeScript.WoodChipper.VictimList = new int[0];
			this.MissionModeScript.WoodChipper.Victims = 0;
			this.MissionModeScript.WoodChipper.VictimID = 0;
			this.DifficultyLevel = 1;
			this.EasterEggManager.ReloadTimer = (float)this.DifficultyLevel;
			this.DifficultyIncreaseTime = this.SecondsToSurvive;
			this.DiscordController.presence.state = string.Concat(new object[]
			{
				"SURVIVING, ",
				this.NemesisManager.Killed,
				" KILLS, ",
				"TIME : 00:00"
			});
			DiscordRpc.UpdatePresence(DiscordController.presence);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004F58 File Offset: 0x00003158
		private void Update()
		{
			bool flag = !this.Paused && Time.timeScale > 0.3f && !this.YandereChan.Hiding;
			if (flag)
			{
				this.SecondsSurvived += Time.deltaTime;
				this.UpdateDifficulty();
				this.UpdateRichPresence();
				this.UpdateClock();
			}
			else
			{
				bool flag2 = !this.GameOver && Time.timeScale < 0.3f && this.MissionModeScript.GameOverPhase > 0;
				if (flag2)
				{
					UILabel gameOverHeader = this.MissionModeScript.GameOverHeader;
					switch (UnityEngine.Random.Range(1, 11))
					{
					case 1:
						gameOverHeader.text = "NOT A BIG SURPRISE...";
						break;
					case 2:
						gameOverHeader.text = "SERIOUSLY?";
						break;
					case 3:
						gameOverHeader.text = "AS I THOUGHT!";
						break;
					case 4:
						gameOverHeader.text = "ARE YOU GONNA CRY?";
						break;
					case 5:
						gameOverHeader.text = "OK.";
						break;
					case 6:
						gameOverHeader.text = "NICE!";
						break;
					case 7:
						gameOverHeader.text = "I LOVE DYING!";
						break;
					case 8:
						gameOverHeader.text = "I SAID 0 DEATHS!";
						break;
					case 9:
						gameOverHeader.text = "I BELIEVE IN YOUR FAILURE!";
						break;
					case 10:
						gameOverHeader.text = "NEMESIS : 1, YOU : 0";
						break;
					default:
						gameOverHeader.text = ";-;";
						break;
					}
					this.MissionModeScript.GameOverReason.text = "YOU DIED AFTER KILLING " + this.NemesisManager.Killed.ToString() + " NEMESIS.\n" + this.Clock.TimeLabel.text;
					this.GameOver = true;
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005124 File Offset: 0x00003324
		private void UpdateRichPresence()
		{
			int num;
			int num2;
			int num3;
			this.SecondsToTime((int)this.SecondsSurvived, out num, out num2, out num3);
			bool flag = (float)num2 != this.previousMinutes;
			if (flag)
			{
				this.DiscordController.presence.state = string.Concat(new object[]
				{
					"SURVIVING, ",
					this.NemesisManager.Killed,
					" KILLS, ",
					this.Clock.TimeLabel.text
				});
				DiscordRpc.UpdatePresence(DiscordController.presence);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000051BC File Offset: 0x000033BC
		private void UpdateClock()
		{
			bool flag = this.SecondsSurvived > 0f && this.previousHour < 99f;
			if (flag)
			{
				int num;
				int num2;
				int num3;
				this.SecondsToTime((int)this.SecondsSurvived, out num, out num2, out num3);
				bool flag2 = (float)num3 != this.previousSeconds || (float)num2 != this.previousMinutes || (float)num != this.previousHour;
				if (flag2)
				{
					this.previousSeconds = (float)num3;
					this.previousMinutes = (float)num2;
					this.previousHour = (float)num;
					string text = string.Empty;
					bool flag3 = num == 0;
					if (flag3)
					{
						text = "TIME : " + num2.ToString("00") + ":" + num3.ToString("00");
					}
					else
					{
						text = string.Concat(new string[]
						{
							"TIME : ",
							num.ToString("00"),
							":",
							num2.ToString("00"),
							":",
							num3.ToString("00")
						});
					}
					this.Clock.TimeLabel.text = text;
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000052F0 File Offset: 0x000034F0
		private void SecondsToTime(int baseSeconds, out int hours, out int minutes, out int seconds)
		{
			seconds = baseSeconds % 60;
			minutes = baseSeconds / 60;
			hours = minutes / 60;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005308 File Offset: 0x00003508
		private void UpdateDifficulty()
		{
			bool flag = this.DifficultyLevel < 99 && this.SecondsSurvived >= this.DifficultyIncreaseTime;
			if (flag)
			{
				this.DifficultyLevel++;
				this.EasterEggManager.ReloadTimer = (float)this.DifficultyLevel;
				bool flag2 = this.DifficultyLevel < 99;
				if (flag2)
				{
					this.MissionModeScript.Reputation.PendingRep = (float)(-(float)this.DifficultyLevel);
					this.DifficultyIncreaseTime *= 2f;
				}
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005394 File Offset: 0x00003594
		private IEnumerator RemoveStudents()
		{
			while (this.StudentManager.SpawnID < this.StudentManager.NPCsTotal || this.StudentManager.NPCsTotal == 0)
			{
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			StudentScript[] students = UnityEngine.Object.FindObjectsOfType<StudentScript>();
			foreach (StudentScript student in students)
			{
				bool flag = student.name != "Nemesis";
				if (flag)
				{
					student.gameObject.SetActive(false);
				}
			}
			StudentScript[] array = null;
			yield break;
		}

		// Token: 0x0400003C RID: 60
		public NemesisManager NemesisManager = null;

		// Token: 0x0400003D RID: 61
		public EasterEggManager EasterEggManager = null;

		// Token: 0x0400003E RID: 62
		public MissionModeScript MissionModeScript = null;

		// Token: 0x0400003F RID: 63
		public StudentManagerScript StudentManager = null;

		// Token: 0x04000040 RID: 64
		public YandereScript YandereChan = null;

		// Token: 0x04000042 RID: 66
		public ClockScript Clock = null;

		// Token: 0x04000043 RID: 67
		public NotificationManagerScript NotificationManager = null;

		// Token: 0x04000044 RID: 68
		public DiscordController DiscordController = null;

		// Token: 0x04000045 RID: 69
		public SubtitleScript Subtitles = null;

		// Token: 0x04000046 RID: 70
		private RichPresenceHelper RichPresenceHelper = null;

		// Token: 0x04000047 RID: 71
		public float SecondsSurvived = 0f;

		// Token: 0x04000048 RID: 72
		public float DifficultyIncreaseTime = 0f;

		// Token: 0x04000049 RID: 73
		public float SecondsToSurvive = 20f;

		// Token: 0x0400004A RID: 74
		public int DifficultyLevel = 1;

		// Token: 0x0400004B RID: 75
		public bool Paused = false;

		// Token: 0x0400004C RID: 76
		private float previousMinutes = 0f;

		// Token: 0x0400004D RID: 77
		private float previousHour = 0f;

		// Token: 0x0400004E RID: 78
		private float previousSeconds = 0f;

		// Token: 0x0400004F RID: 79
		private bool GameOver = false;
	}
}
