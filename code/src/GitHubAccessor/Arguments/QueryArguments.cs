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
using Plexdata.GitHub.Accessor.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plexdata.GitHub.Accessor.Arguments
{
    public abstract class QueryArguments
    {
        #region Construction

        public QueryArguments()
            : this(null)
        {
        }

        public QueryArguments(PagingArgument paging)
               : base()
        {
            this.Separator = ",";
            this.Paging = paging;
        }

        #endregion

        #region Public properties

        public String Separator { get; set; }

        public PagingArgument Paging { get; set; }

        #endregion

        #region Internal methods

        internal abstract IDictionary<String, String> AddArguments(IDictionary<String, String> values);

        internal abstract IDictionary<String, String> AddArguments(IDictionary<String, String> values, IList<Type> excluded);

        #endregion

        #region Protected methods

        protected Boolean IsExcluded<TType>(IList<Type> excluded)
        {
            return excluded != null && excluded.Contains(typeof(TType));
        }

        protected void TryAddArgument(IDictionary<String, String> values, String label, String value)
        {
            if (values != null && !String.IsNullOrWhiteSpace(label) && !String.IsNullOrWhiteSpace(value) && !values.ContainsKey(label))
            {
                values.Add(label, value);
            }
        }

        protected void TryAddArgument<TType>(IDictionary<String, String> values, String label, String value, IList<Type> excluded)
        {
            if (!this.IsExcluded<TType>(excluded))
            {
                this.TryAddArgument(values, label, value);
            }
        }

        protected void TryAddArgument<TType>(IDictionary<String, String> values, TType value, IList<Type> excluded)
        {
            if (value != null && !this.IsExcluded<TType>(excluded))
            {
                if (typeof(TType).IsEnum)
                {
                    this.TryAddArgument(values, this.GetEnumArgumentLabel<TType>(), this.GetEnumArgumentValue<TType>(value));
                }
                else if (typeof(TType).IsClass)
                {
                    foreach (PropertyInfo property in typeof(TType).GetProperties())
                    {
                        this.TryAddArgument(values, this.GetClassArgumentLabel<TType>(property), this.GetClassArgumentValue<TType>(property, value));
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private Boolean IsFlagsAttribute<TType>()
        {
            return typeof(TType).IsEnum && typeof(TType).GetCustomAttributes(typeof(FlagsAttribute), false).FirstOrDefault() is FlagsAttribute;
        }

        private String GetEnumArgumentValues<TType>(IList<TType> values)
        {
            List<String> result = new List<String>();

            if (typeof(TType).IsEnum && values != null && values.Any())
            {
                foreach (TType value in values)
                {
                    MemberInfo member = typeof(TType)
                        .GetMember(value.ToString())
                        .FirstOrDefault();

                    if (member != null)
                    {
                        QueryArgumentValueAttribute attribute = member
                            .GetCustomAttributes(typeof(QueryArgumentValueAttribute), false)
                            .FirstOrDefault() as QueryArgumentValueAttribute;

                        if (attribute != null && !String.IsNullOrWhiteSpace(attribute.Name))
                        {
                            result.Add(attribute.Name);
                        }
                    }
                }
            }

            return String.Join(this.Separator, result);
        }

        private String GetEnumArgumentLabel<TType>()
        {
            QueryArgumentLabelAttribute attribute = typeof(TType)
                .GetCustomAttributes(typeof(QueryArgumentLabelAttribute), false)
                .FirstOrDefault() as QueryArgumentLabelAttribute;

            return attribute?.Name;
        }

        private String GetEnumArgumentValue<TType>(TType value)
        {
            if (this.IsFlagsAttribute<TType>())
            {
                Int32 temp = Convert.ToInt32(value);

                List<TType> flags = Enum.GetValues(typeof(TType))
                    .Cast<Int32>()
                    .Where(flag => (flag & temp) == flag)
                    .Cast<TType>()
                    .ToList();

                return this.GetEnumArgumentValues(flags);
            }
            else
            {
                return this.GetEnumArgumentValues(new List<TType>() { value });
            }
        }

        private String GetClassArgumentLabel<TType>(PropertyInfo property)
        {
            QueryArgumentValueAttribute attribute = property
                .GetCustomAttributes(typeof(QueryArgumentValueAttribute), false)
                .FirstOrDefault() as QueryArgumentValueAttribute;

            return attribute?.Name;
        }

        private String GetClassArgumentValue<TType>(PropertyInfo property, TType value)
        {
            return (property.GetValue(value) ?? String.Empty).ToString();
        }

        #endregion
    }
}
