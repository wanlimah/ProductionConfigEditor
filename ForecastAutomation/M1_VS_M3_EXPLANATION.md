# 📊 M1 vs M3 - Understanding the Difference

## 🎯 **Key Concept**

**M1** and **M3** represent the **SAME item** at different stages:

- **M1** = **Forecast** (initial estimate)
- **M3** = **Actual/Confirmed** (updated, confirmed amount)

**M3 REPLACES M1** - They should NOT be added together!

---

## 📅 **Your Quarterly Cycle**

### **Quarter 1 Example:**

```
October (Q1M1):  User enters FORECAST → $10,000 in Q1M1 column
December (Q1M3): User updates with ACTUAL → $10,500 in Q1M3 column

Result: The item cost $10,500 (not $20,500!)
```

### **All Quarters:**

| Period | When | Type | Column | Description |
|--------|------|------|--------|-------------|
| **Q1M1** | Oct | Forecast | T | Initial Q1 forecast |
| **Q1M3** | Dec | Actual | U | Confirmed Q1 amount (replaces M1) |
| **Q2M1** | Jan | Forecast | V | Initial Q2 forecast |
| **Q2M3** | Mar | Actual | W | Confirmed Q2 amount (replaces M1) |
| **Q3M1** | Apr | Forecast | X | Initial Q3 forecast |
| **Q3M3** | Jun | Actual | Y | Confirmed Q3 amount (replaces M1) |
| **Q4M1** | Jul | Forecast | Z | Initial Q4 forecast |
| **Q4M3** | Sep | Actual | AA | Confirmed Q4 amount (replaces M1) |

---

## ❌ **WRONG Total FY Formula (Double Counting):**

```excel
=SUM(T:AA)  // Sums ALL quarterly columns
```

**Problem:**
```
Q1M1 (T): $10,000 (forecast)
Q1M3 (U): $10,500 (actual - replaces M1)
Q2M1 (V): $15,000 (forecast)
Q2M3 (W): $15,200 (actual - replaces M1)

Wrong Total: $10,000 + $10,500 + $15,000 + $15,200 = $50,700 ❌
(This double-counts each quarter!)
```

---

## ✅ **CORRECT Total FY Formula (M3 Only):**

```excel
=SUM(U,W,Y,AA)  // Sums only M3 columns (actuals)
```

**Why:**
- U (Q1M3): Confirmed Q1 amount
- W (Q2M3): Confirmed Q2 amount  
- Y (Q3M3): Confirmed Q3 amount
- AA (Q4M3): Confirmed Q4 amount

**Correct Calculation:**
```
Q1M1 (T): $10,000 (forecast) ← NOT included
Q1M3 (U): $10,500 (actual)   ← INCLUDED ✓
Q2M1 (V): $15,000 (forecast) ← NOT included
Q2M3 (W): $15,200 (actual)   ← INCLUDED ✓

Correct Total: $10,500 + $15,200 = $25,700 ✓
```

---

## 📊 **Visual Example from Your Sheet**

Based on your screenshot:

```
| Item | Q1M1 (T) | Q1M3 (U) | Q2M1 (V) | Q2M3 (W) | ... | Total FY (AB) |
|------|----------|----------|----------|----------|-----|---------------|
| A    | $8,000   | $8,300   | blank    | blank    | ... | $8,300        |
| B    | $70,000  | $70,000  | blank    | blank    | ... | $70,000       |
| C    | blank    | blank    | $5,000   | $5,200   | ... | $5,200        |
```

**Item A:**
- Forecasted $8,000 in Oct (Q1M1)
- Actually cost $8,300 in Dec (Q1M3) ← Use this!
- Total FY = $8,300 (not $16,300)

**Item B:**
- Forecasted $70,000 in Oct (Q1M1)
- Confirmed $70,000 in Dec (Q1M3) ← Use this!
- Total FY = $70,000 (not $140,000)

**Item C:**
- Forecasted $5,000 for Q2 in Jan (Q2M1)
- Actually cost $5,200 in Mar (Q2M3) ← Use this!
- Total FY = $5,200 (not $10,200)

