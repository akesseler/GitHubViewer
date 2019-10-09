/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using GitHubViewer.Abstraction;
using GitHubViewer.Controls;
using GitHubViewer.Extensions;
using GitHubViewer.Models;
using Newtonsoft.Json;
using Plexdata.Formatters;
using Plexdata.GitHub.Accessor.Abstraction;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Arguments;
using Plexdata.GitHub.Accessor.Arguments.Entities;
using Plexdata.GitHub.Accessor.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GitHubViewer.Panels
{
    public partial class ReleasesOverview : UserControl, ILoadReleasesProcessor
    {
        #region Private fields

        private ReleaseQueryArgumentsModel model = null;
        private CapacityFormatter capacity = null;
        private Object requester = null;

        #endregion

        #region Construction

        public ReleasesOverview()
        {
            this.InitializeComponent();
            this.ApplyPagings(null);

            this.capacity = new CapacityFormatter(new CultureInfo("en-US"));

            // Data binding is a mystery.
            this.model = new ReleaseQueryArgumentsModel();
            this.DataContext = this.model;
        }

        #endregion

        #region Public methods 

        public void LoadReleases(Object requester, IRepository repository)
        {
            if (repository == null) { return; }

            try
            {
                this.requester = requester;
                this.backLabel.Visibility = Visibility.Visible;

                String[] pieces = repository.FullName.Split('/');

                if (pieces.Length > 0)
                {
                    this.model.OwnerName = pieces[0];
                }

                if (pieces.Length > 1)
                {
                    this.model.RepositoryName = pieces[1];
                }

                this.OnLoadReleasesClick(this, new RoutedEventArgs());
            }
            catch (Exception exception)
            {
                this.requester = null;
                this.backLabel.Visibility = Visibility.Collapsed;

                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        #endregion

        #region Private methods

        private void ApplyResults(IEnumerable<IRelease> releases)
        {
            if (releases == null)
            {
                return;
            }

            foreach (IRelease release in releases)
            {
                Grid container = new Grid
                {
                    Background = Application.Current.Resources["DefaultControlBrush"] as SolidColorBrush,
                };

                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Star) });

                Thickness indent = new Thickness(10, 5, 5, 5);

                this.AddHeader(release, container, indent, 0, 0);

                this.AddDetails(release, container, indent, 1, 0);

                this.AddBody(release, container, indent, 2, 0);

                this.AddAssets(release, container, indent, 3, 0);

                Border border = new Border
                {
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = Application.Current.Resources["DefaultBorderBrush"] as SolidColorBrush,
                    Child = container,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                this.resultContainer.Children.Add(border);
            }
        }

        private void AddHeader(IRelease source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            TextBlock header = new TextBlock()
            {
                Padding = indent,
                Cursor = Cursors.Hand,
                Foreground = Brushes.Blue,
                TextDecorations = TextDecorations.Underline,
                Text = source.Name,
                Tag = source,
                ToolTip = "Click to show all release details...",
                FontSize = this.repositoryViewer.FontSize * 1.5,
                FontWeight = FontWeights.SemiBold,
            };

            header.MouseLeftButtonUp += this.OnHeaderMouseLeftButtonUp;

            panel.Children.Add(header);

            if (!String.IsNullOrWhiteSpace(source.TagName))
            {
                TextBlock version = new TextBlock()
                {
                    Padding = indent,
                    Text = source.TagName,
                    FontSize = this.repositoryViewer.FontSize * 1.5,
                    FontWeight = FontWeights.SemiBold,
                };

                panel.Children.Add(version);
            }

            DropDownButton button = new DropDownButton
            {
                Style = Application.Current.Resources["TripleDotsDropDownButton"] as Style,
                DropDown = this.GetContextMenu(source),
                Margin = new Thickness(10, 0, 0, 0),
                Padding = new Thickness(3),
                Width = 30,
                Height = 30
            };

            panel.Children.Add(button);

            parent.Children.Add(panel);

            Grid.SetRow(panel, row);
            Grid.SetColumn(panel, col);
        }

        private void AddDetails(IRelease source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            Dictionary<String, Object> details = new Dictionary<String, Object>
            {
                { nameof(source.Author),      source.Author.Login }, // Intentionally, null not checked!
                { nameof(source.CreatedAt),   source.CreatedAt    },
                { nameof(source.PublishedAt), source.PublishedAt  },
                { nameof(source.Draft),       source.Draft        },
                { nameof(source.PreRelease),  source.PreRelease   },
            };

            Grid panel = new Grid();

            panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            Int32 index = 0;

            foreach (KeyValuePair<String, Object> detail in details)
            {
                panel.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                UIElement label = detail.Key.ToLabelElement(indent);

                panel.Children.Add(label);

                Grid.SetRow(label, index);
                Grid.SetColumn(label, 0);

                UIElement value = detail.Value.ToValueElement(indent);

                panel.Children.Add(value);

                Grid.SetRow(value, index);
                Grid.SetColumn(value, 1);

                index++;
            }

            parent.Children.Add(panel);

            Grid.SetRow(panel, row);
            Grid.SetColumn(panel, col);
        }

        private void AddBody(IRelease source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            if (String.IsNullOrWhiteSpace(source.Body))
            {
                return;
            }

            UIElement value = source.Body.ToValueElement(indent);

            parent.Children.Add(value);

            Grid.SetRow(value, row);
            Grid.SetColumn(value, col);
        }

        private void AddAssets(IRelease source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            if (!source.Assets.Any())
            {
                return;
            }

            StackPanel caddy = new StackPanel()
            {
                Background = Application.Current.Resources["DefaultControlBrush"] as SolidColorBrush,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            Int32 line = 0;

            foreach (IAsset asset in source.Assets)
            {
                if (asset == null) { continue; }

                Grid panel = new Grid();

                panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                Dictionary<String, (Object, String, IFormatProvider, String)> details = new Dictionary<String, (Object, String, IFormatProvider, String)>
                {
                    { nameof(asset.Size),               ( asset.Size,               "{0:two~2}", this.capacity, String.Format(this.capacity, "{0:bytes}", asset.Size) ) },
                    { nameof(asset.DownloadCount),      ( asset.DownloadCount,      null,        null,          null                                                  ) },
                    { nameof(asset.BrowserDownloadUrl), ( asset.BrowserDownloadUrl, null,        null,          null                                                  ) },
                    { nameof(asset.CreatedAt),          ( asset.CreatedAt,          null,        null,          null                                                  ) },
                    { nameof(asset.UpdatedAt),          ( asset.UpdatedAt,          null,        null,          null                                                  ) },
                };

                Int32 index = 0;

                foreach (KeyValuePair<String, (Object Value, String Format, IFormatProvider Provider, String Tooltip)> detail in details)
                {
                    panel.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                    UIElement label = detail.Key.ToLabelElement(indent);

                    panel.Children.Add(label);

                    Grid.SetRow(label, index);
                    Grid.SetColumn(label, 0);

                    Object current = detail.Value.Value;

                    if (detail.Value.Provider != null && detail.Value.Format != null && detail.Value.Value is IFormattable)
                    {
                        current = String.Format(detail.Value.Provider, detail.Value.Format, detail.Value.Value);
                    }

                    UIElement value = current.ToValueElement(indent, detail.Value.Tooltip);

                    panel.Children.Add(value);

                    Grid.SetRow(value, index);
                    Grid.SetColumn(value, 1);

                    index++;
                }

                Expander expander = new Expander()
                {
                    Header = $"{asset.Name.ToDisplayValue()} ({asset.DownloadCount.ToDisplayValue()})",
                    Template = Application.Current.Resources["NiceExpanderTemplate"] as ControlTemplate,
                    Margin = indent,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Content = panel,
                };

                caddy.Children.Add(expander);

                Grid.SetRow(expander, line);
                Grid.SetColumn(expander, 0);

                line++;
            }

            parent.Children.Add(caddy);

            Grid.SetRow(caddy, row);
            Grid.SetColumn(caddy, col);
        }

        private void ApplyPagings(IPagination pagination)
        {
            this.paginationContainer.ApplyPagings(pagination, this.OnPagingButtonClick);
        }

        private ContextMenu GetContextMenu(IRelease source)
        {
            ContextMenu menu = new ContextMenu()
            {
                Style = Application.Current.Resources["TripleDotsDropContextMenu"] as Style,
            };

            MenuItem item;

            item = new MenuItem()
            {
                Header = "Show Details...",
                Tag = source,
                Style = Application.Current.Resources["TripleDotsDropMenuItem"] as Style,
                ToolTip = "Open details view and show all content.",
            };
            item.Click += this.OnShowDetailsClick;
            menu.Items.Add(item);

            item = new MenuItem()
            {
                Header = "Copy to Clipboard",
                Tag = source,
                Style = Application.Current.Resources["TripleDotsDropMenuItem"] as Style,
                ToolTip = "Copy the whole object data into the clipboard.",
            };
            item.Click += this.OnCopyClipboardClick;
            menu.Items.Add(item);

            return menu;
        }

        #endregion

        #region Private event handlers

        private async void OnLoadReleasesClick(Object sender, RoutedEventArgs args)
        {
            try
            {
                ReleaseQueryArguments arguments = new ReleaseQueryArguments(new PagingArgument(this.model.CountPerPage));

                await this.ExecuteLoadReleasesAsync(arguments);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToMessage(), "Oops");
            }
        }

        private void OnShowDetailsClick(Object sender, RoutedEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRelease release)
            {
                this.ExecuteShowDetails(release);
            }
        }

        private void OnCopyClipboardClick(Object sender, RoutedEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRelease release)
            {
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetText(JsonConvert.SerializeObject(release, Formatting.Indented), TextDataFormat.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToMessage(), "Oops");
                }
            }
        }

        private void OnHeaderMouseLeftButtonUp(Object sender, MouseButtonEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRelease release)
            {
                this.ExecuteShowDetails(release);
            }
        }

        private void OnBackLabelMouseLeftButtonUp(Object sender, MouseButtonEventArgs args)
        {
            this.backLabel.Visibility = Visibility.Collapsed;
            Shared.PanelManager.ShowPanel(this.requester);
            this.requester = null;
        }

        private async void OnPagingButtonClick(Object sender, RoutedEventArgs args)
        {
            try
            {
                if (sender is Button button)
                {
                    ReleaseQueryArguments arguments = new ReleaseQueryArguments(new PagingArgument(button.Tag as Int32?, this.model.CountPerPage));

                    await this.ExecuteLoadReleasesAsync(arguments);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToMessage(), "Oops");
            }
        }

        #endregion

        #region Private request handlers

        private void ExecuteShowDetails(IRelease release)
        {
            Shared.PanelManager.ShowDetails(this, release);
        }

        private async Task ExecuteLoadReleasesAsync(ReleaseQueryArguments arguments)
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            try
            {
                this.resultContainer.Children.Clear();

                IPublicReleaseReader accessor = AccessorFactory.Create<IPublicReleaseReader>(this.model.OwnerName, this.model.RepositoryName);

                IResult<IRelease> result = await accessor.ReadAsync(arguments);

                if (result != null)
                {
                    this.ApplyResults(result.Results);
                    this.ApplyPagings(result.Pagination);

                    if (result.Limitation != null)
                    {
                        Shared.StatusHandler.GlobalStatus.LimitationMaximum = result.Limitation.Maximum;
                        Shared.StatusHandler.GlobalStatus.LimitationRemaining = result.Limitation.Remaining;
                        Shared.StatusHandler.GlobalStatus.LimitationExpiration = result.Limitation.Expiration.DateTime;
                    }
                }
            }
            finally
            {
                this.Cursor = cursor;
            }
        }

        #endregion
    }
}
