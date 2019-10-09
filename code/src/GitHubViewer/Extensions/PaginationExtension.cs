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

using GitHubViewer.Helpers;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GitHubViewer.Extensions
{
    internal static class PaginationExtension
    {
        #region Public methods

        public static void ApplyPagings(this StackPanel container, IPagination pagination, RoutedEventHandler pagingButtonClickHandler)
        {
            // [<<] [<] [1] [2] .. [n] [>] [>>] 

            IPaging first = pagination?.First;
            IPaging previous = pagination?.Previous;
            IPaging next = pagination?.Next;
            IPaging last = pagination?.Last;

            container.Children.Clear();

            Int32 start = 1;
            Int32 count = PaginationExtension.GetPageCount(pagination);
            Button button = null;

            button = PagingButtonHelper.GetButton(PagingButtonHelper.DoubleLeftArrowButton, first != null);
            button.Tag = first != null ? first.Index : (Int32?)start;
            button.Click += pagingButtonClickHandler;
            container.Children.Add(button);

            button = PagingButtonHelper.GetButton(PagingButtonHelper.SingleLeftArrowButton, previous != null);
            button.Tag = previous != null ? previous.Index : (Int32?)start;
            button.Click += pagingButtonClickHandler;
            container.Children.Add(button);

            // BUG: Number of paging buttons is not yet limited to a maximum value and missing buttons are not yet replaced by for example triple-dots.
            for (Int32 index = start; index <= count; index++)
            {
                Boolean enabled = pagination != null;

                if (pagination != null && pagination.Current != null)
                {
                    enabled = pagination.Current != null && pagination.Current.Index.HasValue && pagination.Current.Index.Value != index;
                }

                button = PagingButtonHelper.GetButton(index, enabled);
                button.Tag = index;
                button.Click += pagingButtonClickHandler;
                container.Children.Add(button);
            }

            button = PagingButtonHelper.GetButton(PagingButtonHelper.SingleRightArrowButton, next != null);
            button.Tag = next != null ? next.Index : (Int32?)count;
            button.Click += pagingButtonClickHandler;
            container.Children.Add(button);

            button = PagingButtonHelper.GetButton(PagingButtonHelper.DoubleRightArrowButton, last != null);
            button.Tag = last != null ? last.Index : (Int32?)count;
            button.Click += pagingButtonClickHandler;
            container.Children.Add(button);
        }

        #endregion

        #region Private methods

        private static Int32 GetPageCount(IPagination pagination)
        {
            if (pagination == null)
            {
                return 1;
            }

            if (pagination.Last != null)
            {
                if (pagination.Last.Index.HasValue)
                {
                    // The index of last page relation defines the number of total pages.
                    return pagination.Last.Index.Value;
                }
                else
                {
                    return 1;
                }
            }

            // This piece of code serves as fallback in case of reaching "last" page while 
            // stepping through all pages. It happens when calling the last page. In this 
            // case the "last" page relation is null and we must guess the total page count 
            // from the "previous" page relation. For sure, this is pretty ugly but necessary.

            if (pagination.Previous != null)
            {
                if (pagination.Previous.Index.HasValue)
                {
                    // The index of (previous + 1) page relation defines the number of total pages.
                    return pagination.Previous.Index.Value + 1;
                }
                else
                {
                    return 1;
                }
            }

            return 1;
        }

        #endregion
    }
}
