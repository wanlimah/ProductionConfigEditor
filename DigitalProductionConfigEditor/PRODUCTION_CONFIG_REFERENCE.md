# 📘 Production Configuration Reference

**File:** `Master_ProductionUserConfig.xml`  
**Purpose:** Defines test limits, hardware behaviors, and yield controls for the production floor.

---

## 1. Common Attributes
Every feature contains "Packages". These attributes apply to almost all features:

| Attribute | Description |
| :--- | :--- |
| **`name`** | The specific product or test package (e.g., `WW-PROD`, `AFEM-8266`). <br> **`SUSER`** usually refers to Engineering/Debug mode. |
| **`enable`** | `TRUE` = Feature is active.<br>`FALSE` = Feature is ignored. |
| **`count`** | A numeric threshold (e.g., max failures allowed, or number of units to test). |

### Package Naming Legend (Test Stages)
Use these identifiers in the **`name`** field to target specific test stages:

| Test Stage Identifier | Meaning | Usage Example |
| :--- | :--- | :--- |
| **`RF1`** | **Radio Frequency 1** | `AFEM-8266-AP1-RF1-QA` |
| **`RF2`** | **Radio Frequency 2** | `AFEM-8266-AP1-RF2-QA` |
| **`NFR`** / **`NF`** | **Noise Figure** / **Non-RF** | `ENGR-8288-AP1-NF` |
| **`PROD`** | **Production** (General) | `WW-PROD` |

---

## 2. Test Flow & Yield Control
These settings control when the test program should stop or pause based on failures.

| Function Tag | Description | Supported Stage |
| :--- | :--- | :--- |
| **`STOP_TEST_ON_...`** | Stops test on consecutive failures. | **ALL** |
| **`HANDLER_ARM_...`** | Checks yield difference between handler arms. | **ALL** |
| **`PARAM_COUNT_CHECK`** | Verifies result file parameter count. | **ALL** |
| **`SKIP_OUTPUT_PORT_...`** | Skips binning logic on failure. | **ALL** |

---

## 3. Data Logging & Tracing
These settings control what debug files are generated. Note that some traces are specific to certain hardware (Network Analyzer vs. DC Meter).

| Function Tag | Description | Supported Stage |
| :--- | :--- | :--- |
| **`TTD_FILE_ENABLE`** | Generates detailed debug logs. | **ALL** |
| **`ENA_SNP_FILE_ENABLE`** | Saves S-Parameter files from Network Analyzer. | **RF1, RF2** |
| **`ENA_AVERAGE_MODE`** | Controls Network Analyzer averaging. | **RF1, RF2** |
| **`DC_PDM_TRACE_ENABLE`** | Traces for DC measurements. | **NFR** |
| **`CAP_MEASUREMENT_...`** | Traces for Capacitor measurements. | **NFR** |
| **`RF_TIMING_TEST_...`** | Debugs RF timing. | **RF1, RF2** |

---

## 4. Hardware & Equipment Protection

| Function Tag | Description | Supported Stage |
| :--- | :--- | :--- |
| **`SWITCH_LIFE_TIME...`** | Warnings for relay cycle counts. | **ALL** |
| **`DC_CONTACT_MODE...`** | Overrides DC continuity settings. | **NFR** |

---

## 5. External Systems

| Function Tag | Description | Supported Stage |
| :--- | :--- | :--- |
| **`MQTT_ENABLE`** | Factory automation messaging. | **ALL** |
| **`WEB_SERVICE_ENABLE`** | External API calls. | **ALL** |
| **`KEYENCE_2DID_ENABLE`** | 2D Barcode reading. | **ALL** |
