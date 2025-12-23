/**
 * TDG Forecast Automation System - Version 2 (Column-Based Design)
 * 
 * Features:
 * - Individual columns for each forecast period (AOP 0.5, AOP 1, AOP Final, Q1M1-Q4M3)
 * - One Forecast ID per item (not per period)
 * - Track forecast evolution across all periods
 * - Period-specific reminders
 * - Open items dashboard
 * - Variance tracking across periods
 * - Auto-fill "Carry To" when status changes
 * - Complete change history logging
 */

// ============================================================================
// CONFIGURATION
// ============================================================================

const CONFIG_V2 = {
  // Sheet Names
  FORECAST_SHEET: 'Forecast',
  NOTE_SHEET: 'Note',
  OPEN_ITEMS_SHEET: 'Open Items Dashboard',
  VARIANCE_SHEET: 'Variance Analysis',
  CONFIG_SHEET: 'Config',
  
  // Column positions for new design
  // OPTIMIZED: Description and Remark moved to end for narrower data entry view
  COLS: {
    // Core Info (A-E) - Narrow view for data entry
    FORECAST_ID: 1,      // A
    REQUESTOR: 2,        // B
    CATEGORY: 3,         // C
    PROJECT_NAME: 4,     // D
    ITEM_NAME: 5,        // E
    
    // AOP Round 0.5 - Q1 to Q4 (F-I)
    AOP05_Q1: 6,         // F
    AOP05_Q2: 7,         // G
    AOP05_Q3: 8,         // H
    AOP05_Q4: 9,         // I
    
    // AOP Round 1 - Q1 to Q4 (J-M)
    AOP1_Q1: 10,         // J
    AOP1_Q2: 11,         // K
    AOP1_Q3: 12,         // L
    AOP1_Q4: 13,         // M
    
    // AOP Final - Q1 to Q4 (N-Q)
    AOPF_Q1: 14,         // N
    AOPF_Q2: 15,         // O
    AOPF_Q3: 16,         // P
    AOPF_Q4: 17,         // Q
    
    // Quarterly Forecasts (R-Y)
    Q1M1: 18,            // R
    Q1M3: 19,            // S
    Q2M1: 20,            // T
    Q2M3: 21,            // U
    Q3M1: 22,            // V
    Q3M3: 23,            // W
    Q4M1: 24,            // X
    Q4M3: 25,            // Y
    
    // Summary & Tracking (Z-AD)
    TOTAL_FY: 26,        // Z (Renamed to "Total" in header)
    CARRY_TO: 27,        // AA
    STATUS: 28,          // AB
    PO_AMOUNT: 29,       // AC - Amount (PO)
    PO_NUMBER: 30,       // AD
    
    // Non-critical info (AE-AG) - Moved to end
    DESCRIPTION: 31,     // AE
    REMARK: 32,          // AF
    
    // Auto-tracking (AG-AJ)
    EST_RECEIVING_DATE: 33, // AG - NEW
    LAST_UPDATED: 34,    // AH
    UPDATED_BY: 35,      // AI
    CHANGE_LOG: 36       // AJ
  },
  
  // Period definitions with deadlines (updated column positions)
  PERIODS: {
    'AOP 0.5 Q1': { col: 6, deadline: '2025-07-11', label: 'AOP 0.5 Q1' },
    'AOP 0.5 Q2': { col: 7, deadline: '2025-07-11', label: 'AOP 0.5 Q2' },
    'AOP 0.5 Q3': { col: 8, deadline: '2025-07-11', label: 'AOP 0.5 Q3' },
    'AOP 0.5 Q4': { col: 9, deadline: '2025-07-11', label: 'AOP 0.5 Q4' },
    'AOP 1 Q1': { col: 10, deadline: '2025-07-30', label: 'AOP 1 Q1' },
    'AOP 1 Q2': { col: 11, deadline: '2025-07-30', label: 'AOP 1 Q2' },
    'AOP 1 Q3': { col: 12, deadline: '2025-07-30', label: 'AOP 1 Q3' },
    'AOP 1 Q4': { col: 13, deadline: '2025-07-30', label: 'AOP 1 Q4' },
    'AOP Final Q1': { col: 14, deadline: '2025-10-15', label: 'AOP Final Q1' },
    'AOP Final Q2': { col: 15, deadline: '2025-10-15', label: 'AOP Final Q2' },
    'AOP Final Q3': { col: 16, deadline: '2025-10-15', label: 'AOP Final Q3' },
    'AOP Final Q4': { col: 17, deadline: '2025-10-15', label: 'AOP Final Q4' },
    'Q1M1': { col: 18, deadline: '2025-10-29', label: 'Q1M1' },
    'Q1M3': { col: 19, deadline: '2025-12-20', label: 'Q1M3' },
    'Q2M1': { col: 20, deadline: '2026-01-20', label: 'Q2M1' },
    'Q2M3': { col: 21, deadline: '2026-03-20', label: 'Q2M3' },
    'Q3M1': { col: 22, deadline: '2026-04-20', label: 'Q3M1' },
    'Q3M3': { col: 23, deadline: '2026-06-20', label: 'Q3M3' },
    'Q4M1': { col: 24, deadline: '2026-07-20', label: 'Q4M1' },
    'Q4M3': { col: 25, deadline: '2026-09-20', label: 'Q4M3' }
  },
  
  // Status values
  STATUS_VALUES: [
    'Draft',
    'Placeholder',        // NEW: For placeholder/estimated items
    'Active',
    'Approved',
    'PR Raised',          // NEW: When PR is submitted
    'In Progress',
    'Completed',
    'Unbudgeted',         // NEW: For unbudgeted items
    'Carry To Q1',        // NEW: Manual Carry
    'Carry To Q2',
    'Carry To Q3',
    'Carry To Q4',
    'Carry To Next FY',
    'On Hold',
    'Cancelled',
    'Rejected'
  ],
  
  // Email settings
  EMAIL_SUBJECT_PREFIX: '[TDG Forecast]',
  REMINDER_DAYS: [7, 3, 1, 0], // 7 days, 3 days, 1 day, day of deadline

  // External PR Sheet Config
  PR_SPREADSHEET_ID: '168MzTav9Wlhkm_hPmSuIT6yEvCNHFENf16h-oiYnqWA',
  PR_SHEET_NAME: '2) FY26 PR request form',
  
  // Category Mapping (PR Category -> Forecast Category)
  CATEGORY_MAPPING: {
    'Test Hardware/svc': '21A) Test hardware (TDG)',
    'Tooling': '22A) Tooling (TDG)',
    'Computer/Software': '4A) Computer/Software (TDG)',
    'Equipment, cal, and repairs': '8A) Equipment, cal, and repairs (TDG)',
    'Processing Supplies': '18A) Processing Supplies (TDG)',
    'Freight': '13D) Freight (DHL DGF)',
    // Add more if needed (key = PR value, value = Forecast value)
  },

  // PR Sheet Columns (1-based index)
  PR_COLS: {
    FORECAST_ID: 1,       // A
    EXPENSES_GROUP: 7,    // G (Not used for mapping, but might be needed for reference)
    REQUESTOR: 8,         // H
    LINE_NUMBER: 9,       // I -> PR Line Number
    ITEM_NAME: 10,        // J -> Item Name
    JUSTIFICATION: 11,    // K -> Project Name
    BUDGET_STATUS: 12,    // L
    EXPENSES_CATEGORY: 18,// R -> Category
    AMOUNT: 48,           // AV
    PO_NUMBER: 50,        // AX
    RECEIPT_NUMBER: 56,   // BD
    RECEIPT_DATE: 57,     // BE
    RECEIVED_AMOUNT: 58,  // BF
    ESTIMATED_RECEIVING_DATE: 27 // AA
  }
};

// ============================================================================
// HELPER: DATE TO QUARTER
// ============================================================================

function getQuarterFromDate(dateObj) {
  if (!dateObj) return null;
  
  let d = dateObj;
  // Handle string inputs (e.g. from text formatted cells)
  if (!(d instanceof Date)) {
    d = new Date(dateObj);
  }
  
  if (isNaN(d.getTime())) return null;
  
  const month = d.getMonth(); // 0 = Jan, 11 = Dec
  
  // Q1: Oct, Nov, Dec (9, 10, 11)
  if (month >= 9) return 'Q1';
  
  // Q2: Jan, Feb, Mar (0, 1, 2)
  if (month <= 2) return 'Q2';
  
  // Q3: Apr, May, Jun (3, 4, 5)
  if (month >= 3 && month <= 5) return 'Q3';
  
  // Q4: Jul, Aug, Sep (6, 7, 8)
  if (month >= 6 && month <= 8) return 'Q4';
  
  return null;
}

// ============================================================================
// INITIALIZATION & MENU
// ============================================================================

function onOpen() {
  const ui = SpreadsheetApp.getUi();
  ui.createMenu('📊 TDG Forecast (v2)')
    .addSubMenu(ui.createMenu('👁️ Switch View')
      .addItem('📅 AOP Planning (Budget Only)', 'viewMode_AOP')
      .addItem('🚀 Execution (Forecast Only)', 'viewMode_Execution')
      .addItem('🎯 Focus: Active Period', 'viewMode_FocusActive')
      .addItem('📝 Show All Columns', 'viewMode_All'))
    .addSeparator()
    .addItem('⚡ Initialize Forecast from AOP', 'seedForecastFromAOP')
    .addSeparator()
    .addItem('🎯 Generate Forecast IDs', 'generateForecastIDs_v2')
    .addSeparator()
    .addItem('🔄 Sync Unbudgeted PRs', 'syncUnbudgetedItems')
    .addItem('🔄 Sync PO Numbers (Budgeted)', 'syncPONumbersFromPR')
    .addItem('🔄 Update Open Items Dashboard', 'updateOpenItemsDashboard')
    .addItem('📊 Generate Variance Analysis', 'generateVarianceAnalysis')
    .addItem('📦 List Placeholder Items', 'listPlaceholderItems')
    .addSeparator()
    .addItem('📧 Send Period Reminders', 'sendPeriodReminders')
    .addItem('⚠️ Check Overdue POs', 'checkOverduePOAndNotify') // NEW Menu Item
    .addItem('📋 Send Forecast IDs to Users', 'sendForecastIDsToUsers') // NEW Menu Item
    .addItem('👥 Check User Completion Status', 'checkUserCompletionStatus')
    .addSeparator()
    .addItem('⚙️ Setup Data Validation', 'setupDataValidation')
    .addItem('🔢 Setup Total Row', 'setupTotalRow')
    .addItem('🎨 Setup Visual Formatting', 'setupVisualFormatting')
    .addItem('📋 Initialize Config', 'initializeConfig_v2')
    .addItem('⏰ Setup Automated Triggers', 'setupTriggers_v2')
    .addToUi();
}

// ============================================================================
// CONFIGURATION INITIALIZATION
// ============================================================================

function initializeConfig_v2() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  let configSheet = ss.getSheetByName(CONFIG_V2.CONFIG_SHEET);
  
  if (!configSheet) {
    configSheet = ss.insertSheet(CONFIG_V2.CONFIG_SHEET);
  }
  
  configSheet.clear();
  
  // Settings section
  const settings = [
    ['Setting', 'Value', 'Description'],
    ['Current Fiscal Year', 'FY25', 'Current fiscal year (e.g., FY25, FY26)'],
    ['Active Period', 'Q1M1', 'Currently active forecast period'],
    ['Active Period Deadline', '2025-10-29', 'Deadline for active period (YYYY-MM-DD)'],
    ['Reminder Enabled', 'TRUE', 'Enable/disable email reminders'],
    ['Admin Email', '', 'Admin email for notifications'],
    ['Auto Fill Carry To', 'TRUE', 'Auto-fill Carry To amount when status changes'],
    ['']
  ];
  
  configSheet.getRange(1, 1, settings.length, 3).setValues(settings);
  
  // Format headers
  configSheet.getRange(1, 1, 1, 3)
    .setBackground('#4285f4')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  
  configSheet.autoResizeColumns(1, 3);
  
  SpreadsheetApp.getUi().alert('✅ Config initialized!\n\nPlease update the settings in the Config sheet.');
}

// ============================================================================
// FORECAST ID GENERATION
// ============================================================================

function generateForecastIDs_v2() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const config = getConfigValues_v2();
  const lastRow = forecastSheet.getLastRow();
  
  if (lastRow < 4) { // Row 1 = deadlines, Row 2 = totals, Row 3 = headers
    SpreadsheetApp.getUi().alert('ℹ️ No data to process.');
    return;
  }
  
  let generatedCount = 0;
  const currentUser = Session.getActiveUser().getEmail();
  
  // Start from row 4 (data rows)
  for (let row = 4; row <= lastRow; row++) {
    const idCell = forecastSheet.getRange(row, CONFIG_V2.COLS.FORECAST_ID);
    const currentId = idCell.getValue();
    
    // Only generate ID if cell is empty and row has data
    if (!currentId || currentId === '') {
      const requestor = forecastSheet.getRange(row, CONFIG_V2.COLS.REQUESTOR).getValue();
      const category = forecastSheet.getRange(row, CONFIG_V2.COLS.CATEGORY).getValue();
      
      if (requestor || category) { // If row has any data
        const newId = generateUniqueId_v2(config.fiscalYear);
        idCell.setValue(newId);
        
        // Add timestamp and user
        forecastSheet.getRange(row, CONFIG_V2.COLS.LAST_UPDATED).setValue(new Date());
        forecastSheet.getRange(row, CONFIG_V2.COLS.UPDATED_BY).setValue(currentUser);
        
        // Log creation
        const logCell = forecastSheet.getRange(row, CONFIG_V2.COLS.CHANGE_LOG);
        const log = `Created on ${Utilities.formatDate(new Date(), Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')} by ${currentUser}`;
        logCell.setValue(log);
        logCell.setWrap(false).setWrapStrategy(SpreadsheetApp.WrapStrategy.CLIP);
        
        // Fix row height to prevent excessive height
        forecastSheet.setRowHeight(row, 21); // Standard row height
        
        generatedCount++;
      }
    }
  }
  
  SpreadsheetApp.getUi().alert(`✅ Generated ${generatedCount} new Forecast IDs!`);
}

function generateUniqueId_v2(fiscalYear) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  // Get all existing IDs
  const lastRow = forecastSheet.getLastRow();
  // Format: T{YY}Q{Q}-{SEQ} e.g. T26Q1-001
  
  // Get Fiscal Year Short Code (e.g. FY26 -> 26)
  const fyShort = fiscalYear.replace('FY', '');
  const config = getConfigValues_v2();
  // Extract Q number from active period (e.g. Q1M1 -> 1)
  const qMatch = config.activePeriod.match(/Q(\d)/);
  const qNum = qMatch ? qMatch[1] : '1';
  
  const prefix = `T${fyShort}Q${qNum}`; // e.g. T26Q1
  
  if (lastRow < 4) return `${prefix}-001`;
  
  const existingIds = forecastSheet.getRange(4, CONFIG_V2.COLS.FORECAST_ID, lastRow - 3, 1)
    .getValues()
    .flat()
    .filter(id => id && String(id).startsWith(prefix));
  
  // Find highest sequence number
  let maxSeq = 0;
  existingIds.forEach(id => {
    const match = String(id).match(/-(\d+)$/);
    if (match) {
      const seq = parseInt(match[1]);
      if (seq > maxSeq) maxSeq = seq;
    }
  });
  
  // Generate new ID
  const newSeq = String(maxSeq + 1).padStart(3, '0');
  return `${prefix}-${newSeq}`;
}

// ============================================================================
// TOTAL ROW SETUP
// ============================================================================

