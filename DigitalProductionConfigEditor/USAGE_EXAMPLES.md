# Usage Examples - Real XML Scenarios

## Example 1: Simple Package (2 Attributes)

### XML Configuration
```xml
<DC_PDM_TRACE_ENABLE>
  <Package name="SUSER" enable="FALSE"/>
  <Package name="8267-PROD" enable="FALSE"/>
</DC_PDM_TRACE_ENABLE>
```

### What User Sees in Editor

**Step 1: Select Package**
- Dropdown shows: `DC_PDM_TRACE_ENABLE > SUSER` and `DC_PDM_TRACE_ENABLE > 8267-PROD`

**Step 2: Edit Attributes (after selecting first package)**
```
┌─────────────────────────────────────────────────┐
│ Step 2: Edit XML Attributes                     │
│                                                  │
│ Editing Node: DC_PDM_TRACE_ENABLE               │
│ Dynamically generated fields based on XML...    │
│                                                  │
│ name:         [SUSER_____________________] ✏️    │
│ enable:       [FALSE ▼] (TRUE/FALSE)      📋    │
│                                                  │
│ ┌──────────────────────────────────────────┐    │
│ │ 💡 Instructions:                          │    │
│ │ • Only attributes that exist are shown    │    │
│ │ • Dropdowns for predefined options        │    │
│ │ • Text fields for free-form values        │    │
│ └──────────────────────────────────────────┘    │
└─────────────────────────────────────────────────┘
```

**Step 3: Review**
```
┌───────────────────────────────────────────────┐
│ Configuration Node: DC_PDM_TRACE_ENABLE       │
│                                                │
│ ┌──────────────────────────────────────────┐  │
│ │ Package Attributes Summary:              │  │
│ │                                          │  │
│ │ name:       SUSER                        │  │
│ │ enable:     FALSE                        │  │
│ └──────────────────────────────────────────┘  │
└───────────────────────────────────────────────┘
```

**Result:** Shows exactly **2 attributes** ✅

---

## Example 2: Package with 4 Attributes

### XML Configuration
```xml
<TTD_FILE_ENABLE>
  <Package name="SUSER" enable="FALSE" count="3" sampling="5" />
  <!-- Setting Instructions:		
      1. 'count' represents the number of items generated.
      2. 'sampling' represents the sampling interval;
  -->
</TTD_FILE_ENABLE>
```

### What User Sees in Editor

**Step 2: Edit Attributes**
```
┌─────────────────────────────────────────────────┐
│ Step 2: Edit XML Attributes                     │
│                                                  │
│ Editing Node: TTD_FILE_ENABLE                   │
│ Dynamically generated fields based on XML...    │
│                                                  │
│ name:         [SUSER_____________________] ✏️    │
│ enable:       [FALSE ▼] (TRUE/FALSE)      📋    │
│ count:        [3_________________________] ✏️    │
│ sampling:     [5_________________________] ✏️    │
│                                                  │
│ ┌──────────────────────────────────────────┐    │
│ │ 💡 Instructions:                          │    │
│ │ • Only attributes that exist are shown    │    │
│ └──────────────────────────────────────────┘    │
└─────────────────────────────────────────────────┘
```

**Step 3: Review**
```
┌───────────────────────────────────────────────┐
│ Configuration Node: TTD_FILE_ENABLE           │
│                                                │
│ ┌──────────────────────────────────────────┐  │
│ │ Package Attributes Summary:              │  │
│ │                                          │  │
│ │ name:       SUSER                        │  │
│ │ enable:     FALSE                        │  │
│ │ count:      3                            │  │
│ │ sampling:   5                            │  │
│ └──────────────────────────────────────────┘  │
└───────────────────────────────────────────────┘
```

**Result:** Shows exactly **4 attributes** ✅

---

## Example 3: Complex Package with Options

### XML Configuration
```xml
<INLINE_SORTER_ENABLE>
  <RuleOptions> DATETIME | REV </RuleOptions>
  <Package name="SUSER" enable="FALSE" rule="REV" />
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" rule="REV" />
</INLINE_SORTER_ENABLE>
```

### What User Sees in Editor

**Step 2: Edit Attributes**
```
┌─────────────────────────────────────────────────┐
│ Step 2: Edit XML Attributes                     │
│                                                  │
│ Editing Node: INLINE_SORTER_ENABLE              │
│ Dynamically generated fields based on XML...    │
│                                                  │
│ name:         [SUSER_____________________] ✏️    │
│ enable:       [FALSE ▼] (TRUE/FALSE)      📋    │
│ rule:         [REV ▼] (DATETIME/REV)      📋    │
│                                                  │
│ ┌──────────────────────────────────────────┐    │
│ │ 💡 Instructions:                          │    │
│ │ • Attributes with predefined options      │    │
│ │   show as dropdowns (enable, rule)        │    │
│ └──────────────────────────────────────────┘    │
└─────────────────────────────────────────────────┘
```

