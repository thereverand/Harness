using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Harness.Behaviors
{
    public class DropDownButtonBehavior : Behavior<Button>
    {
        private bool _isContextMenuOpen;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
        }

        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var source = sender as Button;
            if (source?.ContextMenu == null) return;
            if (_isContextMenuOpen) return;
            // Add handler to detect when the ContextMenu closes
            source.ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed), true);
            // If there is a drop-down assigned to this button, then position and display it
            source.ContextMenu.PlacementTarget = source;
            source.ContextMenu.Placement = PlacementMode.Bottom;
            source.ContextMenu.IsOpen = true;
            _isContextMenuOpen = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            _isContextMenuOpen = false;
            var contextMenu = sender as ContextMenu;
            contextMenu?.RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed));
        }
    }
}