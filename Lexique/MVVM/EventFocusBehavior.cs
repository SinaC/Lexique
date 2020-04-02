using System.Windows;
using System.Windows.Controls;

namespace Lexique.MVVM
{
    public class EventFocusBehavior
    {
        public static Control GetElementToFocus(Button button)
        {
            return (Control)button.GetValue(ElementToFocusProperty);
        }

        public static void SetElementToFocus(Button button, Control value)
        {
            button.SetValue(ElementToFocusProperty, value);
        }

        public static readonly DependencyProperty ElementToFocusProperty = DependencyProperty.RegisterAttached(
            "ElementToFocus",
            typeof(Control),
            typeof(EventFocusBehavior),
            new UIPropertyMetadata(null, ElementToFocusPropertyChanged));

        public static void ElementToFocusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Button button)
            {
                if (e.OldValue != null)
                    button.Click -= ButtonOnClick;
                if (e.NewValue != null)
                    button.Click += ButtonOnClick;
            }
        }

        private static void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Control control = GetElementToFocus(button);
            if (control != null)
            {
                control.Focus();
                if (control is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }
        }
    }
}