function setupTotalRow() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const lastRow = forecastSheet.getLastRow();
  if (lastRow < 4) {
    SpreadsheetApp.getUi().alert('⚠️ No data rows found. Add some data first.');
    return;
  }
  
  // Row 2 will contain totals
  const totalRow = 2;
  
  // Set label in column A
  forecastSheet.getRange(totalRow, 1).setValue('TOTAL');
  
  // Define which columns should have sum formulas (all cost/amount columns)
  const sumColumns = [
    // AOP Round 0.5 (H-K)
    CONFIG_V2.COLS.AOP05_Q1, CONFIG_V2.COLS.AOP05_Q2, CONFIG_V2.COLS.AOP05_Q3, CONFIG_V2.COLS.AOP05_Q4,
    // AOP Round 1 (L-O)
    CONFIG_V2.COLS.AOP1_Q1, CONFIG_V2.COLS.AOP1_Q2, CONFIG_V2.COLS.AOP1_Q3, CONFIG_V2.COLS.AOP1_Q4,
    // AOP Final (P-S)
    CONFIG_V2.COLS.AOPF_Q1, CONFIG_V2.COLS.AOPF_Q2, CONFIG_V2.COLS.AOPF_Q3, CONFIG_V2.COLS.AOPF_Q4,
    // M1 columns (Forecasts) - T, V, X, Z
    CONFIG_V2.COLS.Q1M1, CONFIG_V2.COLS.Q2M1, CONFIG_V2.COLS.Q3M1, CONFIG_V2.COLS.Q4M1,
    // M3 columns (Actuals) - U, W, Y, AA
    CONFIG_V2.COLS.Q1M3, CONFIG_V2.COLS.Q2M3, CONFIG_V2.COLS.Q3M3, CONFIG_V2.COLS.Q4M3,
    // NOTE: Total FY (AB) excluded from sumColumns because it has a special formula
    // Carry To (AC)
    CONFIG_V2.COLS.CARRY_TO
  ];
  
  // Add SUM formulas for each column
  sumColumns.forEach(col => {
    const columnLetter = columnToLetter(col);
    const formula = `=SUM(${columnLetter}4:${columnLetter})`;
    forecastSheet.getRange(totalRow, col).setFormula(formula);
  });
  
  // Add special formula for Total FY (AB) in Row 2
  // This sums all Total FY values from data rows (AB4:AB)
  const totalFYLetter = columnToLetter(CONFIG_V2.COLS.TOTAL_FY);
  forecastSheet.getRange(totalRow, CONFIG_V2.COLS.TOTAL_FY).setFormula(`=SUM(${totalFYLetter}4:${totalFYLetter})`);
  
  // Format the total row
  const totalRange = forecastSheet.getRange(totalRow, 1, 1, CONFIG_V2.COLS.CHANGE_LOG);
  totalRange.setBackground('#FFF2CC')  // Light yellow background
    .setFontWeight('bold')
    .setFontSize(11);
  
  // Format number columns as currency with $ and thousand separator
  sumColumns.forEach(col => {
    forecastSheet.getRange(totalRow, col).setNumberFormat('$#,##0.00');
  });
  
  // Also format Total FY column (AB) in Row 2
  forecastSheet.getRange(totalRow, CONFIG_V2.COLS.TOTAL_FY).setNumberFormat('$#,##0.00');
  
  // Also format all data rows with currency format
  const lastDataRow = forecastSheet.getLastRow();
  if (lastDataRow >= 4) {
    sumColumns.forEach(col => {
      forecastSheet.getRange(4, col, lastDataRow - 3, 1).setNumberFormat('$#,##0.00');
    });
    // Format Total FY column in data rows
    forecastSheet.getRange(4, CONFIG_V2.COLS.TOTAL_FY, lastDataRow - 3, 1).setNumberFormat('$#,##0.00');
  }
  
  SpreadsheetApp.getUi().alert('✅ Total row setup complete!\n\n' +
    'Row 2 now shows totals with $ format.\n' +
    'All amount columns formatted as currency.\n' +
    'Formula: =SUM(column4:column)');
}

// Helper function to convert column number to letter
function columnToLetter(column) {
  let temp, letter = '';
  while (column > 0) {
    temp = (column - 1) % 26;
    letter = String.fromCharCode(temp + 65) + letter;
    column = (column - temp - 1) / 26;
  }
  return letter;
}

// ============================================================================
// VISUAL FORMATTING & COLUMN SETUP
// ============================================================================

function setupVisualFormatting() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const lastRow = Math.max(forecastSheet.getLastRow(), 500);
  
  // 1. Set column widths for better visibility
  // Core Info (A-E) - Narrow for easy data entry
  forecastSheet.setColumnWidth(1, 120);  // A - Forecast ID
  forecastSheet.setColumnWidth(2, 150);  // B - Requestor
  forecastSheet.setColumnWidth(3, 200);  // C - Category
  forecastSheet.setColumnWidth(4, 150);  // D - Project Name
  forecastSheet.setColumnWidth(5, 200);  // E - Item Name
  
  // AOP and Quarterly columns (F-Y) - consistent width
  for (let col = 6; col <= 25; col++) { // F to Y
    forecastSheet.setColumnWidth(col, 120);
  }
  
  // Summary columns (Z-AD)
  forecastSheet.setColumnWidth(26, 120);  // Z - Total FY
  forecastSheet.setColumnWidth(27, 120);  // AA - Carry To
  forecastSheet.setColumnWidth(28, 150);  // AB - Status
  forecastSheet.setColumnWidth(29, 120);  // AC - Amount (PO)
  forecastSheet.setColumnWidth(30, 120);  // AD - PO #
  
  // Non-critical info (AE-AF) - Moved to end
  forecastSheet.setColumnWidth(31, 250);  // AE - Description
  forecastSheet.setColumnWidth(32, 150);  // AF - Remark
  
  // Auto-tracking columns (AG-AJ)
  forecastSheet.setColumnWidth(33, 120);  // AG - Est. Receiving Date
  forecastSheet.setColumnWidth(34, 150);  // AH - Last Updated
  forecastSheet.setColumnWidth(35, 180);  // AI - Updated By
  forecastSheet.setColumnWidth(36, 300);  // AJ - Change Log
  
  // 2. Text wrapping configuration
  // E - Item Name -> Wrap
  forecastSheet.getRange(4, 5, lastRow - 3, 1).setWrap(true);   
  // AE - Description -> Wrap
  forecastSheet.getRange(4, 31, lastRow - 3, 1).setWrap(true);
  // AF - Remark -> Wrap
  forecastSheet.getRange(4, 32, lastRow - 3, 1).setWrap(true);
  
  // AJ - Change Log -> CLIP (Don't wrap, so row height stays 21)
  // Use explicit wrap strategy on the whole range
  forecastSheet.getRange(4, CONFIG_V2.COLS.CHANGE_LOG, lastRow - 3, 1)
    .setWrap(false)
    .setWrapStrategy(SpreadsheetApp.WrapStrategy.CLIP);
  
  // 3. Clip text for amount columns (prevent overflow)
  for (let col = 6; col <= 30; col++) { // F to AD (all amount/summary columns)
    forecastSheet.getRange(4, col, lastRow - 3, 1).setWrap(false).setWrapStrategy(SpreadsheetApp.WrapStrategy.CLIP);
  }
  
  // 4. Align amount columns to RIGHT
  for (let col = 6; col <= 30; col++) { // F to AD
    forecastSheet.getRange(3, col, lastRow - 2, 1).setHorizontalAlignment('right');
  }
  
  // 5. Freeze rows and columns for easy navigation
  forecastSheet.setFrozenRows(3);  // Freeze Row 1-3 (Deadline, Total, Headers)
  forecastSheet.setFrozenColumns(5); // Freeze Column A-E (Core info for data entry: ID, Requestor, Category, Project, Item)
  
  // NEW: Update Header for Column AC (PO Amount) and Z (Total)
  forecastSheet.getRange(3, CONFIG_V2.COLS.PO_AMOUNT).setValue('Amount (PO)');
  forecastSheet.getRange(3, CONFIG_V2.COLS.TOTAL_FY).setValue('Total');
  forecastSheet.getRange(3, CONFIG_V2.COLS.EST_RECEIVING_DATE).setValue('Est. Delivery');
  
  // Update Auto-tracking headers
  forecastSheet.getRange(3, CONFIG_V2.COLS.LAST_UPDATED).setValue('Last Updated');
  forecastSheet.getRange(3, CONFIG_V2.COLS.UPDATED_BY).setValue('Updated By');
  forecastSheet.getRange(3, CONFIG_V2.COLS.CHANGE_LOG).setValue('Change Log');
  
  // 6. Add borders to separate sections
  // Border after Core Info (Column E)
  forecastSheet.getRange(1, 5, lastRow, 1).setBorder(null, null, null, true, null, null, '#000000', SpreadsheetApp.BorderStyle.SOLID_MEDIUM);
  
  // Border after AOP Final (Column Q)
  forecastSheet.getRange(1, 17, lastRow, 1).setBorder(null, null, null, true, null, null, '#000000', SpreadsheetApp.BorderStyle.SOLID_MEDIUM);
  
  // Border after Quarterly (Column Y)
  forecastSheet.getRange(1, 25, lastRow, 1).setBorder(null, null, null, true, null, null, '#000000', SpreadsheetApp.BorderStyle.SOLID_MEDIUM);
  
  // Border after Summary (Column AD)
  forecastSheet.getRange(1, 30, lastRow, 1).setBorder(null, null, null, true, null, null, '#000000', SpreadsheetApp.BorderStyle.SOLID_MEDIUM);

  // Border after Est. Delivery (Column AG) - NEW
  forecastSheet.getRange(1, 33, lastRow, 1).setBorder(null, null, null, true, null, null, '#000000', SpreadsheetApp.BorderStyle.SOLID_MEDIUM);
  
  // 7. Background colors for section headers (Row 3)
  forecastSheet.getRange(3, 1, 1, 5).setBackground('#E8F4FD');    // Core Info (A-E) - Light Blue
  forecastSheet.getRange(3, 6, 1, 12).setBackground('#FFF4E6');   // AOP columns (F-Q) - Light Orange
  forecastSheet.getRange(3, 18, 1, 8).setBackground('#E8F5E9');   // Quarterly (R-Y) - Light Green
  forecastSheet.getRange(3, 26, 1, 5).setBackground('#F3E5F5');   // Summary (Z-AD) - Light Purple
  forecastSheet.getRange(3, 31, 1, 2).setBackground('#FFF9C4');   // Description/Remark (AE-AF) - Light Yellow
  forecastSheet.getRange(3, 33, 1, 1).setBackground('#D2E3FC');   // Est Date (AG) - Light Blue 2
  forecastSheet.getRange(3, 34, 1, 3).setBackground('#E0E0E0');   // Auto-tracking (AH-AJ) - Light Gray
  
  // 8. Set standard row height for all data rows (Force 21)
  // Use setRowHeights (plural) for better performance and reliability
  forecastSheet.setRowHeights(4, lastRow - 3, 21);
  
  // Explicitly call flush to ensure UI updates
  SpreadsheetApp.flush();
  
  // 9. Bold and center align headers (Row 3)
  forecastSheet.getRange(3, 1, 1, 36).setFontWeight('bold').setHorizontalAlignment('center');
  
  // NEW: Apply Currency Formatting to a large range so new rows are ready
  const currencyColumns = [
    // AOP 0.5 (F-I)
    6, 7, 8, 9,
    // AOP 1 (J-M)
    10, 11, 12, 13,
    // AOP Final (N-Q)
    14, 15, 16, 17,
    // Forecast & Actuals (R-Y)
    18, 19, 20, 21, 22, 23, 24, 25,
    // Total FY (Z) & Carry To (AA) & PO Amount (AC)
    26, 27, 29
  ];
  
  // Format cells as Currency ($)
  currencyColumns.forEach(col => {
    forecastSheet.getRange(4, col, lastRow - 3, 1).setNumberFormat('$#,##0.00');
  });

  // 10. Setup Total FY formulas for existing data rows
  setupTotalFYFormulas(forecastSheet, 4, lastRow);
  
  // 11. Setup Conditional Formatting for Status
  // First, CLEAR any manual background colors in the data range to prevent "Yellow" residue from Syncs
  // This ensures Conditional Formatting is the sole source of truth for colors
  forecastSheet.getRange(4, 1, lastRow - 3, 36).setBackground(null);
  
  setupStatusConditionalFormatting(forecastSheet, lastRow);
  
  SpreadsheetApp.getUi().alert('✅ Visual formatting complete!\n\n' +
    '✓ Column widths optimized\n' +
    '✓ Text wrapping configured\n' +
    '✓ Amount columns right-aligned\n' +
    '✓ Rows 1-3 frozen\n' +
    '✓ Section borders added\n' +
    '✓ Header colors applied\n' +
    '✓ Standard row heights set\n' +
    '✓ Total FY formulas added\n' +
    '✓ Conditional formatting applied');
}

function setupStatusConditionalFormatting(sheet, lastRow) {
  // Define range for the entire data block (A4 to AJ[lastRow])
  // Note: 36 columns total (A-AJ)
  const dataRange = sheet.getRange(4, 1, lastRow - 3, 36);
  
  const statusColLetter = columnToLetter(CONFIG_V2.COLS.STATUS);
  const categoryColLetter = columnToLetter(CONFIG_V2.COLS.CATEGORY);
  
  // 1. Completed Rule (Grey) - Highest Priority
  // = $AB4="Completed"
  const completedFormula = `=$${statusColLetter}4="Completed"`;
  const completedRule = SpreadsheetApp.newConditionalFormatRule()
    .whenFormulaSatisfied(completedFormula)
    .setBackground('#E0E0E0') // Light Grey
    .setFontColor('#000000')
    .setRanges([dataRange])
    .build();

  // 2. Category 21A Rule (Blue) - Test Hardware
  // = AND($AB4="Unbudgeted", SEARCH("21A", $C4))
  const cat21AFormula = `=AND($${statusColLetter}4="Unbudgeted", SEARCH("21A", $${categoryColLetter}4))`;
  const cat21ARule = SpreadsheetApp.newConditionalFormatRule()
    .whenFormulaSatisfied(cat21AFormula)
    .setBackground('#CFE2F3') // Light Blue
    .setFontColor('#000000')
    .setRanges([dataRange])
    .build();

  // 3. Category 22A Rule (Green) - Tooling
  // = AND($AB4="Unbudgeted", SEARCH("22A", $C4))
  const cat22AFormula = `=AND($${statusColLetter}4="Unbudgeted", SEARCH("22A", $${categoryColLetter}4))`;
  const cat22ARule = SpreadsheetApp.newConditionalFormatRule()
    .whenFormulaSatisfied(cat22AFormula)
    .setBackground('#D9EAD3') // Light Green
    .setFontColor('#000000')
    .setRanges([dataRange])
    .build();
    
  // Retrieve existing rules
  let rules = sheet.getConditionalFormatRules();
  
  // Clean up existing rules to avoid duplication
  // We remove any rules that use these specific background colors to ensure a clean slate
  // Using toLowerCase() to ensure case-insensitive matching
  rules = rules.filter(r => {
    const booleanCondition = r.getBooleanCondition();
    // Only check background if it's a boolean condition (not gradient)
    const bg = (booleanCondition ? (booleanCondition.getBackground() || '') : '').toLowerCase();
    
    // Remove if it matches our target colors (#e0e0e0, #cfe2f3, #d9ead3)
    if (bg === '#e0e0e0' || bg === '#cfe2f3' || bg === '#d9ead3') return false;
    return true;
  });
  
  // Add new rules in REVERSE order of priority (unshift adds to top)
  // We want Priority: Completed (Grey) > 21A (Blue) > 22A (Green)
  
  // 3. Green (Lowest of the 3)
  rules.unshift(cat22ARule);
  
  // 2. Blue (Middle)
  rules.unshift(cat21ARule);
  
  // 1. Grey (Highest)
  rules.unshift(completedRule);
  
  sheet.setConditionalFormatRules(rules);
}

// Helper function to setup Total FY formulas
function setupTotalFYFormulas(sheet, startRow, endRow) {
  // Add formula to each row's Total FY column (Z = column 26)
  for (let row = startRow; row <= endRow; row++) {
    const totalFYCell = sheet.getRange(row, CONFIG_V2.COLS.TOTAL_FY);
    
    // Formula: Sum M3 if available, otherwise M1 (Forecast)
    // Logic: IF(M3<>"", M3, M1) for each quarter
    // R=Q1M1, S=Q1M3
    // T=Q2M1, U=Q2M3
    // V=Q3M1, W=Q3M3
    // X=Q4M1, Y=Q4M3
    const formula = `=IF(S${row}<>"",S${row},R${row}) + IF(U${row}<>"",U${row},T${row}) + IF(W${row}<>"",W${row},V${row}) + IF(Y${row}<>"",Y${row},X${row})`;
    totalFYCell.setFormula(formula);
  }
}

// ============================================================================
// DATA VALIDATION SETUP
// ============================================================================

