using System;
using Server.Items;



namespace Server.Engines.Harvest
{
    public class ReagentGathering : HarvestSystem
    {
        public override HarvestVein MutateVein(Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein)
        {
            return base.MutateVein(from, tool, def, bank, toHarvest, vein);
        }

        private static ReagentGathering m_System;

        public static ReagentGathering System
        {
            get
            {
                if (m_System == null)
                    m_System = new ReagentGathering();

                return m_System;
            }
        }

        private HarvestDefinition m_Definition;

        public HarvestDefinition Definition
        {
            get { return m_Definition; }
        }

        private ReagentGathering()
        {
            HarvestResource[] res;
            HarvestVein[] veins;

            #region ReagentGathering
            HarvestDefinition reagent = new HarvestDefinition();

            reagent.BankWidth = 2;
            reagent.BankHeight = 2;

            reagent.MinTotal = 0;
            reagent.MaxTotal = 4;

            reagent.MinRespawn = TimeSpan.FromMinutes(20.0);
            reagent.MaxRespawn = TimeSpan.FromMinutes(30.0);

            // Skill checking is done on the Magery skill
            reagent.Skill = SkillName.Magery;

            // Set the list of harvestable tiles
            reagent.Tiles = m_Tiles;

            reagent.MaxRange = 2;

            reagent.ConsumedPerHarvest = 1;
            reagent.ConsumedPerFeluccaHarvest = 1;

            // The chopping effect
            reagent.EffectActions = new int[] { 32 };
            reagent.EffectSounds = new int[] { 0x04F };
            reagent.EffectCounts = new int[] { 1 };
            reagent.EffectDelay = TimeSpan.FromSeconds(1.3);
            reagent.EffectSoundDelay = TimeSpan.FromSeconds(0.9);

            reagent.NoResourcesMessage = "You can't seem to find any reagents here.";
            reagent.FailMessage = "You try to harvest some reagents but this area is depleted.";
            reagent.OutOfRangeMessage = 500446; // That is too far away.
            reagent.PackFullMessage = "Your pack is too full to carry any more reagents.";
            reagent.ToolBrokeMessage = "You broke your tool.";

            res = new HarvestResource[]
            {
                new HarvestResource( 20, 30, 100.0, "You put some Bloodmoss in your backpack",   typeof( Bloodmoss ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Black Pearl in your backpack",   typeof( BlackPearl ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Garlic in your backpack",     typeof( Garlic ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Ginseng in your backpack",     typeof( Ginseng ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Sulfurous Ash in your backpack",     typeof( SulfurousAsh ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Mandrake Root in your backpack",   typeof( MandrakeRoot ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Nightshade in your backpack",   typeof( Nightshade ) ),
                new HarvestResource( 20, 30, 100.0, "You put some Spiders Silk in your backpack",   typeof( SpidersSilk ) ),
            };

            veins = new HarvestVein[]
            {
                 new HarvestVein( 10.0, 0.0, res[0], null ),
                 new HarvestVein( 10.0, 0.2, res[1], res[0] ),
                 new HarvestVein( 10.0, 0.2, res[2], res[1] ),
                 new HarvestVein( 10.0, 0.2, res[3], res[2] ),
                 new HarvestVein( 10.0, 0.2, res[4], res[3] ),
                 new HarvestVein( 10.0, 0.2, res[5], res[4] ),
                 new HarvestVein( 10.0, 0.2, res[6], res[5] ),
                 new HarvestVein( 10.0, 0.2, res[7], res[6] ),
            };

            reagent.Resources = res;
            reagent.Veins = veins;

            m_Definition = reagent;
            Definitions.Add(reagent);
            #endregion
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            if (!base.CheckHarvest(from, tool))
                return false;

            if (from.Mounted)
            {
                from.SendMessage("You can't harvest reagents while mounted.");
                return false;
            }

            return true;
        }

        public override void DoHarvestingEffect(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc)
        {
            from.Direction = from.GetDirectionTo(loc);

            if (!from.Mounted)
            {
                from.Animate(Utility.RandomList(def.EffectActions), 5, 1, true, false, 0);
            }
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (from.Mounted)
            {
                from.SendMessage("You can't harvest reagents while mounted.");
                return false;
            }

            return true;
        }

        public override void OnBadHarvestTarget(Mobile from, Item tool, object toHarvest)
        {
            from.SendMessage("You can't harvest reagents from that.");
        }

        public static void Initialize()
        {
            Array.Sort(m_Tiles);
        }

        #region Tile lists
        private static int[] m_Tiles = new int[]
        {
            0x0003, 0x0004, 0x0005, 0x0006,

            172,            173,            174,            175,            176,            177,            178,
            179,            180,            181,            182,            183,            184,            185,            186,
            187,            188,            189,            190,            191,            192,            193,            194,
            195,            196,            197,            198,            199,            200,            201,            202,
            203,            204,            205,            206,            207,            208,            209,            210,
            211,            212,            213,            214,            215,            216,            217,            218,

            0x4CCA, 0x4CCB, 0x4CCC, 0x4CCD, 0x4CD0, 0x4CD3, 0x4CD6, 0x4CD8,
            0x4CDA, 0x4CDD, 0x4CE0, 0x4CE3, 0x4CE6, 0x4CF8, 0x4CFB, 0x4CFE,
            0x4D01, 0x4D41, 0x4D42, 0x4D43, 0x4D44, 0x4D57, 0x4D58, 0x4D59,
            0x4D5A, 0x4D5B, 0x4D6E, 0x4D6F, 0x4D70, 0x4D71, 0x4D72, 0x4D84,
            0x4D85, 0x4D86, 0x52B5, 0x52B6, 0x52B7, 0x52B8, 0x52B9, 0x52BA,
            0x52BB, 0x52BC, 0x52BD,

            0x4CCE, 0x4CCF, 0x4CD1, 0x4CD2, 0x4CD4, 0x4CD5, 0x4CD7, 0x4CD9,
            0x4CDB, 0x4CDC, 0x4CDE, 0x4CDF, 0x4CE1, 0x4CE2, 0x4CE4, 0x4CE5,
            0x4CE7, 0x4CE8, 0x4CF9, 0x4CFA, 0x4CFC, 0x4CFD, 0x4CFF, 0x4D00,
            0x4D02, 0x4D03, 0x4D45, 0x4D46, 0x4D47, 0x4D48, 0x4D49, 0x4D4A,
            0x4D4B, 0x4D4C, 0x4D4D, 0x4D4E, 0x4D4F, 0x4D50, 0x4D51, 0x4D52,
            0x4D53, 0x4D5C, 0x4D5D, 0x4D5E, 0x4D5F, 0x4D60, 0x4D61, 0x4D62,
            0x4D63, 0x4D64, 0x4D65, 0x4D66, 0x4D67, 0x4D68, 0x4D69, 0x4D73,
            0x4D74, 0x4D75, 0x4D76, 0x4D77, 0x4D78, 0x4D79, 0x4D7A, 0x4D7B,
            0x4D7C, 0x4D7D, 0x4D7E, 0x4D7F, 0x4D87, 0x4D88, 0x4D89, 0x4D8A,
            0x4D8B, 0x4D8C, 0x4D8D, 0x4D8E, 0x4D8F, 0x4D90, 0x4D95, 0x4D96,
            0x4D97, 0x4D99, 0x4D9A, 0x4D9B, 0x4D9D, 0x4D9E, 0x4D9F, 0x4DA1,
            0x4DA2, 0x4DA3, 0x4DA5, 0x4DA6, 0x4DA7, 0x4DA9, 0x4DAA, 0x4DAB,
            0x52BE, 0x52BF, 0x52C0, 0x52C1, 0x52C2, 0x52C3, 0x52C4, 0x52C5,
            0x52C6, 0x52C7, 0x647D, 0x647E, 0x6476, 0x6477, 0x624A, 0x624B,
            0x624C, 0x624D, 0x4D94, 0x4D98, 0x4D9C, 0x4DA4, 0x4DA8, 0x70A1,
            0x709C, 0x70BD, 0x70C3, 0x70D4, 0x70DA, 0xDA0
        };
        #endregion
    }
}
