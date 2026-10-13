# ⚡ Orchid.Service.Clipboard (`src/Service/Clipboard`)

> Zero-Knowledge 암호화 블롭 릴레이 및 멀티 디바이스 동기화를 담당하는 **Clean Architecture & CQRS 백엔드 서비스**

---

## 1. 프로젝트 역할 및 책임
- **역할**: 클라이언트가 업로드한 E2EE 암호화 클립보드 데이터를 영속화하고, 사용자의 다른 연결된 기기들로 실시간 브로드캐스트(SignalR).
- **특징**:
  - **Zero-Knowledge 원칙**: 서버는 클립보드의 실제 내용(평문)을 절대 알 수 없으며, 오직 `EncryptedPayload`와 메타데이터(크기, 타입, 타임스탬프)만 다룸.
  - **Clean Architecture & CQRS**: 명령(Command)과 쿼리(Query)의 완벽한 분리 및 MediatR 파이프라인 적용.
  - **RabbitMQ 비동기 버스**: 대규모 동시 접속 환경에서도 지연 없는 분산 메시지 전달 보장.

---

## 2. 폴더 및 디렉터리 구조

```text
src/Service/Clipboard/
├── Domain/                      # 도메인 계층 (순수 비즈니스 규칙)
│   ├── Entity/
│   │   ├── ClipboardBlob.cs     # 암호화 블롭 엔티티 (불변식 보호)
│   │   └── DeviceSyncState.cs   # 기기별 최신 수신 커서 위치
│   ├── Event/
│   │   └── ClipboardPushedEvent.cs # 새 클립보드 도착 도메인 이벤트
│   └── Repository/
│       └── IClipboardBlobRepository.cs
├── Application/                 # 애플리케이션 계층 (CQRS Use Cases)
│   ├── Command/
│   │   └── PushClipboardCommand.cs # 암호화 데이터 수신 및 브로드캐스트
│   ├── Query/
│   │   └── GetClipboardHistoryQuery.cs # 증분 히스토리 조회
│   ├── Handler/
│   │   ├── PushClipboardCommandHandler.cs
│   │   └── GetClipboardHistoryQueryHandler.cs
│   └── Common/
│       └── IRealtimeBroadcastService.cs # SignalR 허브 추상화
├── Infrastructure/              # 인프라 계층 (외부 리소스 연동)
│   ├── Persistence/
│   │   ├── ClipboardDbContext.cs # EF Core PostgreSQL 매핑
│   │   └── EfClipboardRepository.cs
│   ├── Messaging/
│   │   └── RabbitMqEventPublisher.cs # RabbitMQ Outbox/이벤트 발행
│   └── Realtime/
│       └── ClipboardHub.cs      # SignalR 실시간 웹소켓 허브
├── Api/                         # 프레젠테이션 계층 (REST 엔드포인트)
│   ├── Controller/
│   │   └── ClipboardController.cs
│   └── Filter/
│       └── GlobalExceptionFilter.cs
├── appsettings.json
├── Program.cs                   # Clean Architecture DI 구성
├── Orchid.Service.Clipboard.csproj # 프로젝트 파일 (.NET 9 Web)
└── README.md                    # 본 설명 문서
```
