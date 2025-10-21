using RoR2;

namespace ExamplePlugin
{
    public static class ItemReference
    {
        // Token: 0x04005009 RID: 20489
        public static string SolidersSyringe = "ItemIndex.Syringe";

        // Token: 0x0400500A RID: 20490
        public static string TougherTimes = "ItemIndex.Bear";

        // Token: 0x0400500B RID: 20491
        public static string BrilliantBehemoth = "ItemIndex.Behemoth";

        // Token: 0x0400500C RID: 20492
        public static string AtgMissileMk1 = "ItemIndex.Missile";

/*        // Token: 0x0400500D RID: 20493
        public static string ExplodeOnDeath;*/

        // Token: 0x0400500E RID: 20494
        public static string Dagger = "ItemIndex.Dagger";
/*
        // Token: 0x0400500F RID: 20495
        public static string Tooth;*/

        // Token: 0x04005010 RID: 20496
        public static string LensMakersGlasses = "ItemIndex.CritGlasses";

        // Token: 0x04005011 RID: 20497
        public static string GoatHoof = "ItemIndex.Hoof";

        // Token: 0x04005012 RID: 20498
        public static string HopooFeather = "ItemIndex.Feather";

        // Token: 0x04005013 RID: 20499
        public static string Ukulele = "ItemIndex.ChainLightning";

        // Token: 0x04005014 RID: 20500
        public static string LeechingSeed = "ItemIndex.Seed";

        // Token: 0x04005015 RID: 20501
        public static string FrostRelic = "ItemIndex.Icicle";

        // Token: 0x04005016 RID: 20502
        public static string HappiestMask = "ItemIndex.GhostOnKill";

        // Token: 0x04005017 RID: 20503
        public static string BustlingFungus = "ItemIndex.Mushroom";

        // Token: 0x04005018 RID: 20504
        public static string Crowbar = "ItemIndex.Crowbar";

/*        // Token: 0x04005019 RID: 20505
        public static string LevelBonus;*/

        // Token: 0x0400501A RID: 20506
        public static string PredatoryInstincts = "ItemIndex.AttackSpeedOnCrit";

        // Token: 0x0400501B RID: 20507
        public static string TriTipDagger = "ItemIndex.BleedOnHit";

        // Token: 0x0400501C RID: 20508
        public static string RedWhip = "ItemIndex.SprintOutOfCombat";

        // Token: 0x04005025 RID: 20517
        public static string TeslaCoil = "ItemIndex.ShockNearby";

        // Token: 0x0400504E RID: 20558
        public static string OldGuillotine = "ItemIndex.ExecuteLowHealthElite";

        // Token: 0x04005027 RID: 20519
        public static string FiftySevenLeafClover = "ItemIndex.Clover";

        // Token: 0x04005041 RID: 20545
        public static string Chronobauble = "ItemIndex.SlowOnHit";

        // Token: 0x04005026 RID: 20518
        public static string Infusion = "ItemIndex.Infusion";


        // Token: 0x0400503A RID: 20538
        public static string BackupMagazine = "ItemIndex.SecondarySkillMagazine";

