# ✅ Installation Checklist

Use this checklist to ensure proper setup of the TDG Forecast Automation System.

---

## 📋 PRE-INSTALLATION

### **Prerequisites**
- [ ] Google Workspace account with access to Google Sheets
- [ ] Edit permissions on the TDG Forecast spreadsheet
- [ ] Admin rights (for installing Apps Script)
- [ ] List of 20 user emails ready
- [ ] Current period deadline known

### **Backup**
- [ ] Make a copy of existing spreadsheet (File > Make a copy)
- [ ] Name it: "TDG Forecast BACKUP [DATE]"
- [ ] Save backup URL somewhere safe

---

## 🔧 INSTALLATION STEPS

### **Step 1: Open Apps Script Editor**
- [ ] Open your Google Sheet
- [ ] Go to: **Extensions > Apps Script**
- [ ] New project opens (or existing one loads)

### **Step 2: Install Code**
- [ ] Delete any existing code in the editor
- [ ] Open the file: `Code.gs`
- [ ] Copy ALL contents (Ctrl+A, Ctrl+C)
- [ ] Paste into Apps Script editor (Ctrl+V)
- [ ] Click **Save** (disk icon)
- [ ] Name project: "TDG Forecast Automation"
- [ ] Close Apps Script tab

### **Step 3: Initial Authorization**
- [ ] Refresh your Google Sheet (F5)
- [ ] New menu appears: **📊 TDG Forecast Automation**
- [ ] Click: **📊 TDG Forecast Automation > 📋 Initialize Config Sheet**
- [ ] Authorization prompt appears
- [ ] Click: **Continue**
- [ ] Choose your Google account
- [ ] Click: **Advanced** (if warning appears)
- [ ] Click: **Go to TDG Forecast Automation (unsafe)**
- [ ] Click: **Allow**
- [ ] Wait for confirmation: "Config sheet initialized!"

### **Step 4: Configure Settings**
- [ ] Go to the new **Config** sheet tab
- [ ] Row 2: Update **Current Period** (e.g., `Q1M1`)
- [ ] Row 3: Update **Current Year** (e.g., `FY25`)
- [ ] Row 4: Update **Deadline Date** (format: `YYYY-MM-DD`)
- [ ] Row 5: Set **Reminder Enabled** to `TRUE`
- [ ] Row 6: Update **Admin Email** (your email)

### **Step 5: Add User Emails**
- [ ] Still in **Config** sheet
- [ ] Starting at row 9, column B:
  - [ ] Add user1@company.com in B9
  - [ ] Add user name in C9
  - [ ] Add user2@company.com in B10
  - [ ] Add user name in C10
  - [ ] Continue for all 20 users...
- [ ] Double-check all emails are correct (no typos!)

---

## 📊 SHEET PREPARATION

### **Step 6: Verify Sheet Names**
Check you have these sheet tabs (rename if needed):

- [ ] **Forecast** - User data entry sheet
- [ ] **PR Tracking** - PR logging sheet
- [ ] **TDG : Forecast vs Actual** - Comparison sheet (your existing one)
- [ ] **Config** - Just created by script

### **Step 7: Verify Column Headers**

**In Forecast sheet (Row 1):**
- [ ] Column A: Forecast ID
- [ ] Column B: Quarter
- [ ] Column C: Year
- [ ] Column D: No
- [ ] Column E: Date
- [ ] Column F: Epicentech Group
- [ ] Column G: Location
- [ ] Column H: Budgeted or Unbudgeted
- [ ] Column I: Prism
- [ ] Column J: Epicentech Category
- [ ] Column K: Item (Simple) Item
- [ ] Column L: Description
- [ ] Column M: Forecast (M1)
- [ ] Column N: Unbudgeted
- [ ] Column O: Forecast (M3)
- [ ] Column P: Forecast Amount
- [ ] Column Q: User
- [ ] Column R: Submitted

**In PR Tracking sheet (Row 1):**
- [ ] Column A: Forecast ID
- [ ] Column B: PR Number
- [ ] Column C: PR Date
- [ ] Column D: Actual Amount
- [ ] Column E: Received Amount
- [ ] Column F: PO Status
- [ ] Column G: Variance
- [ ] Column H: User

---

## 🧪 TESTING

### **Step 8: Test Forecast ID Generation**
- [ ] Go to **Forecast** sheet
- [ ] Add a test row:
  - B2: `Q1`
  - C2: `FY25`
  - D2: `1`
  - E2: Today's date
  - J2: `Equipment`
  - P2: `10000`
- [ ] Click menu: **🎯 Generate Forecast IDs**
- [ ] Check: Column A2 should now have ID like `FY25-Q1M1-001`
- [ ] ✅ If yes, continue. If no, see troubleshooting below.

