using System;
using System.Collections;
using UnityEngine;

namespace YandereNext.SURVIVALPROJECT
{
	// Token: 0x02000018 RID: 24
	public class EasterEggManager : MonoBehaviour
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004314 File Offset: 0x00002514
		private void Start()
		{
			this.CurrentReloadTimer = this.ReloadTimer;
			this.SurvivalManager.NotificationManager.Yandere = new YandereScript();
			this.SurvivalManager.NotificationManager.Yandere.Egg = false;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000434E File Offset: 0x0000254E
		private void Update()
		{
			this.UpdateEasterEggs();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004358 File Offset: 0x00002558
		private void DisableEasterEgg()
		{
			switch (this.CurrentEasterEgg)
			{
			case EasterEggs.BadTime:
				this.SurvivalManager.YandereChan.Sans = false;
				break;
			case EasterEggs.Falcon:
				this.SurvivalManager.YandereChan.FalconHelmet.SetActive(false);
				break;
			case EasterEggs.OnePunch:
				this.SurvivalManager.YandereChan.Cape.SetActive(false);
				break;
			case EasterEggs.Cirno:
				this.SurvivalManager.YandereChan.CirnoHair.SetActive(false);
				this.SurvivalManager.YandereChan.Hairstyle = 1;
				this.SurvivalManager.YandereChan.UpdateHair();
				break;
			case EasterEggs.Nier:
				this.SurvivalManager.YandereChan.Pod.SetActive(false);
				break;
			case EasterEggs.Witch:
				this.SurvivalManager.YandereChan.WitchMode = false;
				break;
			}
			this.SurvivalManager.YandereChan.Egg = false;
			this.SurvivalManager.YandereChan.CanMove = true;
			this.CurrentEasterEgg = EasterEggs.None;
			this.EasterEggName = string.Empty;
			this.CurrentReloadTimer = this.ReloadTimer;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004488 File Offset: 0x00002688
		private void SetRandomEasterEgg()
		{
			bool flag = this.CurrentEasterEgg > EasterEggs.None;
			if (flag)
			{
				this.DisableEasterEgg();
			}
			int currentEasterEgg = UnityEngine.Random.Range(1, Enum.GetNames(typeof(EasterEggs)).Length);
			switch (currentEasterEgg)
			{
			case 1:
				this.SurvivalManager.YandereChan.Sans = true;
				this.Ammo = 10f;
				this.EasterEggName = "BAD TIME MODE";
				break;
			case 2:
				this.SurvivalManager.YandereChan.FalconHelmet.SetActive(true);
				this.Ammo = 5f;
				this.EasterEggName = "FALCON MODE";
				break;
			case 3:
				this.SurvivalManager.YandereChan.Cape.SetActive(true);
				this.Ammo = 5f;
				this.EasterEggName = "ONE PUNCH MODE";
				break;
			case 4:
				this.SurvivalManager.YandereChan.CirnoHair.SetActive(true);
				this.SurvivalManager.YandereChan.Hairstyle = 0;
				this.SurvivalManager.YandereChan.UpdateHair();
				this.Ammo = 10f;
				this.EasterEggName = "CIRNO MODE";
				break;
			case 5:
				this.SurvivalManager.YandereChan.Pod.SetActive(true);
				this.Ammo = 250f;
				this.EasterEggName = "NIER MODE";
				break;
			case 6:
				this.SurvivalManager.YandereChan.WitchMode = true;
				this.Ammo = 4f;
				this.EasterEggName = "WITCH MODE";
				break;
			}
			this.CurrentEasterEgg = (EasterEggs)currentEasterEgg;
			this.SurvivalManager.YandereChan.Egg = true;
			this.SurvivalManager.Subtitles.Label.text = this.EasterEggName;
			this.UpdateAmmo();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004664 File Offset: 0x00002864
		private void UpdateEasterEggs()
		{
			bool flag = this.CurrentEasterEgg != EasterEggs.None && this.CurrentEasterEgg != EasterEggs.Nier;
			if (flag)
			{
				bool flag2 = !this.waiting && this.SurvivalManager.YandereChan.CanMove && Input.GetButtonDown("RB");
				if (flag2)
				{
					base.StartCoroutine(this.WaitForEnd());
				}
			}
			else
			{
				bool flag3 = this.CurrentEasterEgg == EasterEggs.Nier;
				if (flag3)
				{
					this.Nier();
				}
				else
				{
					this.CurrentReloadTimer -= Time.deltaTime;
					bool flag4 = this.previousValueTimer != (int)this.CurrentReloadTimer;
					if (flag4)
					{
						this.SurvivalManager.Subtitles.Label.text = "RELOADING " + ((int)this.CurrentReloadTimer).ToString();
						this.previousValueTimer = (int)this.CurrentReloadTimer;
					}
					bool flag5 = this.CurrentReloadTimer <= 0f;
					if (flag5)
					{
						this.SurvivalManager.Subtitles.Label.text = string.Empty;
						this.SetRandomEasterEgg();
					}
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004790 File Offset: 0x00002990
		private void Nier()
		{
			bool flag = this.SurvivalManager.YandereChan.CanMove && Input.GetButton("RB");
			if (flag)
			{
				this.Ammo -= 1f;
				this.UpdateAmmo();
			}
			else
			{
				bool flag2 = Input.GetButtonDown("Y") || Input.GetButtonDown("X");
				if (flag2)
				{
					this.Ammo -= 1f;
					this.UpdateAmmo();
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004818 File Offset: 0x00002A18
		private void WitchTime()
		{
			bool flag = !this.SurvivalManager.Paused;
			if (flag)
			{
				this.SurvivalManager.Paused = true;
				NemesisScript[] array = UnityEngine.Object.FindObjectsOfType<NemesisScript>();
				foreach (NemesisScript nemesisScript in array)
				{
					nemesisScript.Student.Pathfinding.speed = 0f;
					nemesisScript.Student.Pathfinding.canSearch = false;
					nemesisScript.Student.Pathfinding.canMove = false;
					nemesisScript.Student.CharacterAnimation.Stop();
					bool flag2 = nemesisScript.Student.EventManager != null;
					if (flag2)
					{
						nemesisScript.Student.EventManager.EndEvent();
					}
					nemesisScript.enabled = false;
				}
			}
			else
			{
				this.SurvivalManager.Paused = false;
				NemesisScript[] array3 = UnityEngine.Object.FindObjectsOfType<NemesisScript>();
				foreach (NemesisScript nemesisScript2 in array3)
				{
					nemesisScript2.Student.Pathfinding.speed = (float)(nemesisScript2.Chasing ? 5 : 1);
					nemesisScript2.Student.Pathfinding.canSearch = true;
					nemesisScript2.Student.Pathfinding.canMove = true;
					nemesisScript2.Student.CharacterAnimation.Play();
					nemesisScript2.enabled = true;
					bool flag3 = nemesisScript2.Student.StudentID < 2;
					if (flag3)
					{
						nemesisScript2.Student.StudentID = 2;
					}
				}
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000049B4 File Offset: 0x00002BB4
		private void UpdateAmmo()
		{
			this.SurvivalManager.NotificationManager.CustomText = Math.Max(0f, this.Ammo).ToString() + " LEFT";
			this.SurvivalManager.NotificationManager.DisplayNotification(NotificationType.Custom);
			bool flag = this.Ammo <= 0f;
			if (flag)
			{
				this.DisableEasterEgg();
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004A24 File Offset: 0x00002C24
		public IEnumerator WaitForEnd()
		{
			this.waiting = true;
			bool flag = this.CurrentEasterEgg == EasterEggs.Witch;
			if (flag)
			{
				this.WitchTime();
			}
			while (this.SurvivalManager.YandereChan.CanMove)
			{
				yield return new WaitForEndOfFrame();
			}
			while (!this.SurvivalManager.YandereChan.CanMove)
			{
				yield return new WaitForEndOfFrame();
			}
			this.waiting = false;
			this.Ammo -= 1f;
			this.UpdateAmmo();
			yield break;
		}

		// Token: 0x04000030 RID: 48
		public EasterEggs CurrentEasterEgg = EasterEggs.None;

		// Token: 0x04000031 RID: 49
		public float ReloadTimer = 5f;

		// Token: 0x04000032 RID: 50
		public float CurrentReloadTimer = 0f;

		// Token: 0x04000033 RID: 51
		private int previousValueTimer = 0;

		// Token: 0x04000034 RID: 52
		public float Ammo = 0f;

		// Token: 0x04000035 RID: 53
		private string EasterEggName = string.Empty;

		// Token: 0x04000036 RID: 54
		public SurvivalManager SurvivalManager = null;

		// Token: 0x04000037 RID: 55
		private bool waiting;
	}
}
