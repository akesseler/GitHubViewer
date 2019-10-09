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
using Newtonsoft.Json.Serialization;
using Plexdata.GitHub.Accessor.Abstraction.Entities;
using Plexdata.GitHub.Accessor.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plexdata.GitHub.Accessor.Internals.Entities
{
    internal class Repository : IRepository
    {
        #region Public properties

        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [JsonProperty("node_id")]
        public String NodeId { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("full_name")]
        public String FullName { get; set; }

        [JsonProperty("owner")]
        public IOwner Owner { get; set; }

        [JsonProperty("private")]
        public Boolean Private { get; set; }

        [JsonProperty("html_url")]
        public String HtmlUrl { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("fork")]
        public Boolean Fork { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("archive_url")]
        public String ArchiveUrl { get; set; }

        [JsonProperty("assignees_url")]
        public String AssigneesUrl { get; set; }

        [JsonProperty("blobs_url")]
        public String BlobsUrl { get; set; }

        [JsonProperty("branches_url")]
        public String BranchesUrl { get; set; }

        [JsonProperty("collaborators_url")]
        public String CollaboratorsUrl { get; set; }

        [JsonProperty("comments_url")]
        public String CommentsUrl { get; set; }

        [JsonProperty("commits_url")]
        public String CommitsUrl { get; set; }

        [JsonProperty("compare_url")]
        public String CompareUrl { get; set; }

        [JsonProperty("contents_url")]
        public String ContentsUrl { get; set; }

        [JsonProperty("contributors_url")]
        public String ContributorsUrl { get; set; }

        [JsonProperty("deployments_url")]
        public String DeploymentsUrl { get; set; }

        [JsonProperty("downloads_url")]
        public String DownloadsUrl { get; set; }

        [JsonProperty("events_url")]
        public String EventsUrl { get; set; }

        [JsonProperty("forks_url")]
        public String ForksUrl { get; set; }

        [JsonProperty("git_commits_url")]
        public String GitCommitsUrl { get; set; }

        [JsonProperty("git_refs_url")]
        public String GitRefsUrl { get; set; }

        [JsonProperty("git_tags_url")]
        public String GitTagsUrl { get; set; }

        [JsonProperty("git_url")]
        public String GitUrl { get; set; }

        [JsonProperty("issue_comment_url")]
        public String IssueCommentUrl { get; set; }

        [JsonProperty("issue_events_url")]
        public String IssueEventsUrl { get; set; }

        [JsonProperty("issues_url")]
        public String IssuesUrl { get; set; }

        [JsonProperty("keys_url")]
        public String KeysUrl { get; set; }

        [JsonProperty("labels_url")]
        public String LabelsUrl { get; set; }

        [JsonProperty("languages_url")]
        public String LanguagesUrl { get; set; }

        [JsonProperty("merges_url")]
        public String MergesUrl { get; set; }

        [JsonProperty("milestones_url")]
        public String MilestonesUrl { get; set; }

        [JsonProperty("notifications_url")]
        public String NotificationsUrl { get; set; }

        [JsonProperty("pulls_url")]
        public String PullsUrl { get; set; }

        [JsonProperty("releases_url")]
        public String ReleasesUrl { get; set; }

        [JsonProperty("ssh_url")]
        public String SshUrl { get; set; }

        [JsonProperty("stargazers_url")]
        public String StargazersUrl { get; set; }

        [JsonProperty("statuses_url")]
        public String StatusesUrl { get; set; }

        [JsonProperty("subscribers_url")]
        public String SubscribersUrl { get; set; }

        [JsonProperty("subscription_url")]
        public String SubscriptionUrl { get; set; }

        [JsonProperty("tags_url")]
        public String TagsUrl { get; set; }

        [JsonProperty("teams_url")]
        public String TeamsUrl { get; set; }

        [JsonProperty("trees_url")]
        public String TreesUrl { get; set; }

        [JsonProperty("clone_url")]
        public String CloneUrl { get; set; }

        [JsonProperty("mirror_url")]
        public String MirrorUrl { get; set; }

        [JsonProperty("hooks_url")]
        public String HooksUrl { get; set; }

        [JsonProperty("svn_url")]
        public String SvnUrl { get; set; }

        [JsonProperty("homepage")]
        public String Homepage { get; set; }

        [JsonProperty("language")]
        public String Language { get; set; }

        [JsonProperty("forks_count")]
        public Int32 ForksCount { get; set; }

        [JsonProperty("stargazers_count")]
        public Int32 StargazersCount { get; set; }

        [JsonProperty("watchers_count")]
        public Int32 WatchersCount { get; set; }

        [JsonProperty("size")]
        public Int32 Size { get; set; }

        [JsonProperty("default_branch")]
        public String DefaultBranch { get; set; }

        [JsonProperty("open_issues_count")]
        public Int32 OpenIssuesCount { get; set; }

        [JsonProperty("is_template")]
        public Boolean IsTemplate { get; set; }

        [JsonProperty("topics")]
        public IEnumerable<String> Topics { get; set; }

        [JsonProperty("has_issues")]
        public Boolean HasIssues { get; set; }

        [JsonProperty("has_projects")]
        public Boolean HasProjects { get; set; }

        [JsonProperty("has_wiki")]
        public Boolean HasWiki { get; set; }

        [JsonProperty("has_pages")]
        public Boolean HasPages { get; set; }

        [JsonProperty("has_downloads")]
        public Boolean HasDownloads { get; set; }

        [JsonProperty("archived")]
        public Boolean Archived { get; set; }

        [JsonProperty("disabled")]
        public Boolean Disabled { get; set; }

        [JsonProperty("pushed_at")]
        public DateTime PushedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("permissions")]
        public IPermissions Permissions { get; set; }

        [JsonProperty("template_repository")]
        public String TemplateRepository { get; set; }

        [JsonProperty("subscribers_count")]
        public Int32 SubscribersCount { get; set; }

        [JsonProperty("network_count")]
        public Int32 NetworkCount { get; set; }

        [JsonProperty("license")]
        public ILicense License { get; set; }

        #endregion

        #region Public methods

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(128);

            builder.AppendFormat("{0}: \"{1}\", ", nameof(this.FullName), this.FullName.ToDisplay());
            builder.AppendFormat("{0}: \"{1}\", ", nameof(this.Owner), this.Owner.ToString());

            return builder.ToString().TrimEnd(' ', ',');
        }

        #endregion
    }
}
