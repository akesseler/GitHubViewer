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
using System.Collections.Generic;

namespace Plexdata.GitHub.Accessor.Abstraction.Entities
{
    public interface IRelease
    {
        Int64 Id { get; }

        String NodeId { get; }

        String Name { get; }

        String Body { get; }

        String TagName { get; }

        String TargetCommitish { get; }

        String Url { get; }

        String HtmlUrl { get; }

        String AssetsUrl { get; }

        String UploadUrl { get; }

        String TarBallUrl { get; }

        String ZipBallUrl { get; }

        Boolean Draft { get; }

        Boolean PreRelease { get; }

        IOwner Author { get; }

        IEnumerable<IAsset> Assets { get; }

        DateTime CreatedAt { get; }

        DateTime PublishedAt { get; }
    }
}
