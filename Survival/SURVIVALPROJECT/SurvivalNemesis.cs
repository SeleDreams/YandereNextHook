using System;
using System.Collections;
using UnityEngine;

namespace YandereNext.SURVIVALPROJECT
{
	// Token: 0x0200001E RID: 30
	public class SurvivalNemesis : MonoBehaviour
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00005648 File Offset: 0x00003848
		private void Awake()
		{
			this.Nemesis = base.GetComponent<NemesisScript>();
			this.Nemesis.Student = base.GetComponent<StudentScript>();
			this.Nemesis.Cosmetic = base.GetComponent<CosmeticScript>();
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000567C File Offset: 0x0000387C
		private void Start()
		{
			this.Nemesis.MissionMode.LastKnownPosition.position = this.Nemesis.Yandere.transform.position;
			this.Nemesis.Aggressive = true;
			base.StartCoroutine(this.RandomValues());
			bool paused = this.NemesisManager.SurvivalManager.Paused;
			if (paused)
			{
				this.Nemesis.Student.Pathfinding.speed = 0f;
				this.Nemesis.Student.Pathfinding.canSearch = false;
				this.Nemesis.Student.Pathfinding.canMove = false;
				this.Nemesis.Student.CharacterAnimation.Stop();
				bool flag = this.Nemesis.Student.EventManager != null;
				if (flag)
				{
					this.Nemesis.Student.EventManager.EndEvent();
				}
				this.Nemesis.enabled = false;
			}
			else
			{
				this.Nemesis.Student.StudentID = 2;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005798 File Offset: 0x00003998
		private void LateUpdate()
		{
			bool chibi = this.Chibi;
			if (chibi)
			{
				this.Nemesis.Student.Head.localScale = new Vector3(3f, 3f, 3f);
				this.Nemesis.Student.Character.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
			}
			else
			{
				bool monster = this.Monster;
				if (monster)
				{
					this.Nemesis.Student.LeftHand.localScale = new Vector3(5f, 1f, 3f);
					this.Nemesis.Student.Head.localScale = new Vector3(6f, 2f, 0.5f);
				}
				else
				{
					bool longNecc = this.LongNecc;
					if (longNecc)
					{
						this.Nemesis.Student.Head.localPosition = new Vector3(0f, 3f, 0f);
					}
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000058A8 File Offset: 0x00003AA8
		private IEnumerator AutoDestruction()
		{
			while (this.Nemesis.Student.MyRenderer.isVisible)
			{
				yield return new WaitForSeconds(5f);
			}
			Debug.Log(base.gameObject.name + " Got disabled for performance reasons");
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000058B8 File Offset: 0x00003AB8
		private void Update()
		{
			bool enabled = this.Nemesis.Student.Ragdoll.enabled;
			if (enabled)
			{
				bool attacking = this.Nemesis.Attacking;
				if (attacking)
				{
					this.Nemesis.Yandere.StudentManager.YandereDying = false;
					this.Nemesis.Yandere.YandereVision = false;
					this.Nemesis.Yandere.FollowHips = true;
					this.Nemesis.Yandere.Laughing = false;
					this.Nemesis.Yandere.CanMove = true;
					this.Nemesis.Yandere.EyeShrink = 0f;
				}
				Debug.Log(base.gameObject.name + " has been killed");
				this.NemesisManager.Killed++;
				this.NemesisManager.SurvivalManager.NotificationManager.CustomText = this.NemesisManager.Killed.ToString() + " kills";
				this.NemesisManager.SurvivalManager.NotificationManager.DisplayNotification(NotificationType.Custom);
				this.Nemesis.enabled = false;
				base.enabled = false;
				base.StartCoroutine(this.AutoDestruction());
			}
			else
			{
				bool scarySpawn = this.ScarySpawn;
				if (scarySpawn)
				{
					base.transform.position = this.Nemesis.Yandere.transform.position - this.Nemesis.Yandere.transform.forward * 3f;
					this.ScarySpawn = false;
				}
				this.Timer += Time.deltaTime;
				bool flag = this.Timer >= this.NemesisManager.ReactionTime;
				if (flag)
				{
					bool inView = this.Nemesis.InView;
					if (inView)
					{
						this.Nemesis.Chasing = true;
					}
					else
					{
						this.Nemesis.Chasing = false;
					}
					this.Timer = 0f;
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005AC2 File Offset: 0x00003CC2
		private IEnumerator RandomValues()
		{
			int i = 0;
			while (i < 3)
			{
				int RandomNumber = UnityEngine.Random.Range(1000, 5000);
				int num = RandomNumber;
				int num2 = num;
				int num3 = num2;
				if (num3 <= 3001)
				{
					if (num3 <= 1420)
					{
						if (num3 == 1285)
						{
							goto IL_F7;
						}
						if (num3 == 1420)
						{
							goto IL_105;
						}
					}
					else
					{
						if (num3 == 1580)
						{
							goto IL_E9;
						}
						if (num3 == 2580)
						{
							goto IL_F7;
						}
						if (num3 == 3001)
						{
							goto IL_E9;
						}
					}
				}
				else if (num3 <= 4650)
				{
					if (num3 == 3658)
					{
						goto IL_105;
					}
					if (num3 == 4231 || num3 == 4650)
					{
						goto IL_F7;
					}
				}
				else
				{
					if (num3 == 4658)
					{
						goto IL_105;
					}
					if (num3 == 4820)
					{
						goto IL_E9;
					}
					if (num3 == 4852)
					{
						goto IL_105;
					}
				}
				IL_113:
				yield return null;
				num3 = i;
				i = num3 + 1;
				continue;
				IL_E9:
				this.Chibi = true;
				goto IL_113;
				IL_F7:
				this.Monster = true;
				goto IL_113;
				IL_105:
				this.LongNecc = true;
				goto IL_113;
			}
			yield break;
		}

		// Token: 0x0400005B RID: 91
		public NemesisScript Nemesis;

		// Token: 0x0400005C RID: 92
		public NemesisManager NemesisManager;

		// Token: 0x0400005D RID: 93
		public bool ScarySpawn;

		// Token: 0x0400005E RID: 94
		private float Timer;

		// Token: 0x0400005F RID: 95
		private bool Chibi;

		// Token: 0x04000060 RID: 96
		private bool Monster;

		// Token: 0x04000061 RID: 97
		private bool LongNecc;
	}
}
