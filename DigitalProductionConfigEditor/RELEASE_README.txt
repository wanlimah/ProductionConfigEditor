════════════════════════════════════════════════════════════════
      Digital Production Config Editor - Release Package
════════════════════════════════════════════════════════════════

VERSION INFORMATION
───────────────────────────────────────────────────────────────

Two versions are included:

📁 NET6\
   - For Windows 10 (version 1809+) and Windows 11
   - Requires .NET 6.0 Runtime
   - Most compatible with older systems

📁 NET8\
   - For Windows 11 (version 22000+)
   - Requires .NET 8.0 Runtime
   - Latest features and performance

════════════════════════════════════════════════════════════════

WHICH VERSION TO USE?
───────────────────────────────────────────────────────────────

✓ Use NET6 version if:
  - Running Windows 10
  - Need maximum compatibility
  - Already have .NET 6 installed

✓ Use NET8 version if:
  - Running Windows 11
  - Want latest performance
  - Already have .NET 8 installed

Both versions have identical functionality!

════════════════════════════════════════════════════════════════

INSTALLATION
───────────────────────────────────────────────────────────────

1. Choose NET6 or NET8 folder based on your system

2. Copy the entire folder to your desired location
   Example: C:\Program Files\DigitalProductionConfigEditor\

3. Run DigitalProductionConfigEditor.exe

4. If you get an error about missing .NET runtime:
   - NET6: Download from https://dotnet.microsoft.com/download/dotnet/6.0
   - NET8: Download from https://dotnet.microsoft.com/download/dotnet/8.0
   - Install "Desktop Runtime" (not SDK)

════════════════════════════════════════════════════════════════

KEY FEATURES
───────────────────────────────────────────────────────────────

✓ Wizard-based XML configuration editor
✓ Step 1: Build configuration from master template
✓ Step 2: Manage packages and attributes
✓ Step 3: Review and save

🔒 SAFETY FEATURE:
   All packages default to enable="FALSE"
   - Safer than enable="TRUE" default
   - Enable only the packages you need
   - Prevents accidental activation

════════════════════════════════════════════════════════════════

USAGE WORKFLOW
───────────────────────────────────────────────────────────────

1. Launch the application

2. STEP 1: Build Your Configuration
   - Browse master template on the left
   - Click "Add" to copy configuration nodes
   - All packages automatically set to enable="FALSE"

3. STEP 2: Manage Packages
   - Select a configuration to edit
   - Add new packages with "+ Add Single Product"
   - Edit existing packages as needed
   - Change enable to "TRUE" for packages you want active

4. STEP 3: Review & Save
   - Review your configuration
   - Save to XML file
   - Use in production system

════════════════════════════════════════════════════════════════

IMPORTANT NOTES
───────────────────────────────────────────────────────────────

📌 Master XML File:
   The app requires "Master_Digital_ProductionUserConfig.xml"
   in the same folder as the .exe file
   
   🔒 PROTECTION: This file is set to READ-ONLY
   - The app automatically protects it from editing
   - A backup copy is included: Master_Digital_ProductionUserConfig.BACKUP.xml
   - If Master XML gets corrupted, rename the .BACKUP file

📌 Enable Attribute Default:
   NEW: All packages default to enable="FALSE"
   You manually enable only what you need

📌 Data Safety:
   Original master template is never modified
   You create a new XML file with your changes

════════════════════════════════════════════════════════════════

TROUBLESHOOTING
───────────────────────────────────────────────────────────────

❌ "Application failed to start"
   → Install .NET Desktop Runtime (see Installation section)

❌ "Master XML not found"
   → Ensure Master_Digital_ProductionUserConfig.xml is in same folder
   → Check if file was accidentally deleted
   → Use the .BACKUP file: Rename Master_Digital_ProductionUserConfig.BACKUP.xml
     to Master_Digital_ProductionUserConfig.xml

❌ "Master XML is corrupted or invalid"
   → Restore from backup:
     1. Delete Master_Digital_ProductionUserConfig.xml
     2. Copy Master_Digital_ProductionUserConfig.BACKUP.xml
     3. Rename the copy to Master_Digital_ProductionUserConfig.xml
     4. Restart the app

❌ "Can't save file"
   → Check file permissions
   → Don't save to Program Files (use Documents instead)
   → Make sure you're not trying to overwrite the Master XML
     (it's read-only and shouldn't be modified)

════════════════════════════════════════════════════════════════

SYSTEM REQUIREMENTS
───────────────────────────────────────────────────────────────

NET6 Version:
  - Windows 10 (1809+) or Windows 11
  - .NET 6.0 Desktop Runtime
  - 100 MB disk space
  - 512 MB RAM minimum

NET8 Version:
  - Windows 11 (22000+)
  - .NET 8.0 Desktop Runtime
  - 100 MB disk space
  - 512 MB RAM minimum

════════════════════════════════════════════════════════════════

For support or questions, contact your IT administrator.

════════════════════════════════════════════════════════════════

