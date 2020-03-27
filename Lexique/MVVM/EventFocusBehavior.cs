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
            // TODO: remove old event
            var button = sender as Button;
            if (button != null)
            {
                button.Click += (s, args) =>
                {
                    Control control = GetElementToFocus(button);
                    if (control != null)
                    {
                        control.Focus();
                        if (control is TextBox textBox)
                        {
                            textBox.SelectAll();
                        }
                    }
                };
            }
        }
    }
}
