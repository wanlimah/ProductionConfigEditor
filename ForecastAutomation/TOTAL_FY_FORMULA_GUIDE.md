# 💰 Total FY Formula - How It's Calculated

## Overview

The **Total FY (Column AB)** is tricky because users might enter data in different places. This guide explains how the system handles it.

---

## 🎯 **Current Implementation**

### **Formula Used:**
```excel
=SUM(T{row}:AA{row})
```

This sums **only the Quarterly forecast columns** (Q1M1 through Q4M3).

### **What It Includes:**
- ✅ T: Q1M1
- ✅ U: Q1M3
- ✅ V: Q2M1
- ✅ W: Q2M3
- ✅ X: Q3M1
- ✅ Y: Q3M3
- ✅ Z: Q4M1
- ✅ AA: Q4M3

### **What It EXCLUDES:**
- ❌ H-K: AOP 0.5 (Q1-Q4)
- ❌ L-O: AOP 1 (Q1-Q4)
- ❌ P-S: AOP Final (Q1-Q4)

---

## 🤔 **Why Only Quarterly Forecasts?**

### **Reason:**
AOP (Annual Operating Plan) columns are typically:
- **Planning/budgeting** estimates
- **Not actual forecasts** by period
- **Overwritten** during the year (based on your old system)

The **Quarterly columns (T-AA)** are:
- **Actual period-by-period forecasts**
- What you compare against PRs
- What you track for actuals
- The "real" forecast amounts

### **Example Scenario:**

```
Item: New Equipment

AOP Final Q1-Q4: 70,000 | blank | blank | blank  (only Q1 budgeted)
Q1M1-Q4M3:       70,000 | 70,000 | blank | blank | 14,000 | blank | blank | blank

With current formula (Quarterly only):
Total FY = 70,000 + 70,000 + 14,000 = 154,000 ✓ (Correct - actual forecasts)

If we included AOP:
Total FY = 70,000 (AOP) + 70,000 + 70,000 + 14,000 = 224,000 ✗ (Wrong - double counting!)
```

---

## 📊 **Alternative Formula Options**

If your use case is different, here are alternatives:

### **Option 1: Current (Quarterly Only) - DEFAULT**
```excel
=SUM(T{row}:AA{row})
```
**Use when:**
- ✅ Quarterly forecasts are the "real" amounts
- ✅ AOP is just initial planning
- ✅ Want to avoid double-counting

### **Option 2: Quarterly OR AOP Final (Whichever Has Data)**
```excel
=IF(SUM(T{row}:AA{row})>0, SUM(T{row}:AA{row}), SUM(P{row}:S{row}))
```
**Use when:**
- ✅ Some items only have AOP Final
- ✅ Some items only have Quarterly
- ✅ Never both

**Example:**
```
Item A: AOP Final = 10,000 | Quarterly = blank
→ Total FY = 10,000 (from AOP)

Item B: AOP Final = blank | Quarterly = 12,000
→ Total FY = 12,000 (from Quarterly)
```

### **Option 3: Latest Non-Empty (Most Recent Forecast)**
```excel
=IF(SUM(T{row}:AA{row})>0, SUM(T{row}:AA{row}), IF(SUM(P{row}:S{row})>0, SUM(P{row}:S{row}), IF(SUM(L{row}:O{row})>0, SUM(L{row}:O{row}), SUM(H{row}:K{row}))))
```
**Use when:**
- ✅ Want the most recent forecast available
- ✅ Falls back: Quarterly → AOP Final → AOP 1 → AOP 0.5

### **Option 4: All AOP + Quarterly (Maximum)**
```excel
=SUM(H{row}:AA{row})
```
⚠️ **Warning:** This will likely **double-count** if you have both AOP and Quarterly data!

**Only use if:**
- Different items use different columns
- Never have overlapping data

### **Option 5: Custom Business Logic**

If you have specific rules, we can create custom logic:

```javascript
// In the setupTotalFYFormulas function, replace with:
function setupTotalFYFormulas(sheet, startRow, endRow) {
  for (let row = startRow; row <= endRow; row++) {
    const totalFYCell = sheet.getRange(row, CONFIG_V2.COLS.TOTAL_FY);
    
    // Check what data exists
    const quarterlySum = sheet.getRange(row, 20, 1, 8).getValues()[0]; // T-AA
    const aopFinalSum = sheet.getRange(row, 16, 1, 4).getValues()[0];  // P-S
    
    const hasQuarterly = quarterlySum.some(val => val > 0);
    const hasAOP = aopFinalSum.some(val => val > 0);
    
    let formula;
    if (hasQuarterly) {
      // Use Quarterly if available
      formula = `=SUM(T${row}:AA${row})`;
    } else if (hasAOP) {
      // Fall back to AOP Final
      formula = `=SUM(P${row}:S${row})`;
    } else {
      // Default to Quarterly
      formula = `=SUM(T${row}:AA${row})`;
    }
    
    totalFYCell.setFormula(formula);
  }
}
```

