using System.Windows;
using System.Windows.Controls;

namespace DigitalProductionConfigEditor.Views
{
    public class InputDialog : Window
    {
        private TextBox _textBox;
        public string InputText { get; private set; } = "";

        public InputDialog(string prompt, string title, string defaultText = "")
        {
            Title = title;
            Width = 400;
            Height = 180;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;

            var stackPanel = new StackPanel { Margin = new Thickness(20) };
            
            stackPanel.Children.Add(new TextBlock { Text = prompt, Margin = new Thickness(0,0,0,10), FontWeight = FontWeights.SemiBold });
            
            _textBox = new TextBox { Text = defaultText, Height = 30, VerticalContentAlignment = VerticalAlignment.Center };
            stackPanel.Children.Add(_textBox);
            
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0,20,0,0) };
            
            var okBtn = new Button { Content = "OK", Width = 80, Height = 30, Margin = new Thickness(0,0,10,0), IsDefault = true, Background = System.Windows.Media.Brushes.LightBlue };
            okBtn.Click += (s, e) => { InputText = _textBox.Text; DialogResult = true; };
            
            var cancelBtn = new Button { Content = "Cancel", Width = 80, Height = 30, IsCancel = true };
            
            btnPanel.Children.Add(okBtn);
            btnPanel.Children.Add(cancelBtn);
            stackPanel.Children.Add(btnPanel);
            
            Content = stackPanel;
            
            Loaded += (s, e) => { _textBox.Focus(); _textBox.SelectAll(); };
        }
    }
}























































































































































