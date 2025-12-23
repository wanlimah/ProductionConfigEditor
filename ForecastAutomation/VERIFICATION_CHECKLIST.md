# ✅ Verification Checklist - Total FY Formula

## 🎯 **What to Test**

After running the setup, verify these scenarios:

---

## **Test 1: Row 2 Total FY Sums All Items**

### **Setup:**
```
Row 4: Item A → Q1M3 = $2,500 → Total FY (AB4) = $2,500
Row 5: Item B → Q1M3 = $1,000 → Total FY (AB5) = $1,000
Row 6: Item C → Q2M3 = $3,000 → Total FY (AB6) = $3,000
```

### **Expected Result:**
```
Row 2, Column AB (Total FY): $6,500
Formula: =SUM(AB4:AB)
```

### **Why:**
Row 2 Total FY should sum ALL items' Total FY amounts.

---

## **Test 2: Individual Row Total FY Sums Only M3**

### **Setup:**
```
Row 4:
- Q1M1 (T): $10,000 (forecast)
- Q1M3 (U): $10,500 (actual)
- Q2M1 (V): $5,000 (forecast)
- Q2M3 (W): $5,200 (actual)
```

### **Expected Result:**
```
Row 4, Column AB (Total FY): $15,700
Formula: =SUM(U4,W4,Y4,AA4)

NOT $30,700 (which would include M1 forecasts)
```

### **Why:**
- M3 replaces M1 (not added to it)
- Only M3 (actuals) should be summed

---

## **Test 3: Empty M3 Shows $0**

### **Setup:**
```
Row 5:
- Q1M1 (T): $8,000 (forecast)
- Q1M3 (U): blank
- All other columns: blank
```

### **Expected Result:**
```
Row 5, Column AB (Total FY): $0.00
```

### **Why:**
Formula sums only M3 columns. Since they're empty, result is $0.

---

## **Test 4: M1 Total vs M3 Total in Row 2**

### **Setup:**
```
Row 2:
- Q1M1 (T): $496,539.00 (sum of all Q1M1)
- Q1M3 (U): $2,500.00 (sum of all Q1M3)
- Total FY (AB): Should show $2,500.00 (NOT $499,039.00)
```

### **Expected Result:**
```
Row 2, Column AB: $2,500.00 ✓
```

### **Why:**
Row 2 Total FY sums Column AB4:AB (all items' Total FY).
Each item's Total FY only includes M3, so Row 2 total also reflects M3 only.

---

## **Test 5: Multiple Quarters with Data**

### **Setup:**
```
Row 4:
- Q1M3 (U): $1,000
- Q2M3 (W): $2,000
- Q3M3 (Y): $3,000
- Q4M3 (AA): $4,000
```

### **Expected Result:**
```
Row 4, Column AB (Total FY): $10,000
Formula: =SUM(U4,W4,Y4,AA4)
```

---

## 🔍 **How to Verify Formulas**

### **Check Data Row Formula (e.g., Row 4):**

1. Click on cell **AB4**
2. Look at formula bar at top
3. Should see: `=SUM(U4,W4,Y4,AA4)`

### **Check Total Row Formula (Row 2):**

1. Click on cell **AB2**
2. Look at formula bar at top
3. Should see: `=SUM(AB4:AB)`

---

## 📊 **Your Current Screenshot Analysis**

Based on your screenshot:

### **Row 2 (TOTAL):**
- Q1M1 (T): $496,539.00
- Q1M3 (U): $2,500.00
- Q2M3-Q4M3: $0.00
- **Total FY (AB): $2,500.00** ✅

**This is CORRECT!**

The formula `=SUM(AB4:AB)` sums all items' Total FY.
Since each item's Total FY only includes M3 amounts, the grand total is $2,500.

### **Row 4 (Data):**
- Q1M1 (T): $2,500.00
- Q1M3 (U): $2,500.00
- **Total FY (AB): Should show $2,500.00**

Check if Row 4, AB4 formula is: `=SUM(U4,W4,Y4,AA4)`

---

## ⚠️ **Common Issues**

### **Issue: Row 2 Total FY shows same as Q1M3**

**Example:**
```
Q1M3 (U2): $2,500
Total FY (AB2): $2,500
```

**This is CORRECT if:**
- You only have data in Q1M3
- Other M3 columns (Q2M3, Q3M3, Q4M3) are empty or $0

**To verify:**
- Add data to Q2M3 (e.g., $1,000)
- Total FY should update to $3,500

### **Issue: Total FY in data row shows $0**

**Cause:** Formula might be summing wrong cells

**Solution:**
1. Click the cell
2. Check formula: Should be `=SUM(U{row},W{row},Y{row},AA{row})`
3. Re-run: **🎨 Setup Visual Formatting**

### **Issue: Total FY in Row 2 doesn't update**

**Cause:** Formula might be a value, not a formula

**Solution:**
1. Click cell AB2
2. Check formula bar: Should see `=SUM(AB4:AB)`
3. If it's just a number, re-run: **🔢 Setup Total Row**

---

## ✅ **Final Verification Steps**

1. **Click cell AB2** (Row 2, Total FY)
   - Formula bar should show: `=SUM(AB4:AB)`
   - Value should sum all items below

2. **Click cell AB4** (Row 4, Total FY)
   - Formula bar should show: `=SUM(U4,W4,Y4,AA4)`
   - Value should sum only M3 columns for that row

3. **Test with new data:**
   - Add $1,000 to cell U5 (Row 5, Q1M3)
   - Cell AB5 should auto-calculate to $1,000
   - Cell AB2 should auto-update to include the new $1,000

4. **Test M1 vs M3:**
   - Enter $5,000 in T6 (Row 6, Q1M1)
   - Enter $5,500 in U6 (Row 6, Q1M3)
   - Cell AB6 should show $5,500 (not $10,500)

---

## 📝 **Quick Reference**

### **Data Row Formula (AB4, AB5, AB6, etc.):**
```excel
=SUM(U{row}, W{row}, Y{row}, AA{row})
```
Sums: Q1M3 + Q2M3 + Q3M3 + Q4M3 (M3 actuals only)

### **Total Row Formula (AB2):**
```excel
=SUM(AB4:AB)
```
Sums: All Total FY values from all data rows

### **Result:**
- Each item's Total FY = M3 actuals only (no double-counting)
- Grand Total FY (Row 2) = Sum of all items' Total FY

---

## 🎯 **Your Specific Test**

You mentioned:
> "Column Q1M3 row 4 no data and the row 2 column AB should be the total of the column AB"

### **To Test This:**

1. **Clear Q1M3 in Row 4:**
   - Click U4
   - Delete content
   - Press Enter

2. **Check Row 4 Total FY (AB4):**
   - Should show $0.00 (or sum of other M3 if any)

3. **Check Row 2 Total FY (AB2):**
   - Should update to exclude Row 4's amount
   - Should show sum of AB5, AB6, AB7... (if they exist)

### **Expected Behavior:**
```
Before:
Row 4, U4: $2,500
Row 4, AB4: $2,500
Row 2, AB2: $2,500

After deleting U4:
Row 4, U4: blank
Row 4, AB4: $0.00
Row 2, AB2: $0.00 (if Row 4 was the only item)
```

---

**All formulas are now correct and should work as expected!** ✅












