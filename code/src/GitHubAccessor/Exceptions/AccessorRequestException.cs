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
using Plexdata.GitHub.Accessor.Internals.Entities;
using Plexdata.GitHub.Accessor.Internals.Extensions;
using Plexdata.GitHub.Accessor.Internals.Parsers;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Plexdata.GitHub.Accessor.Exceptions
{
    [Serializable]
    public class AccessorRequestException : AccessorException
    {
        #region Construction

        public AccessorRequestException() : base() { }

        public AccessorRequestException(String message) : base(message) { }

        public AccessorRequestException(String message, Exception exception) : base(message, exception) { }

        internal AccessorRequestException(HttpResponseMessage response)
            : base(response.ToDisplay())
        {
            if (response != null)
            {
                this.Status = response.StatusCode;
                this.Reason = response.ReasonPhrase;
                this.Request = response.RequestMessage.ToString();

                if (LimitationParser.TryParse(response, out Limitation limitation))
                {
                    this.Limitation = limitation;
                }
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected AccessorRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Public properties

        public HttpStatusCode Status { get; private set; }

        public String Reason { get; private set; }

        public String Request { get; private set; }

        public ILimitation Limitation { get; private set; }

        #endregion

        #region Public methods

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue(nameof(this.Status), this.Status);
            info.AddValue(nameof(this.Reason), this.Reason);
            info.AddValue(nameof(this.Request), this.Request);

            if (this.Limitation != null)
            {
                info.AddValue(nameof(this.Limitation.IsLimited), this.Limitation.IsLimited);
                info.AddValue(nameof(this.Limitation.Maximum), this.Limitation.Maximum);
                info.AddValue(nameof(this.Limitation.Remaining), this.Limitation.Remaining);
                info.AddValue(nameof(this.Limitation.Expiration), this.Limitation.Expiration);
                info.AddValue(nameof(this.Limitation.Description), this.Limitation.Description);
                info.AddValue(nameof(this.Limitation.DocumentationUrl), this.Limitation.DocumentationUrl);
            }

            base.GetObjectData(info, context);
        }

        #endregion
    }
}
