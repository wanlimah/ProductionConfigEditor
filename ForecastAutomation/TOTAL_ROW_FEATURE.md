# 🔢 Total Row Feature

## Overview

A **Total Row** has been added to Row 2 that automatically calculates the sum of all quarterly and AOP forecast columns.

---

## ✨ What It Does

### **Automatic Column Totals**

The Total Row (Row 2) displays real-time sums for:

**AOP Columns:**
- AOP 0.5 Q1-Q4 (Columns H-K)
- AOP 1 Q1-Q4 (Columns L-O)
- AOP Final Q1-Q4 (Columns P-S)

**Quarterly Forecast Columns:**
- Q1M1, Q1M3 (Columns T-U)
- Q2M1, Q2M3 (Columns V-W)
- Q3M1, Q3M3 (Columns X-Y)
- Q4M1, Q4M3 (Columns Z-AA)

**Summary Columns:**
- Total FY (Column AB)
- Carry To (Column AC)

---

## 📐 Sheet Structure

```
Row 1: Period Deadlines (e.g., "11 July 2025", "29 October 2025")
Row 2: TOTAL (auto-sum formulas) ← NEW FEATURE
Row 3: Column Headers (Forecast ID, Requestor, Category, etc.)
Row 4+: Data rows
```

### **Row 2 Example:**

| A | H | I | J | K | ... | T | U | ... | AB |
|---|---|---|---|---|-----|---|---|-----|----|
| **TOTAL** | =SUM(H4:H) | =SUM(I4:I) | =SUM(J4:J) | =SUM(K4:K) | ... | =SUM(T4:T) | =SUM(U4:U) | ... | =SUM(AB4:AB) |

---

## 🎨 Formatting

**Visual Design:**
- Background: Light yellow (#FFF2CC)
- Font: Bold, 11pt
- Number format: Currency with 2 decimals (#,##0.00)
- Label in Column A: "TOTAL"

---

## 🚀 How to Setup

### **Option 1: Use Menu Item (Recommended)**

1. Open your Google Sheet
2. Click: **📊 TDG Forecast (v2) > 🔢 Setup Total Row**
3. Confirm: "✅ Total row setup complete!"

### **Option 2: Manual Setup**

If you prefer to create it manually:

1. In Row 2, Column A: Type **TOTAL**
2. In Row 2, Column H: Type `=SUM(H4:H)`
3. Copy formula across to columns I, J, K (AOP 0.5)
4. Repeat for columns L-O (AOP 1), P-S (AOP Final), T-AA (Quarterly), AB-AC (Summary)
5. Format Row 2: Yellow background, bold, currency format

---

## 💡 Benefits

### **1. Instant Overview**
See total forecasted amounts for each quarter at a glance.

### **2. Real-Time Updates**
As users enter or update forecast amounts, Row 2 automatically recalculates.

### **3. Budget Tracking**
Quickly compare quarterly totals against your budget.

### **4. Period Comparison**
Easily compare AOP 0.5 vs AOP 1 vs AOP Final totals.

### **Example Use Case:**

```
Want to know total Q1M1 forecasts across all items?
→ Just look at Row 2, Column T!

Want to know total AOP Final Q3 across all categories?
→ Just look at Row 2, Column R!
```

---

## 🔄 How It Works

### **Formula Explanation:**

```excel
=SUM(H4:H)
```

- **H4:** First data row (Row 4) in column H
- **H:** Last row in column H (open-ended, auto-expands)
- **SUM:** Adds all values from Row 4 to the last row with data

### **Auto-Expanding Range:**

When you add new rows of data (Row 5, 6, 7, etc.), the formula automatically includes them in the sum. No need to update formulas!

---

## 📊 Example Sheet Layout

```
Row 1: [Deadline] | ... | 11 July 2025 | 11 July 2025 | 11 July 2025 | 11 July 2025 | ... | 29 Oct 2025 | ...
Row 2: TOTAL      | ... | 125,000.00   | 80,000.00    | 95,000.00    | 110,000.00   | ... | 150,000.00  | ...
Row 3: Headers    | ... | Q1           | Q2           | Q3           | Q4           | ... | Q1M1        | ...
Row 4: FY25-001   | ... | 25,000       | 15,000       | 20,000       | 25,000       | ... | 30,000      | ...
Row 5: FY25-002   | ... | 50,000       | 35,000       | 40,000       | 45,000       | ... | 60,000      | ...
Row 6: FY25-003   | ... | 30,000       | 15,000       | 20,000       | 25,000       | ... | 35,000      | ...
Row 7: FY25-004   | ... | 20,000       | 15,000       | 15,000       | 15,000       | ... | 25,000      | ...
```

In this example:
- Row 2, Column H shows: **125,000** (sum of 25k + 50k + 30k + 20k)
- As you add more items, the total updates automatically!

---

## 🔒 Protection Recommendation

To prevent accidental edits to the Total Row:

1. Select Row 2 entirely
2. Right-click → **Protect range**
3. Name: "Total Row - Do Not Edit"
4. Set permissions: **Only you** or **Specific people**
5. Check: **Show a warning when editing this range**
6. Click **Done**

This ensures users can't accidentally delete the formulas.

---

## ✅ Testing

After setup, verify:

1. ✅ Row 2 has "TOTAL" label in Column A
2. ✅ All quarterly columns (H-AA) have SUM formulas
3. ✅ Summary columns (AB-AC) have SUM formulas
4. ✅ Row 2 has yellow background and bold text
5. ✅ Numbers display with currency formatting
6. ✅ When you add data in Row 4+, Row 2 updates automatically
7. ✅ When you edit amounts, Row 2 recalculates instantly

---

## 🐛 Troubleshooting

### **Issue: Total showing 0 or blank**

**Cause:** No data entered yet, or data is in rows 1-3

**Solution:** Enter numerical values in Row 4 or below

### **Issue: Formula shows as text (e.g., "=SUM(H4:H)")**

**Cause:** Cell format is set to "Text"

**Solution:**
1. Select Row 2
2. Format → Number → Number (or Currency)
3. Re-enter formula

### **Issue: Total not updating when adding data**

**Cause:** Formula doesn't have open-ended range

**Solution:**
- Verify formula is `=SUM(H4:H)` not `=SUM(H4:H100)`
- The range should end with the column letter only (H, not H100)

### **Issue: Formulas were accidentally deleted**

**Solution:**
- Run: **📊 TDG Forecast (v2) > 🔢 Setup Total Row** again
- This will restore all formulas

---

## 🎯 Best Practices

1. **Protect the Total Row** to prevent accidental edits
2. **Always start data from Row 4** onwards
3. **Don't manually edit Row 2** - let formulas calculate
4. **Use the menu item** to setup/restore formulas if needed
5. **Freeze rows 1-3** for easy viewing (View → Freeze → 3 rows)

---

## 📝 Notes

- Total Row formulas are created by the `setupTotalRow()` function
- Formulas use open-ended ranges (e.g., H4:H) for auto-expansion
- The function includes error handling and validation
- Compatible with all existing features (auto-ID generation, change tracking, etc.)

---

**Version:** 2.0  
**Created:** 2024-11-13  
**Feature Status:** ✅ Production Ready