function setupDataValidation() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  const noteSheet = ss.getSheetByName(CONFIG_V2.NOTE_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const lastRow = Math.max(forecastSheet.getLastRow(), 100); // At least 100 rows
  
  // Get requestors and categories from Note sheet
  // (Start with empty arrays)
  let requestors = [];
  let categories = [];
  
  if (noteSheet) {
    // Assuming Note sheet has: Column A = Requestors, Column B = Categories
    const noteData = noteSheet.getDataRange().getValues();
    requestors = noteData.slice(1).map(row => row[0]).filter(val => val);
    categories = noteData.slice(1).map(row => row[1]).filter(val => val);
    
    // Remove duplicates
    requestors = [...new Set(requestors)];
    categories = [...new Set(categories)];
  } else {
    // Default values if Note sheet doesn't exist
    requestors = [
      'Chen, Weng Cheow',
      'Kim, Hyeong Cheol',
      'Koay, Choon Chin',
      'Koh, Kuok Liang',
      'Lai, Yu Wai',
      'Lam, Tuck Hon',
      'Lim, Kwok Hau Ronald',
      'Low, Jeen Jang',
      'Low, Say Aun',
      'Mah, Wan Ling',
      'Ng, Keng Shan',
      'Ong, Chee On',
      'Ong, Roey Jiea',
      'Tan, Ee Tieng',
      'Tan, Tai Seng',
      'Wong, Chia Yeak',
      'Wong, Chor Ming'
    ];
    
    categories = [
      '3A) Components (TDG)',
      '4A) Computer/Software (TDG)',
      '8A) Equipment, cal, and repairs (TDG)',
      '13D) Freight (DHL DGF)',
      '16A) Other Consumables (TDG)',
      '18A) Processing Supplies (TDG)',
      '19A) Substrates (TDG)',
      '21A) Test hardware (TDG)',
      '22A) Tooling (TDG)'
    ];
  }
  
  // Setup Requestor dropdown (Column B) - start from row 4
  const requestorRule = SpreadsheetApp.newDataValidation()
    .requireValueInList(requestors, true)
    .setAllowInvalid(true)
    .build();
  
  // Apply to a large range (e.g. 500 rows) so new rows are ready
  const targetRows = Math.max(lastRow, 500);
  
  forecastSheet.getRange(4, CONFIG_V2.COLS.REQUESTOR, targetRows - 3, 1)
    .setDataValidation(requestorRule);
  
  // Setup Category dropdown (Column C)
  // Allow invalid = true means users can type custom categories
  const categoryRule = SpreadsheetApp.newDataValidation()
    .requireValueInList(categories, true)
    .setAllowInvalid(true)  // Changed to true - allows custom entry
    .setHelpText('Select from list or type custom category')
    .build();
  
  forecastSheet.getRange(4, CONFIG_V2.COLS.CATEGORY, targetRows - 3, 1)
    .setDataValidation(categoryRule);
  
  // Setup Status dropdown (Column AB in new layout)
  const statusRule = SpreadsheetApp.newDataValidation()
    .requireValueInList(CONFIG_V2.STATUS_VALUES, true)
    .setAllowInvalid(false)
    .build();
  forecastSheet.getRange(4, CONFIG_V2.COLS.STATUS, targetRows - 3, 1)
    .setDataValidation(statusRule);
  
  // Add placeholder hints for Category column (Column C)
  // Light gray text to indicate it's a placeholder
  const categoryRange = forecastSheet.getRange(4, CONFIG_V2.COLS.CATEGORY, targetRows - 3, 1);
  
  SpreadsheetApp.getUi().alert('✅ Data validation setup complete!\n\n' +
    `Requestors: ${requestors.length}\n` +
    `Categories: ${categories.length} (custom entry allowed)\n` +
    `Status values: ${CONFIG_V2.STATUS_VALUES.length}`);
}

// ============================================================================
// OPEN ITEMS DASHBOARD
// ============================================================================

function updateOpenItemsDashboard() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  // Get all forecast data
  const data = getForecastData_v2(forecastSheet);
  
  // Analyze data
  const analysis = {
    byStatus: analyzeByStatus(data),
    byRequestor: analyzeByRequestor(data),
    byCategory: analyzeByCategory(data),
    incompletePeriods: analyzeIncompletePeriods(data),
    overdueItems: analyzeOverdueItems(data) // New Analysis
  };
  
  // Create or update dashboard sheet
  createOpenItemsDashboard(analysis);
  
  SpreadsheetApp.getUi().alert('✅ Open Items Dashboard updated!');
}

function getForecastData_v2(sheet) {
  const lastRow = sheet.getLastRow();
  if (lastRow < 4) return [];
  
  const dataRange = sheet.getRange(4, 1, lastRow - 3, CONFIG_V2.COLS.CHANGE_LOG);
  const values = dataRange.getValues();
  
  return values.map(row => ({
    forecastId: row[CONFIG_V2.COLS.FORECAST_ID - 1],
    requestor: row[CONFIG_V2.COLS.REQUESTOR - 1],
    category: row[CONFIG_V2.COLS.CATEGORY - 1],
    projectName: row[CONFIG_V2.COLS.PROJECT_NAME - 1],
    itemName: row[CONFIG_V2.COLS.ITEM_NAME - 1],
    description: row[CONFIG_V2.COLS.DESCRIPTION - 1],
    estDate: row[CONFIG_V2.COLS.EST_RECEIVING_DATE - 1], // New Field
    
    // AOP values
    aop05_q1: row[CONFIG_V2.COLS.AOP05_Q1 - 1],
    aop05_q2: row[CONFIG_V2.COLS.AOP05_Q2 - 1],
    aop05_q3: row[CONFIG_V2.COLS.AOP05_Q3 - 1],
    aop05_q4: row[CONFIG_V2.COLS.AOP05_Q4 - 1],
    
    aop1_q1: row[CONFIG_V2.COLS.AOP1_Q1 - 1],
    aop1_q2: row[CONFIG_V2.COLS.AOP1_Q2 - 1],
    aop1_q3: row[CONFIG_V2.COLS.AOP1_Q3 - 1],
    aop1_q4: row[CONFIG_V2.COLS.AOP1_Q4 - 1],
    
    aopf_q1: row[CONFIG_V2.COLS.AOPF_Q1 - 1],
    aopf_q2: row[CONFIG_V2.COLS.AOPF_Q2 - 1],
    aopf_q3: row[CONFIG_V2.COLS.AOPF_Q3 - 1],
    aopf_q4: row[CONFIG_V2.COLS.AOPF_Q4 - 1],
    
    // Quarterly forecast values
    q1m1: row[CONFIG_V2.COLS.Q1M1 - 1],
    q1m3: row[CONFIG_V2.COLS.Q1M3 - 1],
    q2m1: row[CONFIG_V2.COLS.Q2M1 - 1],
    q2m3: row[CONFIG_V2.COLS.Q2M3 - 1],
    q3m1: row[CONFIG_V2.COLS.Q3M1 - 1],
    q3m3: row[CONFIG_V2.COLS.Q3M3 - 1],
    q4m1: row[CONFIG_V2.COLS.Q4M1 - 1],
    q4m3: row[CONFIG_V2.COLS.Q4M3 - 1],
    
    totalFY: row[CONFIG_V2.COLS.TOTAL_FY - 1],
    carryTo: row[CONFIG_V2.COLS.CARRY_TO - 1],
    status: row[CONFIG_V2.COLS.STATUS - 1],
    poAmount: row[CONFIG_V2.COLS.PO_AMOUNT - 1],
    poNumber: row[CONFIG_V2.COLS.PO_NUMBER - 1]
  })).filter(item => item.forecastId); // Only items with IDs
}

function analyzeByStatus(data) {
  const statusMap = new Map();
  
  data.forEach(item => {
    const status = item.status || 'Draft';
    if (!statusMap.has(status)) {
      statusMap.set(status, { count: 0, totalAmount: 0 });
    }
    const stats = statusMap.get(status);
    stats.count++;
    stats.totalAmount += parseFloat(item.totalFY) || 0;
  });
  
  return Array.from(statusMap.entries()).map(([status, stats]) => ({
    status,
    count: stats.count,
    totalAmount: stats.totalAmount
  }));
}

function analyzeByRequestor(data) {
  const requestorMap = new Map();
  
  data.forEach(item => {
    const requestor = item.requestor || 'Unassigned';
    if (!requestorMap.has(requestor)) {
      requestorMap.set(requestor, { 
        activeCount: 0, 
        carryForwardCount: 0, 
        totalAmount: 0,
        carryAmount: 0
      });
    }
    const stats = requestorMap.get(requestor);
    
    if (item.status === 'Carry To Next FY') {
      stats.carryForwardCount++;
      stats.carryAmount += parseFloat(item.carryTo) || 0;
    } else if (item.status !== 'Completed' && item.status !== 'Cancelled') {
      stats.activeCount++;
    }
    
    stats.totalAmount += parseFloat(item.totalFY) || 0;
  });
  
  return Array.from(requestorMap.entries()).map(([requestor, stats]) => ({
    requestor,
    activeCount: stats.activeCount,
    carryForwardCount: stats.carryForwardCount,
    totalAmount: stats.totalAmount,
    carryAmount: stats.carryAmount
  }));
}

function analyzeByCategory(data) {
  const categoryMap = new Map();
  
  data.forEach(item => {
    const category = item.category || 'Uncategorized';
    if (!categoryMap.has(category)) {
      categoryMap.set(category, { count: 0, totalAmount: 0 });
    }
    const stats = categoryMap.get(category);
    stats.count++;
    stats.totalAmount += parseFloat(item.totalFY) || 0;
  });
  
  return Array.from(categoryMap.entries()).map(([category, stats]) => ({
    category,
    count: stats.count,
    totalAmount: stats.totalAmount
  }));
}

function analyzeIncompletePeriods(data) {
  const periods = [
    { name: 'Q1M1', col: 'q1m1' },
    { name: 'Q1M3', col: 'q1m3' },
    { name: 'Q2M1', col: 'q2m1' },
    { name: 'Q2M3', col: 'q2m3' },
    { name: 'Q3M1', col: 'q3m1' },
    { name: 'Q3M3', col: 'q3m3' },
    { name: 'Q4M1', col: 'q4m1' },
    { name: 'Q4M3', col: 'q4m3' }
  ];
  
  return periods.map(period => {
    const missing = data.filter(item => !item[period.col] || item[period.col] === '');
    const requestors = [...new Set(missing.map(item => item.requestor))].filter(r => r);
    
    return {
      period: period.name,
      missingCount: missing.length,
      usersAffected: requestors.length,
      requestors: requestors.join(', ')
    };
  }).filter(p => p.missingCount > 0);
}

function analyzeOverdueItems(data) {
  const today = new Date();
  today.setHours(0, 0, 0, 0); // Compare dates only
  
  return data.filter(item => {
    // Check if Status is active (not completed/cancelled)
    const isActive = item.status !== 'Completed' && item.status !== 'Cancelled' && item.status !== 'Rejected';
    
    // Check Date
    let isOverdue = false;
    if (item.estDate && item.estDate instanceof Date) {
      isOverdue = item.estDate < today;
    }
    
    return isActive && isOverdue;
  }).map(item => {
    const diffTime = Math.abs(today - item.estDate);
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)); 
    
    return {
      id: item.forecastId,
      requestor: item.requestor,
      item: item.itemName,
      po: item.poNumber,
      date: Utilities.formatDate(item.estDate, Session.getScriptTimeZone(), 'yyyy-MM-dd'),
      daysOverdue: diffDays,
      status: item.status
    };
  }).sort((a, b) => b.daysOverdue - a.daysOverdue); // Sort by most overdue
}

function createOpenItemsDashboard(analysis) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  let dashboardSheet = ss.getSheetByName(CONFIG_V2.OPEN_ITEMS_SHEET);
  
  if (!dashboardSheet) {
    dashboardSheet = ss.insertSheet(CONFIG_V2.OPEN_ITEMS_SHEET);
  }
  
  dashboardSheet.clear();
  
  let currentRow = 1;
  
  // Title
  dashboardSheet.getRange(currentRow, 1, 1, 5).merge()
    .setValue('Open Items Dashboard')
    .setBackground('#4285f4')
    .setFontColor('#ffffff')
    .setFontWeight('bold')
    .setFontSize(16);
  currentRow += 2;
  
  // By Status
  dashboardSheet.getRange(currentRow, 1, 1, 3)
    .setValues([['Summary by Status', '', '']])
    .setBackground('#34a853')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  currentRow++;
  
  // Header
  dashboardSheet.getRange(currentRow, 1, 1, 3)
    .setValues([['Status', 'Count', 'Total Amount']])
    .setFontWeight('bold');
  currentRow++;
  
  analysis.byStatus.forEach(item => {
    dashboardSheet.getRange(currentRow, 1, 1, 3)
      .setValues([[item.status, item.count, item.totalAmount]]);
    currentRow++;
  });
  currentRow += 2;
  
  // By Requestor
  dashboardSheet.getRange(currentRow, 1, 1, 5)
    .setValues([['Summary by Requestor', '', '', '', '']])
    .setBackground('#fbbc04')
    .setFontWeight('bold');
  currentRow++;
  
  dashboardSheet.getRange(currentRow, 1, 1, 5)
    .setValues([['Requestor', 'Active Items', 'Carry Forward Items', 'Total Amount', 'Carry Amount']])
    .setFontWeight('bold');
  currentRow++;
  
  analysis.byRequestor.forEach(item => {
    dashboardSheet.getRange(currentRow, 1, 1, 5)
      .setValues([[item.requestor, item.activeCount, item.carryForwardCount, item.totalAmount, item.carryAmount]]);
    currentRow++;
  });
  currentRow += 2;
  
  // By Category
  dashboardSheet.getRange(currentRow, 1, 1, 3)
    .setValues([['Summary by Category', '', '']])
    .setBackground('#ea4335')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  currentRow++;
  
  dashboardSheet.getRange(currentRow, 1, 1, 3)
    .setValues([['Category', 'Count', 'Total Amount']])
    .setFontWeight('bold');
  currentRow++;
  
  analysis.byCategory.forEach(item => {
    dashboardSheet.getRange(currentRow, 1, 1, 3)
      .setValues([[item.category, item.count, item.totalAmount]]);
    currentRow++;
  });
  currentRow += 2;
  
  // Incomplete Periods
  if (analysis.incompletePeriods.length > 0) {
    dashboardSheet.getRange(currentRow, 1, 1, 4)
      .setValues([['Incomplete Periods', '', '', '']])
      .setBackground('#9c27b0')
      .setFontColor('#ffffff')
      .setFontWeight('bold');
    currentRow++;
    
    dashboardSheet.getRange(currentRow, 1, 1, 4)
      .setValues([['Period', 'Missing Entries', 'Users Affected', 'Requestors']])
      .setFontWeight('bold');
    currentRow++;
    
    analysis.incompletePeriods.forEach(item => {
      dashboardSheet.getRange(currentRow, 1, 1, 4)
        .setValues([[item.period, item.missingCount, item.usersAffected, item.requestors]]);
      currentRow++;
    });
    currentRow += 2;
  }
  
  // Potential Delays (Overdue Items)
  if (analysis.overdueItems && analysis.overdueItems.length > 0) {
    dashboardSheet.getRange(currentRow, 1, 1, 6)
      .setValues([['⚠️ Potential Delays (Overdue POs)', '', '', '', '', '']])
      .setBackground('#c0392b') // Dark Red
      .setFontColor('#ffffff')
      .setFontWeight('bold');
    currentRow++;
    
    dashboardSheet.getRange(currentRow, 1, 1, 6)
      .setValues([['Forecast ID', 'Requestor', 'Item Name', 'PO #', 'Est. Date', 'Days Overdue']])
      .setFontWeight('bold');
    currentRow++;
    
    analysis.overdueItems.forEach(item => {
      dashboardSheet.getRange(currentRow, 1, 1, 6)
        .setValues([[item.id, item.requestor, item.item, item.po, item.date, item.daysOverdue]]);
      currentRow++;
    });
  }
  
  // Auto-resize columns
  dashboardSheet.autoResizeColumns(1, 6);
}

// ============================================================================
// VARIANCE ANALYSIS
// ============================================================================

