using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using Logger;
using Microsoft.Expression.Encoder.Live;

namespace CameraPreview
{
    /// <summary>
    /// Interaction logic for PreviewPanel
    /// </summary>
    public partial class PreviewPanel
    {
        /// <summary>
        /// The live source.
        /// </summary>
        private Utilities.VideoHandlers.LiveSource source;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewPanel"/> class.
        /// </summary>
        public PreviewPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initialize preview.
        /// </summary>
        public void InitializePreview()
        {
            try
            {
                this.source = new Utilities.VideoHandlers.LiveSource();

                if (this.source.HasError)
                {
                    MessageBox.Show("No Video/Audio capture devices detected.", "Dental Soft", MessageBoxButton.OK);
                    return;
                }

                // Sets preview window to winform panel hosted by xaml window.
                this.Dispatcher.BeginInvoke(
                    DispatcherPriority.ApplicationIdle,
                    new Action(() =>
                    {
                        var handleRef = new HandleRef(this.Panel, this.Panel.Handle);
                        this.source.DeviceSource.PreviewWindow = new PreviewWindow(handleRef);
                    }));
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// Take picture.
        /// </summary>
        /// <returns>Returns picture.</returns>
        public Bitmap TakePicture()
        {
            try
            {
                return this.TakePictureInternal();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// Dispose all used resources.
        /// </summary>
        public void Dispose()
        {
            if (this.source != null)
            {
                this.source.Dispose();
            }
        }

        /// <summary>
        /// Take picture.
        /// </summary>
        /// <returns>Returns picture.</returns>
        private Bitmap TakePictureInternal()
        {
            var x = this.Panel.ClientRectangle.X;
            var y = this.Panel.ClientRectangle.Y;

            var point = new System.Drawing.Point();

            this.Dispatcher.Invoke(
                DispatcherPriority.ApplicationIdle,
                new Action(() =>
                {
                    point = this.Panel.PointToScreen(new System.Drawing.Point(x, y));
                }));

            var size = new System.Drawing.Size(this.Panel.Width, this.Panel.Height);

            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
                using (var graphic = Graphics.FromImage(bitmap))
                {
                    graphic.CopyFromScreen(point, System.Drawing.Point.Empty, size);
                }

                return (Bitmap)bitmap.Clone();
            }
        }

        /// <summary>
        /// Size changed event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.source != null && this.source.DeviceSource.PreviewWindow != null)
            {
                var newSize = new System.Drawing.Size((int)e.NewSize.Width, (int)e.NewSize.Height);
                this.source.DeviceSource.PreviewWindow.SetSize(newSize);
            }
        }
    }
}
