namespace HabitTracker.Constants
{
    public static class ItemCatalogue
    {
        // ── Rarity ───────────────────────────────────────────────────────────
        public static class Rarity
        {
            public const string Common    = "Common";
            public const string Uncommon  = "Uncommon";
            public const string Rare      = "Rare";
            public const string Epic      = "Epic";
            public const string Legendary = "Legendary";
            public const string Mythic    = "Mythic";

            public static string Color(string rarity) => rarity switch
            {
                Common    => "#ffffff",
                Uncommon  => "#4caf50",
                Rare      => "#00bcd4",
                Epic      => "#9c27b0",
                Legendary => "#ff9800",
                Mythic    => "#f44336",
                _         => "#ffffff"
            };
        }

        public record ItemDef(
            string Name,
            string Icon,
            string Description,
            int    Width,
            int    Height,
            string Category,
            string TileColor,
            string Rarity = Rarity.Common
        );

        // Equipment item definition — for items that occupy a slot on the character
        public record EquipDef(
            string Name,
            string Icon,
            string Description,
            string Slot,             // "BackpackSlot" | "ArmorSlot" | "RigSlot"
            double DamageReduction,  // % incoming damage reduction (armor)
            int?   ContainerCols,    // grid container size (backpack/rig)
            int?   ContainerRows,
            string? ContainerType   // e.g. "EquippedBackpack" | "EquippedRig" | null
        );

