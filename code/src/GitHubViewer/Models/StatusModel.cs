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
    public class StatusModel : INotifyPropertyChanged
    {
        #region Public events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private fields

        private Int32 limitationMaximum = 0;

        private Int32 limitationRemaining = 0;

        private DateTime limitationExpiration = DateTime.Now.AddHours(1);

        #endregion

        #region Public properties

        public Int32 LimitationMaximum
        {
            get
            {
                return this.limitationMaximum;
            }
            set
            {
                if (this.limitationMaximum != value)
                {
                    this.limitationMaximum = value;
                    this.RaisePropertyChanged(nameof(this.LimitationMaximum));
                    this.RaisePropertyChanged(nameof(this.LimitationMaximumTooltip));
                }
            }
        }

        public String LimitationMaximumTooltip
        {
            get
            {
                return $"The maximum number of requests you're permitted to make per hour is {this.LimitationMaximum}.";
            }
        }

        public Int32 LimitationRemaining
        {
            get
            {
                return this.limitationRemaining;
            }
            set
            {
                if (this.limitationRemaining != value)
                {
                    this.limitationRemaining = value;
                    this.RaisePropertyChanged(nameof(this.LimitationRemaining));
                    this.RaisePropertyChanged(nameof(this.LimitationRemainingTooltip));
                }
            }
        }

        public String LimitationRemainingTooltip
        {
            get
            {
                return $"The number of requests remaining in the current rate limit window is {this.LimitationRemaining}.";
            }
        }

        public DateTime LimitationExpiration
        {
            get
            {
                return this.limitationExpiration.ToLocalTime();
            }
            set
            {
                if (this.limitationExpiration != value)
                {
                    this.limitationExpiration = value;
                    this.RaisePropertyChanged(nameof(this.LimitationExpiration));
                    this.RaisePropertyChanged(nameof(this.LimitationExpirationooltip));

                }
            }
        }

        public String LimitationExpirationooltip
        {
            get
            {
                return $"The time at which the current rate limit window resets is {this.LimitationExpiration.ToString("yyyy-MM-dd HH:mm:ss")}.";
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
