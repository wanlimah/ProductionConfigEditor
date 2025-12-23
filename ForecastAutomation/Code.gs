/**
 * TDG Forecast vs Actual Automation System
 * 
 * Features:
 * - Auto-generate Forecast IDs
 * - Email reminders for forecast submissions
 * - Auto-match Forecast vs PR Actuals
 * - Track submission status across 20 users and 12 categories
 * - Handle special periods (Q2M3, Q3M1, Q3M3 extended forecasts)
 */

// ============================================================================
// CONFIGURATION - Update these constants for your sheet
// ============================================================================

const CONFIG = {
  // Sheet Names
  FORECAST_SHEET: 'Forecast',
  PR_SHEET: 'PR Tracking',
  FORECAST_VS_ACTUAL_SHEET: 'TDG : Forecast vs Actual',
  CONFIG_SHEET: 'Config',
  USER_TRACKING_SHEET: 'User Status',
  
  // Column positions (adjust based on your actual sheet structure)
  FORECAST_COLS: {
    ID: 1,              // A - Auto-generated Forecast ID
    QUARTER: 2,         // B - Quarter
    YEAR: 3,            // C - Year
    NO: 4,              // D - No
    DATE: 5,            // E - Date
    EPICENTECH_GROUP: 6,// F - Epicentech Group
    LOCATION: 7,        // G - Location
    BUDGETED: 8,        // H - Budgeted or Unbudgeted
    PRISM: 9,           // I - Prism
    CATEGORY: 10,       // J - Epicentech Category
    ITEM: 11,           // K - Item (Simple) Item
    DESCRIPTION: 12,    // L - Description
    FORECAST_M1: 13,    // M - Forecast (M1)
    UNBUDGETED: 14,     // N - Unbudgeted
    FORECAST_M3: 15,    // O - Forecast (M3)
    FORECAST_AMOUNT: 16,// P - Forecast Amount
    USER: 17,           // Q - User who created forecast
    SUBMITTED: 18       // R - Submission timestamp
  },
  
  PR_COLS: {
    FORECAST_ID: 1,     // A - Reference to Forecast ID
    PR_NUMBER: 2,       // B - PR Number
    PR_DATE: 3,         // C - PR Date
    ACTUAL_AMOUNT: 4,   // D - Actual Amount
    RECEIVED_AMOUNT: 5, // E - Received Amount
    PO_STATUS: 6,       // F - PO Status
    VARIANCE: 7,        // G - Variance
    USER: 8             // H - User who raised PR
  },
  
  // Email Settings
  EMAIL_SUBJECT_PREFIX: '[TDG Forecast Reminder]',
  REMINDER_DAYS: [5, 1, 0], // Days before deadline to send reminders
  REMINDER_TIME_HOUR: 7     // 7am for same-day reminder
};

// ============================================================================
// INITIALIZATION & SETUP
// ============================================================================

/**
 * Create custom menu when spreadsheet opens
 */
function onOpen() {
  const ui = SpreadsheetApp.getUi();
  ui.createMenu('📊 TDG Forecast Automation')
    .addItem('🎯 Generate Forecast IDs', 'generateForecastIDs')
    .addSeparator()
    .addItem('🔄 Sync Forecast vs Actual', 'syncForecastVsActual')
    .addItem('📧 Send Reminders Now', 'sendRemindersManual')
    .addSeparator()
    .addItem('👥 Update User Submission Status', 'updateUserSubmissionStatus')
    .addItem('📊 Generate Variance Report', 'generateVarianceReport')
    .addSeparator()
    .addItem('⚙️ Setup Time-based Triggers', 'setupTriggers')
    .addItem('📋 Initialize Config Sheet', 'initializeConfigSheet')
    .addToUi();
}

/**
 * Initialize configuration sheet with default settings
 */