        public static readonly Dictionary<string, ItemDef> Items = new()
        {
            ["bread"] = new ItemDef(
                Name: "Bread", Icon: "🍞",
                Description: "A hearty loaf of bread. Restores 30 HP when consumed during battle.",
                Width: 1, Height: 1, Category: "Food", TileColor: "#7c4f1e",
                Rarity: Rarity.Common),

            ["water_bottle"] = new ItemDef(
                Name: "Water Bottle", Icon: "🧴",
                Description: "A full bottle of water. Allows you to flee from any battle.",
                Width: 2, Height: 1, Category: "Utility", TileColor: "#005f6b",
                Rarity: Rarity.Common),

            ["simple_backpack"] = new ItemDef(
                Name: "Simple Backpack", Icon: "🎒",
                Description: "A sturdy pack. Equip to unlock a 4×4 storage grid.",
                Width: 2, Height: 2, Category: "Equipment", TileColor: "#3d2b6b",
                Rarity: Rarity.Uncommon),

            ["simple_armor"] = new ItemDef(
                Name: "Simple Armor", Icon: "🛡️",
                Description: "Light plating. Equip to reduce incoming damage by 5%.",
                Width: 1, Height: 2, Category: "Equipment", TileColor: "#2b4a6b",
                Rarity: Rarity.Uncommon),

            ["simple_rig"] = new ItemDef(
                Name: "Simple Rig", Icon: "🦺",
                Description: "A tactical rig. Equip to unlock a 4×2 quick-access grid.",
                Width: 2, Height: 1, Category: "Equipment", TileColor: "#6b3a2b",
                Rarity: Rarity.Uncommon),

            ["wood"] = new ItemDef(
                Name: "Wood", Icon: "🪵",
                Description: "Raw wood from the forest. Process at the hideout for crafting material.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#5a3a1a",
                Rarity: Rarity.Common),

            ["stone"] = new ItemDef(
                Name: "Stone", Icon: "🪨",
                Description: "A chunk of stone from the forest. Process at the hideout for crafting material.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#6b6b6b",
                Rarity: Rarity.Common),

            // ── Common ───────────────────────────────────────────────────────
            ["herb_bundle"] = new ItemDef(
                Name: "Bó Thảo Dược", Icon: "❓",
                Description: "Bó thảo dược rừng khô buộc bằng dây gai. Có thể nấu thành thuốc phục hồi ở bàn thợ.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#3a6b3a",
                Rarity: Rarity.Common),

            ["flint"] = new ItemDef(
                Name: "Đá Lửa", Icon: "❓",
                Description: "Mảnh đá lửa sắc bén đẽo từ vách hang. Nguyên liệu chế tác linh hoạt và mồi lửa đáng tin cậy.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#5a5560",
                Rarity: Rarity.Common),

            ["wolf_pelt"] = new ItemDef(
                Name: "Da Sói", Icon: "❓",
                Description: "Bộ da xơ xác xé từ thám tử rừng. Vẫn còn mang theo mùi musk nhẹ của rừng sâu.",
                Width: 2, Height: 2, Category: "Material", TileColor: "#7a6a55",
                Rarity: Rarity.Common),

            ["leather_strip"] = new ItemDef(
                Name: "Dải Da", Icon: "❓",
                Description: "Dải da thuộc cắt từ da đã chế biến. Thành phần thiết yếu trong áo giáp cơ bản và quấn tay cầm vũ khí.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#7a5030",
                Rarity: Rarity.Common),

            ["iron_ore"] = new ItemDef(
                Name: "Quặng Sắt", Icon: "❓",
                Description: "Quặng sắt thô cạy từ vách hang. Nặng, đặc, và đáng được nấu thành thỏi ở bàn thợ.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#4a4855",
                Rarity: Rarity.Common),

            ["cave_mushroom"] = new ItemDef(
                Name: "Nấm Hang", Icon: "❓",
                Description: "Nấm nhợt nhạt phát sáng mờ nhạt trong bóng tối. Đặc tính chưa xác định — các nhà giả kim trả giá rất cao.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#6a3a7a",
                Rarity: Rarity.Common),

            ["tattered_cloth"] = new ItemDef(
                Name: "Vải Rách", Icon: "❓",
                Description: "Vải sờn rách để lại bởi những kẻ lang thang không tìm được đường ra. Dùng để lót bên dưới áo giáp.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#6b5040",
                Rarity: Rarity.Common),

            // ── Uncommon ─────────────────────────────────────────────────────
            ["health_vial"] = new ItemDef(
                Name: "Lọ Hồi Máu", Icon: "❓",
                Description: "Lọ thon chứa chất lỏng đỏ thẫm có mùi đồng nhẹ. Hồi phục 80 HP khi sử dụng trong chiến đấu.",
                Width: 1, Height: 2, Category: "Consumable", TileColor: "#8b1a1a",
                Rarity: Rarity.Uncommon),

            ["stamina_draught"] = new ItemDef(
                Name: "Thuốc Hồi Sức", Icon: "❓",
                Description: "Thuốc sủi bọt màu xanh có cảm giác ngứa ran trên lưỡi. Hồi phục 50 Sức Bền — hữu ích trước chuyến thám hiểm dài.",
                Width: 1, Height: 2, Category: "Consumable", TileColor: "#1a7a2a",
                Rarity: Rarity.Uncommon),

            ["iron_dagger"] = new ItemDef(
                Name: "Dao Găm Sắt", Icon: "❓",
                Description: "Lưỡi dao ngắn, cân bằng tốt với một rãnh thoát máu. Nhẹ đủ để giấu trong ủng.",
                Width: 1, Height: 3, Category: "Weapon", TileColor: "#5a6580",
                Rarity: Rarity.Uncommon),

            ["leather_cap"] = new ItemDef(
                Name: "Mũ Da", Icon: "❓",
                Description: "Mũ ôm đầu khâu từ da nấu. Bảo vệ tối thiểu nhưng không làm chậm bạn.",
                Width: 2, Height: 2, Category: "Armor", TileColor: "#7a5030",
                Rarity: Rarity.Uncommon),

            ["scout_bag"] = new ItemDef(
                Name: "Túi Thám Tử", Icon: "❓",
                Description: "Túi xách nhẹ tháo từ thám tử rừng. Rộng rãi so với kích thước. Trang bị để mở lưới lưu trữ 5×5.",
                Width: 3, Height: 2, Category: "Equipment", TileColor: "#5a4535",
                Rarity: Rarity.Uncommon),

            ["beast_fang"] = new ItemDef(
                Name: "Nanh Quái Vật", Icon: "❓",
                Description: "Nanh bén như dao rút từ hàm quái vật rừng. Thuốc giả kim mạnh và chiến tích đáng tự hào.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#c9a030",
                Rarity: Rarity.Uncommon),

            ["rope_coil"] = new ItemDef(
                Name: "Cuộn Dây Thừng", Icon: "❓",
                Description: "Mười lăm mét dây gai bện chặt. Vô số công dụng trong hoang dã — leo trèo, trói buộc, hay đặt bẫy.",
                Width: 2, Height: 2, Category: "Utility", TileColor: "#8b7545",
                Rarity: Rarity.Uncommon),

            ["crystal_shard"] = new ItemDef(
                Name: "Mảnh Pha Lê", Icon: "❓",
                Description: "Mảnh vỡ trong suốt đập theo nhịp ánh sáng bên trong mờ nhạt. Dùng trong các công thức phong ấn cấp thấp.",
                Width: 1, Height: 2, Category: "Material", TileColor: "#3a7a8b",
                Rarity: Rarity.Uncommon),

            // ── Rare ─────────────────────────────────────────────────────────
            ["mana_flask"] = new ItemDef(
                Name: "Bình Mana", Icon: "❓",
                Description: "Bình màu xanh coban xoáy với hơi khảo dị nén. Trao +15% điểm thưởng XP cho nhiệm vụ kế tiếp hoàn thành.",
                Width: 1, Height: 3, Category: "Consumable", TileColor: "#1a4a8b",
                Rarity: Rarity.Rare),

            ["silver_sword"] = new ItemDef(
                Name: "Kiếm Bạc", Icon: "❓",
                Description: "Trường kiếm với lưỡi khảm bạc được ban phước chống lại sinh vật tà ác. Sự cân bằng hoàn hảo tuyệt đối.",
                Width: 1, Height: 4, Category: "Weapon", TileColor: "#8baabb",
                Rarity: Rarity.Rare),

            ["chain_mail"] = new ItemDef(
                Name: "Áo Giáp Xích", Icon: "❓",
                Description: "Hàng nghìn vòng sắt đan xen phủ lên vải bông đệm. Nặng nhưng đáng tin cậy. Giảm 15% sát thương nhận vào.",
                Width: 2, Height: 3, Category: "Armor", TileColor: "#505a65",
                Rarity: Rarity.Rare),

            ["enchanted_lantern"] = new ItemDef(
                Name: "Đèn Lồng Phong Ấn", Icon: "❓",
                Description: "Đèn lồng đồng bị trói bởi phép lửa vĩnh cửu. Không bao giờ cạn dầu — và soi sáng những thứ tốt hơn nên để trong bóng tối.",
                Width: 2, Height: 2, Category: "Utility", TileColor: "#c99020",
                Rarity: Rarity.Rare),

            ["hunter_quiver"] = new ItemDef(
                Name: "Túi Tên Thợ Săn", Icon: "❓",
                Description: "Túi tên cốt thép với đầu mũi tên sắc bén. Mỗi mũi tên được cân bằng hoàn hảo để xuyên thấu tối đa.",
                Width: 1, Height: 3, Category: "Weapon", TileColor: "#6b3a1a",
                Rarity: Rarity.Rare),

            ["shadow_cloak"] = new ItemDef(
                Name: "Áo Choàng Bóng Tối", Icon: "❓",
                Description: "Dệt từ lụa bóng tối thu hoạch lúc nửa đêm. Đường nét người mặc mờ đi không thể đoán trước trong ánh sáng yếu.",
                Width: 3, Height: 2, Category: "Armor", TileColor: "#2a1a40",
                Rarity: Rarity.Rare),

            ["ancient_tome"] = new ItemDef(
                Name: "Cổ Thư", Icon: "❓",
                Description: "Cuốn sách dày về lore đã bị lãng quên, các trang vàng như xương. Kiến thức bên trong đủ đặc để cảm thấy nguy hiểm.",
                Width: 2, Height: 3, Category: "Utility", TileColor: "#3a2a15",
                Rarity: Rarity.Rare),

            // ── Epic ─────────────────────────────────────────────────────────
            ["dragon_scale"] = new ItemDef(
                Name: "Vảy Rồng", Icon: "❓",
                Description: "Vảy sáng bóng rụng từ một con rồng non. Gần như bất khả xâm phạm và chịu nhiệt. Nguyên liệu hàng đầu cho áo giáp cao cấp.",
                Width: 2, Height: 2, Category: "Material", TileColor: "#1a4a1a",
                Rarity: Rarity.Epic),

            ["runic_pendant"] = new ItemDef(
                Name: "Mặt Dây Chuyền Runic", Icon: "❓",
                Description: "Mặt dây chuyền đá nhỏ khắc con dấu runic vo ve khi cầm. Nhân đôi XP cho chuyến rừng kế tiếp hoàn thành.",
                Width: 1, Height: 1, Category: "Accessory", TileColor: "#6a1a8b",
                Rarity: Rarity.Epic),

            ["enchanted_blade"] = new ItemDef(
                Name: "Lưỡi Kiếm Phong Ấn", Icon: "❓",
                Description: "Lưỡi kiếm với tia sét bị bẫy có thể nhìn thấy dọc theo fuller. Đòn đánh có 20% cơ hội choáng kẻ thù một lượt.",
                Width: 1, Height: 4, Category: "Weapon", TileColor: "#1a4a9b",
                Rarity: Rarity.Epic),

            ["void_crystal"] = new ItemDef(
                Name: "Pha Lê Hư Không", Icon: "❓",
                Description: "Tinh thể đen đặc hấp thụ toàn bộ ánh sáng xung quanh. Ổn định khi một mình nhưng cực kỳ phản ứng trong các phản ứng giả kim.",
                Width: 1, Height: 2, Category: "Material", TileColor: "#150a20",
                Rarity: Rarity.Epic),

            ["plate_cuirass"] = new ItemDef(
                Name: "Giáp Ngực Thép", Icon: "❓",
                Description: "Tấm ngực thép nặng khắc ký hiệu bảo hộ dọc theo từng đường nối. Không khuất phục. Giảm 30% sát thương đến.",
                Width: 3, Height: 3, Category: "Armor", TileColor: "#3a4555",
                Rarity: Rarity.Epic),

            ["tactical_webbing"] = new ItemDef(
                Name: "Dây Đeo Chiến Thuật", Icon: "❓",
                Description: "Dây đai mô-đun phủ đầy vòng gắn và túi rút nhanh. Trang bị để mở lưới truy cập nhanh 6×3.",
                Width: 3, Height: 2, Category: "Equipment", TileColor: "#3a2a15",
                Rarity: Rarity.Epic),

            // ── Legendary ────────────────────────────────────────────────────
            ["phoenix_feather"] = new ItemDef(
                Name: "Lông Phượng Hoàng", Icon: "❓",
                Description: "Chiếc lông phát sáng luôn ấm khi chạm vào. Người mang được hồi sinh một lần khi nhận sát thương chí mạng, phục hồi về 50% HP.",
                Width: 1, Height: 3, Category: "Accessory", TileColor: "#c94500",
                Rarity: Rarity.Legendary),

            ["elder_staff"] = new ItemDef(
                Name: "Trượng Cổ Đại", Icon: "❓",
                Description: "Cây gậy gai sần đẽo từ gỗ lõi bị sét đánh ngàn lần. Nó kêu răng rắc với ý định cổ xưa không thể đo đếm.",
                Width: 1, Height: 5, Category: "Weapon", TileColor: "#5a3000",
                Rarity: Rarity.Legendary),

            ["wardens_shield"] = new ItemDef(
                Name: "Khiên Warden", Icon: "❓",
                Description: "Khiên tháp đẽo từ gỗ rồng hóa đá, viền sắt lạnh. Giảm 45% sát thương nhận vào. Nặng bằng một đứa trẻ nhỏ.",
                Width: 3, Height: 3, Category: "Armor", TileColor: "#2a5015",
                Rarity: Rarity.Legendary),

            ["infinity_satchel"] = new ItemDef(
                Name: "Túi Vô Tận", Icon: "❓",
                Description: "Túi xách được phong ấn bằng phép không gian túi. Không nặng dù đựng bao nhiêu. Trang bị để mở lưới lưu trữ 8×8.",
                Width: 3, Height: 3, Category: "Equipment", TileColor: "#3a1a6b",
                Rarity: Rarity.Legendary),

            // ── Mythic ───────────────────────────────────────────────────────
            ["heart_of_the_forest"] = new ItemDef(
                Name: "Trái Tim Rừng", Icon: "❓",
                Description: "Lõi pha lê đập ở trung tâm khu rừng cổ đại. Bản thân rừng uốn theo ý chí của bất kỳ ai mang nó.",
                Width: 2, Height: 2, Category: "Accessory", TileColor: "#0a3a0a",
                Rarity: Rarity.Mythic),

            ["abyssal_tome"] = new ItemDef(
                Name: "Cổ Thư Vực Thẳm", Icon: "❓",
                Description: "Cuốn sách bìa da bóng tối với các trang làm từ hư không nén. Đọc nó trao kiến thức không có người trần nào được phép sở hữu.",
                Width: 2, Height: 4, Category: "Utility", TileColor: "#0a0a15",
                Rarity: Rarity.Mythic),

            ["crown_of_echoes"] = new ItemDef(
                Name: "Vương Miện Vọng Âm", Icon: "❓",
                Description: "Vương miện cộng hưởng với giọng nói của mọi vị vua đã từng đội nó. Ý chí kết hợp của họ khuếch đại mọi chỉ số của người mang.",
                Width: 3, Height: 2, Category: "Accessory", TileColor: "#7a6000",
                Rarity: Rarity.Mythic),

            // ── Vật phẩm độc quyền từng quái vật ────────────────────────────
            ["bone_shard"] = new ItemDef(
                Name: "Mảnh Xương", Icon: "❓",
                Description: "Mảnh xương phong ấn sần sùi. Từ chối vỡ vụn dù chịu bất kỳ lực nào. Các nhà giả kim trân trọng nó để tăng cường thuốc.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#c9c0a0",
                Rarity: Rarity.Common),

            ["toxic_gland"] = new ItemDef(
                Name: "Tuyến Độc", Icon: "❓",
                Description: "Túi từ cóc đầm lầy, vẫn còn rỉ nọc hổ phách. Cẩn thận khi cầm — lớp phủ bên ngoài đã đủ để phồng rộp da.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#5a7a1a",
                Rarity: Rarity.Uncommon),

            ["corrupted_bark"] = new ItemDef(
                Name: "Vỏ Cây Tha Hóa", Icon: "❓",
                Description: "Vỏ cây bóc từ cổ thụ bị vặn vẹo. Những tĩnh mạch đen chạy qua như mực. Mạnh trong các công thức phong ấn bóng tối.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#2a1a0a",
                Rarity: Rarity.Rare),

            ["iron_core"] = new ItemDef(
                Name: "Lõi Sắt", Icon: "❓",
                Description: "Khối cầu sắt nén bằng nắm tay ở tim golem. Vẫn còn đập nhẹ với lực linh hoạt hóa đã điều khiển cấu trúc.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#3a3a50",
                Rarity: Rarity.Epic),

            ["shadow_essence"] = new ItemDef(
                Name: "Tinh Chất Bóng Tối", Icon: "❓",
                Description: "Lọ bóng tối bán lỏng thu hoạch từ kẻ rình bóng đã bị tiêu diệt. Nó dịch chuyển và quằn quại ngay cả trong kính bịt kín.",
                Width: 1, Height: 1, Category: "Material", TileColor: "#0a0a20",
                Rarity: Rarity.Rare),

            ["serpent_scale"] = new ItemDef(
                Name: "Vảy Rắn", Icon: "❓",
                Description: "Vảy rộng, óng ánh từ thuồng luồng hồ sâu. Linh hoạt nhưng cứng hơn sắt — lý tưởng cho áo giáp nhẹ cao cấp.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#1a5a4a",
                Rarity: Rarity.Uncommon),

            ["ancient_bone"] = new ItemDef(
                Name: "Xương Cổ Đại", Icon: "❓",
                Description: "Đoạn xương từ khổng lồ xương — to lớn, đặc, và cổ xưa hơn ký ức. Phát ra năng lượng hắc ám còn sót lại.",
                Width: 2, Height: 1, Category: "Material", TileColor: "#a09070",
                Rarity: Rarity.Rare),

            ["void_tendril"] = new ItemDef(
                Name: "Xúc Tu Hư Không", Icon: "❓",
                Description: "Chi bị cắt đứt của lữ hành hư không, vẫn còn vươn dài ngay cả khi chết. Nó một phần xuyên qua bất kỳ bề mặt nào nó đặt lên.",
                Width: 1, Height: 2, Category: "Material", TileColor: "#0a0515",
                Rarity: Rarity.Epic),

            ["warden_heartwood"] = new ItemDef(
                Name: "Gỗ Lõi Warden", Icon: "❓",
                Description: "Lõi gỗ cổ đại dày đặc xé từ ngực Warden. Nó đập. Chậm. Nguyên liệu chế tác được trân trọng nhất trong rừng.",
                Width: 2, Height: 2, Category: "Material", TileColor: "#1a3a0a",
                Rarity: Rarity.Legendary),

            // ── Vật phẩm Mythic độc quyền (mỗi quái vật một loại) ────────────
            ["wolf_spirit_gem"] = new ItemDef(
                Name: "Linh Ngọc Sói", Icon: "❓",
                Description: "Viên ngọc hình thành bên trong thám tử rừng đã chạy quá lâu và quá xa đến mức vượt qua cái chết. Linh hồn vẫn còn săn mồi bên trong.",
                Width: 1, Height: 1, Category: "Accessory", TileColor: "#7a9aaa",
                Rarity: Rarity.Mythic),

            ["ancient_bowstring"] = new ItemDef(
                Name: "Dây Cung Cổ Đại", Icon: "❓",
                Description: "Dây cung của cung thủ huyền thoại đã sống sót qua bản thân cái chết. Không bao giờ đứt. Không bao giờ trượt.",
                Width: 1, Height: 3, Category: "Weapon", TileColor: "#c0a040",
                Rarity: Rarity.Mythic),

            ["eye_of_the_bog"] = new ItemDef(
                Name: "Mắt Đầm Lầy", Icon: "❓",
                Description: "Mắt được bảo quản hoàn hảo từ con cóc đầm lầy đủ cổ xưa để đã nhìn thấy thế giới trước khi nó có tên. Nhìn qua nó tiết lộ những thứ không hiện diện.",
                Width: 1, Height: 1, Category: "Accessory", TileColor: "#4a6a0a",
                Rarity: Rarity.Mythic),

            ["brute_war_mask"] = new ItemDef(
                Name: "Mặt Nạ Chiến Tranh Brute", Icon: "❓",
                Description: "Mặt nạ đẽo từ sọ con mồi đầu tiên của brute và đeo qua ngàn trận chiến sau đó. Khủng khiếp tỏa ra từ nó.",
                Width: 2, Height: 2, Category: "Accessory", TileColor: "#3a1a0a",
                Rarity: Rarity.Mythic),

            ["soul_of_iron"] = new ItemDef(
                Name: "Linh Hồn Sắt", Icon: "❓",
                Description: "Tinh thần linh hoạt hóa tách ra từ lõi golem — ý thức chỉ tồn tại để tuân lệnh và đã sống sót qua mọi lệnh nó từng được đưa ra.",
                Width: 1, Height: 1, Category: "Accessory", TileColor: "#404858",
                Rarity: Rarity.Mythic),

            ["shadow_veil"] = new ItemDef(
                Name: "Màn Bóng Tối", Icon: "❓",
                Description: "Không phải vải — bóng tối nén lại, được định hình. Kẻ rình bóng đeo cái này như thân xác của nó. Mặc nó khiến bạn vô hình giữa các nguồn sáng.",
                Width: 2, Height: 3, Category: "Armor", TileColor: "#050510",
                Rarity: Rarity.Mythic),

            ["serpent_sovereign_scale"] = new ItemDef(
                Name: "Vảy Chúa Rắn", Icon: "❓",
                Description: "Vảy đơn từ đỉnh đầu rắn — to hơn khiên kite, cứng hơn kim cương, lấp lánh với ký ức của vùng nước thẳm sâu.",
                Width: 2, Height: 2, Category: "Material", TileColor: "#004a3a",
                Rarity: Rarity.Mythic),

            ["colossus_skull"] = new ItemDef(
                Name: "Sọ Khổng Lồ", Icon: "❓",
                Description: "Sọ hợp nhất của một trăm chiến binh đã ngã xuống, nén lại thành một bởi ràng buộc hắc ám của khổng lồ. Nó thì thầm bằng một trăm giọng nói.",
                Width: 2, Height: 2, Category: "Accessory", TileColor: "#8a8070",
                Rarity: Rarity.Mythic),

            ["void_heart"] = new ItemDef(
                Name: "Trái Tim Hư Không", Icon: "❓",
                Description: "Bất cứ thứ gì đóng vai trò là tim trong sinh vật đến từ bên kia màn vô thực. Nó đập theo khoảng thời gian không tương ứng thang đo nào đã biết. Thực tại dao động gần nó.",
                Width: 1, Height: 1, Category: "Accessory", TileColor: "#03010a",
                Rarity: Rarity.Mythic),
        };

