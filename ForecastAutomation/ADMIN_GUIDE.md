# 🔧 Admin Guide - TDG Forecast Automation System

## Overview

This guide is for **administrators** who manage the forecast automation system.

---

## 👨‍💼 Admin Responsibilities

### **Daily Tasks (5 minutes)**
- [ ] Check **User Status** sheet for pending submissions
- [ ] Monitor email reminders are being sent
- [ ] Review any user questions or issues

### **Weekly Tasks (15 minutes)**
- [ ] Update **Config** sheet with current period/deadline
- [ ] Run **Generate Forecast IDs** for new submissions
- [ ] Send manual reminders if needed
- [ ] Review submission progress

### **Monthly Tasks (30 minutes)**
- [ ] Generate **Variance Report** for completed period
- [ ] Archive old period data (if needed)
- [ ] Update user list in Config (new hires/departures)
- [ ] Review and adjust automation settings

### **Quarterly Tasks (1 hour)**
- [ ] Analyze forecast accuracy trends
- [ ] Share variance reports with management
- [ ] Plan improvements for next quarter
- [ ] Update period calendar

---

## ⚙️ Admin Menu Functions

Access via: **📊 TDG Forecast Automation** menu

### **🎯 Generate Forecast IDs**
**When to use:** Daily, after users add new forecasts  
**What it does:** Auto-generates unique IDs for forecasts without IDs  
**Time:** ~5 seconds

**Steps:**
1. Click menu item
2. System scans Forecast sheet
3. Generates IDs like: `FY25-Q1M1-001`, `FY25-Q1M1-002`
4. Shows confirmation: "Generated X new Forecast IDs"

**Troubleshooting:**
- If no IDs generated, check that columns B (Quarter) and C (Year) have values
- IDs only generate for rows that don't already have an ID

---

### **🔄 Sync Forecast vs Actual**
**When to use:** Daily, or before generating reports  
**What it does:** Matches forecasts with PRs, calculates variances  
**Time:** ~10-30 seconds (depending on data volume)

**Steps:**
1. Click menu item
2. System reads Forecast sheet
3. System reads PR Tracking sheet
4. Matches by Forecast ID
5. Updates Forecast vs Actual sheet
6. Shows summary: "Synced X records, Matched: X, Unmatched: X"

**What gets updated:**
- Actual amounts from PRs
- Received amounts
- Variance calculations
- Variance percentages
- Conditional formatting (red/green highlighting)

**Troubleshooting:**
- If matches are low, check Forecast IDs in PR sheet match exactly
- No extra spaces or typos in Forecast IDs
- Case-sensitive matching

---

### **📧 Send Reminders Now**
**When to use:** Manual reminder before deadline, or test emails  
**What it does:** Immediately sends reminders to users who haven't submitted  
**Time:** ~5-15 seconds per user

**Steps:**
1. Click menu item
2. System checks who has submitted
3. Sends emails to pending users
4. Sends admin summary email to you
5. Shows confirmation: "Sent X reminder emails"

**Email content includes:**
- Urgency indicator (⚠️/⏰/🚨)
- Days until deadline
- Direct link to sheet
- Instructions

**Troubleshooting:**
- Check Config: **Reminder Enabled** = TRUE
- Verify user emails are correct in Config
- Check spam folder if users don't receive
- Daily quota: 100 emails/day (Google limit)

---

### **👥 Update User Submission Status**
**When to use:** Daily, to check progress  
**What it does:** Creates/updates User Status sheet showing who submitted  
**Time:** ~5 seconds

**Steps:**
1. Click menu item
2. System scans Forecast sheet for current period
3. Checks each user from Config
4. Creates User Status sheet with:
   - User name and email
   - Submitted status (✅ or ❌)
   - Count of forecasts submitted
   - Color-coded status (green/red)

**Use this to:**
- Identify who needs follow-up
- Track submission progress
- Share status in meetings

---

### **📊 Generate Variance Report**
**When to use:** After deadline, monthly reporting  
**What it does:** Comprehensive analysis of forecast accuracy  
**Time:** ~10-20 seconds