---

## 🔧 **How to Change the Formula**

### **Method 1: Update Code (Affects All Future Rows)**

1. **Open the code:**
   ```
   Extensions → Apps Script
   ```

2. **Find the function `setupTotalFYFormulas`** (around line 449)

3. **Replace this line:**
   ```javascript
   const formula = `=SUM(T${row}:AA${row})`;
   ```

4. **With your preferred option** (see above)

5. **Save** (Ctrl+S)

6. **Run Visual Formatting** to apply to existing rows

### **Method 2: Manual Formula (Individual Rows)**

1. **Click on cell AB4** (or any Total FY cell)

2. **Enter your formula:**
   ```
   =SUM(T4:AA4)
   ```
   Or any option from above

3. **Copy formula down** to other rows

### **Method 3: Google Sheets Built-in (All Rows at Once)**

1. **Select entire column AB** (from Row 4 down)

2. **Enter formula in first cell:**
   ```
   =SUM(T4:AA4)
   ```

3. **Press Ctrl+D** to fill down

---

## 📋 **Decision Guide**

### **Choose Option 1 (Quarterly Only) if:**
- ✅ You always enter Quarterly forecasts (Q1M1-Q4M3)
- ✅ AOP is just planning (not used for actuals)
- ✅ You want to avoid double-counting
- ✅ **This matches your old system behavior**

### **Choose Option 2 (Quarterly OR AOP) if:**
- ✅ Some items ONLY have AOP Final
- ✅ Some items ONLY have Quarterly
- ✅ Never both at the same time
- ✅ Need flexibility for different item types

### **Choose Option 3 (Latest Non-Empty) if:**
- ✅ You want the most recent/updated forecast
- ✅ Forecasts evolve: AOP 0.5 → AOP 1 → AOP Final → Quarterly
- ✅ Each round is more accurate than the previous

---

## 💡 **Recommended Approach**

Based on your screenshot and old system:

### **Use Option 1 (Current Default):**
```excel
=SUM(T{row}:AA{row})
```

**Why:**
1. Your old system **overwrote** AOP columns
2. Only Quarterly forecasts (Q1M1-Q4M3) contain actual data
3. Matches your current practice
4. Avoids double-counting
5. Simple and clear

### **Your Data Pattern:**

Looking at your screenshot:
```
Row with data across AOP Final Q1-Q4: 243,380 | 111,380 | 702,985 | 101,380
Q1M1 column: 496,539

If we sum Quarterly (T-AA): This is your actual forecast total
If we included AOP: Would double-count amounts
```

The current formula ✅ **correctly sums only Quarterly forecasts**

---

## 🔄 **Auto-Generation**

The system **automatically adds** the Total FY formula when:

1. **Forecast ID is generated** (auto-triggers)
2. **Visual Formatting is run** (manual setup)

**Formula added:**
- Column AB (Total FY)
- For all data rows (Row 4+)
- Automatically updates when you enter amounts in T-AA

---

## ✅ **Verification**

After setup, check:

1. **Click any cell in Column AB (Total FY)**
2. **Look at formula bar** at top
3. **Should see:** `=SUM(T4:AA4)` (or similar)
4. **Enter amount in Q1M1 (Column T)**
5. **Total FY should auto-update** ✓

---

## 🐛 **Troubleshooting**

### **Issue: Total FY shows 0 but I have data**

**Check:**
- Where did you enter data? (AOP columns or Quarterly?)
- Current formula only sums T-AA (Quarterly)
- If data is in P-S (AOP Final), Total FY will be 0

**Solution:**
- Use Option 2 formula (includes AOP as fallback)
- Or enter data in Quarterly columns (T-AA)

### **Issue: Total FY is double the expected amount**

**Cause:** Formula sums both AOP and Quarterly columns

**Solution:**
- Use Option 1 (Quarterly only) - current default
- Or clear duplicate data (keep only in one place)

### **Issue: Formula disappeared**

**Solution:**
- Run: **📊 TDG Forecast (v2) > 🎨 Setup Visual Formatting**
- This will re-add formulas to all rows

---

## 📝 **Summary**

### **Current Setup:**
- **Formula:** `=SUM(T{row}:AA{row})`
- **Includes:** Quarterly forecasts only (Q1M1-Q4M3)
- **Excludes:** AOP columns (to avoid double-counting)
- **Auto-generates:** When Forecast ID is created

### **Why This Works:**
- Matches your old system (AOP was overwritten)
- Avoids double-counting
- Sums the actual forecasts (not planning estimates)
- Simple and clear

### **If You Need Different Logic:**
- See alternative options above
- Update the `setupTotalFYFormulas()` function
- Or manually change formulas in specific rows

---

**Your Total FY calculation is now handled automatically!** 🎯