function initializeConfigSheet() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  let configSheet = ss.getSheetByName(CONFIG.CONFIG_SHEET);
  
  // Create config sheet if it doesn't exist
  if (!configSheet) {
    configSheet = ss.insertSheet(CONFIG.CONFIG_SHEET);
  }
  
  configSheet.clear();
  
  // Set up headers
  const headers = [
    ['Setting', 'Value', 'Description'],
    ['Current Period', 'Q1M1', 'Current forecast period (e.g., Q1M1, Q1M3, Q2M1)'],
    ['Current Year', 'FY25', 'Current fiscal year'],
    ['Deadline Date', '2024-10-15', 'Deadline for current period (YYYY-MM-DD)'],
    ['Reminder Enabled', 'TRUE', 'Enable/disable email reminders'],
    ['Admin Email', '', 'Email for system notifications'],
    [''],
    ['User Email List', 'Email', 'User Name'],
    ['', 'user1@example.com', 'User 1'],
    ['', 'user2@example.com', 'User 2'],
    ['Add more users below...', '', '']
  ];
  
  configSheet.getRange(1, 1, headers.length, 3).setValues(headers);
  
  // Format headers
  configSheet.getRange(1, 1, 1, 3)
    .setBackground('#4285f4')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  
  configSheet.getRange(7, 1, 1, 3)
    .setBackground('#34a853')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  
  // Auto-resize columns
  configSheet.autoResizeColumns(1, 3);
  
  SpreadsheetApp.getUi().alert('✅ Config sheet initialized!\n\nPlease update:\n1. Current Period\n2. Deadline Date\n3. Admin Email\n4. User Email List');
}

// ============================================================================
// FORECAST ID GENERATION
// ============================================================================

/**
 * Auto-generate unique Forecast IDs for new entries
 */
function generateForecastIDs() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const config = getConfigValues();
  const lastRow = forecastSheet.getLastRow();
  
  if (lastRow < 2) {
    SpreadsheetApp.getUi().alert('ℹ️ No data to process.');
    return;
  }
  
  let generatedCount = 0;
  
  // Loop through all rows starting from row 2 (skip header)
  for (let row = 2; row <= lastRow; row++) {
    const idCell = forecastSheet.getRange(row, CONFIG.FORECAST_COLS.ID);
    const currentId = idCell.getValue();
    
    // Only generate ID if cell is empty and row has data
    if (!currentId || currentId === '') {
      const quarter = forecastSheet.getRange(row, CONFIG.FORECAST_COLS.QUARTER).getValue();
      const year = forecastSheet.getRange(row, CONFIG.FORECAST_COLS.YEAR).getValue();
      
      if (quarter && year) {
        const newId = generateUniqueId(year, quarter, config.currentPeriod);
        idCell.setValue(newId);
        
        // Add timestamp
        forecastSheet.getRange(row, CONFIG.FORECAST_COLS.SUBMITTED)
          .setValue(new Date());
        
        generatedCount++;
      }
    }
  }
  
  SpreadsheetApp.getUi().alert(`✅ Generated ${generatedCount} new Forecast IDs!`);
}

/**
 * Generate unique forecast ID
 * Format: FY25-Q1M1-001, FY25-Q1M1-002, etc.
 */
function generateUniqueId(year, quarter, period) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG.FORECAST_SHEET);
  
  // Get all existing IDs
  const dataRange = forecastSheet.getRange(2, CONFIG.FORECAST_COLS.ID, forecastSheet.getLastRow() - 1, 1);
  const existingIds = dataRange.getValues().flat().filter(id => id !== '');
  
  // Find highest sequence number for this period
  const prefix = `${year}-${period}-`;
  let maxSeq = 0;
  
  existingIds.forEach(id => {
    if (String(id).startsWith(prefix)) {
      const seq = parseInt(String(id).split('-')[2]);
      if (!isNaN(seq) && seq > maxSeq) {
        maxSeq = seq;
      }
    }
  });
  
  // Generate new ID
  const newSeq = String(maxSeq + 1).padStart(3, '0');
  return `${prefix}${newSeq}`;
}

// ============================================================================
// FORECAST VS ACTUAL MATCHING
// ============================================================================

/**
 * Sync forecast data with PR actuals and update Forecast vs Actual sheet
 */
