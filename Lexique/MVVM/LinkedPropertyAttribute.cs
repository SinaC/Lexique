using System;

namespace Lexique.WpfApp.MVVM
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LinkedPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public LinkedPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
