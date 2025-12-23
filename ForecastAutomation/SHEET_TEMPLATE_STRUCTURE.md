# 📋 Sheet Template Structure

## Overview

This document defines the exact structure needed for the automation system to work correctly.

---

## 📊 Required Sheets

Your Google Spreadsheet must have these tabs:

1. **Forecast** - Where users enter forecast data
2. **PR Tracking** - Where PRs are recorded
3. **TDG : Forecast vs Actual** - Comparison and variance tracking
4. **Config** - System configuration (auto-created)
5. **User Status** - Submission tracking (auto-created)
6. **Variance Report** - Analysis reports (auto-created)

---

## 1️⃣ Forecast Sheet

### **Column Structure**

| Col | Header | Data Type | Purpose | Auto/Manual | Example |
|-----|--------|-----------|---------|-------------|---------|
| **A** | Forecast ID | Text | Unique identifier | AUTO | `FY25-Q1M1-001` |
| **B** | Quarter | Text | Quarter | Manual | `Q1`, `Q2`, `Q3`, `Q4` |
| **C** | Year | Text | Fiscal year | Manual | `FY25`, `FY26` |
| **D** | No | Number | Sequential number | Manual | `1`, `2`, `3` |
| **E** | Date | Date | Entry date | Manual | `2024-10-01` |
| **F** | Epicentech Group | Text | Department/Group | Manual | `Production`, `Engineering` |
| **G** | Location | Text | Physical location | Manual | `Site A`, `Site B` |
| **H** | Budgeted or Unbudgeted | Text | Budget status | Manual | `Budgeted`, `Unbudgeted` |
| **I** | Prism | Text | Prism code | Manual | `PRISM-001` |
| **J** | Epicentech Category | Text | One of 12 categories | Manual | `Equipment`, `Software` |
| **K** | Item (Simple) | Text | Simple item name | Manual | `Laptop`, `Server` |
| **L** | Description | Text | Detailed description | Manual | `Dell Latitude 7420` |
| **M** | Forecast (M1) | Number | Month 1 forecast | Manual | `50000` |
| **N** | Unbudgeted | Number | Unbudgeted amount | Manual | `0`, `5000` |
| **O** | Forecast (M3) | Number | Month 3 forecast | Manual | `50000` |
| **P** | Forecast Amount | Number | Final forecast amount | Manual | `50000` |
| **Q** | User | Email | User who created | AUTO | `user@company.com` |
| **R** | Submitted | Timestamp | When submitted | AUTO | `2024-10-01 10:30:00` |

### **Header Row**

```
Row 1: Forecast ID | Quarter | Year | No | Date | Epicentech Group | Location | Budgeted or Unbudgeted | Prism | Epicentech Category | Item (Simple) Item | Description | Forecast (M1) | Unbudgeted | Forecast (M3) | Forecast Amount | User | Submitted
```

### **Formatting Suggestions**

