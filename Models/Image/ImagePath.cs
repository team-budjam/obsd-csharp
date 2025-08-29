using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Models.Image;


// Value
public readonly record struct ImageFilePath
{
    // core
    public required string Name { get; init; }
    public required Ext Extension { get; init; }


    // operator
    override public string ToString()
    {
        return Name + "." + Extension.RawValue;
    }


    // value
    public readonly record struct Ext
    {
        // core
        public required string RawValue { get; init; }

        public static readonly Ext PNG = new() { RawValue = "png" };
        public static readonly 
            
            Ext JPG = new() { RawValue = "jpg" };
    }
}
