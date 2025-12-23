# Visual Workflow Guide

## Application Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                Digital Production Config Editor                  │
│                         (Version 2.0)                            │
└─────────────────────────────────────────────────────────────────┘
                                │
                ┌───────────────┴───────────────┐
                │                               │
        ┌───────▼────────┐            ┌────────▼──────┐
        │  Master XML    │            │   New XML     │
        │   (Template)   │            │ (Your Config) │
        │   READ ONLY    │            │   EDITABLE    │
        └───────┬────────┘            └────────┬──────┘
                │                               │
                │     ┌─────────────────────┐   │
                └────►│  Configuration     │◄───┘
                      │     Builder        │
                      └─────────────────────┘
```

---

## Step-by-Step Workflow

### Step 1: Build Configuration Structure

```
┌──────────────────────────────────────────────────────────────────────┐
│                         STEP 1: BUILD STRUCTURE                       │
├─────────────────────────────┬────────────────────────────────────────┤
│  📋 MASTER TEMPLATE         │  📝 YOUR NEW CONFIGURATION             │
│  (Read-Only)                │  (Editable)                            │
├─────────────────────────────┼────────────────────────────────────────┤
│                             │                                        │
│  ┌──────────────────────┐   │   ┌─────────────────────────┐         │
│  │ GU_ENGINEERING_...   │   │   │                         │         │
│  │ (5 items)  [➕ Add]  │───┼──►│  (Empty - Add configs)  │         │
│  └──────────────────────┘   │   │                         │         │
│                             │   │                         │         │
│  ┌──────────────────────┐   │   │                         │         │
│  │ PARAM_COUNT_CHECK    │   │   │                         │         │
│  │ (12 items) [➕ Add]  │───┼──►│                         │         │
│  └──────────────────────┘   │   │                         │         │
│                             │   └─────────────────────────┘         │
│  ┌──────────────────────┐   │                                        │
│  │ ENA_AVERAGE_MODE_ON  │   │   After adding:                        │
│  │ (8 items)  [➕ Add]  │   │   ┌─────────────────────────┐         │
│  └──────────────────────┘   │   │ GU_ENGINEERING_...      │         │
│                             │   │ (5 items)    [🗑 Delete] │         │
│  ┌──────────────────────┐   │   └─────────────────────────┘         │
│  │ STOP_TEST_ON_...     │   │   ┌─────────────────────────┐         │
│  │ (3 items)  [➕ Add]  │   │   │ PARAM_COUNT_CHECK       │         │
│  └──────────────────────┘   │   │ (12 items)   [🗑 Delete] │         │
│                             │   └─────────────────────────┘         │
│       ... more configs      │                                        │
└─────────────────────────────┴────────────────────────────────────────┘
                                │
                                ▼
                          [Next ➡] (Requires ≥1 config)
```

---

### Step 2: Edit Packages

```
┌──────────────────────────────────────────────────────────────────────┐
│                      STEP 2: EDIT PACKAGES                            │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Select Configuration: [GU_ENGINEERING_MODE_ENABLE        ▼]         │
│                                                                       │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │ Package List:                                    [➕ Add New]  │  │
│  ├────────────────────────────────────────────────────────────────┤  │
│  │  ┌─────────────────────────────────────────────────────────┐   │  │
│  │  │ 📦 Package: SUSER                                       │   │  │
│  │  │    enable = "TRUE"                    [Edit] [Delete]   │   │  │
│  │  └─────────────────────────────────────────────────────────┘   │  │
│  │                                                                 │  │
│  │  ┌─────────────────────────────────────────────────────────┐   │  │
│  │  │ 📦 Package: WW-PROD                                     │   │  │
│  │  │    enable = "TRUE"                    [Edit] [Delete]   │   │  │
│  │  └─────────────────────────────────────────────────────────┘   │  │
│  │                                                                 │  │
│  │  ┌─────────────────────────────────────────────────────────┐   │  │
│  │  │ 📦 Package: 8266-PROD                                   │   │  │
│  │  │    enable = "FALSE"                   [Edit] [Delete]   │   │  │
│  │  └─────────────────────────────────────────────────────────┘   │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                                                                       │
│  [⬅ Back]                                                  [Next ➡]  │
└──────────────────────────────────────────────────────────────────────┘
```

---

### Step 3: Review and Save

```
┌──────────────────────────────────────────────────────────────────────┐
│                    STEP 3: REVIEW AND SAVE                            │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Configuration Summary:                                               │
│                                                                       │
│  ✅ GU_ENGINEERING_MODE_ENABLE                                        │
│      └─ 5 packages configured                                         │
│                                                                       │
│  ✅ PARAM_COUNT_CHECK                                                 │
│      └─ 12 packages configured                                        │
│                                                                       │
│  Total: 2 configurations, 17 packages                                 │
│                                                                       │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │ XML Preview:                                                   │  │
│  ├────────────────────────────────────────────────────────────────┤  │
│  │ <?xml version="1.0" encoding="utf-8"?>                        │  │
│  │ <ProductionUserConfig>                                         │  │
│  │   <ProductionUserConfigs viewer="false">                       │  │
│  │     <GU_ENGINEERING_MODE_ENABLE>                               │  │
│  │       <Package name="SUSER" enable="TRUE" />                   │  │
│  │       <Package name="WW-PROD" enable="TRUE" />                 │  │
│  │       ...                                                       │  │
│  │     </GU_ENGINEERING_MODE_ENABLE>                              │  │
│  │     <PARAM_COUNT_CHECK>                                        │  │
│  │       ...                                                       │  │
│  │   </ProductionUserConfigs>                                     │  │
│  │ </ProductionUserConfig>                                        │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                                                                       │
│  [⬅ Back]                                            [💾 Save]        │
└──────────────────────────────────────────────────────────────────────┘
```

---

## File Operations Flow

### Create New Configuration

```
┌─────────────┐
│ [📄 New]    │
└──────┬──────┘
       │
       ▼
