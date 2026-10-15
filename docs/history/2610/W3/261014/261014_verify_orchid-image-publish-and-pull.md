# ✅ Orchid 이미지 Harbor 퍼블리시 workflow 실행과 클러스터 pull 사용자 실행 검증

## 계획 관계

> **관계 스키마**: `1`
> **계획 ID**: `orchid:261014_verify_orchid-image-publish-and-pull`
> **출처 판정**: `연결`

| 관계 | 대상 계획 ID | 조건 | 근거·인수 범위 |
| --- | --- | --- | --- |
| 검증 대상 | `orchid:261014_feat_orchid-harbor-image-publish` | 해당 없음 | 후속 검증 중 publish workflow 실행과 퍼블리시 후 클러스터 pull을 이관한다. 자격 준비와 Pod 기동·경보 해소는 선행 계획이 직접 확인한다 |

<!-- plan-relations-summary:start -->
> 자동 생성 관계 요약. 원본은 위 관계 표입니다.

| 구분 | 계획·조건 |
| --- | --- |
| 최초 계획 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` |
| 직접 출처 | [✅ Orchid 서비스 이미지를 Harbor orchid 프로젝트로 퍼블리시하는 경로 구축](<261014_feat_orchid-harbor-image-publish.md>) `orchid:261014_feat_orchid-harbor-image-publish` |
| 직접 후속 | 없음 |
| 계보 경로 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` → [✅ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증](<261014_verify_orchid-hanul-gitops-deployment.md>) `orchid:261014_verify_orchid-hanul-gitops-deployment` → [✅ Orchid staging Pod가 Harbor 이미지 pull 인증 없이 ImagePullBackOff로 기동하지 못하는 문제 수정](<261014_fix_orchid-harbor-image-pull-secret.md>) `orchid:261014_fix_orchid-harbor-image-pull-secret` → [✅ Orchid 서비스 이미지를 Harbor orchid 프로젝트로 퍼블리시하는 경로 구축](<261014_feat_orchid-harbor-image-publish.md>) `orchid:261014_feat_orchid-harbor-image-publish` → [✅ Orchid 이미지 Harbor 퍼블리시 workflow 실행과 클러스터 pull 사용자 실행 검증](<261014_verify_orchid-image-publish-and-pull.md>) `orchid:261014_verify_orchid-image-publish-and-pull` |
| 검증 대상 | [✅ Orchid 서비스 이미지를 Harbor orchid 프로젝트로 퍼블리시하는 경로 구축](<261014_feat_orchid-harbor-image-publish.md>) `orchid:261014_feat_orchid-harbor-image-publish` — 해당 없음 |
| 착수 조건 | 가능 |
<!-- plan-relations-summary:end -->

## 선행 계획

| 계획 | 계획 ID | 완료 상태 | 이관한 후속 검증 |
| --- | --- | --- | --- |
| [Orchid 서비스 이미지를 Harbor orchid 프로젝트로 퍼블리시하는 경로 구축](261014_feat_orchid-harbor-image-publish.md) | `orchid:261014_feat_orchid-harbor-image-publish` | ✅ | publish workflow 실행, 퍼블리시 후 클러스터 pull |

## 배경

- 선행 계획은 `ibocon/Orchid`의 GitHub workflow(`publish-api-gateway-image.yml`, `publish-clipboard-image.yml`)와 Dockerfile을 신설해 Harbor `orchid` 프로젝트에 이미지를 푸시한다. 단위 테스트는 파일 내용만 검증하므로 실제 workflow 실행과 Harbor 푸시, 클러스터 pull은 사용자 환경에서만 확인할 수 있다.
- 선행 계획의 Step 1(Harbor 프로젝트와 robot, OpenBao 자격, GitHub secret 준비)은 사용자가 수행한다. 이 계획은 그 준비와 선행 계획의 구현(✅)이 끝난 뒤에 실행한다.
- 선행 계획의 후속 검증 "Pod 기동"과 "경보 해소"는 선행 계획과 `orchid:261014_verify_orchid-hanul-gitops-deployment` V2가 다루므로 이 계획은 이미지가 올라가고 pull되는 데까지만 판정한다.

## 검증 범위

| ID | 실행 주체 | 환경 | 대상 | 사용자 시나리오 | 필수 |
| --- | --- | --- | --- | --- | :---: |
| V1 | 사용자 | GitHub Actions | `ibocon/Orchid` `main` | 두 publish workflow를 실행해 성공하고 불변 태그가 Summary에 남는다 | 예 |
| V2 | 사용자 | Harbor | `orchid` 프로젝트 | 푸시한 불변 태그가 Harbor에 존재한다 | 예 |
| V3 | 사용자 | Hanul Kubernetes | `orchid-staging` | 새 태그를 반영한 Pod가 이미지를 pull한다 | 예 |

## 신호 도달 가능성

