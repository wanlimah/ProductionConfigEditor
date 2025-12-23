# 🚀 Setup Guide - TDG Forecast v2 (Column-Based Design)

## Overview

This is Version 2 of the TDG Forecast Automation with **individual columns for each period** instead of reusing the same columns.

---

## 📊 New Design Benefits

✅ **Historical Tracking** - See entire forecast evolution in one row  
✅ **No Data Overwriting** - Each period has its own column  
✅ **Easy Comparison** - Compare AOP 0.5 vs AOP 1 vs AOP Final vs Actuals side-by-side  
✅ **One Forecast ID per Item** - Not per period  
✅ **Variance Analysis** - Track how forecasts changed over time  

---

## 📋 Sheet Structure

### **Row 1: Period Deadlines** (Visual reference)
```
H: 11 July 2025    I: 11 July 2025   ...   T: 29 October 2025   ...
```

### **Row 2: Total Row** (Auto-calculated sums)
```
A: TOTAL    H: =SUM(H4:H)    I: =SUM(I4:I)   ...   AB: =SUM(AB4:AB)   ...
```
This row automatically calculates the sum of all quarterly/AOP columns.

### **Row 3: Column Headers**

**Core Info (A-G):**
- A: Forecast ID
- B: Requestor
- C: Category
- D: Project Name
- E: Item Name
- F: Description
- G: Remark

**AOP Round 0.5 (H-K) - Deadline: 11 July 2025**
- H: Q1
- I: Q2
- J: Q3
- K: Q4

**AOP Round 1 (L-O) - Deadline: 30 July 2025**
- L: Q1
- M: Q2
- N: Q3
- O: Q4

**AOP Final (P-S) - Deadline: Oct 2025**
- P: Q1
- Q: Q2
- R: Q3
- S: Q4

**Quarterly Forecasts (T-AA)**
- T: Q1M1 (29 Oct 2025)
- U: Q1M3 (20 Dec 2025)
- V: Q2M1 (20 Jan 2026)
- W: Q2M3 (20 Mar 2026)
- X: Q3M1 (20 Apr 2026)
- Y: Q3M3 (20 Jun 2026)
- Z: Q4M1 (20 Jul 2026)
- AA: Q4M3 (20 Sep 2026)

**Summary & Tracking (AB-AI)**
- AB: Total FY (Formula per row: `=SUM(T4:AA4)`)
- AC: Carry To
- AD: Status
- AE: PR #
- AF: PO #
- AG: Last Updated (Auto)
- AH: Updated By (Auto)
- AI: Change Log (Auto)

**Total: 35 columns (A-AI)**

**Row Structure:**
- Row 1: Period deadlines (visual reference)
- Row 2: TOTAL row (auto-sum of all data)
- Row 3: Column headers
- Row 4+: Data rows

---

## 🔧 Installation Steps

### **Step 1: Prepare Your Sheet**

1. **Create/Open your Google Sheet**
2. **Add these tabs:**
   - `Forecast` - Main data entry sheet
   - `Note` - Reference data (Requestors and Categories)
   - `Config` - System configuration (will be auto-created)

3. **Setup Forecast Sheet Structure:**
   - Row 1: Period deadlines (optional, for visual reference)
   - Row 2: TOTAL row (will be auto-created)
   - Row 3: Column headers as defined above
   - Row 4+: Data rows

### **Step 2: Setup Note Sheet** (Reference Data)

Create a sheet called `Note` with:

**Column A: Requestors**
```
Requestor
Chen, Weng Cheow
Kim, Hyeong Cheol
Koay, Choon Chin
Koh, Kuok Liang
Lai, Yu Wai
Lam, Tuck Hon
Lim, Kwok Hau Ronald
Low, Jeen Jang
Low, Say Aun
Mah, Wan Ling
Ng, Keng Shan
Ong, Chee On
Ong, Roey Jiea
Tan, Ee Tieng
Tan, Tai Seng
Wong, Chia Yeak
Wong, Chor Ming
```

**Column B: Categories**
```
Category
3A) Components (TDG)
4A) Computer/Software (TDG)
8A) Equipment, cal, and repairs (TDG)
13D) Freight (DHL DGF)
16A) Other Consumables (TDG)
18A) Processing Supplies (TDG)
19A) Substrates (TDG)
21A) Test hardware (TDG)
22A) Tooling (TDG)
```

### **Step 3: Install the Script**

1. **Open Apps Script:**
   - Go to: `Extensions > Apps Script`

2. **Copy the Code:**
   - Open `Code_v2_ColumnBased.gs` from your ForecastAutomation folder
   - Copy ALL the code (Ctrl+A, Ctrl+C)

