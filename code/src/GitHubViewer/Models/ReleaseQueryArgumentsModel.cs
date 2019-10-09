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
using System.ComponentModel;

namespace GitHubViewer.Models
{
    public class ReleaseQueryArgumentsModel : INotifyPropertyChanged
    {
        #region Public events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region  Private fields

        private String ownerName = null;
        private String repositoryName = null;
        private Int32? countPerPage = null;

        #endregion

        #region Construction

        public ReleaseQueryArgumentsModel()
        {
            this.ownerName = null;
            this.repositoryName = null;
            this.countPerPage = null;
        }

        #endregion

        #region Public properties

        public String OwnerName
        {
            get
            {
                return this.ownerName;
            }
            set
            {
                if (this.ownerName != value)
                {
                    this.ownerName = value;
                    this.RaisePropertyChanged(nameof(this.OwnerName));
                }
            }
        }

        public String RepositoryName
        {
            get
            {
                return this.repositoryName;
            }
            set
            {
                if (this.repositoryName != value)
                {
                    this.repositoryName = value;
                    this.RaisePropertyChanged(nameof(this.RepositoryName));
                }
            }
        }

        public Int32? CountPerPage
        {
            get
            {
                return this.countPerPage;
            }
            set
            {
                if (this.countPerPage != value)
                {
                    this.countPerPage = value;
                    this.RaisePropertyChanged(nameof(this.CountPerPage));
                }
            }
        }

        #endregion

        #region Private methods

        private void RaisePropertyChanged(String property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}

