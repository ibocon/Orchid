#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

BeforeAll {
    $script:RepositoryRoot = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '../..')).Path
    $script:AllowedKeys = @(
        'app/orchid/stag/api-gateway'
        'app/orchid/stag/clipboard'
        'infra/harbor/orchid-registry-pull'
    )

    function Get-RenderedManifest {
        [CmdletBinding()]
        param()

        $OverlayPath = Join-Path -Path $script:RepositoryRoot -ChildPath 'deploy/k8s/overlays/staging'
        return (kubectl kustomize $OverlayPath) -join "`n"
    }
}

Describe 'Orchid staging 시크릿 읽기 범위' {
    It 'Given staging overlay, When 렌더링, Then ExternalSecret 은 ClusterSecretStore 를 쓰지 않는다' {
        # Given
        $Manifest = Get-RenderedManifest

        # When
        $ExternalSecretDocuments = [regex]::Matches(
            $Manifest,
            '(?ms)^apiVersion: external-secrets\.io/v1\r?\nkind: ExternalSecret\r?\n.*?(?=^---\r?$|\z)')

        # Then
        $ExternalSecretDocuments.Count | Should -BeGreaterThan 0
        foreach ($Document in $ExternalSecretDocuments) {
            $Document.Value | Should -Not -Match 'kind: ClusterSecretStore'
        }
    }

    It 'Given staging overlay, When 렌더링, Then 모든 ExternalSecret 이 SecretStore orchid-openbao 를 가리킨다' {
        # Given
        $Manifest = Get-RenderedManifest

        # When
        $ExternalSecretDocuments = [regex]::Matches(
            $Manifest,
            '(?ms)^apiVersion: external-secrets\.io/v1\r?\nkind: ExternalSecret\r?\n.*?(?=^---\r?$|\z)')

        # Then
        $ExternalSecretDocuments.Count | Should -Be 3
        foreach ($Document in $ExternalSecretDocuments) {
            $Document.Value | Should -Match '(?s)secretStoreRef:\s+kind: SecretStore\s+name: orchid-openbao'
        }
    }

    It 'Given staging overlay, When 렌더링, Then SecretStore 는 Orchid 전용 role 과 ServiceAccount 를 쓴다' {
        # Given
        $Manifest = Get-RenderedManifest

        # When / Then
        $Manifest | Should -Match '(?m)^kind: SecretStore$'
        $Manifest | Should -Match 'role: orchid-staging'
        $Manifest | Should -Match '(?s)serviceAccountRef:\s+name: orchid-eso'
        $Manifest | Should -Match '(?s)kind: ServiceAccount\r?\nmetadata:\r?\n  name: orchid-eso'
    }

    It 'Given staging overlay, When 렌더링, Then 참조하는 OpenBao key 가 허용 경로 안에 있다' {
        # Given
        $Manifest = Get-RenderedManifest

        # When
        $Keys = [regex]::Matches($Manifest, '(?m)^\s+key: (\S+)$') |
            ForEach-Object { $_.Groups[1].Value } |
            Sort-Object -Unique

        # Then
        $Keys | Should -Be $script:AllowedKeys
    }
}
