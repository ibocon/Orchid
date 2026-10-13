# 📱 Orchid.Mobile (`src/Mobile`)

> **Android & iOS** 모바일 환경을 지원하는 **.NET MAUI Blazor Hybrid 모바일 애플리케이션**

---

## 1. 프로젝트 역할 및 책임
- **역할**: 모바일 사용자를 위한 클립보드 공유, 백그라운드 푸시 수신, iOS 샌드박스 극복 솔루션 제공.
- **특징**:
  - `Peony/src/Mobile`의 실무 설계를 계승하여 Android와 iOS 네이티브 생명주기를 완벽히 제어.
  - **Android**: `AccessibilityService`를 통해 백그라운드 클립보드 변경을 자동 감지하여 PC로 즉시 송신.
  - **iOS**: Apple 보안 정책에 맞추어 `Share Extension`(공유 시트)을 통한 원클릭 수동 전송 및 `APNs` 무음 푸시 수신 시 즉시 클립보드 쓰기 파이프라인 탑재.

---

## 2. 폴더 및 디렉터리 구조

```text
src/Mobile/
├── Platforms/                   # 모바일 OS별 네이티브 서비스 구현부
│   ├── Android/                 # Android 전용
│   │   ├── ClipboardAccessibilityService.cs # 백그라운드 클립보드 자동 수집 서비스
│   │   ├── AndroidClipboardAdapter.cs       # Android ClipboardManager 바인딩
│   │   └── MainActivity.cs      # 네이티브 인텐트 및 액티비티 진입점
│   └── iOS/                     # iOS 전용
│       ├── IosClipboardAdapter.cs           # UIPasteboard.general 읽기/쓰기 바인딩
│       ├── ShareExtensionHandler.cs         # 사파리/앱 공유 시트 원클릭 전송 핸들러
│       └── AppDelegate.cs       # APNs 백그라운드 푸시 수신 진입점
├── Component/                   # 모바일 터치 최적화 Blazor UI 컴포넌트
│   ├── MobileHeader.razor       # 모바일 전용 앱바 및 동기화 상태 인디케이터
│   ├── SwipeableItem.razor      # 스와이프 제스처 삭제/즐겨찾기 카드
│   └── QrPairingScanner.razor   # PC 화면의 QR 코드를 스캔하는 카메라 컴포넌트
├── Service/                     # 모바일 호스트 서비스
│   ├── MobileSyncBackgroundWorker.cs # 백그라운드 수명주기 관리
│   └── MobileNotificationService.cs  # OS 푸시 알림 핸들러
├── wwwroot/                     # 모바일 터치 최적화 웹 자산
├── App.xaml / App.xaml.cs       # MAUI 애플리케이션 진입점
├── MainPage.xaml                # BlazorWebView 호스트 컨테이너
├── MauiProgram.cs               # DI 컨테이너 및 모바일 빌더 부트스트랩
├── Orchid.Mobile.csproj         # 프로젝트 파일 (.NET MAUI Blazor)
└── README.md                    # 본 설명 문서
```
