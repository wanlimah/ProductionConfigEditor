using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using DigitalProductionConfigEditor.ViewModels;
using DigitalProductionConfigEditor.Views;
using System.Linq;

namespace DigitalProductionConfigEditor
{
    public partial class MainWindow : Window
    {
        private WizardViewModel viewModel = new();
        private FileSystemWatcher? _masterFileWatcher;
        private DateTime _lastReloadTime = DateTime.MinValue;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            
            // Check for updates from Box immediately on startup
            CheckForBoxUpdate();
            
            // Load Master XML with error handling
            try
            {
                string xmlPath = FindXmlFile();
                viewModel.LoadMasterXml(xmlPath);
                SetupFileWatcher(xmlPath);
                
                // Try to auto-load ProductionUserConfig.xml from Documents folder
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string defaultConfigPath = Path.Combine(documentsPath, "ProductionUserConfig.xml");
                
                if (File.Exists(defaultConfigPath))
                {
                    // Auto-load existing file
                    try
                    {
                        viewModel.LoadNewXml(defaultConfigPath);
                        viewModel.StatusMessage = $"✅ Auto-loaded: {defaultConfigPath}";
                        ShowConfigurationLoadedDialog(defaultConfigPath);
                    }
                    catch (Exception ex)
                    {
                        // If auto-load fails, fall back to blank XML
                        viewModel.CreateNewBlankXml();
                        MessageBox.Show(
                            $"⚠️ Could not auto-load {defaultConfigPath}\n\n{ex.Message}\n\n" +
                            "Starting with blank configuration instead.",
                            "Auto-Load Failed",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    // No existing file - create blank XML
                    viewModel.CreateNewBlankXml();
                    viewModel.StatusMessage = "ℹ️ No existing config found. Starting with blank XML.";
                }
                
                LoadStep();
                
                // Show welcome guide on first launch
                ShowWelcomeGuide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load Master XML file: {ex.Message}\n\nCurrent directory: {Environment.CurrentDirectory}\nApplication directory: {AppDomain.CurrentDomain.BaseDirectory}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetupFileWatcher(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (_masterFileWatcher != null)
                {
                    _masterFileWatcher.Dispose();
                    _masterFileWatcher = null;
                }

                string? directory = Path.GetDirectoryName(path);
                string? filename = Path.GetFileName(path);

                if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(filename)) return;

                _masterFileWatcher = new FileSystemWatcher
                {
                    Path = directory,
                    Filter = filename,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime
                };

                _masterFileWatcher.Changed += OnMasterFileChanged;
                _masterFileWatcher.Created += OnMasterFileChanged; // In case it was replaced
                _masterFileWatcher.EnableRaisingEvents = true;
                
                System.Diagnostics.Debug.WriteLine($"Watching file for changes: {path}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to setup file watcher: {ex.Message}");
            }
        }

        private bool _isInternalOperation = false;

        private void OnMasterFileChanged(object sender, FileSystemEventArgs e)
        {
            // Debounce: Avoid multiple reloads for single save (Notepad++ does this often)
            if ((DateTime.Now - _lastReloadTime).TotalSeconds < 2) return;
            
            // Ignore changes that happened while we were performing an internal operation
            if (_isInternalOperation) return;

            _lastReloadTime = DateTime.Now;

            // Dispatch to UI Thread
            Dispatcher.Invoke(() =>
            {
                // Only care if we are in normal viewing mode or Edit Master mode
                // If we are in "Edit Master", we expect changes from Notepad++
                
                try
                {
                    // Wait briefly for file lock to release
                    System.Threading.Thread.Sleep(200); 
                    
                    if (viewModel.IsDirty)
                    {
                        // Conflict!
                        var result = MessageBox.Show(
                            "⚠️ Master XML file has been modified externally (Notepad++).\n\n" +
                            "However, you have unsaved changes in this App.\n\n" +
                            "Reloading now will DISCARD your unsaved app changes.\n\n" +
                            "Click OK to Reload (Lose App Changes).\n" +
                            "Click Cancel to Keep App Changes (Ignore File Change).",
                            "External Change Detected",
                            MessageBoxButton.OKCancel,
                            MessageBoxImage.Exclamation);

                        if (result == MessageBoxResult.OK)
                        {
                            PerformAutoReload();
                        }
                    }
                    else
                    {
                        // Safe to reload automatically
                        PerformAutoReload(silent: false); // Show small popup/status or silent
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Auto-reload failed: {ex.Message}");
                }
            });
        }

        private void PerformAutoReload(bool silent = false)
        {
            _isInternalOperation = true; // Start internal op
            try
            {
                string xmlPath = viewModel.MasterXmlPath;
                if (string.IsNullOrEmpty(xmlPath)) xmlPath = FindXmlFile();

                // Reload Master XML
                viewModel.LoadMasterXml(xmlPath);

                // If Editing Master, reload edit buffer too
                if (viewModel.IsEditingMaster)
                {
                    viewModel.LoadNewXml(xmlPath);
                    viewModel.IsEditingMaster = true; // Restore flag
                    viewModel.SetMasterReadOnly(false); // Keep unlocked
                }

                LoadStep();
                
                if (!silent)
                {
                    // Option 1: Non-intrusive status
                    viewModel.StatusMessage = "🔄 Master XML updated from local file";
                    
                    // Option 2: MessageBox (As requested "OK" button show)
                    MessageBox.Show("Master XML updated from local file.", "Reloaded", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                if (!silent)
                    MessageBox.Show($"Failed to auto-reload: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Add a small buffer time after operation before listening again
                // This prevents the watcher from picking up our own file unlocking/saving immediately
                System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ => _isInternalOperation = false);
            }
        }

        private void ShowWelcomeGuide(bool forceShow = false)
        {
            // Check settings first
            var settings = UserSettings.Load();
            
            // Only show if user hasn't opted out, unless forced (e.g. by Help button)
            if (!forceShow && !settings.ShowPcbGuide)
            {
                return;
            }

            string welcomeMessage = 
                "🎉 Welcome to Production Config Editor!\n\n" +
                
                "📋 QUICK START GUIDE\n" +
                "────────────────────\n\n" +
                
                "1️⃣  STEP 1: Build Your Configuration\n" +
                "   • Browse the Master Template (left panel)\n" +
                "   • Click ➕ Add to copy configurations you need\n" +
                "   • Click 🗑 Delete to remove unwanted ones\n" +
                "   • Must add at least 1 configuration\n\n" +
                
                "2️⃣  STEP 2: Manage Packages\n" +
                "   • Select a configuration from dropdown\n" +
                "   • ➕ Add New Package to create packages\n" +
                "   • Edit or Delete existing packages\n\n" +
                
                "3️⃣  STEP 3: Review & Save\n" +
                "   • Review your configuration summary\n" +
                "   • Click 💾 Save to save your custom XML\n\n" +
                
                "💡 KEY CONCEPTS\n" +
                "────────────────────\n" +
                "• Master Template = Read-only library (never modified)\n" +
                "• Your Config = Custom XML you're building\n" +
                "• Configuration = Top-level category\n" +
                "• Package = Product-specific settings\n\n" +
                
                "🔘 NAVIGATION BUTTONS\n" +
                "────────────────────\n" +
                "❓ Help - Show this guide anytime\n" +
                "🏠 Return to Start - Go back to Step 1\n" +
                "📄 New - Create blank configuration\n" +
                "📂 Open - Load different file\n" +
                "🔄 Reload Master - Refresh template\n" +
                "⬅ Back / Next ➡ - Navigate steps\n" +
                "💾 Save / Save As - Save your work\n\n" +
                
                "⚠️  IMPORTANT NOTES\n" +
                "────────────────────\n" +
                "• App auto-loads Documents\\ProductionUserConfig.xml if exists\n" +
                "• Changes are NOT auto-saved - always click 💾 Save\n" +
                "• Master XML is protected and never modified\n" +
                "• Use 📂 Open to load a different configuration file\n\n" +
                
                "Ready to create your configuration? Click OK to begin!\n\n" +
                
                "💡 TIP: Click ❓ Help button anytime for instructions!";

            // Create a custom dialog to include the checkbox
            var guideDialog = new Window
            {
                Title = "🚀 Getting Started Guide",
                Width = 600,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanResize,
                Icon = this.Icon
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Padding = new Thickness(20)
            };

            var textBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                Text = welcomeMessage
            };

            scrollViewer.Content = textBlock;

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 10)
            };

            var dontShowCheck = new CheckBox
            {
                Content = "Don't show this again",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 20, 0)
            };

            if (forceShow)
            {
                dontShowCheck.Visibility = Visibility.Collapsed;
            }

            var okButton = new Button
            {
                Content = "OK",
                Padding = new Thickness(20, 10, 20, 10),
                IsDefault = true,
                Margin = new Thickness(5, 0, 0, 0)
            };
            
            // Use a local flag to track dialog result
            bool showNextGuide = true;

            okButton.Click += (s, e) => 
            {
                if (dontShowCheck.IsChecked == true)
                {
                    settings.ShowPcbGuide = false;
                    settings.Save();
                    showNextGuide = false; // User opted out
                }
                guideDialog.Close();
            };
            
            if (!forceShow)
            {
                // In Startup mode, show "Don't show again"
                buttonPanel.Children.Add(dontShowCheck);
            }
            
            buttonPanel.Children.Add(okButton);

            var mainPanel = new DockPanel();
            DockPanel.SetDock(buttonPanel, Dock.Bottom);
            mainPanel.Children.Add(buttonPanel);
            mainPanel.Children.Add(scrollViewer);

            guideDialog.Content = mainPanel;
            guideDialog.ShowDialog();
            
            // Show PCB guide if user didn't opt out
            if (showNextGuide && settings.ShowPcbGuide) 
            {
                 ShowPcbSpecificGuide();
            }
        }