function syncForecastVsActual() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG.FORECAST_SHEET);
  const prSheet = ss.getSheetByName(CONFIG.PR_SHEET);
  const fvaSheet = ss.getSheetByName(CONFIG.FORECAST_VS_ACTUAL_SHEET);
  
  if (!forecastSheet || !prSheet || !fvaSheet) {
    SpreadsheetApp.getUi().alert('❌ Required sheets not found!');
    return;
  }
  
  // Get all forecast data
  const forecastData = getForecastData(forecastSheet);
  
  // Get all PR data
  const prData = getPRData(prSheet);
  
  // Match forecasts with PRs
  const matchedData = matchForecastsWithPRs(forecastData, prData);
  
  // Update Forecast vs Actual sheet
  updateForecastVsActualSheet(fvaSheet, matchedData);
  
  SpreadsheetApp.getUi().alert(`✅ Synced ${matchedData.length} records!\n\nMatched: ${matchedData.filter(r => r.prMatched).length}\nUnmatched: ${matchedData.filter(r => !r.prMatched).length}`);
}

/**
 * Get all forecast data
 */
function getForecastData(sheet) {
  const lastRow = sheet.getLastRow();
  if (lastRow < 2) return [];
  
  const data = sheet.getRange(2, 1, lastRow - 1, CONFIG.FORECAST_COLS.SUBMITTED).getValues();
  
  return data.map(row => ({
    id: row[CONFIG.FORECAST_COLS.ID - 1],
    quarter: row[CONFIG.FORECAST_COLS.QUARTER - 1],
    year: row[CONFIG.FORECAST_COLS.YEAR - 1],
    no: row[CONFIG.FORECAST_COLS.NO - 1],
    date: row[CONFIG.FORECAST_COLS.DATE - 1],
    epicentechGroup: row[CONFIG.FORECAST_COLS.EPICENTECH_GROUP - 1],
    location: row[CONFIG.FORECAST_COLS.LOCATION - 1],
    budgeted: row[CONFIG.FORECAST_COLS.BUDGETED - 1],
    prism: row[CONFIG.FORECAST_COLS.PRISM - 1],
    category: row[CONFIG.FORECAST_COLS.CATEGORY - 1],
    item: row[CONFIG.FORECAST_COLS.ITEM - 1],
    description: row[CONFIG.FORECAST_COLS.DESCRIPTION - 1],
    forecastM1: row[CONFIG.FORECAST_COLS.FORECAST_M1 - 1],
    unbudgeted: row[CONFIG.FORECAST_COLS.UNBUDGETED - 1],
    forecastM3: row[CONFIG.FORECAST_COLS.FORECAST_M3 - 1],
    forecastAmount: row[CONFIG.FORECAST_COLS.FORECAST_AMOUNT - 1],
    user: row[CONFIG.FORECAST_COLS.USER - 1],
    submitted: row[CONFIG.FORECAST_COLS.SUBMITTED - 1]
  })).filter(r => r.id); // Only return rows with IDs
}

/**
 * Get all PR data
 */
function getPRData(sheet) {
  const lastRow = sheet.getLastRow();
  if (lastRow < 2) return [];
  
  const data = sheet.getRange(2, 1, lastRow - 1, CONFIG.PR_COLS.USER).getValues();
  
  return data.map(row => ({
    forecastId: row[CONFIG.PR_COLS.FORECAST_ID - 1],
    prNumber: row[CONFIG.PR_COLS.PR_NUMBER - 1],
    prDate: row[CONFIG.PR_COLS.PR_DATE - 1],
    actualAmount: row[CONFIG.PR_COLS.ACTUAL_AMOUNT - 1],
    receivedAmount: row[CONFIG.PR_COLS.RECEIVED_AMOUNT - 1],
    poStatus: row[CONFIG.PR_COLS.PO_STATUS - 1],
    variance: row[CONFIG.PR_COLS.VARIANCE - 1],
    user: row[CONFIG.PR_COLS.USER - 1]
  })).filter(r => r.forecastId); // Only return rows with forecast IDs
}

/**
 * Match forecasts with PRs by Forecast ID
 */
