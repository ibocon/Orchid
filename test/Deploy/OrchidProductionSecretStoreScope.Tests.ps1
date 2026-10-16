#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

BeforeAll {
    $script:RepositoryRoot = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '../..')).Path
    $script:AllowedKeys = @(
        'app/prod/orchid/api-gateway'
        'app/prod/orchid/clipboard'
        'infra/harbor/orchid-registry-pull'
    )

    function Get-RenderedManifest {
        [CmdletBinding()]
        param()

        $OverlayPath = Join-Path -Path $script:RepositoryRoot -ChildPath 'deploy/k8s/overlays/production'
        return (kubectl kustomize $OverlayPath) -join "`n"
    }
}

Describe 'Orchid production 시크릿 읽기 범위' {
    It 'Given production overlay, When 렌더링, Then ExternalSecret 은 ClusterSecretStore 를 쓰지 않는다' {
        $Manifest = Get-RenderedManifest
        $ExternalSecretDocuments = [regex]::Matches(
            $Manifest,
            '(?ms)^apiVersion: external-secrets\.io/v1\r?\nkind: ExternalSecret\r?\n.*?(?=^---\r?$|\z)')

        $ExternalSecretDocuments.Count | Should -Be 3
        foreach ($Document in $ExternalSecretDocuments) {
            $Document.Value | Should -Not -Match 'kind: ClusterSecretStore'
            $Document.Value | Should -Match '(?s)secretStoreRef:\s+(?:kind: SecretStore\s+name: orchid-openbao|name: orchid-openbao\s+kind: SecretStore)'
        }
    }

    It 'Given production overlay, When 렌더링, Then SecretStore 는 production 전용 OpenBao role 과 ServiceAccount 를 쓴다' {
        $Manifest = Get-RenderedManifest

        $Manifest | Should -Match '(?m)^kind: SecretStore$'
        $Manifest | Should -Match 'role: orchid-production'
        $Manifest | Should -Match '(?s)serviceAccountRef:\s+name: orchid-eso'
        $Manifest | Should -Match '(?s)kind: ServiceAccount\r?\nmetadata:\r?\n  name: orchid-eso'
    }

    It 'Given production overlay, When 렌더링, Then 참조하는 OpenBao key 가 허용 경로 안에 있다' {
        $Manifest = Get-RenderedManifest
        $Keys = [regex]::Matches($Manifest, '(?m)^\s+key: (\S+)$') |
            ForEach-Object { $_.Groups[1].Value } |
            Sort-Object -Unique

        $Keys | Should -Be $script:AllowedKeys
    }
}