3. **Paste into Apps Script:**
   - Delete any existing code in the editor
   - Paste the copied code (Ctrl+V)
   - Save: Click 💾 or Ctrl+S
   - Name it: "TDG Forecast Automation v2"

4. **Close Apps Script tab**

### **Step 4: Initial Authorization**

1. **Refresh your Google Sheet** (F5)

2. **New menu appears:**
   - `📊 TDG Forecast (v2)`

3. **Click:** `📊 TDG Forecast (v2) > 📋 Initialize Config`

4. **Authorize the script:**
   - Click `Continue`
   - Choose your account
   - Click `Advanced`
   - Click `Go to TDG Forecast Automation v2 (unsafe)`
   - Click `Allow`

5. **Config sheet created**
   - New tab: `Config`

### **Step 5: Configure Settings**

Go to the **Config** sheet and update:

| Setting | Example Value | Description |
|---------|---------------|-------------|
| **Current Fiscal Year** | `FY25` | Your fiscal year |
| **Active Period** | `Q1M1` | Period you're currently collecting |
| **Active Period Deadline** | `2025-10-29` | Deadline (YYYY-MM-DD format) |
| **Reminder Enabled** | `TRUE` | Enable email reminders |
| **Admin Email** | `your.email@company.com` | Your email for summaries |
| **Auto Fill Carry To** | `TRUE` | Auto-populate Carry To amount |

### **Step 6: Setup Total Row**

1. **Click menu:** `📊 TDG Forecast (v2) > 🔢 Setup Total Row`

2. **This creates:**
   - Row 2 label: "TOTAL"
   - SUM formulas for all AOP columns (H-K, L-O, P-S)
   - SUM formulas for all Quarterly columns (T-AA)
   - SUM formulas for Total FY and Carry To (AB-AC)
   - Yellow background formatting
   - Bold text with currency formatting

3. **Confirm:** You should see "✅ Total row setup complete!"

### **Step 7: Setup Data Validation**

1. **Click menu:** `📊 TDG Forecast (v2) > ⚙️ Setup Data Validation`

2. **This creates dropdowns for:**
   - Column B: Requestor (17 users)
   - Column C: Category (9 categories)
   - Column AD: Status (9 status values)

3. **Confirm:** You should see "✅ Data validation setup complete!"

### **Step 8: Test Auto-Generated Forecast ID**

The system now **automatically generates Forecast IDs** when you enter data!

1. **Go to Forecast sheet, Row 4** (Row 1 = deadlines, Row 2 = totals, Row 3 = headers)

2. **Enter data in any order** (try this):
   - **B (Row 4):** Select a Requestor from dropdown (e.g., Wong, Chor Ming)
   - **C (Row 4):** Select a Category from dropdown (e.g., 4A) Computer/Software)
   
3. **Enter Item Name:**
   - **E (Row 4):** Type: Test Software License
   - ✨ **Watch Column A Row 4!** Forecast ID should auto-generate: `FY25-001`
   - ✨ **Watch Row 2!** Total row should automatically update as you enter amounts

4. **Add more test data:**
   - **D:** Test Project
   - **H:** 1000 (AOP 0.5 Q1)
   - **T:** 1100 (Q1M1)

**How Auto-Generation Works:**

The system generates ID when you fill:
- **(Requestor OR Category)** - at least one identifier
- **AND (Item Name OR any cost column)** - at least one content

**Example scenarios:**

✅ **Triggers ID generation:**
- Enter Requestor → Enter Item Name → ID generated!
- Enter Category → Enter Q1M1 cost → ID generated!
- Enter Requestor + Category → Enter AOP 0.5 Q1 → ID generated!

❌ **Does NOT trigger:**
- Only Requestor entered (no content yet)
- Only Item Name entered (no identifier yet)
- Only Project Name entered (not a key trigger field)

**Manual generation option:**
- Menu: `📊 TDG Forecast (v2) > 🎯 Generate Forecast IDs`
- Use for bulk generation of existing rows without IDs

### **Step 9: Setup Automated Triggers**

1. **Click menu:** `📊 TDG Forecast (v2) > ⏰ Setup Automated Triggers`

2. **This creates:**
   - **onEdit trigger** - Tracks all changes automatically
   - **7am daily** - Sends reminder emails
   - **8am daily** - Updates Open Items Dashboard

3. **Verify triggers:**
   - Go to: `Extensions > Apps Script`
   - Click: ⏰ Triggers (left sidebar)
   - Should see 3 triggers

### **Step 10: Test Features**

