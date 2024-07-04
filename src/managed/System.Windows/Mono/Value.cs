//
// Value.cs: represents the unmanaged Value structure from runtime.cpp
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2007 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using Moonlight;
using System.Windows.Controls;
using System.Diagnostics;

namespace Mono
{

    internal struct UnmanagedFontFamily
    {
        public IntPtr source;
    }

    internal struct UnmanagedFontWeight
    {
        public FontWeightKind weight;
    }

    internal struct UnmanagedFontStyle
    {
        public FontStyleKind style;
    }

    internal struct UnmanagedFontStretch
    {
        public FontStretchKind stretch;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct FontSourceUnion
    {
        [FieldOffset(0)] public IntPtr stream;
        [FieldOffset(0)] public IntPtr typeface;
    }

    internal struct UnmanagedFontSource
    {
        public FontSourceUnion source;
        public FontSourceType type;
    }

    internal struct UnmanagedStreamCallbacks
    {
        public IntPtr handle;
        public IntPtr CanSeek;
        public IntPtr CanRead;
        public IntPtr Length;
        public IntPtr Position;
        public IntPtr Read;
        public IntPtr Write;
        public IntPtr Seek;
        public IntPtr Close;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct UnmanagedPropertyPath
    {
        public IntPtr pathString;
        public IntPtr expandedPathString;
        public IntPtr property;
    }

    internal struct UnmanagedColor
    {
        public double r;
        public double g;
        public double b;
        public double a;

