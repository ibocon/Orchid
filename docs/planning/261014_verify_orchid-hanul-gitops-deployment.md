# ⬜ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증

## 계획 관계

> **관계 스키마**: `1`
> **계획 ID**: `orchid:261014_verify_orchid-hanul-gitops-deployment`
> **출처 판정**: `연결`

| 관계 | 대상 계획 ID | 조건 | 근거·인수 범위 |
| --- | --- | --- | --- |
| 검증 대상 | `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` | 해당 없음 | Orchid 백엔드 서비스 GitOps 배포 매니페스트 후속 검증 |

<!-- plan-relations-summary:start -->
> 자동 생성 관계 요약. 원본은 위 관계 표입니다.

| 구분 | 계획·조건 |
| --- | --- |
| 최초 계획 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` |
| 직접 출처 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` |
| 직접 후속 | 없음 |
| 계보 경로 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` → [⬜ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증](<261014_verify_orchid-hanul-gitops-deployment.md>) `orchid:261014_verify_orchid-hanul-gitops-deployment` |
| 검증 대상 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` — 해당 없음 |
| 착수 조건 | 가능 |
<!-- plan-relations-summary:end -->

## 선행 계획

| 계획 | 계획 ID | 완료 상태 | 이관한 후속 검증 |
| --- | --- | --- | --- |
| [Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](../history/2610/W3/261014/261014_ops_orchid-service-gitops-manifest-onboarding.md) | `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` | ✅ | Ingress 접속 및 E2EE 웹소켓 접속 검증 |

## 배경

- 선행 계획 `261014_ops_orchid-service-gitops-manifest-onboarding`을 통해 Orchid 저장소 내 `deploy/k8s/` 구조(Base, Overlays)가 확립되었습니다.
- 본 계획서는 Hanul Kubernetes 클러스터 접근 환경에서 Orchid 서비스(`orchid-api-gateway`, `orchid-clipboard`)가 정상 배포되고, 외부 클라이언트와의 통신이 원활한지 사용자가 직접 검증하는 절차를 기술합니다.

## 검증 범위

| ID | 실행 주체 | 환경 | 대상 | 사용자 시나리오 | 필수 |
| --- | --- | --- | --- | --- | :---: |
| V1 | 사용자 | Hanul Kubernetes | `orchid-staging` | ExternalSecret 시크릿 동기화 확인 | 예 |
| V2 | 사용자 | Hanul Kubernetes | `orchid-staging` | ApiGateway 및 Clipboard 서비스 헬스체크 확인 | 예 |
| V3 | 사용자 | 외부 네트워크 | `orchid.stag.botanicsoft.com` | Ingress TLS 인증서 및 HTTPS 응답 확인 | 예 |

## 신호 도달 가능성

| ID | 판정 신호 | 발화 지점 | 조작에서 발화까지의 경로 | 차단 가드 | 도달 판정 |
| --- | --- | --- | --- | --- | :---: |
| V1 | Secret 동기화 성공 상태 | `kubectl get externalsecrets -n orchid-staging` | OpenBao 연동 -> ExternalSecrets Operator Reconcile | OpenBao 경로 누락 | 도달 가능 |
| V2 | Pod 1/1 Ready | `kubectl get pods -n orchid-staging` | Deployment 스케줄링 -> Kubelet 컨테이너 기동 | 이미지 풀 실패 | 도달 가능 |
| V3 | HTTP 200 OK 및 SSL 유효 | `curl -Iv https://orchid.stag.botanicsoft.com/health` | 클라이언트 -> Cloudflare/Ingress-Nginx -> Gateway Service | DNS 미전파 | 도달 가능 |

## 사전 조건

| 항목 | 확인 방법 | 기대값 | 상태 |
| --- | --- | --- | --- |
| Kubeconfig 컨텍스트 | `kubectl config current-context` | Hanul 클러스터 컨텍스트 | 미충족 |
| OpenBao 시크릿 등록 | OpenBao `app/stag/orchid/*` 경로 | 필수 시크릿 주입 완료 | 미충족 |

## 사용자 실행 절차

### V1: ExternalSecret 시크릿 동기화 확인

#### 에이전트 안내문

OpenBao에서 Orchid 시크릿을 가져와 Kubernetes Secret으로 정상 생성했는지 확인합니다.

#### 사용자 수행 동작