**Test Open Items Dashboard:**
- Menu: `📊 TDG Forecast (v2) > 🔄 Update Open Items Dashboard`
- New sheet `Open Items Dashboard` appears
- Shows summary by Status, Requestor, Category, and Incomplete Periods

**Test Variance Analysis:**
- Menu: `📊 TDG Forecast (v2) > 📊 Generate Variance Analysis`
- New sheet `Variance Analysis` appears
- Shows how forecasts changed across periods

**Test Change Tracking:**
- Edit any cell in your test row (e.g., change Q1M1 from 1100 to 1200)
- Column AG (Last Updated) auto-fills with timestamp
- Column AH (Updated By) auto-fills with your email
- Column AI (Change Log) records the change
- **Row 2 (Total)** automatically updates to reflect the new value

**Test Auto-Fill Carry To:**
- Change Status (Column AD) to: `Carry To Next FY`
- Column AC (Carry To) should auto-fill with Total FY amount
- (if Auto Fill Carry To = TRUE in Config)

### **Step 11: Clean Up Test Data**

- Delete row 4 (test data)
- Keep Row 1 (deadlines), Row 2 (totals), Row 3 (headers)
- Sheet is ready for real use!

---

## 📧 Email Reminder System

### **How It Works:**

1. **System checks daily at 7am:**
   - Gets "Active Period" from Config
   - Finds users with empty cells in that period column
   - Sends reminders if deadline is in 7, 3, 1, or 0 days

2. **Users receive email:**
```
Subject: [TDG Forecast] ⚠️ Reminder - Q1M1 Due Oct 29, 2025

Hi,

⚠️ This is a reminder to update your Q1M1 forecast.

📅 Deadline: October 29, 2025
⏳ Time Remaining: 3 day(s)

You have 5 item(s) that need Q1M1 forecasts:

1. FY25-015 | 13D) Freight | DHL DGF
2. FY25-022 | 18A) Processing Supplies | Desiccator N2
3. FY25-033 | 4A) Computer/Software | Software
4. FY25-041 | 4A) Computer/Software | Asset Management
5. FY25-055 | 21A) Test hardware | Test Equipment

📊 Please update the forecast sheet:
[Sheet URL]
```

3. **Admin receives summary:**
   - Total users with incomplete items
   - Total incomplete items
   - List of users and item counts

---

## 🎯 Key Features

### **1. Auto-Generated Forecast ID**

**Triggers automatically when you enter:**
- Requestor OR Category (at least one)
- AND Item Name OR any cost value

**How it works:**
```
User enters:
- Requestor: Wong, Chor Ming
- Category: 4A) Computer/Software
- Item Name: Software License

→ System immediately generates: FY25-001
→ No need to run menu item manually!
```

**ID Format:**
- One item = One row = One Forecast ID
- Format: `FY25-001`, `FY25-002`, etc.
- ID stays the same across all periods
- Track item's forecast evolution over time

**Manual generation still available:**
- Menu: `🎯 Generate Forecast IDs`
- Use for bulk generation or if auto-generation missed any rows

### **2. Period-Specific Reminders**

- System knows which period is active (from Config)
- Only reminds users about empty cells in that period column
- Sends reminders: 7 days, 3 days, 1 day, day of deadline
- Lists specific items that need updates

### **3. Auto-Fill Carry To**

When Status = `Carry To Next FY`:
- System auto-fills Carry To amount (Column AC)
- Uses Total FY value (Column AB)
- User can manually adjust if needed

### **4. Change Tracking**

Every edit automatically logs:
- When: Timestamp
- What: Column name and old/new values
- Who: User email
- Stored in Change Log column (AI)

Example log:
```
2025-10-15 10:30: Q1M1 changed from "1000" to "1100" by wong.chor.ming@company.com
2025-10-28 14:20: Status changed from "Active" to "In Progress" by wong.chor.ming@company.com
```

### **5. Open Items Dashboard**

Auto-generated summary showing:

**By Status:**
| Status | Count | Total Amount |
|--------|-------|--------------|
| Active | 45 | $250,000 |
| In Progress | 12 | $85,000 |
| Carry To Next FY | 8 | $40,000 |

**By Requestor:**
| Requestor | Active | Carry Forward | Total $ |
|-----------|--------|---------------|---------|
| Wong, Chor Ming | 12 | 2 | $65,000 |

**By Category:**
| Category | Count | Amount |
|----------|-------|--------|
| 4A) Computer/Software | 15 | $120,000 |

**Incomplete Periods:**
| Period | Missing | Users Affected |
|--------|---------|----------------|
| Q1M1 | 23 | 8 |

### **6. Variance Analysis**

