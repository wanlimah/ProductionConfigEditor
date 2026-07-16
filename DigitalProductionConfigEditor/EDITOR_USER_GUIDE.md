# 🛠️ Config Editor User Guide

**Tool:** `DigitalProductionConfigEditor`
**Purpose:** Safely edit the `Master_ProductionUserConfig.xml` file without manual text editing.

---

## 1. Getting Started

1.  **Launch the Application**.
2.  The application will automatically load the default configuration file:
    *   `Master_ProductionUserConfig.xml`
3.  **Status Bar**: Check the bottom of the window to see the loaded file path.

---

## 2. Viewing Configurations

The main screen displays the configuration tree:

1.  **Left Panel (Tree View)**: Shows the hierarchy of the XML.
    *   **Nodes (Folders)**: Represent Feature Categories (e.g., `STOP_TEST_ON_FAILURE`).
    *   **Leaves (Items)**: Represent specific Packages (e.g., `WW-PROD`, `SUSER`).
2.  **Right Panel (Details)**:
    *   Select a **Package** in the tree to view its settings.
    *   You will see fields like `Enable`, `Count`, `Name`, etc.

---

## 3. Editing Values

### Modifying a Value
1.  Expand the Feature Node (e.g., `PARAM_COUNT_CHECK`).
2.  Select the **Package** you want to change (e.g., `WW-PROD`).
3.  In the details panel:
    *   **Dropdowns**: Select `TRUE`/`FALSE` for enable.
    *   **Text Boxes**: Type new numbers for `count` or `threshold`.
4.  **Press Enter** or click away to confirm the change in the UI memory.

### Adding a New Package
1.  Right-click on a **Feature Node** (e.g., `TTD_FILE_ENABLE`).
2.  Select **"Add Package"**.
3.  Enter the name of the new package (e.g., `New-Product-A`).
4.  The new package will appear with default settings (usually `enable="FALSE"`).

---

## 4. Saving Changes

1.  Click the **Save** button (or `File > Save`).
2.  **Validation**: The tool may check for errors before saving.
    *   *Note: Ensure no invalid characters are entered in text fields.*
3.  The file on disk is updated immediately.

---

## 5. Troubleshooting

| Issue | Solution |
| :--- | :--- |
| **"File not found"** | Ensure the XML file is in the same directory as the executable. |
| **Values not saving** | Make sure to click "Save" before closing the application. |
| **Unknown Node** | If you manually edited the XML and broke the structure, the tool might fail to load. Restore a backup. |






























































































