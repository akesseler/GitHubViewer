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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Plexdata.GitHub.Accessor.Internals.Extensions
{
    internal static class FormatExtension
    {
        #region Private fields

        private static String NullString = "<null>";

        private static String EmptyString = "<empty>";

        #endregion

        #region Public methods

        public static String ToDisplay(this String value)
        {
            if (value == null)
            {
                return FormatExtension.NullString;
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                return FormatExtension.EmptyString;
            }

            return value;
        }

        public static String ToDisplay(this Object value)
        {
            if (value == null)
            {
                return FormatExtension.NullString;
            }

            if (value is IEnumerable)
            {
                if ((value as IEnumerable<Object>).Any())
                {
                    return $"{(value as IEnumerable<Object>).Count()}";
                }

                return FormatExtension.EmptyString;
            }

            return value.ToString();
        }

        public static String ToDisplay(this HttpResponseMessage message)
        {
            if (message == null)
            {
                return FormatExtension.NullString;
            }

            return $"{message.StatusCode} ({Convert.ChangeType(message.StatusCode, message.StatusCode.GetTypeCode()).ToDisplay()}): \"{message.ReasonPhrase.ToDisplay()}\"";
        }

        #endregion
    }
}