| ID | 판정 신호 | 발화 지점 | 조작에서 발화까지의 경로 | 차단 가드 | 도달 판정 |
| --- | --- | --- | --- | --- | :---: |
| V1 | workflow run 성공과 Summary의 `Published staging tag` | `.github/workflows/publish-api-gateway-image.yml`, `publish-clipboard-image.yml`의 Summary 단계 | `workflow_dispatch` -> checkout -> build -> Harbor push -> Summary 기록 | Harbor secret 누락, robot 권한 부족, 빌드 실패 | 도달 가능 |
| V2 | `docker manifest inspect` 종료 코드 0 | Harbor 레지스트리 API | 인증된 `docker manifest inspect` -> Harbor `orchid/<service>` 매니페스트 조회 | `orchid` 프로젝트 부재, robot 권한 부족 | 도달 가능 |
| V3 | Pod 이벤트 `Successfully pulled image` | kubelet 이미지 pull | overlay `newTag` 반영 푸시 -> Flux 적용 -> kubelet pull | pull Secret 권한, 태그 불일치, Flux 미반영 | 도달 가능 |

## 사전 조건

```bash
# 사전 조회 (2026-10-14, Hanul `agents-mcp` 컨텍스트)
kubectl get namespaces
# orchid-staging 네임스페이스는 Garden.Cluster bootstrap 이 만든다
```

| 항목 | 확인 방법 | 기대값 | 상태 |
| --- | --- | --- | --- |
| 선행 계획 구현 | `docs/history/2610/W3/261014/261014_feat_orchid-harbor-image-publish.md` H1 | `✅` | 충족(2026-10-15, 아카이브됨) |
| Harbor `orchid` 프로젝트와 robot | Harbor UI 프로젝트 목록과 Robot Accounts | `orchid` 프로젝트, `ci-push`와 `orchid-pull-flux` robot | 충족: 선행 계획 후속 검증 "자격 준비"(2026-10-14 20:43 KST)와 클러스터 pull 성공으로 확인 |
| GitHub Actions secret | `gh secret list --repo ibocon/Orchid` | `HARBOR_REGISTRY`, `HARBOR_USERNAME`, `HARBOR_PASSWORD` | 충족(2026-10-15, run-autopilot `261015-124010`): 이름 3개 모두 조회됨 |
| pull 자격 OpenBao 등록 | OpenBao `infra/harbor/orchid-registry-pull` 존재(값은 확인하지 않음) | 키 존재 | 충족: Pod가 `harbor-registry` pull secret으로 이미지를 받아 기동 중이라 키가 동기화돼 있다 |

## 사용자 실행 절차

### V1: publish workflow 실행

#### 에이전트 안내문

`api-gateway`와 `clipboard`의 publish workflow를 `main`에서 실행하고 성공 여부와 Summary의 불변 태그를 확인합니다.

#### 사용자 수행 동작

1. `dispatch-workflow` 절차의 단일 진입점으로 두 workflow를 실행합니다.
   ```powershell
   foreach ($Workflow in @('publish-api-gateway-image.yml', 'publish-clipboard-image.yml')) {
       $Arguments = @{ Repository = 'ibocon/Orchid'; Workflow = $Workflow; Ref = 'main' }
       & lib/Garden.Harness/src/git/scripts/Invoke-WorkflowDispatch.ps1 @Arguments
   }
   ```
2. 각 run의 Summary에 기록된 `Published staging tag` 값을 확인합니다.

#### 사용자 필수 답변

- 실행 시각
- 두 run의 결론(`success` 여부)과 Summary의 `Published staging tag` 값

#### 통과 기준

두 workflow run이 모두 `success`이고, 각 Summary의 태그가 `stag-<UTC 14자리>-<sha 7자리>` 형식이다.

#### 응답 대기 게이트

V1이 통과하기 전에는 V2를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V2로 진행한다.
- FAIL: 실패한 단계의 로그를 수집하고 `fix` 계획으로 승격한다.
- BLOCKED: secret 누락 또는 Harbor robot 권한 문제면 사전 조건 해소 후 재시도한다.

---

### V2: Harbor에 태그 존재 확인

#### 에이전트 안내문

V1 workflow가 푸시한 불변 태그가 Harbor `orchid` 프로젝트에 실제로 올라갔는지 확인합니다.

#### 사용자 수행 동작

1. V1 Summary의 태그로 각 서비스를 조회합니다.
   ```powershell
   docker manifest inspect harbor.hanul.botanicsoft.com/orchid/api-gateway:<태그>
   docker manifest inspect harbor.hanul.botanicsoft.com/orchid/clipboard:<태그>
   ```

#### 사용자 필수 답변

- 실행 시각
- 두 명령의 종료 코드

#### 통과 기준

두 명령이 모두 종료 코드 0으로 끝나고 매니페스트 JSON이 출력된다.

#### 응답 대기 게이트

