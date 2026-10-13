# 🌐 Orchid.Web (`src/Web`)

> 모든 브라우저에서 내 클립보드 히스토리를 확인하고 관리할 수 있는 **Blazor WebAssembly 대시보드**

---

## 1. 프로젝트 역할 및 책임
- **역할**: 앱 설치가 제한된 공용 PC나 웹 브라우저 환경에서 저장된 클립보드 내역을 안전하게 열람, 검색, 복사하는 반응형 웹 클라이언트.
- **특징**:
  - WebAssembly(WASM) 기반으로 클라이언트 사이드에서 직접 복호화(Web Crypto API / C# WASM) 수행.
  - 대용량 클립보드 히스토리도 부드럽게 스크롤할 수 있도록 `Virtualize` 컴포넌트 탑재.

---

## 2. 폴더 및 디렉터리 구조

```text
src/Web/
├── Pages/                       # 라우팅 페이지
│   ├── Index.razor              # 클립보드 타임라인 대시보드
│   ├── Bookmarks.razor          # 중요 보관함(핀) 전용 페이지
│   └── Devices.razor            # 페어링된 디바이스 관리 및 키 설정
├── Component/                   # 웹 전용 컴포넌트
│   ├── VirtualizedHistoryGrid.razor # 가상화 대용량 클립보드 그리드
│   ├── SearchBar.razor          # 실시간 텍스트 및 태그 필터 바
│   └── CodePreview.razor        # 프로그래밍 언어 구문 강조(Syntax Highlighting)
├── Service/                     # 브라우저 전용 서비스
│   ├── WebClipboardAdapter.cs   # navigator.clipboard JS interop 바인딩
│   └── WebKeyStorageService.cs  # IndexedDB / SessionStorage 보안 키 관리
├── wwwroot/                     # 정적 웹 자산
│   ├── css/tailwind.css
│   └── index.html               # SPA 호스트 HTML
├── App.razor                    # 라우팅 루트 컴포넌트
├── Program.cs                   # WebAssembly 부트스트랩 및 DI 등록
├── Orchid.Web.csproj            # 프로젝트 파일 (Blazor WASM)
└── README.md                    # 본 설명 문서
```