        public Color ToColor()
        {
            return Color.FromArgb((byte)(255 * a), (byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }
    }

    internal struct UnmanagedAudioFormat
    {
        public int bitsPerSample;
        public int channels;
        public int samplesPerSecond;
        public WaveFormatType waveFormat;

        public AudioFormat ToAudioFormat()
        {
            return new AudioFormat(bitsPerSample, channels, samplesPerSecond, waveFormat);
        }
    }

    internal struct UnmanagedVideoFormat
    {
        public float framesPerSecond;
        public int height;
        public int width;
        public int stride;
        public PixelFormatType pixelFormat;

        public VideoFormat ToVideoFormat()
        {
            return new VideoFormat(pixelFormat, width, height, framesPerSecond);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ManagedTypeInfo
    {
        public Kind Kind;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct ValUnion
    {
        [FieldOffset(0)] public float f;
        [FieldOffset(0)] public double d;
        [FieldOffset(0)] public long i64;
        [FieldOffset(0)] public ulong ui64;
        [FieldOffset(0)] public int i32;
        [FieldOffset(0)] public uint ui32;
        [FieldOffset(0)] public IntPtr p;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Value : IDisposable
    {
        // Note: Keep these flags in sync with the native version
        const uint NullMask = (uint)1 << 31;
        const uint GCHandleMask = (uint)1 << 30;
        const uint DoesNotNeedUnrefMask = (uint)1 << 29;
        const uint KindMask = (uint)0x1fffffff;

        uint k;
        GCHandle boxed_valuetype;
        internal ValUnion u;

        public Kind Kind
        {
            get => (Kind)(k & KindMask);
            set => k = (k & ~KindMask) | ((uint)value & KindMask);
        }

        public bool IsGCHandle
        {
            get => (k & GCHandleMask) == GCHandleMask;
            set
            {
                if (value)
                    k |= GCHandleMask;
                else
                    k &= ~GCHandleMask;
            }
        }

        public bool IsNull
        {
            get => (k & NullMask) == NullMask;
            set
            {
                if (value)
                    k |= NullMask;
                else
                    k &= ~NullMask;
            }
        }

        public static Value Empty => new Value { Kind = Kind.INVALID, IsNull = true };

        public static unsafe object ToObject(Type type, Value* value)
        {
            if (value == null || value->IsNull)
            {
                return null;
            }

            if (value->boxed_valuetype.IsAllocated)
            {
                return value->boxed_valuetype.Target;
            }

            if (value->IsGCHandle)
            {
                IntPtr managed_object = value->u.p;
                GCHandle handle = GCHandle.FromIntPtr(managed_object);
                if (type != null && handle.IsAllocated && handle.Target != null)
                    Debug.Assert(type.IsAssignableFrom(handle.Target.GetType()));

                return handle.Target;
            }

            switch (value->Kind)
            {
                case Kind.INVALID:
                    return null;

                case Kind.DEPENDENCYPROPERTY:
                    return DependencyProperty.Lookup(value->u.p);

                case Kind.BOOL:
                    return value->u.i32 != 0;

                case Kind.DOUBLE:
                    return value->u.d;

                case Kind.FLOAT:
                    return value->u.f;

                case Kind.UINT64:
                    return value->u.ui64;

                case Kind.INT64:
                    return value->u.i64;

                case Kind.TIMESPAN:
                    return new TimeSpan(value->u.i64);

                case Kind.CURSORTYPE:
                    return Cursors.FromEnum((CursorType)value->u.i32);

                case Kind.TEXTDECORATIONS:
                    return (value->u.i32 == (int)TextDecorationKind.Underline) ? TextDecorations.Underline : null;

                case Kind.INT32:
                    return value->u.i32;

                case Kind.UINT32:
                    return value->u.ui32;

                case Kind.CHAR:
                    return (char)value->u.ui32;

                case Kind.SURFACE:
                    return NativeDependencyObjectHelper.FromIntPtr(value->u.p);

                case Kind.STRING:
                    {
                        return MarshalEx.PtrToStringUtf8(value->u.p);
                    }

                case Kind.URI:
                    return UriHelper.FromNativeUri(value->u.p);

                case Kind.XMLLANGUAGE:
                    {
                        string str = MarshalEx.PtrToStringUtf8(value->u.p);
                        return XmlLanguage.GetLanguage(str);
                    }

                case Kind.FONTFAMILY:
                    {
                        UnmanagedFontFamily* family = (UnmanagedFontFamily*)value->u.p;
                        return new FontFamily(family == null ? null : MarshalEx.PtrToStringUtf8(family->source));
                    }

                case Kind.FONTSTRETCH:
                    {
                        UnmanagedFontStretch* stretch = (UnmanagedFontStretch*)value->u.p;
                        return new FontStretch(stretch == null ? FontStretchKind.Normal : stretch->stretch);
                    }

                case Kind.FONTSTYLE:
                    {
                        UnmanagedFontStyle* style = (UnmanagedFontStyle*)value->u.p;
                        return new FontStyle(style == null ? FontStyleKind.Normal : style->style);
                    }

                case Kind.FONTWEIGHT:
                    {
                        UnmanagedFontWeight* weight = (UnmanagedFontWeight*)value->u.p;
                        return new FontWeight(weight == null ? FontWeightKind.Normal : weight->weight);
                    }

                case Kind.FONTSOURCE:
                    {
                        UnmanagedFontSource* fs = (UnmanagedFontSource*)value->u.p;
                        ManagedStreamCallbacks callbacks;
                        GlyphTypeface typeface;
                        StreamWrapper wrapper;

                        switch (fs->type)
                        {
                            case FontSourceType.ManagedStream:
                                callbacks = (ManagedStreamCallbacks)Marshal.PtrToStructure(fs->source.stream, typeof(ManagedStreamCallbacks));

                                wrapper = (StreamWrapper)GCHandle.FromIntPtr(callbacks.handle).Target;

                                return new FontSource(wrapper.stream);
                            case FontSourceType.GlyphTypeface:
                                typeface = new GlyphTypeface(fs->source.typeface);
                                return new FontSource(typeface);
                            default:
                                throw new Exception(String.Format("Do not know how to create a FontSource of type {0}",
                                                    fs->type.ToString()));
                        }
                    }

                case Kind.GLYPHTYPEFACE:
                    {
                        return new GlyphTypeface(value->u.p);
                    }

                case Kind.PROPERTYPATH:
                    {
                        UnmanagedPropertyPath* propertypath = (UnmanagedPropertyPath*)value->u.p;
                        if (propertypath == null)
                            return new PropertyPath(null);
                        if (propertypath->property != IntPtr.Zero)
                            return null;
                        return new PropertyPath(MarshalEx.PtrToStringUtf8(propertypath->pathString), MarshalEx.PtrToStringUtf8(propertypath->expandedPathString));
                    }

                case Kind.POINT:
                    {
                        Point* point = (Point*)value->u.p;
                        return (point == null) ? new Point(0, 0) : *point;
                    }

                case Kind.RECT:
                    {
                        Rect* rect = (Rect*)value->u.p;
                        return (rect == null) ? new Rect(0, 0, 0, 0) : *rect;
                    }

                case Kind.SIZE:
                    {
                        Size* size = (Size*)value->u.p;
                        return (size == null) ? new Size(0, 0) : *size;
                    }

                case Kind.CORNERRADIUS:
                    {
                        CornerRadius* corner = (CornerRadius*)value->u.p;
                        return (corner == null) ? new CornerRadius(0) : *corner;
                    }

                case Kind.AUDIOFORMAT:
                    {
                        UnmanagedAudioFormat* format = (UnmanagedAudioFormat*)value->u.p;
                        return (format == null) ? new AudioFormat() : format->ToAudioFormat();
                    }

                case Kind.VIDEOFORMAT:
                    {
                        UnmanagedVideoFormat* format = (UnmanagedVideoFormat*)value->u.p;
                        return (format == null) ? new VideoFormat() : format->ToVideoFormat();
                    }

                case Kind.THICKNESS:
                    {
                        Thickness* thickness = (Thickness*)value->u.p;
                        return (thickness == null) ? new Thickness(0) : *thickness;
                    }

                case Kind.COLOR:
                    {
                        UnmanagedColor* color = (UnmanagedColor*)value->u.p;
                        if (color == null)
                            return new Color();
                        return color->ToColor();
                    }

                case Kind.MATRIX:
                case Kind.UNMANAGEDMATRIX:
                    {
                        return new Matrix(value->u.p);
                    }

                case Kind.MATRIX3D:
                case Kind.UNMANAGEDMATRIX3D:
                    {
                        return new Matrix3D(value->u.p);
                    }

                case Kind.STYLUSPOINT:
                case Kind.UNMANAGEDSTYLUSPOINT:
                    {
                        var kind = value->Kind;
                        var ptr = value->u.p;
                        var x = (double)ToObject(typeof(double), NativeMethods.dependency_object_get_value(ptr, UnmanagedStylusPoint.XProperty.Native));
                        var y = (double)ToObject(typeof(double), NativeMethods.dependency_object_get_value(ptr, UnmanagedStylusPoint.YProperty.Native));
                        var pressure = (double)ToObject(typeof(double), NativeMethods.dependency_object_get_value(ptr, UnmanagedStylusPoint.PressureFactorProperty.Native));
                        return new StylusPoint { X = x, Y = y, PressureFactor = (float)pressure };
                    }

                case Kind.DURATION:
                    {
                        Duration* duration = (Duration*)value->u.p;
                        return (duration == null) ? Duration.Automatic : *duration;
                    }

                case Kind.KEYTIME:
                    {
                        KeyTime* keytime = (KeyTime*)value->u.p;
                        return (keytime == null) ? KeyTime.FromTimeSpan(TimeSpan.Zero) : *keytime;
                    }

                case Kind.GRIDLENGTH:
                    {
                        GridLength* gridlength = (GridLength*)value->u.p;
                        return (gridlength == null) ? new GridLength() : *gridlength;
                    }

                case Kind.REPEATBEHAVIOR:
                    {
                        RepeatBehavior* repeat = (RepeatBehavior*)value->u.p;
                        return (repeat == null) ? new RepeatBehavior() : *repeat;
                    }

                case Kind.MEDIAATTRIBUTE_COLLECTION:
                    {
                        IntPtr p = value->u.p;
                        if (p == IntPtr.Zero)
                            return null;

                        int count = NativeMethods.collection_get_count(p);
                        var dict = new Dictionary<string, string>();
                        for (int i = 0; i < count; i++)
                        {
                            IntPtr map = NativeMethods.collection_get_value_at(p, i);
                            if (map == IntPtr.Zero)
                                continue;
                            Value* attribute = (Value*)map;
                            if (attribute->Kind != Kind.MEDIAATTRIBUTE || attribute->u.p == IntPtr.Zero)
                                continue;
                            string name = NativeMethods.media_attribute_get_name(attribute->u.p);
                            string val = NativeMethods.media_attribute_get_value(attribute->u.p);

                            dict.Add(name, val);
                        }
                        return dict;
                    }

                case Kind.MANAGEDTYPEINFO:
                    {
                        ManagedTypeInfo* type_info = (ManagedTypeInfo*)value->u.p;
                        if (type_info == null)
                            return null;
                        return Deployment.Current.Types.KindToType(type_info->Kind);
                    }
                default:
                    Type tt = Deployment.Current.Types.KindToType(value->Kind);
                    if (tt != null && tt.IsEnum)
                        return Enum.ToObject(tt, value->u.i32);
                    break;
            }

            if (NativeMethods.type_is_event_object(value->Kind))
            {
                if (value->u.p == IntPtr.Zero)
                    return null;

                return NativeDependencyObjectHelper.Lookup(value->u.p);
            }

            throw new Exception(String.Format("Do not know how to convert {0}  {1}. Managed type: {2}",
                                                value->Kind, (int)value->Kind, Deployment.Current.Types.KindToType(value->Kind)));
        }

        public static unsafe object ToObject(IntPtr value)
        {
            return ToObject(null, value);
        }

        public static unsafe object ToObject(Type type, IntPtr value)
        {
            return ToObject(type, (Value*)value);
        }

        // The caller is responsible for calling value_free_value on the returned Value
        public static Value FromObject(object v)
        {
            return FromObject(v, false);
        }

        //
        // How do we support "null" values, should the caller take care of that?
        //
        // The caller is responsible for calling value_free_value on the returned Value
        public static Value FromObject(object v, bool box_value_types)
        {
            Value value = new Value();

            unsafe
            {
                if (box_value_types && (v is ValueType || v is string))
                {
                    value.boxed_valuetype = GCHandle.Alloc(v);
                }

                if (v is IEasingFunction && v is not EasingFunctionBase)
                    v = new EasingFunctionWrapper(v as IEasingFunction);

                if (v is INativeEventObjectWrapper dov)
                {
                    if (dov.NativeHandle == IntPtr.Zero)
                        throw new Exception(String.Format(
                            "Object {0} has not set its native property", dov.GetType()));

                    NativeMethods.event_object_ref(dov.NativeHandle);

                    value.Kind = dov.GetKind();
                    value.u.p = dov.NativeHandle;

                }
                else if (v is DependencyProperty)
                {
                    value.Kind = Kind.DEPENDENCYPROPERTY;
                    value.u.p = ((DependencyProperty)v).Native;
                }
                else if (v is int || (v.GetType().IsEnum && Enum.GetUnderlyingType(v.GetType()) == typeof(int)))
                {
                    value.Kind = Deployment.Current.Types.TypeToKind(v.GetType());
                    value.u.i32 = (int)v;
                }
                else if (v is byte || (v.GetType().IsEnum && Enum.GetUnderlyingType(v.GetType()) == typeof(byte)))
                {
                    value.Kind = Deployment.Current.Types.TypeToKind(v.GetType());
                    value.u.i32 = (byte)v;
                }
                else if (v is bool)
                {
                    value.Kind = Kind.BOOL;
                    value.u.i32 = ((bool)v) ? 1 : 0;
                }
                else if (v is double)
                {
                    value.Kind = Kind.DOUBLE;
                    value.u.d = (double)v;
                }
                else if (v is float)
                {
                    value.Kind = Kind.FLOAT;
                    value.u.f = (float)v;
                }
                else if (v is long)
                {
                    value.Kind = Kind.INT64;
                    value.u.i64 = (long)v;
                }
                else if (v is TimeSpan ts)
                {
                    value.Kind = Kind.TIMESPAN;
                    value.u.i64 = ts.Ticks;
                }
                else if (v is ulong)
                {
                    value.Kind = Kind.UINT64;
                    value.u.ui64 = (ulong)v;
                }
                else if (v is uint)
                {
                    value.Kind = Kind.UINT32;
                    value.u.ui32 = (uint)v;
                }
                else if (v is char)
                {
                    value.Kind = Kind.CHAR;
                    value.u.ui32 = (uint)(char)v;
                }
                else if (v is string)
                {
                    value.Kind = Kind.STRING;

                    value.u.p = StringToIntPtr((string)v);
                }
                else if (v is Rect rect)
                {
                    value.Kind = Kind.RECT;
                    value.u.p = Allocate(sizeof(Rect));
                    Marshal.StructureToPtr(rect, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is Size size)
                {
                    value.Kind = Kind.SIZE;
                    value.u.p = Allocate(sizeof(Size));
                    Marshal.StructureToPtr(size, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is CornerRadius corner)
                {
                    value.Kind = Kind.CORNERRADIUS;
                    value.u.p = Allocate(sizeof(CornerRadius));
                    Marshal.StructureToPtr(corner, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is AudioFormat f)
                {
                    value.Kind = Kind.AUDIOFORMAT;
                    value.u.p = Allocate(sizeof(UnmanagedAudioFormat));
                    UnmanagedAudioFormat* format = (UnmanagedAudioFormat*)value.u.p;
                    format->bitsPerSample = f.BitsPerSample;
                    format->channels = f.Channels;
                    format->samplesPerSecond = f.SamplesPerSecond;
                    format->waveFormat = f.WaveFormat;
                }
                else if (v is VideoFormat frmt)
                {
                    value.Kind = Kind.VIDEOFORMAT;
                    value.u.p = Allocate(sizeof(UnmanagedVideoFormat));
                    UnmanagedVideoFormat* format = (UnmanagedVideoFormat*)value.u.p;
                    format->framesPerSecond = frmt.FramesPerSecond;
                    format->height = frmt.PixelHeight;
                    format->width = frmt.PixelWidth;
                    format->stride = frmt.Stride;
                    format->pixelFormat = frmt.PixelFormat;
                }
                else if (v is Point pnt)
                {
                    value.Kind = Kind.POINT;
                    value.u.p = Allocate(sizeof(Point));
                    Marshal.StructureToPtr(pnt, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is Thickness thickness)
                {
                    value.Kind = Kind.THICKNESS;
                    value.u.p = Allocate(sizeof(Thickness));
                    Marshal.StructureToPtr(thickness, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is Color c)
                {
                    value.Kind = Kind.COLOR;
                    value.u.p = Allocate(sizeof(UnmanagedColor));
                    UnmanagedColor* color = (UnmanagedColor*)value.u.p;
                    color->r = c.R / 255.0f;
                    color->g = c.G / 255.0f;
                    color->b = c.B / 255.0f;
                    color->a = c.A / 255.0f;
                }
                else if (v is Matrix matrix)
                {
                    // hack around the fact that managed Matrix is a struct while unmanaged Matrix is a DO
                    // i.e. the unmanaged and managed structure layouts ARE NOT equal
                    return FromObject(new UnmanagedMatrix(matrix), box_value_types);
                }
                else if (v is StylusPoint point)
                {
                    return FromObject(new UnmanagedStylusPoint(point), box_value_types);
                }
                else if (v is Matrix3D d1)
                {
                    // hack around the fact that managed Matrix3D is a struct while unmanaged Matrix3D is a DO
                    // i.e. the unmanaged and managed structure layouts ARE NOT equal
                    return FromObject(new UnmanagedMatrix3D(d1), box_value_types);
                }
                else if (v is Duration d)
                {
                    value.Kind = Kind.DURATION;
                    value.u.p = Allocate(sizeof(Duration));
                    Marshal.StructureToPtr(d, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is KeyTime k)
                {
                    value.Kind = Kind.KEYTIME;
                    value.u.p = Allocate(sizeof(KeyTime));
                    Marshal.StructureToPtr(k, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is RepeatBehavior behavior)
                {
                    value.Kind = Kind.REPEATBEHAVIOR;
                    value.u.p = Allocate(sizeof(RepeatBehavior));
                    Marshal.StructureToPtr(behavior, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is FontFamily family)
                {
                    value.Kind = Kind.FONTFAMILY;
                    value.u.p = Allocate(sizeof(UnmanagedFontFamily));
                    Marshal.StructureToPtr(family, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }

                else if (v is FontSource source)
                {
                    value.Kind = Kind.FONTSOURCE;

                    if (source.wrapper != null || source.typeface != null)
                    {
                        value.u.p = Allocate(sizeof(UnmanagedFontSource));
                        UnmanagedFontSource* ufs = (UnmanagedFontSource*)value.u.p;
                        ufs->type = source.type;

                        switch (source.type)
                        {
                            case FontSourceType.ManagedStream:
                                ManagedStreamCallbacks callbacks = source.wrapper.GetCallbacks();
                                ufs->source.stream = Allocate(sizeof(UnmanagedStreamCallbacks));
                                Marshal.StructureToPtr(callbacks, ufs->source.stream, false);
                                break;
                            case FontSourceType.GlyphTypeface:
                                ufs->source.typeface = source.typeface.Native;
                                break;
                        }
                    }
                    else
                    {
                        value.IsNull = true;
                    }
                }

                else if (v is PropertyPath propertypath)
                {
                    value.Kind = Kind.PROPERTYPATH;
                    value.u.p = Allocate(sizeof(UnmanagedPropertyPath));

                    UnmanagedPropertyPath* upp = (UnmanagedPropertyPath*)value.u.p;
                    upp->property = propertypath.NativeDP;
                    if (upp->property == IntPtr.Zero)
                    {
                        upp->pathString = StringToIntPtr(propertypath.Path);
                        upp->expandedPathString = StringToIntPtr(propertypath.ExpandedPath);
                    }
                    else
                    {
                        upp->pathString = IntPtr.Zero;
                        upp->expandedPathString = IntPtr.Zero;
                    }
                }
                else if (v is Uri uri)
                {
                    value.Kind = Kind.URI;
                    value.u.p = UriHelper.ToNativeUri(uri);
                }
                else if (v is XmlLanguage lang)
                {
                    value.Kind = Kind.XMLLANGUAGE;
                    value.u.p = StringToIntPtr(lang.IetfLanguageTag);
                }
                else if (v is Cursor cur)
                {
                    value.Kind = Kind.CURSORTYPE;
                    value.u.i32 = (int)cur.cursor;
                }
                else if (v is GridLength gl)
                {
                    value.Kind = Kind.GRIDLENGTH;
                    value.u.p = Allocate(sizeof(GridLength));
                    Marshal.StructureToPtr(gl, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is FontStretch stretch)
                {
                    value.Kind = Kind.FONTSTRETCH;
                    value.u.p = Allocate(sizeof(UnmanagedFontStretch));
                    Marshal.StructureToPtr(stretch, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is FontStyle style)
                {
                    value.Kind = Kind.FONTSTYLE;
                    value.u.p = Allocate(sizeof(UnmanagedFontStyle));
                    Marshal.StructureToPtr(style, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is FontWeight weight)
                {
                    value.Kind = Kind.FONTWEIGHT;
                    value.u.p = Allocate(sizeof(UnmanagedFontWeight));
                    Marshal.StructureToPtr(weight, value.u.p, false); // Unmanaged and managed structure layout is equal.
                }
                else if (v is TextDecorationCollection)
                {
                    value.Kind = Kind.TEXTDECORATIONS;
                    value.u.i32 = (int)(v as TextDecorationCollection).Kind;
                }
                else if (v is Type)
                {
                    Type t = v as Type;
                    value.Kind = Kind.MANAGEDTYPEINFO;
                    value.u.p = NativeMethods.managed_type_info_new(Deployment.Current.Types.TypeToKind(t));
                }
                else if (v is Value)
                {
                    throw new InvalidOperationException("You can not create a Mono.Value from a Mono.Value.");
                }
                else
                {
                    if (v is Type)
                    {
                        Debugger.Break();
                    }

                    //Console.WriteLine ("Do not know how to encode {0} yet, boxing it", v.GetType ());

                    // TODO: We probably need to marshal types that can animate as the 
                    // corresponding type (Point, Double, Color, etc).
                    // TODO: We need to store the GCHandle somewhere so that we can free it,
                    // or register a callback on the surface for the unmanaged code to call.
                    GCHandle handle = GCHandle.Alloc(v);
                    value.IsGCHandle = true;
                    value.Kind = Deployment.Current.Types.TypeToKind(v.GetType());
                    value.u.p = GCHandle.ToIntPtr(handle);
                }
            }
            return value;
        }

        public static unsafe IntPtr StringToIntPtr(string str)
        {
            if (str == null)
                return IntPtr.Zero;

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            IntPtr result = Allocate(bytes.Length + 1);
            Marshal.Copy(bytes, 0, result, bytes.Length);
            Marshal.WriteByte(result, bytes.Length, 0);

            return result;
        }

        public static unsafe IntPtr Allocate(int bytes)
        {
#if NET6_0_OR_GREATER
            return (IntPtr)NativeMemory.Alloc((nuint)bytes);
#else
            return Marshal.AllocHGlobal(bytes);
#endif
        }

        public void Dispose()
        {
            if (u.p != IntPtr.Zero)
            {
                NativeMethods.value_free_value(ref this);
                u.p = IntPtr.Zero;
            }
        }
    }
}
