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
using GitHubViewer.Events;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GitHubViewer
{
    public partial class ViewerApplication : Application, IPanelManager
    {
        #region Private fields

        private readonly List<Object> panels;

        #endregion

        #region Construction

        public ViewerApplication()
            : base()
        {
            this.panels = new List<Object>();
        }

        #endregion

        #region Public methods

        public void RegisterPanel(Object panel)
        {
            if (panel == null)
            {
                return;
            }

            if (!this.panels.Contains(panel))
            {
                this.panels.Add(panel);

                if (panel is ILoadReleasesRequester)
                {
                    (panel as ILoadReleasesRequester).LoadReleases += this.OnViewerApplicationLoadReleases;
                }
            }
        }

        public void UnregisterPanel(Object panel)
        {
            if (panel == null)
            {
                return;
            }

            if (this.panels.Contains(panel))
            {
                if (panel is ILoadReleasesRequester)
                {
                    (panel as ILoadReleasesRequester).LoadReleases -= this.OnViewerApplicationLoadReleases;
                }

                this.panels.Remove(panel);
            }
        }

        public void ShowPanel(Object panel)
        {
            if (panel == null)
            {
                return;
            }

            foreach (Object current in this.panels)
            {
                Visibility visibility = Visibility.Collapsed;

                if (current.Equals(panel))
                {
                    visibility = Visibility.Visible;
                }

                if (current is UIElement element)
                {
                    element.Visibility = visibility;
                }
            }
        }

        public void ShowDetails(Object requester, IRepository repository)
        {
            if (requester == null || repository == null)
            {
                return;
            }

            foreach (Object panel in this.panels)
            {
                if (panel is IRepositoryVisualizer visualizer)
                {
                    visualizer.Requester = requester;
                    visualizer.Repository = repository;

                    this.ShowPanel(visualizer);
                }
            }
        }

        public void ShowDetails(Object requester, IRelease release)
        {
            if (requester == null || release == null)
            {
                return;
            }

            foreach (Object panel in this.panels)
            {
                if (panel is IReleaseVisualizer visualizer)
                {
                    visualizer.Requester = requester;
                    visualizer.Release = release;

                    this.ShowPanel(visualizer);
                }
            }
        }

        #endregion

        #region Private event handlers

        private void OnViewerApplicationLoadReleases(Object sender, LoadReleasesEventArgs args)
        {
            foreach (Object panel in this.panels)
            {
                if (panel is ILoadReleasesProcessor)
                {
                    this.ShowPanel(panel);

                    (panel as ILoadReleasesProcessor).LoadReleases(sender, args.Repository);
                }
            }
        }

        #endregion
    }
}