        public static readonly Dictionary<string, EquipDef> Equipment = new()
        {
            ["simple_backpack"] = new EquipDef(
                Name: "Simple Backpack", Icon: "🎒",
                Description: "Unlocks a 4×4 storage grid.",
                Slot: SLOT_BACKPACK, DamageReduction: 0,
                ContainerCols: 4, ContainerRows: 4, ContainerType: EQUIPPED_BACKPACK),

            ["simple_armor"] = new EquipDef(
                Name: "Simple Armor", Icon: "🛡️",
                Description: "Reduces incoming damage by 5%.",
                Slot: SLOT_ARMOR, DamageReduction: 5.0,
                ContainerCols: null, ContainerRows: null, ContainerType: null),

            ["simple_rig"] = new EquipDef(
                Name: "Simple Rig", Icon: "🦺",
                Description: "Unlocks a 4×2 quick-access grid (4 slots of 2×1).",
                Slot: SLOT_RIG, DamageReduction: 0,
                ContainerCols: 4, ContainerRows: 2, ContainerType: EQUIPPED_RIG),
        };

        public static bool CanRotate(string itemId) =>
            Items.TryGetValue(itemId, out var d) && d.Width != d.Height;

        public static bool IsEquippable(string itemId) => Equipment.ContainsKey(itemId);

        public static (int Cols, int Rows) ContainerSize(string containerType) => containerType switch {
            "Backpack"         => (4, 1),
            "EquippedBackpack" => (4, 4),
            "EquippedRig"      => (4, 2),
            _                  => (10, 10)
        };

        // Dynamic storage size based on Storage Room facility level
        public static (int Cols, int Rows) StorageSizeForLevel(int level) =>
            (10, 10 + (level - 1) * 3);

        // Exact item size required to enter a container (null = no restriction)
        public static (int W, int H)? SlotConstraint(string containerType) => containerType switch {
            "Backpack"    => (1, 1),
            "EquippedRig" => (1, 2),
            _             => null
        };

        public const int CELL_PX = 64;
        public const string STORAGE          = "Storage";
        public const string BACKPACK         = "Backpack";
        public const string HIDEOUT_STORAGE  = "HideoutStorage";
        public const string EQUIPPED_BACKPACK = "EquippedBackpack";
        public const string EQUIPPED_RIG     = "EquippedRig";
        public const string SLOT_BACKPACK    = "BackpackSlot";
        public const string SLOT_ARMOR       = "ArmorSlot";
        public const string SLOT_RIG         = "RigSlot";
    }
}
