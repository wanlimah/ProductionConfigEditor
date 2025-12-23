# XML Structure Guide

## Overall Structure

```
<ProductionUserConfig>                          ← Root element
  <ProductionUserConfigs viewer="false">        ← Main container
    
    <CONFIGURATION_NODE_NAME>                   ← Configuration category
      <Package name="..." enable="..." />       ← Individual package
      <Package name="..." enable="..." />       ← Another package
    </CONFIGURATION_NODE_NAME>
    
    <ANOTHER_CONFIGURATION_NODE>                ← Another category
      <OptionNodeName> VALUE1 | VALUE2 </OptionNodeName>  ← Optional: Predefined options
      <Package name="..." enable="..." attribute="..." />
    </ANOTHER_CONFIGURATION_NODE>
    
  </ProductionUserConfigs>
  
  <PcbFormatConfig>                             ← Other configurations
    ...
  </PcbFormatConfig>
  
  <DeveloperValidationConfig>                   ← Other configurations
    ...
  </DeveloperValidationConfig>
</ProductionUserConfig>
```

---

## Hierarchy Breakdown

### Level 1: Root
```xml
<ProductionUserConfig>
```
- The outermost container for the entire configuration file

### Level 2: Main Container
```xml
<ProductionUserConfigs viewer="false">
```
- Contains all the configuration nodes
- Has a `viewer` attribute (true/false)

### Level 3: Configuration Nodes (Categories)
```xml
<GU_ENGINEERING_MODE_ENABLE>
<SKIP_OUTPUT_PORT_ON_FAILURE>
<STOP_TEST_ON_CONTINUOUS_FAILURE_COUNT_1A>
<DC_PDM_TRACE_ENABLE>
<TTD_FILE_ENABLE>
etc...
```
- These are **feature/configuration categories**
- Each node groups related packages
- Names describe the feature (e.g., "DC_PDM_TRACE_ENABLE" = DC PDM tracing feature)

### Level 4: Package Elements
```xml
<Package name="SUSER" enable="FALSE" />
<Package name="8267-PROD" enable="TRUE" count="5" />
```
- These are the **actual configuration records**
- Each `<Package>` element has **attributes** (name, enable, count, etc.)
- Different packages can have different attributes

---

## Example Structures

### Example 1: Simple Configuration (2 attributes)
```xml
<DC_PDM_TRACE_ENABLE>
  <Package name="SUSER" enable="FALSE"/>
  <Package name="8267-PROD" enable="FALSE"/>
</DC_PDM_TRACE_ENABLE>
```

**Structure:**
- **Configuration Node:** `DC_PDM_TRACE_ENABLE`
- **Package 1:** name="SUSER", enable="FALSE"
- **Package 2:** name="8267-PROD", enable="FALSE"
- **Attributes per package:** 2 (name, enable)

### Example 2: Configuration with More Attributes
```xml
<STOP_TEST_ON_CONTINUOUS_FAILURE_COUNT_1A>
  <Package name="SUSER" enable="FALSE" count="5" />
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" count="5" />
  <!-- Setting Instructions:			
      1. 'count' represents the number of consecutive failures
  -->
</STOP_TEST_ON_CONTINUOUS_FAILURE_COUNT_1A>
```

**Structure:**
- **Configuration Node:** `STOP_TEST_ON_CONTINUOUS_FAILURE_COUNT_1A`
- **Package 1:** name="SUSER", enable="FALSE", count="5"
- **Package 2:** name="AFEM-8266-AP1-RF1-QA", enable="FALSE", count="5"
- **Attributes per package:** 3 (name, enable, count)
- **Comment:** Explains what 'count' means

### Example 3: Configuration with Options
```xml
<INLINE_SORTER_ENABLE>
  <RuleOptions> DATETIME | REV </RuleOptions>
  <Package name="SUSER" enable="FALSE" rule="REV" />
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" rule="REV" />
</INLINE_SORTER_ENABLE>
```

**Structure:**
- **Configuration Node:** `INLINE_SORTER_ENABLE`
- **Options Node:** `<RuleOptions>` defines valid values for `rule` attribute
- **Package 1:** name="SUSER", enable="FALSE", rule="REV"
- **Package 2:** name="AFEM-8266-AP1-RF1-QA", enable="FALSE", rule="REV"
- **Attributes per package:** 3 (name, enable, rule)
- **rule dropdown options:** DATETIME, REV

### Example 4: Complex Configuration
```xml
<ENA_AVERAGE_MODE_ON>
  <ModeOptions> SWEEP | POINT </ModeOptions>
  <AvgChannelOptions> ALL | EACH </AvgChannelOptions>
  <Package name="SUSER" enable="FALSE" count="5" mode="SWEEP" avg_channel="ALL" />
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE" count="5" mode="SWEEP" avg_channel="EACH" />
</ENA_AVERAGE_MODE_ON>
```

**Structure:**
- **Configuration Node:** `ENA_AVERAGE_MODE_ON`
- **Options Nodes:** 
  - `<ModeOptions>` defines valid values for `mode`
  - `<AvgChannelOptions>` defines valid values for `avg_channel`
- **Package 1:** name="SUSER", enable="FALSE", count="5", mode="SWEEP", avg_channel="ALL"
- **Attributes per package:** 5 (name, enable, count, mode, avg_channel)

---

## Key Points to Understand

