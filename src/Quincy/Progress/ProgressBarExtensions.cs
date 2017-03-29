using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Quincy.Progress
{
    public class ProgressBarExtensions
    {
        /// <summary>
        ///     Dependency property to set the IProgressReporter of the progress bar
        /// </summary>
        public static readonly DependencyProperty ProgressReporterProperty = DependencyProperty.RegisterAttached("ProgressReporter", typeof(IProgressReporter),
            typeof(ProgressBarExtensions),
            new PropertyMetadata(OnProgressReporterPropertyChanged));

        private static readonly IDictionary<ProgressBar, IDisposable> Subscriptions;

        static ProgressBarExtensions()
        {
            Subscriptions = new ConcurrentDictionary<ProgressBar, IDisposable>();
        }

        /// <summary>
        ///     Sets the IProgressReporter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetProgressReporter(UIElement element, IProgressReporter value)
        {
            element.SetValue(ProgressReporterProperty, value);
        }

        /// <summary>
        ///     Gets the IProgressReporter.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IProgressReporter GetProgressReporter(UIElement element)
        {
            return (IProgressReporter) element.GetValue(ProgressReporterProperty);
        }

        private static void OnProgressReporterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var progressBar = d as ProgressBar;
            if (progressBar == null)
                return;

            var progressReporter = e.OldValue as IProgressReporter;
            if (progressReporter != null)
            {
                IDisposable subscription;
                Subscriptions.TryGetValue(progressBar, out subscription);
                subscription?.Dispose();
            }

            progressReporter = e.NewValue as IProgressReporter;
            if (progressReporter != null)
            {
                var subscription = progressReporter
                    .Subscribe(state =>
                    {
                        progressBar.Visibility = state.Enabled ? Visibility.Visible : Visibility.Hidden;
                        progressBar.IsIndeterminate = state.Indeterminate;
                        progressBar.Maximum = state.MaximumProgress;
                        progressBar.Value = state.CurrentProgress;
                    });
                Subscriptions.Add(progressBar, subscription);
            }
        }
    }
}