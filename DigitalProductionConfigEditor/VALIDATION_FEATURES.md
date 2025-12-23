# Validation Features Guide

## ✅ New Input Validation Features

### 1. Auto-Convert Package Names to UPPERCASE

**Feature**: All product part numbers are automatically converted to uppercase as you type.

**Applies To**:
- Package Name field in Step 2 (when editing)
- Package Name field in Add Package Dialog

**How It Works**:
```
User types:  ww-prod
Displayed:   WW-PROD

User types:  afem-8266-ap1-rf1-qa
Displayed:   AFEM-8266-AP1-RF1-QA
```

**Benefit**: 
- Ensures consistency across all package names
- Prevents case-sensitivity issues
- No need to hold Shift key!

---

### 2. Integer-Only Validation for Numeric Fields

**Feature**: Specific attributes only accept integer numbers (no letters, no punctuation).

**Validated Attributes**:
- `count`
- `sampling`
- `threshold`
- `x`
- `y`

**How It Works**:
```
✅ Allowed:  123, 456, 0, 9999
❌ Blocked:  abc, 12.5, @#$, -5, 3.14
```

**User Experience**:
- When user tries to type a letter in these fields → Nothing happens (blocked)
- When user tries to type punctuation → Nothing happens (blocked)
- Only digits 0-9 are accepted

**Benefit**:
- Prevents accidental typos
- No error messages needed - just blocks invalid input
- Clean user experience

---

## 📝 Where Validation Applies

### Step 2: Edit Packages

```
┌────────────────────────────────────────────────────────┐
│ 📝 Editing Package Attributes                          │
├────────────────────────────────────────────────────────┤
│                                                         │
│ Package Name:  [WW-PROD________________] ← UPPERCASE   │
│                                                         │
│ count:         [123____] ← Integers only                │
│ sampling:      [456____] ← Integers only                │
│ threshold:     [789____] ← Integers only                │
│ enable:        [TRUE___] ← Any text OK                  │
│ mode:          [AUTO___] ← Any text OK                  │
│                                                         │
└────────────────────────────────────────────────────────┘
```

### Add Package Dialog

```
┌────────────────────────────────────────────────────────┐
│ Add New Package to Configuration                       │
├────────────────────────────────────────────────────────┤
│                                                         │
│ Package Name*: [AFEM-8266-AP1-RF1-QA] ← UPPERCASE      │
│                                                         │
│ ☐ Add Power Suffix                                     │
│   Result: AFEM-8266-AP1-RF1-QA                         │
│                                                         │
│ Attributes:                                            │
│ count:         [100____] ← Integers only                │
│ sampling:      [5______] ← Integers only                │
│ enable:        [TRUE___] ← Any text OK                  │
│                                                         │
└────────────────────────────────────────────────────────┘
```

---

## 🎯 Technical Implementation

### UPPERCASE Conversion

**XAML Property Used**: `CharacterCasing="Upper"`

```xml
<TextBox x:Name="PackageNameTextBox"
         Text="{Binding SelectedPackageName, Mode=TwoWay}" 
         CharacterCasing="Upper"/>
```

**How it works**:
- Built-in WPF feature
- Converts characters to uppercase as user types
- Automatic, no code needed

---

### Integer Validation

**Method Used**: `PreviewTextInput` event handler with Regex

**Code**:
```csharp
private void NumericAttribute_PreviewTextInput(object sender, TextCompositionEventArgs e)
{
    if (sender is TextBox textBox && textBox.Tag is string attributeName)
    {
        // List of attributes that should only accept integers
        var numericFields = new[] { "count", "sampling", "threshold", "x", "y" };
        
        if (numericFields.Contains(attributeName.ToLower()))
        {
            // Only allow digits (integers only)
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
```

**How it works**:
1. Before each character is entered, the event fires
2. Check if the field is a numeric field (count, sampling, threshold, x, y)
3. If yes, use regex to check if input is a digit
4. If NOT a digit, set `e.Handled = true` to block it
5. If it IS a digit, allow it

---

## 🎨 User Experience

### Scenario 1: Entering Package Name

**User Action**: Types "ww-prod" (lowercase)

**What Happens**:
1. User types: w → Displayed: W
2. User types: w → Displayed: WW
3. User types: - → Displayed: WW-
4. User types: p → Displayed: WW-P
5. Final result: WW-PROD