function generateVarianceAnalysis() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const data = getForecastData_v2(forecastSheet);
  
  // Calculate variances
  const variances = data.map(item => {
    // AOP totals
    const aop05Total = (parseFloat(item.aop05_q1) || 0) + (parseFloat(item.aop05_q2) || 0) + 
                       (parseFloat(item.aop05_q3) || 0) + (parseFloat(item.aop05_q4) || 0);
    const aop1Total = (parseFloat(item.aop1_q1) || 0) + (parseFloat(item.aop1_q2) || 0) + 
                      (parseFloat(item.aop1_q3) || 0) + (parseFloat(item.aop1_q4) || 0);
    const aopFTotal = (parseFloat(item.aopf_q1) || 0) + (parseFloat(item.aopf_q2) || 0) + 
                      (parseFloat(item.aopf_q3) || 0) + (parseFloat(item.aopf_q4) || 0);
    
    // Quarterly forecast total
    const qTotal = (parseFloat(item.q1m1) || 0) + (parseFloat(item.q1m3) || 0) +
                   (parseFloat(item.q2m1) || 0) + (parseFloat(item.q2m3) || 0) +
                   (parseFloat(item.q3m1) || 0) + (parseFloat(item.q3m3) || 0) +
                   (parseFloat(item.q4m1) || 0) + (parseFloat(item.q4m3) || 0);
    
    // Calculate variances
    const aop05ToAop1 = aop1Total - aop05Total;
    const aop1ToAopF = aopFTotal - aop1Total;
    const aopFToActual = qTotal - aopFTotal;
    
    return {
      ...item,
      aop05Total,
      aop1Total,
      aopFTotal,
      qTotal,
      aop05ToAop1,
      aop05ToAop1Pct: aop05Total !== 0 ? (aop05ToAop1 / aop05Total * 100).toFixed(2) : 0,
      aop1ToAopF,
      aop1ToAopFPct: aop1Total !== 0 ? (aop1ToAopF / aop1Total * 100).toFixed(2) : 0,
      aopFToActual,
      aopFToActualPct: aopFTotal !== 0 ? (aopFToActual / aopFTotal * 100).toFixed(2) : 0
    };
  });
  
  // Create variance sheet
  createVarianceSheet(variances);
  
  SpreadsheetApp.getUi().alert('✅ Variance Analysis generated!');
}

function createVarianceSheet(variances) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  let varianceSheet = ss.getSheetByName(CONFIG_V2.VARIANCE_SHEET);
  
  if (!varianceSheet) {
    varianceSheet = ss.insertSheet(CONFIG_V2.VARIANCE_SHEET);
  }
  
  varianceSheet.clear();
  
  // Headers
  const headers = [
    'Forecast ID', 'Requestor', 'Category', 'Item Name',
    'AOP 0.5 Total', 'AOP 1 Total', 'AOP Final Total', 'Quarterly Total',
    'AOP 0.5 → 1 Variance', 'AOP 0.5 → 1 %',
    'AOP 1 → Final Variance', 'AOP 1 → Final %',
    'AOP Final → Actual Variance', 'AOP Final → Actual %',
    'Status'
  ];
  
  varianceSheet.getRange(1, 1, 1, headers.length)
    .setValues([headers])
    .setBackground('#4285f4')
    .setFontColor('#ffffff')
    .setFontWeight('bold');
  
  // Data
  const data = variances.map(item => [
    item.forecastId,
    item.requestor,
    item.category,
    item.itemName,
    item.aop05Total,
    item.aop1Total,
    item.aopFTotal,
    item.qTotal,
    item.aop05ToAop1,
    item.aop05ToAop1Pct + '%',
    item.aop1ToAopF,
    item.aop1ToAopFPct + '%',
    item.aopFToActual,
    item.aopFToActualPct + '%',
    item.status
  ]);
  
  if (data.length > 0) {
    varianceSheet.getRange(2, 1, data.length, headers.length).setValues(data);
  }
  
  varianceSheet.autoResizeColumns(1, headers.length);
}

// ============================================================================
// REMINDER SYSTEM
// ============================================================================

function sendPeriodReminders() {
  // TEMPORARILY DISABLED
  Logger.log('Period reminders are temporarily disabled by code.');
  return;

  const config = getConfigValues_v2();
  
  if (!config.reminderEnabled) {
    Logger.log('Reminders disabled');
    return;
  }
  
  const activePeriod = config.activePeriod;
  const deadline = new Date(config.activePeriodDeadline);
  const today = new Date();
  const daysUntilDeadline = Math.ceil((deadline - today) / (1000 * 60 * 60 * 24));
  
  // Check if today is a reminder day
  if (!CONFIG_V2.REMINDER_DAYS.includes(daysUntilDeadline)) {
    Logger.log(`No reminder scheduled for ${daysUntilDeadline} days before deadline`);
    return;
  }
  
  // Get incomplete items per user
  const incompleteByUser = getIncompleteItemsByUser(activePeriod);
  
  let sentCount = 0;
  Object.keys(incompleteByUser).forEach(userEmail => {
    if (incompleteByUser[userEmail].length > 0) {
      try {
        sendPeriodReminderEmail(userEmail, activePeriod, deadline, daysUntilDeadline, incompleteByUser[userEmail]);
        sentCount++;
      } catch (error) {
        Logger.log(`Failed to send email to ${userEmail}: ${error}`);
      }
    }
  });
  
  // Send admin summary
  sendAdminSummary_v2(incompleteByUser, sentCount, activePeriod, deadline);
  
  Logger.log(`Sent ${sentCount} reminder emails for ${activePeriod}`);
}

/**
 * NEW: Check for Overdue POs and notify users
 * Runs daily via trigger
 */
function checkOverduePOAndNotify() {
  // TEMPORARILY DISABLED
  Logger.log('Overdue PO notifications are temporarily disabled by code.');
  return;

  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  const data = getForecastData_v2(forecastSheet);
  
  // Reuse the analysis logic
  const overdueItems = analyzeOverdueItems(data);
  
  if (overdueItems.length === 0) {
    Logger.log('No overdue POs found.');
    return;
  }
  
  // Group by Requestor
  const itemsByUser = {};
  
  // Cache emails to avoid repeated lookups
  const emailMap = getRequestorEmailMap();
  
  overdueItems.forEach(item => {
    const user = item.requestor;
    const email = emailMap[user]; // Look up email
    
    if (email) {
        if (!itemsByUser[email]) itemsByUser[email] = [];
        itemsByUser[email].push(item);
    } else {
        Logger.log(`⚠️ No email found for requestor: ${user}`);
        // Optionally add to a "No Email" list for admin summary
    }
  });
  
  // Send Emails
  let sentCount = 0;
  
  for (const [userEmail, items] of Object.entries(itemsByUser)) {
    try {
      sendOverduePOEmail(userEmail, items);
      sentCount++;
    } catch (e) {
      Logger.log(`Failed to send overdue email to ${userEmail}: ${e}`);
    }
  }
  
  // Send Admin Summary for Overdue Items
  sendAdminOverdueSummary(overdueItems, sentCount);
}

function sendOverduePOEmail(userEmail, items) {
  const subject = `⚠️ Action Required: Overdue PO Deliveries - ${items.length} Item(s)`;
  
  let itemList = items.map(item => 
    `• ID: ${item.id} | PO: ${item.po} | Item: ${item.item} | Due: ${item.date} (${item.daysOverdue} days overdue)`
  ).join('\n');
  
  const body = `
Hi,

Our records indicate the following items have passed their Estimated Receiving Date but are not yet marked as "Completed" (Fully Received).

Please follow up with the supplier or update the Estimated Receiving Date in the PR Sheet if there is a delay.

📋 Overdue Items:
${itemList}

Action Needed:
1. Check delivery status with supplier.
2. If delivered: Ensure receipt is processed.
3. If delayed: Update "Est. Receiving Date" in the PR Sheet.

Best regards,
TDG Forecast Automation System
  `.trim();
  
  MailApp.sendEmail({
    to: userEmail,
    subject: subject,
    body: body
  });
}

function sendAdminOverdueSummary(allOverdueItems, sentCount) {
  const config = getConfigValues_v2();
  if (!config.adminEmail) return;
  
  const subject = `[Admin] Overdue PO Summary - ${allOverdueItems.length} Items Pending`;
  
  // Top 10 overdue
  const topOverdue = allOverdueItems.slice(0, 10).map(item => 
    `• ${item.requestor}: ${item.item} (PO ${item.po}) - ${item.daysOverdue} days`
  ).join('\n');
  
  const body = `
Admin Summary - Overdue POs

Total Overdue Items: ${allOverdueItems.length}
Emails Sent: ${sentCount}

Top Overdue Items:
${topOverdue}
${allOverdueItems.length > 10 ? '...and more.' : ''}

Check "Open Items Dashboard" for full details.
  `.trim();
  
  MailApp.sendEmail({
    to: config.adminEmail,
    subject: subject,
    body: body
  });
}

/**
 * NEW: Send Forecast IDs to users for the Active Period
 * Helps them know which IDs to use when raising PRs
 */
function sendForecastIDsToUsers() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  const data = getForecastData_v2(forecastSheet);
  const config = getConfigValues_v2();
  const activePeriod = config.activePeriod;
  
  // Define column for Active Period Forecast (M1)
  let m1Col = '';
  if (activePeriod.includes('Q1')) m1Col = 'q1m1';
  else if (activePeriod.includes('Q2')) m1Col = 'q2m1';
  else if (activePeriod.includes('Q3')) m1Col = 'q3m1';
  else if (activePeriod.includes('Q4')) m1Col = 'q4m1';
  
  if (!m1Col) {
    SpreadsheetApp.getUi().alert('❌ Could not determine active period column.');
    return;
  }
  
  // Filter for actionable items
  // Criteria: 
  // 1. Has amount in Active Period M1
  // 2. Status is Active, Approved, or Draft (Not yet PR Raised/Completed)
  const actionableItems = data.filter(item => {
    const amount = parseFloat(item[m1Col]) || 0;
    const status = item.status;
    
    // Statuses that need PRs raised
    const pendingStatuses = ['Active', 'Approved', 'Draft', 'Placeholder']; 
    
    return amount > 0 && pendingStatuses.includes(status);
  });
  
  if (actionableItems.length === 0) {
    SpreadsheetApp.getUi().alert('ℹ️ No actionable forecast items found for ' + activePeriod);
    return;
  }
  
  // Group by Requestor
  const itemsByUser = {};
  const emailMap = getRequestorEmailMap();
  
  actionableItems.forEach(item => {
    const user = item.requestor;
    const email = emailMap[user];
    
    if (email) {
        if (!itemsByUser[email]) itemsByUser[email] = [];
        itemsByUser[email].push(item);
    }
  });
  
  // Send Emails
  let sentCount = 0;
  
  for (const [userEmail, items] of Object.entries(itemsByUser)) {
    try {
      sendForecastIDListEmail(userEmail, items, activePeriod);
      sentCount++;
    } catch (e) {
      Logger.log(`Failed to send ID list to ${userEmail}: ${e}`);
    }
  }
  
  SpreadsheetApp.getUi().alert(`✅ Sent Forecast IDs to ${sentCount} users for ${activePeriod}.`);
}

function sendForecastIDListEmail(userEmail, items, period) {
  const subject = `📋 Your Forecast IDs for ${period}`;
  
  let itemList = items.map(item => 
    `• ID: ${item.forecastId} | Item: ${item.itemName} | Category: ${item.category}`
  ).join('\n');
  
  const body = `
Hi,

Here are your approved Forecast IDs for the current period (${period}).

Please copy the "Forecast ID" and paste it into the PR Sheet (Column Forecast ID) when raising your Purchase Request. This ensures your spending is correctly tracked against your budget.

📋 Your Forecast Items:
${itemList}

Instructions:
1. Find the item you are purchasing.
2. Copy the Forecast ID (e.g., T25Q1-001).
3. Paste it into the PR Request Form.

Best regards,
TDG Forecast Automation System
  `.trim();
  
  MailApp.sendEmail({
    to: userEmail,
    subject: subject,
    body: body
  });
}

function getIncompleteItemsByUser(activePeriod) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  const data = getForecastData_v2(forecastSheet);
  
  const periodInfo = CONFIG_V2.PERIODS[activePeriod];
  if (!periodInfo) return {};
  
  const userMap = {};
  const emailMap = getRequestorEmailMap(); // Get email mapping
  
  data.forEach(item => {
    if (!item.requestor) return;
    
    // Check if this period column is empty
    const periodValue = getPeriodValue(item, activePeriod);
    if (!periodValue || periodValue === '') {
      
      const email = emailMap[item.requestor];
      if (email) {
          if (!userMap[email]) {
            userMap[email] = [];
          }
          userMap[email].push(item);
      } else {
          // Fallback: use name if email not found (though won't send)
          // or just log warning
      }
    }
  });
  
  return userMap;
}

/**
 * Helper: Get Map of Requestor Name -> Email from Note Sheet
 */
function getRequestorEmailMap() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const noteSheet = ss.getSheetByName(CONFIG_V2.NOTE_SHEET);
  
  const map = {};
  
  if (noteSheet) {
    const lastRow = noteSheet.getLastRow();
    if (lastRow > 1) {
       // Assume Col A = Name, Col C = Email
       const data = noteSheet.getRange(2, 1, lastRow - 1, 3).getValues();
       
       data.forEach(row => {
         const name = String(row[0]).trim();
         const email = String(row[2]).trim(); // Column C
         
         if (name && email && email.includes('@')) {
           map[name] = email;
         }
       });
    }
  }
  
  return map;
}

function getPeriodValue(item, period) {
  const periodMap = {
    'Q1M1': item.q1m1,
    'Q1M3': item.q1m3,
    'Q2M1': item.q2m1,
    'Q2M3': item.q2m3,
    'Q3M1': item.q3m1,
    'Q3M3': item.q3m3,
    'Q4M1': item.q4m1,
    'Q4M3': item.q4m3
  };
  return periodMap[period];
}

function sendPeriodReminderEmail(userEmail, period, deadline, daysUntil, items) {
  const spreadsheetUrl = SpreadsheetApp.getActiveSpreadsheet().getUrl();
  
  let urgency = '⚠️';
  let urgencyText = 'Reminder';
  
  if (daysUntil === 0) {
    urgency = '🚨';
    urgencyText = 'URGENT - TODAY';
  } else if (daysUntil === 1) {
    urgency = '⏰';
    urgencyText = 'TOMORROW';
  }
  
  const subject = `${CONFIG_V2.EMAIL_SUBJECT_PREFIX} ${urgency} ${urgencyText} - ${period} Due ${Utilities.formatDate(deadline, Session.getScriptTimeZone(), 'MMM dd, yyyy')}`;
  
  const itemList = items.map((item, index) => 
    `${index + 1}. ${item.forecastId} | ${item.category} | ${item.itemName || 'Unnamed'}`
  ).join('\n');
  
  const body = `
Hi,

${urgency} This is a reminder to update your ${period} forecast.

📅 Deadline: ${Utilities.formatDate(deadline, Session.getScriptTimeZone(), 'MMMM dd, yyyy')}
⏳ Time Remaining: ${daysUntil} day(s)

You have ${items.length} item(s) that need ${period} forecasts:

${itemList}

📊 Please update the forecast sheet:
${spreadsheetUrl}

Instructions:
1. Go to the Forecast sheet
2. Find your items (listed above)
3. Fill in the ${period} column for each item
4. Save (auto-saves)

Questions? Contact the admin.

Best regards,
TDG Forecast Automation System
  `.trim();
  
  MailApp.sendEmail({
    to: userEmail,
    subject: subject,
    body: body
  });
}

function sendAdminSummary_v2(incompleteByUser, sentCount, period, deadline) {
  const config = getConfigValues_v2();
  if (!config.adminEmail) return;
  
  const totalUsers = Object.keys(incompleteByUser).length;
  const totalIncomplete = Object.values(incompleteByUser).reduce((sum, items) => sum + items.length, 0);
  
  const userList = Object.entries(incompleteByUser)
    .map(([user, items]) => `  - ${user}: ${items.length} item(s)`)
    .join('\n');
  
  const subject = `${CONFIG_V2.EMAIL_SUBJECT_PREFIX} Admin Summary - ${period} Reminders Sent`;
  
  const body = `
Admin Summary - Period Reminder System

Period: ${period}
Deadline: ${Utilities.formatDate(deadline, Session.getScriptTimeZone(), 'MMMM dd, yyyy')}

📊 Status:
  - Users with incomplete items: ${totalUsers}
  - Total incomplete items: ${totalIncomplete}
  - Reminder emails sent: ${sentCount}

👥 Users with incomplete items:
${userList}

View detailed status in the Open Items Dashboard.

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
// USER COMPLETION STATUS
// ============================================================================

function checkUserCompletionStatus() {
  const config = getConfigValues_v2();
  const activePeriod = config.activePeriod;
  
  // Use email map to display names nicely if possible, or just count emails
  const incompleteByUser = getIncompleteItemsByUser(activePeriod);
  const totalIncomplete = Object.values(incompleteByUser).reduce((sum, items) => sum + items.length, 0);
  
  const message = `📊 Completion Status for ${activePeriod}\n\n` +
    `Users with incomplete items: ${Object.keys(incompleteByUser).length}\n` +
    `Total incomplete items: ${totalIncomplete}\n\n` +
    `Check the "Open Items Dashboard" for detailed breakdown.`;
  
  SpreadsheetApp.getUi().alert(message);
}

// ============================================================================
// PLACEHOLDER ITEM MANAGEMENT
// ============================================================================

/**
 * When PR is raised for a placeholder item, update the item details
 * This is triggered when user enters PO number for an item with "Placeholder" status
 */
function updatePlaceholderItem(row, sheet) {
  const status = sheet.getRange(row, CONFIG_V2.COLS.STATUS).getValue();
  const poNumber = sheet.getRange(row, CONFIG_V2.COLS.PO_NUMBER).getValue();
  const itemName = sheet.getRange(row, CONFIG_V2.COLS.ITEM_NAME).getValue();
  
  // Check if this is a placeholder item with a PO
  if (status === 'Placeholder' && poNumber && itemName.toLowerCase().includes('placeholder')) {
    // Prompt user to update item details
    const ui = SpreadsheetApp.getUi();
    const response = ui.alert(
      '⚠️ Placeholder Item Detected',
      `This item "${itemName}" is marked as Placeholder.\n\n` +
      `PO Number: ${poNumber}\n\n` +
      `Please update:\n` +
      `1. Item Name (Column E) with actual item\n` +
      `2. Description (Column AE) with details\n` +
      `3. Status to "PR Raised" (or "Active")\n\n` +
      `Click OK when done, or Cancel to skip.`,
      ui.ButtonSet.OK_CANCEL
    );
    
    if (response === ui.Button.OK) {
      // Auto-update status to "PR Raised"
      sheet.getRange(row, CONFIG_V2.COLS.STATUS).setValue('PR Raised');
      
      // Log the change
      const user = Session.getActiveUser().getEmail();
      const timestamp = new Date();
      const logCell = sheet.getRange(row, CONFIG_V2.COLS.CHANGE_LOG);
      const existingLog = logCell.getValue() || '';
      const newLog = `${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')}: Placeholder updated to actual item (PO: ${poNumber}) by ${user}`;
      logCell.setValue(existingLog ? `${existingLog}\n${newLog}` : newLog);
    }
  }
}

