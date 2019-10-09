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
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Internals.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Plexdata.GitHub.Accessor.Internals.Entities
{
    internal class Limitation : ILimitation
    {
        #region Private classes

        private class LimitationBody
        {
            [JsonProperty("message")]
            public String Description { get; set; }

            [JsonProperty("documentation_url")]
            public String DocumentationUrl { get; set; }
        }

        #endregion

        #region Private fields

        private readonly LimitationBody limitation = null;

        #endregion

        #region Construction

        internal Limitation(HttpResponseMessage response)
            : base()
        {
            this.IsLimited = false;

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (response.Headers.Contains("x-ratelimit-limit"))
            {
                this.Maximum = Convert.ToInt32(response.Headers.GetValues("x-ratelimit-limit").FirstOrDefault());
            }

            if (response.Headers.Contains("x-ratelimit-remaining"))
            {
                this.Remaining = Convert.ToInt32(response.Headers.GetValues("x-ratelimit-remaining").FirstOrDefault());

                if (this.Remaining <= 0)
                {
                    this.IsLimited = true;
                    this.limitation = this.GetLimitationBody(response);
                }
            }

            if (response.Headers.Contains("x-ratelimit-reset"))
            {
                this.Expiration = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(response.Headers.GetValues("x-ratelimit-reset").FirstOrDefault()));
            }
        }

        #endregion

        #region Public properties

        public Boolean IsLimited { get; private set; }

        public Int32 Maximum { get; private set; }

        public Int32 Remaining { get; private set; }

        public DateTimeOffset Expiration { get; private set; }

        public String Description
        {
            get
            {
                return this.limitation?.Description;
            }
        }

        public String DocumentationUrl
        {
            get
            {
                return this.limitation?.DocumentationUrl;
            }
        }

        #endregion

        #region Public methods

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(64);

            builder.Append($"{nameof(this.Maximum)}: {this.Maximum.ToDisplay()}, ");
            builder.Append($"{nameof(this.Remaining)}: {this.Remaining.ToDisplay()}, ");
            builder.Append($"{nameof(this.Expiration)}: {this.Expiration.ToDisplay()}");

            return builder.ToString();
        }

        #endregion

        #region Private methods

        private LimitationBody GetLimitationBody(HttpResponseMessage response)
        {
            LimitationBody result = null;

            try
            {
                Task.Run(async () =>
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore, };
                    result = JsonConvert.DeserializeObject<LimitationBody>(await response.Content.ReadAsStringAsync(), settings);
                })
                .Wait();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return result;
        }

        #endregion
    }
}
