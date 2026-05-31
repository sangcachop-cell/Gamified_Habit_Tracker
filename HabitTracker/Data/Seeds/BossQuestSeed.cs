using HabitTracker.Models;

namespace HabitTracker.Data.Seeds;

public static class BossQuestSeed
{
    public static BossQuest[] GetAll() =>
    [
        // ── PET BOSS QUESTS (60) ────────────────────────────────────────────────
        Q(1,  "alligator",     "The Insta-Gator",                         "pet",  4,    1100, 2.5,  null, null,  null, null, null, null,  null,                          73,  725, """[{"type":"eggs","key":"Alligator","count":3}]"""),
        Q(2,  "alpaca",        "The Overpacked Alpaca",                   "pet",  4,    800,  2.0,  50,   0.3,   null, null, null, "Alpaca Rage",   null,              90,  900, """[{"type":"eggs","key":"Alpaca","count":3}]"""),
        Q(3,  "armadillo",     "The Indulgent Armadillo",                 "pet",  4,    600,  1.5,  null, null,  null, null, null, null,                          null,  43,  350, """[{"type":"eggs","key":"Armadillo","count":3}]"""),
        Q(4,  "axolotl",       "The Magical Axolotl",                     "pet",  4,    500,  1.5,  50,   0.3,   null, null, null, "Axolotl Rage",  null,              37,  275, """[{"type":"eggs","key":"Axolotl","count":3}]"""),
        Q(5,  "badger",        "Stop Badgering Me!",                      "pet",  4,    600,  1.5,  null, null,  null, null, null, null,                          null,  43,  350, """[{"type":"eggs","key":"Badger","count":3}]"""),
        Q(6,  "beetle",        "The CRITICAL BUG",                        "pet",  4,    500,  1.5,  null, null,  null, null, null, null,                          null,  37,  275, """[{"type":"eggs","key":"Beetle","count":3}]"""),
        Q(7,  "bunny",         "The Killer Bunny",                        "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Bunny","count":3}]"""),
        Q(8,  "butterfly",     "Bye, Bye, Butterfry",                     "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Butterfly","count":3}]"""),
        Q(9,  "cat",           "A Purrplexing Predicament",               "pet",  4,    600,  1.5,  50,   null,  0.33, null, null, "Cat Rage",      null,              55,  500, """[{"type":"eggs","key":"Cat","count":3}]"""),
        Q(10, "chameleon",     "The Chaotic Chameleon",                   "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  35,  250, """[{"type":"eggs","key":"Chameleon","count":3}]"""),
        Q(11, "cheetah",       "Such a Cheetah",                          "pet",  4,    600,  1.5,  null, null,  null, null, null, null,                          null,  43,  350, """[{"type":"eggs","key":"Cheetah","count":3}]"""),
        Q(12, "cow",           "The Mootant Cow",                         "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Cow","count":3}]"""),
        Q(13, "crab",          "The Fiddling Crab",                       "pet",  4,    1200, 2.5,  50,   null,  0.33, null, null, "Crab Rage",     null,              90,  900, """[{"type":"eggs","key":"Crab","count":3}]"""),
        Q(14, "dilatory_derby","The Dilatory Derby",                      "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Seahorse","count":3}]"""),
        Q(15, "dog",           "Triple Dog Dare!",                        "pet",  4,    600,  1.5,  50,   null,  0.3,  null, null, "Dog Rage",      null,              55,  500, """[{"type":"eggs","key":"Dog","count":3}]"""),
        Q(16, "dolphin",       "The Dolphin of Doubt",                    "pet",  4,    300,  1.25, null, null,  null, null, null, null,                          null,  22,  110, """[{"type":"eggs","key":"Dolphin","count":3}]"""),
        Q(17, "falcon",        "The Birds of Preycrastination",           "pet",  4,    700,  2.0,  null, null,  null, null, null, null,                          null,  49,  425, """[{"type":"eggs","key":"Falcon","count":3}]"""),
        Q(18, "ferret",        "The Nefarious Ferret",                    "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Ferret","count":3}]"""),
        Q(19, "frog",          "Swamp of the Clutter Frog",               "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Frog","count":3}]"""),
        Q(20, "ghost_stag",    "The Spirit of Spring",                    "pet",  4,    1200, 2.5,  null, null,  null, null, null, null,                          null,  80,  800, """[{"type":"eggs","key":"Deer","count":3}]"""),
        Q(21, "giraffe",       "The Gear-affe",                           "pet",  4,    700,  2.0,  null, null,  null, null, null, null,                          null,  50,  450, """[{"type":"eggs","key":"Giraffe","count":3}]"""),
        Q(22, "gryphon",       "The Fiery Gryphon",                       "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Gryphon","count":3}]"""),
        Q(23, "guineapig",     "The Guinea Pig Gang",                     "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"GuineaPig","count":3}]"""),
        Q(24, "harpy",         "Help! Harpy!",                            "pet",  4,    600,  1.5,  null, null,  null, null, null, null,                          null,  43,  350, """[{"type":"eggs","key":"Parrot","count":3}]"""),
        Q(25, "hedgehog",      "The Hedgebeast",                          "pet",  4,    400,  1.25, null, null,  null, null, null, null,                          null,  30,  125, """[{"type":"eggs","key":"Hedgehog","count":3}]"""),
        Q(26, "hippo",         "What a Hippo-Crite",                      "pet",  4,    800,  2.0,  null, null,  null, null, null, null,                          null,  55,  500, """[{"type":"eggs","key":"Hippo","count":3}]"""),
        Q(27, "horse",         "Ride the Night-Mare",                     "pet",  4,    500,  1.5,  null, null,  null, null, null, null,                          null,  37,  275, """[{"type":"eggs","key":"Horse","count":3}]"""),
        Q(28, "kangaroo",      "Kangaroo Catastrophe",                    "pet",  4,    700,  2.0,  null, null,  null, null, null, null,                          null,  49,  425, """[{"type":"eggs","key":"Kangaroo","count":3}]"""),
        Q(29, "kraken",        "The Kraken of Inkomplete",                "pet",  4,    800,  2.0,  null, null,  null, null, null, null,                          null,  55,  500, """[{"type":"eggs","key":"Cuttlefish","count":3}]"""),
        Q(30, "monkey",        "Monstrous Mandrill and the Mischief Monkeys","pet",4,   400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Monkey","count":3}]"""),
        Q(31, "nudibranch",    "Infestation of the NowDo Nudibranchs",   "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Nudibranch","count":3}]"""),
        Q(32, "octopus",       "The Call of Octothulu",                   "pet",  4,    1200, 2.5,  null, null,  null, null, null, null,                          null,  80,  800, """[{"type":"eggs","key":"Octopus","count":3}]"""),
        Q(33, "otter",         "The Perfidious Plotter!",                 "pet",  4,    1200, 2.5,  50,   0.3,   null, null, null, "Otter Rage",    null,              90,  900, """[{"type":"eggs","key":"Otter","count":3}]"""),
        Q(34, "owl",           "The Night-Owl",                           "pet",  4,    500,  1.5,  null, null,  null, null, null, null,                          null,  37,  275, """[{"type":"eggs","key":"Owl","count":3}]"""),
        Q(35, "peacock",       "The Push-and-Pull Peacock",               "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Peacock","count":3}]"""),
        Q(36, "penguin",       "The Fowl Frost",                          "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Penguin","count":3}]"""),
        Q(37, "platypus",      "The Perfectionist Platypus",              "pet",  4,    1000, 2.0,  50,   null,  0.33, null, null, "Platypus Rage", null,              100, 1000,"""[{"type":"eggs","key":"Platypus","count":3}]"""),
        Q(38, "pterodactyl",   "The Pterror-dactyl",                      "pet",  4,    1000, 2.0,  null, null,  null, null, null, null,                          null,  67,  650, """[{"type":"eggs","key":"Pterodactyl","count":3}]"""),
        Q(39, "raccoon",       "Raccoon Tycoon",                          "pet",  4,    800,  2.0,  50,   0.3,   null, null, null, "Raccoon Rage",  null,              70,  600, """[{"type":"eggs","key":"Raccoon","count":3}]"""),
        Q(40, "rat",           "The Rat King",                            "pet",  4,    1200, 2.5,  null, null,  null, null, null, null,                          null,  80,  800, """[{"type":"eggs","key":"Rat","count":3}]"""),
        Q(41, "rock",          "Escape the Cave Creature",                "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Rock","count":3}]"""),
        Q(42, "rooster",       "Rooster Rampage",                         "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Rooster","count":3}]"""),
        Q(43, "sabretooth",    "The Sabre Cat",                           "pet",  4,    1000, 2.0,  null, null,  null, null, null, null,                          null,  67,  650, """[{"type":"eggs","key":"Sabretooth","count":3}]"""),
        Q(44, "seaserpent",    "Danger in the Depths: Sea Serpent Strike!","pet", 4,    1200, 2.5,  null, null,  null, null, null, null,                          null,  80,  800, """[{"type":"eggs","key":"SeaSerpent","count":3}]"""),
        Q(45, "sheep",         "The Thunder Ram",                         "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Sheep","count":3}]"""),
        Q(46, "slime",         "The Jelly Regent",                        "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Slime","count":3}]"""),
        Q(47, "sloth",         "The Somnolent Sloth",                     "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Sloth","count":3}]"""),
        Q(48, "snail",         "The Snail of Drudgery Sludge",            "pet",  4,    500,  1.5,  null, null,  null, null, null, null,                          null,  37,  275, """[{"type":"eggs","key":"Snail","count":3}]"""),
        Q(49, "snake",         "The Serpent of Distraction",              "pet",  4,    1100, 2.5,  null, null,  null, null, null, null,                          null,  73,  725, """[{"type":"eggs","key":"Snake","count":3}]"""),
        Q(50, "spider",        "The Icy Arachnid",                        "pet",  4,    400,  1.5,  null, null,  null, null, null, null,                          null,  31,  200, """[{"type":"eggs","key":"Spider","count":3}]"""),
        Q(51, "squirrel",      "The Sneaky Squirrel",                     "pet",  4,    700,  2.0,  null, null,  null, null, null, null,                          null,  49,  425, """[{"type":"eggs","key":"Squirrel","count":3}]"""),
        Q(52, "treeling",      "The Tangle Tree",                         "pet",  4,    600,  1.5,  null, null,  null, null, null, null,                          null,  43,  350, """[{"type":"eggs","key":"Treeling","count":3}]"""),
        Q(53, "trex",          "King of the Dinosaurs",                   "pet",  4,    800,  2.0,  null, null,  null, null, null, null,                          null,  55,  500, """[{"type":"eggs","key":"TRex","count":3}]"""),
        Q(54, "trex_undead",   "The Dinosaur Unearthed",                  "pet",  4,    500,  2.0,  50,   0.3,   null, null, null, "TRex Undead Rage",null,           55,  500, """[{"type":"eggs","key":"TRex","count":3}]"""),
        Q(55, "triceratops",   "The Trampling Triceratops",               "pet",  4,    1200, 2.5,  null, null,  null, null, null, null,                          null,  80,  800, """[{"type":"eggs","key":"Triceratops","count":3}]"""),
        Q(56, "turtle",        "Guide the Turtle",                        "pet",  4,    300,  1.5,  null, null,  null, null, null, null,                          null,  25,  125, """[{"type":"eggs","key":"Turtle","count":3}]"""),
        Q(57, "unicorn",       "Convincing the Unicorn Queen",            "pet",  4,    600,  1.5,  null, null,  null, null, null, null,                          null,  43,  350, """[{"type":"eggs","key":"Unicorn","count":3}]"""),
        Q(58, "velociraptor",  "The Veloci-Rapper",                       "pet",  4,    900,  2.0,  null, null,  null, null, null, null,                          null,  63,  575, """[{"type":"eggs","key":"Velociraptor","count":3}]"""),
        Q(59, "whale",         "Wail of the Whale",                       "pet",  4,    500,  1.5,  null, null,  null, null, null, null,                          null,  37,  275, """[{"type":"eggs","key":"Whale","count":3}]"""),
        Q(60, "yarn",          "A Tangled Yarn",                          "pet",  4,    500,  1.5,  null, null,  null, null, null, null,                          null,  37,  275, """[{"type":"eggs","key":"Yarn","count":3}]"""),

        // ── POTION BOSS QUESTS (6) ──────────────────────────────────────────────
        Q(61, "amber",      "The Amber Alliance",                         "hatchingPotion", 4, 300,  1.25, null, null, null, null, null, null, null,              50,  100, """[{"type":"hatchingPotions","key":"Amber","count":3}]"""),
        Q(62, "blackPearl", "A Startling Starry Idea",                    "hatchingPotion", 4, 725,  1.75, null, null, null, null, null, null, null,              50,  450, """[{"type":"hatchingPotions","key":"BlackPearl","count":3}]"""),
        Q(63, "bronze",     "Brazen Beetle Battle",                       "hatchingPotion", 4, 800,  2.0,  null, null, null, null, null, null, null,              63,  575, """[{"type":"hatchingPotions","key":"Bronze","count":3}]"""),
        Q(64, "fluorite",   "A Bright Fluorite Fright",                   "hatchingPotion", 4, 1200, 2.0,  null, null, null, null, null, null, null,              70,  750, """[{"type":"hatchingPotions","key":"Fluorite","count":3}]"""),
        Q(65, "jade",       "A Jaded Jinx",                               "hatchingPotion", 4, 400,  1.25, null, null, null, null, null, null, null,              40,  400, """[{"type":"hatchingPotions","key":"Jade","count":3}]"""),
        Q(66, "pinkMarble", "Calm the Corrupted Cupid",                   "hatchingPotion", 4, 1200, 2.0,  50,   null, null, 0.5,  null, "Marble Rage", null,    75,  800, """[{"type":"hatchingPotions","key":"PinkMarble","count":3}]"""),

        // ── POTION COLLECTION QUESTS (6) ───────────────────────────────────────
        QC(67, "onyx",      "The Onyx Odyssey",      "hatchingPotion", 4, """{"onyxStone":25,"plutoRune":10,"leoRune":10}""",     50,  100, """[{"type":"hatchingPotions","key":"Onyx","count":3}]"""),
        QC(68, "ruby",      "Ruby Rapport",           "hatchingPotion", 4, """{"rubyGem":25,"venusRune":10,"aquariusRune":10}""", 50,  100, """[{"type":"hatchingPotions","key":"Ruby","count":3}]"""),
        QC(69, "silver",    "The Silver Solution",    "hatchingPotion", 4, """{"silverIngot":20,"moonRune":15,"cancerRune":15}""", 50, 100, """[{"type":"hatchingPotions","key":"Silver","count":3}]"""),
        QC(70, "stone",     "A Maze of Moss",         "hatchingPotion", 4, """{"mossyStone":25,"marsRune":10,"capricornRune":10}""",50,100,"""[{"type":"hatchingPotions","key":"MossyStone","count":3}]"""),
        QC(71, "turquoise", "Turquoise Treasure Toil","hatchingPotion", 4, """{"turquoiseGem":25,"sagittariusRune":10,"neptuneRune":10}""",50,100,"""[{"type":"hatchingPotions","key":"Turquoise","count":3}]"""),
        QC(72, "opal",      "The Legend of the Obscure Opals","hatchingPotion",4,"""{"opalGem":25,"libraRune":10,"mercuryRune":10}""",50,350,"""[{"type":"hatchingPotions","key":"Opal","count":3}]"""),

        // ── SEASONAL QUESTS (7) ─────────────────────────────────────────────────
        Q(73,  "evilsanta",  "Trapper Santa",                             "pet",          4,    300, 1.0,  null, null, null, null, null, null, null,              20,  100, """[{"type":"mounts","key":"BearCub-Polar","count":1}]"""),
        QC(74, "evilsanta2", "Find the Cub",                              "pet",          4,    """{"tracks":20,"branches":10}""",                               20,  100, """[{"type":"pets","key":"BearCub-Polar","count":1}]"""),
        QC(75, "egg",        "Egg Hunt",                                  "pet",          1,    """{"plainEgg":40}""",                                           0,   0,   """[{"type":"eggs","key":"Egg","count":10}]"""),
        Q(76,  "waffle",     "Waffling with the Fool: Disaster Breakfast!","hatchingPotion",4,  500, 2.0,  50,   null, null, 0.5,  null, "Waffle Rage", null,    40,  500, """[{"type":"hatchingPotions","key":"Dessert","count":3}]"""),
        Q(77,  "virtualpet", "Virtual Mayhem with the April Fool: The Beepening","hatchingPotion",4,500,2.0,50,null,null,0.5,null,"VirtualPet Rage",null,       40,  500, """[{"type":"hatchingPotions","key":"VirtualPet","count":3}]"""),
        Q(78,  "fungi",      "The Moody Mushroom",                        "hatchingPotion",4,   500, 2.0,  50,   null, 0.33, null, null, "Fungi Rage", null,     40,  500, """[{"type":"hatchingPotions","key":"Fungi","count":3}]"""),
        Q(79,  "alien",      "Invasion of the Motivation Snatchers",      "hatchingPotion",4,   500, 2.0,  50,   0.3,  null, null, null, "Alien Rage", null,     40,  500, """[{"type":"hatchingPotions","key":"Alien","count":3}]"""),

        // ── SERIES QUESTS (15) ──────────────────────────────────────────────────
        QC(80, "atom1",         "Attack of the Mundane, Part 1: Dish Disaster!", "unlockable",4,"""{"soapBars":20}""",                7,   50,  """[{"type":"quests","key":"atom2","count":1,"onlyOwner":true}]""",             "questGroupAtom",  15,  null),
        Q(81,  "atom2",         "Attack of the Mundane, Part 2: The SnackLess Monster","unlockable",4,300,1.0,null,null,null,null,null,null,null,             20,  100, """[{"type":"quests","key":"atom3","count":1,"onlyOwner":true}]""",         "questGroupAtom",  15,  "atom1"),
        Q(82,  "atom3",         "Attack of the Mundane, Part 3: The Laundromancer","unlockable",4,800,1.5,null,null,null,null,null,null,null,                 25,  125, """[{"type":"hatchingPotions","key":"Base","count":2}]""",                  "questGroupAtom",  15,  "atom2"),
        QC(83, "goldenknight1", "The Golden Knight, Part 1: A Stern Talking-To","unlockable",4,"""{"testimony":60}""",               15,  120, """[{"type":"quests","key":"goldenknight2","count":1,"onlyOwner":true}]""",    "questGroupGoldenknight",40,null),
        Q(84,  "goldenknight2", "The Golden Knight, Part 2: Gold Knight",  "unlockable", 4, 1000,3.0,  null, null, null, null, null, null, null,             75,  750, """[{"type":"quests","key":"goldenknight3","count":1,"onlyOwner":true}]""","questGroupGoldenknight",40,"goldenknight1"),
        Q(85,  "goldenknight3", "The Golden Knight, Part 3: The Iron Knight","unlockable",4, 1700,3.5, null, null, null, null, null, null, null,             900, 1500,"""[{"type":"food","key":"Honey","count":3},{"type":"hatchingPotions","key":"Golden","count":2}]""","questGroupGoldenknight",40,"goldenknight2"),
        QC(86, "moon1",         "Lunar Battle, Part 1: Find the Mysterious Shards","unlockable",4,"""{"shard":20}""",                7,   50,  """[{"type":"gear","key":"head_special_lunarWarriorHelm","count":1}]""",      "questGroupMoon",  0,   null),
        Q(87,  "moon2",         "Lunar Battle, Part 2: Stop the Overshadowing Stress","unlockable",4,100,1.5,null,null,null,null,null,null,null,             37,  275, """[{"type":"gear","key":"armor_special_lunarWarriorArmor","count":1}]""",  "questGroupMoon",  0,   "moon1"),
        Q(88,  "moon3",         "Lunar Battle, Part 3: The Monstrous Moon","unlockable",4,   1000,2.0, null, null, null, null, null, null, null,             67,  650, """[{"type":"gear","key":"weapon_special_lunarScythe","count":1}]""",       "questGroupMoon",  0,   "moon2"),
        QC(89, "moonstone1",    "Recidivate, Part 1: The Moonstone Chain", "unlockable",4,  """{"moonstone":100}""",                 50,  100, """[{"type":"quests","key":"moonstone2","count":1,"onlyOwner":true}]""",     "questGroupMoonstone",60,null),
        Q(90,  "moonstone2",    "Recidivate, Part 2: Recidivate the Necromancer","unlockable",4,1500,3.0,null,null,null,null,null,null,null,                 500, 1000,"""[{"type":"quests","key":"moonstone3","count":1,"onlyOwner":true}]""",    "questGroupMoonstone",60,"moonstone1"),
        Q(91,  "moonstone3",    "Recidivate, Part 3: Recidivate Transformed","unlockable",4,2000,3.5, null, null, null, null, null, null, null,             900, 1500,"""[{"type":"hatchingPotions","key":"Zombie","count":3},{"type":"food","key":"RottenMeat","count":5}]""","questGroupMoonstone",60,"moonstone2"),
        Q(92,  "vice1",         "Vice, Part 1: Free Yourself of the Dragon's Influence","unlockable",4,750,1.5,null,null,null,null,null,null,null,           20,  100, """[{"type":"quests","key":"vice2","count":1,"onlyOwner":true}]""",         "questGroupVice",  30,  null),
        QC(93, "vice2",         "Vice, Part 2: Find the Lair of the Wyrm","unlockable",4,  """{"lightCrystal":30}""",               20,  75,  """[{"type":"quests","key":"vice3","count":1,"onlyOwner":true}]""",           "questGroupVice",  30,  "vice1"),
        Q(94,  "vice3",         "Vice, Part 3: Vice Awakens",              "unlockable", 4, 1500,3.0, null, null, null, null, null, null, null,             100, 1000,"""[{"type":"eggs","key":"Dragon","count":2},{"type":"hatchingPotions","key":"Shade","count":2}]""","questGroupVice",30,"vice2"),

        // ── MASTERCLASSER QUESTS (16) ───────────────────────────────────────────
        QC(95,  "dilatoryDistress1", "Dilatory Distress, Part 1: Message in a Bottle",       "gold",200,"""{"fireCoral":20,"blueFins":20}""",                       0,  75,  """[{"type":"gear","key":"armor_special_finnedOceanicArmor","count":1}]""",  "questGroupDilatoryDistress",0,null),
        QB(96,  "dilatoryDistress2", "Dilatory Distress, Part 2: Creatures of the Crevasse", "gold",300,500,1.0,50,0.3,null,null,null,"Distress Rage",null,         0,  500, """[{"type":"hatchingPotions","key":"Skeleton","count":1},{"type":"gear","key":"head_special_fireCoralCirclet","count":1}]""","questGroupDilatoryDistress",0,"dilatoryDistress1"),
        QB(97,  "dilatoryDistress3", "Dilatory Distress, Part 3: Not a Mere Maid",           "gold",400,1000,2.0,null,null,null,null,null,null,null,                 0,  650, """[{"type":"food","key":"Fish","count":3},{"type":"gear","key":"weapon_special_tridentOfCrashingTides","count":1}]""","questGroupDilatoryDistress",0,"dilatoryDistress2"),
        QB(98,  "mayhemMistiflying1","Mayhem in Mistiflying, Part 1: In Which Mistiflying Experiences a Dreadful Bother","gold",200,500,1.0,50,0.3,null,null,null,"Mistiflying Rage",null,0,500,"""[{"type":"hatchingPotions","key":"Skeleton","count":1},{"type":"gear","key":"armor_special_roguishRainbowMessengerRobes","count":1}]""","questGroupMayhemMistiflying",0,null),
        QC(99,  "mayhemMistiflying2","Mayhem in Mistiflying, Part 2: In Which the Wind Worsens","gold",300,"""{"mistifly1":25,"mistifly2":15,"mistifly3":10}""",      0,  75,  """[{"type":"gear","key":"head_special_roguishRainbowMessengerHood","count":1}]""","questGroupMayhemMistiflying",0,"mayhemMistiflying1"),
        QB(100, "mayhemMistiflying3","Mayhem in Mistiflying, Part 3: In Which a Mailman is Extremely Rude","gold",400,1000,2.0,null,null,null,null,null,null,null,   0,  650, """[{"type":"food","key":"CottonCandyPink","count":3},{"type":"gear","key":"weapon_special_roguishRainbowMessage","count":1}]""","questGroupMayhemMistiflying",0,"mayhemMistiflying2"),
        QB(101, "stoikalmCalamity1", "Stoïkalm Calamity, Part 1: Earthen Enemies",           "gold",200,500,1.0,50,0.3,null,null,null,"Stoikalm Rage",null,          0,  500, """[{"type":"hatchingPotions","key":"Skeleton","count":1},{"type":"gear","key":"armor_special_mammothRiderArmor","count":1}]""","questGroupStoikalmCalamity",0,null),
        QC(102, "stoikalmCalamity2", "Stoïkalm Calamity, Part 2: Seek the Icicle Caverns",  "gold",300,"""{"icicleCoin":40}""",                                      0,  75,  """[{"type":"gear","key":"head_special_mammothRiderHelm","count":1}]""","questGroupStoikalmCalamity",0,"stoikalmCalamity1"),
        QB(103, "stoikalmCalamity3", "Stoïkalm Calamity, Part 3: Icicle Drake Quake",        "gold",400,1000,2.0,null,null,null,null,null,null,null,                  0,  650, """[{"type":"food","key":"CottonCandyBlue","count":3},{"type":"gear","key":"weapon_special_mammothRiderSpear","count":1}]""","questGroupStoikalmCalamity",0,"stoikalmCalamity2"),
        QB(104, "taskwoodsTerror1",  "Terror in the Taskwoods, Part 1: The Blaze in the Taskwoods","gold",200,500,1.0,50,0.3,null,null,null,"Taskwoods Rage",null,   0,  500, """[{"type":"hatchingPotions","key":"Skeleton","count":1},{"type":"gear","key":"head_special_pyromancersTurban","count":1}]""","questGroupTaskwoodsTerror",0,null),
        QC(105, "taskwoodsTerror2",  "Terror in the Taskwoods, Part 2: Finding the Flourishing Fairies","gold",300,"""{"pixie":25,"brownie":15,"dryad":10}""",       0,  75,  """[{"type":"gear","key":"armor_special_pyromancersRobes","count":1}]""","questGroupTaskwoodsTerror",0,"taskwoodsTerror1"),
        QB(106, "taskwoodsTerror3",  "Terror in the Taskwoods, Part 3: Jacko of the Lantern","gold",400,1000,2.0,null,null,null,null,null,null,null,                  0,  650, """[{"type":"food","key":"Strawberry","count":3},{"type":"gear","key":"weapon_special_taskwoodsLantern","count":1}]""","questGroupTaskwoodsTerror",0,"taskwoodsTerror2"),
        QC(107, "lostMasterclasser1","The Mystery of the Masterclassers, Part 1: Read Between the Lines","gold",400,"""{"ancientTome":40,"forbiddenTome":40,"hiddenTome":40}""",0,200,"""[{"type":"food","key":"Meat","count":3},{"type":"food","key":"Milk","count":3}]""","questGroupLostMasterclasser",0,null),
        QB(108, "lostMasterclasser2","The Mystery of the Masterclassers, Part 2: Assembling the a'Voidant","gold",500,1500,2.5,null,null,null,null,null,null,null,  0, 1500,"""[{"type":"food","key":"Chocolate","count":3},{"type":"gear","key":"eyewear_special_aetherMask","count":1}]""","questGroupLostMasterclasser",0,"lostMasterclasser1"),
        QB(109, "lostMasterclasser3","The Mystery of the Masterclassers, Part 3: City in the Sands","gold",600,2000,3.0,25,0.3,null,null,null,"Masterclasser Rage",null,0,2000,"""[{"type":"hatchingPotions","key":"Golden","count":1},{"type":"hatchingPotions","key":"Shade","count":1}]""","questGroupLostMasterclasser",0,"lostMasterclasser2"),
        QB(110, "lostMasterclasser4","The Mystery of the Masterclassers, Part 4: The Lost Masterclasser","gold",700,3000,4.0,15,null,1.0,null,null,"Masterclasser Rage","Full Mana Drain",0,3500,"""[{"type":"gear","key":"back_special_aetherCloak","count":1},{"type":"gear","key":"weapon_special_aetherCrystals","count":1}]""","questGroupLostMasterclasser",0,"lostMasterclasser3"),

        // ── TIME TRAVEL QUESTS (3) — canBuy=false ──────────────────────────────
        QC(111, "robot",       "Mysterious Mechanical Marvels!", "timeTravelers", 0, """{"bolt":15,"gear":10,"spring":10}""",                                      40,  75,  """[{"type":"eggs","key":"Robot","count":3}]"""),
        Q(112,  "solarSystem", "A Voyage of Cosmic Concentration","timeTravelers",0, 1500, 2.5, null, null, null, null, null, null, null,                           90,  900, """[{"type":"hatchingPotions","key":"SolarSystem","count":3}]"""),
        Q(113,  "windup",      "A Whirl with a Wind-Up Warrior", "timeTravelers", 0, 1000, 1.0, null, null, null, null, null, null, null,                           50,  425, """[{"type":"hatchingPotions","key":"Windup","count":3}]"""),

        // ── GENERIC QUESTS (2) ──────────────────────────────────────────────────
        Q(114, "basilist",    "The Basi-List",       "unlockable", 100, 100,  0.5, null, null, null, null, null, null, null,                                        8,   42,  null),
        Q(115, "dustbunnies", "The Feral Dust Bunnies","unlockable",1,  100,  0.5, null, null, null, null, null, null, null,                                        8,   42,  null),
    ];

    // ── Gem cost tiers: HP ≤500→4, ≤1000→6, ≤2000→8, >2000→10; rage adds +1 ──
    private static int G(double hp, double? rage) => hp switch
    {
        <= 500  => rage.HasValue ? 5 : 4,
        <= 1000 => rage.HasValue ? 7 : 6,
        <= 2000 => rage.HasValue ? 9 : 8,
        _       => rage.HasValue ? 11 : 10,
    };

    // ── Helper: Boss quest (GoldCost always 0; timeTravelers = 0 gems) ─────────
    private static BossQuest Q(
        int id, string key, string text, string category, int _unused,
        double bossHp, double bossStr,
        double? rageValue, double? rageHealing, double? rageMpDrain, double? rageProgressDrain, double? rageBossDef,
        string? rageName, string? rageEffect,
        int dropGold, int dropExp, string? dropItemsJson,
        string? group = null, int lvl = 0, string? prereq = null)
    => new()
    {
        Id                   = id,
        Key                  = key,
        Text                 = text,
        Category             = category,
        GemCost              = category == "timeTravelers" ? 0 : G(bossHp, rageValue),
        GoldCost             = 0,
        IsBossQuest          = true,
        BossHp               = bossHp,
        BossStr              = bossStr,
        BossDef              = 1.0,
        RageValue            = rageValue,
        RageHealing          = rageHealing,
        RageMpDrain          = rageMpDrain,
        RageProgressDrain    = rageProgressDrain,
        RageName             = rageName,
        RageEffect           = rageEffect,
        DropGold             = dropGold,
        DropExp              = dropExp,
        DropItemsJson        = dropItemsJson,
        Group                = group,
        LevelRequired        = lvl,
        PrerequisiteQuestKey = prereq,
    };

    // ── Helper: Masterclasser boss quest (goldCost = additional gold bonus) ────
    private static BossQuest QB(
        int id, string key, string text, string category, int goldBonus,
        double bossHp, double bossStr,
        double? rageValue, double? rageHealing, double? rageMpDrain, double? rageProgressDrain, double? rageBossDef,
        string? rageName, string? rageEffect,
        int dropGold, int dropExp, string? dropItemsJson,
        string? group = null, int lvl = 0, string? prereq = null)
    => new()
    {
        Id                   = id,
        Key                  = key,
        Text                 = text,
        Category             = category,
        GemCost              = G(bossHp, rageValue),
        GoldCost             = goldBonus,
        IsBossQuest          = true,
        BossHp               = bossHp,
        BossStr              = bossStr,
        BossDef              = 1.0,
        RageValue            = rageValue,
        RageHealing          = rageHealing,
        RageMpDrain          = rageMpDrain,
        RageProgressDrain    = rageProgressDrain,
        RageName             = rageName,
        RageEffect           = rageEffect,
        DropGold             = dropGold,
        DropExp              = dropExp,
        DropItemsJson        = dropItemsJson,
        Group                = group,
        LevelRequired        = lvl,
        PrerequisiteQuestKey = prereq,
    };

    // ── Helper: Collection quest ────────────────────────────────────────────────
    // goldCost param = gold bonus for "gold"-category masterclassers; ignored otherwise
    private static BossQuest QC(
        int id, string key, string text, string category, int goldCost,
        string collectJson,
        int dropGold, int dropExp, string? dropItemsJson,
        string? group = null, int lvl = 0, string? prereq = null)
    => new()
    {
        Id                   = id,
        Key                  = key,
        Text                 = text,
        Category             = category,
        GemCost              = category == "timeTravelers" ? 0 : 4,
        GoldCost             = category == "gold" ? goldCost : 0,
        IsBossQuest          = false,
        CollectJson          = collectJson,
        DropGold             = dropGold,
        DropExp              = dropExp,
        DropItemsJson        = dropItemsJson,
        Group                = group,
        LevelRequired        = lvl,
        PrerequisiteQuestKey = prereq,
    };
}
