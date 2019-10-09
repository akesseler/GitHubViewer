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

using Plexdata.GitHub.Accessor.Internals.Entities;
using System;
using System.Net.Http;

namespace Plexdata.GitHub.Accessor.Internals.Parsers
{
    internal static class PaginationParser
    {
        #region Private fields

        private static readonly Char LineSeparator = ',';

        private static readonly Char PartSeparator = ';';

        private static readonly Char[] HtmlBrackets = new Char[] { '<', '>' };

        #endregion

        #region Public methods

        public static Boolean TryParse(HttpResponseMessage response, out Pagination pagination)
        {
            pagination = null;

            try
            {
                if (response != null && response.Headers.Contains("link"))
                {
                    return PaginationParser.TryParse(String.Join(PaginationParser.LineSeparator.ToString(), response.Headers.GetValues("link")), out pagination);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return false;
        }

        public static Boolean TryParse(String input, out Pagination pagination)
        {
            pagination = null;

            try
            {
                if (!String.IsNullOrWhiteSpace(input))
                {
                    // According to GitHub docs: The Link header value contains a comma-separated list of URL and TYPE relations
                    String[] lines = input.Split(PaginationParser.LineSeparator);

                    foreach (String line in lines)
                    {
                        // According to GitHub docs: Each of the Link parts is separated by a semicolon.
                        String[] parts = line.Split(PaginationParser.PartSeparator);

                        if (pagination == null)
                        {
                            pagination = new Pagination();
                        }

                        pagination.ApplyPaging(PaginationParser.ToValue(parts));
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return pagination != null;
        }

        #endregion

        #region  Private methods

        private static Paging ToValue(String[] parts)
        {
            if (parts == null || parts.Length < 2)
            {
                throw new ArgumentException(nameof(parts));
            }

            // According to GitHub docs: Each Link part contains as first item the URL reference (enclosed in <,>) 
            // and as second item the name of the belonging relation. Against all of these conditions, it's necessary 
            // to remove all of the annoying characters as well as to bring them into the "right order".

            String relation = parts[1].Trim();
            Uri reference = new Uri(parts[0].Trim().Trim(PaginationParser.HtmlBrackets));

            return new Paging(relation, reference);
        }

        #endregion
    }
}
