# 📦 Orchid.Contract (`src/Shared/Contract`)

> 클라이언트(Desktop/Mobile/Web)와 백엔드(ApiGateway/Clipboard) 전반에서 공유하는 **불변 통신 DTO 및 E2EE 직렬화 계약 라이브러리**

---

## 1. 프로젝트 역할 및 책임
- **역할**: 클라이언트-서버 간 데이터 교환 시 사용하는 암호화 페이로드, DTO, 열거형(Enum) 정의.
- **특징**: 특정 프레임워크나 외부 종속성 없이 순수 C# Record 타입으로 구성되어 초경량 배포 및 직렬화 성능 보장.

---

## 2. 폴더 및 디렉터리 구조

```text
src/Shared/Contract/
├── Payload/                     # E2EE 암호화 전송 블롭
│   ├── EncryptedPayload.cs      # 암호화된 바이트 데이터, Nonce, 태그, 암호화 알고리즘 식별자
│   └── KeyExchangePayload.cs    # 디바이스 페어링용 ECDH 공개키 교환 DTO
├── Model/                       # 공유 직렬화 모델
│   ├── ContentType.cs           # 텍스트, 코드, 이미지(PNG/JPEG), URL 구분 열거형
│   ├── DevicePlatform.cs        # Windows, macOS, Android, iOS, Web 구분 열거형
│   └── SyncAction.cs            # 생성(Created), 수정(Updated), 삭제(Deleted) 이벤트 구분
├── Request/                     # REST / RPC API 요청 DTO
│   ├── PushClipboardRequest.cs  # 암호화 클립보드 블롭 업로드 요청
│   └── PullClipboardRequest.cs  # 증분(Delta) 동기화 요청
├── Response/                    # API 응답 DTO
│   ├── ClipboardItemSummary.cs  # 클립보드 메타데이터 요약 (원문 미포함)
│   └── SyncSessionResponse.cs   # 실시간 세션 정보
├── Orchid.Contract.csproj       # 프로젝트 파일 (.NET 9)
└── README.md                    # 본 설명 문서
```