/**
 * Find all placeholder items and list them
 */
function listPlaceholderItems() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    SpreadsheetApp.getUi().alert('❌ Forecast sheet not found!');
    return;
  }
  
  const lastRow = forecastSheet.getLastRow();
  if (lastRow < 4) {
    SpreadsheetApp.getUi().alert('ℹ️ No data found.');
    return;
  }
  
  const data = forecastSheet.getRange(4, 1, lastRow - 3, CONFIG_V2.COLS.CHANGE_LOG).getValues();
  
  let placeholders = [];
  data.forEach((row, index) => {
    const actualRow = index + 4;
    const forecastId = row[CONFIG_V2.COLS.FORECAST_ID - 1];
    const itemName = row[CONFIG_V2.COLS.ITEM_NAME - 1];
    const status = row[CONFIG_V2.COLS.STATUS - 1];
    const poAmount = row[CONFIG_V2.COLS.PO_AMOUNT - 1];
    const category = row[CONFIG_V2.COLS.CATEGORY - 1];
    const totalFY = row[CONFIG_V2.COLS.TOTAL_FY - 1];
    
    // Check if item is placeholder
    const isPlaceholder = status === 'Placeholder' || 
                          (itemName && itemName.toLowerCase().includes('placeholder'));
    
    if (isPlaceholder && forecastId) {
      placeholders.push({
        row: actualRow,
        id: forecastId,
        item: itemName,
        category: category,
        status: status,
        poAmount: poAmount || 0,
        amount: totalFY || 0
      });
    }
  });
  
  if (placeholders.length === 0) {
    SpreadsheetApp.getUi().alert('✅ No placeholder items found!');
    return;
  }
  
  // Format message
  let message = `⚠️ Found ${placeholders.length} Placeholder Item(s):\n\n`;
  placeholders.forEach(p => {
    message += `• ${p.id} | ${p.item}\n`;
    message += `  Category: ${p.category || 'N/A'}\n`;
    message += `  Amount: $${p.amount.toLocaleString()}\n`;
    message += `  PO Amount: $${p.poAmount.toLocaleString()}\n`;
    message += `  Row: ${p.row}\n\n`;
  });
  
  message += `\n💡 When PO is raised:\n`;
  message += `1. Enter PO # in column AD\n`;
  message += `2. Update Item Name (column E)\n`;
  message += `3. Update Description (column AE)\n`;
  message += `4. Change Status to "PR Raised"`;
  
  SpreadsheetApp.getUi().alert(message);
}

// ============================================================================
// CHANGE TRACKING & AUTO-GENERATION
// ============================================================================

function handleEdit(e) {
  if (!e) return;
  
  const sheet = e.range.getSheet();
  const sheetName = sheet.getName();
  
  // Only track changes in Forecast sheet
  if (sheetName !== CONFIG_V2.FORECAST_SHEET) return;
  
  const row = e.range.getRow();
  const col = e.range.getColumn();
  
  // Skip header rows (1-3: deadline, total, headers)
  if (row < 4) return;
  
  const user = Session.getActiveUser().getEmail();
  const timestamp = new Date();
  
  // Update Last Updated and Updated By
  sheet.getRange(row, CONFIG_V2.COLS.LAST_UPDATED).setValue(timestamp);
  sheet.getRange(row, CONFIG_V2.COLS.UPDATED_BY).setValue(user);
  
  // Add to change log
  const columnName = sheet.getRange(3, col).getValue(); // Get column header from row 3
  const oldValue = e.oldValue || '(empty)';
  const newValue = e.value || '(empty)';
  
  if (oldValue !== newValue) {
    const logCell = sheet.getRange(row, CONFIG_V2.COLS.CHANGE_LOG);
    const existingLog = logCell.getValue() || '';
    const newLogEntry = `${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')}: ${columnName} changed from "${oldValue}" to "${newValue}" by ${user}`;
    const updatedLog = existingLog ? `${existingLog} | ${newLogEntry}` : newLogEntry;
    logCell.setValue(updatedLog);
    // FORCE NO WRAP on every edit to maintain row height
    logCell.setWrap(false).setWrapStrategy(SpreadsheetApp.WrapStrategy.CLIP);
  }

  // ALWAYS force row height to 21 after any edit in the data range
  // This ensures that pasting data or typing long text doesn't expand the row
  sheet.setRowHeight(row, 21);
  SpreadsheetApp.flush();
  
  // Auto-fill Carry To if status changed to "Carry To Next FY" or "Carry To Qx"
  if (col === CONFIG_V2.COLS.STATUS) {
    if (e.value === 'Carry To Next FY') {
      const config = getConfigValues_v2();
      if (config.autoFillCarryTo) {
        const totalFY = sheet.getRange(row, CONFIG_V2.COLS.TOTAL_FY).getValue();
        const carryToCell = sheet.getRange(row, CONFIG_V2.COLS.CARRY_TO);
        
        if (!carryToCell.getValue() && totalFY) {
          carryToCell.setValue(totalFY);
        }
      }
    } else if (e.value && e.value.startsWith('Carry To Q')) {
      handleCarryToQuarter(sheet, row, e.value);
    }
  }
  
  // Check if PO number entered for placeholder item
  if (col === CONFIG_V2.COLS.PO_NUMBER && e.value) {
    updatePlaceholderItem(row, sheet);
  }
  
  // ============================================================================
  // AUTO-GENERATE FORECAST ID
  // ============================================================================
  // Trigger when key fields are filled: Requestor, Category, Project, Item, or any cost column
  
  const keyColumns = [
    CONFIG_V2.COLS.REQUESTOR,
    CONFIG_V2.COLS.CATEGORY,
    CONFIG_V2.COLS.PROJECT_NAME,
    CONFIG_V2.COLS.ITEM_NAME,
    // AOP columns
    CONFIG_V2.COLS.AOP05_Q1, CONFIG_V2.COLS.AOP05_Q2, CONFIG_V2.COLS.AOP05_Q3, CONFIG_V2.COLS.AOP05_Q4,
    CONFIG_V2.COLS.AOP1_Q1, CONFIG_V2.COLS.AOP1_Q2, CONFIG_V2.COLS.AOP1_Q3, CONFIG_V2.COLS.AOP1_Q4,
    CONFIG_V2.COLS.AOPF_Q1, CONFIG_V2.COLS.AOPF_Q2, CONFIG_V2.COLS.AOPF_Q3, CONFIG_V2.COLS.AOPF_Q4,
    // Quarterly forecast columns
    CONFIG_V2.COLS.Q1M1, CONFIG_V2.COLS.Q1M3, CONFIG_V2.COLS.Q2M1, CONFIG_V2.COLS.Q2M3,
    CONFIG_V2.COLS.Q3M1, CONFIG_V2.COLS.Q3M3, CONFIG_V2.COLS.Q4M1, CONFIG_V2.COLS.Q4M3
  ];
  
  // Check if edited column is one of the key columns
  if (keyColumns.includes(col)) {
    const idCell = sheet.getRange(row, CONFIG_V2.COLS.FORECAST_ID);
    const currentId = idCell.getValue();
    
    // Only generate ID if it doesn't exist yet
    if (!currentId || currentId === '') {
      // Check if row has minimum required data
      const requestor = sheet.getRange(row, CONFIG_V2.COLS.REQUESTOR).getValue();
      const category = sheet.getRange(row, CONFIG_V2.COLS.CATEGORY).getValue();
      const itemName = sheet.getRange(row, CONFIG_V2.COLS.ITEM_NAME).getValue();
      
      // Require at least: (Requestor OR Category) AND (Item Name OR any cost value)
      const hasIdentifier = requestor || category;
      const hasContent = itemName || e.value; // e.value is the value just entered
      
      if (hasIdentifier && hasContent) {
        try {
          const config = getConfigValues_v2();
          const newId = generateUniqueId_v2(config.fiscalYear);
          idCell.setValue(newId);
          
          // Log the auto-generation
          const logCell = sheet.getRange(row, CONFIG_V2.COLS.CHANGE_LOG);
          const existingLog = logCell.getValue() || '';
          const autoGenLog = `${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')}: Forecast ID "${newId}" auto-generated by ${user}`;
          const updatedLog = existingLog ? `${existingLog} | ${autoGenLog}` : autoGenLog;
          logCell.setValue(updatedLog);
          logCell.setWrap(false).setWrapStrategy(SpreadsheetApp.WrapStrategy.CLIP);
          
          // Fix row height to prevent excessive height
          sheet.setRowHeight(row, 21); // Standard row height
          
          // Add Total FY formula for this new row
          // Sum M3 if available, else M1 (Forecast)
          const totalFYCell = sheet.getRange(row, CONFIG_V2.COLS.TOTAL_FY);
          if (!totalFYCell.getFormula()) {
            totalFYCell.setFormula(`=IF(S${row}<>"",S${row},R${row}) + IF(U${row}<>"",U${row},T${row}) + IF(W${row}<>"",W${row},V${row}) + IF(Y${row}<>"",Y${row},X${row})`);
          }
          
          Logger.log(`Auto-generated Forecast ID: ${newId} for row ${row}`);
        } catch (error) {
          Logger.log(`Failed to auto-generate Forecast ID: ${error}`);
        }
      }
    }
  }
}

// ============================================================================
// TRIGGER MANAGEMENT
// ============================================================================

function setupTriggers_v2() {
  // Delete existing triggers
  const triggers = ScriptApp.getProjectTriggers();
  triggers.forEach(trigger => ScriptApp.deleteTrigger(trigger));
  
  // Create onEdit trigger
  ScriptApp.newTrigger('handleEdit')
    .forSpreadsheet(SpreadsheetApp.getActiveSpreadsheet())
    .onEdit()
    .create();
  
  // Create daily trigger for reminders (7am)
  ScriptApp.newTrigger('runDailyChecks') // Changed to wrapper function
    .timeBased()
    .atHour(7)
    .everyDays(1)
    .create();
  
  // Create daily trigger for dashboard update (8am)
  ScriptApp.newTrigger('updateOpenItemsDashboard')
    .timeBased()
    .atHour(8)
    .everyDays(1)
    .create();
    
  // Create hourly trigger for auto-sync
  ScriptApp.newTrigger('autoSyncUnbudgetedItems')
    .timeBased()
    .everyHours(1)
    .create();
    
  ScriptApp.newTrigger('autoSyncPONumbers')
    .timeBased()
    .everyHours(1)
    .create();
  
  SpreadsheetApp.getUi().alert('✅ Triggers setup successfully!\n\n' +
    '- onChange: Track edits\n' +
    '- Daily 7am: Send reminders\n' +
    '- Daily 8am: Update dashboard\n' +
    '- Hourly: Auto-sync PRs');
}

function autoSyncUnbudgetedItems() {
  syncUnbudgetedItems({ silent: true });
}

function autoSyncPONumbers() {
  syncPONumbersFromPR({ silent: true });
}

/**
 * Wrapper for daily checks (Reminders + Overdue POs)
 */
function runDailyChecks() {
  // 1. Send Period Reminders (Budgeting)
  sendPeriodReminders();
  
  // 2. Send Overdue PO Reminders (Execution)
  checkOverduePOAndNotify();
}

// ============================================================================
// UTILITY FUNCTIONS
// ============================================================================

function getConfigValues_v2() {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const configSheet = ss.getSheetByName(CONFIG_V2.CONFIG_SHEET);
  
  if (!configSheet) {
    return {
      fiscalYear: 'FY25',
      activePeriod: 'Q1M1',
      activePeriodDeadline: '2025-10-29',
      reminderEnabled: true,
      adminEmail: '',
      autoFillCarryTo: true
    };
  }
  
  return {
    fiscalYear: configSheet.getRange(2, 2).getValue() || 'FY25',
    activePeriod: configSheet.getRange(3, 2).getValue() || 'Q1M1',
    activePeriodDeadline: configSheet.getRange(4, 2).getValue() || '2025-10-29',
    reminderEnabled: String(configSheet.getRange(5, 2).getValue()).toUpperCase() === 'TRUE',
    adminEmail: configSheet.getRange(6, 2).getValue() || '',
    autoFillCarryTo: String(configSheet.getRange(7, 2).getValue()).toUpperCase() === 'TRUE'
  };
}

// ============================================================================
// SYNC & DEDUCTION LOGIC
// ============================================================================

/**
 * Syncs unbudgeted items from PR sheet to Forecast sheet
 * 1. Reads PR sheet
 * 2. Finds items with PO Number but NO Forecast ID
 * 3. Deducts amount from Placeholder bucket
 * 4. Creates new Forecast Item
 * 5. Updates PR sheet with new Forecast ID
 */
