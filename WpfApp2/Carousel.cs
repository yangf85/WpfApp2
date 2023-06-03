using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    [TemplatePart(Name = PART_PrevRepeatButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_NextRepeatButton, Type = typeof(RepeatButton))]
    [TemplatePart(Name = PART_IndexListBox, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_ScrollViewer, Type = typeof(ScrollViewer))]
    public class Carousel : ListBox
    {
        static Carousel()
        {
            PrevCommand = new RoutedCommand("Prev", typeof(Carousel));
            NextCommand = new RoutedCommand("Next", typeof(Carousel));

            CommandManager.RegisterClassCommandBinding(typeof(Carousel),
                new CommandBinding(PrevCommand, OnExecutePrevCommand, OnCanExecutePrevCommand));
            CommandManager.RegisterClassCommandBinding(typeof(Carousel),
               new CommandBinding(NextCommand, OnExecuteNextCommand, OnCanExecuteNextCommand));
        }

        public Carousel()
        {
            _animation = new DoubleAnimation()
            {
                Duration = AnimationDuration,
                EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseInOut },
            };
        }

        private const string PART_PrevRepeatButton = nameof(PART_PrevRepeatButton);

        private const string PART_NextRepeatButton = nameof(PART_NextRepeatButton);

        private const string PART_IndexListBox = nameof(PART_IndexListBox);

        private const string PART_ScrollViewer = nameof(PART_ScrollViewer);

        private ListBox _IndexListBox;

        private RepeatButton _PrevRepeatButton;

        private RepeatButton _NextRepeatButton;

        private ScrollViewer _ScrollViewer;

        #region ExtendObject

        public static readonly DependencyProperty ExtendObjectProperty =
            DependencyProperty.Register(nameof(ExtendObject), typeof(object), typeof(Carousel), new PropertyMetadata(default(object)));

        public object ExtendObject
        {
            get => (object)GetValue(ExtendObjectProperty);
            set => SetValue(ExtendObjectProperty, value);
        }

        #endregion ExtendObject

        #region AnimationDuration

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(Duration), typeof(Carousel), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(1000)), OnAnimationDurationChanged));

        public Duration AnimationDuration
        {
            get => (Duration)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        private static void OnAnimationDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Carousel carousel)
            {
                carousel._animation.Duration = (Duration)e.NewValue;
            }
        }

        #endregion AnimationDuration

        #region PrevCommand

        public static RoutedCommand PrevCommand { get; private set; }

        private static void OnCanExecutePrevCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var carousel = (Carousel)sender;

            e.CanExecute = carousel.SelectedIndex > 0;
        }

        private static void OnExecutePrevCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var carousel = (Carousel)sender;
            carousel.SelectedIndex -= 1;
        }

        #endregion PrevCommand

        #region NextCommand

        public static RoutedCommand NextCommand { get; private set; }

        private static void OnCanExecuteNextCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var carousel = (Carousel)sender;

            e.CanExecute = carousel.SelectedIndex > -1 && carousel.Items.Count > 0;
        }

        private static void OnExecuteNextCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var carousel = (Carousel)sender;
            carousel.SelectedIndex += 1;
        }

        #endregion NextCommand

        #region Animation

        private DoubleAnimation _animation;

        private void Animation(double from, double to, Action action)
        {
            if (_ScrollViewer == null)
            {
                return;
            }

            _animation.From = from;
            _animation.To = to;
            _ScrollViewer.BeginAnimation(ScrollViewerAttacher.HorizontalOffsetProperty, _animation);
            _animation.Completed += (s, e) =>
            {
                action?.Invoke();
            };
        }

        #endregion Animation

        #region Override

        private (int Start, int End) ScrollDirection(SelectionChangedEventArgs e)
        {
            var start = e.RemovedItems.Cast<object>().FirstOrDefault();
            var end = e.AddedItems.Cast<object>().FirstOrDefault();
            if (start == null || end == null)
            {
                return (0, 0);
            }
            return (Items.IndexOf(start), Items.IndexOf(end));
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (_ScrollViewer == null)
            {
                return;
            }

            var result = ScrollDirection(e);

            var offset = result.Start - result.End;
            if (offset == -1)
            {
                var item = ItemContainerGenerator.ContainerFromIndex(result.Start) as FrameworkElement;
                var from = _ScrollViewer.HorizontalOffset;
                var to = _ScrollViewer.HorizontalOffset + item.ActualWidth;
                Animation(from, to, () =>
                {
                    ScrollIntoView(SelectedItem);
                });
            }
            else if (offset == 1)
            {
                var item = ItemContainerGenerator.ContainerFromIndex(result.Start) as FrameworkElement;
                var from = _ScrollViewer.HorizontalOffset;
                var to = _ScrollViewer.HorizontalOffset - item.ActualWidth;
                Animation(from, to, () =>
                {
                    ScrollIntoView(SelectedItem);
                });
            }
            else
            {
                ScrollIntoView(SelectedItem);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CarouselItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CarouselItem;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _PrevRepeatButton = GetTemplateChild(PART_PrevRepeatButton) as RepeatButton;
            _NextRepeatButton = GetTemplateChild(PART_NextRepeatButton) as RepeatButton;
            _IndexListBox = GetTemplateChild(PART_IndexListBox) as ListBox;
            _ScrollViewer = GetTemplateChild(PART_ScrollViewer) as ScrollViewer;
        }

        #endregion Override
    }
}