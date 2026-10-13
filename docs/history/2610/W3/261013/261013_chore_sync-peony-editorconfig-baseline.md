# ✅ Peony 기준선 기반 .editorconfig 동기화 계획

---

## 1. 결정이 필요한 사항

1. **.editorconfig 기준선 정합** — `C:\Source\BotanicSoft\Peony\.editorconfig`의 Roslyn/Sonar/StyleCop analyzer 규칙 및 예외 마커 구조 채택 (확정)
   - 근거: BotanicSoft 하네스 거버넌스 및 `root.editorconfig.sot` 기준선과의 완전 일치 보장.
   - 기각한 대안: 기존 축약형 `.editorconfig` 유지 — `Test-EditorConfigBaselineGuard` 실패 발생.

---

## 2. 배경

- `Orchid` 저장소의 초기 `.editorconfig`에 `# 5. Path scoped exceptions` 마커 및 하네스 필수 analyzer 규칙이 누락되어 있어 `Test-EditorConfigBaselineGuard`가 실패함.
- `Peony`의 검증된 `.editorconfig` 전문을 동기화하여 Roslynator, StyleCop, Sonar, C# 13 코딩 컨벤션을 완전히 준수하도록 갱신함.

---

## 3. 변경 범위

| 파일 | 변경 내용 |
| :--- | :--- |
| `.editorconfig` | Peony 표준 .editorconfig 전문 반영 및 섹션 5 예외 마커 포함 |
