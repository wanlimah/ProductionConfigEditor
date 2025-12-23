# 📦 Data Migration Guide - Old to New Forecast System

## Overview

This guide helps you move data from the **OLD forecast system** (period-based columns) to the **NEW system** (column-based design with historical tracking).

---

## 🔍 Understanding the Difference

### **OLD System (Code.gs):**
- Same columns reused for each period
- Data gets overwritten each period
- No historical tracking
- Multiple Forecast IDs per item (one per period)

**OLD Column Structure:**
```
A: Forecast ID (e.g., FY25-Q1M1-001, FY25-Q1M3-001)
B: Quarter
C: Year
D: No
E: Date
F: Epicentech Group
G: Location
H: Budgeted or Unbudgeted
I: Prism
J: Epicentech Category
K: Item (Simple)
L: Description
M: Forecast (M1)
N: Unbudgeted
O: Forecast (M3)
P: Forecast Amount
Q: User
R: Submitted
```

### **NEW System (Code_v2_ColumnBased.gs):**
- Individual columns for each period
- Historical data preserved
- Side-by-side comparison
- One Forecast ID per item (tracks across all periods)

**NEW Column Structure:**
```
A: Forecast ID (e.g., FY25-001)
B: Requestor
C: Category
D: Project Name
E: Item Name
F: Description
G: Remark
H-K: AOP 0.5 Q1-Q4
L-O: AOP 1 Q1-Q4
P-S: AOP Final Q1-Q4
T-AA: Q1M1, Q1M3, Q2M1, Q2M3, Q3M1, Q3M3, Q4M1, Q4M3
AB: Total FY
AC: Carry To
AD: Status
AE: PR #
AF: PO #
AG: Last Updated
AH: Updated By
AI: Change Log
```

---

## 🚀 Simple Migration Method

### **Option 1: Manual Copy-Paste (Recommended for Small Data)**

This is the **clearest and safest** method if you have < 100 items.

#### **Step 1: Prepare New Sheet**

1. **Create new Google Sheet** or rename old sheet tab:
   - Old sheet: Rename to `Forecast_OLD`
   - Create new tab: `Forecast`

2. **Setup new Forecast sheet structure:**
   - **Row 1:** Period deadlines (optional)
   - **Row 2:** Leave blank (for Total row later)
   - **Row 3:** Add headers:

```
A: Forecast ID
B: Requestor
C: Category
D: Project Name
E: Item Name
F: Description
G: Remark
H: Q1 (AOP 0.5)
I: Q2 (AOP 0.5)
J: Q3 (AOP 0.5)
K: Q4 (AOP 0.5)
L: Q1 (AOP 1)
M: Q2 (AOP 1)
N: Q3 (AOP 1)
O: Q4 (AOP 1)
P: Q1 (AOP Final)
Q: Q2 (AOP Final)
R: Q3 (AOP Final)
S: Q4 (AOP Final)
T: Q1M1
U: Q1M3
V: Q2M1
W: Q2M3
X: Q3M1
Y: Q3M3
Z: Q4M1
AA: Q4M3
AB: Total FY
AC: Carry To
AD: Status
AE: PR #
AF: PO #
AG: Last Updated
AH: Updated By
AI: Change Log
```

#### **Step 2: Map Your Data**

For each item in your OLD sheet, create **ONE ROW** in the new sheet:

**OLD Sheet Mapping → NEW Sheet:**

| OLD Column | OLD Name | → | NEW Column | NEW Name | Notes |
|------------|----------|---|------------|----------|-------|
| K | Item (Simple) | → | E | Item Name | Copy item name |
| J | Epicentech Category | → | C | Category | Copy category |
| L | Description | → | F | Description | Copy description |
| Q | User | → | B | Requestor | Copy user name |
| **Leave A blank** | | → | A | Forecast ID | Will auto-generate |

**For Forecast Amounts:**

If you have data from different periods in separate rows, **consolidate into ONE row**:

**Example:**

**OLD Sheet (Multiple Rows per Item):**
```
Row 5: FY25-Q1M1-001 | Q1 | FY25 | ... | Laptop | ... | 5000 | user@company.com | Q1M1
Row 15: FY25-Q1M3-001 | Q1 | FY25 | ... | Laptop | ... | 5200 | user@company.com | Q1M3
Row 25: FY25-Q2M1-002 | Q2 | FY25 | ... | Laptop | ... | 5500 | user@company.com | Q2M1
```

**NEW Sheet (ONE Row per Item):**
```
Row 4: [blank] | user@company.com | Category | Project | Laptop | Description | ... | [blank H-S] | 5000 | 5200 | 5500 | ...
       ↑ A      ↑ B                ↑ C        ↑ D       ↑ E      ↑ F            ↑ H-S AOP      ↑ T    ↑ U    ↑ V    
```