**Steps:**
1. Click menu item
2. System analyzes Forecast vs Actual data
3. Creates "Variance Report" sheet with:
   - **Summary statistics**:
     - Total items, matched count
     - Over/under budget counts
     - Total forecast vs actual
     - Overall accuracy rate
   - **Variance by category** (all 12 categories):
     - Forecast vs actual by category
     - Category-level accuracy
   - **Variance by user** (all 20 users):
     - Individual user accuracy
     - User-level performance

**Use this for:**
- Management reporting
- Identifying improvement areas
- User performance tracking
- Budget accuracy trends

---

### **⚙️ Setup Time-based Triggers**
**When to use:** Initial setup, or if triggers break  
**What it does:** Configures automatic daily execution  
**Time:** ~2 seconds

**Creates 3 triggers:**
1. **7am daily**: Send automated reminders
2. **8am daily**: Sync forecast vs actual
3. **9am daily**: Update user submission status

**Steps:**
1. Click menu item
2. System deletes old triggers (if any)
3. Creates new triggers
4. Shows confirmation

**Verify triggers:**
1. Go to: **Extensions > Apps Script**
2. Click: **⏰ Triggers** (left sidebar)
3. You should see 3 triggers listed

**Troubleshooting:**
- If triggers don't run, check authorization
- Re-run "Setup Time-based Triggers"
- Check executions: **Apps Script > Executions** for errors

---

### **📋 Initialize Config Sheet**
**When to use:** Initial setup only  
**What it does:** Creates Config sheet with template  
**Time:** ~1 second

**Creates:**
- Configuration table with settings
- User email list template
- Formatted and color-coded

**Only run once** - overwrites existing Config if run again

---

## 📋 Config Sheet Management

### **Critical Settings**

| Setting | Purpose | Format | Example |
|---------|---------|--------|---------|
| **Current Period** | Active forecast period | `Q{1-4}M{1,3}` | `Q1M1` |
| **Current Year** | Fiscal year | `FY{YY}` | `FY25` |
| **Deadline Date** | Submission deadline | `YYYY-MM-DD` | `2024-10-15` |
| **Reminder Enabled** | Enable/disable emails | `TRUE` or `FALSE` | `TRUE` |
| **Admin Email** | Your email for summaries | Valid email | `admin@company.com` |

### **Updating for New Period**

**Example: Transitioning Q1M1 → Q1M3**

1. Open **Config** sheet
2. Change **Current Period**: `Q1M1` → `Q1M3`
3. Change **Deadline Date**: Update to Q1M3 deadline
4. Save (auto-saves)

**What happens:**
- New forecasts get IDs: `FY25-Q1M3-001`, `FY25-Q1M3-002`
- Old Q1M1 data remains in sheets (archived)
- Reminders reference new deadline
- User Status tracks Q1M3 submissions

### **Managing User List**

**Adding a user:**
1. Go to Config sheet, row 9+
2. Add row: `user@company.com` | `User Name`

**Removing a user:**
1. Delete the row, or
2. Clear the email (keep row for spacing)

**Best practice:**
- Keep list alphabetical
- Use full names
- Verify emails are correct
- Update when people leave/join

---

## 🔔 Email Automation Deep Dive

### **Reminder Logic**

The system sends reminders based on:
1. **Days until deadline** (5, 1, 0 days)
2. **User submission status** (submitted = no reminder)
3. **Reminder enabled** (Config setting)

### **Daily Automation Flow**

**7:00am** - Automated Reminder Check:
```
1. Get today's date
2. Get deadline from Config
3. Calculate days until deadline
4. If days = 5, 1, or 0:
   a. Get all users from Config
   b. Check who has submitted
   c. Send reminders to pending users
   d. Send summary to admin
```

**Example:**
- Deadline: October 15
- October 10 (5 days): Send first reminder
- October 14 (1 day): Send urgent reminder
- October 15 (0 days, 7am): Send final reminder