V2가 통과하기 전에는 V3를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V3로 진행한다.
- FAIL: `unauthorized` 또는 `not found` 메시지를 기록하고 Harbor 프로젝트 권한을 점검한다.
- BLOCKED: Harbor 접근 불가 시 해소 조건을 기록한다.

---

### V3: 클러스터가 새 이미지를 pull

#### 에이전트 안내문

staging overlay가 V1 태그를 가리키도록 반영한 뒤 Pod가 이미지를 받는지 확인합니다.

#### 사용자 수행 동작

1. `deploy/k8s/overlays/staging/kustomization.yaml`의 두 이미지 `newTag`를 V1 Summary 태그로 바꾸고 커밋·푸시합니다.
2. Flux가 반영한 뒤 Pod 상태를 확인합니다.
   ```bash
   kubectl get pods -n orchid-staging -o wide
   ```

#### 사용자 필수 답변

- 실행 시각
- `kubectl get pods -n orchid-staging -o wide` 출력

#### 통과 기준

두 Pod의 `STATUS`에 `ImagePullBackOff`와 `ErrImagePull`이 없다. (Pod가 `Running 1/1`인지는 `orchid:261014_verify_orchid-hanul-gitops-deployment` V2에서 판정한다.)

#### 판정 후 다음 단계

- PASS: 모든 필수 시나리오 통과로 검증 계획서를 완료(`✅`)한다.
- FAIL: `kubectl describe pod`의 이벤트를 수집하고 `fix` 계획으로 승격한다.
- BLOCKED: Flux 미반영 시 `flux reconcile`을 사용자에게 안내한다.

---

## 증거 수집

| ID | 실행 시각 | 환경과 릴리스 | 입력 | 기대 결과 | 실제 결과 | 증거 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| V1 | 2026-10-15 02:10Z, 02:12Z (기존 run 읽기 전용 조회) | GitHub Actions `ibocon/Orchid` `main@0a80f65` | `gh run list`·`gh run view`로 기존 `workflow_dispatch` run 조회(재디스패치로 새 이미지를 푸시하지 않음) | run 성공 2건과 불변 태그 형식 | run `38669605760`(publish-api-gateway-image) `success`, run `38669763685`(publish-clipboard-image) `success`. Summary 단계 둘 다 `success`. 태그 `stag-20261015021051-0a80f65`, `stag-20261015021237-0a80f65` | `Published staging tag` 로그 줄과 run 결론 | PASS |
| V2 | 2026-10-15 (Pod 상태 조회) | Harbor `orchid` 프로젝트 | 로컬 Docker 엔진이 500을 반환해 `docker manifest inspect`를 쓰지 못함. 대체로 kubelet이 Harbor에서 받은 Pod `imageID` digest를 조회 | 두 태그가 Harbor에 존재 | `orchid/api-gateway@sha256:d9b3fdead6c81bfa2270ddf3b070712a39956b1830d74d4a08d590cad8f5d461`, `orchid/clipboard@sha256:c136fd32ee4b4cad8cff996a61d35d0bb7120c7cfa3d027cbaf8eeb58a6d699d` | Kubernetes `pods` 응답의 `containerStatuses[].imageID`. 방법이 계획과 다르지만 레지스트리가 해당 태그를 digest로 해석해 이미지를 내려준 사실이라 존재 관찰값을 충족한다 | PASS |
| V3 | 2026-10-15 (Pod 상태 조회) | Hanul Kubernetes `orchid-staging` | Kubernetes API `pods` 조회. staging overlay `newTag`가 V1 태그와 같음 | pull 오류 없음 | 두 Pod `phase Running`, `ready true`, `restartCount 0`, `ImagePullBackOff`·`ErrImagePull` 없음. overlay `newTag` 둘 다 V1 태그와 일치 | Pod 2개 `containerStatuses` 필드 | PASS |

증거에는 비밀값과 자격 정보를 제거한다.

## 판정 결과

| 결과 | 시나리오 | 근거 | 후속 조치 |
| --- | --- | --- | --- |
| PASS | V1, V2, V3 | 두 workflow run 성공과 불변 태그 확인, digest로 Harbor 이미지 존재 확인, Pod 2개 이미지 pull 후 Running. 에이전트가 증거를 통과 기준과 대조해 판정했다(run-autopilot `261015-124010`). V2 확인 방법은 로컬 Docker 엔진 장애로 대체했다 | 없음. `orchid:261014_verify_orchid-gateway-proxy-routing` 의 실행 의존(`verification-pass:V3`)이 충족됐다 |

## 결함 승격

| 실패 시나리오 | 변경 유형 | 후속 계획 | 상태 |
| --- | --- | --- | --- |
| — | — | — | 해당 없음 |

## 작업 순서

1. ✅ 사전 조건을 확인한다.
2. ✅ V1 run 결과와 태그를 수집한다.
3. ✅ V2와 V3를 순차적으로 진행한다.
4. ✅ 모든 시나리오 결과를 기록하고 완료한다.
