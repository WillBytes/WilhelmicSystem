using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Wilhelmic_System.ViewModels;

namespace Wilhelmic_System.Views
{
    public partial class DayCellView : UserControl
    {
        public DayCellView()
        {
            InitializeComponent();
            // Optionally set DataContext in code-behind or use [DataContext] in XAML
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}