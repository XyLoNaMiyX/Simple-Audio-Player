﻿using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    //propsys.h
    //http://msdn.microsoft.com/en-us/library/windows/desktop/bb761474(v=vs.85).aspx
    /// <summary>
    /// See "Functiondiscoverykeys_devpkey.h" for different keys like FriendlyName, DeviceDesc,...
    /// </summary>
    /// <remarks>For a list of all Core Audio Properties see: http://msdn.microsoft.com/en-us/library/windows/desktop/dd370806(v=vs.85).aspx </remarks>
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
    public class PropertyStore : ComObject, IEnumerable<KeyValuePair<PropertyKey, PropertyVariant>>
    {
        const string c = "IPropertyStore";

        public static readonly PropertyKey DeviceDesc = new PropertyKey(
            new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22),
            2);

        public static readonly PropertyKey DeviceInterfaceEnabled = new PropertyKey(
            new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22),
            3);

        public static readonly PropertyKey DeviceInterfaceClassGuid = new PropertyKey(
            new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22),
            4);

        public static readonly PropertyKey FriendlyName = new PropertyKey(
            new Guid("{a45c254e-df1c-4efd-8020-67d146a850e0}"),
            14);

        public PropertyStore(IntPtr ptr)
            : base(ptr)
        {
        }

        public int Count { get { return GetCount(); } }

        public PropertyVariant this[int index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        public PropertyVariant this[PropertyKey key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        public PropertyVariant GetValue(int index)
        {
            if (index >= GetCount())
                throw new ArgumentOutOfRangeException("index");
            return GetValue(GetKey(index));
        }

        public PropertyVariant GetValue(PropertyKey key)
        {
            PropertyVariant value;
            int result = GetValueInternal(key, out value);
            Win32ComException.Try(result, c, "GetValue");
            return value;
        }

        public PropertyKey GetKey(int index)
        {
            PropertyKey key;
            int result = GetAtInternal(index, out key);
            Win32ComException.Try(result, c, "GetAt");
            return key;
        }

        public void SetValue(int index, PropertyVariant value)
        {
            if (index >= GetCount())
                throw new ArgumentOutOfRangeException("index");
            SetValue(GetKey(index), value);
        }

        public void SetValue(PropertyKey key, PropertyVariant value)
        {
            int result = SetValueInternal(key, value);
            Win32ComException.Try(result, c, "SetValue");
        }

        public PropertyKey GetPropertyKey(int index)
        {
            if (index >= Count)
                throw new ArgumentOutOfRangeException("index");
            return GetKey(index);
        }

        unsafe int GetCountInternal(out int propertyCount)
        {
            fixed (void* ppc = &propertyCount)
            {
                return InteropCalls.CallI(UnsafeBasePtr, ppc, ((void**)(*(void**)UnsafeBasePtr))[3]);
            }
        }

        unsafe int GetAtInternal(int propertyIndex, out PropertyKey propertyKey)
        {
            propertyKey = new PropertyKey();
            fixed (void* ppk = &propertyKey)
            {
                return InteropCalls.CallI(UnsafeBasePtr, propertyIndex, ppk, ((void**)(*(void**)UnsafeBasePtr))[4]);
            }
        }

        unsafe int GetValueInternal(PropertyKey key, out PropertyVariant value)
        {
            fixed (void* pvalue = &value)
            {
                return InteropCalls.CallI(UnsafeBasePtr, &key, pvalue, ((void**)(*(void**)UnsafeBasePtr))[5]);
            }
        }

        unsafe int SetValueInternal(PropertyKey key, PropertyVariant value)
        {
            return InteropCalls.CallI(UnsafeBasePtr, &key, &value, ((void**)(*(void**)UnsafeBasePtr))[6]);
        }

        unsafe int CommitInternal()
        {
            return InteropCalls.CallI(UnsafeBasePtr, ((void**)(*(void**)UnsafeBasePtr))[7]);
        }

        int GetCount()
        {
            int count;
            Win32ComException.Try(GetCountInternal(out count), c, "GetCount");
            return count;
        }

        public IEnumerator<KeyValuePair<PropertyKey, PropertyVariant>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                var pair = new KeyValuePair<PropertyKey, PropertyVariant>(GetPropertyKey(i), GetValue(i));
                yield return pair;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}