function matchForecastsWithPRs(forecasts, prs) {
  // Create a map of PRs by forecast ID for quick lookup
  const prMap = new Map();
  prs.forEach(pr => {
    if (!prMap.has(pr.forecastId)) {
      prMap.set(pr.forecastId, []);
    }
    prMap.get(pr.forecastId).push(pr);
  });
  
  // Match each forecast with its PRs
  return forecasts.map(forecast => {
    const matchedPrs = prMap.get(forecast.id) || [];
    
    // Calculate totals if multiple PRs
    const totalActual = matchedPrs.reduce((sum, pr) => sum + (parseFloat(pr.actualAmount) || 0), 0);
    const totalReceived = matchedPrs.reduce((sum, pr) => sum + (parseFloat(pr.receivedAmount) || 0), 0);
    
    // Calculate variance
    const forecastAmt = parseFloat(forecast.forecastAmount) || 0;
    const variance = forecastAmt - totalActual;
    const variancePct = forecastAmt !== 0 ? ((variance / forecastAmt) * 100).toFixed(2) : 0;
    
    return {
      ...forecast,
      prMatched: matchedPrs.length > 0,
      prNumbers: matchedPrs.map(pr => pr.prNumber).join(', '),
      actualAmount: totalActual,
      receivedAmount: totalReceived,
      variance: variance,
      variancePercent: variancePct,
      matchedPrs: matchedPrs
    };
  });
}

/**
 * Update Forecast vs Actual sheet with matched data
 */
function updateForecastVsActualSheet(sheet, matchedData) {
  // Determine starting row (preserve headers)
  const headerRow = 5; // Based on your screenshot
  const startRow = headerRow + 1;
  
  // Clear existing data (keep headers)
  const lastRow = sheet.getLastRow();
  if (lastRow > headerRow) {
    sheet.getRange(startRow, 1, lastRow - headerRow, sheet.getLastColumn()).clearContent();
  }
  
  if (matchedData.length === 0) return;
  
  // Prepare data for writing
  const outputData = matchedData.map(row => [
    row.id,                    // Forecast ID
    row.quarter,               // Quarter
    row.year,                  // Year
    row.no,                    // No
    row.date,                  // Date
    row.epicentechGroup,       // Epicentech Group
    row.location,              // Location
    row.budgeted,              // Budgeted or Unbudgeted
    row.prism,                 // Prism
    row.category,              // Category
    row.item,                  // Item
    row.description,           // Description
    row.forecastAmount,        // Forecast Amount
    row.unbudgeted,            // Unbudgeted
    row.forecastM3,            // Forecast M3
    row.actualAmount,          // Actual Amount
    row.receivedAmount,        // Received Amount
    row.variance,              // Variance
    row.variancePercent + '%', // Variance %
    row.prNumbers              // PR Numbers
  ]);
  
  // Write data
  sheet.getRange(startRow, 1, outputData.length, outputData[0].length).setValues(outputData);
  
  // Apply conditional formatting for variances
  applyVarianceFormatting(sheet, startRow, outputData.length);
}

/**
 * Apply conditional formatting to highlight variances
 */
function applyVarianceFormatting(sheet, startRow, numRows) {
  if (numRows === 0) return;
  
  const varianceCol = 18; // Adjust based on your actual column
  const varianceRange = sheet.getRange(startRow, varianceCol, numRows, 1);
  
  // Get values to apply conditional formatting
  const values = varianceRange.getValues();
  const backgrounds = [];
  
  values.forEach(row => {
    const variance = parseFloat(row[0]) || 0;
    let color = '#ffffff'; // Default white
    
    if (variance > 0) {
      color = '#d9ead3'; // Light green - under budget
    } else if (variance < 0) {
      color = '#f4cccc'; // Light red - over budget
    }
    
    backgrounds.push([color]);
  });
  
  varianceRange.setBackgrounds(backgrounds);
}

// ============================================================================
// USER SUBMISSION TRACKING
// ============================================================================

/**
 * Update user submission status
 */
function updateUserSubmissionStatus() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG.FORECAST_SHEET);
  const config = getConfigValues();
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  // Get all users from config
  const users = config.users;
  
  // Get current period submissions
  const submissions = getForecastData(forecastSheet).filter(f => 
    f.quarter === config.currentPeriod && f.year === config.currentYear
  );
  
  // Track who has submitted
  const submittedUsers = new Set(submissions.map(s => s.user));
  
  // Create status report
  const statusReport = users.map(user => ({
    email: user.email,
    name: user.name,
    submitted: submittedUsers.has(user.email),
    count: submissions.filter(s => s.user === user.email).length
  }));
  
  // Update or create User Status sheet
  updateUserStatusSheet(statusReport, config);
  
  return statusReport;
}

/**
 * Update User Status sheet
 */