### **Customizing Reminders**

**Change reminder days:**

Edit in `Code.gs`:
```javascript
REMINDER_DAYS: [5, 1, 0]
```

Change to:
```javascript
REMINDER_DAYS: [7, 3, 1, 0] // 4 reminders: 7d, 3d, 1d, deadline
```

**Change reminder time:**

Edit in `Code.gs`:
```javascript
REMINDER_TIME_HOUR: 7 // 7am
```

Change to:
```javascript
REMINDER_TIME_HOUR: 9 // 9am
```

Then re-run: **⚙️ Setup Time-based Triggers**

---

## 📊 Reporting & Analytics

### **Weekly Progress Report**

**Generate manually:**
1. Run: **👥 Update User Submission Status**
2. Check **User Status** sheet
3. Note pending count
4. Share with management if needed

**Metrics to track:**
- Submission rate: `Submitted / Total Users * 100`
- Average submissions per user
- Categories covered vs total (12)

### **Monthly Variance Analysis**

**After period closes:**
1. Run: **🔄 Sync Forecast vs Actual**
2. Run: **📊 Generate Variance Report**
3. Review **Variance Report** sheet

**Key metrics:**
- **Accuracy Rate**: Overall forecast accuracy %
- **Over Budget Count**: How many items exceeded forecast
- **Under Budget Count**: How many items under forecast
- **Category Performance**: Which categories are most accurate
- **User Performance**: Which users forecast most accurately

**Use insights to:**
- Identify training needs
- Improve forecast processes
- Reward accurate forecasters
- Adjust budget planning

### **Exporting Reports**

**Option 1: PDF Export**
1. Go to: **File > Download > PDF Document**
2. Select sheet: "Variance Report"
3. Save and share

**Option 2: Excel Export**
1. Go to: **File > Download > Microsoft Excel**
2. Share .xlsx file

**Option 3: Share Sheet Link**
- Share with view-only permissions
- Users can't edit, only view

---

## 🛠️ Advanced Configuration

### **Column Mapping**

If your sheet columns are different, update in `Code.gs`:

```javascript
FORECAST_COLS: {
  ID: 1,              // Column A
  QUARTER: 2,         // Column B
  YEAR: 3,            // Column C
  NO: 4,              // Column D
  DATE: 5,            // Column E
  // ... adjust all to match your sheet
}
```

**How to find column numbers:**
- A=1, B=2, C=3, D=4, E=5, etc.

### **Sheet Name Changes**

If you rename sheets, update in `Code.gs`:

```javascript
const CONFIG = {
  FORECAST_SHEET: 'Forecast',              // Your sheet name
  PR_SHEET: 'PR Tracking',                 // Your sheet name
  FORECAST_VS_ACTUAL_SHEET: 'TDG : Forecast vs Actual',
  CONFIG_SHEET: 'Config',
  USER_TRACKING_SHEET: 'User Status',
}
```

### **Email Template Customization**

Edit the `sendReminderEmail` function in `Code.gs`:

```javascript
const subject = `[TDG Forecast] Reminder - ${config.currentPeriod}`;

const body = `
Your custom email template here...
Use variables:
- ${user.name}
- ${config.currentPeriod}
- ${daysUntilDeadline}
`;
```

---

## 🐛 Troubleshooting Guide

### **Issue: Triggers Not Running**

**Symptoms:**
- No automatic reminders
- Forecast vs Actual not updating
- User Status not refreshing

**Solutions:**
1. Check triggers exist:
   - **Apps Script > Triggers**
   - Should see 3 triggers
2. Re-run: **⚙️ Setup Time-based Triggers**
3. Check authorization:
   - **Apps Script > Run > testSetup**
   - Re-authorize if prompted
4. Check execution logs:
   - **Apps Script > Executions**
   - Look for errors in red

**Common errors:**
- "Script disabled" → Re-enable in Apps Script
- "Authorization required" → Re-authorize
- "Exceeded daily quota" → Wait 24 hours (email quota)

---

### **Issue: Forecast IDs Not Generating**

