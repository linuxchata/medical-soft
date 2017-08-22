using System.Reflection;
using System.Windows.Controls;

namespace WpfEditor.Core
{
    /// <summary>
    /// Represents script handler.
    /// </summary>
    public static class Script
    {
        /// <summary>
        /// Hide script errors.
        /// </summary>
        /// <param name="webBrowser">Web browser.</param>
        public static void HideScriptErrors(WebBrowser webBrowser)
        {
            if (webBrowser == null)
            {
                return;
            }

            var fieldInfo = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);

            if (fieldInfo != null)
            {
                var comWebBrowser = fieldInfo.GetValue(webBrowser);

                if (comWebBrowser != null)
                {
                    comWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, comWebBrowser, new object[] { true });
                }
            }
        }
    }
}
