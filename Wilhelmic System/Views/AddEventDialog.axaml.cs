using Avalonia.Controls;
using Avalonia.Interactivity;
using Wilhelmic_System.ViewModels;

namespace Wilhelmic_System.Views
{
    public partial class AddEventDialog : Window
    {
        public WilhelmicEventItem NewEvent { get; private set; }
        private DateStruct currentlyDisplayedDate;

        public AddEventDialog(DateStruct date)
        {
            InitializeComponent();
            currentlyDisplayedDate = date;
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            var title = this.FindControl<TextBox>("TitleBox").Text;
            var description = this.FindControl<TextBox>("DescriptionBox").Text;
            var location = this.FindControl<TextBox>("LocationBox").Text;

            NewEvent = new WilhelmicEventItem(
                title,
                currentlyDisplayedDate,
                currentlyDisplayedDate,
                description,
                location
            );

            this.Close();
        }
    }
}
