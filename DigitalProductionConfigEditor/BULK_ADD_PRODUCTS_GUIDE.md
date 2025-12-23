# Bulk Add Products Feature Guide

## Overview
The **Bulk Add Products** feature allows you to quickly add multiple product names (packages) to a configuration at once, instead of adding them one by one. This is especially useful when you need to configure many products that share the same attributes.

## How to Access
1. Navigate to **Step 2: Manage Packages**
2. Select a configuration from the dropdown
3. Click the **📋 Bulk Add Products** button

## How to Use

### Step 1: Enter Product Names
- In the left panel, enter product names one per line
- Example:
  ```
  AFEM-8266-AP1-RF1-QA
  AFEM-8267-AP1-RF1-QA
  WW-PROD
  WW-QA
  RS-PROD
  ```

### Step 2: Configure Options
The dialog provides several options:
- **Auto-convert to UPPERCASE**: Automatically converts all product names to uppercase (recommended)
- **Remove empty lines**: Automatically removes blank lines from your input
- **Remove duplicate names**: Ensures each product name is added only once

### Step 3: Preview
- The right panel shows a live preview of the packages that will be created
- You can see exactly how each package will look in the XML
- The count of products to be added is displayed at the bottom of the left panel

### Step 4: Add Products
- Review the preview to ensure everything looks correct
- Click **📋 Add All Products** to create all packages at once
- A confirmation dialog will show you how many products were added
- If any duplicate names are detected, they will be skipped and listed in the summary

## Default Attributes
- The bulk add feature automatically uses the same attributes as existing packages in the configuration
- This ensures consistency across all packages in the same configuration
- Common attributes include: `enable`, `count`, `sampling`, `threshold`, `mode`, etc.

## Example Workflow

### Scenario: Adding 5 new products to WW Configuration
1. Select "GU_ENGINEERING_MODE_ENABLE" configuration
2. Click **📋 Bulk Add Products**
3. Enter the following product names:
   ```
   PRODUCT-A
   PRODUCT-B
   PRODUCT-C
   PRODUCT-D
   PRODUCT-E
   ```
4. Review the preview - each will have `enable="FALSE"` (or matching existing packages)
5. Click **📋 Add All Products**
6. Result: 5 new packages added instantly!

## Benefits
- ⚡ **Fast**: Add dozens of products in seconds
- ✅ **Consistent**: All products use the same attribute template
- 🔄 **Safe**: Duplicate detection prevents accidental overwrites
- 👁 **Visual**: Live preview shows exactly what will be created

## Tips
- Copy product names from Excel or a text file and paste them into the input area
- Use UPPERCASE for consistency with existing packages
- Review the preview before adding to catch any typos
- If you need different attributes for some products, add them in bulk first, then edit individually

## Comparison: Single vs Bulk Add

### Single Add (Traditional)
- Click "➕ Add Single Product"
- Enter one product name
- Configure attributes
- Click "Create Package"
- Repeat for each product

### Bulk Add (New Feature)
- Click "📋 Bulk Add Products"
- Paste all product names (one per line)
- Review preview
- Click "Add All Products"
- Done!

## Related Features
- **Single Product Add**: Use "➕ Add Single Product" when you need to add just one product with custom attributes
- **Edit Package**: After bulk adding, you can still edit individual packages using the "✏️ Edit" button
- **Delete Package**: Remove unwanted packages using the "🗑 Delete" button

## Note
Remember to **save your XML** after bulk adding products! Changes are not persisted until you click the "💾 Save" or "💾 Save As" button in the main window.









































