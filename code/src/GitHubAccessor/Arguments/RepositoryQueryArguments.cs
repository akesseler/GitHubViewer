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

using Plexdata.GitHub.Accessor.Arguments.Entities;
using System;
using System.Collections.Generic;

namespace Plexdata.GitHub.Accessor.Arguments
{
    public class RepositoryQueryArguments : QueryArguments
    {
        #region Construction

        public RepositoryQueryArguments()
            : this(null)
        {
        }

        public RepositoryQueryArguments(PagingArgument paging)
            : base(paging)
        {
            this.Visibility = VisibilityArgument.Unused;
            this.Affiliation = AffiliationArgument.Unused;
            this.Type = TypeArgument.Unused;
            this.Sort = SortArgument.Unused;
            this.Direction = DirectionArgument.Unused;
        }

        #endregion

        #region Public properties

        public VisibilityArgument Visibility { get; set; }

        public AffiliationArgument Affiliation { get; set; }

        public TypeArgument Type { get; set; }

        public SortArgument Sort { get; set; }

        public DirectionArgument Direction { get; set; }

        #endregion

        #region  Internal methods

        internal override IDictionary<String, String> AddArguments(IDictionary<String, String> values)
        {
            return this.AddArguments(values, null);
        }

        internal override IDictionary<String, String> AddArguments(IDictionary<String, String> values, IList<Type> excluded)
        {
            base.TryAddArgument<VisibilityArgument>(values, this.Visibility, excluded);
            base.TryAddArgument<AffiliationArgument>(values, this.Affiliation, excluded);
            base.TryAddArgument<TypeArgument>(values, this.Type, excluded);
            base.TryAddArgument<SortArgument>(values, this.Sort, excluded);
            base.TryAddArgument<DirectionArgument>(values, this.Direction, excluded);

            base.TryAddArgument<PagingArgument>(values, this.Paging, excluded);

            return values;
        }

        #endregion
    }
}
