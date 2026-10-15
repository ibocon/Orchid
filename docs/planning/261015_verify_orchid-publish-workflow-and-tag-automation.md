# ⬜ Orchid 재사용 publish workflow와 staging 태그 자동 갱신 사용자 실행 검증

## 계획 관계

> **관계 스키마**: `1`
> **계획 ID**: `orchid:261015_verify_orchid-publish-workflow-and-tag-automation`
> **출처 판정**: `연결`

| 관계 | 대상 계획 ID | 조건 | 근거·인수 범위 |
| --- | --- | --- | --- |
| 검증 대상 | `orchid:261015_refac_orchid-deploy-duplication` | 해당 없음 | 재사용 workflow 로 publish 실행 성공, Component 반영 후 harbor-registry 동기화 유지 |
| 검증 대상 | `orchid:261015_feat_orchid-staging-image-tag-automation` | 해당 없음 | promote job 의 staging newTag 커밋, 동시 dispatch 충돌 없음, 새 태그 Pod 기동 |

<!-- plan-relations-summary:start -->
> 자동 생성 관계 요약. 원본은 위 관계 표입니다.

| 구분 | 계획·조건 |
| --- | --- |
| 최초 계획 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` |
| 직접 출처 | [⬜ Orchid 이미지 퍼블리시 후 staging overlay 태그를 CI가 자동으로 갱신하는 경로 구축](<261015_feat_orchid-staging-image-tag-automation.md>) `orchid:261015_feat_orchid-staging-image-tag-automation`<br/>[⬜ Orchid 배포 자산의 harbor-registry ExternalSecret 중복과 publish workflow 중복을 공통 정의로 정리](<261015_refac_orchid-deploy-duplication.md>) `orchid:261015_refac_orchid-deploy-duplication` |
| 직접 후속 | 없음 |
| 계보 경로 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` → [✅ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증](<../history/2610/W3/261014/261014_verify_orchid-hanul-gitops-deployment.md>) `orchid:261014_verify_orchid-hanul-gitops-deployment` → [✅ Orchid staging Pod가 Harbor 이미지 pull 인증 없이 ImagePullBackOff로 기동하지 못하는 문제 수정](<../history/2610/W3/261014/261014_fix_orchid-harbor-image-pull-secret.md>) `orchid:261014_fix_orchid-harbor-image-pull-secret` → [✅ Orchid 서비스 이미지를 Harbor orchid 프로젝트로 퍼블리시하는 경로 구축](<../history/2610/W3/261014/261014_feat_orchid-harbor-image-publish.md>) `orchid:261014_feat_orchid-harbor-image-publish` → [⬜ Orchid 이미지 퍼블리시 후 staging overlay 태그를 CI가 자동으로 갱신하는 경로 구축](<261015_feat_orchid-staging-image-tag-automation.md>) `orchid:261015_feat_orchid-staging-image-tag-automation` → [⬜ Orchid 재사용 publish workflow와 staging 태그 자동 갱신 사용자 실행 검증](<261015_verify_orchid-publish-workflow-and-tag-automation.md>) `orchid:261015_verify_orchid-publish-workflow-and-tag-automation`<br/>[✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` → [✅ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증](<../history/2610/W3/261014/261014_verify_orchid-hanul-gitops-deployment.md>) `orchid:261014_verify_orchid-hanul-gitops-deployment` → [✅ Orchid staging Pod가 Harbor 이미지 pull 인증 없이 ImagePullBackOff로 기동하지 못하는 문제 수정](<../history/2610/W3/261014/261014_fix_orchid-harbor-image-pull-secret.md>) `orchid:261014_fix_orchid-harbor-image-pull-secret` → [✅ Orchid 서비스 이미지를 Harbor orchid 프로젝트로 퍼블리시하는 경로 구축](<../history/2610/W3/261014/261014_feat_orchid-harbor-image-publish.md>) `orchid:261014_feat_orchid-harbor-image-publish` → [⬜ Orchid 배포 자산의 harbor-registry ExternalSecret 중복과 publish workflow 중복을 공통 정의로 정리](<261015_refac_orchid-deploy-duplication.md>) `orchid:261015_refac_orchid-deploy-duplication` → [⬜ Orchid 재사용 publish workflow와 staging 태그 자동 갱신 사용자 실행 검증](<261015_verify_orchid-publish-workflow-and-tag-automation.md>) `orchid:261015_verify_orchid-publish-workflow-and-tag-automation` |
| 검증 대상 | [⬜ Orchid 이미지 퍼블리시 후 staging overlay 태그를 CI가 자동으로 갱신하는 경로 구축](<261015_feat_orchid-staging-image-tag-automation.md>) `orchid:261015_feat_orchid-staging-image-tag-automation` — 해당 없음 |
| 검증 대상 | [⬜ Orchid 배포 자산의 harbor-registry ExternalSecret 중복과 publish workflow 중복을 공통 정의로 정리](<261015_refac_orchid-deploy-duplication.md>) `orchid:261015_refac_orchid-deploy-duplication` — 해당 없음 |
| 착수 조건 | 가능 |
<!-- plan-relations-summary:end -->

## 선행 계획

| 계획 | 계획 ID | 완료 상태 | 이관한 후속 검증 |
| --- | --- | --- | --- |
| [Orchid 배포 자산 중복 정리](261015_refac_orchid-deploy-duplication.md) | `orchid:261015_refac_orchid-deploy-duplication` | ⬜ | 재사용 workflow 로 publish 실행 성공, Component 반영 후 harbor-registry 동기화 유지 |
| [Orchid staging 태그 자동 갱신](261015_feat_orchid-staging-image-tag-automation.md) | `orchid:261015_feat_orchid-staging-image-tag-automation` | ⬜ | promote job 커밋, 동시 dispatch 충돌 없음, 새 태그 Pod 기동 |

## 배경

선행 계획 두 건은 Pester로 workflow 파일 계약, 태그 변경 스크립트 동작, Kustomize 렌더링 결과만 확인한다. GitHub Actions 실제 실행(재사용 workflow 호출, `GITHUB_TOKEN` push, job concurrency)과 Flux 적용 후 Pod 기동은 사용자 환경에서만 판정할 수 있다.

## 검증 범위

| ID | 실행 주체 | 환경 | 대상 | 사용자 시나리오 | 필수 |
| --- | --- | --- | --- | --- | :---: |
| V1 | 사용자 | Hanul Kubernetes | `orchid-staging` | Component 반영 후 `harbor-registry` ExternalSecret 이 계속 동기화된다 | 예 |
| V2 | 사용자 | GitHub Actions | `ibocon/Orchid` `main` | api-gateway publish 를 실행하면 publish 와 promote job 이 성공하고 staging newTag 커밋이 생긴다 | 예 |
| V3 | 사용자 | GitHub Actions | `ibocon/Orchid` `main` | 두 서비스를 연달아 dispatch 해도 promote 커밋이 둘 다 main 에 남는다 | 예 |
| V4 | 사용자 | Hanul Kubernetes | `orchid-staging` | Flux 가 새 태그를 적용하고 Pod 가 그 이미지로 기동한다 | 예 |

## 신호 도달 가능성

| ID | 판정 신호 | 발화 지점 | 조작에서 발화까지의 경로 | 차단 가드 | 도달 판정 |
| --- | --- | --- | --- | --- | :---: |
| V1 | ExternalSecret `harbor-registry` `SecretSynced` | `deploy/k8s/components/harbor-registry/external-secret.yaml` | 커밋 푸시 → Flux `orchid-staging-controller` 적용 → ESO reconcile | SecretStore `orchid-openbao` 권한, Flux 의존 Kustomization 대기 | 도달 가능 |
| V2 | run 의 두 job `success` 와 `ops(image): update orchid staging api-gateway` 커밋 | `.github/workflows/publish-service-image.yml` `promote-staging` job | dispatch → publish job → promote job → `Set-OverlayImageTag.ps1` → `git push` | Harbor secret 누락, `contents: write` 누락, push 거부 | 도달 가능 |
| V3 | main 에 promote 커밋 2건 | 같은 promote job, concurrency `orchid-staging-tag-promotion` | 두 dispatch → promote job 직렬 실행 → rebase 재시도 → push | 3회 재시도 초과 | 도달 가능 |
| V4 | Pod 이벤트 `Successfully pulled image ...:<새 태그>` | kubelet | promote 커밋 → Flux 적용 → Deployment rollout | Flux 미반영, pull 자격 | 도달 가능 |

## 사전 조건

| 항목 | 확인 방법 | 기대값 | 상태 |
| --- | --- | --- | --- |
| refac 계획 구현 | `docs/planning/261015_refac_orchid-deploy-duplication.md` H1 | `✅` | 미충족 |
| feat 계획 구현 | `docs/planning/261015_feat_orchid-staging-image-tag-automation.md` H1 | `✅` | 미충족 |
| GitHub Actions secret | `gh secret list --repo ibocon/Orchid` | `HARBOR_REGISTRY`, `HARBOR_USERNAME`, `HARBOR_PASSWORD` | 미충족 |
| main 보호 규칙 | `gh api repos/ibocon/Orchid/branches/main/protection` | 404 (보호 없음) 또는 `github-actions[bot]` push 허용 | 미충족 |
| pull 자격 동기화 | `orchid-staging` ExternalSecret 상태 | `SecretSynced`. 2026-10-15 조회 시 SecretStore `orchid-openbao`가 `invalid role name "orchid-staging"`로 실패 중(`orchid:261014_fix_orchid-secret-store-scope` 범위) | 미충족 |

```bash
# 사전 조회: kubectl get namespace orchid-staging, kubectl get deployments,externalsecrets -n orchid-staging (2026-10-15, Hanul agents-mcp)
NAME             STATUS   AGE
orchid-staging   Active   25h

NAME                                 READY
deployment/orchid-api-gateway        1/1
deployment/orchid-clipboard          1/1

NAME                                                   READY
externalsecret/harbor-registry                         False
externalsecret/orchid-api-gateway-external-secret      False
externalsecret/orchid-clipboard-external-secret        False
```

## 사용자 실행 절차

### V1: Component 반영 후 harbor-registry 동기화

#### 에이전트 안내문

refac 계획 커밋이 Flux 로 적용된 뒤 pull 자격 ExternalSecret 이 계속 동기화되는지 확인합니다.

#### 사용자 수행 동작

1. 상태를 조회합니다.
   ```bash
   kubectl get externalsecret harbor-registry -n orchid-staging
   ```

#### 사용자 필수 답변

- 실행 시각
- 출력의 `STATUS`, `READY` 값

#### 통과 기준

`STATUS`가 `SecretSynced`, `READY`가 `True`다.

#### 응답 대기 게이트

V1 답변이 오기 전에는 V2를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V2로 진행한다.
- FAIL: `kubectl describe externalsecret harbor-registry -n orchid-staging` 이벤트를 기록하고 `fix` 계획으로 승격한다.
- BLOCKED: Flux 의존 Kustomization 대기면 해소 후 재확인한다.

---

### V2: api-gateway publish 와 promote

#### 에이전트 안내문

api-gateway publish 를 `main` 에서 실행하고 promote job 이 staging 태그를 커밋하는지 확인합니다.

#### 사용자 수행 동작

1. 실행합니다.
   ```powershell
   & lib/Garden.Harness/src/git/scripts/Invoke-WorkflowDispatch.ps1 -Workflow publish-api-gateway-image.yml -Ref main
   ```
2. 커밋을 확인합니다.
   ```bash
   git fetch origin && git log --oneline -3 origin/main -- deploy/k8s/overlays/staging/kustomization.yaml
   ```

#### 사용자 필수 답변

- 실행 시각
- run 결론과 Summary 의 `Published staging tag` 값
- 최신 커밋 메시지 한 줄

#### 통과 기준

run 이 `success`이고, 최신 커밋 메시지가 `ops(image): update orchid staging api-gateway -> <Summary 태그>`다.

#### 응답 대기 게이트

V2 답변이 오기 전에는 V3를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V3로 진행한다.
- FAIL: promote job 로그를 기록하고 `fix` 계획으로 승격한다.
- BLOCKED: secret 누락이나 push 거부면 사전 조건을 해소한 뒤 재시도한다.

---

### V3: 두 서비스 연속 dispatch

#### 에이전트 안내문

두 publish 를 연달아 실행해 promote 커밋이 서로 덮어쓰지 않는지 확인합니다.

#### 사용자 수행 동작

1. 대기 없이 연달아 실행합니다.
   ```powershell
   foreach ($Workflow in @('publish-api-gateway-image.yml', 'publish-clipboard-image.yml')) {
       & lib/Garden.Harness/src/git/scripts/Invoke-WorkflowDispatch.ps1 -Workflow $Workflow -Ref main -NoWatch
   }
   ```
2. 두 run 이 끝난 뒤 V2 의 2번 명령으로 커밋을 확인합니다.

#### 사용자 필수 답변

- 실행 시각
- 두 run 결론
- 최신 커밋 메시지 두 줄

#### 통과 기준

두 run 이 `success`이고 api-gateway, clipboard promote 커밋이 각각 하나씩 main 에 있다.

#### 응답 대기 게이트

V3 답변이 오기 전에는 V4를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V4로 진행한다.
- FAIL: 실패한 promote job 로그를 기록하고 `fix` 계획으로 승격한다.
- BLOCKED: GitHub Actions 장애면 해소 후 재시도한다.

---

### V4: 새 태그 Pod 기동

#### 에이전트 안내문

Flux 가 V3 커밋을 적용한 뒤 Pod 가 새 태그로 기동하는지 확인합니다.

#### 사용자 수행 동작

1. 조회합니다.
   ```bash
   kubectl get deploy -n orchid-staging -o wide
   kubectl get pods -n orchid-staging
   ```

#### 사용자 필수 답변

- 실행 시각
- 두 Deployment 의 `IMAGES` 값과 `READY`
- Pod `STATUS`

#### 통과 기준

두 Deployment 이미지 태그가 V3 커밋의 태그와 같고 `READY 1/1`, Pod 가 `Running`이다.

#### 판정 후 다음 단계

- PASS: 모든 필수 시나리오 통과로 검증 계획서를 완료한다.
- FAIL: `kubectl describe pod` 이벤트를 기록하고 `fix` 계획으로 승격한다.
- BLOCKED: Flux 미반영이면 해소 조건을 기록한다.

---

## 증거 수집

| ID | 실행 시각 | 환경과 릴리스 | 입력 | 기대 결과 | 실제 결과 | 증거 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| V1 | — | Hanul Kubernetes | `kubectl get externalsecret harbor-registry` | `SecretSynced` | — | — | 미실행 |
| V2 | — | GitHub Actions | `publish-api-gateway-image.yml` | run success, promote 커밋 | — | — | 미실행 |
| V3 | — | GitHub Actions | 두 workflow 연속 dispatch | promote 커밋 2건 | — | — | 미실행 |
| V4 | — | Hanul Kubernetes | `kubectl get deploy,pods` | 새 태그 `1/1` | — | — | 미실행 |

증거에는 비밀값과 자격 정보를 제거한다.

## 판정 결과

| 결과 | 시나리오 | 근거 | 후속 조치 |
| --- | --- | --- | --- |
| 미실행 | V1, V2, V3, V4 | 선행 계획 구현 대기 | 선행 계획 ✅ 후 사용자 실행 |

## 결함 승격

| 실패 시나리오 | 변경 유형 | 후속 계획 | 상태 |
| --- | --- | --- | --- |
| — | — | — | 해당 없음 |

## 작업 순서

1. 사전 조건을 확인한다.
2. V1을 안내하고 결과를 수집한다.
3. V2, V3, V4를 순서대로 진행한다.
4. 모든 시나리오 결과를 기록하고 완료한다.
