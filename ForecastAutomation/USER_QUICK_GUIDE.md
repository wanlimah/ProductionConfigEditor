# 👤 User Quick Guide - TDG Forecast Submission

## 🎯 Quick Start for Forecast Users

This guide is for the **20 users** who submit quarterly forecasts.

---

## 📋 How to Submit Your Forecast (3 Easy Steps)

### **Step 1: Open the Google Sheet**

1. Click the link provided by your admin
2. Go to the **"Forecast"** tab

### **Step 2: Enter Your Forecast Data**

Fill in these columns for each item you're forecasting:

| Column | What to Enter | Example |
|--------|---------------|---------|
| **B - Quarter** | Which quarter | Q1, Q2, Q3, or Q4 |
| **C - Year** | Fiscal year | FY25 |
| **D - No** | Item number | 1, 2, 3... |
| **E - Date** | Today's date | 2024-10-01 |
| **F - Epicentech Group** | Your department/group | Production |
| **G - Location** | Your location | Site A |
| **H - Budgeted or Unbudgeted** | Status | Budgeted / Unbudgeted |
| **I - Prism** | Prism code | PRISM-001 |
| **J - Epicentech Category** | One of 12 categories | Equipment |
| **K - Item** | Simple item name | Laptop |
| **L - Description** | Detailed description | Dell Latitude 7420 |
| **M - Forecast (M1)** | M1 forecast amount | 50000 |
| **N - Unbudgeted** | Unbudgeted amount | 0 |
| **O - Forecast (M3)** | M3 forecast amount | 50000 |
| **P - Forecast Amount** | Final forecast amount | 50000 |

**⚠️ IMPORTANT:**
- **Column A (Forecast ID)**: Leave this **EMPTY** - it's auto-generated!
- **Column Q (User)**: Auto-filled with your email
- **Column R (Submitted)**: Auto-filled with timestamp

### **Step 3: Done!**

That's it! The system will:
- Auto-generate a unique **Forecast ID** (like `FY25-Q1M1-001`)
- Track your submission
- Stop sending you reminders

---

## 🔄 When You Raise a PR (Purchase Requisition)

### **Important: Link Your PR to Your Forecast**

When you raise a PR in your system, you need to record it:

1. Go to the **"PR Tracking"** tab
2. Add a new row with:

| Column | What to Enter | Example |
|--------|---------------|---------|
| **A - Forecast ID** | Copy from your forecast | FY25-Q1M1-001 |
| **B - PR Number** | Your PR number | PR-2024-12345 |
| **C - PR Date** | Date PR raised | 2024-10-20 |
| **D - Actual Amount** | Actual PR amount | 48000 |
| **E - Received Amount** | Amount received (if any) | 48000 |
| **F - PO Status** | Status of PO | Approved / Pending |
| **H - User** | Your email | your.email@company.com |

**Why is this important?**
- System matches your forecast with actual PR
- Calculates variance (forecast vs actual)
- Helps track budget accuracy

---

## 📧 Email Reminders

### **You'll Receive Reminders If:**
- You haven't submitted a forecast for the current period
- Deadline is approaching

### **Reminder Schedule:**
- **5 days before deadline** - ⚠️ Friendly reminder
- **1 day before deadline** - ⏰ Urgent reminder  
- **7am on deadline day** - 🚨 FINAL reminder

### **Stop Receiving Reminders:**
- Submit at least one forecast item
- System automatically detects your submission

---

## ✅ How to Know You've Submitted

### **Check Your Forecast Sheet:**
1. Look at **Column A** - Do you see a Forecast ID? (e.g., `FY25-Q1M1-001`)
2. Look at **Column R** - Do you see a timestamp?

If YES to both → ✅ **You've successfully submitted!**

### **Check User Status Sheet:**
- Ask admin to check **"User Status"** tab
- Your name should show: ✅ Complete

---

## 📊 Understanding the 12 Categories

Make sure to select the correct category in **Column J**:

1. **Equipment** - Machinery, tools, hardware
2. **Software** - Software licenses, subscriptions
3. **Materials** - Raw materials, supplies
4. **Services** - Professional services, consulting
5. **Maintenance** - Repairs, upkeep
6. **Training** - Employee training, courses
7. **Travel** - Business travel, transportation
8. **Facilities** - Office rent, utilities
9. **IT Infrastructure** - Servers, networking
10. **Marketing** - Advertising, promotions
11. **HR** - Recruitment, benefits
12. **Miscellaneous** - Other expenses

*(Check with your admin if categories are different)*

---

## 🗓️ Forecast Calendar (When to Submit)

