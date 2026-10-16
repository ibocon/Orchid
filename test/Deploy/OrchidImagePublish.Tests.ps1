#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

BeforeDiscovery {
    $script:Services = @(
        @{ Name = 'api-gateway'; Dockerfile = 'src/Service/ApiGateway/Dockerfile'; Workflow = '.github/workflows/publish-api-gateway-image.yml' }
        @{ Name = 'clipboard'; Dockerfile = 'src/Service/Clipboard/Dockerfile'; Workflow = '.github/workflows/publish-clipboard-image.yml' }
    )
}

BeforeAll {
    $script:RepositoryRoot = (Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '../..')).Path

    function Get-RepositoryFile {
        [CmdletBinding()]
        param(
            [Parameter(Mandatory)]
            [string] $RelativePath
        )

        $Path = Join-Path -Path $script:RepositoryRoot -ChildPath $RelativePath
        if (-not (Test-Path -LiteralPath $Path)) {
            return ''
        }

        return Get-Content -Raw -LiteralPath $Path
    }

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

Describe 'Orchid 이미지 퍼블리시 경로' {
    It 'Given <Name> 서비스, When workflow 확인, Then orchid 프로젝트로 불변 태그를 push 한다' -ForEach $script:Services {
        # Given
        $Shared = Get-RepositoryFile -RelativePath '.github/workflows/publish-service-image.yml'
        $Caller = Get-RepositoryFile -RelativePath $Workflow

        # When / Then
        $Shared | Should -Match '/orchid/\$\{\{ inputs\.service \}\}'
        $Shared | Should -Match 'stag-\$\{BUILD_STAMP\}-\$\{SHORT_SHA\}'
        $Shared | Should -Match 'runs-on: ubuntu-latest'
        $Shared | Should -Not -Match 'hanul-linux'
        $Caller | Should -Match "service: $Name"
        $Caller | Should -Match "dockerfile: $([regex]::Escape($Dockerfile))"
        $Caller | Should -Not -Match 'hanul-linux'
    }

    It 'Given <Name> 서비스, When Dockerfile 확인, Then 8080 을 노출하고 비루트로 실행한다' -ForEach $script:Services {
        # Given
        $Content = Get-RepositoryFile -RelativePath $Dockerfile

        # When / Then
        $Content | Should -Match 'EXPOSE 8080'
        $Content | Should -Match 'USER \$APP_UID'
    }

    It 'Given <_> overlay, When 렌더링, Then harbor-registry 는 orchid 전용 pull 자격을 읽는다' -ForEach @('staging', 'production') {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When / Then
        $Manifest | Should -Match 'key: infra/harbor/orchid-registry-pull'
        $Manifest | Should -Not -Match 'key: infra/harbor/registry-pull'
    }

    It 'Given staging overlay, When 렌더링, Then 움직이는 태그 stag-dev 를 쓰지 않는다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay 'staging'

        # When
        $Tags = [regex]::Matches($Manifest, 'image: harbor\.hanul\.botanicsoft\.com/orchid/[^:]+:(\S+)') |
            ForEach-Object { $_.Groups[1].Value }

        # Then
        $Tags | Should -Not -BeNullOrEmpty
        $Tags | Should -Not -Contain 'stag-dev'
        $Tags | ForEach-Object { $_ | Should -Match '^stag-\d{14}-[0-9a-f]{7}$' }
    }

    It 'Given production overlay, When 렌더링, Then 두 서비스가 immutable digest 를 사용한다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay 'production'

        # When
        $Digests = [regex]::Matches(
            $Manifest,
            'image: harbor\.hanul\.botanicsoft\.com/orchid/[^@]+@(sha256:[0-9a-f]{64})') |
            ForEach-Object { $_.Groups[1].Value }

        # Then
        $Digests | Should -HaveCount 2
        $Digests | Should -Not -Contain 'sha256:prod-v1.0.0'
    }
}
