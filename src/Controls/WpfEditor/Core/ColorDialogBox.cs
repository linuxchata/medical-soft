using System.Windows.Media;

namespace WpfEditor.Core
{
    /// <summary>
    /// Represents dialog box.
    /// </summary>
    public class ColorDialogBox
    {
        /// <summary>
        /// Select color.
        /// </summary>
        /// <returns>Returns selected color.</returns>
        public Color? SelectColor()
        {
            using (var colorDialog = new System.Windows.Forms.ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.FullOpen = true;
                var result = colorDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var color = new Color
                    {
                        A = colorDialog.Color.A,
                        B = colorDialog.Color.B,
                        G = colorDialog.Color.G,
                        R = colorDialog.Color.R
                    };

                    return color;
                }
            }

            return null;
        }
    }
}