        private void ShowPcbSpecificGuide()
        {
            // Create a custom scrollable dialog for long content
            var pcbDialog = new Window
            {
                Title = "🔲 PCB Configuration - Special Guide",
                Width = 650,
                Height = 550,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanResize,
                Icon = this.Icon
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Padding = new Thickness(20)
            };

            var textBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 12,
                Text = 
                    "🔲 SPECIAL GUIDE: PCB Format Configuration\n\n" +
                    
                    "✅ KEY CONCEPTS:\n" +
                    "   1. Add PCB Format Config ONCE (only first time)\n" +
                    "   2. Click ✏ Edit button to open the PCB dialog\n" +
                    "   3. Add/Edit/Delete ISLANDS inside the existing PCB\n" +
                    "   4. Each product = Different Island (not different PCB)\n\n" +
                    
                    "📋 TWO WORKFLOWS:\n" +
                    "────────────────────────────────────\n\n" +
                    
                    "🆕 WORKFLOW 1: ADDING PCB (First Time Only)\n" +
                    "   Step 1: Scroll to \"🔲 PCB Format Config\" section\n" +
                    "   Step 2: Click \"➕ Add PCB Format Config\"\n" +
                    "   Step 3: PCB appears in right panel\n" +
                    "   ⚠️  Do this ONLY ONCE per XML file!\n\n" +
                    
                    "✏️  WORKFLOW 2: EDITING EXISTING PCB (Most Common)\n" +
                    "   Step 1: Find \"PCB Format Config\" in right panel\n" +
                    "   Step 2: Click \"✏ Edit\" button\n" +
                    "   Step 3: In the dialog:\n" +
                    "      • Click \"➕ Add Island\" for new product\n" +
                    "      • Edit existing island values\n" +
                    "      • Click \"🗑 Delete\" to remove islands\n" +
                    "   Step 4: Click \"💾 Save\" IN THE DIALOG\n" +
                    "   Step 5: Click \"💾 Save XML\" IN MAIN WINDOW\n\n" +
                    
                    "⚠️  IMPORTANT - TWO SAVES REQUIRED:\n" +
                    "────────────────────────────────────\n" +
                    "1️⃣  Save in Edit Dialog = Apply island changes\n" +
                    "2️⃣  Save XML (main window) = Save to file\n\n" +
                    
                    "Without Step 5, changes stay in memory only!\n" +
                    "You MUST click \"💾 Save XML\" to write to file.\n\n" +
                    
                    "🏝️ UNDERSTANDING ISLANDS\n" +
                    "────────────────────────────────────\n" +
                    "• 1 PCB Config = Multiple Islands\n" +
                    "• 1 Island = 1 Product/PCB variant\n" +
                    "• Island has: ID, Strip Unit (X,Y), Panel Strip (X,Y)\n\n" +
                    
                    "📝 EXAMPLE SCENARIO:\n" +
                    "────────────────────────────────────\n" +
                    "You have 3 different products:\n" +
                    "   Product A → Island 1 (50x17, 2x4)\n" +
                    "   Product B → Island 2 (52x17, 2x4)\n" +
                    "   Product C → Island 3 (48x16, 3x4)\n\n" +
                    
                    "All in ONE PCB Config, not three separate configs!\n\n" +
                    
                    "💡 COMPLETE WORKFLOW EXAMPLE:\n" +
                    "────────────────────────────────────\n" +
                    "1. Add PCB Format Config (if new XML)\n" +
                    "2. Click \"✏ Edit\" button\n" +
                    "3. Add Island 1 for Product A\n" +
                    "4. Add Island 2 for Product B\n" +
                    "5. Click \"💾 Save\" in dialog ← Apply changes\n" +
                    "6. Click \"💾 Save XML\" in main window ← Save to file!\n" +
                    "7. Done! Your XML file now has PCB with 2 islands.\n\n" +
                    
                    "💡 REMEMBER:\n" +
                    "────────────────────────────────────\n" +
                    "• Add PCB Config = ONCE per XML file\n" +
                    "• Edit PCB Config = Add islands inside\n" +
                    "• Different products = Different islands\n" +
                    "• Same PCB section for all products!\n" +
                    "• Always click \"💾 Save XML\" to save to file!\n\n" +
                    
                    "Click Ok to continue..."
            };

