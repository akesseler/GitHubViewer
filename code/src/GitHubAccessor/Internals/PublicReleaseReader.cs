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
using System;
using System.Collections.Generic;

namespace Plexdata.GitHub.Accessor.Internals
{
    internal class PublicReleaseReader : ReleaseReader, IPublicReleaseReader
    {
        #region Private fields

        private readonly String owner;

        private readonly String repository;

        #endregion

        #region Construction

        public PublicReleaseReader(String owner, String repository)
            : base()
        {
            if (String.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("The owner must not be null or empty or only consists of white spaces.", nameof(owner));
            }

            if (String.IsNullOrWhiteSpace(repository))
            {
                throw new ArgumentException("The repository must not be null or empty or only consists of white spaces.", nameof(repository));
            }

            this.owner = owner.Trim().Trim('/').Trim();
            this.repository = repository.Trim().Trim('/').Trim();
        }

        #endregion

        #region Protected methods

        protected override String GetRelativeUri()
        {
            // GET /repos/:owner/:repo/releases

            return $"/repos/{this.owner}/{this.repository}/releases";
        }

        protected override IList<Type> GetExcludedArguments()
        {
            // NOTE: At the moment, nothing to exclude.
            return new List<Type>();
        }

        #endregion
    }
}
