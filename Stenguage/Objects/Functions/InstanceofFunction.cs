﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Stenguage.Objects.Functions
{
    public class InstanceofFunction : CSharpFunction
    {
        public InstanceofFunction() : base("instanceof", new Dictionary<string, ObjectType>() { ["value"] = new ObjectType(typeof(Object)) })
        { }

        public override RuntimeResult Execute(Context context)
        {
            Type type = context.SymbolTable.Get("value").GetType();
            if (!new Type[] { typeof(Number), typeof(String), typeof(List), typeof(ObjectType), typeof(Function) }.Contains(type))
            {
                type = type.BaseType;
            }
            return new RuntimeResult().Success(new ObjectType(type));
        }

        public override Object Copy()
        {
            InstanceofFunction copy = new InstanceofFunction();
            copy.SetPosition(Start, End);
            copy.SetContext(Context);
            return copy;
        }
    }
}