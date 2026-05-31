using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace HabitTracker.Models;

public class BossQuest
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Key { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Text { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Completion { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Group { get; set; }

    public int LevelRequired { get; set; } = 0;

    [MaxLength(100)]
    public string? PrerequisiteQuestKey { get; set; }

    /// <summary>Gem cost to purchase scroll. 0 = not purchasable (time travel).</summary>
    public int GemCost { get; set; } = 4;

    /// <summary>Additional gold required alongside gems (masterclasser quests only).</summary>
    public int GoldCost { get; set; } = 0;

    public bool IsBossQuest { get; set; } = true;

    // ── Boss stats ───────────────────────────────────────────────────────────
    public double? BossHp { get; set; }
    public double BossStr { get; set; } = 1.0;
    public double BossDef { get; set; } = 1.0;

    // ── Rage system ──────────────────────────────────────────────────────────
    public double? RageValue { get; set; }
    public double? RageHealing { get; set; }
    public double? RageMpDrain { get; set; }
    public double? RageProgressDrain { get; set; }

    [MaxLength(100)]
    public string? RageName { get; set; }

    [MaxLength(500)]
    public string? RageEffect { get; set; }

    // ── Collection quest items (JSON) ────────────────────────────────────────
    public string? CollectJson { get; set; }

    // ── Drop rewards ─────────────────────────────────────────────────────────
    public int DropGold { get; set; } = 0;
    public int DropExp  { get; set; } = 0;

    // Format: [{type,key,count,onlyOwner?}, ...]
    public string? DropItemsJson { get; set; }

    // ── Computed helpers (not mapped) ────────────────────────────────────────
    public string BossImagePath        => $"/images/habitica/quests/bosses/quest_{Key}.png";
    public string ScrollImagePath      => $"/images/habitica/quests/scrolls/inventory_quest_scroll_{Key}.png";
    public string ScrollLockedImagePath => $"/images/habitica/quests/scrolls/inventory_quest_scroll_{Key}_locked.png";

    public string DropDescription
    {
        get
        {
            var parts = new List<string>();

            if (DropItemsJson is not null)
            {
                try
                {
                    using var doc = JsonDocument.Parse(DropItemsJson);
                    foreach (var item in doc.RootElement.EnumerateArray())
                    {
                        string type  = item.TryGetProperty("type",  out var t) ? t.GetString() ?? "" : "";
                        string key   = item.TryGetProperty("key",   out var k) ? k.GetString() ?? "" : "";
                        int    count = item.TryGetProperty("count", out var c) ? c.GetInt32() : 1;
                        bool   ownerOnly = item.TryGetProperty("onlyOwner", out var o) && o.GetBoolean();

                        string label = type switch
                        {
                            "eggs"           => $"×{count} {key} Egg",
                            "hatchingPotions"=> $"×{count} {key} Potion",
                            "food"           => $"×{count} {key}",
                            "gear"           => ownerOnly ? "Gear (leader)" : "Special Gear",
                            "quests"         => ownerOnly ? "Next Scroll (leader)" : "Quest Scroll",
                            "mounts"         => $"×{count} Mount",
                            "pets"           => $"×{count} Pet",
                            _                => key,
                        };
                        parts.Add(label);
                    }
                }
                catch { /* malformed JSON — skip */ }
            }

            if (DropGold > 0) parts.Add($"{DropGold} GP");
            if (DropExp  > 0) parts.Add($"{DropExp} XP");

            return parts.Count > 0 ? string.Join(" · ", parts) : "No drops";
        }
    }
}
