#!/usr/bin/env pwsh
#requires -Version 7.0

<#
.SYNOPSIS
    Kustomize overlay의 이미지 태그를 변경한다.

.DESCRIPTION
    kustomization.yaml의 images 목록에서 지정한 이미지의 newTag 한 줄만 변경한다.
    대상 이미지가 없거나 태그가 staging 불변 태그 형식이 아니면 파일을 변경하지 않고 실패한다.

.PARAMETER KustomizationPath
    대상 kustomization.yaml 경로입니다.

.PARAMETER Image
    images[].name 값입니다.

.PARAMETER Tag
    새 staging 불변 태그입니다.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [Parameter(Mandatory)]
    [string] $KustomizationPath,

    [Parameter(Mandatory)]
    [ValidatePattern('^[a-z0-9.-]+/[a-z0-9._/-]+$')]
    [string] $Image,

    [Parameter(Mandatory)]
    [ValidatePattern('^stag-\d{14}-[0-9a-f]{7}$')]
    [string] $Tag
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

try {
    $Path = (Resolve-Path -LiteralPath $KustomizationPath).Path
    $Content = [System.IO.File]::ReadAllText($Path)
    $Pattern = '(?m)^(\s*-\s*name:\s*{0}\s*\r?\n\s*newTag:\s*)(\S+)' -f [regex]::Escape($Image)
    $Match = [regex]::Match($Content, $Pattern)
    if (-not $Match.Success) {
        throw "images에 $Image 항목이 없다: $KustomizationPath"
    }

    if ($Match.Groups[2].Value -eq $Tag) {
        Write-Output "Unchanged $Image $Tag"
        exit 0
    }

    if ($PSCmdlet.ShouldProcess($KustomizationPath, "Set $Image newTag $Tag")) {
        $Updated = $Content.Substring(0, $Match.Groups[2].Index) +
            $Tag +
            $Content.Substring($Match.Groups[2].Index + $Match.Groups[2].Length)
        [System.IO.File]::WriteAllText($Path, $Updated)
    }

    Write-Output "Updated $Image $($Match.Groups[2].Value) -> $Tag"
    exit 0
}
catch {
    Write-Error -Message $_.Exception.Message -ErrorAction Continue
    exit 1
}
