using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Logger;

namespace Autocomplete
{
    /// <summary>
    /// Represents Autocomplete control.
    /// </summary>
    public class Autocomplete : System.Windows.Controls.TextBox
    {
        /// <summary>
        /// Items source.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(Autocomplete),
            new FrameworkPropertyMetadata(OnItemsSourcePropertyChanged));

        /// <summary>
        /// Selected item.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(Autocomplete),
            new FrameworkPropertyMetadata(OnSelectedItemPropertyChanged));

        /// <summary>
        /// Search pattern.
        /// </summary>
        public static readonly DependencyProperty SearchPatternProperty =
            DependencyProperty.Register(
            "SearchPattern",
            typeof(object),
            typeof(Autocomplete));

        /// <summary>
        /// Name of the property that represents value of the item.
        /// </summary>
        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register(
            "SelectedValuePath",
            typeof(string),
            typeof(Autocomplete));

        /// <summary>
        /// Name of the property that represents display name of the item.
        /// </summary>
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
            "DisplayMemberPath",
            typeof(string),
            typeof(Autocomplete));

        /// <summary>
        /// Main panel object.
        /// </summary>
        private StackPanel mainPanel;

        /// <summary>
        /// Result popup object.
        /// </summary>
        private Popup resultPopup;

        /// <summary>
        /// Items panel object.
        /// </summary>
        private StackPanel itemsPanel;

        /// <summary>
        /// Spinner object.
        /// </summary>
        private Spinner.Spinner spinner;

        /// <summary>
        /// Indicates whether the selected item was initialized.
        /// </summary>
        private bool isSelectedItemInitialized;

        /// <summary>
        /// Item type.
        /// </summary>
        private Type sourceItemType;

        /// <summary>
        /// Initializes static members of the Autocomplete class.
        /// </summary>
        static Autocomplete()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Autocomplete), new FrameworkPropertyMetadata(typeof(Autocomplete)));

            EventManager.RegisterClassHandler(typeof(Autocomplete), Keyboard.KeyUpEvent, new KeyEventHandler(OnKeyUp));

            EventManager.RegisterClassHandler(typeof(Autocomplete), Keyboard.KeyDownEvent, new KeyEventHandler(OnKeyDown));
        }

        /// <summary>
        /// Gets or sets items source.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)this.GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets selected item.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return this.GetValue(SelectedItemProperty);
            }

            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets name of the property that represents search pattern.
        /// </summary>
        public string SearchPattern
        {
            get
            {
                return (string)this.GetValue(SearchPatternProperty);
            }

            set
            {
                this.SetValue(SearchPatternProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets name of the property that represents value of the item.
        /// </summary>
        public string SelectedValuePath
        {
            get
            {
                return (string)this.GetValue(SelectedValuePathProperty);
            }

            set
            {
                this.SetValue(SelectedValuePathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets name of the property that represents display name of the item.
        /// </summary>
        public string DisplayMemberPath
        {
            get
            {
                return (string)this.GetValue(DisplayMemberPathProperty);
            }

            set
            {
                this.SetValue(DisplayMemberPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets internal items.
        /// </summary>
        private IList<InternalItem> InternalItems { get; set; }

        /// <summary>
        /// Gets id property name.
        /// </summary>
        private string IdPropertyName
        {
            get
            {
                return this.SelectedValuePath ?? "Id";
            }
        }

        /// <summary>
        /// Gets display property name.
        /// </summary>
        private string DisplayPropertyName
        {
            get
            {
                return this.DisplayMemberPath ?? "Name";
            }
        }

        /// <summary>
        /// On apply template event handler.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.resultPopup = GetTemplateChild("resultPopup") as Popup;
            this.itemsPanel = GetTemplateChild("itemsPanel") as StackPanel;
            this.mainPanel = GetTemplateChild("mainPanel") as StackPanel;
            this.spinner = GetTemplateChild("spinner") as Spinner.Spinner;

            if (this.resultPopup == null)
            {
                // ReSharper disable once NotResolvedInText
                // throw new ArgumentNullException("Result popup is null.");
            }

            if (this.itemsPanel == null)
            {
                // ReSharper disable once NotResolvedInText
                // throw new ArgumentNullException("Items panel is null.");
            }

            if (this.mainPanel == null)
            {
                // ReSharper disable once NotResolvedInText
                // throw new ArgumentNullException("Main panel is null.");
            }

            // Fix issue with moving popup during the resizing.
            this.SizeChanged += (sender, args) =>
            {
                var offset = this.resultPopup.HorizontalOffset;
                this.resultPopup.HorizontalOffset = offset + 1;
                this.resultPopup.HorizontalOffset = offset;
            };
        }

        /// <summary>
        /// On got focus event handler.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            // Fix issue with visible text cursor after losing focus.
            FocusManager.SetIsFocusScope(this, true);
        }

        /// <summary>
        /// On lost focus event handler.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            this.OnEscape();

            // Fix issue with firing LostFocus event only once.
            FocusManager.SetIsFocusScope(this, false);

            this.spinner.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// On items source property changed.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="e">The event argument.</param>
        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Autocomplete;

            if (control != null)
            {
                control.OnItemsSourcePropertyChanged();
            }
        }

        /// <summary>
        /// On items source property changed.
        /// </summary>
        /// <param name="d">Dependency object.</param>
        /// <param name="e">The event argument.</param>
        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Autocomplete;

            if (control != null)
            {
                control.OnSelectedItemPropertyChanged();
            }
        }

        /// <summary>
        /// On key down event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event argument.</param>
        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            var control = sender as Autocomplete;

            if (control != null)
            {
                control.OnKeyDownHandler(e);
            }
        }

        /// <summary>
        /// On key up event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event argument.</param>
        private static void OnKeyUp(object sender, KeyEventArgs e)
        {
            var control = sender as Autocomplete;

            if (control != null)
            {
                control.OnKeyUpHandler(e);
            }
        }

        /// <summary>
        /// On key down event handler.
        /// </summary>
        /// <param name="e">The event argument.</param>
        private void OnKeyDownHandler(KeyEventArgs e)
        {
            if (e.Key == Key.Tab && this.resultPopup.IsOpen)
            {
                this.OnEnterTab();

                e.Handled = true;
            }
        }

        /// <summary>
        /// On key up event handler.
        /// </summary>
        /// <param name="e">The event argument.</param>
        private void OnKeyUpHandler(KeyEventArgs e)
        {
            // Handle Escape key.
            if (e.Key == Key.Escape)
            {
                this.OnEscape();

                return;
            }

            // Handle Enter keys.
            if (e.Key == Key.Enter)
            {
                this.OnEnterTab();

                return;
            }

            // Tab keys was handler on down event.
            if (e.Key == Key.Tab)
            {
                return;
            }

            // Handle Down & Up keys.
            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                this.OnDownUp(e.Key);

                return;
            }

            var context = TaskScheduler.FromCurrentSynchronizationContext();
            var text = this.Text;
            Task.Factory.StartNewWithDefaultCulture(() => this.FilterItems(text)).ContinueWith(_ => context);
        }

        /// <summary>
        /// On mouse up event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event argument.</param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var labelItem = sender as CustomLabel;

            if (labelItem != null)
            {
                this.SelectItem(labelItem.Item);
            }
        }

        /// <summary>
        /// On mouse enter event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event argument.</param>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            var labelItem = sender as CustomLabel;

            if (labelItem != null)
            {
                // Prevent hovering 2 items during the using
                // mouse and keys as the same time.
                foreach (var chid in this.itemsPanel.Children)
                {
                    var childLabel = chid as CustomLabel;
                    if (childLabel != null)
                    {
                        childLabel.IsMouseOver2 = false;
                    }
                }

                labelItem.IsMouseOver2 = true;
            }
        }

        /// <summary>
        /// On mouse leave event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event argument.</param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            var labelItem = sender as CustomLabel;

            if (labelItem != null)
            {
                labelItem.IsMouseOver2 = false;
            }
        }

        /// <summary>
        /// On Escape event handler.
        /// </summary>
        private void OnEscape()
        {
            if (this.SelectedItem == null)
            {
                this.Text = string.Empty;
            }
            else
            {
                var itemType = this.GetItemType(this.SelectedItem);
                var selectedItemId = itemType.GetProperty(this.IdPropertyName);
                var id = (int)selectedItemId.GetValue(this.SelectedItem, null);

                var selectedItem = this.InternalItems.FirstOrDefault(a => a.Id == id);
                if (selectedItem != null)
                {
                    this.Text = selectedItem.Name;
                }
            }

            this.resultPopup.IsOpen = false;
        }

        /// <summary>
        /// On Enter/Tab event handler.
        /// </summary>
        private void OnEnterTab()
        {
            InternalItem hoveredItem = null;

            var hoveredLabel = (from object c in this.itemsPanel.Children
                                let label = c as CustomLabel
                                where label != null && label.IsMouseOver2
                                select c).FirstOrDefault();

            // If some label hovered - select it.
            if (hoveredLabel != null)
            {
                var label = hoveredLabel as CustomLabel;
                if (label != null)
                {
                    hoveredItem = label.Item;
                }
            }

            // If any of labels hovered - select the first one.
            if (hoveredLabel == null && this.itemsPanel.Children.Count > 0)
            {
                var label = this.itemsPanel.Children[0] as CustomLabel;
                if (label != null)
                {
                    label.IsMouseOver2 = true;
                    hoveredItem = label.Item;
                }
            }

            if (hoveredItem != null)
            {
                this.SelectItem(hoveredItem);
            }
        }

        /// <summary>
        /// On Down/Up event handler.
        /// </summary>
        /// <param name="key">Pressed key.</param>
        private void OnDownUp(Key key)
        {
            var count = this.itemsPanel.Children.Count;

            var currect = -1;
            var nextIndex = 0;
            for (var i = 0; i < count; i++)
            {
                var label = this.itemsPanel.Children[i] as CustomLabel;
                if (label != null && label.IsMouseOver2)
                {
                    nextIndex = key == Key.Down ? i + 1 : i - 1;
                    currect = i;
                    if (nextIndex >= count)
                    {
                        nextIndex = 0;
                    }
                    else if (nextIndex < 0)
                    {
                        nextIndex = count - 1;
                    }
                }
            }

            if (this.itemsPanel.Children.Count > 0)
            {
                var nextLabel = this.itemsPanel.Children[nextIndex] as CustomLabel;
                if (nextLabel != null)
                {
                    nextLabel.IsMouseOver2 = true;
                }

                if (currect > -1)
                {
                    var currectLabel = this.itemsPanel.Children[currect] as CustomLabel;
                    if (currectLabel != null)
                    {
                        currectLabel.IsMouseOver2 = false;
                    }
                }
            }
        }

        /// <summary>
        /// Filter items by search text.
        /// </summary>
        /// <param name="text">The search text.</param>
        private void FilterItems(string text)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle,
                new Action(() =>
                {
                    this.spinner.Visibility = Visibility.Visible;
                }));

            Log.Debug("Going to filter internal items.");

            var filtereds = this.InternalItems
                            .Where(a => a.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                            .Take(10)
                            .ToList();

            Log.Debug("Internal items were filtered. Count: {0}.", Log.Args(filtereds.Count));

            this.Dispatcher.BeginInvoke(
                   DispatcherPriority.ApplicationIdle,
                   new Action(() =>
                   {
                       this.itemsPanel.Children.Clear();

                       Log.Debug("Result panel was cleared.");

                       if (filtereds.Any() && !string.IsNullOrEmpty(text))
                       {
                           this.resultPopup.IsOpen = true;

                           foreach (var filtered in filtereds)
                           {
                               var item = new CustomLabel
                               {
                                   Content = filtered.Name,
                                   Item = filtered
                               };

                               // IsMouseOver property is read only and since we need to have more control
                               // on hovered item, IsMouseOver behavior is handled manually.
                               item.MouseEnter += this.OnMouseEnter;
                               item.MouseLeave += this.OnMouseLeave;

                               item.MouseUp += this.OnMouseUp;
                               this.itemsPanel.Children.Add(item);
                           }

                           Log.Debug("Filtered items were added to the result panel. Count: {0}.", Log.Args(this.itemsPanel.Children.Count));
                       }
                       else
                       {
                           this.SelectedItem = null;
                           this.resultPopup.IsOpen = false;
                       }

                       this.spinner.Visibility = Visibility.Hidden;
                   }));
        }

        /// <summary>
        /// On items source property changed.
        /// </summary>
        private void OnItemsSourcePropertyChanged()
        {
            this.InternalItems = new List<InternalItem>();

            PropertyInfo idProperty = null;
            PropertyInfo nameProperty = null;

            var isFirstItem = true;

            Log.Debug("Items source was changed. Going to convert to the internal items.");

            foreach (var item in this.ItemsSource)
            {
                if (isFirstItem)
                {
                    var itemType = this.GetItemType(item);

                    idProperty = itemType.GetProperty(this.IdPropertyName);
                    nameProperty = itemType.GetProperty(this.DisplayPropertyName);

                    isFirstItem = false;
                }

                var id = idProperty.GetValue(item, null);
                var name = nameProperty.GetValue(item, null);

                this.InternalItems.Add(new InternalItem
                {
                    Id = (int)id,
                    Name = (string)name
                });
            }

            Log.Debug("Source items were converted to the internal items.");
        }

        /// <summary>
        /// On items source property changed.
        /// </summary>
        private void OnSelectedItemPropertyChanged()
        {
            // Initialize selected item.
            if (!this.isSelectedItemInitialized)
            {
                var selectedItemType = this.GetItemType(this.SelectedItem);
                var selectedItemIdProperty = selectedItemType.GetProperty(this.IdPropertyName);
                var selectedItemId = (int)selectedItemIdProperty.GetValue(this.SelectedItem, null);

                var selectedItem = this.InternalItems.FirstOrDefault(a => a.Id == selectedItemId);

                if (selectedItem != null)
                {
                    this.SelectItem(selectedItem, false);
                }

                this.isSelectedItemInitialized = true;
            }
        }

        /// <summary>
        /// Select item.
        /// </summary>
        /// <param name="internalItem">Item to select.</param>
        /// <param name="handleSelection">Is selected item has to be selected.</param>
        private void SelectItem(InternalItem internalItem, bool handleSelection = true)
        {
            this.Text = internalItem.Name;

            PropertyInfo idProperty = null;

            var isFirstItem = true;

            foreach (var item in this.ItemsSource)
            {
                if (isFirstItem)
                {
                    var itemType = this.GetItemType(item);

                    idProperty = itemType.GetProperty(this.IdPropertyName);
                    isFirstItem = false;
                }

                var id = idProperty.GetValue(item, null);

                if (id != null && (int)id == internalItem.Id)
                {
                    this.SelectedItem = item;
                    break;
                }
            }

            if (handleSelection)
            {
                // Close popup.
                this.resultPopup.IsOpen = false;

                // Clear results.
                this.itemsPanel.Children.Clear();

                // Unfocus textbox.
                this.mainPanel.Focus();
            }
        }

        /// <summary>
        /// Get item type.
        /// </summary>
        /// <param name="item">Object to get type.</param>
        /// <returns>Returns item type.</returns>
        private Type GetItemType(object item)
        {
            return this.sourceItemType ?? (this.sourceItemType = item.GetType());
        }
    }
}
