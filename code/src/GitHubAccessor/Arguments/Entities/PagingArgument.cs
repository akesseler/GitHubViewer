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
using Plexdata.GitHub.Accessor.Attributes;
using System;

namespace Plexdata.GitHub.Accessor.Arguments.Entities
{
    [QueryArgumentLabel("paging")]
    public class PagingArgument
    {
        #region  Internal fields

        internal const String IndexLabel = "page";

        internal const String CountLabel = "per_page";

        #endregion

        #region Construction

        public PagingArgument()
            : base()
        {
            this.Index = null;
            this.Count = null;
        }

        public PagingArgument(Int32? count)
            : base()
        {
            this.ValidateCount(count);
            this.Count = count;
        }

        public PagingArgument(Int32? index, Int32? count)
            : base()
        {
            this.ValidateCount(count);
            this.Count = count;
            this.Index = index;
        }

        public PagingArgument(IPaging paging)
            : base()
        {
            this.ValidateCount(paging?.Count);
            this.Index = paging?.Index;
            this.Count = paging?.Count;
        }

        #endregion

        #region Public properties

        [QueryArgumentValue(PagingArgument.IndexLabel)]
        public Int32? Index { get; protected set; }

        [QueryArgumentValue(PagingArgument.CountLabel)]
        public Int32? Count { get; protected set; }

        #endregion

        #region Private methods

        private void ValidateCount(Int32? count)
        {
            if (!count.HasValue)
            {
                return;
            }

            if (count.Value < 1 || count.Value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "The paging count must be within range of 1 up to 100.");
            }
        }

        #endregion
    }
}
