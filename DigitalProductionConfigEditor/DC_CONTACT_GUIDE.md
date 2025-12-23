## DC Contact Open/Short (O/S) Test Configuration

**Function Tag 1:** `<DC_CONTACT_MODE_ENABLE>` (The Switch)  
**Function Tag 2:** `<DC_CONTACT_MODE_SETTING_OVERWRITE>` (The Settings)  
**Objective:** Customize the electrical parameters for Open/Short testing on MIPI and DC pins prior to RF testing.  
**Applicable Test Stage:** RF1 / RF2 / NFR

### 1. Enable/Disable (The Switch)
Before configuring parameters, ensure the feature is turned on for your package.

**Tag:** `<DC_CONTACT_MODE_ENABLE>`

| Package | Enable | Note |
| :--- | :--- | :--- |
| **SUSER** | `TRUE` | Enable for local engineering debug |
| **Production (e.g., 8288-AP1)** | `FALSE` | Typically disabled in production unless debugging |

---

### 2. Test Methodology
*(Applies if Enabled above)*

*   **MIPI Pins (VIO, SCLK, SDATA)**:
    *   **Method:** Force Current, Measure Voltage (`FORCEI`).
    *   **Goal:** Detect Open and Short circuits.
*   **DC Pins (Vcc, Vbatt, Vdd)**:
    *   **Method:** Force Voltage, Measure Current (`FORCEV`).
    *   **Goal:** Detect Short circuits only.

> **⚠️ Note:** If a **SHORT** event is detected, the code will immediately turn OFF all DC supplies to prevent damage. An **OPEN** event will allow testing to proceed for data collection.

### 3. Configuration Parameters (Blue Box Settings)
**Tag:** `<DC_CONTACT_MODE_SETTING_OVERWRITE>`

To match the standard "Vettel NFR" methodology, use the following settings:

#### **A. MIPI Pin Settings (Open/Short)**
Apply these settings to pins like **Vio1**, **SCLK**, **SDATA**.

| Parameter (XML) | Value | Description |
| :--- | :--- | :--- |
| `TestMode` | **FORCEI** | Force Current mode |
| `ISource` | **1.5e-6** | Force 1.5 µA (1.5e-6 Amps) |
| `VSetHi` (HighLimit) | **1.2** | Max expected voltage (Open circuit) |
| `VSetLo` (LowLimit) | **-0.3** | Min expected voltage |
| `ILevel` | **2e-6** | Current compliance level |

#### **B. DC Pin Settings (Short Only)**
Apply these settings to pins like **Vdd**, **Vbatt**, **Vcc**.

| Parameter (XML) | Value | Description |
| :--- | :--- | :--- |
| `TestMode` | **FORCEV** | Force Voltage mode |
| `VSet` | **0.1** | Force 100mV (0.1 V) |
| `ISet` | **10e-6** | Max current limit (10 µA) |
| `HighLimit` | **9.5e-6** | Fail threshold (Short if current > 9.5µA) |

### 4. XML Example (Paired)

```xml
<!-- STEP 1: Turn it ON -->
<DC_CONTACT_MODE_ENABLE>
  <Package name="SUSER" enable="TRUE" />
  <Package name="ENGR-8288-AP1-NF" enable="FALSE" />
</DC_CONTACT_MODE_ENABLE>

<!-- STEP 2: Configure it -->
<DC_CONTACT_MODE_SETTING_OVERWRITE>
  <Pin Name="Vdd" VSet="0.1" ISet="10e-6" HighLimit="9.5e-6" TestMode="FORCEV"/>
  <Pin Name="Vio1" VSetHi="1.2" VSetLo="-0.3" ISource="1.5e-6" ILevel="2e-6" TestMode="FORCEI"/>
  <Delayms Value="80"/>
</DC_CONTACT_MODE_SETTING_OVERWRITE>
```

### 5. Expected Results (Reference)

| Parameter | Expected Value (Normal) | Fault Condition |
| :--- | :--- | :--- |
| **M_MeasV-VIO1** | ~0.3V to 0.55V | **Open:** ~1.2V / **Short:** ~0V |
| **M_MeasV-SCLK/SDATA** | ~0.3V to 0.7V | **Open:** ~1.2V / **Short:** ~0V |
| **M_MeasI-Vbatt/Vdd** | Near 0 (Negative OK) | **Short:** > 10µA |
