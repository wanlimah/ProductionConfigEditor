# 📦 Placeholder Item Workflow

## 🎯 **Use Case**

You need to **reserve budget** for an item, but don't know the exact item yet.

**Example:**
- **Category:** 21A) Test hardware (TDG)
- **Estimated Amount:** $5,000
- **Purpose:** Wafer Sort equipment
- **Problem:** Don't know exact model/vendor yet

**Solution:** Create a **placeholder** item!

---

## 📋 **Workflow: From Placeholder to Actual Item**

### **Step 1: Create Placeholder Item**

When entering forecast:

| Column | Value | Example |
|--------|-------|---------|
| **B (Requestor)** | Your name | Lam, Tuck Hon |
| **C (Category)** | Category | 21A) Test hardware (TDG) |
| **D (Project)** | Project name | BlueChip |
| **E (Item Name)** | **"Placeholder [Purpose]"** | **Placeholder Wafer Sort** |
| **Q1M3 (or period)** | Estimated amount | $5,000 |
| **AB (Status)** | **"Placeholder"** | Placeholder |
| **AE (Description)** | Brief description | Budget allocation for wafer sort equipment |

**Result:**
- Forecast ID auto-generates: `FY26-001`
- Status = "Placeholder"
- Amount = $5,000
- Item clearly marked as placeholder

---

### **Step 2: When PR is Raised**

Once you know the actual item and raise a PR:

**What User Does:**

