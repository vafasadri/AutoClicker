using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Input;

namespace AutoClicker.Utils
{
    public static class AssemblyUtils
    {

        private static readonly Assembly assembly = Assembly.GetExecutingAssembly();

        public static AssemblyName GetAssemblyInfo()
            => assembly.GetName();
        public static double Distance(this Point me, Point other)
        {
            var dx = me.X - other.X;
            var dy = me.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        public static Icon GetApplicationIcon()
            => Icon.ExtractAssociatedIcon(assembly.Location);

        public static string GetProjectURL()
            => assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        public static Uri GetProjectUri()
            => new Uri(GetProjectURL());

        public static RoutedUICommand CreateCommand(Type windowType, string commandName, KeyGesture keyGesture = null)
            => keyGesture == null
                ? new RoutedUICommand(commandName, commandName, windowType)
                : new RoutedUICommand(commandName, commandName, windowType, new InputGestureCollection() { keyGesture });
    }
}
