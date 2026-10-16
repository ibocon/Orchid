#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

BeforeAll {
    $script:RepositoryRoot = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '../..')).Path

    function Get-RenderedManifest {
        [CmdletBinding()]
        param([Parameter(Mandatory)] [string] $Overlay)

        $Path = Join-Path -Path $script:RepositoryRoot -ChildPath "deploy/k8s/overlays/$Overlay"
        return (kubectl kustomize $Path) -join "`n"
    }

    function Get-HarborRegistryExternalSecret {
        [CmdletBinding()]
        param([Parameter(Mandatory)] [string] $Manifest)

        # 렌더링 결과를 문서 단위로 나눠 harbor-registry ExternalSecret 문서만 고른다
        return @($Manifest -split '(?m)^---\s*$' | Where-Object {
            $_ -match '(?m)^kind: ExternalSecret\s*$' -and $_ -match '(?m)^  name: harbor-registry\s*$'
        })
    }
}

Describe 'Orchid 배포 자산 중복 제거' {
    It 'Given <Overlay> overlay, When 렌더링, Then harbor-registry ExternalSecret 이 overlay namespace 로 정확히 하나 있다' -ForEach @(
        @{ Overlay = 'staging'; Namespace = 'orchid-staging' }
        @{ Overlay = 'production'; Namespace = 'orchid-production' }
    ) {
        # Given
        $Documents = @(Get-HarborRegistryExternalSecret -Manifest (Get-RenderedManifest -Overlay $Overlay))

        # When
        $Namespaces = $Documents | ForEach-Object { [regex]::Match($_, '(?m)^  namespace: (\S+)\s*$').Groups[1].Value }

        # Then
        $Documents | Should -HaveCount 1
        $Namespaces | Should -Be $Namespace
    }

    It 'Given <Overlay> overlay, When 렌더링, Then harbor-registry 는 namespace 전용 SecretStore 를 쓴다' -ForEach @(@{ Overlay = 'staging' }, @{ Overlay = 'production' }) {
        # Given
        $Documents = @(Get-HarborRegistryExternalSecret -Manifest (Get-RenderedManifest -Overlay $Overlay))

        # When
        $StoreRef = [regex]::Match($Documents[0], '(?m)^  secretStoreRef:\r?\n(?<body>(?:    .*(?:\r?\n|$))+)').Groups['body'].Value

        # Then
        $Documents | Should -HaveCount 1
        $StoreRef | Should -Match '(?m)^    kind: SecretStore\s*$'
        $StoreRef | Should -Match '(?m)^    name: orchid-openbao\s*$'
    }

    It 'Given <_> overlay 디렉터리, When 파일 확인, Then harbor-registry 사본이 없다' -ForEach @('staging', 'production') {
        # Given
        $Path = Join-Path -Path $script:RepositoryRoot -ChildPath "deploy/k8s/overlays/$_/harbor-registry-external-secret.yaml"

        # When / Then
        Test-Path -LiteralPath $Path | Should -BeFalse
    }

    It 'Given <_> 호출 workflow, When 내용 확인, Then 재사용 workflow 를 secrets inherit 로 호출한다' -ForEach @('api-gateway', 'clipboard') {
        # Given
        $Content = Get-Content -Raw -LiteralPath (Join-Path -Path $script:RepositoryRoot -ChildPath ".github/workflows/publish-$_-image.yml")

        # When / Then
        $Content | Should -Match 'uses: \./\.github/workflows/publish-service-image\.yml'
        $Content | Should -Match "service: $_"
        $Content | Should -Match 'secrets: inherit'
        $Content | Should -Match 'workflow_dispatch:'
    }
}
