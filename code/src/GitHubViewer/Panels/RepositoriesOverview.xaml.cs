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
using GitHubViewer.Events;
using GitHubViewer.Extensions;
using GitHubViewer.Models;
using Newtonsoft.Json;
using Plexdata.GitHub.Accessor.Abstraction;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Arguments;
using Plexdata.GitHub.Accessor.Arguments.Entities;
using Plexdata.GitHub.Accessor.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GitHubViewer.Panels
{
    public partial class RepositoriesOverview : UserControl, ILoadReleasesRequester
    {
        #region Public events

        public event LoadReleasesEventHandler LoadReleases;

        #endregion

        #region Private fields

        private RepositoryQueryArgumentsModel model = null;

        #endregion

        #region Construction

        public RepositoriesOverview()
        {
            this.InitializeComponent();
            this.ApplyPagings(null);

            // Data binding is a mystery.
            this.model = new RepositoryQueryArgumentsModel();
            this.DataContext = this.model;
        }

        #endregion

        #region Private methods

        private void ApplyResults(IEnumerable<IRepository> repositories)
        {
            if (repositories == null)
            {
                return;
            }

            foreach (IRepository repository in repositories)
            {
                Grid container = new Grid
                {
                    Background = Application.Current.Resources["DefaultControlBrush"] as SolidColorBrush,
                };

                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70, GridUnitType.Star) });
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10, GridUnitType.Star) });

                Thickness indent = new Thickness(10, 5, 5, 5);

                this.AddHeader(repository, container, indent, 0, 0);

                this.AddForked(repository, container, indent, 0, 1);

                this.AddStars(repository, container, indent, 0, 2);

                this.AddLanguage(repository, container, indent, 0, 3);

                this.AddDetails(repository, container, indent, 1, 0);

                this.AddLicense(repository, container, indent, 2, 0);

                this.AddTopics(repository, container, indent, 3, 0);

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

        private void AddHeader(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
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
                ToolTip = "Click to show all repository details...",
                FontSize = this.repositoryViewer.FontSize * 1.5,
                FontWeight = FontWeights.SemiBold,
            };

            header.MouseLeftButtonUp += this.OnHeaderMouseLeftButtonUp;

            panel.Children.Add(header);

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

        private void AddForked(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            Label label = new Label
            {
                Padding = indent,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = this.repositoryViewer.FontSize * 1.25,
                Content = source.Fork ? "\u16D8" : String.Empty,
                ToolTip = source.Fork ? "This repository has been forked." : String.Empty,
            };

            parent.Children.Add(label);

            Grid.SetRow(label, row);
            Grid.SetColumn(label, col);
        }

        private void AddStars(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            Label label = new Label
            {
                Padding = indent,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = String.Format("\u2605 {0}", source.StargazersCount),
                ToolTip = String.Format("This repository has a stargazers count of {0}.", source.StargazersCount),
            };

            parent.Children.Add(label);

            Grid.SetRow(label, row);
            Grid.SetColumn(label, col);
        }

        private void AddLanguage(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            Label label = new Label
            {
                Padding = indent,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Content = source.Language,
            };

            parent.Children.Add(label);

            Grid.SetRow(label, row);
            Grid.SetColumn(label, col);
        }

        private void AddDetails(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            if (String.IsNullOrWhiteSpace(source.Description))
            {
                return;
            }

            Label label = new Label
            {
                Padding = indent,
                FontSize = this.repositoryViewer.FontSize * 1.25,
                Content = new TextBlock()
                {
                    Text = source.Description,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                }
            };

            parent.Children.Add(label);

            Grid.SetRow(label, row);
            Grid.SetColumn(label, col);
            Grid.SetColumnSpan(label, parent.ColumnDefinitions.Count);
        }

        private void AddLicense(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            if (source.License == null || String.IsNullOrWhiteSpace(source.License.Name))
            {
                return;
            }

            Label label = new Label
            {
                Padding = indent,
                Content = source.License.Name
            };

            parent.Children.Add(label);

            Grid.SetRow(label, row);
            Grid.SetColumn(label, col);
            Grid.SetColumnSpan(label, parent.ColumnDefinitions.Count);
        }

        private void AddTopics(IRepository source, Grid parent, Thickness indent, Int32 row, Int32 col)
        {
            if (source.Topics == null || !source.Topics.Any())
            {
                return;
            }

            StackPanel panel = new StackPanel
            {
                Margin = indent,
                Orientation = Orientation.Horizontal,
            };

            foreach (String topic in source.Topics)
            {
                Label label = new Label
                {
                    Content = topic,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                    ToolTip = $"Topic: \"{topic}\"",
                };

                Border border = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Application.Current.Resources["DefaultBorderBrush"] as SolidColorBrush,
                    Background = Application.Current.Resources["DefaultControlBrushDarker"] as SolidColorBrush,
                    CornerRadius = new CornerRadius(3),
                    Margin = new Thickness(0, 0, 5, 0),
                    Padding = new Thickness(6, 4, 6, 5),
                    Child = label
                };

                panel.Children.Add(border);
            }

            parent.Children.Add(panel);

            Grid.SetRow(panel, row);
            Grid.SetColumn(panel, col);
            Grid.SetColumnSpan(panel, parent.ColumnDefinitions.Count);
        }

        private void ApplyPagings(IPagination pagination)
        {
            this.paginationContainer.ApplyPagings(pagination, this.OnPagingButtonClick);
        }

        private ContextMenu GetContextMenu(IRepository source)
        {
            ContextMenu menu = new ContextMenu()
            {
                Style = Application.Current.Resources["TripleDotsDropContextMenu"] as Style,
            };

            MenuItem item;

            item = new MenuItem()
            {
                Header = "Load Releases...",
                Tag = source,
                Style = Application.Current.Resources["TripleDotsDropMenuItem"] as Style,
                ToolTip = "Request release details and show them in release view.",
            };
            item.Click += this.OnLoadReleasesClick;
            item.IsEnabled = !source.Fork;
            menu.Items.Add(item);

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

        private void OnLoadReleasesClick(Object sender, RoutedEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRepository repository)
            {
                this.LoadReleases?.Invoke(this, new LoadReleasesEventArgs(repository));
            }
        }

        private void OnShowDetailsClick(Object sender, RoutedEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRepository repository)
            {
                this.ExecuteShowDetails(repository);
            }
        }

        private void OnCopyClipboardClick(Object sender, RoutedEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRepository repository)
            {
                try
                {
                    Clipboard.Clear();
                    Clipboard.SetText(JsonConvert.SerializeObject(repository, Formatting.Indented), TextDataFormat.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToMessage(), "Oops");
                }
            }
        }

        private void OnHeaderMouseLeftButtonUp(Object sender, MouseButtonEventArgs args)
        {
            if (sender is FrameworkElement source && source.Tag is IRepository repository)
            {
                this.ExecuteShowDetails(repository);
            }
        }

        private async void OnLoadRepositoriesClick(Object sender, RoutedEventArgs args)
        {
            try
            {
                RepositoryQueryArguments arguments = new RepositoryQueryArguments(new PagingArgument(this.model.CountPerPage))
                {
                    Type = this.model.TypeArgument,
                    Sort = this.model.SortArgument,
                    Direction = this.model.DirectionArgument
                };

                await this.ExecuteLoadRepositoriesAsync(arguments);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToMessage(), "Oops");
            }
        }

        private async void OnPagingButtonClick(Object sender, RoutedEventArgs args)
        {
            try
            {
                if (sender is Button button)
                {
                    RepositoryQueryArguments arguments = new RepositoryQueryArguments(new PagingArgument(button.Tag as Int32?, this.model.CountPerPage))
                    {
                        Type = this.model.TypeArgument,
                        Sort = this.model.SortArgument,
                        Direction = this.model.DirectionArgument
                    };

                    await this.ExecuteLoadRepositoriesAsync(arguments);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToMessage(), "Oops");
            }
        }

        #endregion

        #region Private request handlers

        private void ExecuteShowDetails(IRepository repository)
        {
            Shared.PanelManager.ShowDetails(this, repository);
        }

        private async Task ExecuteLoadRepositoriesAsync(RepositoryQueryArguments arguments)
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.AppStarting;

            try
            {
                this.resultContainer.Children.Clear();

                IPublicRepositoryReader accessor = AccessorFactory.Create<IPublicRepositoryReader>(this.model.RepositoryName, this.model.IsOrganization);

                IResult<IRepository> result = await accessor.ReadAsync(arguments);

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
