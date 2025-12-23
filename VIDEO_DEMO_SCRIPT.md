# Digital Production Config Editor - Video Demo Script
*Note: Text in quotes "..." is spoken narration. Text in brackets [...] is visual direction.*

## Scene 1: Introduction & The "Why"
**[Visual: Split screen. Left side: Someone struggling with Notepad XML (red text/error). Right side: The new sleek UI (green checkmark).]**

"Welcome to the Digital Production Config Editor demo. 

If you've ever broken a production config because of a missing bracket or a typo in Notepad, you know how fragile XML editing can be. The 'Old Way' was risky and slow.

This new tool solves that problem completely. It provides a safe, visual interface that guarantees your configurations are syntax-perfect every time."

## Scene 2: Launch & Interactive Guide
**[Visual: Desktop. Mouse double-clicks `ProductionConfigEditor.exe`. App opens.]**

"Getting started is simple. Just launch the application. It runs on standard .NET 6 or 8."

**[Visual: The Guide overlay appears. Mouse clicks 'Next', 'Next', 'Got it'.]**

"On your first launch, you'll see this Interactive Guide. It walks you through the basics. You can disable it for future runs, or bring it back anytime by clicking the **Help (?)** icon at the bottom."

## Scene 3: The Core Workflow (Production Config)
**[Visual: Main Window. Highlight the 3 steps at the top or bottom if visible.]**

"The workflow is designed in three logical steps."

### Step 1: Select Features
**[Visual: 'Production User Config' tab. Mouse hovers over left panel.]**

"First, we select *what* we want to configure. The Left Panel shows the Master List—this is your 'Menu' of available options."

**[Action: Check `RT_Timing_Test_Trace_Enable` and `PinSweepPowerdrop`.]**

"Let's grab `RT_Timing_Test_Trace_Enable` and `PinSweepPowerdrop`. Check the boxes, then click **Add**."

**[Action: Click Add. Items move to Right Panel.]**

"See how they move to the Right Panel? This is your current configuration. Once you have your list, click **Next**."

### Step 2: Configure Details (The Power Feature)
**[Visual: Step 2 Screen. Dropdown menu.]**

"Now we configure the specifics. Select a package from the dropdown to edit it."

**[Action: Select a package. Click 'Edit'.]**

"You can add a single product manually..."

**[Action: Click 'Bulk Add Products' button.]**

"...but the real time-saver is **Bulk Add**. 
Instead of typing lines one by one, just paste your list of products here."

**[Action: Type/Paste `8267-PROD`, `8268-PROD` into the text box. Click 'Generate Grid'.]**

"Click **Generate Grid**, and the tool automatically builds the correct structure for you, setting everything to 'Enable' by default. Click **Add All Products** to confirm. 

This turns a 10-minute task into a 10-second one."

### Step 3: Review & Save
**[Visual: Review Screen.]**

"Finally, Step 3: Review. If everything looks good, hit **Save**."

**[Action: Click Save. File Dialog appears. Save as `MyConfig.xml`.]**

"The tool exports a perfectly formatted XML file, ready for production. You can even open the file location directly from here."

## Scene 4: specialized Configurations
**[Visual: Briefly show Developer Tab.]**

"The **Developer Tab** works exactly the same way, but filters for engineering-only packages."

**[Visual: Switch to PCB Format Config Tab.]**

"For **PCB Format Configs**, we have a visual editor. 
You can add 'Islands', adjust their coordinates, and delete them visually. No more guessing numbers in a text file."

## Scene 5: Admin Features (Master List)
**[Visual: Click 'Edit Master' button. Enter password.]**

"For Administrators, the **Edit Master** feature allows you to update the 'Source of Truth'. 
If a new feature is released, Admins can add it here once, and it becomes available for the whole team immediately. This ensures everyone is using valid, up-to-date configurations."

## Scene 6: Conclusion
**[Visual: Return to Main Screen or show the generated XML code looking clean.]**

"To wrap up, the Digital Production Config Editor is:
1.  **Safe**: It prevents syntax errors.
2.  **Standardized**: It keeps everyone on the Master definitions.
3.  **Fast**: Bulk tools save significant engineering time.

The tool is available now. We highly recommend switching to this workflow for your next configuration update.

Thank you for watching."
