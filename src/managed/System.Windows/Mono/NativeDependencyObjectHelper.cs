
//
// INativeDependencyObjectWrapper.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2008 Novell, Inc.
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
using Microsoft.Internal;
using MS.Internal;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Messaging;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Threading;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mono
{
    internal static class NativeDependencyObjectHelper
    {

        public static ManagedRefCallback add_strong_ref = new ManagedRefCallback(AddStrongRef);
        public static ManagedRefCallback clear_strong_ref = new ManagedRefCallback(ClearStrongRef);
        public static MentorChangedCallback mentor_changed = new MentorChangedCallback(MentorChanged);
        static UnmanagedEventHandlerInvoker native_collection_changed_callback = native_collection_changed;

#if DEBUG_REF
		internal static string IdToName (IntPtr id)
		{
			var dp = DependencyProperty.Lookup (id);
			if (dp != null)
				return dp.Name;

			if (Enum.IsDefined (typeof (WeakRefs), id.ToInt32 ()))
				return Enum.GetName (typeof (WeakRefs), id.ToInt32 ());

			return "???";
		}
#endif

        static void AddStrongRef(IntPtr referer, IntPtr referent, IntPtr id)
        {
            try
            {
                var handle = GCHandle.FromIntPtr(referent);
                var container = Lookup(referer) as IRefContainer;
                if (container == null)
                    return;
                container.AddStrongRef(id, handle.Target);
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine("Moonlight: Unhandled exception in NativeDependencyObjectHelper.AddStrongRef: {0}", ex);
                }
                catch
                {
                }
            }
        }

        static void ClearStrongRef(IntPtr referer, IntPtr referent, IntPtr id)
        {
            try
            {
                IRefContainer container = Lookup(referer) as IRefContainer;
                if (container == null)
                    return;

                container.ClearStrongRef(id, GCHandle.FromIntPtr(referent).Target);
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine("Moonlight: Unhandled exception in NativeDependencyObjectHelper.ClearStrongRef: {0}", ex);
                }
                catch
                {
                }
            }
        }

        static void MentorChanged(IntPtr ptr, IntPtr mentor_ptr)
        {
            try
            {
                INativeEventObjectWrapper wrapper = Lookup(ptr);
                if (wrapper == null)
                    return;

                wrapper.MentorChanged(mentor_ptr);
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine("Moonlight: Unhandled exception in NativeDependencyObjectHelper.MentorChanged: {0}", ex);
                }
                catch
                {
                }
            }
        }

        public static void SetManagedPeerCallbacks(INativeEventObjectWrapper obj)
        {
            IRefContainer container = obj as IRefContainer;
            NativeMethods.event_object_set_managed_peer_callbacks(obj.NativeHandle,
                                           container == null ? null : add_strong_ref,
                                           container == null ? null : clear_strong_ref,
                                           mentor_changed);

            if (obj is INativeCollectionWrapper)
                Events.AddHandler(obj, EventIds.Collection_ChangedEvent, native_collection_changed_callback);
        }

        static void native_collection_changed(IntPtr sender, int event_id, int token, IntPtr calldata, IntPtr closure)
        {
            try
            {
                var collection = (INativeCollectionWrapper)Lookup(sender);

                if (collection != null)
                {
                    var managedList = collection.ManagedList;
                    var args = (InternalCollectionChangedEventArgs)Lookup(calldata);

                    switch (args.ChangedAction)
                    {
                        case CollectionChangedAction.Add:
#if DEBUG_REF
						Console.WriteLine ("collection {0}/{1} adding ref to {2}/{3}", collection.GetHashCode(), collection, ((T)args.GetNewItem(typeof (T))).GetHashCode(), ((T)args.GetNewItem(typeof (T))));
#endif
                            managedList.Insert(args.Index, args.GetNewItem(null));
                            break;
                        case CollectionChangedAction.Remove:
#if DEBUG_REF
						Console.WriteLine ("collection {0}/{1} removing ref to {2}/{3}", collection.GetHashCode(), collection, managedList[args.Index].GetHashCode(), managedList[args.Index]);
#endif
                            managedList.RemoveAt(args.Index);
                            break;
                        case CollectionChangedAction.Replace:
#if DEBUG_REF
						Console.WriteLine ("collection {0}/{1} replacing ref from {2}/{3} to {4}/{5}", collection.GetHashCode(), collection, managedList[args.Index].GetHashCode(), managedList[args.Index], ((T)args.GetNewItem(typeof (T))), ((T)args.GetNewItem(typeof (T))).GetHashCode());
#endif
                            managedList[args.Index] = args.GetNewItem(null);
                            break;
                        case CollectionChangedAction.Clearing:
                            // nothing to do
                            break;
                        case CollectionChangedAction.Cleared:
#if DEBUG_REF
						foreach (var o in managedList)
							Console.WriteLine (" collection {0}/{1} removing ref to {2}/{3}", collection.GetHashCode(), collection, o.GetHashCode(), o);
#endif
                            managedList.Clear();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine("Unhandled Exception in NativeDependencyObjectHelper.native_collection_changed: {0}", ex);
                }
                catch
                {

                }
            }
        }

        #region "helpers for the INativeDependencyObjectWrapper interface"
        public static object GetValue(INativeDependencyObjectWrapper wrapper, DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException("property");

            CheckNativeAndThread(wrapper);

            IntPtr val = NativeMethods.dependency_object_get_value(wrapper.NativeHandle, dp.Native);
            return Value.ToObject(dp.PropertyType, val);
        }

        public static void SetValue(INativeDependencyObjectWrapper wrapper, DependencyProperty dp, object value)
        {
            Type object_type;

            if (dp == null)
                throw new ArgumentNullException("property");

            CheckNativeAndThread(wrapper);

            if (dp.DeclaringType != null && !dp.IsAttached)
            {
                if (!dp.DeclaringType.IsAssignableFrom(wrapper.GetType()))
                    throw new System.ArgumentException(string.Format("The DependencyProperty '{2}', registered on type {0} can't be used to set a value on an object of type {1}", dp.DeclaringType.AssemblyQualifiedName, wrapper.GetType().AssemblyQualifiedName, dp.Name));
            }

            if (value == null)
            {
                using (var val = new Value { Kind = NativeMethods.dependency_property_get_property_type(dp.Native), IsNull = true })
                {
                    var v = val;
                    NativeMethods.dependency_object_set_value(wrapper.NativeHandle, dp.Native, ref v);
                }
                return;
            }

            if (value == DependencyProperty.UnsetValue)
            {
                // Setting to UnsetValue appears to be equivalent to ClearValue
                ClearValue(wrapper, dp);
                return;
            }

            // XXX we need this to work with all
            // INativeDependencyObjectWrapper, I expect..
            // but right now the only one other than DO is
            // Application, which doesn't have any
            // settable properties, so we're safe for now.
            if (wrapper is DependencyObject)
                dp.Validate((DependencyObject)wrapper, dp, value);

            object_type = value.GetType();

            bool type_valid = false;
            if (dp.PropertyType.IsGenericTypeDefinition)
            {
                Type t = object_type;
                while (t != null)
                {
                    if (dp.PropertyType.IsAssignableFrom(t))
                    {
                        type_valid = true;
                        break;
                    }

                    if (t.IsGenericType && !t.IsGenericTypeDefinition)
                        t = t.GetGenericTypeDefinition();
                    else
                        t = t.BaseType;
                }
            }
            else
            {
                type_valid = dp.PropertyType.IsAssignableFrom(object_type);
            }

            if (!type_valid)
            {
                throw new ArgumentException(string.Format("The DependencyProperty '{3}.{2}', whose property type is {0} can't be set to value whose type is {1}", dp.PropertyType.FullName, object_type.FullName, dp.Name, dp.DeclaringType.FullName));
            }

            using (var val = Value.FromObject(value, dp is CustomDependencyProperty))
            {
                var v = val;
                NativeMethods.dependency_object_set_value(wrapper.NativeHandle, dp.Native, ref v);
            }
        }

        public static object ReadLocalValue(INativeDependencyObjectWrapper wrapper, DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException("dp");

            IntPtr val = NativeMethods.dependency_object_read_local_value(wrapper.NativeHandle, dp.Native);
            if (val == IntPtr.Zero)
            {
                return DependencyProperty.UnsetValue;
            }
            else
            {
                // We can get a style or bindingexpression or something else here
                // so the Value* will not always be of type 'DP.PropertyType'.
                return Value.ToObject(dp.PropertyType, val);
            }
        }

        public static object GetAnimationBaseValue(INativeDependencyObjectWrapper wrapper, DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException("dp");
            return Value.ToObject(null, NativeMethods.dependency_object_get_animation_base_value(wrapper.NativeHandle, dp.Native));
        }

        public static void ClearValue(INativeDependencyObjectWrapper wrapper, DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException("dp");
            NativeMethods.dependency_object_clear_value(wrapper.NativeHandle, dp.Native, true);
        }
        #endregion

        /* thread-safe */
        public static bool AddNativeMapping(IntPtr native, INativeEventObjectWrapper wrapper)
        {
            if (native == IntPtr.Zero)
                return false;

            NativeMethods.event_object_set_managed_handle(native, GCHandle.ToIntPtr(GCHandle.Alloc(wrapper, GCHandleType.Normal)));

#if DEBUG_REF
			Console.WriteLine ("adding native mapping from {0:x} to {1}/{2}", (int)native, wrapper.GetHashCode(), wrapper.GetType());
#endif
            return true;
        }

        /* thread-safe */
        public static void FreeNativeMapping(INativeEventObjectWrapper wrapper)
        {
            FreeNativeMapping(wrapper.NativeHandle);
        }
        public static void FreeNativeMapping(IntPtr native)
        {
            GCHandle handle;

            if (native == IntPtr.Zero)
                return;

            NativeMethods.event_object_set_managed_handle(native, IntPtr.Zero);
        }

        internal static INativeEventObjectWrapper FromIntPtr(IntPtr data)
        {
            return Lookup(data);
        }

        //
        // This version only looks up the object, if it has not been exposed,
        // we return null
        // Thread-safe
        internal static INativeEventObjectWrapper Lookup(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;
            return (INativeEventObjectWrapper)NativeMethods.event_object_get_managed_object(ptr);
        }

        internal static object CreateObject(Kind k, IntPtr raw)
        {
            switch (k)
            {
                case Kind.ARCSEGMENT: return new ArcSegment(raw, false);
                case Kind.APPLICATION: return new Application(raw, false);
                case Kind.ASSEMBLYPART: return new AssemblyPart(raw, false);
                case Kind.ASSEMBLYPART_COLLECTION: return new AssemblyPartCollection(raw, false);
                case Kind.AUDIOCAPTUREDEVICE: return new AudioCaptureDevice(raw, false);
                case Kind.AUDIOCAPTUREDEVICE_COLLECTION: return new AudioCaptureDeviceCollection(raw, false);
                case Kind.AUDIOFORMAT_COLLECTION: return new AudioFormatCollection(raw, false);
                case Kind.BACKEASE: return new BackEase(raw, false);
                case Kind.BEGINSTORYBOARD: return new BeginStoryboard(raw, false);
                case Kind.BEZIERSEGMENT: return new BezierSegment(raw, false);
                //case Kind.BINDINGEXPRESSION: return new BindingExpression (raw, false);
                case Kind.BITMAPCACHE: return new BitmapCache(raw, false);
                case Kind.BITMAPIMAGE: return new BitmapImage(raw, false);
                case Kind.BLOCK_COLLECTION: return new BlockCollection(raw, false);
                case Kind.BLUREFFECT: return new BlurEffect(raw, false);
                case Kind.BOLD: return new Bold(raw, false);
                case Kind.BOUNCEEASE: return new BounceEase(raw, false);
                case Kind.BORDER: return new Border(raw, false);
                case Kind.CANVAS: return new Canvas(raw, false);
                case Kind.CAPTUREIMAGECOMPLETEDEVENTARGS: return new CaptureImageCompletedEventArgs(raw, CaptureImageCompletedEventArgs.GetExceptionFromUnmanaged(raw), false);
                case Kind.CIRCLEEASE: return new CircleEase(raw, false);
                case Kind.COLLECTIONCHANGEDEVENTARGS: return new InternalCollectionChangedEventArgs(raw, false);
                case Kind.COLORANIMATION: return new ColorAnimation(raw, false);
                case Kind.COLORANIMATIONUSINGKEYFRAMES: return new ColorAnimationUsingKeyFrames(raw, false);
                case Kind.COLORKEYFRAME_COLLECTION: return new ColorKeyFrameCollection(raw, false);
                case Kind.COLUMNDEFINITION: return new ColumnDefinition(raw, false);
                case Kind.COLUMNDEFINITION_COLLECTION: return new ColumnDefinitionCollection(raw, false);
                case Kind.CONTENTCONTROL: return new ContentControl(raw, false);
                case Kind.CONTENTPRESENTER: return new ContentPresenter(raw, false);
                case Kind.CONTROLTEMPLATE: return new ControlTemplate(raw, false);
                case Kind.COMPOSITETRANSFORM: return new CompositeTransform(raw, false);
                case Kind.CUBICEASE: return new CubicEase(raw, false);
                case Kind.DATATEMPLATE: return new DataTemplate(raw, false);
                case Kind.DEEPZOOMIMAGETILESOURCE: return new DeepZoomImageTileSource(raw, false);
                case Kind.DEPLOYMENT: return new Deployment(raw, false);
                case Kind.DISCRETECOLORKEYFRAME: return new DiscreteColorKeyFrame(raw, false);
                case Kind.DISCRETEDOUBLEKEYFRAME: return new DiscreteDoubleKeyFrame(raw, false);
                case Kind.DISCRETEPOINTKEYFRAME: return new DiscretePointKeyFrame(raw, false);
                case Kind.DISCRETEOBJECTKEYFRAME: return new DiscreteObjectKeyFrame(raw, false);
                case Kind.DOUBLEANIMATION: return new DoubleAnimation(raw, false);
                case Kind.DOUBLEANIMATIONUSINGKEYFRAMES: return new DoubleAnimationUsingKeyFrames(raw, false);
                case Kind.DOUBLEKEYFRAME_COLLECTION: return new DoubleKeyFrameCollection(raw, false);
                case Kind.DOUBLE_COLLECTION: return new DoubleCollection(raw, false);
                case Kind.DRAWINGATTRIBUTES: return new DrawingAttributes(raw, false);
                case Kind.DROPSHADOWEFFECT: return new DropShadowEffect(raw, false);
                case Kind.EASINGCOLORKEYFRAME: return new EasingColorKeyFrame(raw, false);
                case Kind.EASINGDOUBLEKEYFRAME: return new EasingDoubleKeyFrame(raw, false);
                case Kind.EASINGPOINTKEYFRAME: return new EasingPointKeyFrame(raw, false);
                case Kind.ELASTICEASE: return new ElasticEase(raw, false);
                case Kind.ELLIPSEGEOMETRY: return new EllipseGeometry(raw, false);
                case Kind.ELLIPSE: return new Ellipse(raw, false);
                case Kind.EVENTTRIGGER: return new EventTrigger(raw, false);
                case Kind.EXPONENTIALEASE: return new ExponentialEase(raw, false);
                case Kind.EXTENSIONPART: return new ExtensionPart(raw, false);
                case Kind.EXTERNALPART_COLLECTION: return new ExternalPartCollection(raw, false);
                case Kind.GEOMETRY_COLLECTION: return new GeometryCollection(raw, false);
                case Kind.GEOMETRYGROUP: return new GeometryGroup(raw, false);
                case Kind.GLYPHS: return new Glyphs(raw, false);
                case Kind.GLYPHTYPEFACE_COLLECTION: return new GlyphTypefaceCollection(raw, false);
                case Kind.GRADIENTSTOP_COLLECTION: return new GradientStopCollection(raw, false);
                case Kind.GRADIENTSTOP: return new GradientStop(raw, false);
                case Kind.GRID: return new Grid(raw, false);
                case Kind.HYPERLINK: return new Hyperlink(raw, false);
                case Kind.ICON: return new Icon(raw, false);
                case Kind.ICON_COLLECTION: return new IconCollection(raw, false);
                case Kind.IMAGEBRUSH: return new ImageBrush(raw, false);
                case Kind.IMAGE: return new Image(raw, false);
                case Kind.INLINE_COLLECTION: return new InlineCollection(raw, false);
                case Kind.INLINEUICONTAINER: return new InlineUIContainer(raw, false);
                case Kind.INKPRESENTER: return new InkPresenter(raw, false);
                case Kind.INTERNALTRANSFORM: return new InternalTransform(raw, false);
                case Kind.INPUTMETHOD: return new InputMethod(raw, false);
                case Kind.ITALIC: return new Italic(raw, false);
                case Kind.KEYEVENTARGS: return new KeyEventArgs(raw);
                case Kind.KEYSPLINE: return new KeySpline(raw, false);
                case Kind.LINEARGRADIENTBRUSH: return new LinearGradientBrush(raw, false);
                case Kind.LINEBREAK: return new LineBreak(raw, false);
                case Kind.LINEGEOMETRY: return new LineGeometry(raw, false);
                case Kind.LINE: return new Line(raw, false);
                case Kind.LINEARCOLORKEYFRAME: return new LinearColorKeyFrame(raw, false);
                case Kind.LINEARDOUBLEKEYFRAME: return new LinearDoubleKeyFrame(raw, false);
                case Kind.LINEARPOINTKEYFRAME: return new LinearPointKeyFrame(raw, false);
                case Kind.LINESEGMENT: return new LineSegment(raw, false);
                case Kind.LOCALMESSAGERECEIVER: return new LocalMessageReceiver(raw, false);
                case Kind.LOCALMESSAGESENDER: return new LocalMessageSender(raw, false);
                case Kind.LOGREADYROUTEDEVENTARGS: return new LogReadyRoutedEventArgs(raw, false);
                case Kind.MATRIXTRANSFORM: return new MatrixTransform(raw, false);
                case Kind.MATRIX3DPROJECTION: return new Matrix3DProjection(raw, false);
                case Kind.NAMESCOPE: return new NameScope(raw, false);
                case Kind.TIMELINEMARKERROUTEDEVENTARGS: return new TimelineMarkerRoutedEventArgs(raw, false);
                case Kind.MEDIAELEMENT: return new MediaElement(raw, false);
                case Kind.MOUSEEVENTARGS: return new MouseEventArgs(raw);
                case Kind.MOUSEBUTTONEVENTARGS: return new MouseButtonEventArgs(raw);
                case Kind.MOUSEWHEELEVENTARGS: return new MouseWheelEventArgs(raw);
                case Kind.MULTISCALEIMAGE: return new MultiScaleImage(raw, false);
                case Kind.MULTISCALESUBIMAGE: return new MultiScaleSubImage(raw, false);
                case Kind.MULTISCALESUBIMAGE_COLLECTION: return new MultiScaleSubImageCollection(raw, false);
                case Kind.OBJECTANIMATIONUSINGKEYFRAMES: return new ObjectAnimationUsingKeyFrames(raw, false);
                case Kind.OBJECTKEYFRAME_COLLECTION: return new ObjectKeyFrameCollection(raw, false);
                case Kind.OUTOFBROWSERSETTINGS: return new OutOfBrowserSettings(raw, false);
                case Kind.PARAGRAPH: return new Paragraph(raw, false);
                case Kind.PASSWORDBOX: return new PasswordBox(raw, false);
                case Kind.PATHFIGURE_COLLECTION: return new PathFigureCollection(raw, false);
                case Kind.PATHFIGURE: return new PathFigure(raw, false);
                case Kind.PATHGEOMETRY: return new PathGeometry(raw, false);
                case Kind.PATH: return new Path(raw, false);
                case Kind.PATHSEGMENT_COLLECTION: return new PathSegmentCollection(raw, false);
                case Kind.PIXELSHADER: return new PixelShader(raw, false);
                case Kind.PLANEPROJECTION: return new PlaneProjection(raw, false);
                case Kind.POINTANIMATION: return new PointAnimation(raw, false);
                case Kind.POINTANIMATIONUSINGKEYFRAMES: return new PointAnimationUsingKeyFrames(raw, false);
                case Kind.POINTKEYFRAME_COLLECTION: return new PointKeyFrameCollection(raw, false);
                case Kind.POINT_COLLECTION: return new PointCollection(raw, false);
                case Kind.POLYBEZIERSEGMENT: return new PolyBezierSegment(raw, false);
                case Kind.POLYGON: return new Polygon(raw, false);
                case Kind.POLYLINE: return new Polyline(raw, false);
                case Kind.POLYLINESEGMENT: return new PolyLineSegment(raw, false);
                case Kind.POLYQUADRATICBEZIERSEGMENT: return new PolyQuadraticBezierSegment(raw, false);
                case Kind.POPUP: return new Popup(raw, false);
                case Kind.POWEREASE: return new PowerEase(raw, false);
                case Kind.QUADRATICBEZIERSEGMENT: return new QuadraticBezierSegment(raw, false);
                case Kind.QUADRATICEASE: return new QuadraticEase(raw, false);
                case Kind.QUARTICEASE: return new QuarticEase(raw, false);
                case Kind.QUINTICEASE: return new QuinticEase(raw, false);
                case Kind.RADIALGRADIENTBRUSH: return new RadialGradientBrush(raw, false);
                case Kind.RECTANGLEGEOMETRY: return new RectangleGeometry(raw, false);
                case Kind.RECTANGLE: return new Rectangle(raw, false);
                case Kind.RESOURCE_DICTIONARY: return new ResourceDictionary(raw, false);
                case Kind.RESOURCE_DICTIONARYCHANGEDEVENTARGS: return new InternalResourceDictionaryChangedEventArgs(raw, false);
                case Kind.RESOURCE_DICTIONARY_COLLECTION: return new ResourceDictionaryCollection(raw, false);
                case Kind.RICHTEXTBOX: return new RichTextBox(raw, false);
                case Kind.RICHTEXTBOXVIEW: return new RichTextBoxView(raw, false);
                case Kind.ROTATETRANSFORM: return new RotateTransform(raw, false);
                case Kind.ROUTEDEVENTARGS: return new RoutedEventArgs(raw, false);
                case Kind.ROWDEFINITION: return new RowDefinition(raw, false);
                case Kind.ROWDEFINITION_COLLECTION: return new RowDefinitionCollection(raw, false);
                case Kind.RUN: return new Run(raw, false);
                case Kind.SECTION: return new Section(raw, false);
                case Kind.SECURITYSETTINGS: return new SecuritySettings(raw, false);
                case Kind.SETTERBASE_COLLECTION: return new SetterBaseCollection(raw, false);
                case Kind.SETTER: return new Setter(raw, false);
                case Kind.SCALETRANSFORM: return new ScaleTransform(raw, false);
                case Kind.SINEEASE: return new SineEase(raw, false);
                case Kind.SIZECHANGEDEVENTARGS: return new SizeChangedEventArgs(raw);
                case Kind.SKEWTRANSFORM: return new SkewTransform(raw, false);
                case Kind.SOLIDCOLORBRUSH: return new SolidColorBrush(raw, false);
                case Kind.SPAN: return new Span(raw, false);
                case Kind.SPLINECOLORKEYFRAME: return new SplineColorKeyFrame(raw, false);
                case Kind.SPLINEDOUBLEKEYFRAME: return new SplineDoubleKeyFrame(raw, false);
                case Kind.SPLINEPOINTKEYFRAME: return new SplinePointKeyFrame(raw, false);
                case Kind.STORYBOARD: return new Storyboard(raw, false);
                case Kind.STROKE_COLLECTION: return new StrokeCollection(raw, false);
                case Kind.STROKE: return new Stroke(raw, false);
                case Kind.STYLE: return new Style(raw, false);
                case Kind.STYLUSINFO: return new StylusInfo(raw, false);
                case Kind.STYLUSPOINT_COLLECTION: return new StylusPointCollection(raw, false);
                case Kind.SURFACE: return new Surface(raw, false);
                case Kind.TEXTBLOCK: return new TextBlock(raw, false);
                case Kind.TEXTBOX: return new TextBox(raw, false);
                case Kind.TEXTBOXVIEW: return new TextBoxView(raw, false);
                case Kind.TEXTCHANGEDEVENTARGS: return new TextChangedEventArgs(raw);
                case Kind.TIMELINE_COLLECTION: return new TimelineCollection(raw, false);
                case Kind.TIMEMANAGER: return new TimeManager(raw, false);
                case Kind.TIMELINEMARKER: return new TimelineMarker(raw, false);
                case Kind.TIMELINEMARKER_COLLECTION: return new TimelineMarkerCollection(raw, false);
                case Kind.TRANSFORM_COLLECTION: return new TransformCollection(raw, false);
                case Kind.TRANSFORMGROUP: return new TransformGroup(raw, false);
                case Kind.TRANSLATETRANSFORM: return new TranslateTransform(raw, false);
                case Kind.TRIGGERACTION_COLLECTION: return new TriggerActionCollection(raw, false);
                case Kind.TRIGGER_COLLECTION: return new TriggerCollection(raw, false);
                case Kind.UIELEMENT_COLLECTION: return new UIElementCollection(raw, false);
                case Kind.UNDERLINE: return new Underline(raw, false);
                case Kind.USERCONTROL: return new UserControl(raw, false);
                case Kind.VIDEOBRUSH: return new VideoBrush(raw, false);
                case Kind.VIDEOCAPTUREDEVICE: return new VideoCaptureDevice(raw, false);
                case Kind.VIDEOCAPTUREDEVICE_COLLECTION: return new VideoCaptureDeviceCollection(raw, false);
                case Kind.VIDEOFORMAT_COLLECTION: return new VideoFormatCollection(raw, false);
                case Kind.WEBBROWSER: return new WebBrowser(raw, false);
                case Kind.WINDOW: return new Window(raw, false);
                case Kind.WINDOWSETTINGS: return new WindowSettings(raw, false);
                case Kind.WRITEABLEBITMAP: return new WriteableBitmap(raw, false);

                case Kind.BITMAPSOURCE:
                case Kind.CACHEMODE:
                case Kind.CLOCKGROUP:
                case Kind.ANIMATIONCLOCK:
                case Kind.CLOCK:
                case Kind.TRIGGERACTION:
                case Kind.KEYFRAME_COLLECTION:
                case Kind.UIELEMENT:
                case Kind.PANEL:
                case Kind.TIMELINE:
                case Kind.FRAMEWORKELEMENT:
                case Kind.BRUSH:
                case Kind.EFFECT:
                case Kind.SHADEREFFECT:
                case Kind.TILEBRUSH:
                case Kind.GENERALTRANSFORM:
                case Kind.TRANSFORM:
                case Kind.PROJECTION:
                case Kind.SHAPE:
                case Kind.GEOMETRY:
                case Kind.SETTERBASE:
                case Kind.FRAMEWORKTEMPLATE:
                case Kind.EASINGFUNCTIONBASE:
                    throw new Exception(
                        String.Format("Should never get an abstract class from unmanaged code {0}", k));

                default:
                    throw new Exception(
                                 String.Format("NativeDependencyObjectHelper::CreateObject(): Kind missing from switch: {0}/{1} for ptr 0x{2:x}", k, Deployment.Current.Types.KindToType(k), (int)raw));
            }
        }

        private static void CheckNativeAndThread(INativeDependencyObjectWrapper wrapper)
        {
            if (wrapper.NativeHandle == IntPtr.Zero)
            {
                throw new Exception(
                    string.Format("Uninitialized object: this object ({0}) has not set its native handle set", wrapper.GetType().FullName));
            }

            if (!wrapper.CheckAccess())
                throw new UnauthorizedAccessException("Invalid access of Moonlight from an external thread");
        }

    }

}
