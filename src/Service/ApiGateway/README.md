# 🚪 Orchid.ApiGateway (`src/Service/ApiGateway`)

> 마이크로서비스 진입점을 보호하고 실시간 세션을 라우팅하는 **YARP 기반 초고속 리버스 프록시 게이트웨이**

---

## 1. 프로젝트 역할 및 책임
- **역할**: 외부 클라이언트(Desktop/Mobile/Web)의 모든 요청을 단일 엔드포인트에서 수신하여 내부 서비스로 안전하게 포워딩.
- **특징**:
  - Microsoft **YARP(Yet Another Reverse Proxy)**를 기반으로 초당 수만 건의 트래픽을 지연 없이 중계.
  - 실시간 E2EE 스트리밍을 위한 WebSocket 및 SignalR 커넥션 풀링 유지.
  - Redis 분산 토큰 버킷 알고리즘을 활용한 DDoS 및 비정상 요청 Rate Limiting.

---

## 2. 폴더 및 디렉터리 구조

```text
src/Service/ApiGateway/
├── Middleware/                  # 게이트웨이 전용 파이프라인
│   ├── DeviceAuthenticationMiddleware.cs # 디바이스 토큰 및 서명 검증
│   ├── DistributedRateLimitingMiddleware.cs # Redis 기반 처리율 제한
│   └── CorrelationIdMiddleware.cs # 분산 추적용 W3C TraceContext 헤더 주입
├── Route/                       # YARP 동적 라우팅 구성
│   ├── RouteConfigurationSource.cs # 설정 파일 기반 동적 라우트 프로바이더
│   └── ClusterHealthCheckService.cs # 백엔드 노드 헬스체크 및 서킷 브레이커
├── Configuration/               # 옵션 바인딩 모델
│   ├── GatewayOptions.cs
│   └── RateLimitOptions.cs
├── appsettings.json             # YARP Routes & Clusters 정의
├── appsettings.Development.json
├── Program.cs                   # YARP 파이프라인 부트스트랩
├── Orchid.ApiGateway.csproj     # 프로젝트 파일 (.NET 9 Web)
└── README.md                    # 본 설명 문서
```
