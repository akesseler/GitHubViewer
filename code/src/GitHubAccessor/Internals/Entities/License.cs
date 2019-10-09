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
using System.Text;

namespace Plexdata.GitHub.Accessor.Internals.Entities
{
    internal class License : ILicense
    {
        #region Public properties

        [JsonProperty("key")]
        public String Key { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("spdx_id")]
        public String SpdxId { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("node_id")]
        public String NodeId { get; set; }

        #endregion

        #region Public methods

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(128);

            builder.AppendFormat("{0}: \"{1}\", ", nameof(this.Name), this.Name.ToDisplay());
            builder.AppendFormat("{0}: \"{1}\", ", nameof(this.Url), this.Url.ToDisplay());

            return builder.ToString().TrimEnd(' ', ',');
        }

        #endregion
    }
}