- **Header row**: Bold, background color (e.g., blue #4285f4), white text
- **Freeze first row**: View > Freeze > 1 row
- **Column A (Forecast ID)**: Light gray background (read-only reminder)
- **Column Q (User)**: Can use `=USER()` formula or manual entry
- **Column R (Submitted)**: Can use `=NOW()` when submitting

### **Data Validation**

Optional but recommended:

**Column B (Quarter):**
- Data validation: List from range
- Values: `Q1, Q2, Q3, Q4`

**Column H (Budgeted or Unbudgeted):**
- Data validation: List from range
- Values: `Budgeted, Unbudgeted`

**Column J (Epicentech Category):**
- Data validation: List from range
- Values: `Equipment, Software, Materials, Services, Maintenance, Training, Travel, Facilities, IT Infrastructure, Marketing, HR, Miscellaneous`

### **Sample Data**

```
FY25-Q1M1-001 | Q1 | FY25 | 1 | 2024-10-01 | Production | Site A | Budgeted | PRISM-001 | Equipment | Laptop | Dell Latitude 7420 | 50000 | 0 | 50000 | 50000 | user1@company.com | 2024-10-01 09:00:00
FY25-Q1M1-002 | Q1 | FY25 | 2 | 2024-10-01 | Engineering | Site B | Unbudgeted | PRISM-002 | Software | License | Adobe Creative Cloud | 30000 | 30000 | 30000 | 30000 | user2@company.com | 2024-10-01 10:15:00
```

---

## 2️⃣ PR Tracking Sheet

### **Column Structure**

| Col | Header | Data Type | Purpose | Auto/Manual | Example |
|-----|--------|-----------|---------|-------------|---------|
| **A** | Forecast ID | Text | Reference to forecast | Manual | `FY25-Q1M1-001` |
| **B** | PR Number | Text | Purchase requisition # | Manual | `PR-2024-001` |
| **C** | PR Date | Date | Date PR raised | Manual | `2024-10-15` |
| **D** | Actual Amount | Number | Actual PR amount | Manual | `48000` |
| **E** | Received Amount | Number | Amount received | Manual | `48000` |
| **F** | PO Status | Text | Purchase order status | Manual | `Approved`, `Pending`, `Received` |
| **G** | Variance | Number | Calculated variance | AUTO (formula) | `-2000` |
| **H** | User | Email | User who raised PR | Manual | `user1@company.com` |

### **Header Row**

```
Row 1: Forecast ID | PR Number | PR Date | Actual Amount | Received Amount | PO Status | Variance | User
```

### **Formula for Column G (Variance)**

If you want manual calculation in the sheet (optional, automation calculates it):

```
=VLOOKUP(A2,Forecast!$A$2:$P$1000,16,FALSE)-D2
```

This looks up Forecast Amount (column P in Forecast) and subtracts Actual Amount.

### **Data Validation**

**Column A (Forecast ID):**
- Data validation: Dropdown from Forecast sheet
- Formula: `=Forecast!$A$2:$A$1000`
- Benefits: Prevents typos, ensures valid IDs

**Column F (PO Status):**
- Data validation: List from range
- Values: `Pending, Approved, Received, Cancelled`

### **Sample Data**

```
FY25-Q1M1-001 | PR-2024-001 | 2024-10-15 | 48000 | 48000 | Received | -2000 | user1@company.com
FY25-Q1M1-002 | PR-2024-002 | 2024-10-16 | 32000 | 30000 | Approved | 2000 | user2@company.com
```

---

## 3️⃣ TDG : Forecast vs Actual Sheet

This is your existing comparison sheet. The automation will **write to this sheet**.

### **Column Structure**

Based on your screenshot:

| Col | Header | Source | Notes |
|-----|--------|--------|-------|
| **A** | Forecast ID | Forecast | Auto-populated |
| **B** | Quarter | Forecast | Auto-populated |
| **C** | Year | Forecast | Auto-populated |
| **D** | No | Forecast | Auto-populated |
| **E** | Date | Forecast | Auto-populated |
| **F** | Epicentech Group | Forecast | Auto-populated |
| **G** | Location | Forecast | Auto-populated |
| **H** | Budgeted or Unbudgeted | Forecast | Auto-populated |
| **I** | Prism | Forecast | Auto-populated |
| **J** | Epicentech Category | Forecast | Auto-populated |
| **K** | Item | Forecast | Auto-populated |
| **L** | Description | Forecast | Auto-populated |
| **M** | Forecast Amount | Forecast | Auto-populated |
| **N** | Unbudgeted | Forecast | Auto-populated |
| **O** | Forecast M3 | Forecast | Auto-populated |
| **P** | Actual Amount | PR Tracking | Auto-calculated (sum if multiple PRs) |
| **Q** | Received Amount | PR Tracking | Auto-calculated |
| **R** | Variance | Calculated | Forecast - Actual |
| **S** | Variance % | Calculated | (Variance / Forecast) * 100 |
| **T** | PR Numbers | PR Tracking | Comma-separated if multiple |

### **Header Rows**

The script assumes headers start at **row 5** (based on your screenshot showing row 4 as header).

Adjust in `Code.gs` if different:

```javascript
const headerRow = 5; // Change this if your header is different row
```

### **Conditional Formatting**

The script automatically applies:
- **Green background** (#d9ead3): Positive variance (under budget)
- **Red background** (#f4cccc): Negative variance (over budget)
- **White background**: Zero variance

### **This Sheet is Auto-Updated**

⚠️ **Warning**: Don't manually edit data in this sheet. It gets overwritten by automation.

To update data:
1. Edit **Forecast** sheet for forecast changes
2. Edit **PR Tracking** sheet for actual changes
3. Run: **🔄 Sync Forecast vs Actual**

---

## 4️⃣ Config Sheet

Auto-created by script. Structure:

### **Settings Section**

| Row | Column A | Column B | Column C |
|-----|----------|----------|----------|
| 1 | Setting | Value | Description |
| 2 | Current Period | `Q1M1` | Current forecast period |
| 3 | Current Year | `FY25` | Current fiscal year |
| 4 | Deadline Date | `2024-10-15` | Deadline (YYYY-MM-DD) |
| 5 | Reminder Enabled | `TRUE` | Enable reminders |
| 6 | Admin Email | `admin@company.com` | Admin email |

### **User List Section**

| Row | Column A | Column B | Column C |
|-----|----------|----------|----------|
| 8 | User Email List | Email | User Name |
| 9+ | *(blank)* | `user1@company.com` | User 1 Name |
| 10+ | *(blank)* | `user2@company.com` | User 2 Name |
| ... | ... | ... | ... |

### **Example**

```
Setting           | Value              | Description
Current Period    | Q2M1               | Current forecast period
Current Year      | FY25               | Current fiscal year
Deadline Date     | 2025-01-15         | Deadline for current period
Reminder Enabled  | TRUE               | Enable/disable reminders
Admin Email       | john.doe@tdg.com   | Email for notifications

User Email List   | Email                    | User Name
                  | alice@tdg.com            | Alice Smith
                  | bob@tdg.com              | Bob Johnson
                  | carol@tdg.com            | Carol Williams
```

---

## 5️⃣ User Status Sheet

Auto-created by script. Structure:

### **Layout**

| Row | Content |
|-----|---------|
| 1 | Title: "User Submission Status" (merged cells) |
| 2 | Period info: Period, Year, Deadline |
| 3 | Blank |
| 4 | Headers: User Name, Email, Submitted, Count, Status |
| 5+ | User data rows |

### **Example**

```
User Submission Status
Period: Q1M1    Year: FY25    Deadline: 2024-10-15

User Name       | Email                | Submitted | Count | Status
Alice Smith     | alice@tdg.com        | ✅        | 5     | Complete
Bob Johnson     | bob@tdg.com          | ❌        | 0     | PENDING
Carol Williams  | carol@tdg.com        | ✅        | 3     | Complete
```

### **Color Coding**

- **Complete**: Green background (#d9ead3)
- **PENDING**: Red background (#f4cccc)

---

## 6️⃣ Variance Report Sheet

Auto-created by script. Structure:

### **Sections**

1. **Title** (Row 1)
2. **Summary Statistics** (Rows 3-8)
3. **Variance by Category** (Starting ~row 10)
4. **Variance by User** (Below category section)

### **Example Layout**

```
Variance Report - Q1M1 FY25

Summary Statistics
Total Items          | 100    | Matched with PR | 85    | 85%
Over Budget          | 20     | Under Budget    | 65    |
Total Forecast       | 500000 | Total Actual    | 480000|
Total Variance       | 20000  | Accuracy Rate   | 96%   |

Variance by Category
Category        | Forecast | Actual  | Variance | Accuracy %
Equipment       | 200000   | 195000  | 5000     | 97.5%
Software        | 100000   | 98000   | 2000     | 98.0%
...

Variance by User
User                | Forecast | Actual  | Variance | Accuracy %
alice@tdg.com       | 50000    | 48000   | 2000     | 96.0%
bob@tdg.com         | 30000    | 32000   | -2000    | 93.3%
...
```

---

## 🎨 Formatting Guidelines

### **Overall Spreadsheet**

- **Theme**: Use Google Sheets default or corporate theme
- **Font**: Arial or Roboto, size 10-11
- **Grid lines**: On (default)

### **Color Scheme**

Suggested colors (matching Google's palette):

- **Headers**: Blue #4285f4, white text
- **Success/Positive**: Green #d9ead3
- **Warning/Negative**: Red #f4cccc
- **Section headers**: Green #34a853, white text
- **Alternate headers**: Yellow #fbbc04, dark text

### **Protection**

**Protect these ranges** (to prevent accidental edits):

1. **Forecast sheet, Column A**: Forecast ID (auto-generated)
2. **Forecast sheet, Columns Q-R**: User and Submitted (auto)
3. **PR Tracking, Column G**: Variance (formula)
4. **TDG : Forecast vs Actual**: Entire sheet (auto-updated)
5. **User Status**: Entire sheet (auto-updated)
6. **Variance Report**: Entire sheet (auto-updated)

**How to protect:**
1. Select range
2. Right-click > Protect range
3. Set permissions: "Show a warning when editing this range"
4. Or restrict to admins only

---

## 📏 Column Width Suggestions

### **Forecast Sheet**

- A (Forecast ID): 150px
- B-C (Quarter, Year): 80px
- D (No): 50px
- E (Date): 100px
- F-G (Group, Location): 120px
- H (Budgeted): 150px
- I (Prism): 100px
- J (Category): 150px
- K (Item): 150px
- L (Description): 300px
- M-P (Numbers): 100px
- Q (User): 200px
- R (Submitted): 150px

### **PR Tracking Sheet**

- A (Forecast ID): 150px
- B (PR Number): 120px
- C (Date): 100px
- D-E (Amounts): 100px
- F (Status): 100px
- G (Variance): 100px
- H (User): 200px

**Auto-resize all:** Select all > Double-click column divider

---

## 🔄 Data Flow Diagram

```
Users enter data
       ↓
Forecast Sheet (Manual entry)
       ↓
[Generate Forecast IDs] ← Admin runs
       ↓
Forecast IDs assigned
       ↓
Users raise PRs in system
       ↓
PR Tracking Sheet (Manual entry)
       ↓
[Sync Forecast vs Actual] ← Auto daily / Manual
       ↓
System matches by Forecast ID
       ↓
Forecast vs Actual Sheet (Auto-updated)
       ↓
[Generate Variance Report] ← Admin runs
       ↓
Variance Report Sheet (Auto-created)
       ↓
Management reviews
```

---

## ✅ Validation Checklist

Before activating automation, verify:

- [ ] **Forecast sheet** has columns A-R as defined
- [ ] **PR Tracking sheet** has columns A-H as defined
- [ ] **TDG : Forecast vs Actual** exists (can have any columns)
- [ ] **Header rows** are correct (row 1 for Forecast/PR, row 5 for FvA)
- [ ] **Sample data** entered for testing
- [ ] **Column names** match exactly (case-sensitive)
- [ ] **Data types** correct (numbers as numbers, not text)
- [ ] **No merged cells** in data area (headers OK)
- [ ] **Freeze panes** set for usability
- [ ] **Protection** applied to auto-generated columns

---

## 🛠️ Customization Notes

If your sheet structure is **different**:

1. **Update column mappings** in `Code.gs`:

```javascript
FORECAST_COLS: {
  ID: 1,              // Change to match your sheet
  QUARTER: 2,
  // ... etc
}
```

2. **Update header row** if not row 1:

```javascript
const headerRow = 5; // Your header row number
```

3. **Update sheet names** if different:

```javascript
const CONFIG = {
  FORECAST_SHEET: 'Your Sheet Name',
  // ... etc
}
```

4. **Test thoroughly** after changes:
   - Add test data
   - Run each function manually
   - Check outputs

---

## 📝 Migration from Existing Sheet

If you have an existing sheet:

### **Option 1: Adapt Your Sheet**

1. Rename tabs to match required names
2. Add missing columns
3. Rearrange columns to match structure
4. Test with sample data

### **Option 2: Create New Sheet**

1. Create new sheet with correct structure
2. Copy historical data from old sheet
3. Use `IMPORTRANGE()` if needed
4. Switch users to new sheet

### **Option 3: Modify the Script**

1. Update column mappings in code
2. Update sheet names in code
3. Test extensively

**Recommendation**: Option 1 (adapt your sheet) is usually easiest.

---

## 📊 Sample Template

Want a ready-to-use template?

### **Create Template:**

1. Make a copy of your current sheet
2. Delete all data (keep headers)
3. Add sample row in each sheet
4. Set up formatting and protection
5. Save as "TDG Forecast Template"
6. Use for future years

### **Using Template:**

1. **File > Make a copy**
2. Rename: "TDG Forecast FY26"
3. Update Config sheet with new year
4. Clear sample data
5. Ready to use

---

**Your sheet structure is now ready for automation! 🎉**

Questions? Check SETUP_INSTRUCTIONS.md or ADMIN_GUIDE.md.

---

*Last updated: 2024-11-10*