**Symptoms:**
- Column A stays empty
- Message: "Generated 0 new Forecast IDs"

**Solutions:**
1. Check data exists in columns B (Quarter) and C (Year)
2. Verify rows have data (not just headers)
3. Check if IDs already exist (won't regenerate)
4. Try manual test:
   - Add a row with Quarter=Q1, Year=FY25
   - Run: **🎯 Generate Forecast IDs**
   - Should create ID

---

### **Issue: PRs Not Matching**

**Symptoms:**
- Variance Report shows many "Unmatched"
- Actual amounts showing as 0
- Forecast vs Actual sheet missing data

**Solutions:**
1. Check Forecast IDs match exactly:
   - Forecast sheet: `FY25-Q1M1-001`
   - PR sheet: `FY25-Q1M1-001` (must be identical)
2. Check for extra spaces or typos
3. Verify Forecast ID exists before PR entry
4. Manually verify:
   - Pick one Forecast ID
   - Find it in Forecast sheet
   - Find same ID in PR sheet
   - Should match

---

### **Issue: Emails Not Sending**

**Symptoms:**
- Users not receiving reminders
- Admin not receiving summaries
- Message: "Sent 0 reminder emails"

**Solutions:**
1. Check Config sheet:
   - **Reminder Enabled** = `TRUE`
   - User emails are correct format
   - Admin email is filled
2. Check spam/junk folders
3. Verify email quota not exceeded:
   - **Apps Script > Executions**
   - Look for quota errors
   - Google allows 100 emails/day
4. Test with manual send:
   - **📧 Send Reminders Now**
   - Check if any errors

---

### **Issue: Permission Errors**

**Symptoms:**
- "You need permission to access this"
- "Script not authorized"
- Functions won't run

**Solutions:**
1. Re-authorize script:
   - **Apps Script > Run > onOpen**
   - Click "Review Permissions"
   - Choose account
   - Click "Allow"
2. Check sheet permissions:
   - All users need "Edit" access
   - Or at least "Comment" for read-only users
3. If admin left: 
   - New admin must re-install script
   - Copy code to new Apps Script

---

### **Issue: Data Corruption**

**Symptoms:**
- Formulas broken
- Data missing
- Sheets reorganized

**Solutions:**
1. **Restore from version history:**
   - **File > Version History > See version history**
   - Find last good version (by date/time)
   - Click "Restore this version"
2. **Prevent future issues:**
   - Limit edit permissions
   - Train users not to delete columns
   - Make backup copies monthly

---

## 🔒 Security Best Practices

### **Access Control**

**Recommended permissions:**
- **Admins**: Full edit access
- **Users**: Edit access (to enter forecasts)
- **Management**: View-only (for reports)

**Setup:**
1. Click **Share** (top-right)
2. Add users with appropriate access
3. Uncheck "Editors can change permissions"

### **Script Security**

**The script can:**
- ✅ Read/write your Google Sheet
- ✅ Send emails as you
- ✅ Run on schedule

**The script CANNOT:**
- ❌ Access other files
- ❌ Send data externally
- ❌ Access your personal info (beyond email)

**Best practices:**
- Only admins should edit script
- Review code before installing
- Don't share Apps Script editor access
- Keep backup of original code

### **Data Privacy**

**All data stays in:**
- Your Google Sheet (owned by you/company)
- Your Google Account (for emails)

**No external services used**
**No data sent to third parties**

---

## 📅 Quarterly Workflow Example

### **Example: Q1M1 Period (Mid-October)**

**Week Before (Oct 8-10):**
- [ ] Update Config: Period=`Q1M1`, Deadline=`2024-10-15`
- [ ] Announce to users: "Q1M1 forecast window open"
- [ ] Verify triggers are running

**Week Of (Oct 10-15):**
- [ ] **Oct 10 (5d)**: First reminder sent automatically
- [ ] Monitor User Status daily
- [ ] **Oct 14 (1d)**: Urgent reminder sent
- [ ] **Oct 15 (7am)**: Final reminder sent
- [ ] **Oct 15 (EOD)**: Run Generate Forecast IDs

**After Deadline (Oct 16+):**
- [ ] Run: **🔄 Sync Forecast vs Actual**
- [ ] Follow up with non-submitters individually
- [ ] Lock period (change Config to next period)

**End of Quarter (December):**
- [ ] Collect all PRs in PR Tracking sheet
- [ ] Run: **🔄 Sync Forecast vs Actual** (final)
- [ ] Run: **📊 Generate Variance Report**
- [ ] Share report with management
- [ ] Prepare for Q1M3

---

## 📞 Support Resources

### **When Users Need Help**

**Common user questions:**
1. "How do I enter forecast?" → Share USER_QUICK_GUIDE.md
2. "Didn't receive reminder?" → Check Config, spam folder
3. "Can't edit sheet?" → Grant edit permissions
4. "What's my Forecast ID?" → Run Generate IDs, check Column A
5. "Made a mistake?" → They can edit their row anytime

### **When You Need Help**

**Resources:**
1. **This guide** - Admin procedures
2. **SETUP_INSTRUCTIONS.md** - Technical setup
3. **Code.gs comments** - Inline documentation
4. **Apps Script logs** - Error diagnosis
5. **Google Sheets Help** - General sheet questions

**Escalation:**
- IT department for technical issues
- Management for process questions
- Users for feedback on UX

---

## ✅ Monthly Admin Checklist

**Week 1: New Period**
- [ ] Update Config with new period/deadline
- [ ] Announce forecast window open
- [ ] Verify triggers running
- [ ] Check all 20 users in Config list

**Week 2: Mid-Period**
- [ ] Check User Status (how many submitted?)
- [ ] Run Generate Forecast IDs daily
- [ ] Follow up with stragglers
- [ ] Answer user questions

**Week 3: Before Deadline**
- [ ] Monitor submissions closely
- [ ] Send manual reminders if needed
- [ ] Prepare for deadline rush
- [ ] Run Sync Forecast vs Actual

**Week 4: After Deadline**
- [ ] Generate Variance Report
- [ ] Share reports with management
- [ ] Archive period (optional)
- [ ] Plan for next period

---

## 🎓 Training New Admins

**Onboarding checklist:**
- [ ] Read this guide completely
- [ ] Review SETUP_INSTRUCTIONS.md
- [ ] Watch current admin perform tasks
- [ ] Practice on test sheet first
- [ ] Run test period with small group
- [ ] Get added to Config as Admin Email
- [ ] Get access to Apps Script editor
- [ ] Save bookmark to sheet

**Shadow period:** 1 full quarter recommended

---

## 📈 System Metrics to Track

### **Operational Metrics**
- **On-time submission rate**: Target >90%
- **Reminder effectiveness**: Submissions after reminders
- **Forecast accuracy**: Target >85%
- **Variance trend**: Improving over time?

### **User Engagement**
- **Active users**: Submitting each period
- **Average items per user**: Consistency
- **Category coverage**: All 12 categories used?

### **System Health**
- **Trigger success rate**: Should be 100%
- **Email delivery rate**: Track bounces
- **Sync success**: Data matches correctly

---

## 🚀 Continuous Improvement

### **Quarterly Review**

**Questions to ask:**
1. Did system reduce manual work?
2. Are users satisfied?
3. Any recurring issues?
4. Forecast accuracy improving?
5. Can we automate more?

### **Potential Enhancements**

**Phase 2 ideas:**
- Dashboard with charts/graphs
- Mobile app for on-the-go submissions
- Integration with accounting system
- AI-powered forecast suggestions
- Automated PR import (if system supports API)

---

**You've got this! 🎉**

Remember: The system works best when:
- ✅ Config is always up-to-date
- ✅ Users are trained
- ✅ You check daily (just 5 min)
- ✅ Issues are addressed quickly

**Questions?** Review this guide or check the code comments in `Code.gs`.

---

*Last updated: 2024-11-10*






