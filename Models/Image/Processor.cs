using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Image;

// Object
public class Processor: IDisposable
{

    // action
    public void Dispose()
    {
        Console.WriteLine("객체가 정리됩니다.");
    }
}


// ObjectManager