1. 다음 명령을 실행하여 ExternalSecret 상태를 확인합니다:
   ```bash
   kubectl get externalsecrets -n orchid-staging
   ```

#### 사용자 필수 답변

- 실행 시각
- `kubectl get externalsecrets -n orchid-staging` 출력 텍스트

#### 통과 기준

`orchid-api-gateway-external-secret` 및 `orchid-clipboard-external-secret`의 `STATUS`가 `SecretSynced`이다.

#### 응답 대기 게이트

V1이 통과하기 전에는 V2를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V2 단계로 진행한다.
- FAIL: OpenBao `app/stag/orchid/*` 경로에 등록된 키를 점검한다.
- BLOCKED: ClusterSecretStore 연결 실패 시 인프라 상태를 확인한다.

---

### V2: ApiGateway 및 Clipboard 서비스 헬스체크 확인

#### 에이전트 안내문

배포된 백엔드 Pod들이 정상 기동되어 내부 포트에서 트래픽을 수신할 준비가 되었는지 확인합니다.

#### 사용자 수행 동작

1. Pod 상태와 Service 엔드포인트를 확인합니다:
   ```bash
   kubectl get pods,svc -n orchid-staging -o wide
   ```

#### 사용자 필수 답변

- 실행 시각
- Pod 상태 출력 텍스트

#### 통과 기준

모든 Pod의 `STATUS`가 `Running`이고 `READY`가 `1/1`이다.

#### 응답 대기 게이트

V2가 통과하기 전에는 V3를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V3 단계로 진행한다.
- FAIL: `kubectl describe pod` 또는 `kubectl logs`를 수집한다.
- BLOCKED: 리소스 부족 시 대기한다.

---

### V3: Ingress TLS 인증서 및 HTTPS 응답 확인

#### 에이전트 안내문

공인 도메인(`orchid.stag.botanicsoft.com`)을 통해 Ingress가 트래픽을 수신하고 정상 응답하는지 확인합니다.

#### 사용자 수행 동작

1. 터미널에서 HTTPS 요청을 테스트합니다:
   ```bash
   curl -Ivk https://orchid.stag.botanicsoft.com/health
   ```

#### 사용자 필수 답변

- 실행 시각
- HTTP 응답 헤더 및 상태 코드

#### 통과 기준

HTTP 200 OK 응답이 반환되고 cert-manager가 발급한 인증서가 확인된다.

#### 판정 후 다음 단계

- PASS: 모든 필수 시나리오 통과로 검증 계획서를 완료(`✅`)한다.
- FAIL: Ingress-Nginx 라우팅 로그 및 DNS 레코드를 점검한다.
- BLOCKED: DNS 전파 대기 중일 경우 로컬 hosts 파일로 확인한다.

---

## 증거 수집

| ID | 실행 시각 | 환경과 릴리스 | 입력 | 기대 결과 | 실제 결과 | 증거 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | :---: |
| V1 | 2026-10-14 | Hanul Kubernetes (`agents-mcp`) | `kubectl get externalsecrets -n orchid-staging` | SecretSynced | 출력 없음. `orchid-staging` 네임스페이스 미존재. `orchid-application` GitRepository `READY False`: `secrets "orchid-deploy-key" not found` | namespaces 목록, `flux-system` gitrepositories 조회 | BLOCKED |
| V2 | — | Hanul Kubernetes | `kubectl get pods` | Running 1/1 | — | — | 미실행 |
| V3 | — | 외부 네트워크 | `curl https://...` | HTTP 200 OK | — | — | 미실행 |

## 판정 결과

| 결과 | 시나리오 | 근거 | 후속 조치 |
| --- | --- | --- | --- |
| BLOCKED | V1 | `orchid-deploy-key` Secret 미등록으로 Orchid 소스 동기화 실패, `orchid-staging` 미생성 | Garden.Cluster 검증 계획 V2(Deploy Key 등록) 수행 후 V1 재실행 |
| 미실행 | V2, V3 | V1 게이트 미통과 | V1 통과 후 진행 |

## 결함 승격

| 실패 시나리오 | 변경 유형 | 후속 계획 | 상태 |
| --- | --- | --- | --- |
| — | — | — | 해당 없음 |

## 작업 순서

1. 사전 조건을 확인한다.
2. V1 시나리오를 안내하고 사용자 증거를 수집한다.
3. V2 및 V3를 순차적으로 진행한다.
4. 모든 시나리오 결과를 기록하고 완료한다.
