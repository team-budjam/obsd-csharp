using SixLabors.ImageSharp;
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


    // action
    public void Remove()
    {
        Console.WriteLine("객체가 제거됩니다.");
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