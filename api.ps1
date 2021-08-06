Import-Module PowershellForXti -Force

$script:apiConfig = [PSCustomObject]@{
    RepoOwner = "JasonBenfield"
    RepoName = "XTI_ApiGenerator"
    AppName = "XTI_ApiGenerator"
    AppType = "Package"
}

function Api-NewVersion {
    param(
        [Parameter(Position=0)]
        [ValidateSet("major", "minor", "patch")]
        $VersionType = "minor"
    )
    $script:apiConfig | New-XtiVersion @PsBoundParameters
}

function Api-NewIssue {
    param(
        [Parameter(Mandatory, Position=0)]
        [string] $IssueTitle,
        [switch] $Start
    )
    $script:apiConfig | New-XtiIssue @PsBoundParameters
}

function Api-StartIssue {
    param(
        [Parameter(Position=0)]
        [long]$IssueNumber = 0,
        $IssueBranchTitle = ""
    )
    $script:apiConfig | Xti-StartIssue @PsBoundParameters
}

function Api-Publish {
    param(
        [ValidateSet("Development", "Production", "Staging", "Test")]
        $EnvName = "Development"
    )
    $script:apiConfig | Xti-Publish @PsBoundParameters
}
