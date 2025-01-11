using Octokit;
using XTI_App.Abstractions;
using XTI_Git.Abstractions;
using XTI_Secrets;
using XTI_TempLog.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace XTI_ApiGeneratorTool;

internal sealed class VersionKeyAccessor
{
    private readonly ToolOptions options;
    private readonly ISecretCredentialsFactory secretCredentialsFactory;

    public VersionKeyAccessor(ToolOptions options, ISecretCredentialsFactory secretCredentialsFactory)
    {
        this.options = options;
        this.secretCredentialsFactory = secretCredentialsFactory;
    }

    public async Task<AppVersionKey> Value()
    {
        var gitRepo = new LibGit2Sharp.Repository(options.SolutionDirectory);
        var currentBranchName = gitRepo.Head.FriendlyName;
        AppVersionKey versionKey;
        if (XtiIssueBranchName.CanParse(currentBranchName))
        {
            var issueBranchName = XtiIssueBranchName.Parse(currentBranchName);
            var gitHubClient = await CreateGitHubClient();
            var issue = await gitHubClient.Issue.Get
            (
                options.RepoOwner,
                options.RepoName,
                issueBranchName.IssueNumber
            );
            var milestoneName = XtiMilestoneName.Parse(issue.Milestone.Title);
            versionKey = AppVersionKey.Parse(milestoneName.Version.Key);
        }
        else if (XtiVersionBranchName.CanParse(currentBranchName))
        {
            var versionBranchName = XtiVersionBranchName.Parse(currentBranchName);
            versionKey = AppVersionKey.Parse(versionBranchName.Version.Key);
        }
        else
        {
            var gitHubClient = await CreateGitHubClient();
            var milestones = await gitHubClient.Issue.Milestone.GetAllForRepository
            (
                options.RepoOwner,
                options.RepoName,
                new MilestoneRequest
                {
                    State = ItemStateFilter.Closed
                }
            );
            var mostRecentVersionKey = milestones
                .Select(m => XtiMilestoneName.Parse(m.Title))
                .Where(m => !string.IsNullOrWhiteSpace(m.Version.Key))
                .Select(m => AppVersionKey.Parse(m.Version.Key))
                .Where(key => !key.IsNone() && !key.IsCurrent())
                .OrderByDescending(key => key)
                .FirstOrDefault();
            versionKey = mostRecentVersionKey ?? AppVersionKey.Current;
        }
        return versionKey;
    }

    private async Task<GitHubClient> CreateGitHubClient()
    {
        var credentials = await secretCredentialsFactory.Create("GitHub").Value();
        var gitHubClient = new GitHubClient(new ProductHeaderValue("XTI_ApiGenerator"));
        gitHubClient.Credentials = new Credentials(credentials.UserName, credentials.Password);
        return gitHubClient;
    }
}
