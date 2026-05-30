# Plan: Equipment Slots (back/eyewear/headAccessory) + Background Customization

## Context
Shop sells back, eyewear, and headAccessory gear but Equipment tab only shows 4 slots (weapon/armor/head/shield). Users can buy these items but have no way to equip them. Separately, 395 background images exist in wwwroot but there is no UI, no User model field, and no controller logic to use them.

Habitica source confirmed: same 8-slot gear model, background stored as `user.preferences.background` string key, background image path = `background_{key}.png`.

---

## Feature 1: Expose back / eyewear / headAccessory in Equipment tab

### Root cause
`Equipment/Index.cshtml` line 7: `primarySlots = new[] { "weapon", "armor", "head", "shield" }`.  
All backend logic (controller, SetSlot, EquippedKeys dict) already handles all 8 slots. Pure view bug.

### Avatar layer order (from Habitica avatar.vue)
```
mount-body → BACK → skin → shirt → armor → bangs → hair → mustache → beard → EYEWEAR → head → HEAD_ACCESSORY → shield → weapon → mount-head → pet
```

### Changes

**1. `HabitTracker\Views\Equipment\Index.cshtml`**
- Line 7: add `"back"`, `"eyewear"`, `"headAccessory"` to `primarySlots`
- Lines 33-37: add gear variable declarations:
  ```csharp
  var backGear          = equippedGear.GetValueOrDefault("back");
  var eyewearGear       = equippedGear.GetValueOrDefault("eyewear");
  var headAccessoryGear = equippedGear.GetValueOrDefault("headAccessory");
  ```
- Avatar layers (lines 149-194): insert 3 new `<img>` tags at correct z-positions:
  - `id="layer-back"` — before `layer-skin` (first after mount-body)
  - `id="layer-eyewear"` — after `layer-beard`
  - `id="layer-headAccessory"` — after `layer-head`
- JS `updateAvatarLayer` already uses `'#layer-' + slot` → no JS changes needed

**2. `HabitTracker\Views\Character\Customize.cshtml`**
- Lines 12-16: add `backGear`, `eyewearGear`, `headAccessoryGear` via existing `FindGear()`:
  ```csharp
  var backGear          = FindGear(u.EquippedBack);
  var eyewearGear       = FindGear(u.EquippedEyewear);
  var headAccessoryGear = FindGear(u.EquippedHeadAccessory);
  ```
- Avatar layers (lines 241-287): insert same 3 `<img>` tags at same positions

**3. `HabitTracker\Views\Character\Index.cshtml`**
- Same pattern: add gear variable declarations + 3 avatar layer `<img>` tags

All 3 new layer `<img>` tags follow same pattern as existing gear layers:
```html
<img id="layer-back"
     src="@(backGear != null ? backGear.GetWornImagePath(bodyType) : "")"
     style="position:absolute;top:0;left:0;width:90px;height:90px;image-rendering:pixelated;@(backGear == null ? "display:none;" : "")" />
```

---

## Feature 2: Background customization

### Data model changes

**4. `HabitTracker\Models\User.cs`**  
Add after existing customization fields (around line 177):
```csharp
[StringLength(100)]
public string? Background { get; set; }
```

**5. EF Migration**  
Run: `dotnet ef migrations add AddUserBackground --project HabitTracker`  
This adds `Background` VARCHAR(100) NULL column to `Users` table.

### ViewModel + Controller

**6. `HabitTracker\Models\ViewModels\CustomizeViewModel.cs`**  
Add: `public List<string> Backgrounds { get; set; } = new();`

**7. `HabitTracker\Controllers\CharacterController.cs`**  

In `Customize()` action — enumerate backgrounds from filesystem (same pattern as skin/hair):
```csharp
var bgDir      = Path.Combine(root, "images", "habitica", "backgrounds");
var backgrounds = Directory.EnumerateFiles(bgDir, "background_*.png")
    .Select(f => After(Path.GetFileNameWithoutExtension(f), "background_"))
    .OrderBy(b => b)
    .ToList();
// add to vm: Backgrounds = backgrounds
```

Add new AJAX endpoint:
```csharp
// POST /Character/SetBackground
[HttpPost("SetBackground")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SetBackground([FromForm] string? key)
{
    var userId = GetUserId();
    if (userId == null) return Json(new { success = false });
    var user = await _context.Users.FindAsync(userId.Value);
    if (user == null) return Json(new { success = false });
    user.Background = string.IsNullOrWhiteSpace(key) ? null : key;
    user.UpdatedAt = DateTime.UtcNow;
    await _context.SaveChangesAsync();
    return Json(new { success = true });
}
```

### View changes

**8. `HabitTracker\Views\Character\Customize.cshtml`**

a) Add "🌄 Background" tab button alongside existing Appearance/Hair tabs (line ~50).

b) Add background tab pane with a scrollable grid of icon thumbnails:
   - Each item: `icon_background_{key}.png` (60×60px), clicking calls `setBackground(key)` JS
   - Highlight currently selected background (`u.Background`)
   - "None" option at start to clear background

c) Avatar preview: render background via CSS `background-image` on the `#avatar-preview` div.  
   Server-side initial render:
   ```html
   style="... @(!string.IsNullOrEmpty(u.Background) ? $"background-image:url('/images/habitica/backgrounds/background_{u.Background}.png');background-size:cover;background-position:center top;" : "")"
   ```
   JS `setBackground(key)`:
   ```javascript
   function setBackground(key) {
       var el = document.getElementById('avatar-preview');
       if (key) {
           el.style.backgroundImage = "url('/images/habitica/backgrounds/background_" + key + ".png')";
           el.style.backgroundSize = 'cover';
           el.style.backgroundPosition = 'center top';
       } else {
           el.style.backgroundImage = '';
       }
       fetch('/Character/SetBackground', {
           method: 'POST',
           headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
           body: 'key=' + encodeURIComponent(key || '') + '&__RequestVerificationToken=' + encodeURIComponent(csrf())
       });
   }
   ```

**9. `HabitTracker\Views\Equipment\Index.cshtml`** and **`Character\Index.cshtml`**  
Add same background CSS to their `#avatar-preview` divs (server-side initial render only).

---

## Files modified (summary)

| File | Changes |
|------|---------|
| `Models\User.cs` | + `Background` field |
| `Models\ViewModels\CustomizeViewModel.cs` | + `Backgrounds` list |
| `Controllers\CharacterController.cs` | + background enum in Customize(); + SetBackground POST |
| `Views\Equipment\Index.cshtml` | + 3 slots in primarySlots; + gear vars; + 3 avatar layers; + bg css |
| `Views\Character\Customize.cshtml` | + gear vars; + 3 avatar layers; + bg css; + bg tab UI |
| `Views\Character\Index.cshtml` | + gear vars; + 3 avatar layers; + bg css |
| EF Migration | AddUserBackground |

---

## Verification
1. `dotnet build` — 0 errors
2. `dotnet ef migrations add AddUserBackground` — migration file created
3. Run app → Equipment tab → buy a back/eyewear/headAccessory item → equip → avatar updates + slot shows equipped item
4. Run app → Character → Customize → Background tab → select background → avatar preview shows background immediately; navigate away and back → background persists
5. Character sheet and Equipment page show the background behind the avatar
