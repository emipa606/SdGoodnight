using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace sd_goodnight
{
	// Token: 0x02000003 RID: 3
	internal class sd_goodnight_Beds : Building_Bed
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020B8 File Offset: 0x000010B8
		public override Graphic Graphic
		{
			get
			{
				Graphic result;
				if (!base.ForPrisoners)
				{
					if (base.Medical)
					{
						if (this.MedicalGraphic == null)
						{
							this.GetGraphics();
							if (this.MedicalGraphic == null)
							{
								return base.Graphic;
							}
						}
						result = this.MedicalGraphic;
					}
					else
					{
						if (this.PrimaryGraphic == null)
						{
							this.GetGraphics();
							if (this.PrimaryGraphic == null)
							{
								return base.Graphic;
							}
						}
						result = this.PrimaryGraphic;
					}
				}
				else if (base.Medical)
				{
					if (this.MedicalSecondaryGraphic == null)
					{
						this.GetGraphics();
						if (this.MedicalSecondaryGraphic == null)
						{
							return base.Graphic;
						}
					}
					result = this.MedicalSecondaryGraphic;
				}
				else
				{
					if (this.SecondaryGraphic == null)
					{
						this.GetGraphics();
						if (this.MedicalSecondaryGraphic == null)
						{
							return base.Graphic;
						}
					}
					result = this.SecondaryGraphic;
				}
				return result;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002225 File Offset: 0x00001225
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			LongEventHandler.ExecuteWhenFinished(new Action(this.GraphicHandler));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002243 File Offset: 0x00001243
		private void GraphicHandler()
		{
			this.ReadFromXML();
			this.GetGraphics();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002254 File Offset: 0x00001254
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.RandPrimary, "RandPrimary", null, false);
			Scribe_Values.Look<bool>(ref this.usePrimaryTex, "usePrimaryTex", false, false);
			this.ReadFromXML();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000228C File Offset: 0x0000128C
		private void GetGraphics()
		{
			if (this.PrisonerBedTexture.NullOrEmpty())
			{
				this.ReadFromXML();
			}
			if (this.BedTextures == null)
			{
				this.BedTextures = (sd_goodnight_ThingDefsFurniture)this.def;
			}
			if (this.PrimaryGraphic == null && !this.def.graphicData.texPath.NullOrEmpty())
			{
				if (!this.RandPrimary.NullOrEmpty())
				{
					this.PrimaryGraphic = GraphicDatabase.Get<Graphic_Multi>(this.RandPrimary);
				}
				else
				{
					this.PrimaryGraphic = GraphicDatabase.Get<Graphic_Multi>(this.def.graphicData.texPath);
				}
				this.PrimaryGraphic.drawSize = this.def.graphicData.drawSize;
			}
			if (this.SecondaryGraphic == null && !this.PrisonerBedTexture.NullOrEmpty())
			{
				this.SecondaryGraphic = GraphicDatabase.Get<Graphic_Multi>(this.PrisonerBedTexture);
				this.SecondaryGraphic.drawSize = this.def.graphicData.drawSize;
			}
			if (this.MedicalGraphic == null && !this.MedicalBedTexture.NullOrEmpty())
			{
				this.MedicalGraphic = GraphicDatabase.Get<Graphic_Multi>(this.MedicalBedTexture);
				this.MedicalGraphic.drawSize = this.def.graphicData.drawSize;
			}
			if (this.MedicalSecondaryGraphic == null)
			{
				if (!this.MediPrisonerBedTexture.NullOrEmpty())
				{
					this.MedicalSecondaryGraphic = GraphicDatabase.Get<Graphic_Multi>(this.MediPrisonerBedTexture);
					this.MedicalSecondaryGraphic.drawSize = this.def.graphicData.drawSize;
				}
				else if (!this.MedicalBedTexture.NullOrEmpty())
				{
					this.MedicalGraphic = GraphicDatabase.Get<Graphic_Multi>(this.MedicalBedTexture);
					this.MedicalGraphic.drawSize = this.def.graphicData.drawSize;
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000247C File Offset: 0x0000147C
		private void ReadFromXML()
		{
			this.BedTextures = (sd_goodnight_ThingDefsFurniture)this.def;
			if (!this.BedTextures.PrisonerBedTexture.NullOrEmpty())
			{
				this.PrisonerBedTexture = this.BedTextures.PrisonerBedTexture;
				this.MedicalBedTexture = this.BedTextures.MedicalBedTexture;
				this.MediPrisonerBedTexture = this.BedTextures.MedicalPrisonerBedTexture;
				if (this.BedTextures.RandomBedTex.Count > 0 && this.PrimaryGraphic == null && !this.usePrimaryTex)
				{
					float num = (float)UnityEngine.Random.Range(0, 100);
					if (num < 10f)
					{
						this.RandPrimary = this.BedTextures.RandomBedTex.RandomElement<string>();
					}
					else
					{
						this.usePrimaryTex = true;
					}
				}
			}
		}

		// Token: 0x04000002 RID: 2
		public sd_goodnight_ThingDefsFurniture BedTextures = null;

		// Token: 0x04000003 RID: 3
		public string PrisonerBedTexture;

		// Token: 0x04000004 RID: 4
		public string MedicalBedTexture;

		// Token: 0x04000005 RID: 5
		public string MediPrisonerBedTexture;

		// Token: 0x04000006 RID: 6
		public string RandPrimary;

		// Token: 0x04000007 RID: 7
		public bool usePrimaryTex = false;

		// Token: 0x04000008 RID: 8
		public Graphic SecondaryGraphic;

		// Token: 0x04000009 RID: 9
		public Graphic PrimaryGraphic;

		// Token: 0x0400000A RID: 10
		public Graphic MedicalGraphic;

		// Token: 0x0400000B RID: 11
		public Graphic MedicalSecondaryGraphic;
	}
}
