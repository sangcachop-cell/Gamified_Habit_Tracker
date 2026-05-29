using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddArmoireGearItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "IsArmoire",
                value: false);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 64,
                column: "IsArmoire",
                value: false);

            migrationBuilder.InsertData(
                table: "GearItems",
                columns: new[] { "Id", "CON", "GearClass", "GoldCost", "INT", "IsArmoire", "IsSpecial", "Key", "Name", "PER", "STR", "Slot", "Tier", "TwoHanded" },
                values: new object[,]
                {
                    { 86, 0, "armoire", 0, 7, true, false, "armor_armoire_lunarArmor", "Lunar Armor", 0, 7, "armor", 0, false },
                    { 87, 0, "armoire", 0, 0, true, false, "armor_armoire_gladiatorArmor", "Gladiator Armor", 7, 7, "armor", 0, false },
                    { 88, 0, "armoire", 0, 5, true, false, "armor_armoire_rancherRobes", "Rancher Robes", 5, 5, "armor", 0, false },
                    { 89, 8, "armoire", 0, 0, true, false, "armor_armoire_goldenToga", "Golden Toga", 0, 8, "armor", 0, false },
                    { 90, 9, "armoire", 0, 0, true, false, "armor_armoire_hornedIronArmor", "Horned Iron Armor", 7, 0, "armor", 0, false },
                    { 91, 6, "armoire", 0, 6, true, false, "armor_armoire_plagueDoctorOvercoat", "Plague Doctor Overcoat", 0, 5, "armor", 0, false },
                    { 92, 0, "armoire", 0, 0, true, false, "armor_armoire_shepherdRobes", "Shepherd Robes", 9, 9, "armor", 0, false },
                    { 93, 5, "armoire", 0, 5, true, false, "armor_armoire_royalRobes", "Royal Robes", 5, 0, "armor", 0, false },
                    { 94, 7, "armoire", 0, 0, true, false, "armor_armoire_crystalCrescentRobes", "Crystal Crescent Robes", 7, 0, "armor", 0, false },
                    { 95, 15, "armoire", 0, 0, true, false, "armor_armoire_dragonTamerArmor", "Dragon Tamer Armor", 0, 0, "armor", 0, false },
                    { 96, 10, "armoire", 0, 0, true, false, "armor_armoire_barristerRobes", "Barrister Robes", 0, 0, "armor", 0, false },
                    { 97, 0, "armoire", 0, 15, true, false, "armor_armoire_jesterCostume", "Jester Costume", 0, 0, "armor", 0, false },
                    { 98, 10, "armoire", 0, 0, true, false, "armor_armoire_minerOveralls", "Miner Overalls", 0, 0, "armor", 0, false },
                    { 99, 0, "armoire", 0, 0, true, false, "armor_armoire_basicArcherArmor", "Basic Archer Armor", 12, 0, "armor", 0, false },
                    { 100, 0, "armoire", 0, 10, true, false, "armor_armoire_graduateRobe", "Graduate Robe", 0, 0, "armor", 0, false },
                    { 101, 13, "armoire", 0, 0, true, false, "armor_armoire_stripedSwimsuit", "Striped Swimsuit", 0, 0, "armor", 0, false },
                    { 102, 15, "armoire", 0, 0, true, false, "armor_armoire_cannoneerRags", "Cannoneer Rags", 0, 0, "armor", 0, false },
                    { 103, 10, "armoire", 0, 0, true, false, "armor_armoire_falconerArmor", "Falconer Armor", 0, 0, "armor", 0, false },
                    { 104, 0, "armoire", 0, 0, true, false, "armor_armoire_vermilionArcherArmor", "Vermilion Archer Armor", 15, 0, "armor", 0, false },
                    { 105, 15, "armoire", 0, 0, true, false, "armor_armoire_ogreArmor", "Ogre Armor", 0, 0, "armor", 0, false },
                    { 106, 0, "armoire", 0, 0, true, false, "armor_armoire_ironBlueArcherArmor", "Iron Blue Archer Armor", 0, 12, "armor", 0, false },
                    { 107, 7, "armoire", 0, 7, true, false, "armor_armoire_redPartyDress", "Red Party Dress", 0, 7, "armor", 0, false },
                    { 108, 0, "armoire", 0, 0, true, false, "armor_armoire_woodElfArmor", "Wood Elf Armor", 12, 0, "armor", 0, false },
                    { 109, 9, "armoire", 0, 0, true, false, "armor_armoire_ramFleeceRobes", "Ram Fleece Robes", 0, 7, "armor", 0, false },
                    { 110, 13, "armoire", 0, 0, true, false, "armor_armoire_gownOfHearts", "Gown Of Hearts", 0, 0, "armor", 0, false },
                    { 111, 7, "armoire", 0, 0, true, false, "armor_armoire_mushroomDruidArmor", "Mushroom Druid Armor", 8, 0, "armor", 0, false },
                    { 112, 8, "armoire", 0, 0, true, false, "armor_armoire_greenFestivalYukata", "Green Festival Yukata", 8, 0, "armor", 0, false },
                    { 113, 0, "armoire", 0, 0, true, false, "armor_armoire_merchantTunic", "Merchant Tunic", 10, 0, "armor", 0, false },
                    { 114, 6, "armoire", 0, 0, true, false, "armor_armoire_vikingTunic", "Viking Tunic", 0, 8, "armor", 0, false },
                    { 115, 0, "armoire", 0, 8, true, false, "armor_armoire_swanDancerTutu", "Swan Dancer Tutu", 0, 8, "armor", 0, false },
                    { 116, 0, "armoire", 0, 7, true, false, "armor_armoire_yellowPartyDress", "Yellow Party Dress", 7, 7, "armor", 0, false },
                    { 117, 0, "armoire", 0, 0, true, false, "armor_armoire_antiProcrastinationArmor", "Anti Procrastination Armor", 0, 15, "armor", 0, false },
                    { 118, 6, "armoire", 0, 6, true, false, "armor_armoire_farrierOutfit", "Farrier Outfit", 6, 0, "armor", 0, false },
                    { 119, 12, "armoire", 0, 0, true, false, "armor_armoire_candlestickMakerOutfit", "Candlestick Maker Outfit", 0, 0, "armor", 0, false },
                    { 120, 8, "armoire", 0, 9, true, false, "armor_armoire_wovenRobes", "Woven Robes", 0, 0, "armor", 0, false },
                    { 121, 0, "armoire", 0, 0, true, false, "armor_armoire_lamplightersGreatcoat", "Lamplighters Greatcoat", 14, 0, "armor", 0, false },
                    { 122, 0, "armoire", 0, 0, true, false, "armor_armoire_coachDriverLivery", "Coach Driver Livery", 0, 12, "armor", 0, false },
                    { 123, 0, "armoire", 0, 0, true, false, "armor_armoire_robeOfDiamonds", "Robe Of Diamonds", 13, 0, "armor", 0, false },
                    { 124, 5, "armoire", 0, 0, true, false, "armor_armoire_flutteryFrock", "Fluttery Frock", 5, 5, "armor", 0, false },
                    { 125, 0, "armoire", 0, 0, true, false, "armor_armoire_cobblersCoveralls", "Cobblers Coveralls", 7, 7, "armor", 0, false },
                    { 126, 8, "armoire", 0, 0, true, false, "armor_armoire_glassblowersCoveralls", "Glassblowers Coveralls", 0, 0, "armor", 0, false },
                    { 127, 7, "armoire", 0, 0, true, false, "armor_armoire_bluePartyDress", "Blue Party Dress", 7, 7, "armor", 0, false },
                    { 128, 0, "armoire", 0, 0, true, false, "armor_armoire_piraticalPrincessGown", "Piratical Princess Gown", 7, 0, "armor", 0, false },
                    { 129, 15, "armoire", 0, 0, true, false, "armor_armoire_jeweledArcherArmor", "Jeweled Archer Armor", 0, 0, "armor", 0, false },
                    { 130, 10, "armoire", 0, 0, true, false, "armor_armoire_coverallsOfBookbinding", "Coveralls Of Bookbinding", 5, 0, "armor", 0, false },
                    { 131, 0, "armoire", 0, 0, true, false, "armor_armoire_robeOfSpades", "Robe Of Spades", 0, 13, "armor", 0, false },
                    { 132, 0, "armoire", 0, 10, true, false, "armor_armoire_softBlueSuit", "Soft Blue Suit", 5, 0, "armor", 0, false },
                    { 133, 7, "armoire", 0, 7, true, false, "armor_armoire_softGreenSuit", "Soft Green Suit", 0, 0, "armor", 0, false },
                    { 134, 0, "armoire", 0, 8, true, false, "armor_armoire_softRedSuit", "Soft Red Suit", 0, 5, "armor", 0, false },
                    { 135, 0, "armoire", 0, 7, true, false, "armor_armoire_scribesRobe", "Scribes Robe", 7, 0, "armor", 0, false },
                    { 136, 0, "armoire", 0, 10, true, false, "armor_armoire_chefsJacket", "Chefs Jacket", 0, 0, "armor", 0, false },
                    { 137, 0, "armoire", 0, 6, true, false, "armor_armoire_vernalVestment", "Vernal Vestment", 0, 6, "armor", 0, false },
                    { 138, 0, "armoire", 0, 0, true, false, "armor_armoire_nephriteArmor", "Nephrite Armor", 6, 7, "armor", 0, false },
                    { 139, 0, "armoire", 0, 6, true, false, "armor_armoire_boatingJacket", "Boating Jacket", 6, 6, "armor", 0, false },
                    { 140, 8, "armoire", 0, 0, true, false, "armor_armoire_astronomersRobe", "Astronomers Robe", 8, 0, "armor", 0, false },
                    { 141, 0, "armoire", 0, 7, true, false, "armor_armoire_invernessCape", "Inverness Cape", 7, 0, "armor", 0, false },
                    { 142, 12, "armoire", 0, 0, true, false, "armor_armoire_shadowMastersRobe", "Shadow Masters Robe", 0, 0, "armor", 0, false },
                    { 143, 8, "armoire", 0, 0, true, false, "armor_armoire_alchemistsRobe", "Alchemists Robe", 5, 0, "armor", 0, false },
                    { 144, 7, "armoire", 0, 0, true, false, "armor_armoire_duffleCoat", "Duffle Coat", 7, 0, "armor", 0, false },
                    { 145, 13, "armoire", 0, 0, true, false, "armor_armoire_layerCakeArmor", "Layer Cake Armor", 0, 0, "armor", 0, false },
                    { 146, 7, "armoire", 0, 7, true, false, "armor_armoire_matchMakersApron", "Match Makers Apron", 0, 7, "armor", 0, false },
                    { 147, 10, "armoire", 0, 0, true, false, "armor_armoire_baseballUniform", "Baseball Uniform", 0, 10, "armor", 0, false },
                    { 148, 5, "armoire", 0, 0, true, false, "armor_armoire_boxArmor", "Box Armor", 5, 0, "armor", 0, false },
                    { 149, 6, "armoire", 0, 0, true, false, "armor_armoire_fiddlersCoat", "Fiddlers Coat", 0, 0, "armor", 0, false },
                    { 150, 4, "armoire", 0, 4, true, false, "armor_armoire_pirateOutfit", "Pirate Outfit", 0, 0, "armor", 0, false },
                    { 151, 7, "armoire", 0, 7, true, false, "armor_armoire_heroicHerbalistRobe", "Heroic Herbalist Robe", 0, 0, "armor", 0, false },
                    { 152, 0, "armoire", 0, 7, true, false, "armor_armoire_guardiansGown", "Guardians Gown", 0, 0, "armor", 0, false },
                    { 153, 0, "armoire", 0, 12, true, false, "armor_armoire_autumnEnchantersCloak", "Autumn Enchanters Cloak", 0, 0, "armor", 0, false },
                    { 154, 10, "armoire", 0, 0, true, false, "armor_armoire_doubletOfClubs", "Doublet Of Clubs", 0, 0, "armor", 0, false },
                    { 155, 12, "armoire", 0, 0, true, false, "armor_armoire_dressingGown", "Dressing Gown", 0, 0, "armor", 0, false },
                    { 156, 8, "armoire", 0, 0, true, false, "armor_armoire_blueMoonShozoku", "Blue Moon Shozoku", 0, 0, "armor", 0, false },
                    { 157, 0, "armoire", 0, 0, true, false, "armor_armoire_softPinkSuit", "Soft Pink Suit", 12, 0, "armor", 0, false },
                    { 158, 0, "armoire", 0, 0, true, false, "armor_armoire_jadeArmor", "Jade Armor", 8, 0, "armor", 0, false },
                    { 159, 0, "armoire", 0, 0, true, false, "armor_armoire_clownsMotley", "Clowns Motley", 0, 7, "armor", 0, false },
                    { 160, 6, "armoire", 0, 0, true, false, "armor_armoire_medievalLaundryOutfit", "Medieval Laundry Outfit", 0, 0, "armor", 0, false },
                    { 161, 6, "armoire", 0, 0, true, false, "armor_armoire_medievalLaundryDress", "Medieval Laundry Dress", 0, 0, "armor", 0, false },
                    { 162, 8, "armoire", 0, 0, true, false, "armor_armoire_bathtub", "Bathtub", 0, 0, "armor", 0, false },
                    { 163, 6, "armoire", 0, 0, true, false, "armor_armoire_bagpipersKilt", "Bagpipers Kilt", 0, 0, "armor", 0, false },
                    { 164, 6, "armoire", 0, 0, true, false, "armor_armoire_heraldsTunic", "Heralds Tunic", 0, 0, "armor", 0, false },
                    { 165, 7, "armoire", 0, 0, true, false, "armor_armoire_softBlackSuit", "Soft Black Suit", 7, 0, "armor", 0, false },
                    { 166, 10, "armoire", 0, 0, true, false, "armor_armoire_shootingStarCostume", "Shooting Star Costume", 0, 0, "armor", 0, false },
                    { 167, 7, "armoire", 0, 0, true, false, "armor_armoire_softVioletSuit", "Soft Violet Suit", 0, 7, "armor", 0, false },
                    { 168, 7, "armoire", 0, 0, true, false, "armor_armoire_gardenersOveralls", "Gardeners Overalls", 0, 0, "armor", 0, false },
                    { 169, 9, "armoire", 0, 0, true, false, "armor_armoire_strawRaincoat", "Straw Raincoat", 0, 0, "armor", 0, false },
                    { 170, 4, "armoire", 0, 4, true, false, "armor_armoire_fancyPirateSuit", "Fancy Pirate Suit", 0, 0, "armor", 0, false },
                    { 171, 10, "armoire", 0, 0, true, false, "armor_armoire_sheetGhostCostume", "Sheet Ghost Costume", 0, 0, "armor", 0, false },
                    { 172, 0, "armoire", 0, 10, true, false, "armor_armoire_jewelersApron", "Jewelers Apron", 0, 0, "armor", 0, false },
                    { 173, 8, "armoire", 0, 0, true, false, "armor_armoire_shawlCollarCoat", "Shawl Collar Coat", 0, 0, "armor", 0, false },
                    { 174, 0, "armoire", 0, 5, true, false, "armor_armoire_teaGown", "Tea Gown", 0, 5, "armor", 0, false },
                    { 175, 0, "armoire", 0, 0, true, false, "armor_armoire_basketballUniform", "Basketball Uniform", 10, 0, "armor", 0, false },
                    { 176, 10, "armoire", 0, 0, true, false, "armor_armoire_paintersApron", "Painters Apron", 0, 0, "armor", 0, false },
                    { 177, 0, "armoire", 0, 7, true, false, "armor_armoire_stripedRainbowShirt", "Striped Rainbow Shirt", 0, 7, "armor", 0, false },
                    { 178, 7, "armoire", 0, 0, true, false, "armor_armoire_diagonalRainbowShirt", "Diagonal Rainbow Shirt", 7, 0, "armor", 0, false },
                    { 179, 7, "armoire", 0, 0, true, false, "armor_armoire_admiralsUniform", "Admirals Uniform", 0, 7, "armor", 0, false },
                    { 180, 0, "armoire", 0, 0, true, false, "armor_armoire_karateGi", "Karate Gi", 0, 10, "armor", 0, false },
                    { 181, 0, "armoire", 0, 8, true, false, "armor_armoire_greenFluffTrimmedCoat", "Green Fluff Trimmed Coat", 0, 8, "armor", 0, false },
                    { 182, 0, "armoire", 0, 5, true, false, "armor_armoire_schoolUniformSkirt", "School Uniform Skirt", 0, 0, "armor", 0, false },
                    { 183, 0, "armoire", 0, 5, true, false, "armor_armoire_schoolUniformPants", "School Uniform Pants", 0, 0, "armor", 0, false },
                    { 184, 7, "armoire", 0, 0, true, false, "armor_armoire_softWhiteSuit", "Soft White Suit", 10, 0, "armor", 0, false },
                    { 185, 9, "armoire", 0, 0, true, false, "armor_armoire_hattersSuit", "Hatters Suit", 0, 0, "armor", 0, false },
                    { 186, 0, "armoire", 0, 4, true, false, "armor_armoire_smileyShirt", "Smiley Shirt", 4, 0, "armor", 0, false },
                    { 187, 0, "armoire", 0, 0, true, false, "armor_armoire_pottersApron", "Potters Apron", 0, 8, "armor", 0, false },
                    { 188, 13, "armoire", 0, 0, true, false, "armor_armoire_yellowStripedSwimsuit", "Yellow Striped Swimsuit", 0, 0, "armor", 0, false },
                    { 189, 13, "armoire", 0, 0, true, false, "armor_armoire_blueStripedSwimsuit", "Blue Striped Swimsuit", 0, 0, "armor", 0, false },
                    { 190, 14, "armoire", 0, 0, true, false, "armor_armoire_corsairsCoatAndCape", "Corsairs Coat And Cape", 0, 0, "armor", 0, false },
                    { 191, 0, "armoire", 0, 0, true, false, "armor_armoire_dragonKnightsArmor", "Dragon Knights Armor", 0, 8, "armor", 0, false },
                    { 192, 0, "armoire", 0, 0, true, false, "armor_armoire_funnyFoolCostume", "Funny Fool Costume", 0, 15, "armor", 0, false },
                    { 193, 0, "armoire", 0, 0, true, false, "armor_armoire_stormKnightArmor", "Storm Knight Armor", 11, 0, "armor", 0, false },
                    { 194, 12, "armoire", 0, 0, true, false, "armor_armoire_festiveHelperOveralls", "Festive Helper Overalls", 0, 0, "armor", 0, false },
                    { 195, 0, "armoire", 0, 6, true, false, "armor_armoire_snowyFluffTrimmedCoat", "Snowy Fluff Trimmed Coat", 0, 6, "armor", 0, false },
                    { 196, 8, "armoire", 0, 0, true, false, "armor_armoire_springPetalYukata", "Spring Petal Yukata", 0, 8, "armor", 0, false },
                    { 197, 12, "armoire", 0, 0, true, false, "armor_armoire_sillyOrangeTuxedo", "Silly Orange Tuxedo", 0, 0, "armor", 0, false },
                    { 198, 0, "armoire", 0, 0, true, false, "armor_armoire_sillierBlueTuxedo", "Sillier Blue Tuxedo", 0, 12, "armor", 0, false },
                    { 199, 0, "armoire", 0, 0, true, false, "armor_armoire_gildedKnightsPlate", "Gilded Knights Plate", 11, 0, "armor", 0, false },
                    { 200, 12, "armoire", 0, 0, true, false, "armor_armoire_beekeepersSuit", "Beekeepers Suit", 0, 0, "armor", 0, false },
                    { 201, 7, "armoire", 0, 0, true, false, "armor_armoire_flyFishingWaders", "Fly Fishing Waders", 0, 7, "armor", 0, false },
                    { 202, 8, "armoire", 0, 0, true, false, "armor_armoire_redWaistcoat", "Red Waistcoat", 0, 8, "armor", 0, false },
                    { 203, 8, "armoire", 0, 0, true, false, "armor_armoire_softOrangeSuit", "Soft Orange Suit", 0, 8, "armor", 0, false },
                    { 204, 7, "armoire", 0, 7, true, false, "armor_armoire_blackPartyDress", "Black Party Dress", 0, 7, "armor", 0, false },
                    { 205, 11, "armoire", 0, 0, true, false, "armor_armoire_blacksmithsApron", "Blacksmiths Apron", 0, 0, "armor", 0, false },
                    { 206, 10, "armoire", 0, 0, true, false, "armor_armoire_loneCowpokeOutfit", "Lone Cowpoke Outfit", 0, 0, "armor", 0, false },
                    { 207, 9, "armoire", 0, 0, true, false, "armor_armoire_softYellowSuit", "Soft Yellow Suit", 0, 9, "armor", 0, false },
                    { 208, 0, "armoire", 0, 0, true, false, "armor_armoire_handstandOutfit", "Handstand Outfit", 10, 0, "armor", 0, false },
                    { 209, 7, "armoire", 0, 0, true, false, "armor_armoire_kendoBogu", "Kendo Bogu", 0, 0, "armor", 0, false },
                    { 210, 0, "armoire", 0, 6, true, false, "back_armoire_harpsichord", "Harpsichord", 6, 0, "back", 0, false },
                    { 211, 5, "armoire", 0, 0, true, false, "body_armoire_cozyScarf", "Cozy Scarf", 5, 0, "body", 0, false },
                    { 212, 0, "armoire", 0, 12, true, false, "body_armoire_lifeguardWhistle", "Lifeguard Whistle", 0, 0, "body", 0, false },
                    { 213, 2, "armoire", 0, 2, true, false, "body_armoire_clownsBowtie", "Clowns Bowtie", 2, 2, "body", 0, false },
                    { 214, 0, "armoire", 0, 0, true, false, "body_armoire_karateYellowBelt", "Karate Yellow Belt", 3, 0, "body", 0, false },
                    { 215, 0, "armoire", 0, 3, true, false, "body_armoire_karateWhiteBelt", "Karate White Belt", 0, 0, "body", 0, false },
                    { 216, 0, "armoire", 0, 0, true, false, "body_armoire_karateRedBelt", "Karate Red Belt", 3, 0, "body", 0, false },
                    { 217, 3, "armoire", 0, 0, true, false, "body_armoire_karatePurpleBelt", "Karate Purple Belt", 0, 0, "body", 0, false },
                    { 218, 3, "armoire", 0, 0, true, false, "body_armoire_karateOrangeBelt", "Karate Orange Belt", 0, 0, "body", 0, false },
                    { 219, 0, "armoire", 0, 0, true, false, "body_armoire_karateGreenBelt", "Karate Green Belt", 0, 3, "body", 0, false },
                    { 220, 0, "armoire", 0, 0, true, false, "body_armoire_karateBrownBelt", "Karate Brown Belt", 0, 3, "body", 0, false },
                    { 221, 3, "armoire", 0, 0, true, false, "body_armoire_karateBlueBelt", "Karate Blue Belt", 0, 0, "body", 0, false },
                    { 222, 0, "armoire", 0, 3, true, false, "body_armoire_karateBlackBelt", "Karate Black Belt", 0, 0, "body", 0, false },
                    { 223, 5, "armoire", 0, 5, true, false, "eyewear_armoire_plagueDoctorMask", "Plague Doctor Mask", 0, 0, "eyewear", 0, false },
                    { 224, 0, "armoire", 0, 0, true, false, "eyewear_armoire_goofyGlasses", "Goofy Glasses", 10, 0, "eyewear", 0, false },
                    { 225, 0, "armoire", 0, 5, true, false, "eyewear_armoire_clownsNose", "Clowns Nose", 0, 0, "eyewear", 0, false },
                    { 226, 0, "armoire", 0, 10, true, false, "eyewear_armoire_tragedyMask", "Tragedy Mask", 0, 0, "eyewear", 0, false },
                    { 227, 10, "armoire", 0, 0, true, false, "eyewear_armoire_comedyMask", "Comedy Mask", 0, 0, "eyewear", 0, false },
                    { 228, 0, "armoire", 0, 0, true, false, "eyewear_armoire_jewelersEyeLoupe", "Jewelers Eye Loupe", 10, 0, "eyewear", 0, false },
                    { 229, 0, "armoire", 0, 0, true, false, "eyewear_armoire_roseColoredGlasses", "Rose Colored Glasses", 8, 0, "eyewear", 0, false },
                    { 230, 7, "armoire", 0, 0, true, false, "head_armoire_lunarCrown", "Lunar Crown", 7, 0, "head", 0, false },
                    { 231, 5, "armoire", 0, 5, true, false, "head_armoire_redHairbow", "Red Hairbow", 0, 5, "head", 0, false },
                    { 232, 5, "armoire", 0, 5, true, false, "head_armoire_violetFloppyHat", "Violet Floppy Hat", 5, 0, "head", 0, false },
                    { 233, 0, "armoire", 0, 7, true, false, "head_armoire_gladiatorHelm", "Gladiator Helm", 7, 0, "head", 0, false },
                    { 234, 0, "armoire", 0, 5, true, false, "head_armoire_rancherHat", "Rancher Hat", 5, 5, "head", 0, false },
                    { 235, 0, "armoire", 0, 0, true, false, "head_armoire_royalCrown", "Royal Crown", 0, 10, "head", 0, false },
                    { 236, 5, "armoire", 0, 5, true, false, "head_armoire_blueHairbow", "Blue Hairbow", 5, 0, "head", 0, false },
                    { 237, 8, "armoire", 0, 0, true, false, "head_armoire_goldenLaurels", "Golden Laurels", 8, 0, "head", 0, false },
                    { 238, 9, "armoire", 0, 0, true, false, "head_armoire_hornedIronHelm", "Horned Iron Helm", 0, 7, "head", 0, false },
                    { 239, 0, "armoire", 0, 5, true, false, "head_armoire_yellowHairbow", "Yellow Hairbow", 5, 5, "head", 0, false },
                    { 240, 6, "armoire", 0, 6, true, false, "head_armoire_redFloppyHat", "Red Floppy Hat", 6, 0, "head", 0, false },
                    { 241, 5, "armoire", 0, 5, true, false, "head_armoire_plagueDoctorHat", "Plague Doctor Hat", 0, 6, "head", 0, false },
                    { 242, 0, "armoire", 0, 9, true, false, "head_armoire_blackCat", "Black Cat", 9, 0, "head", 0, false },
                    { 243, 9, "armoire", 0, 0, true, false, "head_armoire_orangeCat", "Orange Cat", 0, 9, "head", 0, false },
                    { 244, 7, "armoire", 0, 7, true, false, "head_armoire_blueFloppyHat", "Blue Floppy Hat", 7, 0, "head", 0, false },
                    { 245, 0, "armoire", 0, 9, true, false, "head_armoire_shepherdHeaddress", "Shepherd Headdress", 0, 0, "head", 0, false },
                    { 246, 0, "armoire", 0, 7, true, false, "head_armoire_crystalCrescentHat", "Crystal Crescent Hat", 7, 0, "head", 0, false },
                    { 247, 0, "armoire", 0, 15, true, false, "head_armoire_dragonTamerHelm", "Dragon Tamer Helm", 0, 0, "head", 0, false },
                    { 248, 0, "armoire", 0, 0, true, false, "head_armoire_barristerWig", "Barrister Wig", 0, 10, "head", 0, false },
                    { 249, 0, "armoire", 0, 0, true, false, "head_armoire_jesterCap", "Jester Cap", 15, 0, "head", 0, false },
                    { 250, 0, "armoire", 0, 5, true, false, "head_armoire_minerHelmet", "Miner Helmet", 0, 0, "head", 0, false },
                    { 251, 0, "armoire", 0, 0, true, false, "head_armoire_basicArcherCap", "Basic Archer Cap", 6, 0, "head", 0, false },
                    { 252, 0, "armoire", 0, 9, true, false, "head_armoire_graduateCap", "Graduate Cap", 0, 0, "head", 0, false },
                    { 253, 8, "armoire", 0, 8, true, false, "head_armoire_greenFloppyHat", "Green Floppy Hat", 8, 0, "head", 0, false },
                    { 254, 0, "armoire", 0, 15, true, false, "head_armoire_cannoneerBandanna", "Cannoneer Bandanna", 15, 0, "head", 0, false },
                    { 255, 0, "armoire", 0, 10, true, false, "head_armoire_falconerCap", "Falconer Cap", 0, 0, "head", 0, false },
                    { 256, 0, "armoire", 0, 0, true, false, "head_armoire_vermilionArcherHelm", "Vermilion Archer Helm", 12, 0, "head", 0, false },
                    { 257, 7, "armoire", 0, 0, true, false, "head_armoire_ogreMask", "Ogre Mask", 0, 7, "head", 0, false },
                    { 258, 9, "armoire", 0, 0, true, false, "head_armoire_ironBlueArcherHelm", "Iron Blue Archer Helm", 0, 0, "head", 0, false },
                    { 259, 12, "armoire", 0, 0, true, false, "head_armoire_woodElfHelm", "Wood Elf Helm", 0, 0, "head", 0, false },
                    { 260, 9, "armoire", 0, 0, true, false, "head_armoire_ramHeaddress", "Ram Headdress", 7, 0, "head", 0, false },
                    { 261, 0, "armoire", 0, 0, true, false, "head_armoire_crownOfHearts", "Crown Of Hearts", 0, 13, "head", 0, false },
                    { 262, 0, "armoire", 0, 6, true, false, "head_armoire_mushroomDruidCap", "Mushroom Druid Cap", 0, 7, "head", 0, false },
                    { 263, 0, "armoire", 0, 7, true, false, "head_armoire_merchantChaperon", "Merchant Chaperon", 7, 0, "head", 0, false },
                    { 264, 0, "armoire", 0, 0, true, false, "head_armoire_vikingHelm", "Viking Helm", 8, 6, "head", 0, false },
                    { 265, 0, "armoire", 0, 8, true, false, "head_armoire_swanFeatherCrown", "Swan Feather Crown", 0, 0, "head", 0, false },
                    { 266, 0, "armoire", 0, 0, true, false, "head_armoire_antiProcrastinationHelm", "Anti Procrastination Helm", 15, 0, "head", 0, false },
                    { 267, 0, "armoire", 0, 6, true, false, "head_armoire_candlestickMakerHat", "Candlestick Maker Hat", 6, 0, "head", 0, false },
                    { 268, 14, "armoire", 0, 0, true, false, "head_armoire_lamplightersTopHat", "Lamplighters Top Hat", 0, 0, "head", 0, false },
                    { 269, 0, "armoire", 0, 12, true, false, "head_armoire_coachDriversHat", "Coach Drivers Hat", 0, 0, "head", 0, false },
                    { 270, 0, "armoire", 0, 13, true, false, "head_armoire_crownOfDiamonds", "Crown Of Diamonds", 0, 0, "head", 0, false },
                    { 271, 0, "armoire", 0, 5, true, false, "head_armoire_flutteryWig", "Fluttery Wig", 5, 5, "head", 0, false },
                    { 272, 0, "armoire", 0, 0, true, false, "head_armoire_bigWig", "Big Wig", 0, 10, "head", 0, false },
                    { 273, 10, "armoire", 0, 0, true, false, "head_armoire_paperBag", "Paper Bag", 0, 0, "head", 0, false },
                    { 274, 0, "armoire", 0, 10, true, false, "head_armoire_birdsNest", "Birds Nest", 0, 0, "head", 0, false },
                    { 275, 0, "armoire", 0, 0, true, false, "head_armoire_glassblowersHat", "Glassblowers Hat", 8, 0, "head", 0, false },
                    { 276, 0, "armoire", 0, 8, true, false, "head_armoire_piraticalPrincessHeaddress", "Piratical Princess Headdress", 8, 0, "head", 0, false },
                    { 277, 0, "armoire", 0, 15, true, false, "head_armoire_jeweledArcherHelm", "Jeweled Archer Helm", 0, 0, "head", 0, false },
                    { 278, 0, "armoire", 0, 0, true, false, "head_armoire_veilOfSpades", "Veil Of Spades", 13, 0, "head", 0, false },
                    { 279, 0, "armoire", 0, 0, true, false, "head_armoire_toqueBlanche", "Toque Blanche", 10, 0, "head", 0, false },
                    { 280, 0, "armoire", 0, 0, true, false, "head_armoire_vernalHennin", "Vernal Hennin", 12, 0, "head", 0, false },
                    { 281, 0, "armoire", 0, 0, true, false, "head_armoire_tricornHat", "Tricorn Hat", 10, 0, "head", 0, false },
                    { 282, 0, "armoire", 0, 6, true, false, "head_armoire_nephriteHelm", "Nephrite Helm", 7, 0, "head", 0, false },
                    { 283, 6, "armoire", 0, 0, true, false, "head_armoire_boaterHat", "Boater Hat", 6, 6, "head", 0, false },
                    { 284, 10, "armoire", 0, 0, true, false, "head_armoire_astronomersHat", "Astronomers Hat", 0, 0, "head", 0, false },
                    { 285, 0, "armoire", 0, 14, true, false, "head_armoire_deerstalkerCap", "Deerstalker Cap", 0, 0, "head", 0, false },
                    { 286, 5, "armoire", 0, 0, true, false, "head_armoire_shadowMastersHood", "Shadow Masters Hood", 5, 0, "head", 0, false },
                    { 287, 0, "armoire", 0, 0, true, false, "head_armoire_alchemistsHat", "Alchemists Hat", 7, 0, "head", 0, false },
                    { 288, 0, "armoire", 0, 7, true, false, "head_armoire_earflapHat", "Earflap Hat", 0, 7, "head", 0, false },
                    { 289, 0, "armoire", 0, 13, true, false, "head_armoire_frostedHelm", "Frosted Helm", 0, 0, "head", 0, false },
                    { 290, 15, "armoire", 0, 0, true, false, "head_armoire_matchMakersBeret", "Match Makers Beret", 0, 0, "head", 0, false },
                    { 291, 8, "armoire", 0, 0, true, false, "head_armoire_baseballCap", "Baseball Cap", 0, 8, "head", 0, false },
                    { 292, 0, "armoire", 0, 0, true, false, "head_armoire_fiddlersCap", "Fiddlers Cap", 6, 0, "head", 0, false },
                    { 293, 0, "armoire", 0, 9, true, false, "head_armoire_heroicHerbalistCrispinette", "Heroic Herbalist Crispinette", 0, 0, "head", 0, false },
                    { 294, 8, "armoire", 0, 0, true, false, "head_armoire_guardiansBonnet", "Guardians Bonnet", 0, 0, "head", 0, false },
                    { 295, 0, "armoire", 0, 0, true, false, "head_armoire_hornsOfAutumn", "Horns Of Autumn", 0, 12, "head", 0, false },
                    { 296, 0, "armoire", 0, 10, true, false, "head_armoire_capOfClubs", "Cap Of Clubs", 0, 0, "head", 0, false },
                    { 297, 0, "armoire", 0, 0, true, false, "head_armoire_nightcap", "Nightcap", 12, 0, "head", 0, false },
                    { 298, 0, "armoire", 0, 8, true, false, "head_armoire_blueMoonHelm", "Blue Moon Helm", 0, 0, "head", 0, false },
                    { 299, 0, "armoire", 0, 12, true, false, "head_armoire_pinkFloppyHat", "Pink Floppy Hat", 0, 0, "head", 0, false },
                    { 300, 8, "armoire", 0, 0, true, false, "head_armoire_jadeHelm", "Jade Helm", 0, 0, "head", 0, false },
                    { 301, 5, "armoire", 0, 0, true, false, "head_armoire_clownsWig", "Clowns Wig", 0, 0, "head", 0, false },
                    { 302, 0, "armoire", 0, 6, true, false, "head_armoire_medievalLaundryCap", "Medieval Laundry Cap", 0, 0, "head", 0, false },
                    { 303, 0, "armoire", 0, 6, true, false, "head_armoire_medievalLaundryHat", "Medieval Laundry Hat", 0, 0, "head", 0, false },
                    { 304, 0, "armoire", 0, 10, true, false, "head_armoire_rubberDucky", "Rubber Ducky", 0, 0, "head", 0, false },
                    { 305, 0, "armoire", 0, 6, true, false, "head_armoire_glengarry", "Glengarry", 0, 0, "head", 0, false },
                    { 306, 0, "armoire", 0, 6, true, false, "head_armoire_heraldsCap", "Heralds Cap", 0, 0, "head", 0, false },
                    { 307, 7, "armoire", 0, 0, true, false, "head_armoire_blackFloppyHat", "Black Floppy Hat", 7, 7, "head", 0, false },
                    { 308, 0, "armoire", 0, 7, true, false, "head_armoire_regalCrown", "Regal Crown", 0, 0, "head", 0, false },
                    { 309, 0, "armoire", 0, 0, true, false, "head_armoire_shootingStarCrown", "Shooting Star Crown", 10, 0, "head", 0, false },
                    { 310, 0, "armoire", 0, 0, true, false, "head_armoire_gardenersSunHat", "Gardeners Sun Hat", 7, 0, "head", 0, false },
                    { 311, 0, "armoire", 0, 0, true, false, "head_armoire_strawRainHat", "Straw Rain Hat", 9, 0, "head", 0, false },
                    { 312, 0, "armoire", 0, 0, true, false, "head_armoire_fancyPirateHat", "Fancy Pirate Hat", 8, 0, "head", 0, false },
                    { 313, 0, "armoire", 0, 0, true, false, "head_armoire_teaHat", "Tea Hat", 10, 0, "head", 0, false },
                    { 314, 3, "armoire", 0, 3, true, false, "head_armoire_beaniePropellerHat", "Beanie Propeller Hat", 3, 3, "head", 0, false },
                    { 315, 0, "armoire", 0, 0, true, false, "head_armoire_paintersBeret", "Painters Beret", 9, 0, "head", 0, false },
                    { 316, 0, "armoire", 0, 7, true, false, "head_armoire_admiralsBicorne", "Admirals Bicorne", 7, 0, "head", 0, false },
                    { 317, 3, "armoire", 0, 5, true, false, "head_armoire_blackSpookySorceryHat", "Black Spooky Sorcery Hat", 0, 0, "head", 0, false },
                    { 318, 3, "armoire", 0, 0, true, false, "head_armoire_purpleSpookySorceryHat", "Purple Spooky Sorcery Hat", 5, 0, "head", 0, false },
                    { 319, 6, "armoire", 0, 0, true, false, "head_armoire_greenTrapperHat", "Green Trapper Hat", 6, 0, "head", 0, false },
                    { 320, 5, "armoire", 0, 5, true, false, "head_armoire_whiteFloppyHat", "White Floppy Hat", 0, 5, "head", 0, false },
                    { 321, 0, "armoire", 0, 0, true, false, "head_armoire_hattersTopHat", "Hatters Top Hat", 10, 0, "head", 0, false },
                    { 322, 0, "armoire", 0, 8, true, false, "head_armoire_pottersBandana", "Potters Bandana", 0, 0, "head", 0, false },
                    { 323, 0, "armoire", 0, 7, true, false, "head_armoire_corsairsBandana", "Corsairs Bandana", 0, 0, "head", 0, false },
                    { 324, 0, "armoire", 0, 8, true, false, "head_armoire_dragonKnightsHelm", "Dragon Knights Helm", 0, 0, "head", 0, false },
                    { 325, 15, "armoire", 0, 0, true, false, "head_armoire_funnyFoolCap", "Funny Fool Cap", 0, 0, "head", 0, false },
                    { 326, 11, "armoire", 0, 0, true, false, "head_armoire_stormKnightHelm", "Storm Knight Helm", 0, 0, "head", 0, false },
                    { 327, 0, "armoire", 0, 12, true, false, "head_armoire_festiveHelperHat", "Festive Helper Hat", 0, 0, "head", 0, false },
                    { 328, 6, "armoire", 0, 0, true, false, "head_armoire_snowyTrapperHat", "Snowy Trapper Hat", 6, 0, "head", 0, false },
                    { 329, 0, "armoire", 0, 14, true, false, "head_armoire_fancyFloralHat", "Fancy Floral Hat", 0, 0, "head", 0, false },
                    { 330, 6, "armoire", 0, 0, true, false, "head_armoire_sillyOrangeTophat", "Silly Orange Tophat", 0, 6, "head", 0, false },
                    { 331, 6, "armoire", 0, 0, true, false, "head_armoire_sillierBlueTophat", "Sillier Blue Tophat", 0, 6, "head", 0, false },
                    { 332, 11, "armoire", 0, 0, true, false, "head_armoire_gildedKnightsHelm", "Gilded Knights Helm", 0, 0, "head", 0, false },
                    { 333, 0, "armoire", 0, 0, true, false, "head_armoire_beekeepersHat", "Beekeepers Hat", 12, 0, "head", 0, false },
                    { 334, 0, "armoire", 0, 0, true, false, "head_armoire_flyFishingHat", "Fly Fishing Hat", 7, 7, "head", 0, false },
                    { 335, 0, "armoire", 0, 8, true, false, "head_armoire_redNewsieHat", "Red Newsie Hat", 8, 0, "head", 0, false },
                    { 336, 4, "armoire", 0, 4, true, false, "head_armoire_floppyOrangeHat", "Floppy Orange Hat", 4, 4, "head", 0, false },
                    { 337, 5, "armoire", 0, 5, true, false, "head_armoire_blackHairbow", "Black Hairbow", 0, 5, "head", 0, false },
                    { 338, 0, "armoire", 0, 0, true, false, "head_armoire_blacksmithsGoggles", "Blacksmiths Goggles", 11, 0, "head", 0, false },
                    { 339, 0, "armoire", 0, 0, true, false, "head_armoire_loneCowpokeHat", "Lone Cowpoke Hat", 10, 0, "head", 0, false },
                    { 340, 3, "armoire", 0, 3, true, false, "head_armoire_floppyYellowHat", "Floppy Yellow Hat", 3, 3, "head", 0, false },
                    { 341, 5, "armoire", 0, 0, true, false, "head_armoire_verdantArmingCap", "Verdant Arming Cap", 5, 0, "head", 0, false },
                    { 342, 0, "armoire", 0, 0, true, false, "head_armoire_kendoMen", "Kendo Men", 7, 0, "head", 0, false },
                    { 343, 5, "armoire", 0, 0, true, false, "shield_armoire_gladiatorShield", "Gladiator Shield", 0, 5, "shield", 0, false },
                    { 344, 10, "armoire", 0, 0, true, false, "shield_armoire_midnightShield", "Midnight Shield", 0, 2, "shield", 0, false },
                    { 345, 5, "armoire", 0, 5, true, false, "shield_armoire_royalCane", "Royal Cane", 5, 0, "shield", 0, false },
                    { 346, 0, "armoire", 0, 0, true, false, "shield_armoire_dragonTamerShield", "Dragon Tamer Shield", 15, 0, "shield", 0, false },
                    { 347, 0, "armoire", 0, 0, true, false, "shield_armoire_mysticLamp", "Mystic Lamp", 15, 0, "shield", 0, false },
                    { 348, 3, "armoire", 0, 0, true, false, "shield_armoire_floralBouquet", "Floral Bouquet", 0, 0, "shield", 0, false },
                    { 349, 0, "armoire", 0, 0, true, false, "shield_armoire_sandyBucket", "Sandy Bucket", 10, 0, "shield", 0, false },
                    { 350, 0, "armoire", 0, 0, true, false, "shield_armoire_perchingFalcon", "Perching Falcon", 0, 16, "shield", 0, false },
                    { 351, 7, "armoire", 0, 0, true, false, "shield_armoire_ramHornShield", "Ram Horn Shield", 0, 7, "shield", 0, false },
                    { 352, 0, "armoire", 0, 0, true, false, "shield_armoire_redRose", "Red Rose", 10, 0, "shield", 0, false },
                    { 353, 9, "armoire", 0, 0, true, false, "shield_armoire_mushroomDruidShield", "Mushroom Druid Shield", 0, 8, "shield", 0, false },
                    { 354, 8, "armoire", 0, 0, true, false, "shield_armoire_festivalParasol", "Festival Parasol", 0, 0, "shield", 0, false },
                    { 355, 0, "armoire", 0, 8, true, false, "shield_armoire_vikingShield", "Viking Shield", 6, 0, "shield", 0, false },
                    { 356, 0, "armoire", 0, 0, true, false, "shield_armoire_swanFeatherFan", "Swan Feather Fan", 0, 8, "shield", 0, false },
                    { 357, 0, "armoire", 0, 4, true, false, "shield_armoire_goldenBaton", "Golden Baton", 0, 4, "shield", 0, false },
                    { 358, 15, "armoire", 0, 0, true, false, "shield_armoire_antiProcrastinationShield", "Anti Procrastination Shield", 0, 0, "shield", 0, false },
                    { 359, 6, "armoire", 0, 0, true, false, "shield_armoire_horseshoe", "Horseshoe", 6, 6, "shield", 0, false },
                    { 360, 0, "armoire", 0, 0, true, false, "shield_armoire_handmadeCandlestick", "Handmade Candlestick", 0, 12, "shield", 0, false },
                    { 361, 0, "armoire", 0, 8, true, false, "shield_armoire_weaversShuttle", "Weavers Shuttle", 9, 0, "shield", 0, false },
                    { 362, 10, "armoire", 0, 0, true, false, "shield_armoire_shieldOfDiamonds", "Shield Of Diamonds", 0, 0, "shield", 0, false },
                    { 363, 5, "armoire", 0, 5, true, false, "shield_armoire_flutteryFan", "Fluttery Fan", 5, 0, "shield", 0, false },
                    { 364, 0, "armoire", 0, 7, true, false, "shield_armoire_fancyShoe", "Fancy Shoe", 7, 0, "shield", 0, false },
                    { 365, 0, "armoire", 0, 6, true, false, "shield_armoire_fancyBlownGlassVase", "Fancy Blown Glass Vase", 0, 0, "shield", 0, false },
                    { 366, 0, "armoire", 0, 4, true, false, "shield_armoire_piraticalSkullShield", "Piratical Skull Shield", 4, 0, "shield", 0, false },
                    { 367, 0, "armoire", 0, 10, true, false, "shield_armoire_unfinishedTome", "Unfinished Tome", 0, 0, "shield", 0, false },
                    { 368, 10, "armoire", 0, 0, true, false, "shield_armoire_softBluePillow", "Soft Blue Pillow", 0, 0, "shield", 0, false },
                    { 369, 8, "armoire", 0, 6, true, false, "shield_armoire_softGreenPillow", "Soft Green Pillow", 0, 0, "shield", 0, false },
                    { 370, 5, "armoire", 0, 0, true, false, "shield_armoire_softRedPillow", "Soft Red Pillow", 0, 5, "shield", 0, false },
                    { 371, 0, "armoire", 0, 0, true, false, "shield_armoire_mightyQuill", "Mighty Quill", 9, 0, "shield", 0, false },
                    { 372, 0, "armoire", 0, 0, true, false, "shield_armoire_mightyPizza", "Mighty Pizza", 8, 0, "shield", 0, false },
                    { 373, 0, "armoire", 0, 7, true, false, "shield_armoire_trustyUmbrella", "Trusty Umbrella", 0, 0, "shield", 0, false },
                    { 374, 0, "armoire", 0, 9, true, false, "shield_armoire_polishedPocketwatch", "Polished Pocketwatch", 0, 0, "shield", 0, false },
                    { 375, 5, "armoire", 0, 0, true, false, "shield_armoire_masteredShadow", "Mastered Shadow", 5, 0, "shield", 0, false },
                    { 376, 0, "armoire", 0, 7, true, false, "shield_armoire_alchemistsScale", "Alchemists Scale", 0, 0, "shield", 0, false },
                    { 377, 0, "armoire", 0, 0, true, false, "shield_armoire_birthdayBanner", "Birthday Banner", 0, 7, "shield", 0, false },
                    { 378, 0, "armoire", 0, 0, true, false, "shield_armoire_perfectMatch", "Perfect Match", 15, 0, "shield", 0, false },
                    { 379, 0, "armoire", 0, 0, true, false, "shield_armoire_baseballGlove", "Baseball Glove", 0, 9, "shield", 0, false },
                    { 380, 4, "armoire", 0, 0, true, false, "shield_armoire_hobbyHorse", "Hobby Horse", 4, 0, "shield", 0, false },
                    { 381, 0, "armoire", 0, 6, true, false, "shield_armoire_fiddle", "Fiddle", 0, 0, "shield", 0, false },
                    { 382, 12, "armoire", 0, 0, true, false, "shield_armoire_lifeBuoy", "Life Buoy", 0, 0, "shield", 0, false },
                    { 383, 0, "armoire", 0, 0, true, false, "shield_armoire_piratesCompanion", "Pirates Companion", 8, 0, "shield", 0, false },
                    { 384, 9, "armoire", 0, 0, true, false, "shield_armoire_mortarAndPestle", "Mortar And Pestle", 0, 0, "shield", 0, false },
                    { 385, 12, "armoire", 0, 0, true, false, "shield_armoire_darkAutumnFlame", "Dark Autumn Flame", 0, 0, "shield", 0, false },
                    { 386, 0, "armoire", 0, 0, true, false, "shield_armoire_blueMoonSai", "Blue Moon Sai", 8, 0, "shield", 0, false },
                    { 387, 6, "armoire", 0, 0, true, false, "shield_armoire_softPinkPillow", "Soft Pink Pillow", 0, 6, "shield", 0, false },
                    { 388, 0, "armoire", 0, 0, true, false, "shield_armoire_clownsBalloons", "Clowns Balloons", 5, 0, "shield", 0, false },
                    { 389, 0, "armoire", 0, 0, true, false, "shield_armoire_strawberryFood", "Strawberry Food", 0, 5, "shield", 0, false },
                    { 390, 5, "armoire", 0, 0, true, false, "shield_armoire_rottenMeatFood", "Rotten Meat Food", 0, 0, "shield", 0, false },
                    { 391, 0, "armoire", 0, 0, true, false, "shield_armoire_potatoFood", "Potato Food", 5, 0, "shield", 0, false },
                    { 392, 0, "armoire", 0, 5, true, false, "shield_armoire_pinkCottonCandyFood", "Pink Cotton Candy Food", 0, 0, "shield", 0, false },
                    { 393, 0, "armoire", 0, 0, true, false, "shield_armoire_meatFood", "Meat Food", 0, 5, "shield", 0, false },
                    { 394, 0, "armoire", 0, 3, true, false, "shield_armoire_honeyFood", "Honey Food", 3, 0, "shield", 0, false },
                    { 395, 0, "armoire", 0, 0, true, false, "shield_armoire_fishFood", "Fish Food", 5, 0, "shield", 0, false },
                    { 396, 0, "armoire", 0, 5, true, false, "shield_armoire_chocolateFood", "Chocolate Food", 0, 0, "shield", 0, false },
                    { 397, 5, "armoire", 0, 0, true, false, "shield_armoire_blueCottonCandyFood", "Blue Cotton Candy Food", 0, 0, "shield", 0, false },
                    { 398, 3, "armoire", 0, 0, true, false, "shield_armoire_milkFood", "Milk Food", 0, 3, "shield", 0, false },
                    { 399, 0, "armoire", 0, 0, true, false, "shield_armoire_medievalLaundry", "Medieval Laundry", 6, 0, "shield", 0, false },
                    { 400, 0, "armoire", 0, 0, true, false, "shield_armoire_bouncyBubbles", "Bouncy Bubbles", 0, 5, "shield", 0, false },
                    { 401, 0, "armoire", 0, 0, true, false, "shield_armoire_bagpipes", "Bagpipes", 0, 6, "shield", 0, false },
                    { 402, 0, "armoire", 0, 0, true, false, "shield_armoire_heraldsMessageScroll", "Heralds Message Scroll", 6, 0, "shield", 0, false },
                    { 403, 0, "armoire", 0, 5, true, false, "shield_armoire_softBlackPillow", "Soft Black Pillow", 5, 0, "shield", 0, false },
                    { 404, 0, "armoire", 0, 10, true, false, "shield_armoire_softVioletPillow", "Soft Violet Pillow", 0, 0, "shield", 0, false },
                    { 405, 0, "armoire", 0, 0, true, false, "shield_armoire_gardenersSpade", "Gardeners Spade", 0, 8, "shield", 0, false },
                    { 406, 0, "armoire", 0, 6, true, false, "shield_armoire_spanishGuitar", "Spanish Guitar", 5, 0, "shield", 0, false },
                    { 407, 5, "armoire", 0, 6, true, false, "shield_armoire_snareDrum", "Snare Drum", 0, 0, "shield", 0, false },
                    { 408, 0, "armoire", 0, 4, true, false, "shield_armoire_treasureMap", "Treasure Map", 0, 4, "shield", 0, false },
                    { 409, 4, "armoire", 0, 4, true, false, "shield_armoire_dustpan", "Dustpan", 0, 0, "shield", 0, false },
                    { 410, 8, "armoire", 0, 0, true, false, "shield_armoire_bubblingCauldron", "Bubbling Cauldron", 0, 0, "shield", 0, false },
                    { 411, 0, "armoire", 0, 0, true, false, "shield_armoire_jewelersPliers", "Jewelers Pliers", 0, 10, "shield", 0, false },
                    { 412, 10, "armoire", 0, 0, true, false, "shield_armoire_teaKettle", "Tea Kettle", 0, 0, "shield", 0, false },
                    { 413, 5, "armoire", 0, 0, true, false, "shield_armoire_basketball", "Basketball", 0, 5, "shield", 0, false },
                    { 414, 0, "armoire", 0, 0, true, false, "shield_armoire_paintersPalette", "Painters Palette", 0, 7, "shield", 0, false },
                    { 415, 0, "armoire", 0, 4, true, false, "shield_armoire_bucket", "Bucket", 0, 4, "shield", 0, false },
                    { 416, 0, "armoire", 0, 0, true, false, "shield_armoire_saucepan", "Saucepan", 10, 0, "shield", 0, false },
                    { 417, 0, "armoire", 0, 10, true, false, "shield_armoire_trustyPencil", "Trusty Pencil", 0, 0, "shield", 0, false },
                    { 418, 0, "armoire", 0, 6, true, false, "shield_armoire_softWhitePillow", "Soft White Pillow", 6, 0, "shield", 0, false },
                    { 419, 0, "armoire", 0, 9, true, false, "shield_armoire_hattersPocketWatch", "Hatters Pocket Watch", 0, 0, "shield", 0, false },
                    { 420, 4, "armoire", 0, 4, true, false, "shield_armoire_happyThoughts", "Happy Thoughts", 4, 4, "shield", 0, false },
                    { 421, 8, "armoire", 0, 0, true, false, "shield_armoire_thrownVessel", "Thrown Vessel", 0, 0, "shield", 0, false },
                    { 422, 0, "armoire", 0, 0, true, false, "shield_armoire_buoyantBeachBall", "Buoyant Beach Ball", 0, 12, "shield", 0, false },
                    { 423, 10, "armoire", 0, 0, true, false, "shield_armoire_safetyFlashlight", "Safety Flashlight", 0, 0, "shield", 0, false },
                    { 424, 0, "armoire", 0, 0, true, false, "shield_armoire_fancyFloralFan", "Fancy Floral Fan", 14, 0, "shield", 0, false },
                    { 425, 0, "armoire", 0, 8, true, false, "shield_armoire_springPetalUchiwa", "Spring Petal Uchiwa", 8, 0, "shield", 0, false },
                    { 426, 0, "armoire", 0, 0, true, false, "shield_armoire_beekeepersHive", "Beekeepers Hive", 0, 12, "shield", 0, false },
                    { 427, 0, "armoire", 0, 7, true, false, "shield_armoire_flyFishingRod", "Fly Fishing Rod", 0, 7, "shield", 0, false },
                    { 428, 0, "armoire", 0, 8, true, false, "shield_armoire_softOrangePillow", "Soft Orange Pillow", 8, 0, "shield", 0, false },
                    { 429, 6, "armoire", 0, 0, true, false, "shield_armoire_doubleBass", "Double Bass", 0, 6, "shield", 0, false },
                    { 430, 2, "armoire", 0, 2, true, false, "shield_armoire_prettyPinkGiftBox", "Pretty Pink Gift Box", 2, 2, "shield", 0, false },
                    { 431, 0, "armoire", 0, 9, true, false, "shield_armoire_softYellowPillow", "Soft Yellow Pillow", 9, 0, "shield", 0, false },
                    { 432, 0, "armoire", 0, 10, true, false, "shield_armoire_verdantBanner", "Verdant Banner", 0, 0, "shield", 0, false },
                    { 433, 0, "armoire", 0, 0, true, false, "shield_armoire_gardenHose", "Garden Hose", 8, 0, "shield", 0, false },
                    { 434, 0, "armoire", 0, 0, true, false, "headAccessory_armoire_comicalArrow", "Comical Arrow", 0, 10, "headAccessory", 0, false },
                    { 435, 0, "armoire", 0, 0, true, false, "headAccessory_armoire_gogglesOfBookbinding", "Goggles Of Bookbinding", 8, 0, "headAccessory", 0, false },
                    { 436, 5, "armoire", 0, 0, true, false, "weapon_armoire_basicCrossbow", "Basic Crossbow", 5, 5, "weapon", 0, false },
                    { 437, 7, "armoire", 0, 7, true, false, "weapon_armoire_lunarSceptre", "Lunar Sceptre", 0, 0, "weapon", 0, false },
                    { 438, 0, "armoire", 0, 5, true, false, "weapon_armoire_rancherLasso", "Rancher Lasso", 5, 5, "weapon", 0, true },
                    { 439, 0, "armoire", 0, 0, true, false, "weapon_armoire_mythmakerSword", "Mythmaker Sword", 6, 6, "weapon", 0, false },
                    { 440, 0, "armoire", 0, 0, true, false, "weapon_armoire_ironCrook", "Iron Crook", 7, 7, "weapon", 0, false },
                    { 441, 4, "armoire", 0, 4, true, false, "weapon_armoire_goldWingStaff", "Gold Wing Staff", 4, 4, "weapon", 0, false },
                    { 442, 0, "armoire", 0, 10, true, false, "weapon_armoire_batWand", "Bat Wand", 2, 0, "weapon", 0, false },
                    { 443, 9, "armoire", 0, 0, true, false, "weapon_armoire_shepherdsCrook", "Shepherds Crook", 0, 0, "weapon", 0, false },
                    { 444, 0, "armoire", 0, 7, true, false, "weapon_armoire_crystalCrescentStaff", "Crystal Crescent Staff", 0, 7, "weapon", 0, false },
                    { 445, 8, "armoire", 0, 0, true, false, "weapon_armoire_blueLongbow", "Blue Longbow", 9, 7, "weapon", 0, true },
                    { 446, 0, "armoire", 0, 0, true, false, "weapon_armoire_glowingSpear", "Glowing Spear", 0, 15, "weapon", 0, false },
                    { 447, 5, "armoire", 0, 0, true, false, "weapon_armoire_barristerGavel", "Barrister Gavel", 0, 5, "weapon", 0, false },
                    { 448, 0, "armoire", 0, 8, true, false, "weapon_armoire_jesterBaton", "Jester Baton", 8, 0, "weapon", 0, false },
                    { 449, 0, "armoire", 0, 0, true, false, "weapon_armoire_miningPickax", "Mining Pickax", 15, 0, "weapon", 0, false },
                    { 450, 0, "armoire", 0, 0, true, false, "weapon_armoire_basicLongbow", "Basic Longbow", 0, 6, "weapon", 0, true },
                    { 451, 0, "armoire", 0, 11, true, false, "weapon_armoire_habiticanDiploma", "Habitican Diploma", 0, 0, "weapon", 0, false },
                    { 452, 0, "armoire", 0, 0, true, false, "weapon_armoire_sandySpade", "Sandy Spade", 0, 10, "weapon", 0, false },
                    { 453, 0, "armoire", 0, 0, true, false, "weapon_armoire_cannon", "Cannon", 0, 15, "weapon", 0, false },
                    { 454, 0, "armoire", 0, 0, true, false, "weapon_armoire_vermilionArcherBow", "Vermilion Archer Bow", 0, 15, "weapon", 0, true },
                    { 455, 0, "armoire", 0, 0, true, false, "weapon_armoire_ogreClub", "Ogre Club", 0, 15, "weapon", 0, false },
                    { 456, 0, "armoire", 0, 12, true, false, "weapon_armoire_woodElfStaff", "Wood Elf Staff", 0, 0, "weapon", 0, false },
                    { 457, 0, "armoire", 0, 13, true, false, "weapon_armoire_wandOfHearts", "Wand Of Hearts", 0, 0, "weapon", 0, false },
                    { 458, 0, "armoire", 0, 8, true, false, "weapon_armoire_forestFungusStaff", "Forest Fungus Staff", 9, 0, "weapon", 0, false },
                    { 459, 0, "armoire", 0, 0, true, false, "weapon_armoire_festivalFirecracker", "Festival Firecracker", 8, 0, "weapon", 0, false },
                    { 460, 0, "armoire", 0, 10, true, false, "weapon_armoire_merchantsDisplayTray", "Merchants Display Tray", 0, 0, "weapon", 0, false },
                    { 461, 8, "armoire", 0, 6, true, false, "weapon_armoire_battleAxe", "Battle Axe", 0, 0, "weapon", 0, false },
                    { 462, 6, "armoire", 0, 6, true, false, "weapon_armoire_hoofClippers", "Hoof Clippers", 0, 6, "weapon", 0, false },
                    { 463, 0, "armoire", 0, 0, true, false, "weapon_armoire_weaversComb", "Weavers Comb", 8, 9, "weapon", 0, false },
                    { 464, 8, "armoire", 0, 0, true, false, "weapon_armoire_lamplighter", "Lamplighter", 6, 0, "weapon", 0, false },
                    { 465, 0, "armoire", 0, 8, true, false, "weapon_armoire_coachDriversWhip", "Coach Drivers Whip", 0, 6, "weapon", 0, false },
                    { 466, 0, "armoire", 0, 0, true, false, "weapon_armoire_scepterOfDiamonds", "Scepter Of Diamonds", 0, 13, "weapon", 0, false },
                    { 467, 5, "armoire", 0, 5, true, false, "weapon_armoire_flutteryArmy", "Fluttery Army", 0, 5, "weapon", 0, false },
                    { 468, 7, "armoire", 0, 0, true, false, "weapon_armoire_cobblersHammer", "Cobblers Hammer", 0, 7, "weapon", 0, false },
                    { 469, 0, "armoire", 0, 0, true, false, "weapon_armoire_glassblowersBlowpipe", "Glassblowers Blowpipe", 0, 6, "weapon", 0, false },
                    { 470, 0, "armoire", 0, 7, true, false, "weapon_armoire_poisonedGoblet", "Poisoned Goblet", 0, 0, "weapon", 0, false },
                    { 471, 0, "armoire", 0, 15, true, false, "weapon_armoire_jeweledArcherBow", "Jeweled Archer Bow", 0, 0, "weapon", 0, true },
                    { 472, 0, "armoire", 0, 0, true, false, "weapon_armoire_needleOfBookbinding", "Needle Of Bookbinding", 0, 8, "weapon", 0, false },
                    { 473, 13, "armoire", 0, 0, true, false, "weapon_armoire_spearOfSpades", "Spear Of Spades", 0, 0, "weapon", 0, false },
                    { 474, 0, "armoire", 0, 9, true, false, "weapon_armoire_arcaneScroll", "Arcane Scroll", 0, 0, "weapon", 0, false },
                    { 475, 0, "armoire", 0, 8, true, false, "weapon_armoire_chefsSpoon", "Chefs Spoon", 0, 0, "weapon", 0, false },
                    { 476, 8, "armoire", 0, 0, true, false, "weapon_armoire_vernalTaper", "Vernal Taper", 0, 0, "weapon", 0, false },
                    { 477, 0, "armoire", 0, 10, true, false, "weapon_armoire_jugglingBalls", "Juggling Balls", 0, 0, "weapon", 0, false },
                    { 478, 0, "armoire", 0, 0, true, false, "weapon_armoire_slingshot", "Slingshot", 0, 10, "weapon", 0, false },
                    { 479, 0, "armoire", 0, 7, true, false, "weapon_armoire_nephriteBow", "Nephrite Bow", 0, 6, "weapon", 0, true },
                    { 480, 6, "armoire", 0, 6, true, false, "weapon_armoire_bambooCane", "Bamboo Cane", 6, 0, "weapon", 0, false },
                    { 481, 0, "armoire", 0, 0, true, false, "weapon_armoire_astronomersTelescope", "Astronomers Telescope", 10, 0, "weapon", 0, false },
                    { 482, 0, "armoire", 0, 0, true, false, "weapon_armoire_magnifyingGlass", "Magnifying Glass", 7, 0, "weapon", 0, false },
                    { 483, 9, "armoire", 0, 0, true, false, "weapon_armoire_floridFan", "Florid Fan", 0, 0, "weapon", 0, false },
                    { 484, 0, "armoire", 0, 0, true, false, "weapon_armoire_resplendentRapier", "Resplendent Rapier", 9, 0, "weapon", 0, false },
                    { 485, 0, "armoire", 0, 0, true, false, "weapon_armoire_shadowMastersMace", "Shadow Masters Mace", 12, 0, "weapon", 0, false },
                    { 486, 0, "armoire", 0, 5, true, false, "weapon_armoire_alchemistsDistiller", "Alchemists Distiller", 0, 8, "weapon", 0, false },
                    { 487, 0, "armoire", 0, 0, true, false, "weapon_armoire_happyBanner", "Happy Banner", 7, 0, "weapon", 0, false },
                    { 488, 0, "armoire", 0, 0, true, false, "weapon_armoire_livelyMatch", "Lively Match", 0, 15, "weapon", 0, false },
                    { 489, 9, "armoire", 0, 0, true, false, "weapon_armoire_baseballBat", "Baseball Bat", 0, 0, "weapon", 0, false },
                    { 490, 0, "armoire", 0, 0, true, false, "weapon_armoire_paperCutter", "Paper Cutter", 0, 9, "weapon", 0, false },
                    { 491, 0, "armoire", 0, 0, true, false, "weapon_armoire_fiddlersBow", "Fiddlers Bow", 0, 6, "weapon", 0, false },
                    { 492, 0, "armoire", 0, 0, true, false, "weapon_armoire_beachFlag", "Beach Flag", 12, 0, "weapon", 0, false },
                    { 493, 0, "armoire", 0, 0, true, false, "weapon_armoire_handyHook", "Handy Hook", 0, 8, "weapon", 0, false },
                    { 494, 0, "armoire", 0, 0, true, false, "weapon_armoire_guardiansCrook", "Guardians Crook", 0, 10, "weapon", 0, false },
                    { 495, 0, "armoire", 0, 0, true, false, "weapon_armoire_enchantersStaff", "Enchanters Staff", 12, 0, "weapon", 0, false },
                    { 496, 0, "armoire", 0, 0, true, false, "weapon_armoire_clubOfClubs", "Club Of Clubs", 0, 10, "weapon", 0, false },
                    { 497, 0, "armoire", 0, 12, true, false, "weapon_armoire_eveningTea", "Evening Tea", 0, 0, "weapon", 0, false },
                    { 498, 0, "armoire", 0, 0, true, false, "weapon_armoire_blueMoonSai", "Blue Moon Sai", 0, 8, "weapon", 0, false },
                    { 499, 0, "armoire", 0, 0, true, false, "weapon_armoire_jadeGlaive", "Jade Glaive", 0, 10, "weapon", 0, false },
                    { 500, 0, "armoire", 0, 0, true, false, "weapon_armoire_medievalWashboard", "Medieval Washboard", 0, 6, "weapon", 0, false },
                    { 501, 0, "armoire", 0, 0, true, false, "weapon_armoire_buoyantBubbles", "Buoyant Bubbles", 5, 0, "weapon", 0, false },
                    { 502, 0, "armoire", 0, 0, true, false, "weapon_armoire_heraldsBuisine", "Heralds Buisine", 0, 6, "weapon", 0, false },
                    { 503, 0, "armoire", 0, 10, true, false, "weapon_armoire_skullLantern", "Skull Lantern", 0, 0, "weapon", 0, false },
                    { 504, 3, "armoire", 0, 3, true, false, "weapon_armoire_potionBase", "Potion Base", 3, 3, "weapon", 0, false },
                    { 505, 8, "armoire", 0, 4, true, false, "weapon_armoire_potionBlue", "Potion Blue", 0, 0, "weapon", 0, false },
                    { 506, 8, "armoire", 0, 0, true, false, "weapon_armoire_potionDesert", "Potion Desert", 0, 4, "weapon", 0, false },
                    { 507, 0, "armoire", 0, 6, true, false, "weapon_armoire_potionGolden", "Potion Golden", 0, 6, "weapon", 0, false },
                    { 508, 8, "armoire", 0, 4, true, false, "weapon_armoire_potionPink", "Potion Pink", 0, 0, "weapon", 0, false },
                    { 509, 6, "armoire", 0, 0, true, false, "weapon_armoire_potionRed", "Potion Red", 0, 6, "weapon", 0, false },
                    { 510, 0, "armoire", 0, 9, true, false, "weapon_armoire_potionShade", "Potion Shade", 3, 0, "weapon", 0, false },
                    { 511, 0, "armoire", 0, 3, true, false, "weapon_armoire_potionSkeleton", "Potion Skeleton", 0, 9, "weapon", 0, false },
                    { 512, 5, "armoire", 0, 0, true, false, "weapon_armoire_potionWhite", "Potion White", 7, 0, "weapon", 0, false },
                    { 513, 4, "armoire", 0, 0, true, false, "weapon_armoire_potionZombie", "Potion Zombie", 8, 0, "weapon", 0, false },
                    { 514, 0, "armoire", 0, 0, true, false, "weapon_armoire_regalSceptre", "Regal Sceptre", 7, 0, "weapon", 0, false },
                    { 515, 0, "armoire", 0, 5, true, false, "weapon_armoire_shootingStarSpell", "Shooting Star Spell", 0, 5, "weapon", 0, true },
                    { 516, 0, "armoire", 0, 0, true, false, "weapon_armoire_pinkLongbow", "Pink Longbow", 6, 5, "weapon", 0, true },
                    { 517, 0, "armoire", 0, 8, true, false, "weapon_armoire_gardenersWateringCan", "Gardeners Watering Can", 0, 0, "weapon", 0, false },
                    { 518, 0, "armoire", 0, 6, true, false, "weapon_armoire_huntingHorn", "Hunting Horn", 0, 5, "weapon", 0, false },
                    { 519, 3, "armoire", 0, 3, true, false, "weapon_armoire_blueKite", "Blue Kite", 3, 3, "weapon", 0, false },
                    { 520, 3, "armoire", 0, 3, true, false, "weapon_armoire_greenKite", "Green Kite", 3, 3, "weapon", 0, false },
                    { 521, 3, "armoire", 0, 3, true, false, "weapon_armoire_orangeKite", "Orange Kite", 3, 3, "weapon", 0, false },
                    { 522, 3, "armoire", 0, 3, true, false, "weapon_armoire_pinkKite", "Pink Kite", 3, 3, "weapon", 0, false },
                    { 523, 3, "armoire", 0, 3, true, false, "weapon_armoire_yellowKite", "Yellow Kite", 3, 3, "weapon", 0, false },
                    { 524, 0, "armoire", 0, 4, true, false, "weapon_armoire_pushBroom", "Push Broom", 0, 4, "weapon", 0, false },
                    { 525, 4, "armoire", 0, 0, true, false, "weapon_armoire_featherDuster", "Feather Duster", 4, 0, "weapon", 0, false },
                    { 526, 0, "armoire", 0, 0, true, false, "weapon_armoire_magicSpatula", "Magic Spatula", 8, 0, "weapon", 0, false },
                    { 527, 10, "armoire", 0, 0, true, false, "weapon_armoire_finelyCutGem", "Finely Cut Gem", 0, 0, "weapon", 0, false },
                    { 528, 0, "armoire", 0, 8, true, false, "weapon_armoire_paintbrush", "Paintbrush", 0, 0, "weapon", 0, false },
                    { 529, 4, "armoire", 0, 0, true, false, "weapon_armoire_mop", "Mop", 4, 0, "weapon", 0, false },
                    { 530, 4, "armoire", 0, 0, true, false, "weapon_armoire_cleaningCloth", "Cleaning Cloth", 0, 4, "weapon", 0, false },
                    { 531, 0, "armoire", 0, 3, true, false, "weapon_armoire_ridingBroom", "Riding Broom", 0, 5, "weapon", 0, false },
                    { 532, 0, "armoire", 0, 0, true, false, "weapon_armoire_rollingPin", "Rolling Pin", 0, 10, "weapon", 0, false },
                    { 533, 0, "armoire", 0, 10, true, false, "weapon_armoire_scholarlyTextbooks", "Scholarly Textbooks", 0, 0, "weapon", 0, false },
                    { 534, 0, "armoire", 0, 0, true, false, "weapon_armoire_hattersShears", "Hatters Shears", 0, 10, "weapon", 0, false },
                    { 535, 4, "armoire", 0, 0, true, false, "weapon_armoire_optimistsClover", "Optimists Clover", 0, 4, "weapon", 0, false },
                    { 536, 0, "armoire", 0, 0, true, false, "weapon_armoire_pottersWheel", "Potters Wheel", 8, 0, "weapon", 0, false },
                    { 537, 0, "armoire", 0, 0, true, false, "weapon_armoire_shadyBeachUmbrella", "Shady Beach Umbrella", 12, 0, "weapon", 0, false },
                    { 538, 0, "armoire", 0, 0, true, false, "weapon_armoire_corsairsBlade", "Corsairs Blade", 0, 7, "weapon", 0, false },
                    { 539, 8, "armoire", 0, 0, true, false, "weapon_armoire_dragonKnightsLance", "Dragon Knights Lance", 0, 0, "weapon", 0, false },
                    { 540, 15, "armoire", 0, 0, true, false, "weapon_armoire_funnyFoolBaton", "Funny Fool Baton", 0, 15, "weapon", 0, false },
                    { 541, 0, "armoire", 0, 10, true, false, "weapon_armoire_spookyCandyBucket", "Spooky Candy Bucket", 0, 0, "weapon", 0, false },
                    { 542, 0, "armoire", 0, 0, true, false, "weapon_armoire_stormKnightAxe", "Storm Knight Axe", 0, 11, "weapon", 0, false },
                    { 543, 0, "armoire", 0, 0, true, false, "weapon_armoire_gildedKnightsSpear", "Gilded Knights Spear", 0, 11, "weapon", 0, false },
                    { 544, 0, "armoire", 0, 12, true, false, "weapon_armoire_beekeepersSmoker", "Beekeepers Smoker", 0, 0, "weapon", 0, false },
                    { 545, 0, "armoire", 0, 0, true, false, "weapon_armoire_blacksmithsHammer", "Blacksmiths Hammer", 0, 11, "weapon", 0, false },
                    { 546, 6, "armoire", 0, 6, true, false, "weapon_armoire_bambooFlute", "Bamboo Flute", 0, 0, "weapon", 0, false },
                    { 547, 2, "armoire", 0, 2, true, false, "weapon_armoire_prettyPinkParasol", "Pretty Pink Parasol", 2, 2, "weapon", 0, false },
                    { 548, 3, "armoire", 0, 3, true, false, "weapon_armoire_brightRainbowKite", "Bright Rainbow Kite", 3, 3, "weapon", 0, false },
                    { 549, 3, "armoire", 0, 3, true, false, "weapon_armoire_pastelRainbowKite", "Pastel Rainbow Kite", 3, 3, "weapon", 0, false },
                    { 550, 0, "armoire", 0, 0, true, false, "weapon_armoire_kendoShinai", "Kendo Shinai", 0, 7, "weapon", 0, false },
                    { 551, 8, "armoire", 0, 0, true, false, "weapon_armoire_gardenRake", "Garden Rake", 0, 0, "weapon", 0, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 266);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 267);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 268);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 271);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 272);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 273);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 274);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 275);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 276);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 277);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 278);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 279);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 280);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 281);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 282);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 283);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 284);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 285);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 286);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 287);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 288);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 289);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 290);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 291);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 292);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 293);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 294);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 295);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 296);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 297);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 298);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 299);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 311);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 312);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 313);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 314);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 315);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 316);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 317);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 318);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 319);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 320);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 321);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 322);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 323);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 324);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 325);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 326);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 327);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 328);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 329);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 330);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 331);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 332);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 333);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 334);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 335);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 336);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 337);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 338);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 339);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 340);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 341);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 342);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 343);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 344);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 345);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 346);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 347);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 348);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 349);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 350);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 351);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 352);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 353);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 354);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 355);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 356);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 357);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 358);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 359);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 360);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 361);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 362);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 363);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 364);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 365);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 366);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 367);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 368);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 369);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 370);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 371);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 372);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 373);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 374);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 375);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 376);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 377);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 378);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 379);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 380);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 381);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 382);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 383);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 384);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 385);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 386);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 387);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 388);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 389);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 390);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 391);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 392);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 393);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 394);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 395);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 396);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 397);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 398);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 399);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 407);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 408);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 409);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 410);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 411);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 412);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 413);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 414);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 415);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 416);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 417);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 418);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 419);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 420);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 421);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 422);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 423);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 424);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 425);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 426);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 427);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 428);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 429);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 430);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 431);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 432);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 433);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 434);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 435);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 436);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 437);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 438);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 439);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 440);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 441);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 442);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 443);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 444);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 445);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 446);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 447);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 448);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 449);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 450);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 451);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 452);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 453);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 454);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 455);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 456);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 457);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 458);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 459);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 460);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 461);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 462);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 463);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 464);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 465);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 466);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 467);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 468);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 469);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 470);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 471);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 472);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 473);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 474);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 475);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 476);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 477);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 478);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 479);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 480);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 481);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 482);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 483);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 484);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 485);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 486);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 487);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 488);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 489);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 490);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 491);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 492);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 493);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 494);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 495);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 496);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 497);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 498);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 499);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 516);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 517);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 518);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 519);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 520);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 522);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 523);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 524);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 525);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 526);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 527);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 528);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 529);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 530);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 531);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 532);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 533);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 534);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 535);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 536);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 537);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 538);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 539);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 540);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 541);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 542);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 543);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 544);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 545);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 546);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 547);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 548);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 549);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 550);

            migrationBuilder.DeleteData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 551);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "IsArmoire",
                value: true);

            migrationBuilder.UpdateData(
                table: "GearItems",
                keyColumn: "Id",
                keyValue: 64,
                column: "IsArmoire",
                value: true);
        }
    }
}