Compare forecast evolution:
- AOP 0.5 Total vs AOP 1 Total (% change)
- AOP 1 Total vs AOP Final Total (% change)
- AOP Final Total vs Quarterly Total (actual variance)
- Identify items with significant changes (>20%)

---

## 📋 Daily Operations

### **For Users:**

1. **Enter forecasts:**
   - Open Forecast sheet
   - Find your items (or add new rows)
   - Fill in the period column(s) you're responsible for
   - System auto-tracks changes

2. **When raising PR:**
   - Update PR # column (AE)
   - Update Status to "In Progress"

3. **When PO received:**
   - Update PO # column (AF)
   - Update Status to "Completed"

### **For Admins:**

**Daily (5 minutes):**
- Check Open Items Dashboard
- Review incomplete items
- Follow up with stragglers

**Weekly (15 minutes):**
- Update Active Period in Config (when transitioning)
- Update Active Period Deadline
- Run Variance Analysis

**Monthly (30 minutes):**
- Generate Variance Analysis for completed periods
- Share reports with management
- Update Note sheet (new users/categories)

---

## 🔄 Period Transitions

When moving to next period:

1. **Update Config sheet:**
   - Row 3: Active Period = `Q1M3` (new period)
   - Row 4: Active Period Deadline = `2025-12-20` (new deadline)

2. **System automatically:**
   - Sends reminders for new period column
   - Tracks completion for new period
   - Updates dashboards

3. **Old periods:**
   - Data stays in their columns
   - Historical record maintained
   - Can compare anytime

---

## 🐛 Troubleshooting

### **Issue: Forecast IDs not generating**

**Check:**
- Row has data in Requestor or Category
- Column A is empty (no manual entry)
- You're starting from Row 4 (Rows 1-3 are deadlines, totals, headers)

**Solution:**
- Clear Column A completely
- Add data to Row 3+
- Run: Generate Forecast IDs

### **Issue: Dropdowns not working**

**Solution:**
- Run: Setup Data Validation
- Check Note sheet has Requestors (Column A) and Categories (Column B)
- Verify data starts from Row 2 (Row 1 = header)

### **Issue: Total row not calculating**

**Check:**
- Row 2 exists and has formulas
- Formulas reference Row 4 onwards (e.g., =SUM(H4:H))
- Data is entered in Row 4 or below

**Solution:**
- Run: Setup Total Row
- Verify formulas are present in Row 2
- Check that data rows start from Row 4

### **Issue: Reminders not sending**

**Check:**
- Config: Reminder Enabled = TRUE
- Config: Active Period is correct
- Config: Active Period Deadline is correct format (YYYY-MM-DD)
- Triggers are setup (Apps Script > Triggers)

**Solution:**
- Re-run: Setup Automated Triggers
- Check spam/junk folder
- Verify user emails match Requestor names

### **Issue: Change Log not recording**

**Check:**
- Triggers are setup (should have onEdit trigger)

**Solution:**
- Re-run: Setup Automated Triggers
- Make a test edit
- Check Column AI (Change Log)

### **Issue: Carry To not auto-filling**

**Check:**
- Config: Auto Fill Carry To = TRUE
- Status changed to exactly: `Carry To Next FY`
- Total FY (Column AB) has a value

**Solution:**
- Manually enter amount if needed
- Or change status again to trigger

---

## ✅ Setup Checklist

Before going live:

- [ ] Forecast sheet has correct structure (columns A-AI)
- [ ] Row 1: Period deadlines (optional)
- [ ] Row 2: Total row with SUM formulas
- [ ] Row 3: Column headers
- [ ] Row 4+: Data entry area
- [ ] Note sheet has all Requestors and Categories
- [ ] Config sheet is filled out
- [ ] Total row setup complete (Row 2 shows sums)
- [ ] Data validation setup (dropdowns work)
- [ ] Triggers are active (3 triggers in Apps Script)
- [ ] Test Forecast ID generation works
- [ ] Test Open Items Dashboard generates
- [ ] Test Variance Analysis generates
- [ ] Test change tracking works
- [ ] Test total row updates automatically
- [ ] Test email reminders work (or wait for scheduled time)
- [ ] All users have Edit access to sheet
- [ ] Admin has reviewed all settings

---

## 📞 Support

**Questions?**
- Check this guide
- Review Code_v2_ColumnBased.gs comments
- Check Apps Script execution logs (Apps Script > Executions)

**After buyoff and lockdown, we'll create:**
- User Quick Guide (simplified for requestors)
- Admin Operations Manual (daily tasks)
- Training materials

---

**Ready to use! 🎉**

*Version 2.0 | Created: 2024-11-10*

