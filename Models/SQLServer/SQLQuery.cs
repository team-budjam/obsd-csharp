using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.SQLServer;


// Value
public readonly record struct SQLQuery()
{
    // core
    public Guid Id { get;  } = Guid.NewGuid();
    public required string Value { get; init; }
}