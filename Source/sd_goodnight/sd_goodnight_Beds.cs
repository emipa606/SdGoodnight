using RimWorld;
using UnityEngine;
using Verse;

namespace sd_goodnight;

internal class sd_goodnight_Beds : Building_Bed
{
    public sd_goodnight_ThingDefsFurniture BedTextures;

    public string MedicalBedTexture;

    public Graphic MedicalGraphic;

    public Graphic MedicalSecondaryGraphic;

    public string MediPrisonerBedTexture;

    public Graphic PrimaryGraphic;

    public string PrisonerBedTexture;

    public string RandPrimary;

    public Graphic SecondaryGraphic;

    public bool usePrimaryTex;

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

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        LongEventHandler.ExecuteWhenFinished(GraphicHandler);
    }

    private void GraphicHandler()
    {
        ReadFromXML();
        GetGraphics();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref RandPrimary, "RandPrimary");
        Scribe_Values.Look(ref usePrimaryTex, "usePrimaryTex");
        ReadFromXML();
    }

    private void GetGraphics()
    {
        if (PrisonerBedTexture.NullOrEmpty())
        {
            ReadFromXML();
        }

        if (BedTextures == null)
        {
            BedTextures = (sd_goodnight_ThingDefsFurniture)def;
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

    private void ReadFromXML()
    {
        BedTextures = (sd_goodnight_ThingDefsFurniture)def;
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