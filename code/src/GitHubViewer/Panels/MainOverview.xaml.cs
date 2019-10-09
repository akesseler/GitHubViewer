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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GitHubViewer.Panels
{
    public partial class MainOverview : UserControl
    {
        #region Public events

        public static readonly RoutedEvent ShowRepositoriesEvent = EventManager.RegisterRoutedEvent(
            "ShowRepositories", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainOverview));

        public static readonly RoutedEvent ShowReleasesEvent = EventManager.RegisterRoutedEvent(
            "ShowReleases", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainOverview));

        public event RoutedEventHandler ShowRepositories
        {
            add
            {
                base.AddHandler(MainOverview.ShowRepositoriesEvent, value);
            }
            remove
            {
                base.RemoveHandler(MainOverview.ShowRepositoriesEvent, value);
            }
        }

        public event RoutedEventHandler ShowReleases
        {
            add
            {
                base.AddHandler(MainOverview.ShowReleasesEvent, value);
            }
            remove
            {
                base.RemoveHandler(MainOverview.ShowReleasesEvent, value);
            }
        }

        #endregion

        #region Construction

        public MainOverview()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Private event handlers

        private void OnRepositoriesMouseLeftButtonUp(Object sender, MouseButtonEventArgs arga)
        {
            base.RaiseEvent(new RoutedEventArgs(MainOverview.ShowRepositoriesEvent, this));
        }

        private void OnReleasesMouseLeftButtonUp(Object sender, MouseButtonEventArgs arga)
        {
            base.RaiseEvent(new RoutedEventArgs(MainOverview.ShowReleasesEvent, this));
        }

        #endregion
    }
}
