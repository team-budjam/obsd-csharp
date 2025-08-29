using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing; // Mutate, Resize, Grayscale
using System.Collections.Concurrent;

namespace Models.Image;

// Object
public class Processor
{
    // core
    public static Processor Create()
    {
        var processorRef = new Processor();

        ProcessorManager.Register(processorRef);

        return processorRef;
    }
    private Processor() { }
    internal void Delete()
    {
        ProcessorManager.Unregister(Id);
    }


    // state
    public ID Id { get; } = ID.Random();

    public ImageFilePath? PathModel { get; set; } = null;

    // action
    /// <summary>
    /// PathModel이 가리키는 이미지를 로드하여
    /// 1) 최대 320x320으로 리사이즈(비율 유지, 긴 변 기준)
    /// 2) 그레이스케일 처리
    /// 3) "{원본이름}_thumb.{확장자}"로 저장
    /// </summary>
    public void MakeThumbnail()
    {
        if (PathModel is null || PathModel.Value.Equals(default(ImageFilePath)))
            throw new InvalidOperationException("원본 이미지 경로(PathModel)가 비어 있습니다.");

        // ImageFilePath.ToString() -> Base + Name + Extension을 결합한 전체 경로
        var source = PathModel.Value.ToString();

        if (!File.Exists(source))
            throw new FileNotFoundException("원본 이미지 파일을 찾을 수 없습니다.", source);

        var dir = Path.GetDirectoryName(source) ?? "";
        var name = Path.GetFileNameWithoutExtension(source);
        var ext = Path.GetExtension(source); // ".jpg" 등
        var dest = Path.Combine(dir, $"{name}_thumb{ext}");

        const int max = 320;

        using var image = SixLabors.ImageSharp.Image.Load(source); // 포맷 자동 감지
        image.Mutate(x => x
            .Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,   // 긴 변 기준 축소, 비율 유지
                Size = new Size(max, max),
                Sampler = KnownResamplers.Lanczos3
            })
            .Grayscale()
        );

        image.Save(dest); // 확장자 기반으로 적절한 인코더 선택
        Console.WriteLine($"썸네일 생성 완료: {dest}");
    }
    public void Remove()
    {
        Console.WriteLine("객체가 제거됩니다.");

        this.Delete();
    }

    // value
    public readonly record struct ID
    {
        // core
        public required Guid RawValue { get; init; }

        public static ID Random() => new ID { RawValue = Guid.NewGuid() };
    }
}


// ObjectManager
internal static class ProcessorManager
{
    internal static ConcurrentDictionary<Processor.ID, Processor> container = new();
    internal static void Register(Processor obj)
    {
        container.TryAdd(obj.Id, obj);
    }
    internal static void Unregister(Processor.ID id)
    {
        container.TryRemove(id, out _);
    }
}