#### **Step 3: Copy Data Row by Row**

**For each UNIQUE ITEM in your old sheet:**

1. **Identify the item** (e.g., "Laptop")

2. **Find ALL rows** for that item across all periods

3. **Create ONE new row** in Row 4 (or next available):
   - **Column B:** Requestor name
   - **Column C:** Category
   - **Column D:** Project name (if available)
   - **Column E:** Item name
   - **Column F:** Description
   - **Column T:** Q1M1 amount (if you have Q1M1 data)
   - **Column U:** Q1M3 amount (if you have Q1M3 data)
   - **Column V:** Q2M1 amount (if you have Q2M1 data)
   - ...and so on

4. **Leave Column A blank** - Forecast ID will auto-generate

5. **Skip AOP columns (H-S)** unless you have AOP data

---

### **Option 2: Excel Formula Helper (For Larger Data)**

If you have **100+ items**, use this method:

#### **Step 1: Create Helper Sheet**

1. Create a new tab: `Migration_Helper`

2. **List unique items:**
   - Column A: Unique Item Names
   - Column B: Requestor
   - Column C: Category
   - Column D: Description

Use this formula to extract unique items from old sheet:
```
=UNIQUE(Forecast_OLD!K:K)
```

#### **Step 2: Use VLOOKUP/FILTER to Pull Period Data**

In your Migration_Helper sheet, use formulas to find period amounts:

**Column E - Q1M1 Amount:**
```
=IFERROR(FILTER(Forecast_OLD!P:P, 
                (Forecast_OLD!K:K=A2)*
                (Forecast_OLD!B:B="Q1")*
                (REGEXMATCH(Forecast_OLD!A:A,"M1"))), "")
```

**Column F - Q1M3 Amount:**
```
=IFERROR(FILTER(Forecast_OLD!P:P, 
                (Forecast_OLD!K:K=A2)*
                (Forecast_OLD!B:B="Q1")*
                (REGEXMATCH(Forecast_OLD!A:A,"M3"))), "")
```

Repeat for Q2M1, Q2M3, Q3M1, Q3M3, Q4M1, Q4M3...

#### **Step 3: Copy to New Forecast Sheet**

1. Select all data from Migration_Helper
2. Copy → Paste Values into new Forecast sheet (starting Row 4)
3. Delete Migration_Helper tab

---

## 📋 Quick Migration Checklist

### **Before Migration:**

- [ ] Backup your current Google Sheet (File → Make a copy)
- [ ] Rename old Forecast sheet to `Forecast_OLD`
- [ ] Create new `Forecast` sheet
- [ ] Add Row 3 headers (35 columns A-AI)
- [ ] Create `Note` sheet with Requestors and Categories

### **During Migration:**

- [ ] Identify unique items (not periods)
- [ ] For each item, create ONE row in new sheet
- [ ] Map old columns to new columns
- [ ] Consolidate period data (Q1M1, Q1M3, etc.) into respective columns
- [ ] Leave Column A (Forecast ID) blank
- [ ] Copy Requestor, Category, Item Name, Description

### **After Migration:**

- [ ] Install new script (Code_v2_ColumnBased.gs)
- [ ] Run: Initialize Config
- [ ] Run: Setup Total Row
- [ ] Run: Setup Data Validation
- [ ] Run: Generate Forecast IDs (will fill Column A)
- [ ] Verify data looks correct
- [ ] Delete old sheet once confirmed

---

## 💡 Practical Example

Let's migrate **one complete item** as an example:

### **OLD System Data (3 separate rows):**

```
Row 10: FY25-Q1M1-015 | Q1 | FY25 | ... | Wong, Chor Ming | ... | 4A) Computer/Software | Software License | ... | 10000 | wong@company.com | Q1M1
Row 25: FY25-Q1M3-015 | Q1 | FY25 | ... | Wong, Chor Ming | ... | 4A) Computer/Software | Software License | ... | 10500 | wong@company.com | Q1M3
Row 40: FY25-Q2M1-018 | Q2 | FY25 | ... | Wong, Chor Ming | ... | 4A) Computer/Software | Software License | ... | 11000 | wong@company.com | Q2M1
```

### **NEW System Data (1 consolidated row):**

```
Row 4:
  A: [blank - will auto-generate as FY25-001]
  B: Wong, Chor Ming
  C: 4A) Computer/Software
  D: [blank or "Software Project"]
  E: Software License
  F: Annual license renewal
  G: [blank or remarks]
  H-S: [blank - no AOP data]
  T: 10000  (Q1M1)
  U: 10500  (Q1M3)
  V: 11000  (Q2M1)
  W-AA: [blank - future periods]
  AB: =SUM(T4:AA4)  (formula for Total FY)
  AC-AI: [blank - tracking columns]
```

