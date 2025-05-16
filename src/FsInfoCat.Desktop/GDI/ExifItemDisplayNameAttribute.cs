﻿using System;

namespace FsInfoCat.Desktop.GDI
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class ExifItemDisplayNameAttribute(int index, string displayText) : Attribute
    {
        private readonly int _index = index;
        private readonly string _displayText = displayText;

        public int Index { get { return _index; } }
        public string DisplayText { get { return _displayText; } }
    }
}
