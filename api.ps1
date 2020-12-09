Import-Module PowershellForXti -Force

$script:apiConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "XTI_ApiGenerator"
    AppName = "XTI_ApiGenerator"
    AppType = "Package"
    ProjectDir = ""
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

function Api-CopyShared {
    $source = "..\SharedWebApp\Apps\SharedWebApp"
    $target = ".\Output\FakeWebApp"
    robocopy "$source\Scripts\Shared\" "$target\Scripts\Shared\" *.ts /e /purge /njh /njs /np /ns /nc /nfl /ndl /a+:R
    robocopy "$source\Scripts\Shared\" "$target\Scripts\Shared\" /xf *.ts /e /purge /njh /njs /np /ns /nc /nfl /ndl /a-:R
    robocopy "$source\Views\Exports\Shared\" "$target\Views\Exports\Shared\" /e /purge /njh /njs /np /ns /nc /nfl /ndl /a+:R
}

function Api-Publish {
    param(
        [switch] $Prod
    )
    $script:apiConfig | Xti-PublishPackage @PsBoundParameters
}