function updateUserStatusSheet(statusReport, config) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  let statusSheet = ss.getSheetByName(CONFIG.USER_TRACKING_SHEET);
  
  if (!statusSheet) {
    statusSheet = ss.insertSheet(CONFIG.USER_TRACKING_SHEET);
  }
  
  statusSheet.clear();
  
  // Headers
  const headers = [
    ['User Submission Status', '', '', '', ''],
    ['Period:', config.currentPeriod, 'Year:', config.currentYear, 'Deadline: ' + config.deadline],
    [''],
    ['User Name', 'Email', 'Submitted', 'Count', 'Status']
  ];
  
  statusSheet.getRange(1, 1, headers.length, 5).setValues(headers);
  
  // Data
  const data = statusReport.map(user => [
    user.name,
    user.email,
    user.submitted ? '✅' : '❌',
    user.count,
    user.submitted ? 'Complete' : 'PENDING'
  ]);
  
  statusSheet.getRange(5, 1, data.length, 5).setValues(data);
  
  // Format
  statusSheet.getRange(1, 1, 1, 5)
    .merge()
    .setBackground('#4285f4')
    .setFontColor('#ffffff')
    .setFontWeight('bold')
    .setFontSize(14);
  
  statusSheet.getRange(4, 1, 1, 5)
    .setBackground('#34a853')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  
  // Conditional formatting for status
  const statusRange = statusSheet.getRange(5, 5, data.length, 1);
  statusRange.getValues().forEach((row, i) => {
    const color = row[0] === 'Complete' ? '#d9ead3' : '#f4cccc';
    statusSheet.getRange(5 + i, 5).setBackground(color);
  });
  
  statusSheet.autoResizeColumns(1, 5);
}

// ============================================================================
// EMAIL REMINDER SYSTEM
// ============================================================================

/**
 * Send reminder emails to users who haven't submitted
 * This function is triggered automatically based on schedule
 */
function sendAutomatedReminders() {
  // TEMPORARILY DISABLED
  Logger.log('Automated reminders are temporarily disabled by code.');
  return;

  const config = getConfigValues();
  
  if (!config.reminderEnabled) {
    Logger.log('Reminders are disabled in config');
    return;
  }
  
  const today = new Date();
  const deadline = new Date(config.deadline);
  const daysUntilDeadline = Math.ceil((deadline - today) / (1000 * 60 * 60 * 24));
  
  // Check if today is a reminder day
  if (!CONFIG.REMINDER_DAYS.includes(daysUntilDeadline)) {
    Logger.log(`No reminder scheduled for ${daysUntilDeadline} days before deadline`);
    return;
  }
  
  sendRemindersManual();
}

/**
 * Manually send reminders (can be triggered from menu)
 */
function sendRemindersManual() {
  const statusReport = updateUserSubmissionStatus();
  const config = getConfigValues();
  
  const pendingUsers = statusReport.filter(user => !user.submitted);
  
  if (pendingUsers.length === 0) {
    SpreadsheetApp.getUi().alert('✅ All users have submitted!');
    return;
  }
  
  const today = new Date();
  const deadline = new Date(config.deadline);
  const daysUntilDeadline = Math.ceil((deadline - today) / (1000 * 60 * 60 * 24));
  
  let sentCount = 0;
  
  pendingUsers.forEach(user => {
    try {
      sendReminderEmail(user, config, daysUntilDeadline);
      sentCount++;
    } catch (error) {
      Logger.log(`Failed to send email to ${user.email}: ${error}`);
    }
  });
  
  // Send summary to admin
  sendAdminSummary(config, pendingUsers, sentCount);
  
  SpreadsheetApp.getUi().alert(`📧 Sent ${sentCount} reminder emails!\n\nPending users: ${pendingUsers.length}`);
}

/**
 * Send reminder email to individual user
 */
