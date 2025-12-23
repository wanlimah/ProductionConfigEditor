# 📋 Category Management - Handling Placeholder Values

## 🎯 **Understanding Category Dropdown**

The Category column (C) uses a **dropdown list** from the `Note` sheet, but you can also allow custom entries.

---

## 📊 **Current Categories (From Your Screenshot)**

Based on your data, you have categories like:
- `22A) Tooling (TDG)`
- `21A) Test hardware (TDG)`
- And likely others like:
  - `3A) Components (TDG)`
  - `4A) Computer/Software (TDG)`
  - `8A) Equipment, cal, and repairs (TDG)`
  - `13D) Freight (DHL DGF)`
  - `16A) Other Consumables (TDG)`
  - `18A) Processing Supplies (TDG)`
  - `19A) Substrates (TDG)`

---

## 🔧 **Option 1: Allow Custom Categories (NEW - Implemented)**

### **What Changed:**

```javascript
setAllowInvalid(true)  // Users can type custom categories
```

### **How It Works:**

1. **Click on Category cell** (Column C)
2. **See dropdown** with predefined categories
3. **Choose from list** OR **type your own**

**Benefits:**
- ✅ Flexible - users can add new categories
- ✅ Still shows common categories in dropdown
- ✅ No errors if category doesn't exist in list

**Drawbacks:**
- ⚠️ Users might create typos (e.g., "Tooling" vs "Toling")
- ⚠️ Inconsistent formatting (e.g., "22A Tooling" vs "22A) Tooling (TDG)")

---

## 🔧 **Option 2: Strict Dropdown Only**

If you want to **force users** to select only from the predefined list:

```javascript
setAllowInvalid(false)  // Users MUST select from list
```

**Benefits:**
- ✅ Consistent data (no typos)
- ✅ Easy to filter and report
- ✅ Controlled category list

**Drawbacks:**
- ❌ Users can't add new categories without admin updating Note sheet
- ❌ Blocks user if their category isn't in the list

---

## 📝 **Managing Category List**

### **Where Categories Are Stored:**

The `Note` sheet (Column B):

```
Row 1: Category (header)
Row 2: 3A) Components (TDG)
Row 3: 4A) Computer/Software (TDG)
Row 4: 8A) Equipment, cal, and repairs (TDG)
Row 5: 13D) Freight (DHL DGF)
Row 6: 16A) Other Consumables (TDG)
Row 7: 18A) Processing Supplies (TDG)
Row 8: 19A) Substrates (TDG)
Row 9: 21A) Test hardware (TDG)
Row 10: 22A) Tooling (TDG)
```

### **To Add New Categories:**

1. **Open `Note` sheet**
2. **Go to Column B**
3. **Add new category** in next empty row
4. **Run:** `⚙️ Setup Data Validation` again
5. **New category appears** in dropdown

---

## 🎨 **Placeholder / Hint Text**

### **What Is It:**

A **hint text** that appears when users hover over or click the Category cell.

### **How It Works:**

```javascript
categoryRule.setHelpText('Select from list or type custom category');
```

**When user clicks Category cell:**
- Shows dropdown arrow
- Shows hint: "Select from list or type custom category"
- User knows they have options

---

## 🔄 **Handling Existing Data with Placeholder Values**

If you have rows with **placeholder** or **default** categories that need updating:

### **Option A: Find & Replace**

1. **Select Column C** (Category)
2. **Edit → Find and replace**
3. **Find:** `[Placeholder]` or whatever your placeholder text is
4. **Replace with:** *(leave empty)*
5. **Replace all**

### **Option B: Filter & Update**

1. **Click on Category column header**
2. **Create a filter**
3. **Filter by:** Placeholder value
4. **Select all visible cells**
5. **Type new category**
5. **Press Ctrl+Enter** (fills all selected cells)

### **Option C: Script to Clear Placeholders**

Add this function to your code:

```javascript
function clearPlaceholderCategories() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  const lastRow = forecastSheet.getLastRow();
  if (lastRow < 4) return;
  
  const categoryRange = forecastSheet.getRange(4, CONFIG_V2.COLS.CATEGORY, lastRow - 3, 1);
  const values = categoryRange.getValues();
  
  let clearedCount = 0;
  
  // Define placeholder patterns
  const placeholders = [
    '[Placeholder]',
    'TBD',
    'To Be Determined',
    '(Select Category)',
    ''
  ];
  
  values.forEach((row, index) => {
    if (placeholders.includes(row[0])) {
      values[index][0] = '';  // Clear placeholder
      clearedCount++;
    }
  });
  
  categoryRange.setValues(values);
  
  SpreadsheetApp.getUi().alert(`✅ Cleared ${clearedCount} placeholder categories`);
}
```

