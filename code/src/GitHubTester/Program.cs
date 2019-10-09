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
using Plexdata.GitHub.Accessor.Arguments.Entities;
using Plexdata.GitHub.Accessor.Factories;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plexdata.GitHub.Tester
{
    class Program
    {
        static void Main(String[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            try
            {
                #region IRepository

                //Program.TestRepositories("akesseler", false);

                #endregion

                #region IRelease

                //Program.TestReleases("akesseler", "QuickCopy");

                #endregion
            }
            catch (AggregateException exception)
            {
                foreach (Exception inner in exception.InnerExceptions)
                {
                    Console.WriteLine(inner.Message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Hit any key...");
            Console.ReadKey();
        }

        private static void TestReleases(String owner, String repository)
        {
            IPublicReleaseReader accessor = AccessorFactory.Create<IPublicReleaseReader>(owner, repository);

            IResult<IRelease> result = null;

            Task.Run(async () =>
            {
                ReleaseQueryArguments arguments = new ReleaseQueryArguments(new PagingArgument(50));

                while (true)
                {
                    result = await accessor.ReadAsync(arguments);

                    if (!result.Results.Any())
                    {
                        break;
                    }

                    Console.WriteLine(result);

                    foreach (IRelease release in result.Results)
                    {
                        Console.WriteLine(release);
                    }

                    Console.WriteLine($"count: {result.Results.Count()}");

                    if (result.Pagination == null || result.Pagination.Next == null)
                    {
                        break;
                    }

                    arguments.Paging = new PagingArgument(result.Pagination.Next);
                }
            })
            .Wait();
        }

        private static void TestRepositories(String value, Boolean organization)
        {
            IPublicRepositoryReader accessor = AccessorFactory.Create<IPublicRepositoryReader>(value, organization);

            IResult<IRepository> result = null;

            Task.Run(async () =>
            {
                var count = await accessor.TotalCountAsync();

                Console.WriteLine($"total count: {count}");

                RepositoryQueryArguments arguments = new RepositoryQueryArguments(new PagingArgument(50));

                while (true)
                {
                    result = await accessor.ReadAsync(arguments);

                    if (!result.Results.Any())
                    {
                        break;
                    }

                    Console.WriteLine(result);

                    foreach (IRepository repository in result.Results)
                    {
                        Console.WriteLine(repository);
                    }

                    Console.WriteLine($"count: {result.Results.Count()}");

                    if (result.Pagination == null || result.Pagination.Next == null)
                    {
                        break;
                    }

                    arguments.Paging = new PagingArgument(result.Pagination.Next);
                }

                Console.WriteLine($"total count: {count}");
            })
            .Wait();
        }
    }
}