### **Step 9: Test User Status Tracking**
- [ ] Click menu: **👥 Update User Submission Status**
- [ ] New sheet **User Status** appears
- [ ] Check: Your test user shows as "Complete" ✅
- [ ] Check: Other users show as "PENDING" ❌
- [ ] ✅ If yes, continue

### **Step 10: Test PR Matching**
- [ ] Go to **PR Tracking** sheet
- [ ] Add test row:
  - A2: Copy Forecast ID from test (e.g., `FY25-Q1M1-001`)
  - B2: `PR-TEST-001`
  - C2: Today's date
  - D2: `9500`
- [ ] Click menu: **🔄 Sync Forecast vs Actual**
- [ ] Go to **TDG : Forecast vs Actual** sheet
- [ ] Check: Test row appears with matched data
- [ ] Check: Variance = 10000 - 9500 = 500
- [ ] ✅ If yes, continue

### **Step 11: Test Email Reminder (Manual)**
- [ ] Click menu: **📧 Send Reminders Now**
- [ ] Check your email (admin email from Config)
- [ ] You should receive: "Admin Summary" email
- [ ] Pending users should receive: Reminder emails
- [ ] ✅ If yes, continue
- [ ] ⚠️ If not, check spam folder and Config settings

### **Step 12: Test Variance Report**
- [ ] Click menu: **📊 Generate Variance Report**
- [ ] New sheet **Variance Report** appears
- [ ] Check: Summary statistics shown
- [ ] Check: Category breakdown shown
- [ ] Check: User breakdown shown
- [ ] ✅ If yes, continue

---

## ⚙️ AUTOMATION SETUP

### **Step 13: Setup Time-based Triggers**
- [ ] Click menu: **⚙️ Setup Time-based Triggers**
- [ ] Wait for confirmation: "Triggers setup successfully!"
- [ ] Message shows:
  - Daily reminders: 7am
  - Daily sync: 8am
  - Status update: 9am

### **Step 14: Verify Triggers**
- [ ] Go to: **Extensions > Apps Script**
- [ ] Click: **⏰ Triggers** (left sidebar, clock icon)
- [ ] Check: 3 triggers are listed:
  - [ ] `sendAutomatedReminders` - Time-driven, Daily, 7-8am
  - [ ] `syncForecastVsActual` - Time-driven, Daily, 8-9am
  - [ ] `updateUserSubmissionStatus` - Time-driven, Daily, 9-10am
- [ ] ✅ If all 3 present, triggers are working

---

## 🧹 CLEANUP

### **Step 15: Remove Test Data**
- [ ] Go to **Forecast** sheet
- [ ] Delete test row (row 2)
- [ ] Go to **PR Tracking** sheet
- [ ] Delete test row (row 2)
- [ ] Go to **TDG : Forecast vs Actual** sheet
- [ ] Run: **🔄 Sync Forecast vs Actual** (to clear test)
- [ ] Delete **Variance Report** sheet (will be recreated when needed)

### **Step 16: Set Permissions**
- [ ] Click **Share** button (top-right)
- [ ] Add all 20 users with **Editor** access
- [ ] Add management with **Viewer** access (if needed)
- [ ] Uncheck: "Editors can change permissions and share"
- [ ] Click **Done**

---

## 📚 DOCUMENTATION

### **Step 17: Share User Guide**
- [ ] Share `USER_QUICK_GUIDE.md` with all 20 users
- [ ] Send email with:
  - Link to Google Sheet
  - When first deadline is
  - Link to user guide
  - Your contact for questions

### **Step 18: Bookmark Resources**
- [ ] Admin saves:
  - [ ] Link to Google Sheet
  - [ ] Link to Apps Script editor
  - [ ] `ADMIN_GUIDE.md` (print or save PDF)
  - [ ] `QUICK_REFERENCE.md` (print)

---

## ✅ FINAL VERIFICATION

### **System Health Check**
- [ ] Config sheet has all settings filled
- [ ] All 20 user emails are in Config
- [ ] Forecast sheet has correct headers
- [ ] PR Tracking sheet has correct headers
- [ ] All 3 triggers are active in Apps Script
- [ ] Menu appears when you refresh sheet
- [ ] Test forecast ID generation works
- [ ] Test email sending works
- [ ] Test PR matching works

### **User Readiness**
- [ ] All 20 users have Editor access to sheet
- [ ] Users received onboarding email with guide
- [ ] Users know the deadline for current period
- [ ] Users know how to contact admin for help

### **Admin Readiness**
- [ ] Admin can access all menu functions
- [ ] Admin has Apps Script editor access
- [ ] Admin knows daily/weekly tasks
- [ ] Admin has saved all documentation

---

## 🎉 GO LIVE!

