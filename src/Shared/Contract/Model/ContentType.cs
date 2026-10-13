namespace Orchid.Contract.Model;

/// <summary>
/// 클립보드 데이터의 콘텐츠 형식을 정의합니다.
/// </summary>
public enum ContentType
{
    /// <summary>
    /// 일반 텍스트
    /// </summary>
    Text = 1,

    /// <summary>
    /// 이미지 데이터 (PNG/JPEG)
    /// </summary>
    Image = 2,

    /// <summary>
    /// 웹 URL 주소
    /// </summary>
    Url = 3,

    /// <summary>
    /// 소스 코드 스니펫
    /// </summary>
    Code = 4
}