┌─────────────────────────────────┐
│ Confirm: Create new blank XML?  │
│ (Unsaved changes will be lost)  │
└───────┬─────────────────────────┘
        │ [Yes]
        ▼
┌────────────────────────┐
│ Blank XML Created      │
│ <ProductionUserConfig> │
│   <ProductionUserCon...│
│   </ProductionUserCon..│
│ </ProductionUserConfig>│
└────────────────────────┘
        │
        ▼
┌────────────────────────┐
│ Add configs from       │
│ Master Template        │
└────────────────────────┘
```

---

### Open Existing Configuration

```
┌─────────────┐
│ [📂 Open]   │
└──────┬──────┘
       │
       ▼
┌────────────────────────┐
│  File Dialog           │
│  Select XML file...    │
└───────┬────────────────┘
        │
        ▼
┌────────────────────────┐
│ XML Loaded             │
│ Ready to edit          │
└────────────────────────┘
```

---

### Save Configuration

```
┌─────────────┐         ┌──────────────────┐
│ [💾 Save]   │────────►│ Has file path?   │
└─────────────┘         └────┬─────────┬───┘
                             │ Yes     │ No
                             ▼         ▼
                    ┌────────────┐  ┌──────────────┐
                    │ Save to    │  │ File Dialog  │
                    │ existing   │  │ Choose name  │
                    │ file       │  │ and location │
                    └────────────┘  └──────┬───────┘
                             │              │
                             └──────┬───────┘
                                    ▼
                          ┌──────────────────┐
                          │ XML Saved        │
                          │ ✅ Success!       │
                          └──────────────────┘
```

---

### Save As New File

```
┌─────────────────┐
│ [💾 Save As...] │
└────────┬────────┘
         │
         ▼
┌──────────────────┐
│ File Dialog      │
│ Choose new name  │
│ and location     │
└────────┬─────────┘
         │
         ▼
┌──────────────────┐
│ XML Saved        │
│ ✅ Success!       │
└──────────────────┘
```

---

## Data Flow Diagram

```
                    ┌──────────────────┐
                    │  Application     │
                    │  Startup         │
                    └────────┬─────────┘
                             │
                ┌────────────┴────────────┐
                │                         │
                ▼                         ▼
    ┌──────────────────┐      ┌─────────────────┐
    │ Load Master XML  │      │ Create Blank    │
    │ (Template)       │      │ New XML         │
    └────────┬─────────┘      └────────┬────────┘
             │                         │
             ▼                         ▼
    ┌──────────────────┐      ┌─────────────────┐
    │ MasterXmlDocument│      │ NewXmlDocument  │
    └────────┬─────────┘      └────────┬────────┘
             │                         │
             │        STEP 1           │
             │     ┌──────────┐        │
             └────►│  Copy    │◄───────┘
                   │  Config  │
                   └─────┬────┘
                         │
                         ▼
                   ┌──────────┐
                   │ STEP 2   │
                   │ Edit     │
                   │ Packages │
                   └─────┬────┘
                         │
                         ▼
                   ┌──────────┐
                   │ STEP 3   │
                   │ Review   │
                   └─────┬────┘
                         │
                         ▼
                   ┌──────────┐
                   │  Save    │
                   │  XML     │
                   └──────────┘
```

---

## User Actions Flow

```
User Launches App
       │
       ▼
┌─────────────────────────────────────┐
│ OPTION 1: Start Fresh               │
│ [📄 New] → Blank XML → Add configs   │
└─────────────────────────────────────┘
       │
       ▼
┌─────────────────────────────────────┐
│ OPTION 2: Load Existing             │
│ [📂 Open] → Select file → Edit       │
└─────────────────────────────────────┘
       │
       ▼
┌─────────────────────────────────────┐
│ STEP 1: Add/Delete Configurations   │
│ • Add from Master Template           │
│ • Delete unwanted ones               │
└─────────────────────────────────────┘
       │
       ▼ [Next ➡]
