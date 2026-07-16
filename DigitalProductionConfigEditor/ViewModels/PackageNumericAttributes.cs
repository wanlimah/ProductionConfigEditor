using System.Collections.Generic;

namespace DigitalProductionConfigEditor.ViewModels
{
    /// <summary>
    /// Package XML attributes that must be non-negative integers (digits only; no decimals or minus).
    /// </summary>
    public static class PackageNumericAttributes
    {
        private static readonly HashSet<string> Keys = new(StringComparer.OrdinalIgnoreCase)
        {
            "count",
            "sampling",
            "threshold",
            "x",
            "y",
            "minUnitsBeforeTrigger",
            "minPassedYieldTrigger"
        };

        public static bool IsNonNegativeInteger(string? attributeName) =>
            attributeName != null && Keys.Contains(attributeName);
    }
}
