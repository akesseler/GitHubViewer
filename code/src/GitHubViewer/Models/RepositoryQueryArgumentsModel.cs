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

using GitHubViewer.Enumerations;
using Plexdata.GitHub.Accessor.Arguments.Entities;
using System;
using System.ComponentModel;

namespace GitHubViewer.Models
{
    public class RepositoryQueryArgumentsModel : INotifyPropertyChanged
    {
        #region Public events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region  Private fields

        private RepositoryType repositoryType = 0;
        private String repositoryName = null;
        private Int32? countPerPage = null;
        private TypeArgument typeArgument;
        private SortArgument sortArgument;
        private DirectionArgument directionArgument;

        #endregion

        #region Construction

        public RepositoryQueryArgumentsModel()
        {
            this.repositoryType = 0;
            this.repositoryName = null;
            this.countPerPage = null;
            this.typeArgument = TypeArgument.Unused;
            this.sortArgument = SortArgument.Unused;
            this.directionArgument = DirectionArgument.Unused;
        }

        #endregion

        #region Public properties

        public Boolean IsOrganization
        {
            get
            {
                return this.repositoryType == RepositoryType.Organization;
            }
        }

        public RepositoryType RepositoryType
        {
            get
            {
                return this.repositoryType;
            }
            set
            {
                if (this.repositoryType != value)
                {
                    this.repositoryType = value;
                    this.RaisePropertyChanged(nameof(this.RepositoryType));
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

        public TypeArgument TypeArgument
        {
            get
            {
                return this.typeArgument;
            }
            set
            {
                if (this.typeArgument != value)
                {
                    this.typeArgument = value;
                    this.RaisePropertyChanged(nameof(this.TypeArgument));
                }
            }
        }

        public SortArgument SortArgument
        {
            get
            {
                return this.sortArgument;
            }
            set
            {
                if (this.sortArgument != value)
                {
                    this.sortArgument = value;
                    this.RaisePropertyChanged(nameof(this.SortArgument));
                }
            }
        }

        public DirectionArgument DirectionArgument
        {
            get
            {
                return this.directionArgument;
            }
            set
            {
                if (this.directionArgument != value)
                {
                    this.directionArgument = value;
                    this.RaisePropertyChanged(nameof(this.DirectionArgument));
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
