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

namespace GitHubViewer.Helpers
{
    public static class PagingButtonHelper
    {
        #region Public constant fields

        public const String PagingNumberButton = "PagingNumberButton";

        public const String DoubleLeftArrowButton = "DoubleLeftArrowButton";

        public const String SingleLeftArrowButton = "SingleLeftArrowButton";

        public const String SingleRightArrowButton = "SingleRightArrowButton";

        public const String DoubleRightArrowButton = "DoubleRightArrowButton";

        #endregion

        #region Public methods

        public static Button GetButton(String resource, Boolean enabled)
        {
            return new Button
            {
                IsEnabled = enabled,
                Style = PagingButtonHelper.GetStyle(resource),
            };
        }

        public static Button GetButton(Int32 number, Boolean enabled)
        {
            return new Button
            {
                Content = new TextBlock()
                {
                    Text = number.ToString(),
                },
                IsEnabled = enabled,
                Style = PagingButtonHelper.GetStyle(PagingButtonHelper.PagingNumberButton),
            };
        }

        #endregion

        #region Private methods

        private static Style GetStyle(String resource)
        {
            return Application.Current.Resources[resource] as Style;
        }

        #endregion
    }
}
