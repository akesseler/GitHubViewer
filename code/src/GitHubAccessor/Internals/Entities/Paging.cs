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
using Plexdata.GitHub.Accessor.Arguments.Entities;
using Plexdata.GitHub.Accessor.Internals.Extensions;
using System;
using System.Text;

namespace Plexdata.GitHub.Accessor.Internals.Entities
{
    internal class Paging : PagingArgument, IPaging
    {
        #region Private fields

        private const Char AmpersandOperator = '&';

        private const Char EqualsOperator = '=';

        #endregion

        #region Construction

        public Paging()
            : this(null)
        {
        }

        public Paging(Int32? index)
            : this(index, null)
        {
        }

        public Paging(Int32? index, Int32? count)
            : base()
        {
            base.Index = index;
            base.Count = count;
        }

        internal Paging(String relation, Uri reference)
            : base()
        {
            this.SetRelation(relation);
            this.SetReference(reference);
        }

        #endregion

        #region Public properties

        public String Relation { get; private set; }

        public Uri Reference { get; private set; }

        public String Name { get; private set; }

        #endregion

        #region Public methods

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(64);

            builder.Append($"{nameof(this.Index)}: {this.Index.ToDisplay()}, ");
            builder.Append($"{nameof(this.Count)}: {this.Count.ToDisplay()}");

            return builder.ToString();
        }

        #endregion

        #region Private methods

        private void SetRelation(String relation)
        {
            if (String.IsNullOrWhiteSpace(relation))
            {
                throw new ArgumentException(nameof(relation));
            }

            this.Relation = relation;

            String[] values = relation.Split(Paging.EqualsOperator);

            if (values.Length > 1)
            {
                String name = values[1].Trim().Trim('"').Trim();

                if (String.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException(String.Format("{0}->{1}", nameof(relation), nameof(this.Name)).ToLower());
                }

                this.Name = name;
            }
        }

        private void SetReference(Uri reference)
        {
            this.Reference = reference ?? throw new ArgumentNullException(nameof(reference));

            String[] parameters = reference.Query.TrimStart('?').Split(Paging.AmpersandOperator);

            foreach (String parameter in parameters)
            {
                String[] values = parameter.Split(Paging.EqualsOperator);

                if (values.Length > 1)
                {
                    String label = values[0].Trim();

                    if (String.Compare(label, Paging.IndexLabel, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        base.Index = Convert.ToInt32(values[1].Trim());
                    }
                    else if (String.Compare(label, Paging.CountLabel, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        base.Count = Convert.ToInt32(values[1].Trim());
                    }
                }
            }
        }

        #endregion
    }
}