**Result:** Instead of 3 rows, you have **1 row** showing the item's forecast evolution!

---

## ⚠️ Important Migration Notes

### **1. Consolidate by Item, NOT by Period**

❌ **WRONG:** Create new row for each period
```
Row 4: Item A | Q1M1 data | [rest blank]
Row 5: Item A | Q1M3 data | [rest blank]
Row 6: Item A | Q2M1 data | [rest blank]
```

✅ **CORRECT:** One row with all periods
```
Row 4: Item A | Q1M1 data | Q1M3 data | Q2M1 data | ...
```

### **2. Forecast ID Changes**

**OLD Format:** `FY25-Q1M1-001`, `FY25-Q1M3-001`, `FY25-Q2M1-001`
**NEW Format:** `FY25-001` (same ID across all periods)

You'll lose the old IDs, but the new system auto-generates better IDs.

### **3. Missing Data is OK**

If you don't have data for certain periods (e.g., AOP 0.5), just leave those columns blank.

### **4. PR Tracking**

If you have PR data in old system, you'll need to update PR references to new Forecast IDs after migration.

---

## 🔧 Migration Script (Advanced)

If you're comfortable with scripting, here's a helper function to automate migration:

```javascript
function migrateOldToNew() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const oldSheet = ss.getSheetByName('Forecast_OLD');
  const newSheet = ss.getSheetByName('Forecast');
  
  const oldData = oldSheet.getDataRange().getValues();
  const uniqueItems = {};
  
  // Group by item name
  oldData.slice(1).forEach(row => {
    const itemName = row[10]; // Column K - Item
    const period = row[1];     // Column B - Quarter + check Forecast ID for M1/M3
    const amount = row[15];    // Column P - Forecast Amount
    const requestor = row[16]; // Column Q - User
    const category = row[9];   // Column J - Category
    const description = row[11]; // Column L - Description
    
    if (!uniqueItems[itemName]) {
      uniqueItems[itemName] = {
        requestor: requestor,
        category: category,
        itemName: itemName,
        description: description,
        periods: {}
      };
    }
    
    // Determine period column (T, U, V, etc.)
    const forecastId = row[0]; // Column A - Forecast ID
    if (forecastId.includes('Q1M1')) uniqueItems[itemName].periods['Q1M1'] = amount;
    else if (forecastId.includes('Q1M3')) uniqueItems[itemName].periods['Q1M3'] = amount;
    else if (forecastId.includes('Q2M1')) uniqueItems[itemName].periods['Q2M1'] = amount;
    // ... add more periods
  });
  
  // Write to new sheet
  let newRow = 4; // Start from Row 4
  Object.values(uniqueItems).forEach(item => {
    newSheet.getRange(newRow, 2).setValue(item.requestor);      // B
    newSheet.getRange(newRow, 3).setValue(item.category);       // C
    newSheet.getRange(newRow, 5).setValue(item.itemName);       // E
    newSheet.getRange(newRow, 6).setValue(item.description);    // F
    newSheet.getRange(newRow, 20).setValue(item.periods['Q1M1'] || ''); // T
    newSheet.getRange(newRow, 21).setValue(item.periods['Q1M3'] || ''); // U
    newSheet.getRange(newRow, 22).setValue(item.periods['Q2M1'] || ''); // V
    // ... add more periods
    newRow++;
  });
  
  SpreadsheetApp.getUi().alert('Migration complete!');
}
```

**To use:**
1. Copy this code to Apps Script
2. Adjust column numbers to match your OLD sheet
3. Run `migrateOldToNew()`
4. Verify results
5. Run `generateForecastIDs_v2()` to create new IDs

---

## ✅ Post-Migration Verification

After migration, check:

1. **Count items:**
   - OLD sheet: Count unique items (not periods)
   - NEW sheet: Count rows (starting Row 4)
   - Should match!

2. **Spot check amounts:**
   - Pick 3-5 items randomly
   - Verify period amounts match old sheet

3. **Test auto-generation:**
   - Add a new test row
   - Verify Forecast ID auto-generates

4. **Test total row:**
   - Verify Row 2 totals match expected sums

---

## 🆘 Need Help?

**If you're stuck:**

1. Share a small sample of your OLD data structure (5-10 rows)
2. I can create a custom migration script for your specific layout
3. Or we can walk through manual migration together

**Common Issues:**

- **"Too much data"** → Use Excel Helper method or script
- **"Different column layout"** → Map your columns to new structure first
- **"Missing periods"** → That's OK, leave those columns blank
- **"Lost old Forecast IDs"** → Export old sheet as backup, new system generates better IDs

---

**Ready to migrate? Start with 5-10 items manually to get comfortable, then scale up!** 🚀












