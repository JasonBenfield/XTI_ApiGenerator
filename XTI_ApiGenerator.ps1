Import-Module PowershellForXti -Force

$script:webAppConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "XTI_ApiGenerator"
    AppKey = "XTI_ApiGenerator"
    AppType = "Package"
    ProjectDir = ""
}

function Api-New-XtiIssue {
    param(
        [Parameter(Mandatory, Position=0)]
        [string] $IssueTitle,
        $Label = @(),
        [string] $Body = ""
    )
    $script:webAppConfig | New-XtiIssue @PsBoundParameters
}

function Api-Xti-StartIssue {
    param(
        [Parameter(Position=0)]
        [long]$IssueNumber = 0,
        $IssueBranchTitle = "",
        $AssignTo = ""
    )
    $script:webAppConfig | Xti-StartIssue @PsBoundParameters
}

function Api-New-XtiVersion {
    param(
        [Parameter(Position=0)]
        [ValidateSet("major", "minor", "patch")]
        $VersionType = "minor",
        [ValidateSet("Development", "Production", "Staging", "Test")]
        $EnvName = "Production"
    )
    $script:webAppConfig | New-XtiVersion @PsBoundParameters
}

function Api-New-XtiPullRequest {
    param(
        [Parameter(Position=0)]
        [string] $CommitMessage
    )
    $script:webAppConfig | New-XtiPullRequest @PsBoundParameters
}

function Api-Xti-PostMerge {
    param(
    )
    $script:webAppConfig | Xti-PostMerge @PsBoundParameters
}

function Api-Publish {
    param(
        [switch] $Prod
    )
    Xti-PublishPackage @PsBoundParameters
}