function syncUnbudgetedItems(options) {
  const silent = (options && options.silent) || false;
  const ui = silent ? null : SpreadsheetApp.getUi();
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    if (ui) ui.alert('❌ Forecast sheet not found!');
    Logger.log('❌ Forecast sheet not found!');
    return;
  }

  // 1. Read PR Sheet
  let prSpreadsheet;
  try {
    prSpreadsheet = SpreadsheetApp.openById(CONFIG_V2.PR_SPREADSHEET_ID);
  } catch (e) {
    if (ui) ui.alert('❌ Could not open PR Sheet. Check ID and permissions.');
    Logger.log('❌ Could not open PR Sheet. Check ID and permissions.');
    return;
  }

  const prSheet = prSpreadsheet.getSheetByName(CONFIG_V2.PR_SHEET_NAME);
  if (!prSheet) {
    if (ui) ui.alert('❌ Sheet "${CONFIG_V2.PR_SHEET_NAME}" not found in PR Spreadsheet.');
    Logger.log('❌ Sheet "${CONFIG_V2.PR_SHEET_NAME}" not found in PR Spreadsheet.');
    return;
  }

  const lastPrRow = prSheet.getLastRow();
  if (lastPrRow < 6) { // Assuming data starts around row 6 based on image
    if (ui) ui.alert('ℹ️ No data found in PR sheet.');
    Logger.log('ℹ️ No data found in PR sheet.');
    return;
  }

  // Read PR Data
  // We need to read up to column 50 (AX) based on config
  const prData = prSheet.getRange(1, 1, lastPrRow, 50).getValues();
  const config = getConfigValues_v2();
  
  let syncedCount = 0;
  let totalDeducted = 0;
  let errors = [];
  let details = []; // To store detail strings
  let rowsToHighlight = []; // To store row indices
  
  // Iterate through PR rows (skip headers, assuming data starts row 6)
  // First, group items by PO Number to aggregate them
  const poGroups = {};
  
  for (let i = 5; i < prData.length; i++) {
    const row = prData[i];
    const rowIndex = i + 1; // 1-based index
    
    const poNumber = row[CONFIG_V2.PR_COLS.PO_NUMBER - 1];
    const forecastId = row[CONFIG_V2.PR_COLS.FORECAST_ID - 1];
    const expensesGroup = String(row[CONFIG_V2.PR_COLS.EXPENSES_GROUP - 1]).trim();
    const estDate = row[CONFIG_V2.PR_COLS.ESTIMATED_RECEIVING_DATE - 1]; // AA
    
    // Check Sync Criteria: 
    // 1. Has PO Number 
    // 2. No Forecast ID
    // 3. Expenses Group is "TDG" (Case-insensitive check)
    if (poNumber && (!forecastId || forecastId === '') && expensesGroup.toUpperCase() === 'TDG') {
      
      const amount = parseFloat(row[CONFIG_V2.PR_COLS.AMOUNT - 1]) || 0;
      let category = row[CONFIG_V2.PR_COLS.EXPENSES_CATEGORY - 1]; // Column R
      if (category) category = String(category).trim();
      
      const requestor = row[CONFIG_V2.PR_COLS.REQUESTOR - 1];
      const itemName = row[CONFIG_V2.PR_COLS.ITEM_NAME - 1];
      const projectName = row[CONFIG_V2.PR_COLS.JUSTIFICATION - 1]; // Column K
      
      if (amount > 0) {
        // Map PR Category to Forecast Category
        const forecastCategory = CONFIG_V2.CATEGORY_MAPPING[category] || category;
        
        if (!poGroups[poNumber]) {
          poGroups[poNumber] = {
            poNumber: poNumber,
            amount: 0,
            items: [],
            category: forecastCategory, // Use mapped category from first item
            requestor: requestor,
            projectName: projectName,
            estDate: estDate, // Store estimated date
            rowsToUpdate: []
          };
        }
        
        // Update estDate if not set yet and available
        if (!poGroups[poNumber].estDate && estDate) {
          poGroups[poNumber].estDate = estDate;
        }
        
        // Aggregate data
        poGroups[poNumber].amount += amount;
        poGroups[poNumber].items.push(itemName);
        poGroups[poNumber].rowsToUpdate.push(rowIndex);
      }
    }
  }
  
  // Process each PO Group
  for (const poNumber in poGroups) {
    const group = poGroups[poNumber];
    
    // Combine item names
    const combinedItemName = group.items.join('; ');
    
    // Debug Log
    Logger.log('Processing Grouped PO ' + poNumber + ': Category "' + group.category + '", Total Amount $' + group.amount);
    
    const deductionResult = deductFromPlaceholder(forecastSheet, group.category, group.amount, config.activePeriod, poNumber, null, silent);
    
    if (deductionResult.success) {
      totalDeducted += group.amount;
      
      // Generate Sub-ID based on Placeholder ID
      const parentId = deductionResult.placeholderId;
      const newId = generateSubId(forecastSheet, parentId);

      // Create New Forecast Row (One row per PO)
      const creationResult = createForecastRow(forecastSheet, {
        id: newId,
        requestor: group.requestor,
        category: group.category,
        projectName: group.projectName,
        itemName: combinedItemName,
        amount: group.amount,
        poNumber: group.poNumber,
        period: config.activePeriod,
        fiscalYear: config.fiscalYear,
        estDate: group.estDate
      });
      
      // Store row and category for highlighting
      rowsToHighlight.push({
        row: creationResult.row,
        category: group.category
      });
      
      details.push(`[-] Deducted $${group.amount.toLocaleString()} from [${group.category}] for PO ${group.poNumber} (New ID: ${newId})`);

      // Update ALL PR Sheets rows for this PO with the same new ID
      group.rowsToUpdate.forEach(rowIndex => {
        prSheet.getRange(rowIndex, CONFIG_V2.PR_COLS.FORECAST_ID).setValue(newId);
      });
      
      syncedCount++;
    } else {
      const msg = `PO ${poNumber}: ${deductionResult.message}`;
      Logger.log(`⚠️ ${msg}`);
      errors.push(msg);
    }
  }

  if (syncedCount > 0) {
    let msg = `✅ Sync Complete!\n\n`;
    msg += `Processed: ${syncedCount} items\n`;
    msg += `Total Deducted: $${totalDeducted.toLocaleString()}\n\n`;
    msg += `DETAILS:\n${details.join('\n')}\n\n`;
    
    if (errors.length > 0) {
      msg += `⚠️ Errors (${errors.length}):\n${errors.slice(0, 5).join('\n')}${errors.length > 5 ? '\n...' : ''}`;
    }
    if (ui) ui.alert(msg);
    Logger.log(msg);
  } else if (errors.length > 0) {
    const msg = `❌ Sync Failed for ${errors.length} items.\n\nErrors:\n${errors.slice(0, 10).join('\n')}`;
    if (ui) ui.alert(msg);
    Logger.log(msg);
  } else {
    if (ui) ui.alert('ℹ️ No unbudgeted items found to sync.\n(Criteria: Has PO # AND Empty Forecast ID)');
    Logger.log('ℹ️ No unbudgeted items found to sync.');
  }
  
  // Update Conditional Formatting to include new rows
  setupStatusConditionalFormatting(forecastSheet, forecastSheet.getLastRow());
}

/**
 * Syncs PO Numbers from PR Sheet to Forecast Sheet based on matching Forecast IDs.
 * Useful for items that were already budgeted but now have a PO generated.
 * Also calculates variance (Forecast - PO) and returns savings to Placeholder.
 */
function syncPONumbersFromPR() {
  const ui = SpreadsheetApp.getUi();
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const forecastSheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!forecastSheet) {
    ui.alert('❌ Forecast sheet not found!');
    return;
  }

  // 1. Open PR Sheet
  let prSpreadsheet;
  try {
    prSpreadsheet = SpreadsheetApp.openById(CONFIG_V2.PR_SPREADSHEET_ID);
  } catch (e) {
    ui.alert('❌ Could not open PR Sheet. Check ID and permissions.');
    return;
  }

  const prSheet = prSpreadsheet.getSheetByName(CONFIG_V2.PR_SHEET_NAME);
  if (!prSheet) {
    ui.alert(`❌ Sheet "${CONFIG_V2.PR_SHEET_NAME}" not found in PR Spreadsheet.`);
    return;
  }

  // 2. Read Forecast Data
  const lastForecastRow = forecastSheet.getLastRow();
  if (lastForecastRow < 4) {
    ui.alert('ℹ️ No data in Forecast sheet.');
    return;
  }
  
  // Read all forecast data for processing
  const forecastRange = forecastSheet.getRange(4, 1, lastForecastRow - 3, CONFIG_V2.COLS.CHANGE_LOG);
  const forecastValues = forecastRange.getValues();
  
  // Create Map: ForecastID -> Row Index (0-based relative to data range)
  const idMap = new Map();
  forecastValues.forEach((row, index) => {
    const id = row[CONFIG_V2.COLS.FORECAST_ID - 1];
    if (id && String(id).trim() !== "") {
      idMap.set(String(id).trim(), index);
    }
  });

  // 3. Read PR Data
  const lastPrRow = prSheet.getLastRow();
  // Read up to Column 60 (covering BF=58)
  const prData = prSheet.getRange(1, 1, lastPrRow, 60).getValues(); 
  
  // Group PR Data by Forecast ID to handle split lines/duplicates
  const prGroups = {};
  for (let i = 5; i < prData.length; i++) {
    const row = prData[i];
    const prId = String(row[CONFIG_V2.PR_COLS.FORECAST_ID - 1]).trim();
    const prPO = String(row[CONFIG_V2.PR_COLS.PO_NUMBER - 1]).trim();
    const prAmount = parseFloat(row[CONFIG_V2.PR_COLS.AMOUNT - 1]) || 0;
    const estDate = row[CONFIG_V2.PR_COLS.ESTIMATED_RECEIVING_DATE - 1];
    
    // Receipt Data
    const rcptNum = String(row[CONFIG_V2.PR_COLS.RECEIPT_NUMBER - 1]).trim();
    const rcptDate = row[CONFIG_V2.PR_COLS.RECEIPT_DATE - 1];
    const rcptAmount = parseFloat(row[CONFIG_V2.PR_COLS.RECEIVED_AMOUNT - 1]) || 0;
    
    if (prId && prPO) {
      if (!prGroups[prId]) {
        prGroups[prId] = { 
          po: prPO, 
          amount: 0,
          hasReceipt: false,
          receivedAmount: 0,
          estDate: estDate
        };
      }
      // Aggregate amounts for the same ID
      // Use the last found PO number if they differ (rare case)
      prGroups[prId].po = prPO; 
      prGroups[prId].amount += prAmount;
      prGroups[prId].receivedAmount += rcptAmount;
      
      if (!prGroups[prId].estDate && estDate) {
        prGroups[prId].estDate = estDate;
      }
      
      // Check if this line has receipt info (Number, Date, or Amount)
      if (rcptNum || (rcptDate && String(rcptDate).trim() !== '') || rcptAmount > 0) {
        prGroups[prId].hasReceipt = true;
      }
    }
  }

  let updatedCount = 0;
  let varianceTotal = 0;
  let logs = [];
  let rowsToHighlight = []; // To store row indices
  
  // 4. Process Grouped Data
  for (const [prId, group] of Object.entries(prGroups)) {
    const prPO = group.po;
    const prAmount = group.amount;
    const hasReceipt = group.hasReceipt;
    const receivedAmount = group.receivedAmount;
    
    // If we have an ID match in Forecast sheet
    if (idMap.has(prId)) {
      const index = idMap.get(prId);
      const forecastRow = forecastValues[index];
      const rowIndex = index + 4;
      
      let itemUpdated = false;
      
      // Get Current Values
      const currentPO = String(forecastRow[CONFIG_V2.COLS.PO_NUMBER - 1]).trim();
      const currentPOAmount = parseFloat(forecastRow[CONFIG_V2.COLS.PO_AMOUNT - 1]) || 0;
      const currentStatus = String(forecastRow[CONFIG_V2.COLS.STATUS - 1]).trim();
      const category = forecastRow[CONFIG_V2.COLS.CATEGORY - 1];
      
      // 1. Update PO Number if changed
      if (prPO && currentPO !== prPO) {
        forecastSheet.getRange(rowIndex, CONFIG_V2.COLS.PO_NUMBER).setValue(prPO);
        itemUpdated = true;
      }
      
      // 2. Handle Amount & Variance
      // FIX: Always use the PO Amount (prAmount) for the Forecast Sheet to track the total committed cost.
      // We do NOT want to overwrite the forecast with the partial 'receivedAmount'.
      // Variance calculation (Forecast vs PO) should be based on the Total PO Amount.
      const targetAmount = prAmount;
      
      // MOVED LOGIC: We now determine targetCol BEFORE the check, so we can check for column moves
      // Find the Budgeted/Forecast Amount from M1 columns
      let forecastAmount = 0;
      let m1TargetCol = 0;
      
      const m1Cols = [CONFIG_V2.COLS.Q1M1, CONFIG_V2.COLS.Q2M1, CONFIG_V2.COLS.Q3M1, CONFIG_V2.COLS.Q4M1];
      const mapM1toM3 = {};
      mapM1toM3[CONFIG_V2.COLS.Q1M1] = CONFIG_V2.COLS.Q1M3;
      mapM1toM3[CONFIG_V2.COLS.Q2M1] = CONFIG_V2.COLS.Q2M3;
      mapM1toM3[CONFIG_V2.COLS.Q3M1] = CONFIG_V2.COLS.Q3M3;
      mapM1toM3[CONFIG_V2.COLS.Q4M1] = CONFIG_V2.COLS.Q4M3;
      
      for (const col of m1Cols) {
        const val = parseFloat(forecastRow[col - 1]) || 0;
        if (val > 0) {
          forecastAmount = val;
          m1TargetCol = mapM1toM3[col]; // We will update the M3 column
          break;
        }
      }
      
      // Determine Final Target Column (M3)
      // Priority 1: Estimated Receiving Date
      let targetCol = 0;
      if (group.estDate) {
          const quarter = getQuarterFromDate(group.estDate);
          
          if (quarter === 'Q1') targetCol = CONFIG_V2.COLS.Q1M3;
          else if (quarter === 'Q2') targetCol = CONFIG_V2.COLS.Q2M3;
          else if (quarter === 'Q3') targetCol = CONFIG_V2.COLS.Q3M3;
          else if (quarter === 'Q4') targetCol = CONFIG_V2.COLS.Q4M3;
          
      }
      
      // Priority 2: Follow Forecast (M1) location
      if (targetCol === 0 && m1TargetCol > 0) {
          targetCol = m1TargetCol;
      }

      // --- NEW CHECK: Determine if we need to update based on Column Movement ---
      // Even if the PO Amount hasn't changed (Math.abs(currentPOAmount - targetAmount) < 0.01),
      // we might need to move the value to a different column (e.g. Q1 -> Q2).
      
      let needsUpdate = false;
      
      // Check 1: Amount Change
      if (targetAmount > 0 && Math.abs(currentPOAmount - targetAmount) > 0.01) {
          needsUpdate = true;
      }
      
      // Check 2: Column Location Change
      if (targetCol > 0) {
          const currentValInTargetCol = parseFloat(forecastRow[targetCol - 1]) || 0;
          if (Math.abs(currentValInTargetCol - targetAmount) > 0.01) {
              needsUpdate = true;
              Logger.log(`[${prId}] Value needs moving to correct column (Current: ${currentValInTargetCol}, Target: ${targetAmount})`);
          }
      }
      
      // Check 3: Date Change
      // If the estimated date has changed, we must update (to reflect it in Col AG)
      // and this will also force a re-evaluation of the column placement
      const currentEstDateRaw = forecastRow[CONFIG_V2.COLS.EST_RECEIVING_DATE - 1];
      let dateChanged = false;
      
      if (group.estDate) {
          const newDate = new Date(group.estDate);
          const oldDate = currentEstDateRaw ? new Date(currentEstDateRaw) : null;
          
          if (!oldDate || newDate.getTime() !== oldDate.getTime()) {
               dateChanged = true;
               needsUpdate = true;
               Logger.log(`[${prId}] Date changed. Old: ${oldDate}, New: ${newDate}`);
          }
      }

      if (needsUpdate) {
      
        // Detect if this is a Placeholder Row
        const project = String(forecastRow[CONFIG_V2.COLS.PROJECT_NAME - 1]).trim();
        const isPlaceholder = (currentStatus === 'Placeholder') || (project === 'Placeholder') || (String(forecastRow[CONFIG_V2.COLS.ITEM_NAME - 1]).toLowerCase().includes('placeholder'));
        
        if (isPlaceholder) {
           // CASE 1: Matched Row is a Placeholder
           // Action: Only Deduct. Do NOT add variance.
           // This handles cases where user entered the Placeholder ID directly in the PR sheet
           Logger.log(`PR ${prId} points to Placeholder Row. Treating as deduction.`);
           
           // Deduct from Placeholder (Only if this is a new sync for this amount)
           // For Placeholders, we ALWAYS deduct if the amount has increased, or return if decreased
           // Because "PO Amount" on a placeholder line IS the amount used.
           const diff = targetAmount - currentPOAmount;
           
           if (diff > 0.01) {
             // Amount increased (more usage) -> Deduct difference from balance
             const config = getConfigValues_v2();
             deductFromPlaceholder(forecastSheet, category, diff, config.activePeriod, prPO, prId);
             logs.push(`[-] Deducted $${diff.toLocaleString()} from [${category}] (Direct Usage Increase) - ID: ${prId}`);
           } else if (diff < -0.01) {
             // Amount decreased (less usage) -> Return difference to balance
             // Note: deductFromPlaceholder with negative amount effectively adds
             const config = getConfigValues_v2();
             deductFromPlaceholder(forecastSheet, category, diff, config.activePeriod, prPO, prId);
             logs.push(`[+] Returned $${Math.abs(diff).toLocaleString()} to [${category}] (Direct Usage Decrease) - ID: ${prId}`);
           }
           
        } else {
           // CASE 2: Matched Row is a Normal Item
           // Action: Calculate Variance (Forecast - PO) and Return Savings
           
           // Calculate Variance ONLY for new syncs (where previous PO Amount was 0)
           if (currentPOAmount === 0) {
              if (forecastAmount > targetAmount) {
                 // SAVINGS: Forecast > PO
                 const variance = forecastAmount - targetAmount;
                 Logger.log(`Found Savings for ${prId}: Forecast $${forecastAmount} - PO $${targetAmount} = $${variance}`);
                 
                 // Add Savings to Placeholder
                 if (returnToPlaceholder(forecastSheet, category, variance)) {
                   varianceTotal += variance;
                   logs.push(`[+] Returned $${variance.toLocaleString()} to [${category}] (Savings) - ID: ${prId}`);
                 }
              } else if (forecastAmount < targetAmount) {
                 // OVERSPEND: Forecast < PO
                 
                 // Only deduct overspend if there was an initial forecast (Forecast > 0)
                 // If Forecast is 0, it implies this item was created as "Unbudgeted" (already deducted) 
                 // or is a placeholder row, so we shouldn't deduct again.
                 if (forecastAmount > 0) {
                   const overspend = targetAmount - forecastAmount;
                   Logger.log(`Found Overspend for ${prId}: Forecast $${forecastAmount} - PO $${targetAmount} = -$${overspend}`);
                   
                   // Deduct Overspend from Placeholder
                   const config = getConfigValues_v2();
                   const deductionResult = deductFromPlaceholder(forecastSheet, category, overspend, config.activePeriod, prPO);
                   
                   if (deductionResult.success) {
                     logs.push(`[-] Deducted $${overspend.toLocaleString()} from [${category}] (Overspend) - ID: ${prId}`);
                   } else {
                     logs.push(`⚠️ ${prId}: Failed to deduct overspend $${overspend}: ${deductionResult.message}`);
                   }
                 } else {
                   Logger.log(`Skipping overspend deduction for ${prId} because Forecast Amount is 0 (Likely previously deducted unbudgeted item)`);
                 }
              }
           }
        }
        
        // Update PO Amount Column (AC)
        forecastSheet.getRange(rowIndex, CONFIG_V2.COLS.PO_AMOUNT).setValue(targetAmount).setNumberFormat('$#,##0.00');
        
        // Update Estimated Receiving Date (Column AG)
        if (group.estDate) {
            forecastSheet.getRange(rowIndex, CONFIG_V2.COLS.EST_RECEIVING_DATE).setValue(group.estDate).setNumberFormat('yyyy-MM-dd');
        } else {
            // Optional: Clear if no date, or leave it? Leaving it is safer.
        }
        
        // Update the Actual Period Column (M3) if identified
        if (targetCol > 0) {
           forecastSheet.getRange(rowIndex, targetCol).setValue(targetAmount).setNumberFormat('$#,##0.00');
           
           // Clean up other M3 columns to avoid double counting if moved
           const allM3 = [CONFIG_V2.COLS.Q1M3, CONFIG_V2.COLS.Q2M3, CONFIG_V2.COLS.Q3M3, CONFIG_V2.COLS.Q4M3];
           allM3.forEach(c => {
               if (c !== targetCol) {
                   // CRITICAL: Set to 0 instead of clearing content.
                   // Formula is IF(M3<>"", M3, M1). If we clear it (""), it reverts to M1 (Budget), causing double counting.
                   // By setting it to 0, we force it to use the Actual value (0), effectively "cancelling" the budget for that quarter in the Total calculation.
                   
                   // Only set to 0 if there is a corresponding M1 value (Budget existed)
                   // Map M3 -> M1
                   let m1Col = 0;
                   if (c === CONFIG_V2.COLS.Q1M3) m1Col = CONFIG_V2.COLS.Q1M1;
                   else if (c === CONFIG_V2.COLS.Q2M3) m1Col = CONFIG_V2.COLS.Q2M1;
                   else if (c === CONFIG_V2.COLS.Q3M3) m1Col = CONFIG_V2.COLS.Q3M1;
                   else if (c === CONFIG_V2.COLS.Q4M3) m1Col = CONFIG_V2.COLS.Q4M1;
                   
                   const budgetVal = parseFloat(forecastRow[m1Col - 1]) || 0;
                   if (budgetVal > 0) {
                       forecastSheet.getRange(rowIndex, c).setValue(0).setNumberFormat('$#,##0.00');
                   } else {
                       // If no budget, we can safely clear it
                       forecastSheet.getRange(rowIndex, c).clearContent();
                   }
               }
           });
           
        } else if (isPlaceholder) {
           // If it's a placeholder, we might want to deduct from the ACTIVE period M3 column
           // But deductFromPlaceholder handles the M1/M3 logic usually?
           // deductFromPlaceholder updates the "Active Period Column" which is defined in config.
        } else {
           Logger.log(`⚠️ Could not find target M3 column for ${prId} (No Forecast M1 value found and No Estimated Date)`);
        }
        
        itemUpdated = true;
      }

      // 3. Update Status
      // Logic:
      // - Only mark "Completed" if Received Amount >= PO Amount (Fully Received)
      // - Otherwise, keep as "In Progress" (or "Unbudgeted")
      
      const isFullyReceived = hasReceipt && (receivedAmount >= prAmount - 0.01); // Epsilon for float compare

      if (isFullyReceived) {
        if (currentStatus !== 'Completed') {
          forecastSheet.getRange(rowIndex, CONFIG_V2.COLS.STATUS).setValue('Completed');
          itemUpdated = true;
          logs.push(`[i] Status -> Completed (Fully Received: $${receivedAmount}) - ID: ${prId}`);
        }
      } else if (prPO && currentStatus !== 'In Progress' && currentStatus !== 'Completed' && currentStatus !== 'Unbudgeted') {
        // If PO exists but not fully received, and not already completed/unbudgeted -> In Progress
        forecastSheet.getRange(rowIndex, CONFIG_V2.COLS.STATUS).setValue('In Progress');
        itemUpdated = true;
      }
      
      if (itemUpdated) {
        updatedCount++;
      }
    }
  }
  
  if (updatedCount > 0) {
    let msg = `✅ Synced ${updatedCount} items.\n\n`;
    
    if (logs.length > 0) {
      msg += `DETAILS:\n${logs.slice(0, 15).join('\n')}`;
      if (logs.length > 15) msg += `\n...and ${logs.length - 15} more`;
    } else {
      msg += `(No variance adjustments needed)`;
    }
    
    ui.alert(msg);
  } else {
    ui.alert('ℹ️ No updates needed. Data is already in sync.');
  }
  
  // Ensure Conditional Formatting is applied and prioritized correctly
  setupStatusConditionalFormatting(forecastSheet, forecastSheet.getLastRow());
}