        // Token: 0x0400501D RID: 20509
        /*           public static string FallBoots;

                // Token: 0x0400501E RID: 20510
                public static string WardOnLevel;

                // Token: 0x0400501F RID: 20511
                public static string Phasing;

                // Token: 0x04005020 RID: 20512
                public static string HealOnCrit;

                // Token: 0x04005021 RID: 20513
                public static string HealWhileSafe;

                // Token: 0x04005022 RID: 20514
                public static string PersonalShield;

                // Token: 0x04005023 RID: 20515
                public static string EquipmentMagazine;

                // Token: 0x04005024 RID: 20516
                public static string NovaOnHeal;



                // Token: 0x04005028 RID: 20520
                public static string Medkit;

                // Token: 0x04005029 RID: 20521
                public static string Bandolier;

                // Token: 0x0400502A RID: 20522
                public static string BounceNearby;

                // Token: 0x0400502B RID: 20523
                public static string IgniteOnKill;

                // Token: 0x0400502C RID: 20524
                public static string StunChanceOnHit;

                // Token: 0x0400502D RID: 20525
                public static string Firework;

                // Token: 0x0400502E RID: 20526
                public static string LunarDagger;

                // Token: 0x0400502F RID: 20527
                public static string GoldOnHit;

                // Token: 0x04005030 RID: 20528
                public static string WarCryOnMultiKill;

                // Token: 0x04005031 RID: 20529
                public static string BoostHp;

                // Token: 0x04005032 RID: 20530
                public static string BoostDamage;

                // Token: 0x04005033 RID: 20531
                public static string ShieldOnly;

                // Token: 0x04005034 RID: 20532
                public static string AlienHead;

                // Token: 0x04005035 RID: 20533
                public static string Talisman;

                // Token: 0x04005036 RID: 20534
                public static string Knurl;

                // Token: 0x04005037 RID: 20535
                public static string BeetleGland;

                // Token: 0x04005038 RID: 20536
                public static string CrippleWardOnLevel;

                // Token: 0x04005039 RID: 20537
                public static string SprintBonus;


                // Token: 0x0400503B RID: 20539
                public static string StickyBomb;

                // Token: 0x0400503C RID: 20540
                public static string TreasureCache;

                // Token: 0x0400503D RID: 20541
                public static string BossDamageBonus;

                // Token: 0x0400503E RID: 20542
                public static string SprintArmor;

                // Token: 0x0400503F RID: 20543
                public static string IceRing;

                // Token: 0x04005040 RID: 20544
                public static string FireRing;



                // Token: 0x04005042 RID: 20546
                public static string ExtraLife;

                // Token: 0x04005043 RID: 20547
                public static string ExtraLifeConsumed;

                // Token: 0x04005044 RID: 20548
                public static string UtilitySkillMagazine;

                // Token: 0x04005045 RID: 20549
                public static string HeadHunter;

                // Token: 0x04005046 RID: 20550
                public static string KillEliteFrenzy;

                // Token: 0x04005047 RID: 20551
                public static string RepeatHeal;

                // Token: 0x04005048 RID: 20552
                public static string Ghost;

                // Token: 0x04005049 RID: 20553
                public static string HealthDecay;

                // Token: 0x0400504A RID: 20554
                public static string AutoCastEquipment;

                // Token: 0x0400504B RID: 20555
                public static string IncreaseHealing;

                // Token: 0x0400504C RID: 20556
                public static string JumpBoost;

                // Token: 0x0400504D RID: 20557
                public static string DrizzlePlayerHelper;

                // Token: 0x0400504F RID: 20559
                public static string EnergizedOnEquipmentUse;

                // Token: 0x04005050 RID: 20560
                public static string BarrierOnOverHeal;

                // Token: 0x04005051 RID: 20561
                public static string TonicAffliction;

                // Token: 0x04005052 RID: 20562
                public static string TitanGoldDuringTP;

                // Token: 0x04005053 RID: 20563
                public static string SprintWisp;

                // Token: 0x04005054 RID: 20564
                public static string BarrierOnKill;

                // Token: 0x04005055 RID: 20565
                public static string ArmorReductionOnHit;

                // Token: 0x04005056 RID: 20566
                public static string TPHealingNova;

                // Token: 0x04005057 RID: 20567
                public static string NearbyDamageBonus;

                // Token: 0x04005058 RID: 20568
                public static string LunarUtilityReplacement;

                // Token: 0x04005059 RID: 20569
                public static string MonsoonPlayerHelper;

                // Token: 0x0400505A RID: 20570
                public static string Thorns;

                // Token: 0x0400505B RID: 20571
                public static string FlatHealth;

                // Token: 0x0400505C RID: 20572
                public static string Pearl;

                // Token: 0x0400505D RID: 20573
                public static string ShinyPearl;

                // Token: 0x0400505E RID: 20574
                public static string BonusGoldPackOnKill;

                // Token: 0x0400505F RID: 20575
                public static string LaserTurbine;

                // Token: 0x04005060 RID: 20576
                public static string LunarPrimaryReplacement;

                // Token: 0x04005061 RID: 20577
                public static string NovaOnLowHealth;

                // Token: 0x04005062 RID: 20578
                public static string LunarTrinket;

                // Token: 0x04005063 RID: 20579
                public static string InvadingDoppelganger;

                // Token: 0x04005064 RID: 20580
                public static string CutHp;

                // Token: 0x04005065 RID: 20581
                public static string ArtifactKey;

                // Token: 0x04005066 RID: 20582
                public static string ArmorPlate;

                // Token: 0x04005067 RID: 20583
                public static string Squid;

                // Token: 0x04005068 RID: 20584
                public static string DeathMark;

                // Token: 0x04005069 RID: 20585
                public static string Plant;

                // Token: 0x0400506A RID: 20586
                public static string FocusConvergence;

                // Token: 0x0400506B RID: 20587
                public static string BoostAttackSpeed;

                // Token: 0x0400506C RID: 20588
                public static string AdaptiveArmor;

                // Token: 0x0400506D RID: 20589
                public static string CaptainDefenseMatrix;

                // Token: 0x0400506E RID: 20590
                public static string FireballsOnHit;

                // Token: 0x0400506F RID: 20591
                public static string LightningStrikeOnHit;

                // Token: 0x04005070 RID: 20592
                public static string BleedOnHitAndExplode;

                // Token: 0x04005071 RID: 20593
                public static string SiphonOnLowHealth;

                // Token: 0x04005072 RID: 20594
                public static string MonstersOnShrineUse;

                // Token: 0x04005073 RID: 20595
                public static string RandomDamageZone;

                // Token: 0x04005074 RID: 20596
                public static string ScrapWhite;

                // Token: 0x04005075 RID: 20597
                public static string ScrapGreen;

                // Token: 0x04005076 RID: 20598
                public static string ScrapRed;

                // Token: 0x04005077 RID: 20599
                public static string ScrapYellow;

                // Token: 0x04005078 RID: 20600
                public static string LunarBadLuck;

                // Token: 0x04005079 RID: 20601
                public static string BoostEquipmentRecharge;

                // Token: 0x0400507A RID: 20602
                public static string LunarSecondaryReplacement;

                // Token: 0x0400507B RID: 20603
                public static string LunarSpecialReplacement;

                // Token: 0x0400507C RID: 20604
                public static string TeamSizeDamageBonus;

                // Token: 0x0400507D RID: 20605
                public static string RoboBallBuddy;

                // Token: 0x0400507E RID: 20606
                public static string ParentEgg;

                // Token: 0x0400507F RID: 20607
                public static string SummonedEcho;

                // Token: 0x04005080 RID: 20608
                public static string MinionLeash;

                // Token: 0x04005081 RID: 20609
                public static string UseAmbientLevel;

                // Token: 0x04005082 RID: 20610
                public static string TeleportWhenOob;

                // Token: 0x04005083 RID: 20611
                public static string MinHealthPercentage;*/
    }
}