Add to menu:
```javascript
.addItem('🧹 Clear Placeholder Categories', 'clearPlaceholderCategories')
```

---

## 🎯 **Best Practices**

### **For Admins:**

1. **Keep Note sheet updated:**
   - Add all common categories
   - Use consistent format: `##A) Name (TDG)`

2. **Set validation mode based on users:**
   - **Allow Invalid = true:** If users need flexibility
   - **Allow Invalid = false:** If you want strict control

3. **Periodic cleanup:**
   - Review categories used
   - Add frequent custom ones to Note sheet
   - Standardize inconsistent entries

### **For Users:**

1. **Use dropdown first:**
   - Check if your category exists in the list
   - Saves typing and ensures consistency

2. **If typing custom category:**
   - Use consistent format: `##A) Name (TDG)`
   - Check spelling carefully

3. **Report new categories:**
   - Tell admin if you frequently use a custom category
   - Admin can add it to the official list

---

## 📊 **Category Reporting**

### **Find All Unique Categories:**

To see what categories are actually being used:

```javascript
function listAllCategories() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  const lastRow = forecastSheet.getLastRow();
  if (lastRow < 4) return;
  
  const categoryRange = forecastSheet.getRange(4, CONFIG_V2.COLS.CATEGORY, lastRow - 3, 1);
  const values = categoryRange.getValues().flat().filter(v => v);
  
  // Get unique categories
  const unique = [...new Set(values)].sort();
  
  // Display results
  const message = `📊 Categories Currently Used:\n\n${unique.join('\n')}\n\nTotal: ${unique.length} unique categories`;
  
  SpreadsheetApp.getUi().alert(message);
}
```

---

## ⚙️ **Configuration Options**

### **Current Setting (After Update):**

```javascript
setAllowInvalid(true)  // Custom categories allowed
```

### **To Change to Strict Mode:**

In `setupDataValidation()` function, change:

```javascript
.setAllowInvalid(false)  // Only predefined categories
```

Then run: **⚙️ Setup Data Validation**

---

## 🔍 **Validation Check**

### **Find Invalid Categories:**

To find categories that aren't in your Note sheet list:

```javascript
function findInvalidCategories() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  const noteSheet = ss.getSheetByName(CONFIG_V2.NOTE_SHEET);
  
  // Get valid categories from Note sheet
  const validCategories = noteSheet.getRange('B2:B').getValues().flat().filter(v => v);
  
  // Get used categories from Forecast sheet
  const lastRow = forecastSheet.getLastRow();
  const usedCategories = forecastSheet.getRange(4, CONFIG_V2.COLS.CATEGORY, lastRow - 3, 1)
    .getValues().flat().filter(v => v);
  
  // Find invalid ones
  const invalid = usedCategories.filter(cat => !validCategories.includes(cat));
  const uniqueInvalid = [...new Set(invalid)];
  
  if (uniqueInvalid.length === 0) {
    SpreadsheetApp.getUi().alert('✅ All categories are valid!');
  } else {
    const message = `⚠️ Found ${uniqueInvalid.length} invalid categories:\n\n${uniqueInvalid.join('\n')}\n\nConsider adding these to the Note sheet or correcting them.`;
    SpreadsheetApp.getUi().alert(message);
  }
}
```

---

## ✅ **Summary**

### **Default Behavior (NEW):**
- Dropdown shows predefined categories from Note sheet
- Users can **also type custom categories**
- Flexible but allows inconsistency

### **To Change:**
- Update `setAllowInvalid(false)` for strict mode
- Run **⚙️ Setup Data Validation**

### **To Manage Categories:**
- Edit `Note` sheet, Column B
- Add/remove categories as needed
- Run **⚙️ Setup Data Validation** to update dropdowns

### **To Clean Up:**
- Use Find & Replace for placeholder values
- Use filter to bulk update
- Or use custom script functions above

---

**Your Category column now allows both predefined selection and custom entry!** 🎯