/**
 * Deducts amount from the matching Placeholder row in Forecast Sheet
 * @param {Object} sheet - The Forecast Sheet object
 * @param {string} category - Category name to search for (if specificId not provided)
 * @param {number} amount - Amount to deduct
 * @param {string} period - Active period (e.g. 'Q1M1')
 * @param {string} poNumber - PO Number for logging
 * @param {string} [specificId=null] - Optional: Target a specific Forecast ID instead of searching by category
 */
function deductFromPlaceholder(sheet, category, amount, period, poNumber, specificId = null, silent = false) {
  const lastRow = sheet.getLastRow();
  const data = sheet.getRange(4, 1, lastRow - 3, CONFIG_V2.COLS.CHANGE_LOG).getValues();
  
  // Find Placeholder Row
  let placeholderRowIndex = -1;
  let placeholderId = null;
  
  if (specificId) {
    Logger.log(`Looking for Placeholder by ID: ${specificId}`);
  } else {
    Logger.log(`Looking for Placeholder by Category: "${category}"`);
  }
  
  for (let i = 0; i < data.length; i++) {
    // Robust comparison: convert to string and trim whitespace
    const rowId = String(data[i][CONFIG_V2.COLS.FORECAST_ID - 1]).trim();
    const rowProjectName = String(data[i][CONFIG_V2.COLS.PROJECT_NAME - 1]).trim();
    const rowCategory = String(data[i][CONFIG_V2.COLS.CATEGORY - 1]).trim();
    const targetCategory = String(category).trim();
    
    if (specificId) {
      // Case A: Match by Specific ID (Direct Usage)
      if (rowId === String(specificId).trim()) {
        placeholderRowIndex = i + 4;
        placeholderId = rowId;
        break; 
      }
    } else {
      // Case B: Match by Category (Unbudgeted / Overspend fallback)
      if (rowProjectName === 'Placeholder' && rowCategory === targetCategory) {
        placeholderRowIndex = i + 4; // Convert to 1-based row index
        placeholderId = rowId;
        break;
      }
    }
  }
  
  if (placeholderRowIndex === -1) {
    if (specificId) {
       return { success: false, message: `Could not find Placeholder row with ID: ${specificId}` };
    }
    
    // Enhanced error logging for Category search
    const categoryMatches = [];
    for (let i = 0; i < data.length; i++) {
        if (String(data[i][CONFIG_V2.COLS.CATEGORY - 1]).trim() === String(category).trim()) {
            categoryMatches.push('Row ' + (i+4) + ' Project Name: "' + data[i][CONFIG_V2.COLS.PROJECT_NAME - 1] + '"');
        }
    }
    
    if (categoryMatches.length > 0) {
        return { success: false, message: 'Found category "' + category + '" but Project Name was not "Placeholder". Details: ' + categoryMatches.join(', ') };
    }
    
    return { success: false, message: 'No placeholder found for category: "' + category + '" (Check spelling/spaces in Forecast Sheet)' };
  }
  
  // Get Column for Active Period
  const config = getConfigValues_v2(); // Ensure we have config for period info if needed, though passed in
  const periodInfo = CONFIG_V2.PERIODS[period];
  if (!periodInfo) {
    return { success: false, message: `Invalid active period: ${period}` };
  }
  
  const amountCol = periodInfo.col;
  
  // Verify we have a valid column number
  if (!amountCol || isNaN(amountCol)) {
    return { success: false, message: `Invalid column configuration for period: ${period}` };
  }
  
  const cell = sheet.getRange(placeholderRowIndex, amountCol);
  const currentAmount = cell.getValue() || 0;
  const newAmount = currentAmount - amount;
  
  // Check for Negative Balance (Warning)
  if (newAmount < 0) {
    if (!silent) {
      const ui = SpreadsheetApp.getUi();
      const response = ui.alert(
        '⚠️ Insufficient Placeholder Funds',
        `Target: ${specificId || category}\n` +
        `Current Balance: $${currentAmount.toLocaleString()}\n` +
        `Required Deduction: $${amount.toLocaleString()}\n` +
        `New Balance would be: -$${Math.abs(newAmount).toLocaleString()}\n\n` +
        `Do you want to proceed anyway?`,
        ui.ButtonSet.YES_NO
      );
      
      if (response === ui.Button.NO) {
        return { success: false, message: 'Cancelled by user due to insufficient funds.' };
      }
    } else {
      Logger.log(`⚠️ Warning: Deduction created negative balance for ${specificId || category}. New Balance: ${newAmount}`);
      // In silent mode, we proceed (assuming automation should not block)
    }
  }
  
  // Update Amount
  cell.setValue(newAmount);
  
  // Log Change
  const user = Session.getActiveUser().getEmail();
  const timestamp = new Date();
  const logCell = sheet.getRange(placeholderRowIndex, CONFIG_V2.COLS.CHANGE_LOG);
  const existingLog = logCell.getValue() || '';
  const newLog = `${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')}: Deducted $${amount.toLocaleString()} for PO ${poNumber} (Sync) by ${user}`;
  logCell.setValue(existingLog ? `${existingLog}\n${newLog}` : newLog);
  
  return { success: true, placeholderId: placeholderId };
}

/**
 * Returns amount to the matching Placeholder row in Forecast Sheet
 * Used when actual PO amount is less than forecast
 */
function returnToPlaceholder(sheet, category, amount) {
  const lastRow = sheet.getLastRow();
  const data = sheet.getRange(4, 1, lastRow - 3, CONFIG_V2.COLS.CHANGE_LOG).getValues();
  
  // Find Placeholder Row
  let placeholderRowIndex = -1;
  
  for (let i = 0; i < data.length; i++) {
    const rowProjectName = String(data[i][CONFIG_V2.COLS.PROJECT_NAME - 1]).trim();
    const rowCategory = String(data[i][CONFIG_V2.COLS.CATEGORY - 1]).trim();
    const targetCategory = String(category).trim();
    
    if (rowProjectName === 'Placeholder' && rowCategory === targetCategory) {
      placeholderRowIndex = i + 4; // Convert to 1-based row index
      break;
    }
  }
  
  if (placeholderRowIndex === -1) {
    Logger.log(`⚠️ Could not find placeholder for category "${category}" to return $${amount}`);
    return false;
  }
  
  // Add to the active period column
  // We'll use the Active Period from config
  const config = getConfigValues_v2();
  const periodInfo = CONFIG_V2.PERIODS[config.activePeriod];
  
  if (!periodInfo) {
    Logger.log(`⚠️ Invalid active period: ${config.activePeriod}`);
    return false;
  }
  
  const amountCol = periodInfo.col;
  
  // Verify we have a valid column number
  if (!amountCol || isNaN(amountCol)) {
    Logger.log(`⚠️ Invalid column configuration for period: ${config.activePeriod}`);
    return false;
  }
  
  const cell = sheet.getRange(placeholderRowIndex, amountCol);
  const currentAmount = cell.getValue() || 0;
  const newAmount = currentAmount + amount;
  
  // Update Amount
  cell.setValue(newAmount);
  
  // Log Change
  const user = Session.getActiveUser().getEmail();
  const timestamp = new Date();
  const logCell = sheet.getRange(placeholderRowIndex, CONFIG_V2.COLS.CHANGE_LOG);
  const existingLog = logCell.getValue() || '';
  const newLog = `${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')}: Returned $${amount.toLocaleString()} (Variance) by ${user}`;
  const updatedLog = existingLog ? `${existingLog} | ${newLog}` : newLog;
  logCell.setValue(updatedLog);
  // FORCE NO WRAP
  logCell.setWrap(false).setWrapStrategy(SpreadsheetApp.WrapStrategy.CLIP);
  
  // Explicitly reset row height to 21
  sheet.setRowHeight(placeholderRowIndex, 21);
  SpreadsheetApp.flush();
  
  return true;
}

/**
 * Generates a Sub-ID based on the parent ID (e.g. T26Q1-001 -> T26Q1-001-1)
 */
function generateSubId(sheet, parentId) {
  const lastRow = sheet.getLastRow();
  const ids = sheet.getRange(4, CONFIG_V2.COLS.FORECAST_ID, lastRow - 3, 1).getValues().flat();
  
  // Find all IDs that start with parentId + "-" (e.g. T26Q1-001-1, T26Q1-001-2)
  const regex = new RegExp(`^${parentId}-(\\d+)$`);
  let maxSeq = 0;
  
  ids.forEach(id => {
    const match = String(id).match(regex);
    if (match) {
      const seq = parseInt(match[1]);
      if (seq > maxSeq) maxSeq = seq;
    }
  });
  
  return `${parentId}-${maxSeq + 1}`;
}

/**
 * Creates a new row in Forecast Sheet for the synced item
 */
