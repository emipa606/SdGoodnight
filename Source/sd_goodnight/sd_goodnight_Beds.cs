using RimWorld;
using UnityEngine;
using Verse;

namespace sd_goodnight
{
    // Token: 0x02000003 RID: 3
    internal class sd_goodnight_Beds : Building_Bed
    {
        // Token: 0x04000002 RID: 2
        public sd_goodnight_ThingDefsFurniture BedTextures;

        // Token: 0x04000004 RID: 4
        public string MedicalBedTexture;

        // Token: 0x0400000A RID: 10
        public Graphic MedicalGraphic;

        // Token: 0x0400000B RID: 11
        public Graphic MedicalSecondaryGraphic;

        // Token: 0x04000005 RID: 5
        public string MediPrisonerBedTexture;

        // Token: 0x04000009 RID: 9
        public Graphic PrimaryGraphic;

        // Token: 0x04000003 RID: 3
        public string PrisonerBedTexture;

        // Token: 0x04000006 RID: 6
        public string RandPrimary;

        // Token: 0x04000008 RID: 8
        public Graphic SecondaryGraphic;

        // Token: 0x04000007 RID: 7
        public bool usePrimaryTex;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000004 RID: 4 RVA: 0x000020B8 File Offset: 0x000010B8
        public override Graphic Graphic
        {
            get
            {
                Graphic result;
                if (!ForPrisoners)
                {
                    if (Medical)
                    {
                        if (MedicalGraphic == null)
                        {
                            GetGraphics();
                            if (MedicalGraphic == null)
                            {
                                return base.Graphic;
                            }
                        }

                        result = MedicalGraphic;
                    }
                    else
                    {
                        if (PrimaryGraphic == null)
                        {
                            GetGraphics();
                            if (PrimaryGraphic == null)
                            {
                                return base.Graphic;
                            }
                        }

                        result = PrimaryGraphic;
                    }
                }
                else if (Medical)
                {
                    if (MedicalSecondaryGraphic == null)
                    {
                        GetGraphics();
                        if (MedicalSecondaryGraphic == null)
                        {
                            return base.Graphic;
                        }
                    }

                    result = MedicalSecondaryGraphic;
                }
                else
                {
                    if (SecondaryGraphic == null)
                    {
                        GetGraphics();
                        if (MedicalSecondaryGraphic == null)
                        {
                            return base.Graphic;
                        }
                    }

                    result = SecondaryGraphic;
                }

                return result;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002225 File Offset: 0x00001225
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            LongEventHandler.ExecuteWhenFinished(GraphicHandler);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002243 File Offset: 0x00001243
        private void GraphicHandler()
        {
            ReadFromXML();
            GetGraphics();
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002254 File Offset: 0x00001254
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref RandPrimary, "RandPrimary");
            Scribe_Values.Look(ref usePrimaryTex, "usePrimaryTex");
            ReadFromXML();
        }

        // Token: 0x06000008 RID: 8 RVA: 0x0000228C File Offset: 0x0000128C
        private void GetGraphics()
        {
            if (PrisonerBedTexture.NullOrEmpty())
            {
                ReadFromXML();
            }

            if (BedTextures == null)
            {
                BedTextures = (sd_goodnight_ThingDefsFurniture) def;
            }

            if (PrimaryGraphic == null && !def.graphicData.texPath.NullOrEmpty())
            {
                PrimaryGraphic =
                    GraphicDatabase.Get<Graphic_Multi>(!RandPrimary.NullOrEmpty()
                        ? RandPrimary
                        : def.graphicData.texPath);

                PrimaryGraphic.drawSize = def.graphicData.drawSize;
            }

            if (SecondaryGraphic == null && !PrisonerBedTexture.NullOrEmpty())
            {
                SecondaryGraphic = GraphicDatabase.Get<Graphic_Multi>(PrisonerBedTexture);
                SecondaryGraphic.drawSize = def.graphicData.drawSize;
            }

            if (MedicalGraphic == null && !MedicalBedTexture.NullOrEmpty())
            {
                MedicalGraphic = GraphicDatabase.Get<Graphic_Multi>(MedicalBedTexture);
                MedicalGraphic.drawSize = def.graphicData.drawSize;
            }

            if (MedicalSecondaryGraphic != null)
            {
                return;
            }

            if (!MediPrisonerBedTexture.NullOrEmpty())
            {
                MedicalSecondaryGraphic = GraphicDatabase.Get<Graphic_Multi>(MediPrisonerBedTexture);
                MedicalSecondaryGraphic.drawSize = def.graphicData.drawSize;
            }
            else if (!MedicalBedTexture.NullOrEmpty())
            {
                MedicalGraphic = GraphicDatabase.Get<Graphic_Multi>(MedicalBedTexture);
                MedicalGraphic.drawSize = def.graphicData.drawSize;
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x0000247C File Offset: 0x0000147C
        private void ReadFromXML()
        {
            BedTextures = (sd_goodnight_ThingDefsFurniture) def;
            if (BedTextures.PrisonerBedTexture.NullOrEmpty())
            {
                return;
            }

            PrisonerBedTexture = BedTextures.PrisonerBedTexture;
            MedicalBedTexture = BedTextures.MedicalBedTexture;
            MediPrisonerBedTexture = BedTextures.MedicalPrisonerBedTexture;
            if (BedTextures.RandomBedTex.Count <= 0 || PrimaryGraphic != null || usePrimaryTex)
            {
                return;
            }

            float num = Random.Range(0, 100);
            if (num < 10f)
            {
                RandPrimary = BedTextures.RandomBedTex.RandomElement();
            }
            else
            {
                usePrimaryTex = true;
            }
        }
    }
}