            scrollViewer.Content = textBlock;

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10)
            };

            var okButton = new Button
            {
                Content = "Ok",
                Padding = new Thickness(20, 10, 20, 10),
                IsDefault = true
            };
            okButton.Click += (s, e) => 
            {
                pcbDialog.Close();
                // Show Advanced Settings Guide after PCB guide closes
                ShowAdvancedSettingsGuide();
            };
            buttonPanel.Children.Add(okButton);

            var mainPanel = new DockPanel();
            DockPanel.SetDock(buttonPanel, Dock.Bottom);
            mainPanel.Children.Add(buttonPanel);
            mainPanel.Children.Add(scrollViewer);

            pcbDialog.Content = mainPanel;
            pcbDialog.ShowDialog();
        }

        private void ShowAdvancedSettingsGuide()
        {
            // Create a custom scrollable dialog for Advanced Settings guide
            var advDialog = new Window
            {
                Title = "⚙️ Advanced Settings - Quick Guide",
                Width = 650,
                Height = 550,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanResize,
                Icon = this.Icon
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Padding = new Thickness(20)
            };

            var textBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                Text = 
                    "⚙️ ADVANCED SETTINGS GUIDE\n\n" +

                    "📌 WHAT IS THIS?\n" +
                    "Advanced Settings handles special XML configurations that don't follow the standard Package structure. " +
                    "These global settings (applying to all products) are automatically detected from the Master template.\n\n" +
                    
                    "🔍 WHAT YOU CAN DO\n" +
                    "• View and edit global configurations (e.g., hardware settings, pin definitions)\n" +
                    "• Modify custom XML structures without editing raw files\n\n" +
                    
                    "📝 HOW TO USE\n" +
                    "1. Open: Scroll to \"⚙️ Advanced Settings\" in Step 1 and click Open.\n" +
                    "2. Select: Click an item in the left tree. (📁 = Group, 📄 = Setting)\n" +
                    "3. Edit: The right panel displays editable fields (Text, Numbers, Dropdowns).\n" +
                    "4. Save: Click the \"💾 Save Changes\" button inside the dialog.\n\n" +
                    
                    "💾 IMPORTANT SAVING INSTRUCTIONS\n" +
                    "• \"Save Changes\" in the dialog only updates the application's memory.\n" +
                    "• You MUST click the main \"💾 Save\" button in the main window to write your changes to the XML file.\n\n" +

                    "✅ DYNAMIC UPDATES\n" +
                    "When new settings are added to the Master Template, they automatically appear here without requiring an application update."
            };

            scrollViewer.Content = textBlock;

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 15, 0, 10)
            };

            var okButton = new Button
            {
                Content = "Got it!",
                Padding = new Thickness(30, 10, 30, 10),
                IsDefault = true,
                Background = System.Windows.Media.Brushes.LightBlue,
                FontWeight = FontWeights.SemiBold
            };
            okButton.Click += (s, e) => advDialog.Close();
            buttonPanel.Children.Add(okButton);

            var mainPanel = new DockPanel();
            DockPanel.SetDock(buttonPanel, Dock.Bottom);
            mainPanel.Children.Add(buttonPanel);
            mainPanel.Children.Add(scrollViewer);

            advDialog.Content = mainPanel;
            advDialog.ShowDialog();
        }

        private void ShowFunctionGuide()
        {
            var guideDialog = new Window
            {
                Title = "📖 Function Guide",
                Width = 800,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanResize,
                Icon = this.Icon
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Padding = new Thickness(20)
            };

            var textBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                FontFamily = new System.Windows.Media.FontFamily("Consolas, Segoe UI, Courier New")
            };

            // Use Run for bold formatting where appropriate
            textBlock.Inlines.Add(new Run("📖 FUNCTION GUIDE\n") { FontWeight = FontWeights.Bold, FontSize = 16 });
            textBlock.Inlines.Add("──────────────────────────────────────────────────\n\n");

            // Added "Under Construction" notice
            textBlock.Inlines.Add(new Run("🚧 UNDER CONSTRUCTION 🚧\n") { FontWeight = FontWeights.Bold, Foreground = System.Windows.Media.Brushes.Red, FontSize = 14 });
            textBlock.Inlines.Add("The content below is a draft and subject to change.\n\n");
            textBlock.Inlines.Add("──────────────────────────────────────────────────\n\n");

            /* 
             * CONTENT COMMENTED OUT AS REQUESTED
             * 
            textBlock.Inlines.Add(new Run("📋 GUIDE FOR FILLING OUT THIS TABLE (TDG)\n\n") { FontWeight = FontWeights.Bold, Foreground = System.Windows.Media.Brushes.DarkBlue });
            
            textBlock.Inlines.Add(new Run("1. Function Tag: ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("The exact name of the configuration node in the XML (do not change this).\n");
            
            textBlock.Inlines.Add(new Run("2. Description & Allowed Values: ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("This is a Reference column. It explains what the function does and what inputs are valid.\n");
            
            textBlock.Inlines.Add(new Run("3. Desired Settings (Action Required): ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Write specific values here.\n   • Example: 'count: 10' or 'mode: SWEEP'.\n");
            
            textBlock.Inlines.Add(new Run("4. Applicable Stage (Action Required): ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Specify which test stage this applies to:\n   • RF1 / RF2 / NFR\n");
            
            textBlock.Inlines.Add(new Run("5. Enable (Action Required): ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("\n   • TRUE: Activate function.\n   • FALSE: Deactivate function.\n\n");

            textBlock.Inlines.Add("──────────────────────────────────────────────────\n\n");

            textBlock.Inlines.Add(new Run("🔹 1. COMMON ATTRIBUTES\n") { FontWeight = FontWeights.Bold, Foreground = System.Windows.Media.Brushes.Blue });
            textBlock.Inlines.Add("Every configuration block contains <Package> elements. These are standard attributes:\n\n");
            
            textBlock.Inlines.Add(new Run("   Attribute  | Value        | Description\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("   name       | String       | \"SUSER\": Local / Engineering Mode\n");
            textBlock.Inlines.Add("              |              | \"Product P/N\": ENG_xxxx-AP1-RFx\n");
            textBlock.Inlines.Add("   enable     | TRUE/FALSE   | TRUE: Activates this feature for the package\n\n");

            textBlock.Inlines.Add("──────────────────────────────────────────────────\n\n");

            textBlock.Inlines.Add(new Run("🔹 DC CONTACT OPEN/SHORT (O/S) TEST CONFIGURATION\n") { FontWeight = FontWeights.Bold, Foreground = System.Windows.Media.Brushes.Blue });
            textBlock.Inlines.Add(new Run("Function Tag 1: ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("<DC_CONTACT_MODE_ENABLE> (The Switch)\n");
            textBlock.Inlines.Add(new Run("Function Tag 2: ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("<DC_CONTACT_MODE_SETTING_OVERWRITE> (The Settings)\n\n");
            
            textBlock.Inlines.Add(new Run("Objective: ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Customize electrical parameters for Open/Short testing on MIPI and DC pins prior to RF testing.\n");
            textBlock.Inlines.Add(new Run("Applicable Test Stage: ") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("RF1 / RF2 / NFR\n\n");

            textBlock.Inlines.Add(new Run("1. Enable/Disable (The Switch)\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Before configuring parameters, ensure the feature is turned on for your package.\n");
            textBlock.Inlines.Add("Tag: <DC_CONTACT_MODE_ENABLE>\n\n");
            textBlock.Inlines.Add("   Package                       | Enable | Note\n");
            textBlock.Inlines.Add("   SUSER                         | TRUE   | Enable for local debug\n");
            textBlock.Inlines.Add("   Production (e.g. 8288-AP1)    | FALSE  | Typically disabled unless debugging\n\n");

            textBlock.Inlines.Add(new Run("2. Test Methodology\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("• MIPI Pins (VIO, SCLK, SDATA):\n");
            textBlock.Inlines.Add("  - Method: Force Current, Measure Voltage (FORCEI).\n");
            textBlock.Inlines.Add("  - Goal: Detect Open and Short circuits.\n");
            textBlock.Inlines.Add("• DC Pins (Vcc, Vbatt, Vdd):\n");
            textBlock.Inlines.Add("  - Method: Force Voltage, Measure Current (FORCEV).\n");
            textBlock.Inlines.Add("  - Goal: Detect Short circuits only.\n\n");
            
            textBlock.Inlines.Add(new Run("⚠️ Note: ") { FontWeight = FontWeights.Bold, Foreground = System.Windows.Media.Brushes.Red });
            textBlock.Inlines.Add("If a SHORT event is detected, the code will immediately turn OFF all DC supplies to prevent damage.\n\n");

            textBlock.Inlines.Add(new Run("3. Configuration Parameters (Blue Box Settings)\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Tag: <DC_CONTACT_MODE_SETTING_OVERWRITE>\n\n");
            
            textBlock.Inlines.Add(new Run("A. MIPI Pin Settings (Open/Short)\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Apply to pins like Vio1, SCLK, SDATA:\n");
            textBlock.Inlines.Add("   Parameter   | Value   | Description\n");
            textBlock.Inlines.Add("   TestMode    | FORCEI  | Force Current mode\n");
            textBlock.Inlines.Add("   ISource     | 1.5e-6  | Force 1.5 µA\n");
            textBlock.Inlines.Add("   VSetHi      | 1.2     | Max expected voltage (Open circuit)\n");
            textBlock.Inlines.Add("   VSetLo      | -0.3    | Min expected voltage\n");
            textBlock.Inlines.Add("   ILevel      | 2e-6    | Current compliance level\n\n");

            textBlock.Inlines.Add(new Run("B. DC Pin Settings (Short Only)\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("Apply to pins like Vdd, Vbatt, Vcc:\n");
            textBlock.Inlines.Add("   Parameter   | Value   | Description\n");
            textBlock.Inlines.Add("   TestMode    | FORCEV  | Force Voltage mode\n");
            textBlock.Inlines.Add("   VSet        | 0.1     | Force 100mV (0.1 V)\n");
            textBlock.Inlines.Add("   ISet        | 10e-6   | Max current limit (10 µA)\n");
            textBlock.Inlines.Add("   HighLimit   | 9.5e-6  | Fail threshold (Short if > 9.5µA)\n\n");

            textBlock.Inlines.Add(new Run("4. XML Example\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("<!-- STEP 1: Turn it ON -->\n");
            textBlock.Inlines.Add("<DC_CONTACT_MODE_ENABLE>\n");
            textBlock.Inlines.Add("  <Package name=\"SUSER\" enable=\"TRUE\" />\n");
            textBlock.Inlines.Add("  <Package name=\"ENGR-8288-AP1-NF\" enable=\"FALSE\" />\n");
            textBlock.Inlines.Add("</DC_CONTACT_MODE_ENABLE>\n\n");
            
            textBlock.Inlines.Add("<!-- STEP 2: Configure it -->\n");
            textBlock.Inlines.Add("<DC_CONTACT_MODE_SETTING_OVERWRITE>\n");
            textBlock.Inlines.Add("  <Pin Name=\"Vdd\" VSet=\"0.1\" ISet=\"10e-6\" HighLimit=\"9.5e-6\" TestMode=\"FORCEV\"/>\n");
            textBlock.Inlines.Add("  <Pin Name=\"Vio1\" VSetHi=\"1.2\" VSetLo=\"-0.3\" ISource=\"1.5e-6\" ILevel=\"2e-6\" TestMode=\"FORCEI\"/>\n");
            textBlock.Inlines.Add("  <Delayms Value=\"80\"/>\n");
            textBlock.Inlines.Add("</DC_CONTACT_MODE_SETTING_OVERWRITE>\n\n");

            textBlock.Inlines.Add(new Run("5. Expected Results (Reference)\n") { FontWeight = FontWeights.Bold });
            textBlock.Inlines.Add("   Parameter            | Expected (Normal) | Fault Condition\n");
            textBlock.Inlines.Add("   M_MeasV-VIO1         | ~0.3V to 0.55V    | Open: ~1.2V / Short: ~0V\n");
            textBlock.Inlines.Add("   M_MeasV-SCLK/SDATA   | ~0.3V to 0.7V     | Open: ~1.2V / Short: ~0V\n");
            textBlock.Inlines.Add("   M_MeasI-Vbatt/Vdd    | Near 0            | Short: > 10µA");
            */

            scrollViewer.Content = textBlock;
            
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 15, 0, 10)
            };

            var okButton = new Button
            {
                Content = "Close",
                Padding = new Thickness(30, 10, 30, 10),
                IsDefault = true
            };
            okButton.Click += (s, e) => guideDialog.Close();
            buttonPanel.Children.Add(okButton);

            var mainPanel = new DockPanel();
            DockPanel.SetDock(buttonPanel, Dock.Bottom);
            mainPanel.Children.Add(buttonPanel);
            mainPanel.Children.Add(scrollViewer);

            guideDialog.Content = mainPanel;
            guideDialog.ShowDialog();
        }

        private void OnFunctionGuideClick(object sender, RoutedEventArgs e)
        {
            ShowFunctionGuide();
        }

        private void OnHelpClick(object sender, RoutedEventArgs e)
        {
            ShowWelcomeGuide(true);
        }

        private void LoadStep()
        {
            switch (viewModel.CurrentStep)
            {
                case 1: WizardContent.Content = new Step1_SelectNode { DataContext = viewModel }; break;
                case 2: WizardContent.Content = new Step2_EditAttributes { DataContext = viewModel }; break;
                case 3: WizardContent.Content = new Step3_ReviewAndSave { DataContext = viewModel }; break;
            }
        }

        public void NavigateToStep2()
        {
            viewModel.CurrentStep = 2;
            LoadStep();
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            // Save any pending changes before moving to next step
            if (viewModel.CurrentStep == 2)
            {
                viewModel.SaveAttributesToNode();
            }
            
            // Validate that user has added at least one configuration before proceeding from Step 1
            if (viewModel.CurrentStep == 1)
            {
                if (viewModel.NewConfigurationNodes == null || viewModel.NewConfigurationNodes.Count == 0)
                {
                    MessageBox.Show("Please add at least one Configuration Node to your new XML in Step 1 before proceeding.\n\nClick '➕ Add' next to a configuration from the Master Template list.", 
                        "Add Configurations Required", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                // Auto-select the first configuration node for editing
                if (viewModel.SelectedConfigurationNode == null && viewModel.NewConfigurationNodes.Count > 0)
                {
                    viewModel.SelectedConfigurationNode = viewModel.NewConfigurationNodes.Cast<System.Xml.XmlNode>().FirstOrDefault();
                }
            }

            if (viewModel.CurrentStep < 3)
            {
                viewModel.CurrentStep++;
                LoadStep();
            }
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            // Save any pending changes before going back
            if (viewModel.CurrentStep == 2)
            {
                viewModel.SaveAttributesToNode();
            }
            
            if (viewModel.CurrentStep > 1)
            {
                viewModel.CurrentStep--;
                LoadStep();
            }
        }

        private void OnReturnToStartClick(object sender, RoutedEventArgs e)
        {
            // If we are already on Step 1, do nothing
            if (viewModel.CurrentStep == 1)
            {
                return;
            }

            // Save any pending changes
            if (viewModel.CurrentStep == 2)
            {
                viewModel.SaveAttributesToNode();
            }
            
            // Create a custom dialog for clear choices
            var dialog = new Window
            {
                Title = "Return to Start",
                Width = 450,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this,
                WindowStyle = WindowStyle.ToolWindow,
                Background = System.Windows.Media.Brushes.WhiteSmoke,
                Icon = this.Icon // Use main icon instead of default ToolWindow look
            };

            var mainPanel = new StackPanel { Margin = new Thickness(20) };

            // Message
            var messageText = new TextBlock 
            { 
                Text = "You are going back to Step 1.\nWhat would you like to do?", 
                FontSize = 15, 
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20),
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };
            mainPanel.Children.Add(messageText);

            // Option 1: Keep Current (Yes)
            var btnKeep = new Button
            {
                Padding = new Thickness(15, 10, 15, 10),
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = System.Windows.Media.Brushes.White,
                BorderBrush = System.Windows.Media.Brushes.LightGreen,
                BorderThickness = new Thickness(2)
            };
            var keepPanel = new StackPanel();
            keepPanel.Children.Add(new TextBlock { Text = "🎨 Keep Current Configuration", FontWeight = FontWeights.Bold });
            keepPanel.Children.Add(new TextBlock { Text = "    (Add more items to current XML)", FontSize = 11, Foreground = System.Windows.Media.Brushes.Gray });
            btnKeep.Content = keepPanel;
            
            // Option 2: Start Fresh (No)
            var btnNew = new Button
            {
                Padding = new Thickness(15, 10, 15, 10),
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Background = System.Windows.Media.Brushes.White,
                BorderBrush = System.Windows.Media.Brushes.LightSkyBlue,
                BorderThickness = new Thickness(2)
            };
            var newPanel = new StackPanel();
            newPanel.Children.Add(new TextBlock { Text = "📄 Start Fresh", FontWeight = FontWeights.Bold });
            newPanel.Children.Add(new TextBlock { Text = "    (Create a new blank XML)", FontSize = 11, Foreground = System.Windows.Media.Brushes.Gray });
            btnNew.Content = newPanel;

            // Option 3: Cancel
            var btnCancel = new Button
            {
                Content = "Cancel",
                Padding = new Thickness(10, 5, 10, 5),
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 100,
                Margin = new Thickness(0, 10, 0, 0)
            };

            // Event Handlers
            btnKeep.Click += (s, args) => { dialog.Tag = "Keep"; dialog.Close(); };
            btnNew.Click += (s, args) => { dialog.Tag = "New"; dialog.Close(); };
            btnCancel.Click += (s, args) => { dialog.Close(); };

            mainPanel.Children.Add(btnKeep);
            mainPanel.Children.Add(btnNew);
            mainPanel.Children.Add(btnCancel);

            dialog.Content = mainPanel;
            dialog.ShowDialog();
            
            // Handle Result
            if (dialog.Tag?.ToString() == "Keep")
            {
                // Go back to Step 1, keeping current XML
                viewModel.CurrentStep = 1;
                LoadStep();
                viewModel.StatusMessage = "Returned to Step 1. You can now add more configurations or edit existing ones.";
            }
            else if (dialog.Tag?.ToString() == "New")
            {
                // Create new blank XML and go to Step 1
                try
                {
                    viewModel.CreateNewBlankXml();
                    viewModel.CurrentStep = 1;
                    LoadStep();
                    MessageBox.Show("New blank XML created! Add configuration nodes from the Master list.", 
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create new XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            // If Cancel (Tag is null), do nothing
        }

        private void OnNewXmlClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Create a new blank XML configuration? Any unsaved changes will be lost.", 
                "New Configuration", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    viewModel.CreateNewBlankXml();
                    LoadStep();
                    MessageBox.Show("New blank XML created! Add configuration nodes from the Master list.", 
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create new XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnOpenXmlClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    Title = "Open XML Configuration"
                };

                if (dialog.ShowDialog() == true)
                {
                    viewModel.LoadNewXml(dialog.FileName);
                    LoadStep();
                    MessageBox.Show($"XML loaded successfully from:\n{dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnReloadXmlClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string xmlPath = FindXmlFile();
                
                // Reload Master XML (Library on the left)
                viewModel.LoadMasterXml(xmlPath);
                
                // If we are in Edit Master mode, we must ALSO reload the editor (NewXmlDocument)
                // Otherwise, the editor stays stale and will overwrite the file if saved again.
                if (viewModel.IsEditingMaster)
                {
                    viewModel.LoadNewXml(xmlPath);
                    viewModel.IsEditingMaster = true; // LoadNewXml resets this flag, restore it
                }
                
                LoadStep();
                MessageBox.Show("Master XML reloaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to reload Master XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetBoxMasterPath()
        {
            var settings = UserSettings.Load();
            string boxPath = settings.BoxFolderPath;

            // If path is not set, try to auto-detect
            if (string.IsNullOrEmpty(boxPath))
            {
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string[] possiblePaths = {
                    Path.Combine(userProfile, "Box", "Production_Config"),
                    Path.Combine(userProfile, "Box Sync", "Production_Config")
                };

                foreach (var p in possiblePaths)
                {
                    if (Directory.Exists(p))
                    {
                        boxPath = p;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(boxPath))
            {
                return Path.Combine(boxPath, "Master_ProductionUserConfig.xml");
            }
            
            return "";
        }

        private void CheckForBoxUpdate()
        {
            try
            {
                string boxMasterFile = GetBoxMasterPath();
                if (string.IsNullOrEmpty(boxMasterFile)) return; // Box not found

                if (!File.Exists(boxMasterFile)) return; // No master file in Box

                // Find local master file
                string localMasterFile = "";
                try { localMasterFile = FindXmlFile(); } catch { return; }

                // Compare timestamps
                DateTime boxTime = File.GetLastWriteTime(boxMasterFile);
                DateTime localTime = File.GetLastWriteTime(localMasterFile);

                // If Box is newer (> 1 minute difference to avoid timezone issues)
                if (boxTime > localTime.AddMinutes(1))
                {
                    // Copy Box -> Local
                    // Make sure local is writable first
                    var fileInfo = new FileInfo(localMasterFile);
                    bool wasReadOnly = fileInfo.IsReadOnly;
                    if (wasReadOnly) fileInfo.IsReadOnly = false;

                    File.Copy(boxMasterFile, localMasterFile, true);
                    
                    // Restore ReadOnly
                    if (wasReadOnly) fileInfo.IsReadOnly = true;

                    MessageBox.Show(
                        "✅ Master Template Updated from Box!\n\n" +
                        "A newer version was found in your Box folder and has been automatically installed.",
                        "Update Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Box Sync Error: {ex.Message}");
            }
        }

        private void SyncMasterToBox(string localMasterPath)
        {
            try
            {
                string boxMasterFile = GetBoxMasterPath();
                
                // If path is not set, ask user to configure it
                if (string.IsNullOrEmpty(boxMasterFile))
                {
                    MessageBox.Show(
                        "⚠️ Box Sync Path Not Found\n\n" +
                        "Could not automatically detect your Box folder.\n\n" +
                        "The application looked in:\n" +
                        $"• {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Box", "Production_Config")}\n" +
                        $"• {Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Box Sync", "Production_Config")}\n\n" +
                        "Please ensure you have created the folder 'Production_Config' inside your Box root folder.",
                        "Box Sync Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Ensure directory exists
                string? boxDir = Path.GetDirectoryName(boxMasterFile);
                if (boxDir != null && !Directory.Exists(boxDir))
                {
                    Directory.CreateDirectory(boxDir);
                }

                // Copy Local -> Box
                try
                {
                    // Ensure the destination file is not Read-Only before copying
                    if (File.Exists(boxMasterFile))
                    {
                        var boxFileInfo = new FileInfo(boxMasterFile);
                        if (boxFileInfo.IsReadOnly)
                        {
                            boxFileInfo.IsReadOnly = false;
                        }
                    }
                }
                catch { /* Ignore attribute errors */ }

                File.Copy(localMasterPath, boxMasterFile, true);

                MessageBox.Show(
                    "☁️ Master Template Published to Box!\n\n" +
                    $"File synced to: {boxMasterFile}\n\n" +
                    "Other users will receive this update when they restart the app.",
                    "Publish Successful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to sync to Box: {ex.Message}", "Sync Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OnEditMasterClick(object sender, RoutedEventArgs e)
        {
            // 1. Create Password Dialog
            var passwordDialog = new Window
            {
                Title = "Developer Authentication",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this,
                WindowStyle = WindowStyle.ToolWindow
            };

            var panel = new StackPanel { Margin = new Thickness(20) };
            
            panel.Children.Add(new TextBlock { Text = "Enter Passcode:", Margin = new Thickness(0,0,0,5) });
            
            var passwordBox = new PasswordBox();
            panel.Children.Add(passwordBox);
            
            var btnPanel = new StackPanel { 
                Orientation = Orientation.Horizontal, 
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0,15,0,0) 
            };
            
            var btnOk = new Button { Content = "OK", Width = 60, Margin = new Thickness(0,0,10,0), IsDefault = true };
            var btnCancel = new Button { Content = "Cancel", Width = 60, IsCancel = true };
            
            bool isAuthenticated = false;
            
            btnOk.Click += (s, args) => {
                // Hardcoded passcode as requested
                if (passwordBox.Password == "admin123") {
                    isAuthenticated = true;
                    passwordDialog.Close();
                } else {
                    MessageBox.Show("Invalid passcode!", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                    passwordBox.Password = "";
                    passwordBox.Focus();
                }
            };
            
            btnCancel.Click += (s, args) => {
                passwordDialog.Close();
            };
            
            btnPanel.Children.Add(btnOk);
            btnPanel.Children.Add(btnCancel);
            panel.Children.Add(btnPanel);
            
            passwordDialog.Content = panel;
            
            passwordDialog.Loaded += (s, args) => passwordBox.Focus();
            
            passwordDialog.ShowDialog();
            
            if (!isAuthenticated) return;

            var result = MessageBox.Show(
                "⚠️ DANGER ZONE: EDIT MASTER TEMPLATE\n\n" +
                "You are about to unlock and edit the Master Template file directly.\n\n" +
                "• This will MODIFY the read-only Master XML file.\n" +
                "• Any changes will affect ALL users who load this template.\n" +
                "• Use this ONLY for adding new global configurations.\n\n" +
                "Are you sure you want to proceed?",
                "Developer Mode",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _isInternalOperation = true; // Suppress watcher during initial load/unlock
                try
                {
                    // 1. Unlock the file
                    viewModel.SetMasterReadOnly(false);
                    
                    // 2. Load it as the "New" XML (editable)
                    if (string.IsNullOrEmpty(viewModel.MasterXmlPath))
                    {
                        string path = FindXmlFile();
                        viewModel.LoadMasterXml(path);
                        // Ensure unlocked if LoadMasterXml locked it
                        viewModel.SetMasterReadOnly(false);
                    }
                    
                    viewModel.LoadNewXml(viewModel.MasterXmlPath);
                    
                    // Set flag to indicate we are editing the Master file
                    viewModel.IsEditingMaster = true;
                    
                    // 3. Reset to Step 1
                    viewModel.CurrentStep = 1;
                    LoadStep();
                    
                    viewModel.StatusMessage = "⚠️ EDITING MASTER TEMPLATE - SAVE WILL OVERWRITE MASTER FILE";
                    
                    // 4. Open in Notepad++ if available (optional convenience)
                    try
                    {
                        string notepadPath = @"C:\Program Files\Notepad++\notepad++.exe";
                        if (File.Exists(notepadPath))
                        {
                            System.Diagnostics.Process.Start(notepadPath, viewModel.MasterXmlPath);
                        }
                        else
                        {
                            // Fallback to default editor if Notepad++ not found
                            System.Diagnostics.Process.Start("notepad.exe", viewModel.MasterXmlPath);
                        }
                        
                        // Give Notepad++ a moment to open and grab focus before we steal it back
                        await System.Threading.Tasks.Task.Delay(500);

                        // Bring app to front so prompt is visible
                        this.Activate();
                        this.Topmost = true;
                        this.Topmost = false;
                        this.Focus();
                    }
                    catch { /* Ignore errors if external editor fails to open */ }

                    MessageBox.Show(
                        "🔓 Master Template Unlocked!\n\n" +
                        "The Master XML is now loaded as your active document.\n" +
                        "Click 'Save' to update the Master file directly.\n\n" +
                        "Please be careful!",
                        "Master Unlocked",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open Master for editing: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    // Re-enable watcher after delay
                    System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ => _isInternalOperation = false);
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            // If a file is loaded and has content, check for saving
            if (viewModel.NewXmlDocument != null && viewModel.NewXmlDocument.DocumentElement != null)
            {
                var result = MessageBox.Show(
                    "Do you want to save your changes before exiting?\n\n" +
                    "• Click 'Yes' to save changes\n" +
                    "• Click 'No' to exit without saving\n" +
                    "• Click 'Cancel' to stay",
                    "Save Changes?",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Save any pending attribute changes to the XML node
                        viewModel.SaveAttributesToNode();
                        
                        // Check if we have a path, otherwise prompt for save location
                        string currentPath = viewModel.GetNewXmlPath();
                        string savePath;
                        
                        if (string.IsNullOrEmpty(currentPath))
                        {
                            // New file - prompt for save location
                            var dialog = new Microsoft.Win32.SaveFileDialog
                            {
                                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                                Title = "Save XML Configuration",
                                FileName = "ProductionUserConfig.xml"
                            };

                            if (dialog.ShowDialog() == true)
                            {
                                savePath = dialog.FileName;
                            }
                            else
                            {
                                e.Cancel = true; // User cancelled save dialog
                                return;
                            }
                        }
                        else
                        {
                            savePath = currentPath;
                        }
                        
                        viewModel.SaveXml(savePath);
                        
                        // 🔴 SAFETY: Check for Master File Sync on Exit
                        string normalizedSavePath = Path.GetFullPath(savePath);
                        string normalizedMasterPath = Path.GetFullPath(viewModel.MasterXmlPath);
                        bool isPathMatch = normalizedSavePath.Equals(normalizedMasterPath, StringComparison.OrdinalIgnoreCase);

                        if (viewModel.IsEditingMaster || isPathMatch)
                        {
                            // 1. Sync to Box
                            // SyncMasterToBox(savePath); // Disable sync on exit to prevent double sync or sync of incomplete work
                            
                            // 2. Ensure file remains unlocked if we just saved it (for consistent developer experience)
                            try 
                            {
                                if (viewModel.IsEditingMaster)
                                {
                                    viewModel.SetMasterReadOnly(false);
                                }
                            }
                            catch { /* Best effort on exit */ }
                        }

                        // Save successful - proceed to close
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save XML: {ex.Message}\n\nApplication will not close.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true; // Cancel closing on error
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; // Cancel closing
                }
            }

            // 🛡️ SAFETY CHECK ON CLOSE:
            // If we are editing Master (or file matches Master), and Local File is NEWER than Box File,
            // it means we have saved locally but NOT synced to Box yet.
            // Ask user if they want to Sync now.
            if (!e.Cancel)
            {
                try
                {
                    // Only check if we are dealing with the Master file
                    string currentPath = viewModel.GetNewXmlPath();
                    if (!string.IsNullOrEmpty(currentPath) && !string.IsNullOrEmpty(viewModel.MasterXmlPath))
                    {
                        string normalizedCurrent = Path.GetFullPath(currentPath);
                        string normalizedMaster = Path.GetFullPath(viewModel.MasterXmlPath);
                        
                        if (viewModel.IsEditingMaster || normalizedCurrent.Equals(normalizedMaster, StringComparison.OrdinalIgnoreCase))
                        {
                            string boxMasterFile = GetBoxMasterPath();
                            if (!string.IsNullOrEmpty(boxMasterFile) && File.Exists(boxMasterFile) && File.Exists(currentPath))
                            {
                                DateTime boxTime = File.GetLastWriteTime(boxMasterFile);
                                DateTime localTime = File.GetLastWriteTime(currentPath);

                                // If Local is significantly newer (> 5 seconds) than Box
                                if (localTime > boxTime.AddSeconds(5))
                                {
                                    var syncResult = MessageBox.Show(
                                        "⚠️ UNPUBLISHED CHANGES DETECTED\n\n" +
                                        "Your local Master Template has changes that haven't been synced to Box.\n" +
                                        "If you close now, these changes will NOT be deployed to other users.\n\n" +
                                        "Do you want to Sync to Box before closing?",
                                        "Sync on Exit?",
                                        MessageBoxButton.YesNoCancel,
                                        MessageBoxImage.Warning);

                                    if (syncResult == MessageBoxResult.Yes)
                                    {
                                        SyncMasterToBox(currentPath);
                                    }
                                    else if (syncResult == MessageBoxResult.Cancel)
                                    {
                                        e.Cancel = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error checking sync status on close: {ex.Message}");
                }
            }
        }

        private void OnSaveXmlClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Save any pending attribute changes to the XML node
                viewModel.SaveAttributesToNode();
                
                // Check if we have a path, otherwise prompt for save location
                string currentPath = viewModel.GetNewXmlPath();
                string savePath;
                
                if (string.IsNullOrEmpty(currentPath))
                {
                    // New file - prompt for save location
                    var dialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                        Title = "Save XML Configuration",
                        FileName = "ProductionUserConfig.xml"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        savePath = dialog.FileName;
                    }
                    else
                    {
                        return; // User cancelled
                    }
                }
                else
                {
                    // Existing file - ask for confirmation before overwriting
                    var confirmResult = MessageBox.Show(
                        $"This will overwrite the existing file:\n\n{currentPath}\n\n" +
                        "Do you want to continue?\n\n" +
                        "• Click 'Yes' to overwrite the existing file\n" +
                        "• Click 'No' to save as a new file instead\n" +
                        "• Click 'Cancel' to abort",
                        "Confirm Overwrite",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning);
                    
                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        // User confirmed - overwrite existing file
                        savePath = currentPath;
                    }
                    else if (confirmResult == MessageBoxResult.No)
                    {
                        // User wants to save as new file - prompt for new location
                        var dialog = new Microsoft.Win32.SaveFileDialog
                        {
                            Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                            Title = "Save XML Configuration As New File",
                            FileName = System.IO.Path.GetFileName(currentPath),
                            InitialDirectory = System.IO.Path.GetDirectoryName(currentPath)
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            savePath = dialog.FileName;
                        }
                        else
                        {
                            return; // User cancelled
                        }
                    }
                    else
                    {
                        return; // User cancelled
                    }
                }

                // Check if we are saving the Master file (in Edit Master mode)
                // We must detect this BEFORE saving to suppress the file watcher
                string normalizedSavePath = Path.GetFullPath(savePath);
                string normalizedMasterPath = Path.GetFullPath(viewModel.MasterXmlPath);
                bool isPathMatch = normalizedSavePath.Equals(normalizedMasterPath, StringComparison.OrdinalIgnoreCase);
                bool isMasterSave = viewModel.IsEditingMaster || isPathMatch;

                if (isMasterSave)
                {
                    _isInternalOperation = true; // Suppress watcher immediately so we don't trigger "External Change" message
                }

                try
                {
                    viewModel.SaveXml(savePath);
                }
                catch
                {
                    if (isMasterSave) _isInternalOperation = false; // Reset if save failed
                    throw;
                }
                
                // If so, sync to Box and reload
                if (isMasterSave)
                {
                    try
                    {
                        // 1. Sync to Box
                        SyncMasterToBox(savePath);

                        // 2. Enforce Reload logic as requested by user to prevent state inconsistency
                        try 
                        {
                            viewModel.LoadMasterXml(savePath);
                            
                            // CRITICAL FIX: LoadMasterXml automatically locks the file (ReadOnly=true).
                            // Since we are in 'Edit Master' mode, we must UNLOCK it immediately so external editors (Notepad++) 
                            // or subsequent saves don't hit Access Denied errors.
                            if (viewModel.IsEditingMaster)
                            {
                                viewModel.SetMasterReadOnly(false);
                            }
                            
                            // Reload the current working document from the saved file
                            bool wasEditing = viewModel.IsEditingMaster;
                            viewModel.LoadNewXml(savePath);
                            if (wasEditing) viewModel.IsEditingMaster = true; 
                            
                            LoadStep(); // Refresh UI
                            
                            MessageBox.Show(
                                "Master Template saved and reloaded successfully.\n\n" +
                                "The application state has been synchronized with the file on disk.",
                                "Master Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception loadEx)
                        {
                            MessageBox.Show($"Warning: Auto-reload after save failed: {loadEx.Message}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    finally
                    {
                        // Re-enable watcher after short delay
                        System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ => _isInternalOperation = false);
                    }
                }
                else
                {
                    // Normal save success message for non-master files
                    var result = MessageBox.Show(
                        $"XML saved successfully to:\n{savePath}\n\nDo you want to open the file location?", 
                        "Success", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Information);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        // Ensure path is absolute and uses backslashes for explorer
                        string fullPath = Path.GetFullPath(savePath);
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnSaveAsXmlClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Save any pending attribute changes to the XML node
                viewModel.SaveAttributesToNode();
                
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    Title = "Save XML Configuration As",
                    FileName = "ProductionUserConfig.xml"
                };

                if (dialog.ShowDialog() == true)
                {
                    viewModel.SaveXml(dialog.FileName);
                    
                    // Show success message with option to open file location
                    var result = MessageBox.Show(
                        $"XML saved successfully to:\n{dialog.FileName}\n\nDo you want to open the file location?", 
                        "Success", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Information);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        // Open file explorer and select the saved file
                        // Ensure path is absolute and uses backslashes for explorer
                        string fullPath = Path.GetFullPath(dialog.FileName);
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnFieldChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            // Add validation logic here if needed
            // For now, this is a placeholder method
        }

        private string FindXmlFile()
        {
            string fileName = "Master_ProductionUserConfig.xml";
            
            // List of potential paths to check
            string[] searchPaths = {
                fileName, // Current working directory
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), // Application base directory
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), fileName), // Explicit current directory
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "", fileName), // Assembly location
                System.IO.Path.Combine(Environment.CurrentDirectory, fileName), // Environment current directory
            };

            // Check each path
            foreach (string path in searchPaths)
            {
                if (System.IO.File.Exists(path))
                {
                    string fullPath = System.IO.Path.GetFullPath(path);
                    System.Diagnostics.Debug.WriteLine($"Found XML file at: {fullPath}");
                    return fullPath;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"XML file not found at: {path}");
                }
            }

            // If not found in any standard location, provide detailed error information
            string errorMessage = $"XML file '{fileName}' not found in any of the following locations:\n";
            for (int i = 0; i < searchPaths.Length; i++)
            {
                errorMessage += $"{i + 1}. {searchPaths[i]}\n";
            }
            errorMessage += $"\nCurrent working directory: {Environment.CurrentDirectory}";
            errorMessage += $"\nApplication base directory: {AppDomain.CurrentDomain.BaseDirectory}";
            errorMessage += $"\nAssembly location: {System.Reflection.Assembly.GetExecutingAssembly().Location}";

            throw new FileNotFoundException(errorMessage);
        }
        
        private void OnAdvancedSettingsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create ViewModel and load from user's file (or Master as fallback)
                var advancedViewModel = new AdvancedSettingsViewModel();
                
                // Try to load from user's ProductionUserConfig.xml first (to show saved values)
                XmlDocument? documentToLoad = null;
                
                if (viewModel.NewXmlDocument != null && viewModel.NewXmlDocument.DocumentElement != null)
                {
                    // User has a file loaded - check if it has Advanced Settings
                    var testViewModel = new AdvancedSettingsViewModel();
                    testViewModel.LoadFromXmlDocument(viewModel.NewXmlDocument);
                    
                    if (testViewModel.RootNodes != null && testViewModel.RootNodes.Count > 0)
                    {
                        // User's file has Advanced Settings - use it (shows saved values)
                        documentToLoad = viewModel.NewXmlDocument;
                        System.Diagnostics.Debug.WriteLine("Loading Advanced Settings from user's file (saved values)");
                    }
                }
                
                // Fall back to Master if user file doesn't have Advanced Settings
                if (documentToLoad == null)
                {
                    if (viewModel.MasterXmlDocument != null)
                    {
                        documentToLoad = viewModel.MasterXmlDocument;
                        System.Diagnostics.Debug.WriteLine("Loading Advanced Settings from Master (default values)");
                    }
                    else
                    {
                        MessageBox.Show(
                            "⚠️ Master XML not loaded!\n\nPlease reload the Master XML first.",
                            "Master XML Required",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }
                }
                
                // Load the document
                advancedViewModel.LoadFromXmlDocument(documentToLoad);
                
                // If we loaded from User file (NewXmlDocument), verify if we are missing any settings from Master
                // This ensures the dialog shows ALL available options, not just what the user has already saved.
                if (documentToLoad == viewModel.NewXmlDocument && viewModel.MasterXmlDocument != null)
                {
                    // 1. Add missing items (Standard feature)
                    advancedViewModel.MergeMissingSettingsFromMaster(viewModel.MasterXmlDocument);
                    
                    // 2. If in "Edit Master" mode, also check for DELETED items (Sync feature)
                    if (viewModel.IsEditingMaster)
                    {
                        advancedViewModel.PruneOrphanedSettings(viewModel.MasterXmlDocument);
                    }
                }
                
                // Store which document we loaded from for save operation
                XmlDocument sourceDocument = documentToLoad;
                
                // Check if there are any advanced settings
                if (advancedViewModel.RootNodes == null || advancedViewModel.RootNodes.Count == 0)
                {
                    MessageBox.Show(
                        "ℹ️ No Advanced Settings Found\n\n" +
                        "The Master template doesn't contain any special configurations\n" +
                        "that need Advanced Settings.\n\n" +
                        "All available settings can be managed through the main wizard.",
                        "No Advanced Settings",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                
                // Create dialog for Advanced Settings (no welcome popup - list already shown in Step 1)
                var dialog = new Window
                {
                    Title = "⚙️ Advanced Settings",
                    Width = 1200,
                    Height = 700,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                // Load view
                if (viewModel.MasterXmlDocument != null)
                {
                    // Already loaded above
                }
                else
                {
                    MessageBox.Show(
                        "No XML document is loaded.\n\nPlease reload the application.",
                        "No XML Loaded",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Check if any non-standard configurations were found
                if (advancedViewModel.RootNodes.Count == 0)
                {
                    MessageBox.Show(
                        "No non-standard configurations detected in the current XML.\n\n" +
                        "Advanced Settings displays configurations that:\n" +
                        "• Do not use <Package> elements\n" +
                        "• Have custom XML structures\n\n" +
                        "Your current XML only contains standard Package-based configurations.",
                        "No Advanced Settings Available",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                // Create and show the view
                var advancedView = new AdvancedSettingsView
                {
                    DataContext = advancedViewModel
                };

                dialog.Content = advancedView;
                
                // Save changes when dialog closes
                dialog.Closing += (s, args) =>
                {
                    var result = MessageBox.Show(
                        "Do you want to save the changes made in Advanced Settings?",
                        "Save Changes?",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Cancel)
                    {
                        args.Cancel = true; // Cancel the closing
                        return;
                    }
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Save changes to Master XML in-memory
                            advancedViewModel.SaveChanges();
                            
                            // Auto-load default file if none exists
                            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            string defaultConfigPath = System.IO.Path.Combine(documentsPath, "ProductionUserConfig.xml");
                            
                            // Load existing file or create new one
                            if (viewModel.NewXmlDocument == null || viewModel.NewXmlDocument.DocumentElement == null)
                            {
                                if (System.IO.File.Exists(defaultConfigPath))
                                {
                                    // Load existing file
                                    viewModel.LoadNewXml(defaultConfigPath);
                                }
                                else
                                {
                                    // Create new blank XML
                                    viewModel.CreateNewBlankXml();
                                }
                            }
                            
                            // Remove and re-add ALL Advanced Settings nodes dynamically
                            // Copy from sourceDocument (where changes were saved), not always from Master
                            if (advancedViewModel.RootNodes != null && viewModel.NewXmlDocument != null && sourceDocument != null)
                            {
                                foreach (var rootNode in advancedViewModel.RootNodes)
                                {
                                    if (rootNode.SourceNode == null) continue;
                                    string nodeName = rootNode.SourceNode.Name;
                                    
                                    // Remove existing node from NewXmlDocument (if present)
                                    var existingNode = viewModel.NewXmlDocument.SelectSingleNode($"//{nodeName}");
                                    if (existingNode != null)
                                    {
                                        existingNode.ParentNode?.RemoveChild(existingNode);
                                    }
                                    
                                    // Copy updated node from sourceDocument (where SaveChanges() updated it)
                                    var updatedNode = sourceDocument.SelectSingleNode($"//{nodeName}");
                                    if (updatedNode != null)
                                    {
                                        var importedNode = viewModel.NewXmlDocument.ImportNode(updatedNode, true);
                                        viewModel.NewXmlDocument.DocumentElement?.AppendChild(importedNode);
                                    }
                                }
                            }
                            
                            // Auto-save directly to file (Option C)
                            string savePath = viewModel.GetNewXmlPath();
                            if (string.IsNullOrEmpty(savePath))
                            {
                                // No path yet - use default location
                                savePath = defaultConfigPath;
                            }
                            
                            // Save to file
                            viewModel.SaveXml(savePath);
                            
                            // CRITICAL: If we are editing master, reload it immediately to keep MasterXmlDocument in sync
                            // This ensures the UI (Step 1) and internal state are fresh, preventing accidental revert.
                            if (viewModel.IsEditingMaster)
                            {
                                try 
                                {
                                    viewModel.LoadMasterXml(savePath);
                                    LoadStep(); // Refresh UI to reflect changes
                                }
                                catch (Exception reloadEx)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Failed to auto-reload master after advanced settings save: {reloadEx.Message}");
                                }
                            }
                            
                            viewModel.StatusMessage = $"Advanced Settings saved to {savePath}";
                            MessageBox.Show(
                                "✅ Changes saved successfully!\n\n" +
                                $"File: {savePath}\n\n" +
                                "Your Advanced Settings have been saved to the file.",
                                "Save Successful",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"Error saving Advanced Settings:\n\n{ex.Message}",
                                "Save Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                };

                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error opening Advanced Settings:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ShowConfigurationLoadedDialog(string filePath)
        {
            // Check settings first
            var settings = UserSettings.Load();
            if (!settings.ShowConfigLoadedMessage) return;

            string message = 
                $"📂 Existing configuration loaded automatically:\n\n{filePath}\n\n" +
                "You can now:\n" +
                "• Add/Edit packages\n" +
                "• Click 💾 Save to update this file\n" +
                "• Click 📂 Open to load a different file\n" +
                "• Click 📄 New to start fresh";

            var dialog = new Window
            {
                Title = "Configuration Loaded",
                Width = 500,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Icon = this.Icon,
                // Owner = this // Don't set Owner here as MainWindow might not be fully shown yet in Constructor
            };

            var panel = new StackPanel { Margin = new Thickness(20) };

            // Icon and Message
            var msgPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 20) };
            
            // Add info icon (using simple text representation or system icon if possible, but sticking to simple for now)
            // WPF doesn't make standard icons easy without binding. We'll stick to text.
            
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                Margin = new Thickness(5, 0, 0, 0)
            };
            msgPanel.Children.Add(textBlock);
            panel.Children.Add(msgPanel);

            // Checkbox
            var dontShowCheck = new CheckBox
            {
                Content = "Don't show this again",
                Margin = new Thickness(5, 0, 0, 15),
                FontSize = 12
            };
            panel.Children.Add(dontShowCheck);

            // Button
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            var okButton = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 25,
                IsDefault = true
            };
            okButton.Click += (s, e) =>
            {
                if (dontShowCheck.IsChecked == true)
                {
                    settings.ShowConfigLoadedMessage = false;
                    settings.Save();
                }
                dialog.Close();
            };
            btnPanel.Children.Add(okButton);
            panel.Children.Add(btnPanel);

            dialog.Content = panel;
            dialog.ShowDialog();
        }

    }
}

