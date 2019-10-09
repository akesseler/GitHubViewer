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

using Plexdata.GitHub.Accessor.Abstraction;
using Plexdata.GitHub.Accessor.Arguments.Entities;
using System;
using System.Collections.Generic;

namespace Plexdata.GitHub.Accessor.Internals
{
    internal class PublicRepositoryReader : RepositoryReader, IPublicRepositoryReader
    {
        #region Private fields

        private readonly String value;

        private readonly Boolean organization;

        #endregion

        #region Construction

        public PublicRepositoryReader(String value, Boolean organization)
            : base()
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The name value must not be null or empty or only consists of white spaces.", nameof(value));
            }

            this.value = value.Trim().Trim('/').Trim();
            this.organization = organization;
        }

        #endregion

        #region Protected methods

        protected override String GetRelativeUri()
        {
            if (this.organization)
            {
                // GET /orgs/:org/repos
                return $"/orgs/{this.value}/repos";
            }
            else
            {
                // GET /users/:username/repos
                return $"/users/{this.value}/repos";
            }
        }

        protected override IList<Type> GetExcludedArguments()
        {
            // NOTE: It's the same exclude list in both cases.
            return new List<Type>() { typeof(VisibilityArgument), typeof(AffiliationArgument) };
        }

        #endregion
    }
}