1. **Enter PR Number** in Column AC (PR #):
   - Type PR number (e.g., `PR-2026-045`)
   - Press Enter

2. **System Alert Appears:**
   ```
   ⚠️ Placeholder Item Detected
   
   This item "Placeholder Wafer Sort" is marked as Placeholder.
   
   PR Number: PR-2026-045
   
   Please update:
   1. Item Name (Column E) with actual item
   2. Description (Column AE) with details
   3. Status to "PR Raised"
   
   Click OK when done, or Cancel to skip.
   ```

3. **Update Item Details:**
   - **Column E (Item Name):** Replace with actual item
     - ❌ OLD: "Placeholder Wafer Sort"
     - ✅ NEW: "Keysight Wafer Prober Model XYZ"
   
   - **Column AE (Description):** Add actual details
     - ❌ OLD: "Budget allocation for wafer sort equipment"
     - ✅ NEW: "Keysight 150mm Wafer Prober, Model XYZ-2000, Serial #12345"
   
   - **Column AB (Status):** Auto-updates to "PR Raised"

4. **Click OK** in the alert

**System Auto-Updates:**
- ✅ Status → "PR Raised"
- ✅ Change Log → Records placeholder update with PR number
- ✅ Last Updated → Current timestamp
- ✅ Updated By → Your email

---

## 📊 **Example: Complete Flow**

### **October (Q1M1 Forecast):**

```
FY26-001 | Lam, Tuck Hon | 21A) Test hardware | BlueChip | Placeholder Wafer Sort | ... | $5,000 | Placeholder
```

**Status:** Budget reserved, waiting for vendor selection

---

### **December (PR Raised):**

User enters: **PR #:** `PR-2026-045`

**Alert appears:**
> ⚠️ Update placeholder with actual item details

User updates:
- **Item Name:** Keysight Wafer Prober XYZ-2000
- **Description:** Keysight 150mm automated wafer prober with optical alignment
- Clicks OK

**After Update:**
```
FY26-001 | Lam, Tuck Hon | 21A) Test hardware | BlueChip | Keysight Wafer Prober XYZ-2000 | ... | $5,000 | PR Raised | PR-2026-045
```

**Change Log:**
```
2025-10-15 10:30: Created placeholder item
2025-12-20 14:45: Placeholder updated to actual item (PR: PR-2026-045) by lam.tuck.hon@company.com
```

---

## 🔍 **Finding Placeholder Items**

### **Menu Option:**

```
📊 TDG Forecast (v2) > 📦 List Placeholder Items
```

**Shows:**
```
⚠️ Found 3 Placeholder Item(s):

• FY26-001 | Placeholder Wafer Sort
  Category: 21A) Test hardware (TDG)
  Amount: $5,000
  PR: Not raised
  Row: 24

• FY26-015 | Placeholder Test Board
  Category: 21A) Test hardware (TDG)
  Amount: $2,500
  PR: PR-2026-045
  Row: 38

• FY26-022 | Placeholder Calibration Equipment
  Category: 8A) Equipment, cal, and repairs (TDG)
  Amount: $8,000
  PR: Not raised
  Row: 45

💡 When PR is raised:
1. Enter PR # in column AC
2. Update Item Name (column E)
3. Update Description (column AE)
4. Change Status to "PR Raised"
```

---

## ⚙️ **Status Values**

The system now includes:

| Status | Meaning | Use For |
|--------|---------|---------|
| **Draft** | Initial entry | New items being entered |
| **Placeholder** | Budget reserved | Items without exact details yet |
| **Active** | Approved forecast | Standard forecasted items |
| **Approved** | Management approved | Items approved for purchase |
| **PR Raised** | PR submitted | Placeholder → Actual, PR in system |
| **In Progress** | PO issued | Purchase order created |
| **Completed** | Item received | Item delivered and completed |
| **Carry To Next FY** | Deferred | Carrying to next fiscal year |
| **On Hold** | Temporarily stopped | Waiting for approval/decision |
| **Cancelled** | No longer needed | Cancelled items |
| **Rejected** | Not approved | Rejected by management |

---

## 💡 **Best Practices**

### **Creating Placeholders:**

1. **Clear Naming:**
   - ✅ "Placeholder Wafer Sort"
   - ✅ "Placeholder Test Equipment"
   - ❌ "TBD" (too vague)
   - ❌ "Unknown Item" (not helpful)

2. **Use Proper Category:**
   - Helps with budget tracking
   - Easier to find related items

3. **Estimate Accurately:**
   - Research typical costs
   - Add contingency if uncertain
   - Update amount when PR raised if different

4. **Add Description:**
   - Explain what the placeholder is for
   - Add requirements or constraints
   - Note any specific vendors being considered

### **Updating Placeholders:**

1. **Update Promptly:**
   - When PR is raised, update immediately
   - Don't leave placeholders with PR numbers

2. **Complete Details:**
   - Full item name with model number
   - Vendor information
   - Serial numbers if available

3. **Check Amount:**
   - If PR amount differs from placeholder
   - Update the forecast amount accordingly

4. **Update Status:**
   - Placeholder → PR Raised → In Progress → Completed

---

## 📈 **Reporting on Placeholders**

### **Dashboard Tracking:**

The **Open Items Dashboard** shows:
- Items by status (including Placeholders)
- Placeholder items by category
- Placeholder amounts by requestor

**Example:**
```
Summary by Status:
  Placeholder: 3 items | $15,500

Summary by Category:
  21A) Test hardware: 2 placeholders | $7,500
  8A) Equipment, cal: 1 placeholder | $8,000
```

---

## 🔄 **Workflow Summary**

```
┌─────────────────────────────────────────────────────────────┐
│ STEP 1: CREATE PLACEHOLDER                                  │
├─────────────────────────────────────────────────────────────┤
│ • Item Name: "Placeholder [Purpose]"                        │
│ • Status: "Placeholder"                                      │
│ • Amount: Estimated budget                                   │
│ • Description: Purpose/requirements                          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ STEP 2: VENDOR SELECTION / APPROVAL                         │
├─────────────────────────────────────────────────────────────┤
│ • Research vendors                                           │
│ • Get quotes                                                 │
│ • Management approval                                        │
│ • Select final item                                          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ STEP 3: RAISE PR                                            │
├─────────────────────────────────────────────────────────────┤
│ • Submit PR in purchasing system                            │
│ • Get PR number                                              │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ STEP 4: UPDATE FORECAST (AUTOMATED PROMPT)                  │
├─────────────────────────────────────────────────────────────┤
│ • Enter PR # in Column AC → Alert appears!                  │
│ • Update Item Name (Column E)                                │
│ • Update Description (Column AE)                             │
│ • System auto-updates Status to "PR Raised"                 │
│ • Change logged automatically                                │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ STEP 5: TRACK TO COMPLETION                                 │
├─────────────────────────────────────────────────────────────┤
│ • PO issued: Status = "In Progress"                         │
│ • Enter PO # in Column AD                                    │
│ • Item received: Status = "Completed"                       │
└─────────────────────────────────────────────────────────────┘
```

---

## ✅ **Benefits**

### **For Budget Planning:**
- ✅ Reserve budget early (even without details)
- ✅ Track placeholder vs actual costs
- ✅ Easy to see which items need finalization

### **For Procurement:**
- ✅ Clear distinction between placeholder and actual items
- ✅ Auto-prompt to update when PR raised
- ✅ Complete audit trail from placeholder to completion

### **For Reporting:**
- ✅ See how many items are still TBD
- ✅ Track conversion from placeholder to actual
- ✅ Monitor budget accuracy (placeholder estimate vs actual)

---

## 🐛 **Troubleshooting**

### **Issue: Alert doesn't appear when I enter PR**

**Check:**
1. Is Status = "Placeholder"?
2. Does Item Name contain "placeholder" (case insensitive)?
3. Did you enter a PR number in Column AC?

**Solution:**
- Set Status to "Placeholder"
- Ensure Item Name includes word "Placeholder"
- Re-enter PR number

### **Issue: Can't find my placeholder items**

**Solution:**
- Run: **📦 List Placeholder Items**
- Or filter by Status = "Placeholder"

### **Issue: Forgot to update placeholder after PR**

**Solution:**
1. Find the item (look for "Placeholder" in Item Name)
2. Manually update Item Name and Description
3. Change Status to "PR Raised"
4. System will log the change

---

## 📝 **Summary**

**Placeholder Workflow:**
1. ✅ Create placeholder with estimated amount
2. ✅ Status = "Placeholder"
3. ✅ Raise PR when ready
4. ✅ Enter PR # → System prompts to update
5. ✅ Update item details and status
6. ✅ Track to completion

**Key Features:**
- Automatic detection when PR entered
- Prompt to update details
- Auto-status update
- Complete change logging
- Easy reporting

**Your placeholder items are now tracked from estimate to actual!** 📦✅











