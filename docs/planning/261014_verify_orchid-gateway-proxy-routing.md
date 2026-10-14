# ⬜ Orchid api-gateway 프록시와 실시간 허브 경로 사용자 실행 검증

## 계획 관계

> **관계 스키마**: `1`
> **계획 ID**: `orchid:261014_verify_orchid-gateway-proxy-routing`
> **출처 판정**: `연결`

| 관계 | 대상 계획 ID | 조건 | 근거·인수 범위 |
| --- | --- | --- | --- |
| 검증 대상 | `orchid:261014_fix_orchid-configmap-not-mounted` | 해당 없음 | 후속 검증 2항목(이미지 전제, 프록시 동작)을 이관한다 |
| 실행 의존 | `orchid:261014_verify_orchid-image-publish-and-pull` | `verification-pass:V3` | 이미지가 푸시되고 Pod가 pull해야 gateway와 clipboard가 기동해 프록시를 확인할 수 있다 |

<!-- plan-relations-summary:start -->
<!-- plan-relations-summary:end -->

## 선행 계획

| 계획 | 계획 ID | 완료 상태 | 이관한 후속 검증 |
| --- | --- | --- | --- |
| [Orchid ConfigMap의 appsettings.json이 컨테이너에 적용되지 않아 gateway 프록시가 동작하지 않는 문제 수정](261014_fix_orchid-configmap-not-mounted.md) | `orchid:261014_fix_orchid-configmap-not-mounted` | ⬜ | 이미지 전제, 프록시 동작 |

## 배경

- 선행 계획은 ConfigMap을 `/app/appsettings.json`으로 마운트해 gateway가 clipboard 서비스 주소(`http://orchid-clipboard-service:8080`)로 프록시하게 한다. 단위 테스트는 렌더링 결과의 정합성만 확인하며 실제 프록시 동작과 마운트 경로(`WORKDIR /app`) 전제는 확인하지 못한다.
- gateway 경로 `/api/clipboard/{**catch-all}`와 `/hubs/clipboard/{**catch-all}`은 clipboard 서비스의 `POST /api/clipboard/push`와 SignalR 허브 `/hubs/clipboard`로 전달돼야 한다.
- 이 계획은 선행 계획 구현과 이미지 pull 검증(`orchid:261014_verify_orchid-image-publish-and-pull` V3 PASS)이 끝난 뒤 실행한다.

## 검증 범위

| ID | 실행 주체 | 환경 | 대상 | 사용자 시나리오 | 필수 |
| --- | --- | --- | --- | --- | :---: |
| V1 | 사용자 | Hanul Kubernetes | `orchid-staging` | 두 서비스 Pod가 Ready이고 마운트된 설정 파일이 존재한다 | 예 |
| V2 | 사용자 | 외부 네트워크 | `orchid.stag.botanicsoft.com` | gateway를 거친 `/api/clipboard/push` 요청이 clipboard에 도달한다 | 예 |
| V3 | 사용자 | 외부 네트워크 | `orchid.stag.botanicsoft.com` | gateway를 거친 SignalR negotiate가 WebSocket 전송을 제안한다 | 예 |

## 신호 도달 가능성

| ID | 판정 신호 | 발화 지점 | 조작에서 발화까지의 경로 | 차단 가드 | 도달 판정 |
| --- | --- | --- | --- | --- | :---: |
| V1 | Pod `READY 1/1`과 `/app/appsettings.json` 존재 | `deploy/k8s/base/*/deployment.yaml`의 `volumeMounts` | Deployment 마운트 -> 컨테이너 파일시스템 | 이미지 `WORKDIR`가 `/app`이 아님, 시크릿 미동기화 | 도달 가능 |
| V2 | HTTP 상태 코드가 5xx가 아님 | `src/Service/Clipboard/Api/Controller/ClipboardController.cs`의 `Push` | Ingress -> gateway `/api/clipboard/{**catch-all}` -> clipboard `POST /api/clipboard/push` | 목적지 주소 오기, clipboard Pod 미기동 | 도달 가능 |
| V3 | negotiate 응답의 `availableTransports`에 `WebSockets` | `src/Service/Clipboard/Program.cs`의 `MapHub<ClipboardHub>("/hubs/clipboard")` | Ingress -> gateway `/hubs/clipboard/{**catch-all}` -> SignalR negotiate | Ingress websocket 어노테이션, 라우트 누락 | 도달 가능 |

## 사전 조건

```bash
# 사전 조회 (2026-10-14, Hanul `agents-mcp` 컨텍스트)
kubectl get namespaces
# orchid-staging 네임스페이스는 Garden.Cluster bootstrap 이 만든다
```

| 항목 | 확인 방법 | 기대값 | 상태 |
| --- | --- | --- | --- |
| 이미지 pull 검증 | `orchid:261014_verify_orchid-image-publish-and-pull` V3 | `PASS` | 미충족 |
| 선행 fix 구현 | `docs/planning/261014_fix_orchid-configmap-not-mounted.md` H1 | `✅` | 미충족 |
| 앱 시크릿 동기화 | `kubectl get externalsecrets -n orchid-staging` | `SecretSynced` | 미충족 |

