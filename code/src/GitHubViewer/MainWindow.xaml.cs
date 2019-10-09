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
using GitHubViewer.Models;
using System;
using System.Windows;

namespace GitHubViewer
{
    public partial class MainWindow : Window, IStatusHandler
    {
        #region Construction

        public MainWindow()
        {
            this.GlobalStatus = new StatusModel();

            this.InitializeComponent();
            this.Style = (Style)FindResource(typeof(Window));

            this.mainStatusBar.DataContext = this.GlobalStatus;

            Shared.PanelManager.RegisterPanel(this.panelMainOverview);
            Shared.PanelManager.RegisterPanel(this.panelRepositoriesOverview);
            Shared.PanelManager.RegisterPanel(this.panelRepositoryDetails);
            Shared.PanelManager.RegisterPanel(this.panelReleasesOverview);
            Shared.PanelManager.RegisterPanel(this.panelReleaseDetails);
        }

        #endregion

        #region Public properties

        public StatusModel GlobalStatus { get; private set; }

        #endregion

        #region Private event handlers

        private void OnMenuExitClicked(Object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void OnMenuOverviewClicked(Object sender, RoutedEventArgs args)
        {
            Shared.PanelManager.ShowPanel(this.panelMainOverview);
        }

        private void OnMenuRepositoriesClicked(Object sender, RoutedEventArgs args)
        {
            Shared.PanelManager.ShowPanel(this.panelRepositoriesOverview);
        }

        private void OnMenuReleasesClicked(Object sender, RoutedEventArgs args)
        {
            Shared.PanelManager.ShowPanel(this.panelReleasesOverview);
        }

        #endregion
    }
}
