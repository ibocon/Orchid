namespace Orchid.Contract.Model;

/// <summary>
/// 클립보드 동기화 이벤트 액션 유형을 정의합니다.
/// </summary>
public enum SyncAction
{
    /// <summary>
    /// 신규 클립보드 항목 생성
    /// </summary>
    Created = 1,

    /// <summary>
    /// 기존 클립보드 항목 수정 (즐겨찾기, 태그 등)
    /// </summary>
    Updated = 2,

    /// <summary>
    /// 클립보드 항목 삭제
    /// </summary>
    Deleted = 3
}