## 사용자 실행 절차

### V1: Pod Ready와 설정 파일 마운트

#### 에이전트 안내문

두 서비스 Pod가 Ready이고 ConfigMap이 설정 파일로 마운트됐는지 확인합니다.

#### 사용자 수행 동작

1. Pod 상태를 확인합니다.
   ```bash
   kubectl get pods -n orchid-staging -o wide
   ```
2. gateway 컨테이너의 설정 파일에서 clipboard 목적지를 확인합니다.
   ```bash
   kubectl exec -n orchid-staging deploy/orchid-api-gateway -- grep orchid-clipboard-service /app/appsettings.json
   ```

#### 사용자 필수 답변

- 실행 시각
- 두 명령의 출력

#### 통과 기준

두 Pod가 `Running`이고 `READY`가 `1/1`이며, 두 번째 명령이 `http://orchid-clipboard-service:8080`을 포함한 줄을 출력한다.

#### 응답 대기 게이트

V1이 통과하기 전에는 V2를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V2로 진행한다.
- FAIL: `kubectl describe pod`와 `kubectl logs`를 수집하고 `fix` 계획으로 승격한다. 파일이 없으면 이미지 `WORKDIR`를 점검한다.
- BLOCKED: 이미지 pull 또는 시크릿 문제로 Pod가 기동하지 않으면 해소 조건을 기록한다.

---

### V2: gateway를 통한 clipboard API 도달

#### 에이전트 안내문

외부 주소로 보낸 요청이 gateway를 거쳐 clipboard 서비스에 도달하는지 확인합니다. 빈 JSON 본문을 보내 clipboard가 직접 응답하는 4xx와 gateway의 프록시 실패(502·503·504)를 구분합니다.

#### 사용자 수행 동작

1. 터미널에서 요청을 보냅니다.
   ```bash
   curl -s -o /dev/null -w "%{http_code}\n" -X POST -H "Content-Type: application/json" -d "{}" https://orchid.stag.botanicsoft.com/api/clipboard/push
   ```

#### 사용자 필수 답변

- 실행 시각
- 출력된 HTTP 상태 코드

#### 통과 기준

상태 코드가 `200` 또는 `400`이다. `502`, `503`, `504`이면 프록시 실패이다.

#### 응답 대기 게이트

V2가 통과하기 전에는 V3를 안내하지 않는다.

#### 판정 후 다음 단계

- PASS: V3로 진행한다.
- FAIL: 상태 코드와 gateway 로그를 수집하고 `fix` 계획으로 승격한다.
- BLOCKED: DNS 또는 인증서 문제로 접속 불가면 해소 조건을 기록한다.

---

### V3: SignalR negotiate의 WebSocket 전송

#### 에이전트 안내문

실시간 허브 경로가 gateway를 통해 SignalR negotiate에 응답하고 WebSocket 전송을 제안하는지 확인합니다.

#### 사용자 수행 동작

1. negotiate 요청을 보냅니다.
   ```bash
   curl -s -X POST "https://orchid.stag.botanicsoft.com/hubs/clipboard/negotiate?negotiateVersion=1"
   ```

#### 사용자 필수 답변

- 실행 시각
- 응답 본문(JSON). `connectionId` 값은 가리고 전송 목록만 남겨도 된다.

#### 통과 기준

응답 JSON의 `availableTransports`에 `transport`가 `WebSockets`인 항목이 있다.

#### 판정 후 다음 단계

- PASS: 모든 필수 시나리오 통과로 검증 계획서를 완료(`✅`)한다.
- FAIL: 응답 본문과 Ingress·gateway 로그를 수집하고 `fix` 계획으로 승격한다.
- BLOCKED: 접속 불가 시 해소 조건을 기록한다.

---

## 증거 수집

| ID | 실행 시각 | 환경과 릴리스 | 입력 | 기대 결과 | 실제 결과 | 증거 | 판정 |
| --- | --- | --- | --- | --- | --- | --- | --- |
| V1 | — | Hanul Kubernetes | `kubectl get pods`와 `kubectl exec` | Ready, 설정 파일 확인 | — | — | 미실행 |
| V2 | — | 외부 네트워크 | `POST /api/clipboard/push` | 200 또는 400 | — | — | 미실행 |
| V3 | — | 외부 네트워크 | `POST /hubs/clipboard/negotiate` | `WebSockets` 전송 포함 | — | — | 미실행 |

증거에는 `connectionId`와 비밀값을 제거한다.

## 판정 결과

| 결과 | 시나리오 | 근거 | 후속 조치 |
| --- | --- | --- | --- |
| 미실행 | V1, V2, V3 | 이미지 pull 검증과 선행 fix 구현 대기 | 선행 조건 충족 후 사용자 실행 |

## 결함 승격

| 실패 시나리오 | 변경 유형 | 후속 계획 | 상태 |
| --- | --- | --- | --- |
| — | — | — | 해당 없음 |

## 작업 순서

1. 사전 조건을 확인한다.
2. V1을 안내하고 증거를 수집한다.
3. V2와 V3를 순차적으로 진행한다.
4. 모든 시나리오 결과를 기록하고 완료한다.
