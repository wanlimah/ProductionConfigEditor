# 📊 TDG Forecast vs Actual Automation - Setup Guide

## Overview

This automation system helps manage your quarterly forecast process with:
- ✅ Auto-generated Forecast IDs for tracking
- ✅ Automated reminders (5 days, 1 day, 7am on deadline)
- ✅ Auto-matching of Forecast vs PR Actuals
- ✅ User submission tracking across 20 users and 12 categories
- ✅ Variance reporting and analysis
- ✅ Special handling for extended forecast periods (Q2M3, Q3M1, Q3M3)

---

## 🚀 Quick Start (Step-by-Step)

### **Step 1: Open Your Google Sheet**

1. Open your existing Google Sheet: [TDG : Forecast vs Actual](https://docs.google.com/spreadsheets/d/1JuSuO53Azd-Kt5ekSACM8eAX5K8phFkFeJB_8PLhKMw/)
2. Make sure you have **edit permissions**

### **Step 2: Install the Script**

1. In your Google Sheet, go to: **Extensions > Apps Script**
2. Delete any existing code in the editor
3. Copy the entire contents of `Code.gs` (provided)
4. Paste it into the Apps Script editor
5. Click **Save** (disk icon) and give it a name: "TDG Forecast Automation"
6. Close the Apps Script tab

### **Step 3: Initial Setup**

1. **Refresh your Google Sheet** (press F5 or Cmd/Ctrl+R)
2. You'll see a new menu: **📊 TDG Forecast Automation**
3. Click: **📊 TDG Forecast Automation > 📋 Initialize Config Sheet**
4. **Authorize the script** (first time only):
   - Click "Continue"
   - Choose your Google account
   - Click "Advanced" → "Go to TDG Forecast Automation (unsafe)"
   - Click "Allow"
5. A new sheet called **"Config"** will be created

### **Step 4: Configure Settings**

Go to the **Config** sheet and update these values:

| Setting | What to Enter | Example |
|---------|---------------|---------|
| **Current Period** | Current forecast period | `Q1M1` or `Q2M3` |
| **Current Year** | Fiscal year | `FY25` |
| **Deadline Date** | Submission deadline | `2024-10-15` (YYYY-MM-DD format) |
| **Reminder Enabled** | Enable reminders? | `TRUE` or `FALSE` |
| **Admin Email** | Your email for summaries | `admin@company.com` |

### **Step 5: Add User Emails**

In the **Config** sheet, starting from row 9:

| User Email List | Email | User Name |
|----------------|-------|-----------|
| | user1@company.com | John Doe |
| | user2@company.com | Jane Smith |
| | ... | ... |

Add all **20 users** who need to submit forecasts.

### **Step 6: Prepare Your Sheets**

Make sure your Google Sheet has these tabs (or rename them):

1. **Forecast** - Where users enter forecasts
2. **PR Tracking** - Where PRs are recorded
3. **TDG : Forecast vs Actual** - Comparison sheet (you already have this)

**Important Column Setup for "Forecast" Sheet:**

| Column | Header | Purpose |
|--------|--------|---------|
| A | Forecast ID | Auto-generated (leave empty) |
| B | Quarter | e.g., Q1, Q2, Q3, Q4 |
| C | Year | e.g., FY25 |
| D | No | Sequential number |
| E | Date | Entry date |
| F | Epicentech Group | Department/group |
| G | Location | Location |
| H | Budgeted or Unbudgeted | Status |
| I | Prism | Prism code |
| J | Epicentech Category | 1 of 12 categories |
| K | Item (Simple) | Item description |
| L | Description | Detailed description |
| M | Forecast (M1) | M1 forecast amount |
| N | Unbudgeted | Unbudgeted amount |
| O | Forecast (M3) | M3 forecast amount |
| P | Forecast Amount | Final amount |
| Q | User | User email (auto) |
| R | Submitted | Timestamp (auto) |

**Important Column Setup for "PR Tracking" Sheet:**

| Column | Header | Purpose |
|--------|--------|---------|
| A | Forecast ID | Reference to forecast (dropdown) |
| B | PR Number | Purchase requisition number |
| C | PR Date | Date PR was raised |
| D | Actual Amount | Actual amount in PR |
| E | Received Amount | Amount received |
| F | PO Status | Purchase order status |
| G | Variance | Auto-calculated |
| H | User | User who raised PR |

### **Step 7: Test the System**

1. **Generate Test Forecast IDs:**
   - Add a few test rows in the **Forecast** sheet (fill columns B-P)
   - Go to: **📊 TDG Forecast Automation > 🎯 Generate Forecast IDs**
   - Column A should now have IDs like: `FY25-Q1M1-001`, `FY25-Q1M1-002`

2. **Test User Status Tracking:**
   - Go to: **📊 TDG Forecast Automation > 👥 Update User Submission Status**
   - A new sheet **"User Status"** will be created showing who has submitted

3. **Test Manual Reminder:**
   - Go to: **📊 TDG Forecast Automation > 📧 Send Reminders Now**
   - Users who haven't submitted will receive emails

### **Step 8: Setup Automated Triggers**

To enable automatic daily reminders and syncing:

1. Go to: **📊 TDG Forecast Automation > ⚙️ Setup Time-based Triggers**
2. This creates:
   - **7am daily**: Check deadlines and send reminders
   - **8am daily**: Sync forecast vs actual data
   - **9am daily**: Update user submission status

---

## 📋 Daily Workflow

### **For Users (Forecast Submission):**

1. **Enter Forecast Data:**
   - Open the **Forecast** sheet
   - Fill in your forecast details (columns B-P)
   - Leave column A (Forecast ID) **empty** - it auto-generates

2. **Forecast ID Generation:**
   - IDs are generated automatically when you run the menu item
   - Or an admin can run: **🎯 Generate Forecast IDs** daily
   - Format: `FY25-Q1M1-001`, `FY25-Q1M1-002`, etc.

3. **When Raising PR:**
   - Go to **PR Tracking** sheet
   - In column A, enter the **Forecast ID** from your forecast
   - Fill in PR details (PR Number, Date, Amount, etc.)

### **For Admins:**

**Weekly Tasks:**
1. **Monday**: Update Config sheet with current period and deadline
2. **Daily**: System sends automatic reminders
3. **Check Status**: View **User Status** sheet to see who hasn't submitted
4. **Before Deadline**: Run **📧 Send Reminders Now** for manual nudge

**After Deadline:**
1. **Lock Period**: Update Config to next period
2. **Generate Reports**: **📊 Generate Variance Report**
3. **Sync Data**: **🔄 Sync Forecast vs Actual** (auto-runs daily at 8am)

---

## 🔄 Period Transitions

### **When Moving to Next Period (e.g., Q1M1 → Q1M3):**

1. Go to **Config** sheet
2. Update **Current Period** from `Q1M1` to `Q1M3`
3. Update **Deadline Date** to new deadline
4. Old data remains in sheets (historical archive)
5. New forecasts will get new IDs: `FY25-Q1M3-001`, `FY25-Q1M3-002`

### **Special Periods (Q2M3, Q3M1, Q3M3):**

These periods require **extended forecasting** (Q3M1 through Q4M1):
- Users need to forecast further ahead
- System tracks cumulative data across quarters
- Just set the period in Config - system handles the rest

### **Period Calendar:**

| Period | Timing | Type | Config Setting |
|--------|--------|------|----------------|
| Q1M1 | Mid-Oct | Forecast | `Q1M1` |
| Q1M3 | Mid-Dec | Confirm | `Q1M3` |
| Q2M1 | Mid-Jan | Forecast | `Q2M1` |
| Q2M3 | Mid-Mar | Confirm + Extended | `Q2M3` |
| Q3M1 | Mid-Apr | Forecast + Extended | `Q3M1` |
| Q3M3 | Mid-Jun | Confirm + Extended | `Q3M3` |
| Q4M1 | Mid-Jul | Forecast | `Q4M1` |
| Q4M3 | Mid-Sep | Confirm | `Q4M3` |

---

## 📧 Email Reminders

### **When Reminders Are Sent:**

The system automatically sends reminders at:
1. **5 days before deadline** - First reminder
2. **1 day before deadline** - Urgent reminder
3. **7am on deadline day** - Final reminder

### **Who Receives Reminders:**

- Only users who **haven't submitted** any forecast for current period
- User submission is tracked by email in column Q of Forecast sheet

### **Email Content:**

Users receive emails with:
- Urgency indicator (⚠️ / ⏰ / 🚨)
- Days remaining until deadline
- Direct link to Google Sheet
- Instructions for submission
- Admin contact for questions

### **Admin Summary:**

Admin receives a daily summary email with:
- Total pending users
- List of who hasn't submitted
- Link to detailed status sheet

---

## 🎯 Key Features Explained

### **1. Auto-Generated Forecast IDs**

**Purpose:** Track each forecast through to PR

**How it works:**
- System generates unique IDs: `FY25-Q1M1-001`
- Format: `{Year}-{Period}-{Sequential Number}`
- Numbers auto-increment (001, 002, 003...)
- Users reference this ID when raising PR

**Benefits:**
- No manual numbering needed
- Perfect traceability from forecast → PR
- Easy to match forecast vs actual

### **2. Forecast vs Actual Matching**

**How Matching Works:**
1. System reads all forecasts from **Forecast** sheet
2. System reads all PRs from **PR Tracking** sheet
3. Matches by **Forecast ID**
4. Calculates variance: `Forecast Amount - Actual Amount`
5. Updates **Forecast vs Actual** sheet with results

**Run Matching:**
- **Manual**: Menu → **🔄 Sync Forecast vs Actual**
- **Automatic**: Runs daily at 8am

### **3. User Submission Tracking**

**Tracks:**
- Who has submitted forecast for current period
- How many forecasts each user submitted
- Status: Complete ✅ or PENDING ❌

**View Status:**
- Check **User Status** sheet
- Green = submitted, Red = pending

### **4. Variance Reporting**

**Generate Report:**
- Menu → **📊 Generate Variance Report**

**Report Includes:**
- Total forecast accuracy percentage
- Over/under budget counts
- Variance by category (all 12 categories)
- Variance by user (all 20 users)
- Color-coded for easy reading

---

## 🛠️ Customization

### **Adjust Reminder Days**

Edit in `Code.gs`:

```javascript
REMINDER_DAYS: [5, 1, 0], // Change to [7, 3, 1, 0] for more reminders
```

### **Change Reminder Time**

Edit in `Code.gs`:

```javascript
REMINDER_TIME_HOUR: 7 // Change to 9 for 9am reminders
```

### **Adjust Column Positions**

If your sheet columns are different, update in `Code.gs`:

```javascript
FORECAST_COLS: {
  ID: 1,        // Column A
  QUARTER: 2,   // Column B
  // ... adjust as needed
}
```

### **Custom Email Template**

Edit the `sendReminderEmail` function in `Code.gs` to customize email content.

---

## 🐛 Troubleshooting

### **Problem: Menu doesn't appear**

**Solution:**
1. Refresh the page (F5)
2. Close and reopen the sheet
3. Check if script is saved in Apps Script editor

### **Problem: "Config sheet not found" error**

**Solution:**
1. Run: **📋 Initialize Config Sheet**
2. Check the sheet is named exactly "Config"

### **Problem: Forecast IDs not generating**

**Check:**
1. Column A is empty (don't manually enter IDs)
2. Columns B (Quarter) and C (Year) have values
3. You've saved data before running **🎯 Generate Forecast IDs**

### **Problem: Emails not sending**

**Check:**
1. Config sheet: **Reminder Enabled** = `TRUE`
2. User emails are correctly entered in Config sheet
3. You've authorized the script (see Step 3)
4. Check spam folder

### **Problem: Forecast vs Actual not matching**

**Check:**
1. PR sheet has correct **Forecast ID** in column A
2. Forecast ID format matches (e.g., `FY25-Q1M1-001`)
3. No extra spaces in Forecast ID
4. Run: **🔄 Sync Forecast vs Actual** manually

### **Problem: Permission denied**

**Solution:**
1. Go to: **Extensions > Apps Script**
2. Click: **Run > Run function > testSetup**
3. Re-authorize the script
4. Allow all permissions

---

## 📊 Sample Data Template

### **Forecast Sheet Example:**

| Forecast ID | Quarter | Year | No | Date | ... | Forecast Amount | User |
|-------------|---------|------|----|------|-----|-----------------|------|
| FY25-Q1M1-001 | Q1 | FY25 | 1 | 2024-10-01 | ... | 50000 | user1@company.com |
| FY25-Q1M1-002 | Q1 | FY25 | 2 | 2024-10-01 | ... | 30000 | user2@company.com |

### **PR Tracking Sheet Example:**

| Forecast ID | PR Number | PR Date | Actual Amount | ... |
|-------------|-----------|---------|---------------|-----|
| FY25-Q1M1-001 | PR-2024-001 | 2024-10-15 | 48000 | ... |
| FY25-Q1M1-002 | PR-2024-002 | 2024-10-16 | 32000 | ... |

---

## 🔐 Security & Permissions

### **What Permissions Are Needed:**

The script requires:
- ✅ **Read/write access** to your Google Sheet
- ✅ **Send email** on your behalf
- ✅ **Time-based triggers** for automation

### **Data Privacy:**

- All data stays in **your Google Sheet**
- No data sent to external servers
- Emails sent through your Google account
- Only authorized users can access

### **Who Can Run the Script:**

- Anyone with **edit permissions** on the sheet
- Recommended: Limit edit access to admins + forecast submitters

---

## 📞 Support & Maintenance

### **Monthly Maintenance:**

1. **Check triggers are running:**
   - Apps Script → Triggers → Verify 3 triggers exist
2. **Review Config:**
   - Update period and deadline for next cycle
3. **Archive old data:**
   - Old forecasts stay in sheets as historical record
4. **User list update:**
   - Add/remove users in Config sheet as needed

### **If Something Breaks:**

1. Check **Apps Script > Executions** for error logs
2. Verify Config sheet settings
3. Re-run: **⚙️ Setup Time-based Triggers**
4. Contact your Google Workspace admin

---

## ✅ Checklist: Is Your System Ready?

- [ ] Script installed in Apps Script editor
- [ ] Config sheet initialized and filled out
- [ ] All 20 user emails added to Config
- [ ] Current period and deadline set
- [ ] Admin email configured
- [ ] Forecast sheet has correct column headers
- [ ] PR Tracking sheet has correct column headers
- [ ] Test forecasts created and IDs generated
- [ ] Manual reminder test successful
- [ ] Time-based triggers setup
- [ ] Users informed about new system
- [ ] Admin trained on daily tasks

---

## 🚀 Next Steps

1. **Week 1**: Test with small group (3-5 users)
2. **Week 2**: Roll out to all 20 users
3. **Week 3**: Monitor and adjust
4. **Week 4**: Full production use

**Ready to go live? Let's automate! 🎉**

---

## 📝 Version History

- **v1.0** (2024-11-10): Initial release
  - Auto-generated Forecast IDs
  - Email reminders (5d, 1d, 7am)
  - Forecast vs Actual matching
  - User submission tracking
  - Variance reporting
  - Special period handling

---

**Questions?** Update the Config sheet or check the Troubleshooting section above.

**Need customization?** Edit `Code.gs` in the Apps Script editor.

**Happy forecasting! 📊**