function sendReminderEmail(user, config, daysUntilDeadline) {
  const spreadsheetUrl = SpreadsheetApp.getActiveSpreadsheet().getUrl();
  
  let urgency = '⚠️';
  let urgencyText = 'Reminder';
  
  if (daysUntilDeadline === 0) {
    urgency = '🚨';
    urgencyText = 'URGENT - TODAY';
  } else if (daysUntilDeadline === 1) {
    urgency = '⏰';
    urgencyText = 'TOMORROW';
  }
  
  const subject = `${CONFIG.EMAIL_SUBJECT_PREFIX} ${urgency} ${urgencyText} - ${config.currentPeriod} Forecast Due`;
  
  const body = `
Dear ${user.name},

${urgency} This is a reminder to submit your forecast for ${config.currentPeriod} ${config.currentYear}.

📅 Deadline: ${config.deadline}
⏳ Time Remaining: ${daysUntilDeadline} day(s)

You have not yet submitted your forecast. Please complete your submission as soon as possible.

📊 Access the forecast sheet here:
${spreadsheetUrl}

Instructions:
1. Go to the "${CONFIG.FORECAST_SHEET}" tab
2. Fill in your forecast details for all 12 categories
3. The system will automatically generate a Forecast ID for tracking
4. When you raise a PR, reference this Forecast ID

If you have already submitted, please disregard this email.

For questions, contact: ${config.adminEmail}

Best regards,
TDG Forecast Automation System
  `.trim();
  
  MailApp.sendEmail({
    to: user.email,
    subject: subject,
    body: body
  });
  
  Logger.log(`Reminder sent to ${user.email}`);
}

/**
 * Send summary email to admin
 */
function sendAdminSummary(config, pendingUsers, sentCount) {
  if (!config.adminEmail) return;
  
  const spreadsheetUrl = SpreadsheetApp.getActiveSpreadsheet().getUrl();
  
  const subject = `${CONFIG.EMAIL_SUBJECT_PREFIX} Summary - ${pendingUsers.length} Users Pending`;
  
  const userList = pendingUsers.map(u => `  - ${u.name} (${u.email})`).join('\n');
  
  const body = `
Admin Summary - Forecast Reminder System

Period: ${config.currentPeriod} ${config.currentYear}
Deadline: ${config.deadline}

📊 Status:
  - Total Users: ${config.users.length}
  - Pending: ${pendingUsers.length}
  - Reminders Sent: ${sentCount}

👥 Pending Users:
${userList}

View detailed status:
${spreadsheetUrl}#gid=${SpreadsheetApp.getActiveSpreadsheet().getSheetByName(CONFIG.USER_TRACKING_SHEET).getSheetId()}

Best regards,
TDG Forecast Automation System
  `.trim();
  
  MailApp.sendEmail({
    to: config.adminEmail,
    subject: subject,
    body: body
  });
}

// ============================================================================
// VARIANCE REPORTING
// ============================================================================

/**
 * Generate variance report for current period
 */
function generateVarianceReport() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const fvaSheet = ss.getSheetByName(CONFIG.FORECAST_VS_ACTUAL_SHEET);
  
  if (!fvaSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast vs Actual sheet not found!');
    return;
  }
  
  // Analyze variances
  const report = analyzeVariances(fvaSheet);
  
  // Create report sheet
  displayVarianceReport(report);
}

/**
 * Analyze variance data
 */
function analyzeVariances(sheet) {
  const lastRow = sheet.getLastRow();
  const startRow = 6; // Adjust based on your header row
  
  if (lastRow < startRow) {
    return { totalItems: 0, matched: 0, overBudget: 0, underBudget: 0 };
  }
  
  // Get data (adjust columns as needed)
  const forecastCol = 13; // Forecast Amount
  const actualCol = 16;   // Actual Amount
  const varianceCol = 18; // Variance
  
  const data = sheet.getRange(startRow, 1, lastRow - startRow + 1, 20).getValues();
  
  let totalItems = 0;
  let matched = 0;
  let overBudget = 0;
  let underBudget = 0;
  let totalForecast = 0;
  let totalActual = 0;
  let totalVariance = 0;
  
  const categoryVariances = new Map();
  const userVariances = new Map();
  
  data.forEach(row => {
    if (!row[0]) return; // Skip empty rows
    
    totalItems++;
    
    const forecast = parseFloat(row[forecastCol - 1]) || 0;
    const actual = parseFloat(row[actualCol - 1]) || 0;
    const variance = parseFloat(row[varianceCol - 1]) || 0;
    const category = row[9]; // Category column
    const user = row[16]; // User column
    
    totalForecast += forecast;
    totalActual += actual;
    totalVariance += variance;
    
    if (actual > 0) matched++;
    if (variance < 0) overBudget++;
    if (variance > 0) underBudget++;
    
    // Track by category
    if (!categoryVariances.has(category)) {
      categoryVariances.set(category, { forecast: 0, actual: 0, variance: 0, count: 0 });
    }
    const catData = categoryVariances.get(category);
    catData.forecast += forecast;
    catData.actual += actual;
    catData.variance += variance;
    catData.count++;
    
    // Track by user
    if (!userVariances.has(user)) {
      userVariances.set(user, { forecast: 0, actual: 0, variance: 0, count: 0 });
    }
    const userData = userVariances.get(user);
    userData.forecast += forecast;
    userData.actual += actual;
    userData.variance += variance;
    userData.count++;
  });
  
  return {
    totalItems,
    matched,
    overBudget,
    underBudget,
    totalForecast,
    totalActual,
    totalVariance,
    accuracyRate: totalForecast !== 0 ? ((1 - Math.abs(totalVariance) / totalForecast) * 100).toFixed(2) : 0,
    categoryVariances,
    userVariances
  };
}

