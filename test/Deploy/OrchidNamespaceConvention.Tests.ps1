BeforeAll {
    $script:RepositoryRoot = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '../..')).Path

    function Get-RenderedManifest {
        [CmdletBinding()]
        param(
            [Parameter(Mandatory)]
            [string] $Overlay
        )

        $Path = Join-Path -Path $script:RepositoryRoot -ChildPath "deploy/k8s/overlays/$Overlay"
        return (kubectl kustomize $Path) -join "`n"
    }
}

Describe 'Orchid overlay 네임스페이스 규칙' -ForEach @('staging', 'production') {
    It 'Given <_> overlay, When 렌더링, Then 모든 리소스 네임스페이스가 orchid-<환경> 하나다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When
        $Namespaces = [regex]::Matches($Manifest, '(?m)^  namespace: (\S+)$') |
            ForEach-Object { $_.Groups[1].Value } |
            Sort-Object -Unique

        # Then
        $Namespaces | Should -Be @("orchid-$_")
    }
}
