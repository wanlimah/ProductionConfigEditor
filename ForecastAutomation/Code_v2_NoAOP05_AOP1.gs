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
// INITIALIZATION & MENU
// ============================================================================

function onOpen() {
  const ui = SpreadsheetApp.getUi();
  ui.createMenu('📊 TDG Forecast (v2)')
    .addItem('🎯 Generate Forecast IDs', 'generateForecastIDs_v2')
    .addSeparator()
    .addItem('🔄 Update Open Items Dashboard', 'updateOpenItemsDashboard')
    .addItem('📊 Generate Variance Analysis', 'generateVarianceAnalysis')
    .addSeparator()
    .addItem('📧 Send Period Reminders', 'sendPeriodReminders')
    .addItem('👥 Check User Completion Status', 'checkUserCompletionStatus')
    .addSeparator()
    .addItem('⚙️ Setup Data Validation', 'setupDataValidation')
    .addItem('🔢 Setup Total Row', 'setupTotalRow')
    .addItem('📋 Initialize Config', 'initializeConfig_v2')
    .addItem('⏰ Setup Automated Triggers', 'setupTriggers_v2')
    .addToUi();
}

// NOTE: Rest of the functions remain the same as Code_v2_ColumnBased.gs
// Only the column numbers have changed (shifted left by 8 columns)

// The setupTotalRow function needs updating:
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
  // UPDATED: No more AOP 0.5 or AOP 1
  const sumColumns = [
    // AOP Final (H-K)
    CONFIG_V2.COLS.AOPF_Q1, CONFIG_V2.COLS.AOPF_Q2, CONFIG_V2.COLS.AOPF_Q3, CONFIG_V2.COLS.AOPF_Q4,
    // Quarterly Forecasts (L-S)
    CONFIG_V2.COLS.Q1M1, CONFIG_V2.COLS.Q1M3, CONFIG_V2.COLS.Q2M1, CONFIG_V2.COLS.Q2M3,
    CONFIG_V2.COLS.Q3M1, CONFIG_V2.COLS.Q3M3, CONFIG_V2.COLS.Q4M1, CONFIG_V2.COLS.Q4M3,
    // Total FY (T)
    CONFIG_V2.COLS.TOTAL_FY,
    // Carry To (U)
    CONFIG_V2.COLS.CARRY_TO
  ];
  
  // Add SUM formulas for each column
  sumColumns.forEach(col => {
    const columnLetter = columnToLetter(col);
    const formula = `=SUM(${columnLetter}4:${columnLetter})`;
    forecastSheet.getRange(totalRow, col).setFormula(formula);
  });
  
  // Format the total row
  const totalRange = forecastSheet.getRange(totalRow, 1, 1, CONFIG_V2.COLS.CHANGE_LOG);
  totalRange.setBackground('#FFF2CC')  // Light yellow background
    .setFontWeight('bold')
    .setFontSize(11);
  
  // Format number columns as currency
  sumColumns.forEach(col => {
    forecastSheet.getRange(totalRow, col).setNumberFormat('#,##0.00');
  });
  
  SpreadsheetApp.getUi().alert('✅ Total row setup complete!\n\n' +
    'Row 2 now shows totals for all cost columns.\n' +
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

// NOTE: Copy all other functions from Code_v2_ColumnBased.gs
// The column references in CONFIG_V2.COLS will automatically adjust all functions











