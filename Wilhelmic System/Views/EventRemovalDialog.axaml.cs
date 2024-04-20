using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Wilhelmic_System.Views
{
    internal partial class EventRemovalDialog : Window
    {
        public bool DialogResult { get; private set; } = false;

        public EventRemovalDialog()
        {
            InitializeComponent(); // call the procedure initialize the axaml for the dialog box
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this); // initialize axaml for the dialog box
        }

        public void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;  // Set the dialog result to true
            this.Close();  // Close the dialog box
        }

        public void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // Set the dialog result to false
            this.Close();  // Close the dialog box
        }
    }
}
