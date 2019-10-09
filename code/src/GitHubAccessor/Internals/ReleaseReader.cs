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

using Newtonsoft.Json;
using Plexdata.GitHub.Accessor.Abstraction;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Arguments;
using Plexdata.GitHub.Accessor.Exceptions;
using Plexdata.GitHub.Accessor.Internals.Converters;
using Plexdata.GitHub.Accessor.Internals.Entities;
using Plexdata.GitHub.Accessor.Internals.Extensions;
using Plexdata.GitHub.Accessor.Internals.Parsers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Plexdata.GitHub.Accessor.Internals
{
    // SEE: https://developer.github.com/v3/repos/releases/

    internal abstract class ReleaseReader : Reader<IRelease, ReleaseQueryArguments>, IReleaseReader
    {
        #region Construction

        public ReleaseReader() : base() { }

        public ReleaseReader(String host) : base(host) { }

        #endregion

        #region Public methods

        public override async Task<IResult<IRelease>> ReadAsync()
        {
            return await this.ReadAsync(new ReleaseQueryArguments());
        }

        public override async Task<IResult<IRelease>> ReadAsync(ReleaseQueryArguments arguments)
        {
            // How to use this SonarQube rule right here?
            // https://rules.sonarsource.com/csharp/tag/async-await/RSPEC-4457

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            // BUG: Be aware, pagination may apply to all request types (but not yet investigated)!
            // You may read here: https://developer.github.com/v3/#link-header

            using (HttpClient client = new HttpClient())
            using (HttpRequestMessage request = this.GetRequestMessage(HttpMethod.Get, arguments))
            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    ReleaseResult result = new ReleaseResult();

                    base.ApplyCommonResults(response, arguments, result);

                    JsonSerializerSettings settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore, };
                    settings.Converters.Add(new AssetConverter());
                    settings.Converters.Add(new OwnerConverter());

                    result.Results = JsonConvert.DeserializeObject<IEnumerable<Release>>(await response.Content.ReadAsStringAsync(), settings);

                    return result;
                }

                throw new AccessorRequestException(response);
            }
        }

        public async Task<Int32> TotalCountAsync()
        {
            // Under the condition of using a "per_page" count of 1 and then taking 
            // the number of received last page is indeed a trick (or better a hack) 
            // but it solves the problem of getting the "total number of something". 
            // But be aware this trick should only be used together with method HEAD. 
            // Otherwise at least one record is returned by the GitHub API!

            ReleaseQueryArguments arguments = new ReleaseQueryArguments
            {
                Paging = new Paging(null, 1)
            };

            using (HttpClient client = new HttpClient())
            using (HttpRequestMessage request = this.GetRequestMessage(HttpMethod.Head, arguments))
            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                // TODO: Handle limitation results

                if (response.IsSuccessStatusCode)
                {
                    if (PaginationParser.TryParse(response, out Pagination pagination))
                    {
                        return pagination.Last.ToIndex();
                    }

                    return 0;
                }

                throw new AccessorRequestException(response);
            }
        }

        #endregion

        #region Protected methods

        protected abstract String GetRelativeUri();

        protected abstract IList<Type> GetExcludedArguments();

        #endregion

        #region Private methods

        private HttpRequestMessage GetRequestMessage(HttpMethod method, ReleaseQueryArguments arguments)
        {
            UriBuilder builder = new UriBuilder(new Uri(base.Host, this.GetRelativeUri()));

            builder.AddQueryValues(arguments.AddArguments(new Dictionary<String, String>(), this.GetExcludedArguments()));

            return base.GetRequestMessage(method, builder.Uri);
        }

        #endregion
    }
}