┌─────────────────────────────────────┐
│ STEP 2: Manage Packages              │
│ • Select configuration               │
│ • Add/Edit/Delete packages           │
│ • Modify attributes                  │
└─────────────────────────────────────┘
       │
       ▼ [Next ➡]
┌─────────────────────────────────────┐
│ STEP 3: Review & Save                │
│ • Review summary                     │
│ • Preview XML                        │
│ • Save to file                       │
└─────────────────────────────────────┘
       │
       ▼ [💾 Save]
┌─────────────────────────────────────┐
│ Custom XML File Created!             │
│ ✅ Ready to use                       │
└─────────────────────────────────────┘
```

---

## Color-Coded UI Reference

```
┌────────────────────────────────────────────────┐
│ Button Toolbar                                  │
├────────────────────────────────────────────────┤
│                                                 │
│  [📄 New]      [📂 Open]     [🔄 Reload Master] │
│  Light Cyan    Light Yellow   Light Blue        │
│  (Create)      (Load)         (Refresh)         │
│                                                 │
│  [⬅ Back]      [Next ➡]      [💾 Save]          │
│  Default       Default        Light Green       │
│  (Navigate)    (Navigate)     (Save Current)    │
│                                                 │
│                              [💾 Save As...]    │
│                               Pale Green        │
│                               (Save Copy)       │
└────────────────────────────────────────────────┘

┌────────────────────────────────────────────────┐
│ Step 1: Configuration Selection                 │
├───────────────────┬────────────────────────────┤
│ Master Template   │ Your New Configuration     │
│ Purple/Lavender   │ Green/Honeydew             │
│ (Read-Only)       │ (Editable)                 │
│                   │                            │
│ [➕ Add]          │ [🗑 Delete]                 │
│ Light Green       │ Light Coral                │
│ (Copy to new)     │ (Remove)                   │
└───────────────────┴────────────────────────────┘
```

---

## Quick Action Guide

```
╔════════════════════════════════════════════════════════╗
║              WHAT DO YOU WANT TO DO?                    ║
╠════════════════════════════════════════════════════════╣
║                                                         ║
║  ① Create a new configuration from scratch              ║
║     → Click [📄 New]                                    ║
║     → Add configs from Master Template                  ║
║                                                         ║
║  ② Edit an existing configuration file                  ║
║     → Click [📂 Open]                                   ║
║     → Select your XML file                              ║
║                                                         ║
║  ③ Add a configuration to your XML                      ║
║     → Step 1 → Find config in Master (left)             ║
║     → Click [➕ Add]                                     ║
║                                                         ║
║  ④ Remove a configuration from your XML                 ║
║     → Step 1 → Find config in Your Config (right)       ║
║     → Click [🗑 Delete]                                  ║
║                                                         ║
║  ⑤ Add a package to a configuration                     ║
║     → Step 2 → Select configuration                     ║
║     → Click [➕ Add New Package]                         ║
║                                                         ║
║  ⑥ Edit a package's attributes                          ║
║     → Step 2 → Select configuration                     ║
║     → Click [Edit] next to package                      ║
║                                                         ║
║  ⑦ Delete a package                                     ║
║     → Step 2 → Select configuration                     ║
║     → Click [Delete] next to package                    ║
║                                                         ║
║  ⑧ Save your work                                       ║
║     → Click [💾 Save] or [💾 Save As...]                 ║
║                                                         ║
╚════════════════════════════════════════════════════════╝
```

---

## Navigation Map

```
        ┌──────────────────────────────────────┐
        │         Application Window           │
        │                                      │
        │  [File Operations Toolbar]           │
        │  [📄] [📂] [🔄] [⬅] [➡] [💾] [💾]    │
        │                                      │
        │  ┌────────────────────────────────┐  │
        │  │   Current Step Content         │  │
        │  │                                │  │
        │  │  Step 1: Build Structure       │  │
        │  │  Step 2: Edit Packages         │  │
        │  │  Step 3: Review & Save         │  │
        │  │                                │  │
        │  └────────────────────────────────┘  │
        │                                      │
        │  Status: "Added 2 configurations..." │
        │                                      │
        └──────────────────────────────────────┘
```

---

## Summary Flow Chart

```
START
  │
  ├─→ [New Blank XML] ────────┐
  │                            │
  └─→ [Open Existing XML] ────┤
                               │
                               ▼
                    ┌──────────────────┐
                    │   STEP 1:        │
                    │   Select Configs │
                    │   ➕ Add / 🗑 Del │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │   STEP 2:        │
                    │   Edit Packages  │
                    │   ➕ ✏️ 🗑        │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │   STEP 3:        │
                    │   Review & Save  │
                    │   💾              │
                    └────────┬─────────┘
                             │
                             ▼
                       ┌──────────┐
                       │ Custom   │
                       │ XML File │
                       │ ✅ Done! │
                       └──────────┘
```

---

**End of Visual Workflow Guide**

























































