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

using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plexdata.GitHub.Accessor.Internals.Entities
{
    internal class Pagination : IPagination
    {
        #region Private fields

        private readonly Dictionary<String, Paging> pagings = null;

        private const String FirstLabel = "first";

        private const String PrevioustLabel = "prev";

        private const String NextLabel = "next";

        private const String LastLabel = "last";

        #endregion

        #region Construction

        public Pagination()
            : base()
        {
            this.pagings = new Dictionary<String, Paging>(StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Public properties

        public IPaging First { get { return this.GetPage(Pagination.FirstLabel); } }

        public IPaging Previous { get { return this.GetPage(Pagination.PrevioustLabel); } }

        public IPaging Current { get; set; }

        public IPaging Next { get { return this.GetPage(Pagination.NextLabel); } }

        public IPaging Last { get { return this.GetPage(Pagination.LastLabel); } }

        #endregion

        #region Public methods

        public void ApplyPaging(Paging paging)
        {
            if (paging == null)
            {
                return;
            }

            if (this.pagings.ContainsKey(paging.Name))
            {
                this.pagings[paging.Name] = paging;
            }
            else
            {
                this.pagings.Add(paging.Name, paging);
            }
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(256);

            builder.Append($"{nameof(this.First)}: [{this.First.ToDisplay()}], ");
            builder.Append($"{nameof(this.Previous)}: [{this.Previous.ToDisplay()}], ");
            builder.Append($"{nameof(this.Current)}: [{this.Current.ToDisplay()}], ");
            builder.Append($"{nameof(this.Next)}: [{this.Next.ToDisplay()}], ");
            builder.Append($"{nameof(this.Last)}: [{this.Last.ToDisplay()}]");

            return builder.ToString();
        }

        #endregion

        #region Private methods

        private Paging GetPage(String name)
        {
            if (this.pagings.ContainsKey(name))
            {
                return this.pagings[name];
            }

            return null;
        }

        #endregion
    }
}
