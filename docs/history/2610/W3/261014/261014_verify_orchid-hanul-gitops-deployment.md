# ✅ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증

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
| 최초 계획 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` |
| 직접 출처 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` |
| 직접 후속 | [✅ Orchid ConfigMap의 appsettings.json이 컨테이너에 적용되지 않아 gateway 프록시가 동작하지 않는 문제 수정](<261014_fix_orchid-configmap-not-mounted.md>) `orchid:261014_fix_orchid-configmap-not-mounted`<br/>[✅ Orchid GitOps 매니페스트가 Hanul 클러스터 API와 맞지 않아 배포가 실패하는 문제 수정](<261014_fix_orchid-gitops-manifest-cluster-compat.md>) `orchid:261014_fix_orchid-gitops-manifest-cluster-compat`<br/>[✅ Orchid staging Pod가 Harbor 이미지 pull 인증 없이 ImagePullBackOff로 기동하지 못하는 문제 수정](<261014_fix_orchid-harbor-image-pull-secret.md>) `orchid:261014_fix_orchid-harbor-image-pull-secret`<br/>[🔄 Orchid production overlay 네임스페이스가 클러스터에 없고 staging 이름 규칙과 달라 적용되지 않는 문제 수정](<../../../../planning/261014_fix_orchid-production-namespace-name.md>) `orchid:261014_fix_orchid-production-namespace-name` |
| 계보 경로 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` → [✅ Orchid 서비스 Hanul GitOps 배포 사용자 실행 검증](<261014_verify_orchid-hanul-gitops-deployment.md>) `orchid:261014_verify_orchid-hanul-gitops-deployment` |
| 검증 대상 | [✅ Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](<261014_ops_orchid-service-gitops-manifest-onboarding.md>) `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` — 해당 없음 |
| 착수 조건 | 가능 |
<!-- plan-relations-summary:end -->

## 선행 계획

| 계획 | 계획 ID | 완료 상태 | 이관한 후속 검증 |
| --- | --- | --- | --- |
| [Orchid 서비스 독립 GitOps 배포 매니페스트 구축 및 Hanul 온보딩 계획](261014_ops_orchid-service-gitops-manifest-onboarding.md) | `orchid:261014_ops_orchid-service-gitops-manifest-onboarding` | ✅ | Ingress 접속 및 E2EE 웹소켓 접속 검증 |

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
| V3 | HTTP 200 OK 및 SSL 유효 | `curl -sv https://orchid.stag.botanicsoft.com/health` | 클라이언트 -> Cloudflare/Ingress-Nginx -> Gateway Service | DNS 미전파 | 도달 가능 |

## 사전 조건

| 항목 | 확인 방법 | 기대값 | 상태 |
| --- | --- | --- | --- |
| Kubeconfig 컨텍스트 | `kubectl config current-context` | Hanul 클러스터 컨텍스트 | 충족(2026-10-15): Kubernetes MCP가 Hanul `agents-mcp` 컨텍스트로 조회된다. Bash 셸의 kubectl에는 컨텍스트가 없다 |
| OpenBao 시크릿 등록 | OpenBao `app/stag/orchid/*` 경로 | 필수 시크릿 주입 완료 | 충족: V1의 ExternalSecret 2건이 `SecretSynced` True |

```bash
# 사전 조회 (2026-10-14, Hanul `agents-mcp` 컨텍스트)
kubectl get namespaces
# orchid-staging 네임스페이스는 Garden.Cluster bootstrap/namespaces/orchid-staging.yaml 로 생성된다
```

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
   curl -sv https://orchid.stag.botanicsoft.com/health
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
| V1 | 2026-10-14 | Hanul Kubernetes (`agents-mcp`) | `kubectl get externalsecrets -n orchid-staging` | SecretSynced | 3건 모두 `STATUS SecretSynced`, `READY True`: `orchid-api-gateway-external-secret`(LAST SYNC 13m), `orchid-clipboard-external-secret`(13m), `harbor-registry`(38m). 사용자가 OpenBao `app/stag/orchid/{api-gateway,clipboard}`에 `PLACEHOLDER`를 등록한 뒤 동기화됨. ExternalSecret 이벤트 오류 없음 | ExternalSecret 상태 조회 결과 | PASS |
| V2 | 2026-10-15 (run-autopilot `261015-124010`) | Hanul Kubernetes `orchid-staging`, 이미지 `stag-20261015021051-0a80f65`·`stag-20261015021237-0a80f65` | Kubernetes API `pods,svc` 조회 | Running 1/1 | `orchid-api-gateway-5d8cc47957-fnsvb` `1/1 Running`, `orchid-clipboard-86488b96b8-xwszk` `1/1 Running`, 둘 다 RESTARTS 0, AGE 104m. Service `orchid-api-gateway-service`·`orchid-clipboard-service` ClusterIP 8080 | Pod·Service 조회 출력 | PASS |
| V3 | 2026-10-15T04:03:12Z | 외부 네트워크 | `curl -sv https://orchid.stag.botanicsoft.com/health`(검증 생략 옵션 없음) | HTTP 200 OK, cert-manager 인증서 | HTTP `200`, 본문 `{"status":"Healthy","service":"Orchid.ApiGateway"}`. 인증서 Subject `CN=orchid.stag.botanicsoft.com`, Issuer `Let's Encrypt YR2`, 만료 2027-01-12, `ssl_verify_result` 0(검증 성공). Certificate `orchid-stag-tls` READY True, Issuer `lets-encrypt`. 계획 원문의 `curl -I`(HEAD)는 `/health`가 405로 거부하므로 GET으로 대체 | curl 출력과 Certificate 조회 | PASS |

## 판정 결과

| 결과 | 시나리오 | 근거 | 후속 조치 |
| --- | --- | --- | --- |
| PASS | V1 | 3차 실행. OpenBao `app/stag/orchid/*` 등록 후 ExternalSecret 2건이 `SecretSynced`. 1차 FAIL(`v1beta1`)은 fix로, 2차 BLOCKED(시크릿 부재)는 사용자 등록으로 해소됨 | V2 진행 |
| PASS | V2, V3 | 이미지 퍼블리시(`orchid:261014_feat_orchid-harbor-image-publish`) 후 재실행. Pod 2개 `1/1 Running`, `/health` GET 200과 Let's Encrypt 인증서 검증 성공. 에이전트가 증거를 통과 기준과 대조해 판정했다(run-autopilot `261015-124010`). V3 명령은 HEAD가 405라 GET으로 바꿔 실행했다 | 없음 |

## 결함 승격

| 실패 시나리오 | 변경 유형 | 후속 계획 | 상태 |
| --- | --- | --- | --- |
| V1 | `fix` | [✅ Orchid GitOps 매니페스트가 Hanul 클러스터 API와 맞지 않아 배포가 실패하는 문제 수정](<../history/2610/W3/261014/261014_fix_orchid-gitops-manifest-cluster-compat.md>) `orchid:261014_fix_orchid-gitops-manifest-cluster-compat` | 구현 완료, Flux 적용 확인. V1은 OpenBao 시크릿 등록 후 재검증 |

## 작업 순서

1. ✅ 사전 조건을 확인한다.
2. ✅ V1 시나리오를 안내하고 사용자 증거를 수집한다.
3. ✅ V2 및 V3를 순차적으로 진행한다.
4. ✅ 모든 시나리오 결과를 기록하고 완료한다.
