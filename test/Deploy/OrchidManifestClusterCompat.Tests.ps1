#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

BeforeAll {
    $script:RepositoryRoot = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '../..')).Path
    $script:ClusterIssuers = @('lets-encrypt', 'lets-encrypt-stag')

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

Describe 'Orchid overlay 클러스터 호환성' -ForEach @('staging', 'production') {
    It 'Given <_> overlay, When 렌더링, Then ExternalSecret 은 external-secrets.io/v1 만 사용한다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When
        $Versions = [regex]::Matches($Manifest, '(?m)^apiVersion: (external-secrets\.io/\S+)\r?\n(?=kind: ExternalSecret)') |
            ForEach-Object { $_.Groups[1].Value } |
            Sort-Object -Unique

        # Then
        $Versions | Should -Be @('external-secrets.io/v1')
    }

    It 'Given <_> overlay, When 렌더링, Then Ingress ClusterIssuer 는 클러스터에 존재하는 이름이다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When
        $Issuer = [regex]::Match($Manifest, 'cert-manager\.io/cluster-issuer: (\S+)').Groups[1].Value

        # Then
        $script:ClusterIssuers | Should -Contain $Issuer
    }
}
