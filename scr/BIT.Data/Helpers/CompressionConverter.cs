﻿using DevExpress.Xpo.Metadata;
using System;
using System.IO;

namespace BIT.Data.Helpers
{
    public class CompressionConverter : ValueConverter
    {
        public override object ConvertToStorageType(object value)
        {
            if (value != null && !(value is byte[]))
            {
                throw new ArgumentException();
            }
            if (value == null || ((byte[])value).Length == 0)
            {
                return value;
            }
            return CompressionUtils.Compress(new MemoryStream((byte[])value)).ToArray();
        }
        public override object ConvertFromStorageType(object value)
        {
            if (value != null && !(value is byte[]))
            {
                throw new ArgumentException();
            }
            if (value == null || ((byte[])value).Length == 0)
            {
                return value;
            }
            return CompressionUtils.Decompress(new MemoryStream((byte[])value)).ToArray();
        }
        public override Type StorageType
        {
            get { return typeof(byte[]); }
        }
    }
}
