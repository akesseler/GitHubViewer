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
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Arguments;
using Plexdata.GitHub.Accessor.Internals.Entities;
using Plexdata.GitHub.Accessor.Internals.Parsers;
using Plexdata.GitHub.Viewer;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Plexdata.GitHub.Accessor.Internals
{
    internal abstract class Reader<TResult, TArguments> : IReader<TResult, TArguments>
    {
        #region Construction

        public Reader()
            : this("https://api.github.com")
        {
        }

        public Reader(String host)
        {
            if (String.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException($"Argument '{nameof(host)}' must not null or empty, or consists only of white spaces.", nameof(host));
            }

            if (!Uri.TryCreate(host, UriKind.Absolute, out Uri temp))
            {
                throw new ArgumentException($"Could not convert value '{host}' of argument {nameof(host)} into a valid URL.", nameof(host));
            }

            // This is actually required, because otherwise a web-exception is thrown 
            // stating "The request was aborted: Could not create SSL/TLS secure channel."
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            this.Host = temp;
            this.IncludeApiPreviews = true;
        }

        #endregion

        #region Public properties

        public Uri Host { get; private set; }

        public Boolean IncludeApiPreviews { get; set; }

        #endregion

        #region Public methods

        public abstract Task<IResult<TResult>> ReadAsync();

        public abstract Task<IResult<TResult>> ReadAsync(TArguments arguments);

        #endregion

        #region Protected methods

        protected HttpRequestMessage GetRequestMessage(HttpMethod method, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            HttpRequestMessage result = new HttpRequestMessage(method, uri);

            // Required for GitHub-API!
            UserAgentHelper.AddUserAgent(result);

            // Accept: application/vnd.github.v3+json
            result.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            if (this.IncludeApiPreviews)
            {
                // Needed to receive "topics" information (seems to be experimental).
                result.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.mercy-preview+json"));

                // Needed to receive "template" information (seems to be experimental).
                result.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.baptiste-preview+json"));
            }

            // TODO: Add more header information, if needed.

            return result;
        }

        protected void ApplyCommonResults(HttpResponseMessage response, QueryArguments arguments, RequestResult result)
        {
            if (LimitationParser.TryParse(response, out Limitation limitation))
            {
                result.Limitation = limitation;
            }

            if (PaginationParser.TryParse(response, out Pagination pagination))
            {
                // Pagination workaround. Because of the fact that working with pagination 
                // is a bit more complicated as actually expected, it becomes necessary to 
                // give back the provided "current" page to the caller. For that reason, take 
                // requested page and put it into the returned pagination settings.
                pagination.Current = new Paging(arguments.Paging?.Index ?? 1, arguments.Paging.Count);

                result.Pagination = pagination;
            }
        }

        #endregion
    }
}
