# PCB Help System - Visual Guide

## 🎨 What Users Will See

This document shows the visual presentation of the help system from the user's perspective.

---

## 1️⃣ On Application Launch

### First Dialog: General Welcome Guide

```
┌───────────────────────────────────────────────────────────┐
│  🚀 Getting Started Guide                          [X]    │
├───────────────────────────────────────────────────────────┤
│                                                            │
│  🎉 Welcome to Digital Production Config Editor!          │
│                                                            │
│  📋 QUICK START GUIDE                                     │
│  ────────────────────                                     │
│                                                            │
│  1️⃣  STEP 1: Build Your Configuration                    │
│     • Browse the Master Template (left panel)             │
│     • Click ➕ Add to copy configurations you need        │
│     • Click 🗑 Delete to remove unwanted ones             │
│     • Must add at least 1 configuration                   │
│                                                            │
│  2️⃣  STEP 2: Manage Packages                             │
│     • Select a configuration from dropdown                │
│     • ➕ Add New Package to create packages               │
│     • Edit or Delete existing packages                    │
│                                                            │
│  3️⃣  STEP 3: Review & Save                                │
│     • Review your configuration summary                   │
│     • Click 💾 Save to save your custom XML               │
│                                                            │
│  💡 KEY CONCEPTS                                          │
│  ────────────────────                                     │
│  • Master Template = Read-only library (never modified)   │
│  • Your Config = Custom XML you're building               │
│  • Configuration = Top-level category                     │
│  • Package = Product-specific settings                    │
│                                                            │
│  🔘 NAVIGATION BUTTONS                                    │
│  ────────────────────                                     │
│  ❓ Help - Show this guide anytime                        │
│  🏠 Return to Start - Go back to Step 1                   │
│  📄 New - Create blank configuration                      │
│  📂 Open - Load existing file                             │
│  🔄 Reload Master - Refresh template                      │
│  ⬅ Back / Next ➡ - Navigate steps                        │
│  💾 Save / Save As - Save your work                       │
│                                                            │
│  ⚠️  IMPORTANT NOTES                                      │
│  ────────────────────                                     │
│  • Changes are NOT auto-saved - always click 💾 Save     │
│  • Master XML is protected and never modified             │
│  • You can create multiple custom XML files               │
│                                                            │
│  Ready to create your configuration? Click OK to begin!   │
│                                                            │
│  💡 TIP: Click ❓ Help button anytime for instructions!   │
│                                                            │
│                              [ OK ]                        │
└───────────────────────────────────────────────────────────┘
```

**User Action:** Click OK

---

### Second Dialog: PCB-Specific Guide (Appears Immediately After)

```
┌───────────────────────────────────────────────────────────┐
│  ⚠️  🔲 PCB Configuration - Special Guide          [X]    │
├───────────────────────────────────────────────────────────┤
│                                                            │
│  🔲 SPECIAL GUIDE: PCB Format Configuration               │
│                                                            │
│  ⚠️  COMMON MISTAKE - PLEASE READ!                        │
│  ────────────────────────────────────                     │
│                                                            │
│  PCB configurations work differently than other configs!  │
│                                                            │
│  ❌ WRONG WAY (Common Mistake):                           │
│     • Adding a NEW PCB Format Config each time            │
│     • Clicking "➕ Add PCB Format Config" repeatedly      │
│     • Creating multiple PCB configs                       │
│                                                            │
│  ✅ CORRECT WAY:                                          │
│     1. Add PCB Format Config ONCE (only first time)       │
│     2. Click ✏ Edit button to open the PCB dialog         │
│     3. Add/Edit/Delete ISLANDS inside the existing PCB    │
│     4. Each product = Different Island (not different PCB)│
│                                                            │
│  📋 TWO WORKFLOWS:                                        │
│  ────────────────────────────────────                     │
│                                                            │
│  🆕 WORKFLOW 1: ADDING PCB (First Time Only)              │
│     Step 1: Scroll to "🔲 PCB Format Config" section     │
│     Step 2: Click "➕ Add PCB Format Config"              │
│     Step 3: PCB appears in right panel                    │
│     ⚠️  Do this ONLY ONCE per XML file!                   │
│                                                            │
│  ✏️  WORKFLOW 2: EDITING EXISTING PCB (Most Common)       │
│     Step 1: Find "PCB Format Config" in right panel       │
│     Step 2: Click "✏ Edit" button                         │
│     Step 3: In the dialog:                                │
│        • Click "➕ Add Island" for new product            │
│        • Edit existing island values                      │
│        • Click "🗑 Delete" to remove islands              │
│     Step 4: Click "💾 Save" to apply changes              │
│                                                            │
│  🏝️ UNDERSTANDING ISLANDS                                 │
│  ────────────────────────────────────                     │
│  • 1 PCB Config = Multiple Islands                        │
│  • 1 Island = 1 Product/PCB variant                       │
│  • Island has: ID, Strip Unit (X,Y), Panel Strip (X,Y)   │
│                                                            │
│  📝 EXAMPLE SCENARIO:                                     │
│  ────────────────────────────────────                     │
│  You have 3 different products:                           │
│     Product A → Island 1 (50x17, 2x4)                     │
│     Product B → Island 2 (52x17, 2x4)                     │
│     Product C → Island 3 (48x16, 3x4)                     │
│                                                            │
│  All in ONE PCB Config, not three separate configs!       │
│                                                            │
│  💡 REMEMBER:                                             │
│  ────────────────────────────────────                     │
│  • Add PCB Config = ONCE per XML file                     │
│  • Edit PCB Config = Add islands inside                   │
│  • Different products = Different islands                 │
│  • Same PCB section for all products!                     │
│                                                            │
│  Click OK to start using the editor!                      │
│                                                            │
│                              [ OK ]                        │
└───────────────────────────────────────────────────────────┘
```

