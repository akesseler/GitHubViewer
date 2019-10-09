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
    public interface IRepository
    {
        Int64 Id { get; }

        String NodeId { get; }

        String Name { get; }

        String FullName { get; }

        IOwner Owner { get; }

        Boolean Private { get; }

        String HtmlUrl { get; }

        String Description { get; }

        Boolean Fork { get; }

        String Url { get; }

        String ArchiveUrl { get; }

        String AssigneesUrl { get; }

        String BlobsUrl { get; }

        String BranchesUrl { get; }

        String CollaboratorsUrl { get; }

        String CommentsUrl { get; }

        String CommitsUrl { get; }

        String CompareUrl { get; }

        String ContentsUrl { get; }

        String ContributorsUrl { get; }

        String DeploymentsUrl { get; }

        String DownloadsUrl { get; }

        String EventsUrl { get; }

        String ForksUrl { get; }

        String GitCommitsUrl { get; }

        String GitRefsUrl { get; }

        String GitTagsUrl { get; }

        String GitUrl { get; }

        String IssueCommentUrl { get; }

        String IssueEventsUrl { get; }

        String IssuesUrl { get; }

        String KeysUrl { get; }

        String LabelsUrl { get; }

        String LanguagesUrl { get; }

        String MergesUrl { get; }

        String MilestonesUrl { get; }

        String NotificationsUrl { get; }

        String PullsUrl { get; }

        String ReleasesUrl { get; }

        String SshUrl { get; }

        String StargazersUrl { get; }

        String StatusesUrl { get; }

        String SubscribersUrl { get; }

        String SubscriptionUrl { get; }

        String TagsUrl { get; }

        String TeamsUrl { get; }

        String TreesUrl { get; }

        String CloneUrl { get; }

        String MirrorUrl { get; }

        String HooksUrl { get; }

        String SvnUrl { get; }

        String Homepage { get; }

        String Language { get; }

        Int32 ForksCount { get; }

        Int32 StargazersCount { get; }

        Int32 WatchersCount { get; }

        Int32 Size { get; }

        String DefaultBranch { get; }

        Int32 OpenIssuesCount { get; }

        Boolean IsTemplate { get; }

        IEnumerable<String> Topics { get; }

        Boolean HasIssues { get; }

        Boolean HasProjects { get; }

        Boolean HasWiki { get; }

        Boolean HasPages { get; }

        Boolean HasDownloads { get; }

        Boolean Archived { get; }

        Boolean Disabled { get; }

        DateTime PushedAt { get; }

        DateTime CreatedAt { get; }

        DateTime UpdatedAt { get; }

        IPermissions Permissions { get; }

        String TemplateRepository { get; }

        Int32 SubscribersCount { get; }

        Int32 NetworkCount { get; }

        ILicense License { get; }
    }
}
