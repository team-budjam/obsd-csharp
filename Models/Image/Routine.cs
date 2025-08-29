namespace Models.Image;


// Routine
public static class Routine
{
    public static void ProcessPartyPNGInDownloads()
    {
        // 1) Windows 사용자 프로필 아래의 Downloads 폴더 계산
        var downloads = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );

        // 2) 원본 파일: Downloads\party.png
        var file = new ImageFilePath
        {
            Base = downloads,
            Name = "party",
            Extension = ImageFilePath.Ext.PNG
        };

        // 3) Processor 생성 후 경로 주입
        var processor = Processor.Create();
        processor.PathModel = file;

        // 4) 썸네일 생성 실행
        try
        {
            Console.WriteLine($"Source: {file}");
            processor.MakeThumbnail();
            Console.WriteLine("완료: 썸네일은 같은 폴더에 'pary_thumb.png'로 저장됩니다.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("썸네일 생성 중 오류:");
            Console.WriteLine(ex);
        }
    }
}