/**
 * Display variance report in a new sheet
 */
function displayVarianceReport(report) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const config = getConfigValues();
  let reportSheet = ss.getSheetByName('Variance Report');
  
  if (!reportSheet) {
    reportSheet = ss.insertSheet('Variance Report');
  }
  
  reportSheet.clear();
  
  // Title
  const title = [[`Variance Report - ${config.currentPeriod} ${config.currentYear}`]];
  reportSheet.getRange(1, 1, 1, 5).merge().setValues(title);
  reportSheet.getRange(1, 1).setBackground('#4285f4').setFontColor('#ffffff').setFontWeight('bold').setFontSize(16);
  
  // Summary stats
  const summary = [
    ['Summary Statistics', '', '', '', ''],
    ['Total Items', report.totalItems, 'Matched with PR', report.matched, `${((report.matched/report.totalItems)*100).toFixed(1)}%`],
    ['Over Budget', report.overBudget, 'Under Budget', report.underBudget, ''],
    ['Total Forecast', report.totalForecast, 'Total Actual', report.totalActual, ''],
    ['Total Variance', report.totalVariance, 'Accuracy Rate', report.accuracyRate + '%', ''],
    ['']
  ];
  
  reportSheet.getRange(3, 1, summary.length, 5).setValues(summary);
  reportSheet.getRange(3, 1, 1, 5).setBackground('#34a853').setFontColor('#ffffff').setFontWeight('bold');
  
  // Category breakdown
  let row = 3 + summary.length + 1;
  reportSheet.getRange(row, 1, 1, 5).setValues([['Variance by Category', '', '', '', '']]);
  reportSheet.getRange(row, 1, 1, 5).merge().setBackground('#fbbc04').setFontWeight('bold');
  row++;
  
  reportSheet.getRange(row, 1, 1, 5).setValues([['Category', 'Forecast', 'Actual', 'Variance', 'Accuracy %']]);
  reportSheet.getRange(row, 1, 1, 5).setBackground('#f4b400').setFontWeight('bold');
  row++;
  
  const catData = Array.from(report.categoryVariances.entries()).map(([cat, data]) => [
    cat,
    data.forecast,
    data.actual,
    data.variance,
    data.forecast !== 0 ? ((1 - Math.abs(data.variance) / data.forecast) * 100).toFixed(2) + '%' : 'N/A'
  ]);
  
  if (catData.length > 0) {
    reportSheet.getRange(row, 1, catData.length, 5).setValues(catData);
    row += catData.length;
  }
  
  // User breakdown
  row += 2;
  reportSheet.getRange(row, 1, 1, 5).setValues([['Variance by User', '', '', '', '']]);
  reportSheet.getRange(row, 1, 1, 5).merge().setBackground('#ea4335').setFontColor('#ffffff').setFontWeight('bold');
  row++;
  
  reportSheet.getRange(row, 1, 1, 5).setValues([['User', 'Forecast', 'Actual', 'Variance', 'Accuracy %']]);
  reportSheet.getRange(row, 1, 1, 5).setBackground('#d93025').setFontColor('#ffffff').setFontWeight('bold');
  row++;
  
  const userData = Array.from(report.userVariances.entries()).map(([user, data]) => [
    user,
    data.forecast,
    data.actual,
    data.variance,
    data.forecast !== 0 ? ((1 - Math.abs(data.variance) / data.forecast) * 100).toFixed(2) + '%' : 'N/A'
  ]);
  
  if (userData.length > 0) {
    reportSheet.getRange(row, 1, userData.length, 5).setValues(userData);
  }
  
  reportSheet.autoResizeColumns(1, 5);
  
  SpreadsheetApp.getUi().alert('✅ Variance report generated!\n\nCheck the "Variance Report" tab.');
}

