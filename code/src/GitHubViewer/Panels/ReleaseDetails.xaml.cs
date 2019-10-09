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
using GitHubViewer.Extensions;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace GitHubViewer.Panels
{
    public partial class ReleaseDetails : UserControl, IReleaseVisualizer
    {
        #region Private fields

        private IRelease release = null;

        #endregion

        #region Construction

        public ReleaseDetails()
            : base()
        {
            this.InitializeComponent();
            this.release = null;
        }

        #endregion

        #region Public properties

        public Object Requester { get; set; }

        public IRelease Release
        {
            get
            {
                return this.Release;
            }
            set
            {
                this.release = value;
                this.detailsContainer.Children.Clear();
                this.detailsContainer.Children.Add(this.release.ToElement());
            }
        }

        #endregion

        #region Private methods

        private void OnBackLabelMouseLeftButtonUp(Object sender, MouseButtonEventArgs args)
        {
            this.detailsViewer.ScrollToTop();
            Shared.PanelManager.ShowPanel(this.Requester);
        }

        #endregion
    }
}
