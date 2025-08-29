namespace Models.Image;


// Value
public readonly record struct ImageFilePath
{
    // core
    public required string Base { get; init; }    // 디렉터리/경로
    public required string Name { get; init; }    // 파일 이름(확장자 제외)
    public required Ext Extension { get; init; }  // 확장자


    // operator
    public override string ToString()
    {
        // "C:\images\cat.png" 형태로 반환
        return Path.Combine(Base, $"{Name}.{Extension.RawValue}");
    }


    // value
    public readonly record struct Ext
    {
        // core
        public required string RawValue { get; init; }

        public static readonly Ext PNG = new() { RawValue = "png" };
        public static readonly Ext JPG = new() { RawValue = "jpg" };
        public static readonly Ext JPEG = new() { RawValue = "jpeg" };
        public static readonly Ext WEBP = new() { RawValue = "webp" };
    }
}