**User feels**: "Wow, it auto-capitalizes for me!"

---

### Scenario 2: Entering Count (Numeric Field)

**User Action**: Tries to type "abc123"

**What Happens**:
1. User types: a → Nothing happens (blocked)
2. User types: b → Nothing happens (blocked)
3. User types: c → Nothing happens (blocked)
4. User types: 1 → Displayed: 1
5. User types: 2 → Displayed: 12
6. User types: 3 → Displayed: 123

**User feels**: "Oh, I can only enter numbers here. That makes sense!"

---

### Scenario 3: Entering Enable (Text Field)

**User Action**: Types "TRUE"

**What Happens**:
1. User types: T → Displayed: T
2. User types: R → Displayed: TR
3. User types: U → Displayed: TRU
4. User types: E → Displayed: TRUE

**No validation** - accepts any text (TRUE, FALSE, true, false, etc.)

---

## 💡 Tips for Users

### Package Names
✅ **Do**: Just type normally (lowercase is fine)
```
Type: ww-prod
Result: WW-PROD ✓
```

❌ **Don't**: Worry about pressing Shift/Caps Lock
```
No need to hold Shift! It auto-converts for you!
```

---

### Numeric Fields (count, sampling, threshold, x, y)
✅ **Do**: Type numbers only
```
count: 100 ✓
sampling: 5 ✓
threshold: 99 ✓
```

❌ **Don't**: Try to enter decimals, negatives, or text
```
count: 12.5 ✗ (will appear as: 125)
count: -10 ✗ (will appear as: 10)
count: abc ✗ (nothing will appear)
```

---

### Text Fields (enable, mode, url, note, etc.)
✅ **Do**: Type anything you need
```
enable: TRUE ✓
mode: AUTO ✓
url: http://example.com ✓
note: This is a test note ✓
```

No restrictions on these fields!

---

## 🔧 Validated Fields Reference

| Attribute Name | Type | Validation | Allowed Values |
|----------------|------|------------|----------------|
| **name** (Package Name) | Text | Auto-UPPERCASE | Any characters |
| **count** | Integer | Digits only | 0-9 only |
| **sampling** | Integer | Digits only | 0-9 only |
| **threshold** | Integer | Digits only | 0-9 only |
| **x** | Integer | Digits only | 0-9 only |
| **y** | Integer | Digits only | 0-9 only |
| **enable** | Text | None | Any text (usually TRUE/FALSE) |
| **mode** | Text | None | Any text (usually AUTO/MANUAL) |
| **rule** | Text | None | Any text |
| **avg_channel** | Text | None | Any text (usually ALL/EACH) |
| **url** | Text | None | Any text |
| **note** | Text | None | Any text |

---

## 🆘 Common Questions

**Q: What if I need to enter a decimal number like 12.5 in count?**
A: The count, sampling, and threshold fields are designed for integers only. If you need decimals, discuss with your system administrator about adding a new field type.

**Q: Can I paste text into numeric fields?**
A: Yes, but only the numeric characters will be accepted. For example, pasting "abc123xyz" will result in "123".

**Q: Why does my package name turn into uppercase automatically?**
A: This is intentional! It ensures all package names are consistent and prevents case-sensitivity issues in the system.

**Q: What if I want a lowercase package name?**
A: Package names must be uppercase for consistency with the production system. This is a requirement, not a bug!

---

## 📊 Benefits Summary

| Feature | Before | After |
|---------|--------|-------|
| **Package Names** | Mixed case (ww-prod, WW-PROD, Ww-Prod) | Consistent (WW-PROD) |
| **Numeric Fields** | Users could type "abc", get error | Blocked at input, no errors |
| **User Experience** | Validation errors after submit | Instant feedback while typing |
| **Data Quality** | Mixed, inconsistent | Clean, consistent |

---

## 🎉 Summary

**Two simple features, huge impact**:

1. ✅ **Auto-UPPERCASE** for package names → Consistency
2. ✅ **Integer-only** for numeric fields → Data quality

**Result**: 
- Fewer errors
- Better user experience  
- Cleaner data
- No error messages needed!

---

**Version**: 2.1 - Validation Features
**Last Updated**: October 2025

























































