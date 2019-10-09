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
using Plexdata.GitHub.Accessor.Internals;
using System;

namespace Plexdata.GitHub.Accessor.Factories
{
    public static class AccessorFactory
    {
        #region Public methods

        public static IType Create<IType>(params Object[] parameters) where IType : class
        {
            if (typeof(IType) == typeof(IPublicRepositoryReader))
            {
                return AccessorFactory.CreatePublicRepositoryReader<IType>(parameters);
            }

            if (typeof(IType) == typeof(IPublicReleaseReader))
            {
                return AccessorFactory.CreatePublicReleaseReader<IType>(parameters);
            }

            throw new NotSupportedException($"Type \"{typeof(IType).Name}\" is not supported.");
        }

        #endregion

        #region Private methods

        private static IType CreatePublicRepositoryReader<IType>(Object[] parameters) where IType : class
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Length < 2 || parameters[0] == null || parameters[1] == null)
            {
                throw new ArgumentException(
                    "Creating this interface type requires two valid parameters. " +
                    "A user name or an organization name is expected as first parameter. " +
                    "As second parameter a Boolean is expected stating that the name is for organizations.",
                    nameof(parameters)
                );
            }

            if (parameters[0].GetType() != typeof(String))
            {
                throw new InvalidCastException($"Unable to convert value of \"{parameters[0]}\" into string.");
            }

            if (parameters[1].GetType() != typeof(Boolean))
            {
                throw new InvalidCastException($"Unable to convert value of \"{parameters[1]}\" into Boolean.");
            }

            return new PublicRepositoryReader(
                (String)Convert.ChangeType(parameters[0], typeof(String)),
                (Boolean)Convert.ChangeType(parameters[1], typeof(Boolean))
            ) as IType;
        }

        private static IType CreatePublicReleaseReader<IType>(Object[] parameters) where IType : class
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Length < 2 || parameters[0] == null || parameters[1] == null)
            {
                throw new ArgumentException(
                    "Creating this interface type requires two valid parameters. " +
                    "An owner name is expected as first parameter. " +
                    "As second parameter a repository name is expected.",
                    nameof(parameters)
                );
            }

            if (parameters[0].GetType() != typeof(String))
            {
                throw new InvalidCastException($"Unable to convert value of \"{parameters[0]}\" into string.");
            }

            if (parameters[1].GetType() != typeof(String))
            {
                throw new InvalidCastException($"Unable to convert value of \"{parameters[1]}\" into string.");
            }

            return new PublicReleaseReader(
                (String)Convert.ChangeType(parameters[0], typeof(String)),
                (String)Convert.ChangeType(parameters[1], typeof(String))
            ) as IType;
        }

        #endregion
    }
}