// ============================================================================
// TRIGGER MANAGEMENT
// ============================================================================

/**
 * Setup time-based triggers for automated reminders
 */
function setupTriggers() {
  // Delete existing triggers first
  const triggers = ScriptApp.getProjectTriggers();
  triggers.forEach(trigger => ScriptApp.deleteTrigger(trigger));
  
  // Create daily trigger at 7am for automated reminders
  // TEMPORARILY DISABLED
  /*
  ScriptApp.newTrigger('sendAutomatedReminders')
    .timeBased()
    .atHour(CONFIG.REMINDER_TIME_HOUR)
    .everyDays(1)
    .create();
  */
  
  // Create trigger to sync forecast vs actual daily at 8am
  ScriptApp.newTrigger('syncForecastVsActual')
    .timeBased()
    .atHour(8)
    .everyDays(1)
    .create();
  
  // Create trigger to update user status daily at 9am
  ScriptApp.newTrigger('updateUserSubmissionStatus')
    .timeBased()
    .atHour(9)
    .everyDays(1)
    .create();
  
  SpreadsheetApp.getUi().alert('✅ Triggers setup successfully!\n\n' +
    '- Daily reminders: 7am\n' +
    '- Daily sync: 8am\n' +
    '- Status update: 9am');
}

/**
 * Remove all triggers (for cleanup)
 */
function removeTriggers() {
  const triggers = ScriptApp.getProjectTriggers();
  triggers.forEach(trigger => ScriptApp.deleteTrigger(trigger));
  
  SpreadsheetApp.getUi().alert('✅ All triggers removed.');
}

// ============================================================================
// UTILITY FUNCTIONS
// ============================================================================

/**
 * Get configuration values from Config sheet
 */
function getConfigValues() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const configSheet = ss.getSheetByName(CONFIG.CONFIG_SHEET);
  
  if (!configSheet) {
    throw new Error('Config sheet not found! Please run "Initialize Config Sheet" first.');
  }
  
  // Read configuration values
  const currentPeriod = configSheet.getRange(2, 2).getValue() || 'Q1M1';
  const currentYear = configSheet.getRange(3, 2).getValue() || 'FY25';
  const deadline = configSheet.getRange(4, 2).getValue() || new Date();
  const reminderEnabled = configSheet.getRange(5, 2).getValue() === true || configSheet.getRange(5, 2).getValue() === 'TRUE';
  const adminEmail = configSheet.getRange(6, 2).getValue() || '';
  
  // Read user list (starting from row 9)
  const userStartRow = 9;
  const userLastRow = configSheet.getLastRow();
  const users = [];
  
  if (userLastRow >= userStartRow) {
    const userRange = configSheet.getRange(userStartRow, 2, userLastRow - userStartRow + 1, 2).getValues();
    userRange.forEach(row => {
      if (row[0] && row[0] !== '') {
        users.push({
          email: row[0],
          name: row[1] || row[0]
        });
      }
    });
  }
  
  return {
    currentPeriod,
    currentYear,
    deadline,
    reminderEnabled,
    adminEmail,
    users
  };
}

/**
 * Test function to verify setup
 */
function testSetup() {
  try {
    const config = getConfigValues();
    Logger.log('Config loaded successfully:');
    Logger.log(config);
    
    const ui = SpreadsheetApp.getUi();
    ui.alert('✅ Setup test passed!\n\n' +
      `Period: ${config.currentPeriod}\n` +
      `Year: ${config.currentYear}\n` +
      `Deadline: ${config.deadline}\n` +
      `Users: ${config.users.length}\n` +
      `Admin: ${config.adminEmail}`);
  } catch (error) {
    SpreadsheetApp.getUi().alert('❌ Setup test failed:\n\n' + error.message);
  }
}






