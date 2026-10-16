#requires -Modules @{ ModuleName = 'Pester'; ModuleVersion = '5.0' }

BeforeAll {
    $script:ScriptPath = Join-Path -Path $PSScriptRoot -ChildPath '../../deploy/scripts/Set-OverlayImageTag.ps1'

    function New-KustomizationFixture {
        [CmdletBinding()]
        param()

        $Path = Join-Path -Path $TestDrive -ChildPath ([guid]::NewGuid().ToString('N') + '.yaml')
        $Content = @(
            'images:'
            '  - name: harbor.hanul.botanicsoft.com/orchid/api-gateway'
            '    newTag: stag-20261015021051-0a80f65'
            '  - name: harbor.hanul.botanicsoft.com/orchid/clipboard'
            '    newTag: stag-20261015021237-0a80f65'
        ) -join "`n"
        Set-Content -LiteralPath $Path -Value $Content -NoNewline
        return $Path
    }

    function Invoke-SetTag {
        [CmdletBinding()]
        param(
            [Parameter(Mandatory)] [string] $Path,
            [Parameter(Mandatory)] [string] $Image,
            [Parameter(Mandatory)] [string] $Tag
        )

        $Output = pwsh -NoProfile -NonInteractive -File $script:ScriptPath -KustomizationPath $Path -Image $Image -Tag $Tag 2>&1 | Out-String
        return [pscustomobject]@{ ExitCode = $LASTEXITCODE; Output = $Output }
    }
}

Describe 'Set-OverlayImageTag' {
    It 'Given 두 이미지 overlay, When api-gateway 태그 변경, Then api-gateway newTag 만 바뀐다' {
        # Given
        $Path = New-KustomizationFixture

        # When
        $Result = Invoke-SetTag -Path $Path -Image 'harbor.hanul.botanicsoft.com/orchid/api-gateway' -Tag 'stag-20261016000000-abcdef1'

        # Then
        $Result.ExitCode | Should -Be 0
        $Content = Get-Content -Raw -LiteralPath $Path
        $Content | Should -Match 'orchid/api-gateway\s+newTag: stag-20261016000000-abcdef1'
        $Content | Should -Match 'orchid/clipboard\s+newTag: stag-20261015021237-0a80f65'
    }

    It 'Given 같은 태그, When 실행, Then 파일을 쓰지 않고 Unchanged 를 출력한다' {
        # Given
        $Path = New-KustomizationFixture
        $Before = Get-Content -Raw -LiteralPath $Path

        # When
        $Result = Invoke-SetTag -Path $Path -Image 'harbor.hanul.botanicsoft.com/orchid/clipboard' -Tag 'stag-20261015021237-0a80f65'

        # Then
        $Result.ExitCode | Should -Be 0
        $Result.Output | Should -Match 'Unchanged'
        (Get-Content -Raw -LiteralPath $Path) | Should -Be $Before
    }

    It 'Given 없는 이미지, When 실행, Then 실패하고 파일을 바꾸지 않는다' {
        # Given
        $Path = New-KustomizationFixture
        $Before = Get-Content -Raw -LiteralPath $Path

        # When
        $Result = Invoke-SetTag -Path $Path -Image 'harbor.hanul.botanicsoft.com/orchid/unknown' -Tag 'stag-20261016000000-abcdef1'

        # Then
        $Result.ExitCode | Should -Be 1
        (Get-Content -Raw -LiteralPath $Path) | Should -Be $Before
    }

    It 'Given 움직이는 태그 stag-dev, When 실행, Then 매개변수 검증으로 실패한다' {
        # Given
        $Path = New-KustomizationFixture

        # When
        $Result = Invoke-SetTag -Path $Path -Image 'harbor.hanul.botanicsoft.com/orchid/api-gateway' -Tag 'stag-dev'

        # Then
        $Result.ExitCode | Should -Not -Be 0
    }
}