| Period | Deadline | Type | What You Need to Do |
|--------|----------|------|---------------------|
| **Q1M1** | Mid-Oct | Forecast | Submit Q1 forecast |
| **Q1M3** | Mid-Dec | Confirm | Confirm PO/PR for Q1 |
| **Q2M1** | Mid-Jan | Forecast | Submit Q2 forecast |
| **Q2M3** | Mid-Mar | Confirm + Extended | Confirm Q2 + Forecast Q3-Q4 |
| **Q3M1** | Mid-Apr | Forecast + Extended | Submit Q3 + Forecast to Q4 |
| **Q3M3** | Mid-Jun | Confirm + Extended | Confirm Q3 + Forecast to Q4 |
| **Q4M1** | Mid-Jul | Forecast | Submit Q4 forecast |
| **Q4M3** | Mid-Sep | Confirm | Confirm PO/PR for Q4 |

**Extended Periods (Q2M3, Q3M1, Q3M3):**
- You need to forecast further ahead (Q3M1 through Q4M1)
- Just add multiple rows for different quarters

---

## 💡 Pro Tips

### **Tip 1: Copy Last Period**
- Find your previous forecast rows
- Copy and paste them to new rows
- Update amounts and dates
- System will generate new Forecast IDs

### **Tip 2: Bulk Entry**
- Enter all your items at once
- Don't wait until deadline
- Reduces stress and errors

### **Tip 3: Double-Check Before Deadline**
- Review all 12 categories
- Make sure nothing is missing
- Update amounts if plans changed

### **Tip 4: Keep Forecast ID Handy**
- When you raise PR, note the Forecast ID
- Makes linking PR to forecast easier
- Helps with audit trail

### **Tip 5: Communicate Changes**
- If forecast changes significantly, inform your admin
- Update the forecast amount
- Add notes in Description column

---

## ❓ Common Questions

### **Q: What if I forget to submit?**
**A:** You'll receive reminders. Submit as soon as possible, even after deadline.

### **Q: Can I edit my forecast after submitting?**
**A:** Yes! Just update the row. The Forecast ID stays the same.

### **Q: What if I don't have any forecast this period?**
**A:** Let your admin know via email so they don't send reminders.

### **Q: What if I have multiple items to forecast?**
**A:** Add multiple rows! Each item gets its own Forecast ID.

### **Q: What's the difference between M1 and M3?**
**A:** 
- **M1** = Initial forecast (early in quarter)
- **M3** = Confirmed forecast (later in quarter, more certain)

### **Q: What if the PR amount differs from forecast?**
**A:** That's okay! System tracks the variance. Try to keep it minimal.

### **Q: Can I see variance reports?**
**A:** Ask your admin to generate and share the **Variance Report**.

---

## 🆘 Troubleshooting

### **Problem: I can't edit the sheet**
**Solution:** Contact admin for edit permissions.

### **Problem: Forecast ID not appearing**
**Solution:** 
1. Make sure columns B (Quarter) and C (Year) are filled
2. Wait for admin to run "Generate Forecast IDs"
3. Or contact admin

### **Problem: I'm not receiving reminders**
**Solution:** 
1. Check spam/junk folder
2. Verify your email is in the Config sheet (ask admin)
3. Make sure you haven't already submitted

### **Problem: I accidentally deleted a row**
**Solution:** 
1. Use Google Sheets version history: **File > Version History**
2. Restore previous version
3. Contact admin if you need help

---

## 📞 Need Help?

**Contact Your Admin:**
- Email address in reminder emails
- They can help with:
  - Technical issues
  - Permission problems
  - Understanding the process
  - Special cases

**Before Contacting:**
- Check this guide first
- Try refreshing the page
- Make sure you're using the correct sheet link

---

## ✅ Quick Checklist

**Before Each Submission Deadline:**

- [ ] Opened the **Forecast** sheet
- [ ] Entered data for all my items (Columns B-P)
- [ ] Left Column A (Forecast ID) empty
- [ ] Selected correct category from 12 options
- [ ] Double-checked amounts
- [ ] Verified all my items are included
- [ ] Saved the sheet (auto-saves, but check)
- [ ] Noted my Forecast IDs for when I raise PRs

**After Raising a PR:**

- [ ] Opened **PR Tracking** sheet
- [ ] Entered Forecast ID from my forecast
- [ ] Filled in PR details
- [ ] Verified amounts match

---

## 🎉 You're Ready!

Remember:
- ✅ Submit on time → No reminders
- ✅ Link PRs to forecasts → Better tracking
- ✅ Ask questions if unclear → Admin is here to help

**Happy forecasting! 📊**

---

*Last updated: 2024-11-10*