### 1. Configuration Nodes vs Packages
```
Configuration Node (Category)
    ↓
    Contains one or more Package elements
```

**Example:**
```xml
<DC_PDM_TRACE_ENABLE>           ← Configuration Node (the feature)
  <Package name="SUSER" ... />   ← Package 1 (configuration for SUSER)
  <Package name="8267-PROD" .../>← Package 2 (configuration for 8267-PROD)
</DC_PDM_TRACE_ENABLE>
```

### 2. Attributes vs Elements

**Attributes** (what we edit):
```xml
<Package name="SUSER" enable="FALSE" count="5" />
         ↑               ↑              ↑
      attribute      attribute      attribute
```

**Elements** (containers):
```xml
<DC_PDM_TRACE_ENABLE>           ← Element (container)
  <Package ... />                ← Element (with attributes)
</DC_PDM_TRACE_ENABLE>
```

### 3. Multiple Packages per Configuration Node

One configuration node can have **multiple packages**:
```xml
<DC_PDM_TRACE_ENABLE>
  <Package name="SUSER" enable="FALSE"/>        ← Package for SUSER mode
  <Package name="8267-PROD" enable="FALSE"/>    ← Package for production
  <Package name="TEST-PKG" enable="TRUE"/>      ← Package for testing
</DC_PDM_TRACE_ENABLE>
```

Each package is **independent** and can have different attribute values.

---

## How the Editor Works with This Structure

### Current Behavior (XPath: `//Package`)

The editor uses `//Package` which means:
- **Finds ALL `<Package>` elements** in the entire XML file
- Returns them as a flat list

**Example:**
If the XML has:
```xml
<DC_PDM_TRACE_ENABLE>
  <Package name="SUSER" enable="FALSE"/>
  <Package name="8267-PROD" enable="FALSE"/>
</DC_PDM_TRACE_ENABLE>
<CAP_MEASUREMENT_TRACE_ENABLE>
  <Package name="SUSER" enable="FALSE"/>
  <Package name="AFEM-8266-AP1-RF1-QA" enable="FALSE"/>
</CAP_MEASUREMENT_TRACE_ENABLE>
```

The dropdown will show **4 packages:**
1. DC_PDM_TRACE_ENABLE > SUSER
2. DC_PDM_TRACE_ENABLE > 8267-PROD
3. CAP_MEASUREMENT_TRACE_ENABLE > SUSER
4. CAP_MEASUREMENT_TRACE_ENABLE > AFEM-8266-AP1-RF1-QA

### When You Select a Package

**Example:** You select "DC_PDM_TRACE_ENABLE > SUSER"

The editor shows the attributes for that specific package:
```
name:      SUSER
enable:    FALSE
```

**Example:** You select "STOP_TEST_ON_CONTINUOUS_FAILURE_COUNT_1A > SUSER"

The editor shows:
```
name:      SUSER
enable:    FALSE
count:     5
```

Different packages have different attributes - **that's the dynamic part!**

---

## Common Attribute Types

| Attribute | Description | Type | Example Values |
|-----------|-------------|------|----------------|
| `name` | Package name/identifier | Text | "SUSER", "8267-PROD", "WW-PROD" |
| `enable` | Whether feature is enabled | Dropdown | "TRUE", "FALSE" |
| `count` | Number of items/occurrences | Number | "5", "10", "100" |
| `sampling` | Sampling interval | Number | "5", "10", "20" |
| `threshold` | Threshold value | Number | "99", "95" |
| `mode` | Operating mode | Dropdown | "AUTO", "MANUAL", "SWEEP", "POINT" |
| `rule` | Rule type | Dropdown | "DATETIME", "REV" |
| `avg_channel` | Average channel setting | Dropdown | "ALL", "EACH" |
| `url` | Web service URL | Text | "http://example.com" |
| `note` | Description/note | Text | "Please use AFEM-8266-AP1-QA..." |

---

## Questions to Clarify

Please help me understand your needs by answering these questions:

### Question 1: Package Selection
When you select a package from the dropdown, what do you want to see?

**Option A:** Show that specific package's attributes
```
Select: DC_PDM_TRACE_ENABLE > SUSER
Show:   name: SUSER
        enable: FALSE
```

**Option B:** Show all packages under that configuration node
```
Select: DC_PDM_TRACE_ENABLE
Show:   Package 1: name=SUSER, enable=FALSE
        Package 2: name=8267-PROD, enable=FALSE
```

### Question 2: Editing Multiple Packages
Do you want to:

**Option A:** Edit one package at a time (current behavior)

**Option B:** Edit multiple packages under the same configuration node at once

### Question 3: Package Display in Dropdown
How should packages be displayed in the dropdown?

**Current:** `DC_PDM_TRACE_ENABLE > SUSER` (shows parent node and package name)

**Alternative 1:** Just `SUSER` (package name only)

**Alternative 2:** `DC_PDM_TRACE_ENABLE: SUSER (enable=FALSE)` (with attribute preview)

### Question 4: Your Use Case
What is your typical workflow?

**Example 1:** "I need to enable DC_PDM_TRACE for production package 8267-PROD"

**Example 2:** "I need to configure sampling settings for multiple features"

**Example 3:** "I need to add new production packages to multiple configuration nodes"

---

Please let me know:
1. ✅ Is the XML structure clear now?
2. ❓ Which options above match your needs?
3. ❓ What changes would make the editor more useful for you?































































