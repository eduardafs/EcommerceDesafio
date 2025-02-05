﻿using System.ComponentModel;
using System.Reflection;

namespace ECommerceBem.Core.Enum;
public static class EnumExtensions
{
    public static string GetDescription(this System.Enum value)
    {
        Type type = value.GetType();

        string name = System.Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
        }

        return name;
    }
}