**Icon:** ⚠️ Warning icon (yellow triangle) to grab attention  
**User Action:** Click OK to proceed to main application

---

## 2️⃣ Main Application Window

### Navigation Bar with Help Button

```
┌────────────────────────────────────────────────────────────────┐
│  Digital Production Config Editor                              │
│  Manage XML Configuration Packages                             │
├────────────────────────────────────────────────────────────────┤
│                                                                 │
│  [Main Content Area - Step 1, 2, or 3]                         │
│                                                                 │
├────────────────────────────────────────────────────────────────┤
│  Status: Ready                                                  │
├────────────────────────────────────────────────────────────────┤
│                                                                 │
│  [❓ Help] [🏠 Return to Start] [📄 New] [📂 Open]            │
│  [🔄 Reload Master] [⬅ Back] [Next ➡] [💾 Save] [💾 Save As] │
│        ↑                                                        │
│        │                                                        │
│   LIGHT YELLOW - Stands out for easy access                    │
│                                                                 │
└────────────────────────────────────────────────────────────────┘
```

**Help Button Details:**
- Color: Light golden yellow background
- Position: First button on the left
- Tooltip (on hover): "Show quick start guide and instructions"
- Action: Shows both welcome guide and PCB guide

---

## 3️⃣ Step 1: Left Panel (Master Template)

### PCB Format Config Section with Warning

```
┌──────────────────────────────────────────┐
│  📋 Production User Configs              │  ← PURPLE/LAVENDER
│  Select configuration nodes to add...    │
└──────────────────────────────────────────┘

[List of production configs with ➕ Add buttons]

┌──────────────────────────────────────────┐
│  🔧 Developer Validation Config          │  ← ORANGE/PEACH
│  Developer validation and equipment...   │
└──────────────────────────────────────────┘

[List of developer configs with ➕ Add buttons]

┌──────────────────────────────────────────┐
│  🔲 PCB Format Config                    │  ← BLUE
│                                           │
│  PCB panel and island configuration      │
│                                           │
│  ⚠️ Important: Add PCB Config ONCE,     │  ← RED WARNING
│     then Edit to add islands!            │
└──────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│        ➕ Add PCB Format Config          │  ← LIGHT BLUE BUTTON
└──────────────────────────────────────────┘
         ↑
         └─ Tooltip: "Add PCB Format Config ONCE. 
            After adding, use the Edit button to 
            manage islands."
```

---

## 4️⃣ Step 1: Right Panel (Your Configuration)

### PCB Format Config with Help Tip Box

```
┌──────────────────────────────────────────┐
│  📝 Your New Configuration               │  ← GREEN
│  3 production config(s)                  │
└──────────────────────────────────────────┘

Production User Configs:
[List of added configs with ✏ Edit and 🗑 Delete]

Developer Validation Configs:
[List of added configs with ✏ Edit and 🗑 Delete]

PCB Format Config:

┌──────────────────────────────────────────┐  ← ORANGE TIP BOX
│  💡 PCB Quick Tip:                       │
│                                           │
│  • Click ✏ Edit to add/modify islands   │
│    (most common)                          │
│  • Each product = Different island        │
│    inside same PCB                        │
└──────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│  PcbFormatConfig                          │
│  Status: Added (2 items)                  │
│                                           │
│                  [ ✏ Edit ]  [ 🗑 Delete ]│
└──────────────────────────────────────────┘
                       ↑           ↑
                       │           └─ Tooltip: "Remove entire 
                       │              PCB Format Config section"
                       │
                       └─ Tooltip: "Click here to add/edit islands.
                          Each island = one product variant."
```

**Visual Highlights:**
1. **Orange Tip Box** - Stands out with contrasting color
2. **Edit Button Emphasis** - Clearly marked as "most common"
3. **Contextual Tooltips** - Hover guidance on both buttons

---

## 5️⃣ Tooltip Examples (On Hover)

### Add PCB Format Config Button (Left Panel)

```
┌────────────────────────────────────────────┐
│  ➕ Add PCB Format Config                  │
└────────────────────────────────────────────┘
         ↓
    ┌────────────────────────────────────────┐
    │  Add PCB Format Config ONCE. After     │
    │  adding, use the Edit button to        │
    │  manage islands.                       │
    └────────────────────────────────────────┘
```

### Edit Button (Right Panel)

