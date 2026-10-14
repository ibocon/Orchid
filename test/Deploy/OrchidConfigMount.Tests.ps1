#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

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

Describe 'Orchid ConfigMap 설정 파일 마운트' -ForEach @('staging', 'production') {
    It 'Given <_> overlay, When 렌더링, Then ConfigMap 을 envFrom 으로 주입하지 않는다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When
        $Injected = [regex]::Matches($Manifest, '(?m)^\s+- configMapRef:').Count

        # Then
        $Injected | Should -Be 0
    }

    It 'Given <_> overlay, When 렌더링, Then 모든 Deployment 가 /app/appsettings.json 을 subPath 로 마운트한다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When
        $Deployments = [regex]::Matches($Manifest, '(?m)^kind: Deployment$').Count
        $Mounts = [regex]::Matches($Manifest, '(?m)mountPath: /app/appsettings\.json\r?\n\s+name: app-settings\r?\n\s+readOnly: true\r?\n\s+subPath: appsettings\.json').Count

        # Then
        $Deployments | Should -BeGreaterThan 0
        $Mounts | Should -Be $Deployments
    }

    It 'Given <_> overlay, When 렌더링, Then gateway 의 clipboard 목적지가 렌더링된 Service 이름과 포트와 같다' {
        # Given
        $Manifest = Get-RenderedManifest -Overlay $_

        # When
        $Destination = [regex]::Match($Manifest, 'Address": "http://([a-z0-9-]+):(\d+)"')
        $ServiceName = $Destination.Groups[1].Value
        $ServicePort = $Destination.Groups[2].Value

        # Then
        $Destination.Success | Should -BeTrue
        $Manifest | Should -Match "(?m)^  name: $ServiceName\r?\n"
        $Manifest | Should -Match "port: $ServicePort"
    }
}
