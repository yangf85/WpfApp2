using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    public class ScrollViewerAttacher
    {
        #region HorizontalOffset

        public static readonly DependencyProperty HorizontalOffsetProperty =
                    DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerAttacher), new PropertyMetadata(OnHorizontalOffsetChanged));

        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scroll)
            {
                scroll.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }

        public static double GetHorizontalOffset(ScrollViewer obj) => (double)obj.GetValue(HorizontalOffsetProperty);

        public static void SetHorizontalOffset(ScrollViewer obj, double value) => obj.SetValue(HorizontalOffsetProperty, value);

        #endregion HorizontalOffset

        #region VerticalOffset

        public static readonly DependencyProperty VerticalOffsetProperty =
                    DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerAttacher), new PropertyMetadata(OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scroll)
            {
                scroll.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        public static double GetVerticalOffset(ScrollViewer obj) => (double)obj.GetValue(VerticalOffsetProperty);

        public static void SetVerticalOffset(ScrollViewer obj, double value) => obj.SetValue(VerticalOffsetProperty, value);

        #endregion VerticalOffset
    }
}