### **Launch Announcement**
- [ ] Send email to all users:
  ```
  Subject: New TDG Forecast Automation System - Now Live!
  
  Hi Team,
  
  We've launched a new automated forecast system to make 
  your life easier! 
  
  📊 What's New:
  - Auto-generated Forecast IDs
  - Email reminders (no more forgetting!)
  - Automated variance tracking
  
  🚀 Next Steps:
  1. Bookmark the sheet: [LINK]
  2. Read the user guide: [LINK]
  3. Submit your Q1M1 forecast by: [DEADLINE]
  
  Questions? Reply to this email.
  
  Thanks!
  [Admin Name]
  ```

### **Monitor First Week**
- [ ] Day 1: Check User Status daily
- [ ] Day 2: Verify reminders sent (if 5 days before deadline)
- [ ] Day 3: Answer user questions promptly
- [ ] Day 4: Run Generate Forecast IDs
- [ ] Day 5: Check for any errors in Apps Script logs
- [ ] End of Week: Review submission rate

### **After First Period**
- [ ] Generate Variance Report
- [ ] Share report with management
- [ ] Survey users for feedback
- [ ] Document lessons learned
- [ ] Adjust process if needed

---

## 🐛 TROUBLESHOOTING

### **Issue: Menu doesn't appear**
- ➡️ Solution: Refresh page (F5) and wait 5 seconds
- ➡️ If still no menu: Re-save script in Apps Script editor

### **Issue: Authorization error**
- ➡️ Solution: Go to Apps Script > Run > onOpen
- ➡️ Re-authorize when prompted

### **Issue: Forecast IDs not generating**
- ➡️ Check: Columns B (Quarter) and C (Year) have values
- ➡️ Check: Column A is empty (not formula, not text)
- ➡️ Solution: Clear column A completely, try again

### **Issue: Emails not sending**
- ➡️ Check: Config sheet, Reminder Enabled = TRUE
- ➡️ Check: User emails are valid format
- ➡️ Check: Spam/junk folder
- ➡️ Solution: Run manual send to test

### **Issue: Triggers not running**
- ➡️ Check: Apps Script > Triggers shows 3 triggers
- ➡️ Check: Apps Script > Executions for errors
- ➡️ Solution: Delete and re-run Setup Time-based Triggers

### **Issue: PR not matching**
- ➡️ Check: Forecast ID is EXACTLY the same (case-sensitive)
- ➡️ Check: No extra spaces in Forecast ID
- ➡️ Solution: Copy-paste Forecast ID from Forecast sheet

### **If All Else Fails**
1. Make a copy of your sheet (with data)
2. Start fresh installation on original
3. Copy data back if successful
4. Contact IT support if still failing

---

## 📞 POST-INSTALLATION SUPPORT

### **Where to Get Help**

**For users:**
- Read: `USER_QUICK_GUIDE.md`
- Contact: Admin email (from Config sheet)

**For admins:**
- Read: `ADMIN_GUIDE.md`
- Check: Apps Script execution logs
- Review: `SETUP_INSTRUCTIONS.md`

### **Resources**
- `README.md` - Overview and benefits
- `SETUP_INSTRUCTIONS.md` - Detailed setup
- `ADMIN_GUIDE.md` - Daily operations
- `USER_QUICK_GUIDE.md` - User instructions
- `QUICK_REFERENCE.md` - Printable cheat sheet
- `SHEET_TEMPLATE_STRUCTURE.md` - Technical reference

---

## 📊 SUCCESS CRITERIA

Your installation is successful if:

✅ All menu items work without errors  
✅ Forecast IDs generate automatically  
✅ Email reminders are received  
✅ Forecast vs Actual syncs correctly  
✅ User Status tracks submissions  
✅ Variance Report generates  
✅ Triggers run daily without issues  
✅ Users can submit forecasts  
✅ Admin can manage system  

**If all ✅, congratulations! Your system is live! 🎉**

---

## 📅 MAINTENANCE SCHEDULE

### **Daily (5 min)**
- [ ] Check User Status
- [ ] Run Generate Forecast IDs
- [ ] Monitor any user questions

### **Weekly (15 min)**
- [ ] Update Config for new periods
- [ ] Review submission progress
- [ ] Check Apps Script logs for errors

### **Monthly (30 min)**
- [ ] Generate Variance Report
- [ ] Update user list (if changes)
- [ ] Review and improve process

### **Quarterly (1 hour)**
- [ ] Full system health check
- [ ] Review triggers still active
- [ ] User feedback survey
- [ ] Plan improvements

---

**Installation Date:** _______________  
**Installed By:** _______________  
**Go-Live Date:** _______________  
**First Period:** _______________  

**Notes:**
_________________________________________
_________________________________________
_________________________________________

---

**Save this checklist for future reference!**

*Version 1.0 | Updated: 2024-11-10*