---

## 🔄 **What About Items Only in M1 (Forecast)?**

**Scenario:** It's October, Q1M3 hasn't happened yet.

```
| Item | Q1M1 (T) | Q1M3 (U) | Total FY (AB) |
|------|----------|----------|---------------|
| New  | $5,000   | blank    | $0 (for now)  |
```

**Issue:** Formula `=SUM(U,W,Y,AA)` shows $0 because M3 columns are empty.

**Solution:** This is correct behavior! 

**Why:**
- Total FY should show **confirmed amounts**
- M1 is just a forecast, not finalized
- When M3 is entered (December), Total FY will update

**Alternative (if you want to show forecast until M3 is entered):**

For each quarter, use M3 if available, otherwise fallback to M1:

```excel
=SUM(
  IF(U<>"", U, T),  // Use Q1M3 if exists, else Q1M1
  IF(W<>"", W, V),  // Use Q2M3 if exists, else Q2M1
  IF(Y<>"", Y, X),  // Use Q3M3 if exists, else Q3M1
  IF(AA<>"", AA, Z) // Use Q4M3 if exists, else Q4M1
)
```

But this is more complex. **Simpler to show M3 only** (confirmed amounts).

---

## 📝 **Current Implementation**

### **Formula in Column AB (Total FY):**

```excel
=SUM(U{row}, W{row}, Y{row}, AA{row})
```

### **What It Includes:**
- ✅ Q1M3 (U) - Q1 actual
- ✅ Q2M3 (W) - Q2 actual
- ✅ Q3M3 (Y) - Q3 actual
- ✅ Q4M3 (AA) - Q4 actual

### **What It Excludes:**
- ❌ Q1M1 (T) - Q1 forecast
- ❌ Q2M1 (V) - Q2 forecast
- ❌ Q3M1 (X) - Q3 forecast
- ❌ Q4M1 (Z) - Q4 forecast
- ❌ AOP columns (H-S) - planning estimates

---

## 🎯 **Why This Is Correct**

1. **Avoids Double-Counting:**
   - M3 replaces M1 for the same item
   - Adding both = counting the same expense twice

2. **Shows Confirmed Amounts:**
   - M3 = actual/confirmed
   - M1 = just an estimate
   - Total FY should show real amounts

3. **Matches Financial Practice:**
   - In accounting, you report actuals, not forecasts
   - M3 is the "true" amount for that quarter

4. **Variance Tracking:**
   - Can compare M1 (forecast) vs M3 (actual) separately
   - Variance = M3 - M1
   - But Total FY = sum of M3 only

---

## 📊 **Row 2 (TOTAL) Formula**

Row 2 also uses M3 only:

```excel
Row 2, Column AB (Total FY): =SUM(U2, W2, Y2, AA2)
```

This sums all items' M3 actuals to show **total confirmed spending for the fiscal year**.

---

## 🔧 **When to Use M1 vs M3**

### **Use M1 (Forecast) For:**
- ✅ Initial planning (before M3 exists)
- ✅ Variance reports (compare forecast vs actual)
- ✅ Early-year tracking (Q1M1 entered, Q1M3 not yet)

### **Use M3 (Actual) For:**
- ✅ **Total FY calculation** ← Your case
- ✅ Budget vs actual reports
- ✅ Final year-end totals
- ✅ PR matching (actual amounts spent)

### **Never:**
- ❌ Sum M1 + M3 together (double-counting!)
- ❌ Include both in Total FY

---

## ✅ **Summary**

**M1 = Forecast (initial guess)**  
**M3 = Actual (confirmed amount)**  
**M3 REPLACES M1** (not added to it)

**Total FY Formula:**
```excel
=SUM(U, W, Y, AA)  // Only M3 columns (actuals)
```

**Result:**
- ✅ No double-counting
- ✅ Shows confirmed amounts
- ✅ Accurate fiscal year total
- ✅ Can still compare M1 vs M3 separately for variance

---

**Your Total FY now correctly calculates based on M3 (actuals) only!** 🎯












