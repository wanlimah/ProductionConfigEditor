# 📋 Production Configuration Request Table

Copy the table below into your documentation. This format ensures all necessary information is captured for updates to `Master_ProductionUserConfig.xml`.

---

| Function Tag (XML Node) | Description & Allowed Values (Reference) | Desired Settings (Team Input) | Applicable Stage (RF1 / RF2 / NFR) | Enable? (TRUE/FALSE) |
| :--- | :--- | :--- | :--- | :--- |
| **`ENA_SNP_FILE_ENABLE`** | Enables saving of S-Parameter (SNP) files.<br><br>**Parameters:**<br>- `sampling`: Integer (e.g., 5 = every 5th unit) | `sampling`: _____ | **RF2** | **TRUE** |
| **`ENA_AVERAGE_MODE_ON`** | Controls Network Analyzer averaging behavior.<br><br>**Parameters:**<br>- `mode`: SWEEP \| POINT<br>- `count`: Integer (0-1500)<br>- `avg_channel`: ALL \| EACH | `mode`: _____<br>`count`: _____<br>`avg_channel`: _____ | **RF2** | **TRUE** |
| **`TTD_FILE_ENABLE`** | Generates detailed debug logs (Time-To-Die).<br><br>**Parameters:**<br>- `count`: Max files to save<br>- `sampling`: Save interval | `count`: _____<br>`sampling`: _____ | **ALL** | **FALSE** |
| **`STOP_TEST_ON_CONTINUOUS_FAILURE`** | Stops testing after consecutive failures.<br><br>**Parameters:**<br>- `count`: Number of failures (e.g., 5, 30) | `count`: _____ | **ALL** | **TRUE** |
| **`HANDLER_ARM_YIELD_DELTA_ENABLE`** | Checks yield difference between arms.<br><br>**Parameters:**<br>- `count`: Units to test<br>- `threshold`: Max % diff | `count`: _____<br>`threshold`: _____ | **ALL** | **FALSE** |
| **`PARAM_COUNT_CHECK`** | Verifies result file parameter count.<br><br>**Parameters:**<br>- `count`: Expected param count | `count`: _____ | **ALL** | **TRUE** |
| **`SWITCH_LIFE_TIME_CHECK`** | Warns when relay usage exceeds limit.<br><br>**Parameters:**<br>- `count`: Max cycles | `count`: _____ | **ALL** | **TRUE** |
| **`DC_PDM_TRACE_ENABLE`** | Enables DC measurement traces.<br>*(No extra parameters)* | *(None)* | **NFR** | **FALSE** |

---

## Guide for Filling Out This Table

1.  **Function Tag**: The exact name of the configuration node in the XML.
2.  **Description & Allowed Values**: Reference column. Shows what the function does and valid inputs.
3.  **Desired Settings**: **(Action Required)** Write specific values here.
    *   *Example:* `count: 10` or `mode: SWEEP`.
4.  **Applicable Stage**: **(Action Required)** Specify which test stage this applies to:
    *   **RF1** / **RF2** (Radio Frequency)
    *   **NFR** (Noise Figure / DC)
    *   **ALL** (All stages)
5.  **Enable?**: **(Action Required)**
    *   **TRUE**: Activate function.
    *   **FALSE**: Deactivate function.






























































































