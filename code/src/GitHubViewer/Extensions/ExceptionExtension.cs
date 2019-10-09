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

using Plexdata.GitHub.Accessor.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubViewer.Extensions
{
    internal static class ExceptionExtension
    {
        #region Public methods

        public static String ToMessage(this Exception exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            if (exception is AccessorRequestException)
            {
                return ExceptionExtension.ToMessage(exception as AccessorRequestException);
            }

            if (exception is AggregateException)
            {
                return ExceptionExtension.ToMessage(exception as AggregateException);
            }

            return exception.Message;
        }

        #endregion

        #region Private methods

        private static String ToMessage(AccessorRequestException exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            StringBuilder builder = new StringBuilder(256);

            builder.AppendLine(exception.Message);

            if (exception.Limitation != null && exception.Limitation.IsLimited)
            {
                if (!String.IsNullOrWhiteSpace(exception.Limitation.Description))
                {
                    builder.AppendLine();
                    builder.AppendLine(exception.Limitation.Description);
                }

                if (!String.IsNullOrWhiteSpace(exception.Limitation.DocumentationUrl))
                {
                    builder.AppendLine();
                    builder.AppendLine($"See documentation at \"{exception.Limitation.DocumentationUrl}\".");
                }
            }

            return builder.ToString().TrimEnd();
        }

        private static String ToMessage(AggregateException exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            List<String> messages = new List<String>();

            foreach (Exception current in (exception as AggregateException).InnerExceptions)
            {
                messages.Add(current.ToMessage());
            }

            return String.Join(Environment.NewLine, messages);
        }

        #endregion
    }
}