**Result:** Shows **3 attributes**, with 2 dropdowns (enable, rule) and 1 text field (name) ✅

---

## Example 4: Editing Production Package Names

### Scenario
You want to change the package name from "SUSER" to "8267-PROD" for production use.

### Steps

1. **Select the package** from dropdown at top of window
   
2. **Navigate to Step 2** (or it loads automatically)

3. **Edit the `name` field:**
   ```
   name: [SUSER_____________________]
   ```
   Change to:
   ```
   name: [8267-PROD_________________]
   ```

4. **Optionally change other fields:**
   ```
   enable: [TRUE ▼]  (change from FALSE to TRUE)
   ```

5. **Navigate to Step 3** to review changes:
   ```
   ┌──────────────────────────────────────────┐
   │ Package Attributes Summary:              │
   │                                          │
   │ name:       8267-PROD                    │
   │ enable:     TRUE                         │
   └──────────────────────────────────────────┘
   ```

6. **Click "Save XML"** button at the bottom

7. **Result:** XML file updated:
   ```xml
   <DC_PDM_TRACE_ENABLE>
     <Package name="8267-PROD" enable="TRUE"/>
   </DC_PDM_TRACE_ENABLE>
   ```

---

## Real-World Usage Workflow

### Workflow 1: Enable a Feature for Production

**Goal:** Enable `DC_PDM_TRACE_ENABLE` for production package "8267-PROD"

1. Launch the application
2. Select package: `DC_PDM_TRACE_ENABLE > 8267-PROD`
3. In Step 2, change: `enable: FALSE` → `enable: TRUE`
4. Click "Save XML"
5. Done! ✅

### Workflow 2: Configure Sampling Settings

**Goal:** Change sampling settings for `TTD_FILE_ENABLE`

1. Launch the application
2. Select package: `TTD_FILE_ENABLE > SUSER`
3. In Step 2, you see:
   - `name: SUSER`
   - `enable: FALSE`
   - `count: 3`
   - `sampling: 5`
4. Change values:
   - `count: 3` → `count: 10`
   - `sampling: 5` → `sampling: 2`
5. Navigate to Step 3 to review
6. Click "Save XML"
7. Done! ✅

### Workflow 3: Add New Production Package

**Goal:** Change "SUSER" to a production package name

1. Select package: `DC_PDM_TRACE_ENABLE > SUSER`
2. Change `name: SUSER` → `name: MY-PROD-PKG-001`
3. Change `enable: FALSE` → `enable: TRUE`
4. Click "Save XML"
5. Result:
   ```xml
   <DC_PDM_TRACE_ENABLE>
     <Package name="MY-PROD-PKG-001" enable="TRUE"/>
   </DC_PDM_TRACE_ENABLE>
   ```

---

## Comparison Table

| Package Configuration | Attributes Shown | Dropdowns | Text Fields |
|----------------------|------------------|-----------|-------------|
| `<Package name="SUSER" enable="FALSE"/>` | 2 | enable | name |
| `<Package name="SUSER" enable="FALSE" count="3"/>` | 3 | enable | name, count |
| `<Package name="SUSER" enable="FALSE" count="3" sampling="5"/>` | 4 | enable | name, count, sampling |
| `<Package name="SUSER" enable="FALSE" rule="REV"/>` | 3 | enable, rule | name |
| `<Package name="SUSER" enable="FALSE" mode="AUTO"/>` | 3 | enable, mode | name |

---

## Key Features Demonstrated

✅ **Dynamic Field Generation**
- Editor shows 2, 3, 4, or more fields depending on XML
- No empty fields
- No disabled fields

✅ **Smart Input Controls**
- Dropdowns for: enable, mode, rule, avg_channel
- Text fields for: name, count, sampling, threshold, url, note

✅ **XML Options Detection**
- Reads `<RuleOptions>`, `<ModeOptions>` from XML
- Automatically populates dropdowns

✅ **Clean Interface**
- Only relevant fields shown
- Clear labels
- Helpful instructions

✅ **Easy Editing**
- Select package → Edit values → Save
- 3-step wizard guides you through the process

---

## Tips for Users

💡 **Tip 1:** Use the package selector at the top to quickly switch between packages

💡 **Tip 2:** Step 3 shows a summary - always review before saving

💡 **Tip 3:** The "Reload XML" button refreshes if you've edited the XML file manually

💡 **Tip 4:** Dropdown fields show all available options from the XML

💡 **Tip 5:** Text fields accept any value - be careful with numeric fields (count, sampling, threshold)

---

## Summary

The dynamic editor **automatically adapts** to show exactly the right number of attributes for each package:

- **2 attributes** (name, enable) → Shows 2 fields
- **3 attributes** (name, enable, count) → Shows 3 fields
- **4 attributes** (name, enable, count, sampling) → Shows 4 fields
- **And so on...**

No configuration needed - it just works! ✨































































