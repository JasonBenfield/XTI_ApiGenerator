Import-Module PowershellForXti -Force

$script:apiConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "XTI_ApiGenerator"
    AppName = "XTI_ApiGenerator"
    AppType = "Package"
    ProjectDir = "Output/FakeWebApp"
}

function Api-New-XtiIssue {
    param(
        [Parameter(Mandatory, Position=0)]
        [string] $IssueTitle,
        $Label = @(),
        [string] $Body = "",
        [switch] $Start
    )
    $script:apiConfig | New-XtiIssue @PsBoundParameters
}

function Api-Xti-StartIssue {
    param(
        [Parameter(Position=0)]
        [long]$IssueNumber = 0,
        $IssueBranchTitle = "",
        $AssignTo = ""
    )
    $script:apiConfig | Xti-StartIssue @PsBoundParameters
}

function Api-New-XtiVersion {
    param(
        [Parameter(Position=0)]
        [ValidateSet("major", "minor", "patch")]
        $VersionType = "minor",
        [ValidateSet("Development", "Production", "Staging", "Test")]
        $EnvName = "Production"
    )
    $script:apiConfig | New-XtiVersion @PsBoundParameters
}

function Api-Xti-Merge {
    param(
        [Parameter(Position=0)]
        [string] $CommitMessage
    )
    $script:apiConfig | Xti-Merge @PsBoundParameters
}

function Api-New-XtiPullRequest {
    param(
        [Parameter(Position=0)]
        [string] $CommitMessage
    )
    $script:apiConfig | New-XtiPullRequest @PsBoundParameters
}

function Api-Xti-PostMerge {
    param(
    )
    $script:apiConfig | Xti-PostMerge @PsBoundParameters
}

function Api-Publish {
    param(
        [switch] $Prod
    )
    $script:apiConfig | Xti-PublishPackage @PsBoundParameters
    if($Prod) {
        $script:apiConfig | Xti-Merge
    }
}

function Api-ImportWeb {
    param(
        [switch] $Prod
    )
    $script:apiConfig | Xti-ImportWeb -Prod:$Prod -AppToImport Shared
}