```
┌────────────────┐
│   ✏ Edit      │
└────────────────┘
         ↓
    ┌────────────────────────────────────────┐
    │  Click here to add/edit islands.       │
    │  Each island = one product variant.    │
    └────────────────────────────────────────┘
```

### Delete Button (Right Panel)

```
┌────────────────┐
│  🗑 Delete    │
└────────────────┘
         ↓
    ┌────────────────────────────────────────┐
    │  Remove entire PCB Format Config       │
    │  section                                │
    └────────────────────────────────────────┘
```

---

## 6️⃣ Help Button Action Flow

### User Clicks ❓ Help Button

```
User clicks: [❓ Help]
      ↓
Shows Dialog 1: General Welcome Guide
      ↓
User clicks: [OK]
      ↓
Shows Dialog 2: PCB-Specific Guide
      ↓
User clicks: [OK]
      ↓
Returns to application
```

**Note:** Help button can be clicked anytime - during any step, any workflow state.

---

## 7️⃣ Color Palette Summary

```
┌─────────────────────────────────────────────────┐
│  ELEMENT                │  COLOR                │
├─────────────────────────┼───────────────────────┤
│  Help Button            │  🟨 Light Golden Yellow│
│  PCB Warning Text       │  🔴 Red               │
│  PCB Tip Box            │  🟠 Orange            │
│  PCB Section            │  🔵 Blue              │
│  Production Configs     │  🟣 Purple/Lavender   │
│  Your Configs           │  🟢 Green             │
│  Developer Configs      │  🟠 Orange/Peach      │
└─────────────────────────┴───────────────────────┘
```

---

## 8️⃣ Typography & Size Reference

```
┌─────────────────────────────────────────────────┐
│  ELEMENT                │  SIZE    │  WEIGHT    │
├─────────────────────────┼──────────┼────────────┤
│  Dialog Title           │  Default │  Bold      │
│  Section Headers        │  14pt    │  Bold      │
│  Warning Text (Red)     │  11pt    │  Bold      │
│  Tip Box Header         │  11pt    │  Bold      │
│  Tip Box Content        │  10pt    │  Regular   │
│  Button Text            │  Default │  Regular   │
│  Tooltip Text           │  Default │  Regular   │
└─────────────────────────┴──────────┴────────────┘
```

---

## 9️⃣ User Journey Visualization

### Scenario A: First-Time User (Correct Flow)

```
1. Launch App
   └─> Welcome Guide appears ✅
       └─> Read general instructions
           └─> Click OK
               └─> PCB Guide appears ⚠️
                   └─> Read PCB instructions carefully
                       └─> Click OK
                           └─> See Step 1 with visual warnings
                               └─> Click "➕ Add PCB Format Config" (once)
                                   └─> PCB added to right panel
                                       └─> Click "✏ Edit" button
                                           └─> Add islands in dialog
                                               └─> SUCCESS! ✅
```

### Scenario B: Confused User (Gets Help)

```
1. User forgets PCB workflow
   └─> Tries to click "➕ Add PCB Format Config" again
       └─> Sees red warning: "Add PCB Config ONCE"
           └─> Still confused
               └─> Clicks "❓ Help" button
                   └─> Sees both guides again
                       └─> Refreshes memory
                           └─> Clicks "✏ Edit" instead
                               └─> Adds islands correctly
                                   └─> SUCCESS! ✅
```

---

## 🎯 Key Takeaways for Users

### Visual Cues to Look For:

1. **🟨 Yellow Help Button** - Your friend when confused
2. **🔴 Red Warning** - Important! Read this
3. **🟠 Orange Tip Box** - Quick reminder about workflow
4. **Tooltips on Hover** - Extra context for buttons

### Mental Model:

```
PCB Format Config (Container)
    └─> Island 1 (Product A)
    └─> Island 2 (Product B)
    └─> Island 3 (Product C)
    └─> ... (more islands)

NOT:

PCB Format Config (Product A)
PCB Format Config (Product B)  ← WRONG!
PCB Format Config (Product C)  ← WRONG!
```

---

## 📱 Accessibility Notes

- **High Contrast:** Red warnings, orange tips stand out
- **Clear Typography:** Bold for important messages
- **Tooltips:** Provide context without cluttering UI
- **Consistent Icons:** ➕ Add, ✏ Edit, 🗑 Delete
- **Color + Text:** Not relying on color alone

---

## 🎓 Learning Path

```
Level 1: Startup Guides
    └─> User learns basics + PCB concept

Level 2: Visual Indicators
    └─> Red warnings + Orange tips reinforce learning

Level 3: Tooltips
    └─> Just-in-time help on hover

Level 4: Help Button
    └─> On-demand reference anytime

Result: Multi-layered learning with reinforcement ✅
```

---

**Visual Guide Version:** 1.0  
**Last Updated:** October 2025  
**Companion Documents:**  
- `HELP_GUIDE_FEATURES.md` - Feature documentation
- `PCB_HELP_IMPLEMENTATION_SUMMARY.md` - Technical details

---

*This visual guide shows exactly what users will see at each stage of their journey, making it clear how the help system guides them toward correct PCB configuration workflows.*