function createForecastRow(sheet, data) {
  // Find the first truly empty row based on Column A (Forecast ID)
  const lastRowWithData = getTrueLastRow(sheet);
  const newRow = lastRowWithData + 1;
  
  // Use provided ID or generate new one
  const newId = data.id || generateUniqueId_v2(data.fiscalYear);
  
  // Set Values
  sheet.getRange(newRow, CONFIG_V2.COLS.FORECAST_ID).setValue(newId);
  
  // Set Requestor with error handling for validation
  try {
    sheet.getRange(newRow, CONFIG_V2.COLS.REQUESTOR).setValue(data.requestor);
  } catch (e) {
    // If strict validation fails, clear validation for this cell and retry
    const reqCell = sheet.getRange(newRow, CONFIG_V2.COLS.REQUESTOR);
    reqCell.clearDataValidations();
    reqCell.setValue(data.requestor);
    Logger.log(`⚠️ Validation cleared for Requestor: ${data.requestor}`);
  }

  // Set Category & Project & Item
  sheet.getRange(newRow, CONFIG_V2.COLS.CATEGORY).setValue(data.category);
  sheet.getRange(newRow, CONFIG_V2.COLS.PROJECT_NAME).setValue(data.projectName);
  sheet.getRange(newRow, CONFIG_V2.COLS.ITEM_NAME).setValue(data.itemName);
  sheet.getRange(newRow, CONFIG_V2.COLS.PO_NUMBER).setValue(data.poNumber);
  
  // Set Status to 'Unbudgeted' so it picks up the correct conditional formatting color
  // We explicitly clear data validation for this cell first to avoid "Exception: The data you entered ... violates the data validation rules"
  // This can happen if 'Unbudgeted' is not in the dropdown list or if row formatting was copied with strict validation
  const statusCell = sheet.getRange(newRow, CONFIG_V2.COLS.STATUS);
  statusCell.clearDataValidations(); 
  statusCell.setValue('Unbudgeted');
  
  // Re-apply correct validation allowing 'Unbudgeted' if needed, or rely on setupDataValidation() to fix it later
  // For now, leaving it without validation is safer for the script to succeed.
  
  // Set Amount in Active Period Column (M3)
  // Note: We want to write to the M3 (Actual) column for unbudgeted items
  // Logic: Map the configured active period (usually forecast/M1) to its M3 counterpart
  const config = getConfigValues_v2();
  const activePeriod = config.activePeriod; // e.g. "Q1M1"
  
  // Determine Target Column (M3)
  let targetCol = 0;
  
  // Priority 1: Use Estimated Receiving Date if available
  if (data.estDate) {
    const quarter = getQuarterFromDate(data.estDate);
    if (quarter === 'Q1') targetCol = CONFIG_V2.COLS.Q1M3;
    else if (quarter === 'Q2') targetCol = CONFIG_V2.COLS.Q2M3;
    else if (quarter === 'Q3') targetCol = CONFIG_V2.COLS.Q3M3;
    else if (quarter === 'Q4') targetCol = CONFIG_V2.COLS.Q4M3;
    
    if (targetCol > 0) {
      Logger.log(`Using Estimated Date ${data.estDate} -> Quarter ${quarter} -> Column M3`);
    }
  }
  
  // Priority 2: Use Active Period
  if (targetCol === 0) {
    // Direct mapping if active period is M1
    if (activePeriod === 'Q1M1') targetCol = CONFIG_V2.COLS.Q1M3;
    else if (activePeriod === 'Q2M1') targetCol = CONFIG_V2.COLS.Q2M3;
    else if (activePeriod === 'Q3M1') targetCol = CONFIG_V2.COLS.Q3M3;
    else if (activePeriod === 'Q4M1') targetCol = CONFIG_V2.COLS.Q4M3;
    // Fallback: If active period is already M3 (e.g. Q1M3), use it directly
    else if (CONFIG_V2.PERIODS[activePeriod]) {
      targetCol = CONFIG_V2.PERIODS[activePeriod].col;
    }
  }
  
  if (targetCol > 0) {
    const amountCell = sheet.getRange(newRow, targetCol);
    amountCell.setValue(data.amount);
    amountCell.setNumberFormat('$#,##0.00'); 
  } else {
    Logger.log(`⚠️ Could not determine M3 column for active period: ${activePeriod}`);
  }
  
  // Set PO Amount (Column AC) so subsequent syncs don't treat it as new
  sheet.getRange(newRow, CONFIG_V2.COLS.PO_AMOUNT).setValue(data.amount).setNumberFormat('$#,##0.00');
  
  // Set Estimated Receiving Date if available (Column AG)
  if (data.estDate) {
    sheet.getRange(newRow, CONFIG_V2.COLS.EST_RECEIVING_DATE).setValue(data.estDate).setNumberFormat('yyyy-MM-dd');
  }
  
  // Set Total FY Formula
  const totalFYCell = sheet.getRange(newRow, CONFIG_V2.COLS.TOTAL_FY);
  totalFYCell.setFormula(`=IF(S${newRow}<>"",S${newRow},R${newRow}) + IF(U${newRow}<>"",U${newRow},T${newRow}) + IF(W${newRow}<>"",W${newRow},V${newRow}) + IF(Y${newRow}<>"",Y${newRow},X${newRow})`);
  
  // Log Creation
  const user = Session.getActiveUser().getEmail();
  const timestamp = new Date();
  const logCell = sheet.getRange(newRow, CONFIG_V2.COLS.CHANGE_LOG);
  const log = `Synced from PR Sheet on ${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')} by ${user}`;
  logCell.setValue(log);
  
  // Format Row (copy formats from previous row)
  if (newRow > 4) {
    // Copy formats but NOT validation if it causes issues, though standard usually is fine
    // We just copy basic styles
    sheet.getRange(newRow - 1, 1, 1, CONFIG_V2.COLS.CHANGE_LOG).copyFormatToRange(sheet, 1, CONFIG_V2.COLS.CHANGE_LOG, newRow, newRow);
  }

  return { id: newId, row: newRow };
}

/**
 * Helper to find the visual last row based on Forecast ID column
 * Ignores rows that are empty but might have formatting
 */
function getTrueLastRow(sheet) {
  const lastRow = sheet.getLastRow();
  if (lastRow < 4) return 3; // Header is row 3
  
  // Get all Forecast IDs from row 4 down to the last row
  const data = sheet.getRange(4, CONFIG_V2.COLS.FORECAST_ID, lastRow - 3, 1).getValues();
  
  // Scan backwards to find the first non-empty value
  for (let i = data.length - 1; i >= 0; i--) {
    if (data[i][0] && String(data[i][0]).trim() !== "") {
      return i + 4; // Convert array index back to sheet row number
    }
  }
  
  return 3; // If no data found in Column A, start after headers
}

/**
 * Handles manual "Carry To Quarter" action triggered by Status change.
 * Moves the budget (M1) from the current quarter to the target quarter.
 * @param {Object} sheet - Forecast Sheet
 * @param {number} row - Row Index
 * @param {string} statusValue - Status string (e.g. "Carry To Q2")
 */
function handleCarryToQuarter(sheet, row, statusValue) {
  const ui = SpreadsheetApp.getUi();
  const targetQuarter = statusValue.replace('Carry To ', ''); // "Q2"
  
  // 1. Identify Target Column
  let targetCol = 0;
  if (targetQuarter === 'Q1') targetCol = CONFIG_V2.COLS.Q1M1;
  else if (targetQuarter === 'Q2') targetCol = CONFIG_V2.COLS.Q2M1;
  else if (targetQuarter === 'Q3') targetCol = CONFIG_V2.COLS.Q3M1;
  else if (targetQuarter === 'Q4') targetCol = CONFIG_V2.COLS.Q4M1;
  
  if (targetCol === 0) {
    ui.alert('❌ Invalid Target Quarter.');
    return;
  }
  
  // 2. Identify Source Column (Where is the money currently?)
  // We check M1 columns in order. The first one with value > 0 is assumed to be the current plan.
  const m1Cols = [
    { name: 'Q1', col: CONFIG_V2.COLS.Q1M1 },
    { name: 'Q2', col: CONFIG_V2.COLS.Q2M1 },
    { name: 'Q3', col: CONFIG_V2.COLS.Q3M1 },
    { name: 'Q4', col: CONFIG_V2.COLS.Q4M1 }
  ];
  
  let sourceCol = 0;
  let sourceQuarter = '';
  let amount = 0;
  
  for (const q of m1Cols) {
    const val = parseFloat(sheet.getRange(row, q.col).getValue()) || 0;
    if (val > 0) {
      sourceCol = q.col;
      sourceQuarter = q.name;
      amount = val;
      break; // Found the first planned quarter
    }
  }
  
  if (sourceCol === 0) {
    ui.alert('ℹ️ No budget found to carry over (M1 columns are empty).');
    return;
  }
  
  // 3. Validation
  // Check if trying to carry backwards (e.g. Q2 -> Q1)
  const qMap = { 'Q1': 1, 'Q2': 2, 'Q3': 3, 'Q4': 4 };
  if (qMap[targetQuarter] <= qMap[sourceQuarter]) {
    ui.alert(`⚠️ Cannot carry backwards or to same quarter (${sourceQuarter} -> ${targetQuarter}).`);
    return;
  }
  
  // 4. Execute Move
  // Set Target M1
  sheet.getRange(row, targetCol).setValue(amount).setNumberFormat('$#,##0.00');
  
  // Clear Source M1
  sheet.getRange(row, sourceCol).setValue(0).setNumberFormat('$#,##0.00'); 
  
  // Update Log
  const timestamp = new Date();
  const user = Session.getActiveUser().getEmail();
  const logCell = sheet.getRange(row, CONFIG_V2.COLS.CHANGE_LOG);
  const existingLog = logCell.getValue() || '';
  const newLogEntry = `${Utilities.formatDate(timestamp, Session.getScriptTimeZone(), 'yyyy-MM-dd HH:mm')}: Carried $${amount} from ${sourceQuarter} to ${targetQuarter} by ${user}`;
  const updatedLog = existingLog ? `${existingLog}\n${newLogEntry}` : newLogEntry;
  logCell.setValue(updatedLog);
  
  // Reset Status to "Active"
  sheet.getRange(row, CONFIG_V2.COLS.STATUS).setValue('Active');
  
  ui.alert(`✅ Successfully carried $${amount} from ${sourceQuarter} to ${targetQuarter}. Status reset to "Active".`);
}

// ============================================================================
// VIEW MODES & VISUALIZATION
// ============================================================================

function viewMode_AOP() { switchViewMode('AOP'); }
function viewMode_Execution() { switchViewMode('EXECUTION'); }
function viewMode_FocusActive() { switchViewMode('FOCUS_ACTIVE'); }
function viewMode_All() { switchViewMode('ALL'); }

function switchViewMode(mode) {
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const sheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  if (!sheet) return;
  
  const maxCols = sheet.getMaxColumns();
  
  // 1. Reset: Show all columns first
  sheet.showColumns(1, maxCols);
  
  if (mode === 'ALL') {
    SpreadsheetApp.getUi().alert('✅ Showing all columns.');
    return;
  }
  
  // Define Column Ranges (1-based)
  const cols = CONFIG_V2.COLS;
  
  const ranges = {
    INFO: { start: 1, end: 5 },          // A-E
    AOP05: { start: 6, end: 9 },         // F-I
    AOP1: { start: 10, end: 13 },        // J-M
    AOP_FINAL: { start: 14, end: 17 },   // N-Q
    FORECAST: { start: 18, end: 25 },    // R-Y (Q1-Q4 M1/M3)
    SUMMARY: { start: 26, end: 36 }      // Z-AJ
  };
  
  if (mode === 'AOP') {
    // AOP View: Show Info + AOPs. Hide Forecast + Summary (except Total?)
    // Actually, AOP planners might want to see Total FY too.
    // Let's hide Forecast columns (R-Y) and maybe Auto-tracking
    
    // Hide Forecasts (18-25)
    sheet.hideColumns(ranges.FORECAST.start, ranges.FORECAST.end - ranges.FORECAST.start + 1);
    
    // Hide PO info in Summary (29-30)
    sheet.hideColumns(cols.PO_AMOUNT, 2);
    
    SpreadsheetApp.getUi().alert('✅ Switched to AOP Planning View.\n(Hidden: Quarterly Forecasts)');
  } 
  else if (mode === 'EXECUTION') {
    // Execution View: Hide old AOP rounds (0.5 and 1). Show Final + Forecasts.
    
    // Hide AOP 0.5 (6-9)
    sheet.hideColumns(ranges.AOP05.start, ranges.AOP05.end - ranges.AOP05.start + 1);
    
    // Hide AOP 1 (10-13)
    sheet.hideColumns(ranges.AOP1.start, ranges.AOP1.end - ranges.AOP1.start + 1);
    
    SpreadsheetApp.getUi().alert('✅ Switched to Execution View.\n(Hidden: AOP 0.5, AOP 1)');
  }
  else if (mode === 'FOCUS_ACTIVE') {
    // Focus View: Show only columns relevant to CURRENT Quarter
    // 1. Hide old AOPs (0.5, 1)
    sheet.hideColumns(ranges.AOP05.start, ranges.AOP05.end - ranges.AOP05.start + 1);
    sheet.hideColumns(ranges.AOP1.start, ranges.AOP1.end - ranges.AOP1.start + 1);
    
    const config = getConfigValues_v2();
    const activePeriod = config.activePeriod; // e.g. Q1M1
    
    let targetQ = '';
    if (activePeriod.includes('Q1')) targetQ = 'Q1';
    else if (activePeriod.includes('Q2')) targetQ = 'Q2';
    else if (activePeriod.includes('Q3')) targetQ = 'Q3';
    else if (activePeriod.includes('Q4')) targetQ = 'Q4';
    
    if (!targetQ) {
      SpreadsheetApp.getUi().alert('⚠️ Could not determine active quarter from config.');
      return;
    }
    
    // Define columns to KEEP per quarter (AOP Final Qx, QxM1, QxM3)
    const qCols = {
      'Q1': [cols.AOPF_Q1, cols.Q1M1, cols.Q1M3],
      'Q2': [cols.AOPF_Q2, cols.Q2M1, cols.Q2M3],
      'Q3': [cols.AOPF_Q3, cols.Q3M1, cols.Q3M3],
      'Q4': [cols.AOPF_Q4, cols.Q4M1, cols.Q4M3]
    };
    
    // Hide IRRELEVANT AOP Final columns
    // Loop through AOP Final columns (14-17)
    for (let c = 14; c <= 17; c++) {
      if (!qCols[targetQ].includes(c)) sheet.hideColumns(c);
    }
    
    // Hide IRRELEVANT Forecast columns
    // Loop through Forecast columns (18-25)
    for (let c = 18; c <= 25; c++) {
      if (!qCols[targetQ].includes(c)) sheet.hideColumns(c);
    }
    
    SpreadsheetApp.getUi().alert(`✅ Focus View: ${targetQ}\n(Showing only ${targetQ} data for side-by-side comparison)`);
  }
}

// ============================================================================
// AUTO-COPY / SEEDING
// ============================================================================

/**
 * Copies AOP Final values to Forecast (M1) column for the active quarter.
 * Only fills empty cells (does not overwrite existing forecast data).
 */
function seedForecastFromAOP() {
  const ui = SpreadsheetApp.getUi();
  const ss = SpreadsheetApp.getActiveSpreadsheet();
  const sheet = ss.getSheetByName(CONFIG_V2.FORECAST_SHEET);
  
  if (!sheet) return;
  
  const config = getConfigValues_v2();
  const activePeriod = config.activePeriod;
  
  // Determine Columns
  let aopCol = 0;
  let forecastCol = 0;
  let quarterLabel = '';
  
  if (activePeriod.includes('Q1')) {
    aopCol = CONFIG_V2.COLS.AOPF_Q1;
    forecastCol = CONFIG_V2.COLS.Q1M1;
    quarterLabel = 'Q1';
  } else if (activePeriod.includes('Q2')) {
    aopCol = CONFIG_V2.COLS.AOPF_Q2;
    forecastCol = CONFIG_V2.COLS.Q2M1;
    quarterLabel = 'Q2';
  } else if (activePeriod.includes('Q3')) {
    aopCol = CONFIG_V2.COLS.AOPF_Q3;
    forecastCol = CONFIG_V2.COLS.Q3M1;
    quarterLabel = 'Q3';
  } else if (activePeriod.includes('Q4')) {
    aopCol = CONFIG_V2.COLS.AOPF_Q4;
    forecastCol = CONFIG_V2.COLS.Q4M1;
    quarterLabel = 'Q4';
  }
  
  if (aopCol === 0 || forecastCol === 0) {
    ui.alert('❌ Could not determine columns for Active Period: ' + activePeriod);
    return;
  }
  
  const response = ui.alert(
    `Initialize Forecast for ${quarterLabel}?`,
    `This will copy "AOP Final ${quarterLabel}" values to "Forecast ${quarterLabel}" (Column ${columnToLetter(forecastCol)}).\n\n` +
    `Only EMPTY forecast cells will be filled.\nExisting data will be preserved.\n\nProceed?`,
    ui.ButtonSet.YES_NO
  );
  
  if (response === ui.Button.NO) return;
  
  // Process Rows
  const lastRow = sheet.getLastRow();
  if (lastRow < 4) {
    ui.alert('ℹ️ No data rows found.');
    return;
  }
  
  // Read Data: AOP Col and Forecast Col
  // We read the whole range to handle updates efficiently
  // Actually, let's read just the two columns to check, but we need to write individually or batch
  // Optimization: Read entire block to minimize calls
  
  const startRow = 4;
  const numRows = lastRow - 3;
  
  const aopValues = sheet.getRange(startRow, aopCol, numRows, 1).getValues();
  const forecastRange = sheet.getRange(startRow, forecastCol, numRows, 1);
  const forecastValues = forecastRange.getValues();
  
  let updateCount = 0;
  const newValues = [];
  
  for (let i = 0; i < numRows; i++) {
    const aopVal = parseFloat(aopValues[i][0]) || 0;
    const currentForecast = forecastValues[i][0];
    
    // Check if Forecast is empty/blank (null or "") AND AOP has value
    // We treat 0 as a value if explicitly entered, but usually "" is the target
    if ((currentForecast === '' || currentForecast === null) && aopVal > 0) {
      newValues.push([aopVal]);
      updateCount++;
    } else {
      newValues.push([currentForecast]); // Keep existing
    }
  }
  
  if (updateCount > 0) {
    forecastRange.setValues(newValues);
    
    // Format as currency
    forecastRange.setNumberFormat('$#,##0.00');
    
    ui.alert(`✅ Successfully initialized ${updateCount} items from AOP ${quarterLabel}.`);
  } else {
    ui.alert('ℹ️ No changes made. All items already have forecast data or no AOP budget.');
  }
}
