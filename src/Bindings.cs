using System.Runtime.InteropServices;

namespace Moonlight
{
    public unsafe partial struct MoonUnmanagedFactory
    {

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAnimation@MoonUnmanagedFactory@Moonlight@@SAPEAVAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Animation *")]
        public static extern Animation* CreateAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAnimationClock@MoonUnmanagedFactory@Moonlight@@SAPEAVAnimationClock@2@PEAVAnimation@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AnimationClock *")]
        public static extern AnimationClock* CreateAnimationClock([NativeTypeName("Moonlight::Animation *")] Animation* timeline);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateApplication@MoonUnmanagedFactory@Moonlight@@SAPEAVApplication@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Application *")]
        public static extern Application* CreateApplication();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateArcSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVArcSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ArcSegment *")]
        public static extern ArcSegment* CreateArcSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAssemblyPart@MoonUnmanagedFactory@Moonlight@@SAPEAVAssemblyPart@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AssemblyPart *")]
        public static extern AssemblyPart* CreateAssemblyPart();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAssemblyPartCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVAssemblyPartCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AssemblyPartCollection *")]
        public static extern AssemblyPartCollection* CreateAssemblyPartCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioCaptureDevice@MoonUnmanagedFactory@Moonlight@@SAPEAVAudioCaptureDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioCaptureDevice *")]
        public static extern AudioCaptureDevice* CreateAudioCaptureDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioCaptureDeviceCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVAudioCaptureDeviceCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioCaptureDeviceCollection *")]
        public static extern AudioCaptureDeviceCollection* CreateAudioCaptureDeviceCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioFormatCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVAudioFormatCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioFormatCollection *")]
        public static extern AudioFormatCollection* CreateAudioFormatCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioStream@MoonUnmanagedFactory@Moonlight@@SAPEAVAudioStream@2@PEAVMedia@2@HHHHHHPEAXI@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioStream *")]
        public static extern AudioStream* CreateAudioStream([NativeTypeName("Moonlight::Media *")] Media* media, int codec_id, int bits_per_sample, int block_align, int sample_rate, int channels, int bit_rate, [NativeTypeName("gpointer")] void* extra_data, [NativeTypeName("guint32")] uint extra_data_size);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBackEase@MoonUnmanagedFactory@Moonlight@@SAPEAVBackEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BackEase *")]
        public static extern BackEase* CreateBackEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBeginStoryboard@MoonUnmanagedFactory@Moonlight@@SAPEAVBeginStoryboard@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BeginStoryboard *")]
        public static extern BeginStoryboard* CreateBeginStoryboard();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBezierSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BezierSegment *")]
        public static extern BezierSegment* CreateBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBitmapCache@MoonUnmanagedFactory@Moonlight@@SAPEAVBitmapCache@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BitmapCache *")]
        public static extern BitmapCache* CreateBitmapCache();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBitmapImage@MoonUnmanagedFactory@Moonlight@@SAPEAVBitmapImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BitmapImage *")]
        public static extern BitmapImage* CreateBitmapImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBitmapSource@MoonUnmanagedFactory@Moonlight@@SAPEAVBitmapSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BitmapSource *")]
        public static extern BitmapSource* CreateBitmapSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBlock@MoonUnmanagedFactory@Moonlight@@SAPEAVBlock@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Block *")]
        public static extern Block* CreateBlock();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBlockCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVBlockCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BlockCollection *")]
        public static extern BlockCollection* CreateBlockCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBlurEffect@MoonUnmanagedFactory@Moonlight@@SAPEAVBlurEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BlurEffect *")]
        public static extern BlurEffect* CreateBlurEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBold@MoonUnmanagedFactory@Moonlight@@SAPEAVBold@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Bold *")]
        public static extern Bold* CreateBold();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBorder@MoonUnmanagedFactory@Moonlight@@SAPEAVBorder@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Border *")]
        public static extern Border* CreateBorder();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBounceEase@MoonUnmanagedFactory@Moonlight@@SAPEAVBounceEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BounceEase *")]
        public static extern BounceEase* CreateBounceEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Brush *")]
        public static extern Brush* CreateBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCacheMode@MoonUnmanagedFactory@Moonlight@@SAPEAVCacheMode@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CacheMode *")]
        public static extern CacheMode* CreateCacheMode();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCanvas@MoonUnmanagedFactory@Moonlight@@SAPEAVCanvas@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Canvas *")]
        public static extern Canvas* CreateCanvas();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureDevice@MoonUnmanagedFactory@Moonlight@@SAPEAVCaptureDevice@2@H@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureDevice *")]
        public static extern CaptureDevice* CreateCaptureDevice(int object_type);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureDevice@MoonUnmanagedFactory@Moonlight@@SAPEAVCaptureDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureDevice *")]
        public static extern CaptureDevice* CreateCaptureDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureFormatChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCaptureFormatChangedEventArgs@2@PEAUAudioFormat@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureFormatChangedEventArgs *")]
        public static extern CaptureFormatChangedEventArgs* CreateCaptureFormatChangedEventArgs([NativeTypeName("Moonlight::AudioFormat *")] AudioFormat* newFormat);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureFormatChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCaptureFormatChangedEventArgs@2@PEAUVideoFormat@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureFormatChangedEventArgs *")]
        public static extern CaptureFormatChangedEventArgs* CreateCaptureFormatChangedEventArgs([NativeTypeName("Moonlight::VideoFormat *")] VideoFormat* newFormat);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureImageCompletedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCaptureImageCompletedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureImageCompletedEventArgs *")]
        public static extern CaptureImageCompletedEventArgs* CreateCaptureImageCompletedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureSource@MoonUnmanagedFactory@Moonlight@@SAPEAVCaptureSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureSource *")]
        public static extern CaptureSource* CreateCaptureSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCheckAndDownloadUpdateCompletedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCheckAndDownloadUpdateCompletedEventArgs@2@_NPEBD@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CheckAndDownloadUpdateCompletedEventArgs *")]
        public static extern CheckAndDownloadUpdateCompletedEventArgs* CreateCheckAndDownloadUpdateCompletedEventArgs([NativeTypeName("bool")] byte updated, [NativeTypeName("const char *")] sbyte* error);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCircleEase@MoonUnmanagedFactory@Moonlight@@SAPEAVCircleEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CircleEase *")]
        public static extern CircleEase* CreateCircleEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateClock@MoonUnmanagedFactory@Moonlight@@SAPEAVClock@2@PEAVTimeline@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Clock *")]
        public static extern Clock* CreateClock([NativeTypeName("Moonlight::Timeline *")] Timeline* timeline);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateClockGroup@MoonUnmanagedFactory@Moonlight@@SAPEAVClockGroup@2@PEAVTimelineGroup@2@_N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ClockGroup *")]
        public static extern ClockGroup* CreateClockGroup([NativeTypeName("Moonlight::TimelineGroup *")] TimelineGroup* timeline, [NativeTypeName("bool")] byte timeManagerClockGroup);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCollectionChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCollectionChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CollectionChangedEventArgs *")]
        public static extern CollectionChangedEventArgs* CreateCollectionChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCollectionItemChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCollectionItemChangedEventArgs@2@PEAVDependencyObject@2@PEAVDependencyProperty@2@PEAUValue@2@2@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CollectionItemChangedEventArgs *")]
        public static extern CollectionItemChangedEventArgs* CreateCollectionItemChangedEventArgs([NativeTypeName("Moonlight::DependencyObject *")] DependencyObject* collectionItem, [NativeTypeName("Moonlight::DependencyProperty *")] DependencyProperty* property, [NativeTypeName("Moonlight::Value *")] Value* oldValue, [NativeTypeName("Moonlight::Value *")] Value* newValue);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCollectionItemChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVCollectionItemChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CollectionItemChangedEventArgs *")]
        public static extern CollectionItemChangedEventArgs* CreateCollectionItemChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorAnimation@MoonUnmanagedFactory@Moonlight@@SAPEAVColorAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorAnimation *")]
        public static extern ColorAnimation* CreateColorAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorAnimationUsingKeyFrames@MoonUnmanagedFactory@Moonlight@@SAPEAVColorAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorAnimationUsingKeyFrames *")]
        public static extern ColorAnimationUsingKeyFrames* CreateColorAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorKeyFrame *")]
        public static extern ColorKeyFrame* CreateColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorKeyFrameCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVColorKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorKeyFrameCollection *")]
        public static extern ColorKeyFrameCollection* CreateColorKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColumnDefinition@MoonUnmanagedFactory@Moonlight@@SAPEAVColumnDefinition@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColumnDefinition *")]
        public static extern ColumnDefinition* CreateColumnDefinition();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColumnDefinitionCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVColumnDefinitionCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColumnDefinitionCollection *")]
        public static extern ColumnDefinitionCollection* CreateColumnDefinitionCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCompositeTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVCompositeTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CompositeTransform *")]
        public static extern CompositeTransform* CreateCompositeTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVContentChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentChangedEventArgs *")]
        public static extern ContentChangedEventArgs* CreateContentChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentControl@MoonUnmanagedFactory@Moonlight@@SAPEAVContentControl@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentControl *")]
        public static extern ContentControl* CreateContentControl();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentControlChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVContentControlChangedEventArgs@2@PEAUValue@2@0@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentControlChangedEventArgs *")]
        public static extern ContentControlChangedEventArgs* CreateContentControlChangedEventArgs([NativeTypeName("Moonlight::Value *")] Value* old_content, [NativeTypeName("Moonlight::Value *")] Value* new_content);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentPresenter@MoonUnmanagedFactory@Moonlight@@SAPEAVContentPresenter@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentPresenter *")]
        public static extern ContentPresenter* CreateContentPresenter();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateControl@MoonUnmanagedFactory@Moonlight@@SAPEAVControl@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Control *")]
        public static extern Control* CreateControl();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateControlTemplate@MoonUnmanagedFactory@Moonlight@@SAPEAVControlTemplate@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ControlTemplate *")]
        public static extern ControlTemplate* CreateControlTemplate();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCubicEase@MoonUnmanagedFactory@Moonlight@@SAPEAVCubicEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CubicEase *")]
        public static extern CubicEase* CreateCubicEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDataTemplate@MoonUnmanagedFactory@Moonlight@@SAPEAVDataTemplate@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DataTemplate *")]
        public static extern DataTemplate* CreateDataTemplate();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDeepZoomImageTileSource@MoonUnmanagedFactory@Moonlight@@SAPEAVDeepZoomImageTileSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DeepZoomImageTileSource *")]
        public static extern DeepZoomImageTileSource* CreateDeepZoomImageTileSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObject@MoonUnmanagedFactory@Moonlight@@SAPEAVDependencyObject@2@H@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObject *")]
        public static extern DependencyObject* CreateDependencyObject(int object_type);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObject@MoonUnmanagedFactory@Moonlight@@SAPEAVDependencyObject@2@PEAVDeployment@2@H@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObject *")]
        public static extern DependencyObject* CreateDependencyObject([NativeTypeName("Moonlight::Deployment *")] Deployment* deployment, int object_type);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObject@MoonUnmanagedFactory@Moonlight@@SAPEAVDependencyObject@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObject *")]
        public static extern DependencyObject* CreateDependencyObject();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObjectCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVDependencyObjectCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObjectCollection *")]
        public static extern DependencyObjectCollection* CreateDependencyObjectCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDeployment@MoonUnmanagedFactory@Moonlight@@SAPEAVDeployment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Deployment *")]
        public static extern Deployment* CreateDeployment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscreteColorKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVDiscreteColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscreteColorKeyFrame *")]
        public static extern DiscreteColorKeyFrame* CreateDiscreteColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscreteDoubleKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVDiscreteDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscreteDoubleKeyFrame *")]
        public static extern DiscreteDoubleKeyFrame* CreateDiscreteDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscreteObjectKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVDiscreteObjectKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscreteObjectKeyFrame *")]
        public static extern DiscreteObjectKeyFrame* CreateDiscreteObjectKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscretePointKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVDiscretePointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscretePointKeyFrame *")]
        public static extern DiscretePointKeyFrame* CreateDiscretePointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDispatcherTimer@MoonUnmanagedFactory@Moonlight@@SAPEAVDispatcherTimer@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DispatcherTimer *")]
        public static extern DispatcherTimer* CreateDispatcherTimer();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleAnimation@MoonUnmanagedFactory@Moonlight@@SAPEAVDoubleAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleAnimation *")]
        public static extern DoubleAnimation* CreateDoubleAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleAnimationUsingKeyFrames@MoonUnmanagedFactory@Moonlight@@SAPEAVDoubleAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleAnimationUsingKeyFrames *")]
        public static extern DoubleAnimationUsingKeyFrames* CreateDoubleAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVDoubleCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleCollection *")]
        public static extern DoubleCollection* CreateDoubleCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleKeyFrame *")]
        public static extern DoubleKeyFrame* CreateDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleKeyFrameCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVDoubleKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleKeyFrameCollection *")]
        public static extern DoubleKeyFrameCollection* CreateDoubleKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDownloader@MoonUnmanagedFactory@Moonlight@@SAPEAVDownloader@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Downloader *")]
        public static extern Downloader* CreateDownloader();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDownloadProgressEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVDownloadProgressEventArgs@2@N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DownloadProgressEventArgs *")]
        public static extern DownloadProgressEventArgs* CreateDownloadProgressEventArgs(double progress);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDownloadProgressEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVDownloadProgressEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DownloadProgressEventArgs *")]
        public static extern DownloadProgressEventArgs* CreateDownloadProgressEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDrawingAttributes@MoonUnmanagedFactory@Moonlight@@SAPEAVDrawingAttributes@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DrawingAttributes *")]
        public static extern DrawingAttributes* CreateDrawingAttributes();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDropShadowEffect@MoonUnmanagedFactory@Moonlight@@SAPEAVDropShadowEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DropShadowEffect *")]
        public static extern DropShadowEffect* CreateDropShadowEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingColorKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVEasingColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingColorKeyFrame *")]
        public static extern EasingColorKeyFrame* CreateEasingColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingDoubleKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVEasingDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingDoubleKeyFrame *")]
        public static extern EasingDoubleKeyFrame* CreateEasingDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingFunctionBase@MoonUnmanagedFactory@Moonlight@@SAPEAVEasingFunctionBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingFunctionBase *")]
        public static extern EasingFunctionBase* CreateEasingFunctionBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingPointKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVEasingPointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingPointKeyFrame *")]
        public static extern EasingPointKeyFrame* CreateEasingPointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEffect@MoonUnmanagedFactory@Moonlight@@SAPEAVEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Effect *")]
        public static extern Effect* CreateEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateElasticEase@MoonUnmanagedFactory@Moonlight@@SAPEAVElasticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ElasticEase *")]
        public static extern ElasticEase* CreateElasticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEllipse@MoonUnmanagedFactory@Moonlight@@SAPEAVEllipse@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Ellipse *")]
        public static extern Ellipse* CreateEllipse();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEllipseGeometry@MoonUnmanagedFactory@Moonlight@@SAPEAVEllipseGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EllipseGeometry *")]
        public static extern EllipseGeometry* CreateEllipseGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EventArgs *")]
        public static extern EventArgs* CreateEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEventTrigger@MoonUnmanagedFactory@Moonlight@@SAPEAVEventTrigger@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EventTrigger *")]
        public static extern EventTrigger* CreateEventTrigger();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExponentialEase@MoonUnmanagedFactory@Moonlight@@SAPEAVExponentialEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExponentialEase *")]
        public static extern ExponentialEase* CreateExponentialEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExtensionPart@MoonUnmanagedFactory@Moonlight@@SAPEAVExtensionPart@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExtensionPart *")]
        public static extern ExtensionPart* CreateExtensionPart();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExternalPart@MoonUnmanagedFactory@Moonlight@@SAPEAVExternalPart@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExternalPart *")]
        public static extern ExternalPart* CreateExternalPart();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExternalPartCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVExternalPartCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExternalPartCollection *")]
        public static extern ExternalPartCollection* CreateExternalPartCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateFrameworkElement@MoonUnmanagedFactory@Moonlight@@SAPEAVFrameworkElement@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::FrameworkElement *")]
        public static extern FrameworkElement* CreateFrameworkElement();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateFrameworkTemplate@MoonUnmanagedFactory@Moonlight@@SAPEAVFrameworkTemplate@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::FrameworkTemplate *")]
        public static extern FrameworkTemplate* CreateFrameworkTemplate();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeneralTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVGeneralTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GeneralTransform *")]
        public static extern GeneralTransform* CreateGeneralTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeometry@MoonUnmanagedFactory@Moonlight@@SAPEAVGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Geometry *")]
        public static extern Geometry* CreateGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeometryCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVGeometryCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GeometryCollection *")]
        public static extern GeometryCollection* CreateGeometryCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeometryGroup@MoonUnmanagedFactory@Moonlight@@SAPEAVGeometryGroup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GeometryGroup *")]
        public static extern GeometryGroup* CreateGeometryGroup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGlyphs@MoonUnmanagedFactory@Moonlight@@SAPEAVGlyphs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Glyphs *")]
        public static extern Glyphs* CreateGlyphs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGlyphTypefaceCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVGlyphTypefaceCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GlyphTypefaceCollection *")]
        public static extern GlyphTypefaceCollection* CreateGlyphTypefaceCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGradientBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVGradientBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GradientBrush *")]
        public static extern GradientBrush* CreateGradientBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGradientStop@MoonUnmanagedFactory@Moonlight@@SAPEAVGradientStop@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GradientStop *")]
        public static extern GradientStop* CreateGradientStop();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGradientStopCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVGradientStopCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GradientStopCollection *")]
        public static extern GradientStopCollection* CreateGradientStopCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGrid@MoonUnmanagedFactory@Moonlight@@SAPEAVGrid@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Grid *")]
        public static extern Grid* CreateGrid();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHitTestCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVHitTestCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HitTestCollection *")]
        public static extern HitTestCollection* CreateHitTestCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHttpRequestProgressChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVHttpRequestProgressChangedEventArgs@2@N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HttpRequestProgressChangedEventArgs *")]
        public static extern HttpRequestProgressChangedEventArgs* CreateHttpRequestProgressChangedEventArgs(double progress);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHttpRequestStoppedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVHttpRequestStoppedEventArgs@2@PEBD@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HttpRequestStoppedEventArgs *")]
        public static extern HttpRequestStoppedEventArgs* CreateHttpRequestStoppedEventArgs([NativeTypeName("const char *")] sbyte* error_msg);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHttpRequestWriteEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVHttpRequestWriteEventArgs@2@PEAX_JH@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HttpRequestWriteEventArgs *")]
        public static extern HttpRequestWriteEventArgs* CreateHttpRequestWriteEventArgs(void* data, [NativeTypeName("gint64")] long offset, [NativeTypeName("gint32")] int count);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHyperlink@MoonUnmanagedFactory@Moonlight@@SAPEAVHyperlink@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Hyperlink *")]
        public static extern Hyperlink* CreateHyperlink();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateIcon@MoonUnmanagedFactory@Moonlight@@SAPEAVIcon@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Icon *")]
        public static extern Icon* CreateIcon();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateIconCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVIconCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::IconCollection *")]
        public static extern IconCollection* CreateIconCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImage@MoonUnmanagedFactory@Moonlight@@SAPEAVImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Image *")]
        public static extern Image* CreateImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImageBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVImageBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ImageBrush *")]
        public static extern ImageBrush* CreateImageBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImageSource@MoonUnmanagedFactory@Moonlight@@SAPEAVImageSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ImageSource *")]
        public static extern ImageSource* CreateImageSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateIMediaObject@MoonUnmanagedFactory@Moonlight@@SAPEAVIMediaObject@2@HPEAVMedia@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::IMediaObject *")]
        public static extern IMediaObject* CreateIMediaObject(int kind, [NativeTypeName("Moonlight::Media *")] Media* media);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImplicitInputBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVImplicitInputBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ImplicitInputBrush *")]
        public static extern ImplicitInputBrush* CreateImplicitInputBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInkPresenter@MoonUnmanagedFactory@Moonlight@@SAPEAVInkPresenter@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InkPresenter *")]
        public static extern InkPresenter* CreateInkPresenter();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInline@MoonUnmanagedFactory@Moonlight@@SAPEAVInline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Inline *")]
        public static extern Inline* CreateInline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInlineCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVInlineCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InlineCollection *")]
        public static extern InlineCollection* CreateInlineCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInlineUIContainer@MoonUnmanagedFactory@Moonlight@@SAPEAVInlineUIContainer@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InlineUIContainer *")]
        public static extern InlineUIContainer* CreateInlineUIContainer();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInputMethod@MoonUnmanagedFactory@Moonlight@@SAPEAVInputMethod@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InputMethod *")]
        public static extern InputMethod* CreateInputMethod();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInputScope@MoonUnmanagedFactory@Moonlight@@SAPEAVInputScope@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InputScope *")]
        public static extern InputScope* CreateInputScope();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInputScopeName@MoonUnmanagedFactory@Moonlight@@SAPEAVInputScopeName@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InputScopeName *")]
        public static extern InputScopeName* CreateInputScopeName();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInternalTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVInternalTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InternalTransform *")]
        public static extern InternalTransform* CreateInternalTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateItalic@MoonUnmanagedFactory@Moonlight@@SAPEAVItalic@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Italic *")]
        public static extern Italic* CreateItalic();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateItemCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVItemCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ItemCollection *")]
        public static extern ItemCollection* CreateItemCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateKeyEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVKeyEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::KeyEventArgs *")]
        public static extern KeyEventArgs* CreateKeyEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateKeyFrameCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::KeyFrameCollection *")]
        public static extern KeyFrameCollection* CreateKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateKeySpline@MoonUnmanagedFactory@Moonlight@@SAPEAVKeySpline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::KeySpline *")]
        public static extern KeySpline* CreateKeySpline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLine@MoonUnmanagedFactory@Moonlight@@SAPEAVLine@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Line *")]
        public static extern Line* CreateLine();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearColorKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVLinearColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearColorKeyFrame *")]
        public static extern LinearColorKeyFrame* CreateLinearColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearDoubleKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVLinearDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearDoubleKeyFrame *")]
        public static extern LinearDoubleKeyFrame* CreateLinearDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearGradientBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVLinearGradientBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearGradientBrush *")]
        public static extern LinearGradientBrush* CreateLinearGradientBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearPointKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVLinearPointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearPointKeyFrame *")]
        public static extern LinearPointKeyFrame* CreateLinearPointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLineBreak@MoonUnmanagedFactory@Moonlight@@SAPEAVLineBreak@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LineBreak *")]
        public static extern LineBreak* CreateLineBreak();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLineGeometry@MoonUnmanagedFactory@Moonlight@@SAPEAVLineGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LineGeometry *")]
        public static extern LineGeometry* CreateLineGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLineSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVLineSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LineSegment *")]
        public static extern LineSegment* CreateLineSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLocalMessageReceiver@MoonUnmanagedFactory@Moonlight@@SAPEAVLocalMessageReceiver@2@PEBDH@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LocalMessageReceiver *")]
        public static extern LocalMessageReceiver* CreateLocalMessageReceiver([NativeTypeName("const char *")] sbyte* receiverName, int namescope);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLocalMessageSender@MoonUnmanagedFactory@Moonlight@@SAPEAVLocalMessageSender@2@PEBD0@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LocalMessageSender *")]
        public static extern LocalMessageSender* CreateLocalMessageSender([NativeTypeName("const char *")] sbyte* receiverName, [NativeTypeName("const char *")] sbyte* receiverDomain);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLogReadyRoutedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVLogReadyRoutedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LogReadyRoutedEventArgs *")]
        public static extern LogReadyRoutedEventArgs* CreateLogReadyRoutedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateManualTimeSource@MoonUnmanagedFactory@Moonlight@@SAPEAVManualTimeSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ManualTimeSource *")]
        public static extern ManualTimeSource* CreateManualTimeSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMarkerStream@MoonUnmanagedFactory@Moonlight@@SAPEAVMarkerStream@2@PEAVMedia@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MarkerStream *")]
        public static extern MarkerStream* CreateMarkerStream([NativeTypeName("Moonlight::Media *")] Media* media);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMatrix3DProjection@MoonUnmanagedFactory@Moonlight@@SAPEAVMatrix3DProjection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Matrix3DProjection *")]
        public static extern Matrix3DProjection* CreateMatrix3DProjection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMatrixTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVMatrixTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MatrixTransform *")]
        public static extern MatrixTransform* CreateMatrixTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMediaBase@MoonUnmanagedFactory@Moonlight@@SAPEAVMediaBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MediaBase *")]
        public static extern MediaBase* CreateMediaBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMediaElement@MoonUnmanagedFactory@Moonlight@@SAPEAVMediaElement@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MediaElement *")]
        public static extern MediaElement* CreateMediaElement();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMediaPlayer@MoonUnmanagedFactory@Moonlight@@SAPEAVMediaPlayer@2@PEAVMediaElement@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MediaPlayer *")]
        public static extern MediaPlayer* CreateMediaPlayer([NativeTypeName("Moonlight::MediaElement *")] MediaElement* element);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMessageReceivedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVMessageReceivedEventArgs@2@PEBDH00@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MessageReceivedEventArgs *")]
        public static extern MessageReceivedEventArgs* CreateMessageReceivedEventArgs([NativeTypeName("const char *")] sbyte* message, int namescope, [NativeTypeName("const char *")] sbyte* receiverName, [NativeTypeName("const char *")] sbyte* senderDomain);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMmsDemuxer@MoonUnmanagedFactory@Moonlight@@SAPEAVMmsDemuxer@2@PEAVMedia@2@PEAVMmsSource@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MmsDemuxer *")]
        public static extern MmsDemuxer* CreateMmsDemuxer([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::MmsSource *")] MmsSource* source);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMmsPlaylistEntry@MoonUnmanagedFactory@Moonlight@@SAPEAVMmsPlaylistEntry@2@PEAVMedia@2@PEAVMmsSource@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MmsPlaylistEntry *")]
        public static extern MmsPlaylistEntry* CreateMmsPlaylistEntry([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::MmsSource *")] MmsSource* source);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMouseButtonEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVMouseButtonEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MouseButtonEventArgs *")]
        public static extern MouseButtonEventArgs* CreateMouseButtonEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMouseEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVMouseEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MouseEventArgs *")]
        public static extern MouseEventArgs* CreateMouseEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMouseWheelEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVMouseWheelEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MouseWheelEventArgs *")]
        public static extern MouseWheelEventArgs* CreateMouseWheelEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleImage@MoonUnmanagedFactory@Moonlight@@SAPEAVMultiScaleImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleImage *")]
        public static extern MultiScaleImage* CreateMultiScaleImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleSubImage@MoonUnmanagedFactory@Moonlight@@SAPEAVMultiScaleSubImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleSubImage *")]
        public static extern MultiScaleSubImage* CreateMultiScaleSubImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleSubImageCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVMultiScaleSubImageCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleSubImageCollection *")]
        public static extern MultiScaleSubImageCollection* CreateMultiScaleSubImageCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleTileSource@MoonUnmanagedFactory@Moonlight@@SAPEAVMultiScaleTileSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleTileSource *")]
        public static extern MultiScaleTileSource* CreateMultiScaleTileSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateNameScope@MoonUnmanagedFactory@Moonlight@@SAPEAVNameScope@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::NameScope *")]
        public static extern NameScope* CreateNameScope();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateNotificationWindow@MoonUnmanagedFactory@Moonlight@@SAPEAVNotificationWindow@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::NotificationWindow *")]
        public static extern NotificationWindow* CreateNotificationWindow();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateNullDecoder@MoonUnmanagedFactory@Moonlight@@SAPEAVNullDecoder@2@PEAVMedia@2@PEAVIMediaStream@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::NullDecoder *")]
        public static extern NullDecoder* CreateNullDecoder([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::IMediaStream *")] IMediaStream* stream);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateObjectAnimationUsingKeyFrames@MoonUnmanagedFactory@Moonlight@@SAPEAVObjectAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ObjectAnimationUsingKeyFrames *")]
        public static extern ObjectAnimationUsingKeyFrames* CreateObjectAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateObjectKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVObjectKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ObjectKeyFrame *")]
        public static extern ObjectKeyFrame* CreateObjectKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateObjectKeyFrameCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVObjectKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ObjectKeyFrameCollection *")]
        public static extern ObjectKeyFrameCollection* CreateObjectKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateOutOfBrowserSettings@MoonUnmanagedFactory@Moonlight@@SAPEAVOutOfBrowserSettings@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::OutOfBrowserSettings *")]
        public static extern OutOfBrowserSettings* CreateOutOfBrowserSettings();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePanel@MoonUnmanagedFactory@Moonlight@@SAPEAVPanel@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Panel *")]
        public static extern Panel* CreatePanel();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateParagraph@MoonUnmanagedFactory@Moonlight@@SAPEAVParagraph@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Paragraph *")]
        public static extern Paragraph* CreateParagraph();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateParallelTimeline@MoonUnmanagedFactory@Moonlight@@SAPEAVParallelTimeline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ParallelTimeline *")]
        public static extern ParallelTimeline* CreateParallelTimeline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateParserErrorEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVParserErrorEventArgs@2@PEAVDependencyObject@2@PEBD1HHH11@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ParserErrorEventArgs *")]
        public static extern ParserErrorEventArgs* CreateParserErrorEventArgs([NativeTypeName("Moonlight::DependencyObject *")] DependencyObject* original_source, [NativeTypeName("const char *")] sbyte* msg, [NativeTypeName("const char *")] sbyte* file, int line, int column, int error_code, [NativeTypeName("const char *")] sbyte* element, [NativeTypeName("const char *")] sbyte* attribute);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePassThroughDecoder@MoonUnmanagedFactory@Moonlight@@SAPEAVPassThroughDecoder@2@PEAVMedia@2@PEAVIMediaStream@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PassThroughDecoder *")]
        public static extern PassThroughDecoder* CreatePassThroughDecoder([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::IMediaStream *")] IMediaStream* stream);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePasswordBox@MoonUnmanagedFactory@Moonlight@@SAPEAVPasswordBox@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PasswordBox *")]
        public static extern PasswordBox* CreatePasswordBox();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePath@MoonUnmanagedFactory@Moonlight@@SAPEAVPath@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Path *")]
        public static extern Path* CreatePath();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathFigure@MoonUnmanagedFactory@Moonlight@@SAPEAVPathFigure@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathFigure *")]
        public static extern PathFigure* CreatePathFigure();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathFigureCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVPathFigureCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathFigureCollection *")]
        public static extern PathFigureCollection* CreatePathFigureCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathGeometry@MoonUnmanagedFactory@Moonlight@@SAPEAVPathGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathGeometry *")]
        public static extern PathGeometry* CreatePathGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVPathSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathSegment *")]
        public static extern PathSegment* CreatePathSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathSegmentCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVPathSegmentCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathSegmentCollection *")]
        public static extern PathSegmentCollection* CreatePathSegmentCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePixelShader@MoonUnmanagedFactory@Moonlight@@SAPEAVPixelShader@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PixelShader *")]
        public static extern PixelShader* CreatePixelShader();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePlaneProjection@MoonUnmanagedFactory@Moonlight@@SAPEAVPlaneProjection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PlaneProjection *")]
        public static extern PlaneProjection* CreatePlaneProjection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointAnimation@MoonUnmanagedFactory@Moonlight@@SAPEAVPointAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointAnimation *")]
        public static extern PointAnimation* CreatePointAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointAnimationUsingKeyFrames@MoonUnmanagedFactory@Moonlight@@SAPEAVPointAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointAnimationUsingKeyFrames *")]
        public static extern PointAnimationUsingKeyFrames* CreatePointAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVPointCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointCollection *")]
        public static extern PointCollection* CreatePointCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVPointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointKeyFrame *")]
        public static extern PointKeyFrame* CreatePointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointKeyFrameCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVPointKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointKeyFrameCollection *")]
        public static extern PointKeyFrameCollection* CreatePointKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyBezierSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVPolyBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PolyBezierSegment *")]
        public static extern PolyBezierSegment* CreatePolyBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolygon@MoonUnmanagedFactory@Moonlight@@SAPEAVPolygon@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Polygon *")]
        public static extern Polygon* CreatePolygon();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyline@MoonUnmanagedFactory@Moonlight@@SAPEAVPolyline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Polyline *")]
        public static extern Polyline* CreatePolyline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyLineSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVPolyLineSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PolyLineSegment *")]
        public static extern PolyLineSegment* CreatePolyLineSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyQuadraticBezierSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVPolyQuadraticBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PolyQuadraticBezierSegment *")]
        public static extern PolyQuadraticBezierSegment* CreatePolyQuadraticBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePopup@MoonUnmanagedFactory@Moonlight@@SAPEAVPopup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Popup *")]
        public static extern Popup* CreatePopup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePowerEase@MoonUnmanagedFactory@Moonlight@@SAPEAVPowerEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PowerEase *")]
        public static extern PowerEase* CreatePowerEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePrintDocument@MoonUnmanagedFactory@Moonlight@@SAPEAVPrintDocument@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PrintDocument *")]
        public static extern PrintDocument* CreatePrintDocument();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateProgressEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVProgressEventArgs@2@NN@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ProgressEventArgs *")]
        public static extern ProgressEventArgs* CreateProgressEventArgs(double progress, double offset);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateProjection@MoonUnmanagedFactory@Moonlight@@SAPEAVProjection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Projection *")]
        public static extern Projection* CreateProjection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePropertyChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVPropertyChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PropertyChangedEventArgs *")]
        public static extern PropertyChangedEventArgs* CreatePropertyChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePropertyChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVPropertyChangedEventArgs@2@PEAVDependencyProperty@2@HPEAUValue@2@1_N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PropertyChangedEventArgs *")]
        public static extern PropertyChangedEventArgs* CreatePropertyChangedEventArgs([NativeTypeName("Moonlight::DependencyProperty *")] DependencyProperty* p, int pid, [NativeTypeName("Moonlight::Value *")] Value* ov, [NativeTypeName("Moonlight::Value *")] Value* nv, [NativeTypeName("bool")] byte copy_values);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuadraticBezierSegment@MoonUnmanagedFactory@Moonlight@@SAPEAVQuadraticBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuadraticBezierSegment *")]
        public static extern QuadraticBezierSegment* CreateQuadraticBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuadraticEase@MoonUnmanagedFactory@Moonlight@@SAPEAVQuadraticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuadraticEase *")]
        public static extern QuadraticEase* CreateQuadraticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuarticEase@MoonUnmanagedFactory@Moonlight@@SAPEAVQuarticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuarticEase *")]
        public static extern QuarticEase* CreateQuarticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuinticEase@MoonUnmanagedFactory@Moonlight@@SAPEAVQuinticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuinticEase *")]
        public static extern QuinticEase* CreateQuinticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRadialGradientBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVRadialGradientBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RadialGradientBrush *")]
        public static extern RadialGradientBrush* CreateRadialGradientBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRectangle@MoonUnmanagedFactory@Moonlight@@SAPEAVRectangle@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Rectangle *")]
        public static extern Rectangle* CreateRectangle();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRectangleGeometry@MoonUnmanagedFactory@Moonlight@@SAPEAVRectangleGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RectangleGeometry *")]
        public static extern RectangleGeometry* CreateRectangleGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRenderingEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVRenderingEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RenderingEventArgs *")]
        public static extern RenderingEventArgs* CreateRenderingEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateResourceDictionary@MoonUnmanagedFactory@Moonlight@@SAPEAVResourceDictionary@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ResourceDictionary *")]
        public static extern ResourceDictionary* CreateResourceDictionary();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateResourceDictionaryChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVResourceDictionaryChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ResourceDictionaryChangedEventArgs *")]
        public static extern ResourceDictionaryChangedEventArgs* CreateResourceDictionaryChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateResourceDictionaryCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVResourceDictionaryCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ResourceDictionaryCollection *")]
        public static extern ResourceDictionaryCollection* CreateResourceDictionaryCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRichTextBox@MoonUnmanagedFactory@Moonlight@@SAPEAVRichTextBox@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RichTextBox *")]
        public static extern RichTextBox* CreateRichTextBox();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRichTextBoxView@MoonUnmanagedFactory@Moonlight@@SAPEAVRichTextBoxView@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RichTextBoxView *")]
        public static extern RichTextBoxView* CreateRichTextBoxView();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRotateTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVRotateTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RotateTransform *")]
        public static extern RotateTransform* CreateRotateTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRoutedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVRoutedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RoutedEventArgs *")]
        public static extern RoutedEventArgs* CreateRoutedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRowDefinition@MoonUnmanagedFactory@Moonlight@@SAPEAVRowDefinition@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RowDefinition *")]
        public static extern RowDefinition* CreateRowDefinition();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRowDefinitionCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVRowDefinitionCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RowDefinitionCollection *")]
        public static extern RowDefinitionCollection* CreateRowDefinitionCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRun@MoonUnmanagedFactory@Moonlight@@SAPEAVRun@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Run *")]
        public static extern Run* CreateRun();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSampleReadyEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVSampleReadyEventArgs@2@_J0PEAXHPEAUAudioFormat@2@PEAUVideoFormat@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SampleReadyEventArgs *")]
        public static extern SampleReadyEventArgs* CreateSampleReadyEventArgs([NativeTypeName("gint64")] long sampleTime, [NativeTypeName("gint64")] long frameDuration, void* sampleData, int sampleDataLength, [NativeTypeName("Moonlight::AudioFormat *")] AudioFormat* aformat, [NativeTypeName("Moonlight::VideoFormat *")] VideoFormat* vformat);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateScaleTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVScaleTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ScaleTransform *")]
        public static extern ScaleTransform* CreateScaleTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSection@MoonUnmanagedFactory@Moonlight@@SAPEAVSection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Section *")]
        public static extern Section* CreateSection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSecuritySettings@MoonUnmanagedFactory@Moonlight@@SAPEAVSecuritySettings@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SecuritySettings *")]
        public static extern SecuritySettings* CreateSecuritySettings();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSendCompletedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVSendCompletedEventArgs@2@PEAVMoonError@2@PEBD111PEAX@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SendCompletedEventArgs *")]
        public static extern SendCompletedEventArgs* CreateSendCompletedEventArgs([NativeTypeName("Moonlight::MoonError *")] MoonError* error, [NativeTypeName("const char *")] sbyte* message, [NativeTypeName("const char *")] sbyte* receiverName, [NativeTypeName("const char *")] sbyte* receiverDomain, [NativeTypeName("const char *")] sbyte* response, void* managedUserState);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSetter@MoonUnmanagedFactory@Moonlight@@SAPEAVSetter@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Setter *")]
        public static extern Setter* CreateSetter();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSetterBase@MoonUnmanagedFactory@Moonlight@@SAPEAVSetterBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SetterBase *")]
        public static extern SetterBase* CreateSetterBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSetterBaseCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVSetterBaseCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SetterBaseCollection *")]
        public static extern SetterBaseCollection* CreateSetterBaseCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateShaderEffect@MoonUnmanagedFactory@Moonlight@@SAPEAVShaderEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ShaderEffect *")]
        public static extern ShaderEffect* CreateShaderEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateShape@MoonUnmanagedFactory@Moonlight@@SAPEAVShape@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Shape *")]
        public static extern Shape* CreateShape();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSineEase@MoonUnmanagedFactory@Moonlight@@SAPEAVSineEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SineEase *")]
        public static extern SineEase* CreateSineEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSizeChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVSizeChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SizeChangedEventArgs *")]
        public static extern SizeChangedEventArgs* CreateSizeChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSkewTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVSkewTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SkewTransform *")]
        public static extern SkewTransform* CreateSkewTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSolidColorBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVSolidColorBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SolidColorBrush *")]
        public static extern SolidColorBrush* CreateSolidColorBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSpan@MoonUnmanagedFactory@Moonlight@@SAPEAVSpan@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Span *")]
        public static extern Span* CreateSpan();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSplineColorKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVSplineColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SplineColorKeyFrame *")]
        public static extern SplineColorKeyFrame* CreateSplineColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSplineDoubleKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVSplineDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SplineDoubleKeyFrame *")]
        public static extern SplineDoubleKeyFrame* CreateSplineDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSplinePointKeyFrame@MoonUnmanagedFactory@Moonlight@@SAPEAVSplinePointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SplinePointKeyFrame *")]
        public static extern SplinePointKeyFrame* CreateSplinePointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStoryboard@MoonUnmanagedFactory@Moonlight@@SAPEAVStoryboard@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Storyboard *")]
        public static extern Storyboard* CreateStoryboard();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStroke@MoonUnmanagedFactory@Moonlight@@SAPEAVStroke@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Stroke *")]
        public static extern Stroke* CreateStroke();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStrokeCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVStrokeCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::StrokeCollection *")]
        public static extern StrokeCollection* CreateStrokeCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStyle@MoonUnmanagedFactory@Moonlight@@SAPEAVStyle@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Style *")]
        public static extern Style* CreateStyle();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStylusInfo@MoonUnmanagedFactory@Moonlight@@SAPEAVStylusInfo@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::StylusInfo *")]
        public static extern StylusInfo* CreateStylusInfo();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStylusPointCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVStylusPointCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::StylusPointCollection *")]
        public static extern StylusPointCollection* CreateStylusPointCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSystemTimeSource@MoonUnmanagedFactory@Moonlight@@SAPEAVSystemTimeSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SystemTimeSource *")]
        public static extern SystemTimeSource* CreateSystemTimeSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSystemTimeSource@MoonUnmanagedFactory@Moonlight@@SAPEAVSystemTimeSource@2@PEAVDeployment@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SystemTimeSource *")]
        public static extern SystemTimeSource* CreateSystemTimeSource([NativeTypeName("Moonlight::Deployment *")] Deployment* deployment);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBlock@MoonUnmanagedFactory@Moonlight@@SAPEAVTextBlock@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBlock *")]
        public static extern TextBlock* CreateTextBlock();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBox@MoonUnmanagedFactory@Moonlight@@SAPEAVTextBox@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBox *")]
        public static extern TextBox* CreateTextBox();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBoxModelChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVTextBoxModelChangedEventArgs@2@HPEAVPropertyChangedEventArgs@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBoxModelChangedEventArgs *")]
        public static extern TextBoxModelChangedEventArgs* CreateTextBoxModelChangedEventArgs(int changed, [NativeTypeName("Moonlight::PropertyChangedEventArgs *")] PropertyChangedEventArgs* property);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBoxView@MoonUnmanagedFactory@Moonlight@@SAPEAVTextBoxView@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBoxView *")]
        public static extern TextBoxView* CreateTextBoxView();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextChangedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVTextChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextChangedEventArgs *")]
        public static extern TextChangedEventArgs* CreateTextChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextElement@MoonUnmanagedFactory@Moonlight@@SAPEAVTextElement@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextElement *")]
        public static extern TextElement* CreateTextElement();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextElementCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTextElementCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextElementCollection *")]
        public static extern TextElementCollection* CreateTextElementCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTileBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVTileBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TileBrush *")]
        public static extern TileBrush* CreateTileBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTileLayerInvalidatedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVTileLayerInvalidatedEventArgs@2@HHHH@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TileLayerInvalidatedEventArgs *")]
        public static extern TileLayerInvalidatedEventArgs* CreateTileLayerInvalidatedEventArgs(int level, int x, int y, int layer);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeline@MoonUnmanagedFactory@Moonlight@@SAPEAVTimeline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Timeline *")]
        public static extern Timeline* CreateTimeline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTimelineCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineCollection *")]
        public static extern TimelineCollection* CreateTimelineCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineGroup@MoonUnmanagedFactory@Moonlight@@SAPEAVTimelineGroup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineGroup *")]
        public static extern TimelineGroup* CreateTimelineGroup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineMarker@MoonUnmanagedFactory@Moonlight@@SAPEAVTimelineMarker@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineMarker *")]
        public static extern TimelineMarker* CreateTimelineMarker();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineMarkerCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTimelineMarkerCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineMarkerCollection *")]
        public static extern TimelineMarkerCollection* CreateTimelineMarkerCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineMarkerRoutedEventArgs@MoonUnmanagedFactory@Moonlight@@SAPEAVTimelineMarkerRoutedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineMarkerRoutedEventArgs *")]
        public static extern TimelineMarkerRoutedEventArgs* CreateTimelineMarkerRoutedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeManager@MoonUnmanagedFactory@Moonlight@@SAPEAVTimeManager@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimeManager *")]
        public static extern TimeManager* CreateTimeManager();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeSource@MoonUnmanagedFactory@Moonlight@@SAPEAVTimeSource@2@PEAVDeployment@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimeSource *")]
        public static extern TimeSource* CreateTimeSource([NativeTypeName("Moonlight::Deployment *")] Deployment* deployment);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeSource@MoonUnmanagedFactory@Moonlight@@SAPEAVTimeSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimeSource *")]
        public static extern TimeSource* CreateTimeSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTouchDevice@MoonUnmanagedFactory@Moonlight@@SAPEAVTouchDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TouchDevice *")]
        public static extern TouchDevice* CreateTouchDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTouchPoint@MoonUnmanagedFactory@Moonlight@@SAPEAVTouchPoint@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TouchPoint *")]
        public static extern TouchPoint* CreateTouchPoint();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTouchPointCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTouchPointCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TouchPointCollection *")]
        public static extern TouchPointCollection* CreateTouchPointCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Transform *")]
        public static extern Transform* CreateTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTransformCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTransformCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TransformCollection *")]
        public static extern TransformCollection* CreateTransformCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTransformGroup@MoonUnmanagedFactory@Moonlight@@SAPEAVTransformGroup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TransformGroup *")]
        public static extern TransformGroup* CreateTransformGroup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTranslateTransform@MoonUnmanagedFactory@Moonlight@@SAPEAVTranslateTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TranslateTransform *")]
        public static extern TranslateTransform* CreateTranslateTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerAction@MoonUnmanagedFactory@Moonlight@@SAPEAVTriggerAction@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerAction *")]
        public static extern TriggerAction* CreateTriggerAction();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerActionCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTriggerActionCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerActionCollection *")]
        public static extern TriggerActionCollection* CreateTriggerActionCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerBase@MoonUnmanagedFactory@Moonlight@@SAPEAVTriggerBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerBase *")]
        public static extern TriggerBase* CreateTriggerBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVTriggerCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerCollection *")]
        public static extern TriggerCollection* CreateTriggerCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUIElementCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVUIElementCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::UIElementCollection *")]
        public static extern UIElementCollection* CreateUIElementCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUnderline@MoonUnmanagedFactory@Moonlight@@SAPEAVUnderline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Underline *")]
        public static extern Underline* CreateUnderline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUnmanagedStylusPoint@MoonUnmanagedFactory@Moonlight@@SAPEAVUnmanagedStylusPoint@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::UnmanagedStylusPoint *")]
        public static extern UnmanagedStylusPoint* CreateUnmanagedStylusPoint();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUserControl@MoonUnmanagedFactory@Moonlight@@SAPEAVUserControl@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::UserControl *")]
        public static extern UserControl* CreateUserControl();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVVideoBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoBrush *")]
        public static extern VideoBrush* CreateVideoBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoCaptureDevice@MoonUnmanagedFactory@Moonlight@@SAPEAVVideoCaptureDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoCaptureDevice *")]
        public static extern VideoCaptureDevice* CreateVideoCaptureDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoCaptureDeviceCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVVideoCaptureDeviceCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoCaptureDeviceCollection *")]
        public static extern VideoCaptureDeviceCollection* CreateVideoCaptureDeviceCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoFormatCollection@MoonUnmanagedFactory@Moonlight@@SAPEAVVideoFormatCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoFormatCollection *")]
        public static extern VideoFormatCollection* CreateVideoFormatCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoStream@MoonUnmanagedFactory@Moonlight@@SAPEAVVideoStream@2@PEAVMedia@2@HII_KPEAXI@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoStream *")]
        public static extern VideoStream* CreateVideoStream([NativeTypeName("Moonlight::Media *")] Media* media, int codec_id, [NativeTypeName("guint32")] uint width, [NativeTypeName("guint32")] uint height, [NativeTypeName("guint64")] ulong duration, [NativeTypeName("gpointer")] void* extra_data, [NativeTypeName("guint32")] uint extra_data_size);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVisualBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVVisualBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VisualBrush *")]
        public static extern VisualBrush* CreateVisualBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWebBrowser@MoonUnmanagedFactory@Moonlight@@SAPEAVWebBrowser@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WebBrowser *")]
        public static extern WebBrowser* CreateWebBrowser();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWebBrowserBrush@MoonUnmanagedFactory@Moonlight@@SAPEAVWebBrowserBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WebBrowserBrush *")]
        public static extern WebBrowserBrush* CreateWebBrowserBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWindow@MoonUnmanagedFactory@Moonlight@@SAPEAVWindow@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Window *")]
        public static extern Window* CreateWindow();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWindowSettings@MoonUnmanagedFactory@Moonlight@@SAPEAVWindowSettings@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WindowSettings *")]
        public static extern WindowSettings* CreateWindowSettings();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWriteableBitmap@MoonUnmanagedFactory@Moonlight@@SAPEAVWriteableBitmap@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WriteableBitmap *")]
        public static extern WriteableBitmap* CreateWriteableBitmap();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateYUVConverter@MoonUnmanagedFactory@Moonlight@@SAPEAVYUVConverter@2@PEAVMedia@2@PEAVVideoStream@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::YUVConverter *")]
        public static extern YUVConverter* CreateYUVConverter([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::VideoStream *")] VideoStream* stream);
    }

    public unsafe partial struct MoonManagedFactory
    {
        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAnimation@MoonManagedFactory@Moonlight@@SAPEAVAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Animation *")]
        public static extern Animation* CreateAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAnimationClock@MoonManagedFactory@Moonlight@@SAPEAVAnimationClock@2@PEAVAnimation@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AnimationClock *")]
        public static extern AnimationClock* CreateAnimationClock([NativeTypeName("Moonlight::Animation *")] Animation* timeline);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateApplication@MoonManagedFactory@Moonlight@@SAPEAVApplication@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Application *")]
        public static extern Application* CreateApplication();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateArcSegment@MoonManagedFactory@Moonlight@@SAPEAVArcSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ArcSegment *")]
        public static extern ArcSegment* CreateArcSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAssemblyPart@MoonManagedFactory@Moonlight@@SAPEAVAssemblyPart@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AssemblyPart *")]
        public static extern AssemblyPart* CreateAssemblyPart();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAssemblyPartCollection@MoonManagedFactory@Moonlight@@SAPEAVAssemblyPartCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AssemblyPartCollection *")]
        public static extern AssemblyPartCollection* CreateAssemblyPartCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioCaptureDevice@MoonManagedFactory@Moonlight@@SAPEAVAudioCaptureDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioCaptureDevice *")]
        public static extern AudioCaptureDevice* CreateAudioCaptureDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioCaptureDeviceCollection@MoonManagedFactory@Moonlight@@SAPEAVAudioCaptureDeviceCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioCaptureDeviceCollection *")]
        public static extern AudioCaptureDeviceCollection* CreateAudioCaptureDeviceCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioFormatCollection@MoonManagedFactory@Moonlight@@SAPEAVAudioFormatCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioFormatCollection *")]
        public static extern AudioFormatCollection* CreateAudioFormatCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateAudioStream@MoonManagedFactory@Moonlight@@SAPEAVAudioStream@2@PEAVMedia@2@HHHHHHPEAXI@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::AudioStream *")]
        public static extern AudioStream* CreateAudioStream([NativeTypeName("Moonlight::Media *")] Media* media, int codec_id, int bits_per_sample, int block_align, int sample_rate, int channels, int bit_rate, [NativeTypeName("gpointer")] void* extra_data, [NativeTypeName("guint32")] uint extra_data_size);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBackEase@MoonManagedFactory@Moonlight@@SAPEAVBackEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BackEase *")]
        public static extern BackEase* CreateBackEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBeginStoryboard@MoonManagedFactory@Moonlight@@SAPEAVBeginStoryboard@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BeginStoryboard *")]
        public static extern BeginStoryboard* CreateBeginStoryboard();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBezierSegment@MoonManagedFactory@Moonlight@@SAPEAVBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BezierSegment *")]
        public static extern BezierSegment* CreateBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBitmapCache@MoonManagedFactory@Moonlight@@SAPEAVBitmapCache@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BitmapCache *")]
        public static extern BitmapCache* CreateBitmapCache();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBitmapImage@MoonManagedFactory@Moonlight@@SAPEAVBitmapImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BitmapImage *")]
        public static extern BitmapImage* CreateBitmapImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBitmapSource@MoonManagedFactory@Moonlight@@SAPEAVBitmapSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BitmapSource *")]
        public static extern BitmapSource* CreateBitmapSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBlock@MoonManagedFactory@Moonlight@@SAPEAVBlock@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Block *")]
        public static extern Block* CreateBlock();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBlockCollection@MoonManagedFactory@Moonlight@@SAPEAVBlockCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BlockCollection *")]
        public static extern BlockCollection* CreateBlockCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBlurEffect@MoonManagedFactory@Moonlight@@SAPEAVBlurEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BlurEffect *")]
        public static extern BlurEffect* CreateBlurEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBold@MoonManagedFactory@Moonlight@@SAPEAVBold@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Bold *")]
        public static extern Bold* CreateBold();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBorder@MoonManagedFactory@Moonlight@@SAPEAVBorder@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Border *")]
        public static extern Border* CreateBorder();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBounceEase@MoonManagedFactory@Moonlight@@SAPEAVBounceEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::BounceEase *")]
        public static extern BounceEase* CreateBounceEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateBrush@MoonManagedFactory@Moonlight@@SAPEAVBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Brush *")]
        public static extern Brush* CreateBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCacheMode@MoonManagedFactory@Moonlight@@SAPEAVCacheMode@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CacheMode *")]
        public static extern CacheMode* CreateCacheMode();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCanvas@MoonManagedFactory@Moonlight@@SAPEAVCanvas@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Canvas *")]
        public static extern Canvas* CreateCanvas();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureDevice@MoonManagedFactory@Moonlight@@SAPEAVCaptureDevice@2@H@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureDevice *")]
        public static extern CaptureDevice* CreateCaptureDevice(int object_type);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureDevice@MoonManagedFactory@Moonlight@@SAPEAVCaptureDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureDevice *")]
        public static extern CaptureDevice* CreateCaptureDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureFormatChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCaptureFormatChangedEventArgs@2@PEAUAudioFormat@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureFormatChangedEventArgs *")]
        public static extern CaptureFormatChangedEventArgs* CreateCaptureFormatChangedEventArgs([NativeTypeName("Moonlight::AudioFormat *")] AudioFormat* newFormat);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureFormatChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCaptureFormatChangedEventArgs@2@PEAUVideoFormat@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureFormatChangedEventArgs *")]
        public static extern CaptureFormatChangedEventArgs* CreateCaptureFormatChangedEventArgs([NativeTypeName("Moonlight::VideoFormat *")] VideoFormat* newFormat);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureImageCompletedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCaptureImageCompletedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureImageCompletedEventArgs *")]
        public static extern CaptureImageCompletedEventArgs* CreateCaptureImageCompletedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCaptureSource@MoonManagedFactory@Moonlight@@SAPEAVCaptureSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CaptureSource *")]
        public static extern CaptureSource* CreateCaptureSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCheckAndDownloadUpdateCompletedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCheckAndDownloadUpdateCompletedEventArgs@2@_NPEBD@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CheckAndDownloadUpdateCompletedEventArgs *")]
        public static extern CheckAndDownloadUpdateCompletedEventArgs* CreateCheckAndDownloadUpdateCompletedEventArgs([NativeTypeName("bool")] byte updated, [NativeTypeName("const char *")] sbyte* error);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCircleEase@MoonManagedFactory@Moonlight@@SAPEAVCircleEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CircleEase *")]
        public static extern CircleEase* CreateCircleEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateClock@MoonManagedFactory@Moonlight@@SAPEAVClock@2@PEAVTimeline@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Clock *")]
        public static extern Clock* CreateClock([NativeTypeName("Moonlight::Timeline *")] Timeline* timeline);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateClockGroup@MoonManagedFactory@Moonlight@@SAPEAVClockGroup@2@PEAVTimelineGroup@2@_N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ClockGroup *")]
        public static extern ClockGroup* CreateClockGroup([NativeTypeName("Moonlight::TimelineGroup *")] TimelineGroup* timeline, [NativeTypeName("bool")] byte timeManagerClockGroup);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCollectionChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCollectionChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CollectionChangedEventArgs *")]
        public static extern CollectionChangedEventArgs* CreateCollectionChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCollectionItemChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCollectionItemChangedEventArgs@2@PEAVDependencyObject@2@PEAVDependencyProperty@2@PEAUValue@2@2@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CollectionItemChangedEventArgs *")]
        public static extern CollectionItemChangedEventArgs* CreateCollectionItemChangedEventArgs([NativeTypeName("Moonlight::DependencyObject *")] DependencyObject* collectionItem, [NativeTypeName("Moonlight::DependencyProperty *")] DependencyProperty* property, [NativeTypeName("Moonlight::Value *")] Value* oldValue, [NativeTypeName("Moonlight::Value *")] Value* newValue);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCollectionItemChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVCollectionItemChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CollectionItemChangedEventArgs *")]
        public static extern CollectionItemChangedEventArgs* CreateCollectionItemChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorAnimation@MoonManagedFactory@Moonlight@@SAPEAVColorAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorAnimation *")]
        public static extern ColorAnimation* CreateColorAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorAnimationUsingKeyFrames@MoonManagedFactory@Moonlight@@SAPEAVColorAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorAnimationUsingKeyFrames *")]
        public static extern ColorAnimationUsingKeyFrames* CreateColorAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorKeyFrame *")]
        public static extern ColorKeyFrame* CreateColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColorKeyFrameCollection@MoonManagedFactory@Moonlight@@SAPEAVColorKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColorKeyFrameCollection *")]
        public static extern ColorKeyFrameCollection* CreateColorKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColumnDefinition@MoonManagedFactory@Moonlight@@SAPEAVColumnDefinition@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColumnDefinition *")]
        public static extern ColumnDefinition* CreateColumnDefinition();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateColumnDefinitionCollection@MoonManagedFactory@Moonlight@@SAPEAVColumnDefinitionCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ColumnDefinitionCollection *")]
        public static extern ColumnDefinitionCollection* CreateColumnDefinitionCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCompositeTransform@MoonManagedFactory@Moonlight@@SAPEAVCompositeTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CompositeTransform *")]
        public static extern CompositeTransform* CreateCompositeTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVContentChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentChangedEventArgs *")]
        public static extern ContentChangedEventArgs* CreateContentChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentControl@MoonManagedFactory@Moonlight@@SAPEAVContentControl@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentControl *")]
        public static extern ContentControl* CreateContentControl();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentControlChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVContentControlChangedEventArgs@2@PEAUValue@2@0@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentControlChangedEventArgs *")]
        public static extern ContentControlChangedEventArgs* CreateContentControlChangedEventArgs([NativeTypeName("Moonlight::Value *")] Value* old_content, [NativeTypeName("Moonlight::Value *")] Value* new_content);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateContentPresenter@MoonManagedFactory@Moonlight@@SAPEAVContentPresenter@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ContentPresenter *")]
        public static extern ContentPresenter* CreateContentPresenter();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateControl@MoonManagedFactory@Moonlight@@SAPEAVControl@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Control *")]
        public static extern Control* CreateControl();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateControlTemplate@MoonManagedFactory@Moonlight@@SAPEAVControlTemplate@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ControlTemplate *")]
        public static extern ControlTemplate* CreateControlTemplate();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateCubicEase@MoonManagedFactory@Moonlight@@SAPEAVCubicEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::CubicEase *")]
        public static extern CubicEase* CreateCubicEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDataTemplate@MoonManagedFactory@Moonlight@@SAPEAVDataTemplate@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DataTemplate *")]
        public static extern DataTemplate* CreateDataTemplate();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDeepZoomImageTileSource@MoonManagedFactory@Moonlight@@SAPEAVDeepZoomImageTileSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DeepZoomImageTileSource *")]
        public static extern DeepZoomImageTileSource* CreateDeepZoomImageTileSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObject@MoonManagedFactory@Moonlight@@SAPEAVDependencyObject@2@H@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObject *")]
        public static extern DependencyObject* CreateDependencyObject(int object_type);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObject@MoonManagedFactory@Moonlight@@SAPEAVDependencyObject@2@PEAVDeployment@2@H@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObject *")]
        public static extern DependencyObject* CreateDependencyObject([NativeTypeName("Moonlight::Deployment *")] Deployment* deployment, int object_type);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObject@MoonManagedFactory@Moonlight@@SAPEAVDependencyObject@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObject *")]
        public static extern DependencyObject* CreateDependencyObject();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDependencyObjectCollection@MoonManagedFactory@Moonlight@@SAPEAVDependencyObjectCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DependencyObjectCollection *")]
        public static extern DependencyObjectCollection* CreateDependencyObjectCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDeployment@MoonManagedFactory@Moonlight@@SAPEAVDeployment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Deployment *")]
        public static extern Deployment* CreateDeployment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscreteColorKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVDiscreteColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscreteColorKeyFrame *")]
        public static extern DiscreteColorKeyFrame* CreateDiscreteColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscreteDoubleKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVDiscreteDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscreteDoubleKeyFrame *")]
        public static extern DiscreteDoubleKeyFrame* CreateDiscreteDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscreteObjectKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVDiscreteObjectKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscreteObjectKeyFrame *")]
        public static extern DiscreteObjectKeyFrame* CreateDiscreteObjectKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDiscretePointKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVDiscretePointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DiscretePointKeyFrame *")]
        public static extern DiscretePointKeyFrame* CreateDiscretePointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDispatcherTimer@MoonManagedFactory@Moonlight@@SAPEAVDispatcherTimer@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DispatcherTimer *")]
        public static extern DispatcherTimer* CreateDispatcherTimer();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleAnimation@MoonManagedFactory@Moonlight@@SAPEAVDoubleAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleAnimation *")]
        public static extern DoubleAnimation* CreateDoubleAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleAnimationUsingKeyFrames@MoonManagedFactory@Moonlight@@SAPEAVDoubleAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleAnimationUsingKeyFrames *")]
        public static extern DoubleAnimationUsingKeyFrames* CreateDoubleAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleCollection@MoonManagedFactory@Moonlight@@SAPEAVDoubleCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleCollection *")]
        public static extern DoubleCollection* CreateDoubleCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleKeyFrame *")]
        public static extern DoubleKeyFrame* CreateDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDoubleKeyFrameCollection@MoonManagedFactory@Moonlight@@SAPEAVDoubleKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DoubleKeyFrameCollection *")]
        public static extern DoubleKeyFrameCollection* CreateDoubleKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDownloader@MoonManagedFactory@Moonlight@@SAPEAVDownloader@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Downloader *")]
        public static extern Downloader* CreateDownloader();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDownloadProgressEventArgs@MoonManagedFactory@Moonlight@@SAPEAVDownloadProgressEventArgs@2@N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DownloadProgressEventArgs *")]
        public static extern DownloadProgressEventArgs* CreateDownloadProgressEventArgs(double progress);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDownloadProgressEventArgs@MoonManagedFactory@Moonlight@@SAPEAVDownloadProgressEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DownloadProgressEventArgs *")]
        public static extern DownloadProgressEventArgs* CreateDownloadProgressEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDrawingAttributes@MoonManagedFactory@Moonlight@@SAPEAVDrawingAttributes@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DrawingAttributes *")]
        public static extern DrawingAttributes* CreateDrawingAttributes();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateDropShadowEffect@MoonManagedFactory@Moonlight@@SAPEAVDropShadowEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::DropShadowEffect *")]
        public static extern DropShadowEffect* CreateDropShadowEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingColorKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVEasingColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingColorKeyFrame *")]
        public static extern EasingColorKeyFrame* CreateEasingColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingDoubleKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVEasingDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingDoubleKeyFrame *")]
        public static extern EasingDoubleKeyFrame* CreateEasingDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingFunctionBase@MoonManagedFactory@Moonlight@@SAPEAVEasingFunctionBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingFunctionBase *")]
        public static extern EasingFunctionBase* CreateEasingFunctionBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEasingPointKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVEasingPointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EasingPointKeyFrame *")]
        public static extern EasingPointKeyFrame* CreateEasingPointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEffect@MoonManagedFactory@Moonlight@@SAPEAVEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Effect *")]
        public static extern Effect* CreateEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateElasticEase@MoonManagedFactory@Moonlight@@SAPEAVElasticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ElasticEase *")]
        public static extern ElasticEase* CreateElasticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEllipse@MoonManagedFactory@Moonlight@@SAPEAVEllipse@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Ellipse *")]
        public static extern Ellipse* CreateEllipse();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEllipseGeometry@MoonManagedFactory@Moonlight@@SAPEAVEllipseGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EllipseGeometry *")]
        public static extern EllipseGeometry* CreateEllipseGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEventArgs@MoonManagedFactory@Moonlight@@SAPEAVEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EventArgs *")]
        public static extern EventArgs* CreateEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateEventTrigger@MoonManagedFactory@Moonlight@@SAPEAVEventTrigger@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::EventTrigger *")]
        public static extern EventTrigger* CreateEventTrigger();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExponentialEase@MoonManagedFactory@Moonlight@@SAPEAVExponentialEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExponentialEase *")]
        public static extern ExponentialEase* CreateExponentialEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExtensionPart@MoonManagedFactory@Moonlight@@SAPEAVExtensionPart@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExtensionPart *")]
        public static extern ExtensionPart* CreateExtensionPart();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExternalPart@MoonManagedFactory@Moonlight@@SAPEAVExternalPart@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExternalPart *")]
        public static extern ExternalPart* CreateExternalPart();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateExternalPartCollection@MoonManagedFactory@Moonlight@@SAPEAVExternalPartCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ExternalPartCollection *")]
        public static extern ExternalPartCollection* CreateExternalPartCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateFrameworkElement@MoonManagedFactory@Moonlight@@SAPEAVFrameworkElement@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::FrameworkElement *")]
        public static extern FrameworkElement* CreateFrameworkElement();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateFrameworkTemplate@MoonManagedFactory@Moonlight@@SAPEAVFrameworkTemplate@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::FrameworkTemplate *")]
        public static extern FrameworkTemplate* CreateFrameworkTemplate();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeneralTransform@MoonManagedFactory@Moonlight@@SAPEAVGeneralTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GeneralTransform *")]
        public static extern GeneralTransform* CreateGeneralTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeometry@MoonManagedFactory@Moonlight@@SAPEAVGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Geometry *")]
        public static extern Geometry* CreateGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeometryCollection@MoonManagedFactory@Moonlight@@SAPEAVGeometryCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GeometryCollection *")]
        public static extern GeometryCollection* CreateGeometryCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGeometryGroup@MoonManagedFactory@Moonlight@@SAPEAVGeometryGroup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GeometryGroup *")]
        public static extern GeometryGroup* CreateGeometryGroup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGlyphs@MoonManagedFactory@Moonlight@@SAPEAVGlyphs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Glyphs *")]
        public static extern Glyphs* CreateGlyphs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGlyphTypefaceCollection@MoonManagedFactory@Moonlight@@SAPEAVGlyphTypefaceCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GlyphTypefaceCollection *")]
        public static extern GlyphTypefaceCollection* CreateGlyphTypefaceCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGradientBrush@MoonManagedFactory@Moonlight@@SAPEAVGradientBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GradientBrush *")]
        public static extern GradientBrush* CreateGradientBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGradientStop@MoonManagedFactory@Moonlight@@SAPEAVGradientStop@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GradientStop *")]
        public static extern GradientStop* CreateGradientStop();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGradientStopCollection@MoonManagedFactory@Moonlight@@SAPEAVGradientStopCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::GradientStopCollection *")]
        public static extern GradientStopCollection* CreateGradientStopCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateGrid@MoonManagedFactory@Moonlight@@SAPEAVGrid@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Grid *")]
        public static extern Grid* CreateGrid();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHitTestCollection@MoonManagedFactory@Moonlight@@SAPEAVHitTestCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HitTestCollection *")]
        public static extern HitTestCollection* CreateHitTestCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHttpRequestProgressChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVHttpRequestProgressChangedEventArgs@2@N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HttpRequestProgressChangedEventArgs *")]
        public static extern HttpRequestProgressChangedEventArgs* CreateHttpRequestProgressChangedEventArgs(double progress);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHttpRequestStoppedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVHttpRequestStoppedEventArgs@2@PEBD@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HttpRequestStoppedEventArgs *")]
        public static extern HttpRequestStoppedEventArgs* CreateHttpRequestStoppedEventArgs([NativeTypeName("const char *")] sbyte* error_msg);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHttpRequestWriteEventArgs@MoonManagedFactory@Moonlight@@SAPEAVHttpRequestWriteEventArgs@2@PEAX_JH@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::HttpRequestWriteEventArgs *")]
        public static extern HttpRequestWriteEventArgs* CreateHttpRequestWriteEventArgs(void* data, [NativeTypeName("gint64")] long offset, [NativeTypeName("gint32")] int count);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateHyperlink@MoonManagedFactory@Moonlight@@SAPEAVHyperlink@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Hyperlink *")]
        public static extern Hyperlink* CreateHyperlink();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateIcon@MoonManagedFactory@Moonlight@@SAPEAVIcon@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Icon *")]
        public static extern Icon* CreateIcon();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateIconCollection@MoonManagedFactory@Moonlight@@SAPEAVIconCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::IconCollection *")]
        public static extern IconCollection* CreateIconCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImage@MoonManagedFactory@Moonlight@@SAPEAVImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Image *")]
        public static extern Image* CreateImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImageBrush@MoonManagedFactory@Moonlight@@SAPEAVImageBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ImageBrush *")]
        public static extern ImageBrush* CreateImageBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImageSource@MoonManagedFactory@Moonlight@@SAPEAVImageSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ImageSource *")]
        public static extern ImageSource* CreateImageSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateIMediaObject@MoonManagedFactory@Moonlight@@SAPEAVIMediaObject@2@HPEAVMedia@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::IMediaObject *")]
        public static extern IMediaObject* CreateIMediaObject(int kind, [NativeTypeName("Moonlight::Media *")] Media* media);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateImplicitInputBrush@MoonManagedFactory@Moonlight@@SAPEAVImplicitInputBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ImplicitInputBrush *")]
        public static extern ImplicitInputBrush* CreateImplicitInputBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInkPresenter@MoonManagedFactory@Moonlight@@SAPEAVInkPresenter@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InkPresenter *")]
        public static extern InkPresenter* CreateInkPresenter();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInline@MoonManagedFactory@Moonlight@@SAPEAVInline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Inline *")]
        public static extern Inline* CreateInline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInlineCollection@MoonManagedFactory@Moonlight@@SAPEAVInlineCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InlineCollection *")]
        public static extern InlineCollection* CreateInlineCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInlineUIContainer@MoonManagedFactory@Moonlight@@SAPEAVInlineUIContainer@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InlineUIContainer *")]
        public static extern InlineUIContainer* CreateInlineUIContainer();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInputMethod@MoonManagedFactory@Moonlight@@SAPEAVInputMethod@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InputMethod *")]
        public static extern InputMethod* CreateInputMethod();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInputScope@MoonManagedFactory@Moonlight@@SAPEAVInputScope@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InputScope *")]
        public static extern InputScope* CreateInputScope();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInputScopeName@MoonManagedFactory@Moonlight@@SAPEAVInputScopeName@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InputScopeName *")]
        public static extern InputScopeName* CreateInputScopeName();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateInternalTransform@MoonManagedFactory@Moonlight@@SAPEAVInternalTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::InternalTransform *")]
        public static extern InternalTransform* CreateInternalTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateItalic@MoonManagedFactory@Moonlight@@SAPEAVItalic@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Italic *")]
        public static extern Italic* CreateItalic();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateItemCollection@MoonManagedFactory@Moonlight@@SAPEAVItemCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ItemCollection *")]
        public static extern ItemCollection* CreateItemCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateKeyEventArgs@MoonManagedFactory@Moonlight@@SAPEAVKeyEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::KeyEventArgs *")]
        public static extern KeyEventArgs* CreateKeyEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateKeyFrameCollection@MoonManagedFactory@Moonlight@@SAPEAVKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::KeyFrameCollection *")]
        public static extern KeyFrameCollection* CreateKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateKeySpline@MoonManagedFactory@Moonlight@@SAPEAVKeySpline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::KeySpline *")]
        public static extern KeySpline* CreateKeySpline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLine@MoonManagedFactory@Moonlight@@SAPEAVLine@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Line *")]
        public static extern Line* CreateLine();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearColorKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVLinearColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearColorKeyFrame *")]
        public static extern LinearColorKeyFrame* CreateLinearColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearDoubleKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVLinearDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearDoubleKeyFrame *")]
        public static extern LinearDoubleKeyFrame* CreateLinearDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearGradientBrush@MoonManagedFactory@Moonlight@@SAPEAVLinearGradientBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearGradientBrush *")]
        public static extern LinearGradientBrush* CreateLinearGradientBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLinearPointKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVLinearPointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LinearPointKeyFrame *")]
        public static extern LinearPointKeyFrame* CreateLinearPointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLineBreak@MoonManagedFactory@Moonlight@@SAPEAVLineBreak@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LineBreak *")]
        public static extern LineBreak* CreateLineBreak();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLineGeometry@MoonManagedFactory@Moonlight@@SAPEAVLineGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LineGeometry *")]
        public static extern LineGeometry* CreateLineGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLineSegment@MoonManagedFactory@Moonlight@@SAPEAVLineSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LineSegment *")]
        public static extern LineSegment* CreateLineSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLocalMessageReceiver@MoonManagedFactory@Moonlight@@SAPEAVLocalMessageReceiver@2@PEBDH@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LocalMessageReceiver *")]
        public static extern LocalMessageReceiver* CreateLocalMessageReceiver([NativeTypeName("const char *")] sbyte* receiverName, int namescope);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLocalMessageSender@MoonManagedFactory@Moonlight@@SAPEAVLocalMessageSender@2@PEBD0@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LocalMessageSender *")]
        public static extern LocalMessageSender* CreateLocalMessageSender([NativeTypeName("const char *")] sbyte* receiverName, [NativeTypeName("const char *")] sbyte* receiverDomain);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateLogReadyRoutedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVLogReadyRoutedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::LogReadyRoutedEventArgs *")]
        public static extern LogReadyRoutedEventArgs* CreateLogReadyRoutedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateManualTimeSource@MoonManagedFactory@Moonlight@@SAPEAVManualTimeSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ManualTimeSource *")]
        public static extern ManualTimeSource* CreateManualTimeSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMarkerStream@MoonManagedFactory@Moonlight@@SAPEAVMarkerStream@2@PEAVMedia@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MarkerStream *")]
        public static extern MarkerStream* CreateMarkerStream([NativeTypeName("Moonlight::Media *")] Media* media);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMatrix3DProjection@MoonManagedFactory@Moonlight@@SAPEAVMatrix3DProjection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Matrix3DProjection *")]
        public static extern Matrix3DProjection* CreateMatrix3DProjection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMatrixTransform@MoonManagedFactory@Moonlight@@SAPEAVMatrixTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MatrixTransform *")]
        public static extern MatrixTransform* CreateMatrixTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMediaBase@MoonManagedFactory@Moonlight@@SAPEAVMediaBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MediaBase *")]
        public static extern MediaBase* CreateMediaBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMediaElement@MoonManagedFactory@Moonlight@@SAPEAVMediaElement@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MediaElement *")]
        public static extern MediaElement* CreateMediaElement();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMediaPlayer@MoonManagedFactory@Moonlight@@SAPEAVMediaPlayer@2@PEAVMediaElement@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MediaPlayer *")]
        public static extern MediaPlayer* CreateMediaPlayer([NativeTypeName("Moonlight::MediaElement *")] MediaElement* element);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMessageReceivedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVMessageReceivedEventArgs@2@PEBDH00@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MessageReceivedEventArgs *")]
        public static extern MessageReceivedEventArgs* CreateMessageReceivedEventArgs([NativeTypeName("const char *")] sbyte* message, int namescope, [NativeTypeName("const char *")] sbyte* receiverName, [NativeTypeName("const char *")] sbyte* senderDomain);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMmsDemuxer@MoonManagedFactory@Moonlight@@SAPEAVMmsDemuxer@2@PEAVMedia@2@PEAVMmsSource@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MmsDemuxer *")]
        public static extern MmsDemuxer* CreateMmsDemuxer([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::MmsSource *")] MmsSource* source);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMmsPlaylistEntry@MoonManagedFactory@Moonlight@@SAPEAVMmsPlaylistEntry@2@PEAVMedia@2@PEAVMmsSource@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MmsPlaylistEntry *")]
        public static extern MmsPlaylistEntry* CreateMmsPlaylistEntry([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::MmsSource *")] MmsSource* source);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMouseButtonEventArgs@MoonManagedFactory@Moonlight@@SAPEAVMouseButtonEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MouseButtonEventArgs *")]
        public static extern MouseButtonEventArgs* CreateMouseButtonEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMouseEventArgs@MoonManagedFactory@Moonlight@@SAPEAVMouseEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MouseEventArgs *")]
        public static extern MouseEventArgs* CreateMouseEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMouseWheelEventArgs@MoonManagedFactory@Moonlight@@SAPEAVMouseWheelEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MouseWheelEventArgs *")]
        public static extern MouseWheelEventArgs* CreateMouseWheelEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleImage@MoonManagedFactory@Moonlight@@SAPEAVMultiScaleImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleImage *")]
        public static extern MultiScaleImage* CreateMultiScaleImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleSubImage@MoonManagedFactory@Moonlight@@SAPEAVMultiScaleSubImage@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleSubImage *")]
        public static extern MultiScaleSubImage* CreateMultiScaleSubImage();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleSubImageCollection@MoonManagedFactory@Moonlight@@SAPEAVMultiScaleSubImageCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleSubImageCollection *")]
        public static extern MultiScaleSubImageCollection* CreateMultiScaleSubImageCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateMultiScaleTileSource@MoonManagedFactory@Moonlight@@SAPEAVMultiScaleTileSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::MultiScaleTileSource *")]
        public static extern MultiScaleTileSource* CreateMultiScaleTileSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateNameScope@MoonManagedFactory@Moonlight@@SAPEAVNameScope@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::NameScope *")]
        public static extern NameScope* CreateNameScope();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateNotificationWindow@MoonManagedFactory@Moonlight@@SAPEAVNotificationWindow@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::NotificationWindow *")]
        public static extern NotificationWindow* CreateNotificationWindow();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateNullDecoder@MoonManagedFactory@Moonlight@@SAPEAVNullDecoder@2@PEAVMedia@2@PEAVIMediaStream@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::NullDecoder *")]
        public static extern NullDecoder* CreateNullDecoder([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::IMediaStream *")] IMediaStream* stream);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateObjectAnimationUsingKeyFrames@MoonManagedFactory@Moonlight@@SAPEAVObjectAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ObjectAnimationUsingKeyFrames *")]
        public static extern ObjectAnimationUsingKeyFrames* CreateObjectAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateObjectKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVObjectKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ObjectKeyFrame *")]
        public static extern ObjectKeyFrame* CreateObjectKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateObjectKeyFrameCollection@MoonManagedFactory@Moonlight@@SAPEAVObjectKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ObjectKeyFrameCollection *")]
        public static extern ObjectKeyFrameCollection* CreateObjectKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateOutOfBrowserSettings@MoonManagedFactory@Moonlight@@SAPEAVOutOfBrowserSettings@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::OutOfBrowserSettings *")]
        public static extern OutOfBrowserSettings* CreateOutOfBrowserSettings();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePanel@MoonManagedFactory@Moonlight@@SAPEAVPanel@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Panel *")]
        public static extern Panel* CreatePanel();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateParagraph@MoonManagedFactory@Moonlight@@SAPEAVParagraph@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Paragraph *")]
        public static extern Paragraph* CreateParagraph();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateParallelTimeline@MoonManagedFactory@Moonlight@@SAPEAVParallelTimeline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ParallelTimeline *")]
        public static extern ParallelTimeline* CreateParallelTimeline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateParserErrorEventArgs@MoonManagedFactory@Moonlight@@SAPEAVParserErrorEventArgs@2@PEAVDependencyObject@2@PEBD1HHH11@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ParserErrorEventArgs *")]
        public static extern ParserErrorEventArgs* CreateParserErrorEventArgs([NativeTypeName("Moonlight::DependencyObject *")] DependencyObject* original_source, [NativeTypeName("const char *")] sbyte* msg, [NativeTypeName("const char *")] sbyte* file, int line, int column, int error_code, [NativeTypeName("const char *")] sbyte* element, [NativeTypeName("const char *")] sbyte* attribute);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePassThroughDecoder@MoonManagedFactory@Moonlight@@SAPEAVPassThroughDecoder@2@PEAVMedia@2@PEAVIMediaStream@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PassThroughDecoder *")]
        public static extern PassThroughDecoder* CreatePassThroughDecoder([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::IMediaStream *")] IMediaStream* stream);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePasswordBox@MoonManagedFactory@Moonlight@@SAPEAVPasswordBox@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PasswordBox *")]
        public static extern PasswordBox* CreatePasswordBox();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePath@MoonManagedFactory@Moonlight@@SAPEAVPath@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Path *")]
        public static extern Path* CreatePath();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathFigure@MoonManagedFactory@Moonlight@@SAPEAVPathFigure@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathFigure *")]
        public static extern PathFigure* CreatePathFigure();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathFigureCollection@MoonManagedFactory@Moonlight@@SAPEAVPathFigureCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathFigureCollection *")]
        public static extern PathFigureCollection* CreatePathFigureCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathGeometry@MoonManagedFactory@Moonlight@@SAPEAVPathGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathGeometry *")]
        public static extern PathGeometry* CreatePathGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathSegment@MoonManagedFactory@Moonlight@@SAPEAVPathSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathSegment *")]
        public static extern PathSegment* CreatePathSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePathSegmentCollection@MoonManagedFactory@Moonlight@@SAPEAVPathSegmentCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PathSegmentCollection *")]
        public static extern PathSegmentCollection* CreatePathSegmentCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePixelShader@MoonManagedFactory@Moonlight@@SAPEAVPixelShader@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PixelShader *")]
        public static extern PixelShader* CreatePixelShader();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePlaneProjection@MoonManagedFactory@Moonlight@@SAPEAVPlaneProjection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PlaneProjection *")]
        public static extern PlaneProjection* CreatePlaneProjection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointAnimation@MoonManagedFactory@Moonlight@@SAPEAVPointAnimation@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointAnimation *")]
        public static extern PointAnimation* CreatePointAnimation();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointAnimationUsingKeyFrames@MoonManagedFactory@Moonlight@@SAPEAVPointAnimationUsingKeyFrames@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointAnimationUsingKeyFrames *")]
        public static extern PointAnimationUsingKeyFrames* CreatePointAnimationUsingKeyFrames();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointCollection@MoonManagedFactory@Moonlight@@SAPEAVPointCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointCollection *")]
        public static extern PointCollection* CreatePointCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVPointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointKeyFrame *")]
        public static extern PointKeyFrame* CreatePointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePointKeyFrameCollection@MoonManagedFactory@Moonlight@@SAPEAVPointKeyFrameCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PointKeyFrameCollection *")]
        public static extern PointKeyFrameCollection* CreatePointKeyFrameCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyBezierSegment@MoonManagedFactory@Moonlight@@SAPEAVPolyBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PolyBezierSegment *")]
        public static extern PolyBezierSegment* CreatePolyBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolygon@MoonManagedFactory@Moonlight@@SAPEAVPolygon@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Polygon *")]
        public static extern Polygon* CreatePolygon();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyline@MoonManagedFactory@Moonlight@@SAPEAVPolyline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Polyline *")]
        public static extern Polyline* CreatePolyline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyLineSegment@MoonManagedFactory@Moonlight@@SAPEAVPolyLineSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PolyLineSegment *")]
        public static extern PolyLineSegment* CreatePolyLineSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePolyQuadraticBezierSegment@MoonManagedFactory@Moonlight@@SAPEAVPolyQuadraticBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PolyQuadraticBezierSegment *")]
        public static extern PolyQuadraticBezierSegment* CreatePolyQuadraticBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePopup@MoonManagedFactory@Moonlight@@SAPEAVPopup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Popup *")]
        public static extern Popup* CreatePopup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePowerEase@MoonManagedFactory@Moonlight@@SAPEAVPowerEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PowerEase *")]
        public static extern PowerEase* CreatePowerEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePrintDocument@MoonManagedFactory@Moonlight@@SAPEAVPrintDocument@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PrintDocument *")]
        public static extern PrintDocument* CreatePrintDocument();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateProgressEventArgs@MoonManagedFactory@Moonlight@@SAPEAVProgressEventArgs@2@NN@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ProgressEventArgs *")]
        public static extern ProgressEventArgs* CreateProgressEventArgs(double progress, double offset);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateProjection@MoonManagedFactory@Moonlight@@SAPEAVProjection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Projection *")]
        public static extern Projection* CreateProjection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePropertyChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVPropertyChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PropertyChangedEventArgs *")]
        public static extern PropertyChangedEventArgs* CreatePropertyChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreatePropertyChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVPropertyChangedEventArgs@2@PEAVDependencyProperty@2@HPEAUValue@2@1_N@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::PropertyChangedEventArgs *")]
        public static extern PropertyChangedEventArgs* CreatePropertyChangedEventArgs([NativeTypeName("Moonlight::DependencyProperty *")] DependencyProperty* p, int pid, [NativeTypeName("Moonlight::Value *")] Value* ov, [NativeTypeName("Moonlight::Value *")] Value* nv, [NativeTypeName("bool")] byte copy_values);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuadraticBezierSegment@MoonManagedFactory@Moonlight@@SAPEAVQuadraticBezierSegment@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuadraticBezierSegment *")]
        public static extern QuadraticBezierSegment* CreateQuadraticBezierSegment();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuadraticEase@MoonManagedFactory@Moonlight@@SAPEAVQuadraticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuadraticEase *")]
        public static extern QuadraticEase* CreateQuadraticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuarticEase@MoonManagedFactory@Moonlight@@SAPEAVQuarticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuarticEase *")]
        public static extern QuarticEase* CreateQuarticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateQuinticEase@MoonManagedFactory@Moonlight@@SAPEAVQuinticEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::QuinticEase *")]
        public static extern QuinticEase* CreateQuinticEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRadialGradientBrush@MoonManagedFactory@Moonlight@@SAPEAVRadialGradientBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RadialGradientBrush *")]
        public static extern RadialGradientBrush* CreateRadialGradientBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRectangle@MoonManagedFactory@Moonlight@@SAPEAVRectangle@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Rectangle *")]
        public static extern Rectangle* CreateRectangle();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRectangleGeometry@MoonManagedFactory@Moonlight@@SAPEAVRectangleGeometry@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RectangleGeometry *")]
        public static extern RectangleGeometry* CreateRectangleGeometry();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRenderingEventArgs@MoonManagedFactory@Moonlight@@SAPEAVRenderingEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RenderingEventArgs *")]
        public static extern RenderingEventArgs* CreateRenderingEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateResourceDictionary@MoonManagedFactory@Moonlight@@SAPEAVResourceDictionary@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ResourceDictionary *")]
        public static extern ResourceDictionary* CreateResourceDictionary();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateResourceDictionaryChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVResourceDictionaryChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ResourceDictionaryChangedEventArgs *")]
        public static extern ResourceDictionaryChangedEventArgs* CreateResourceDictionaryChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateResourceDictionaryCollection@MoonManagedFactory@Moonlight@@SAPEAVResourceDictionaryCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ResourceDictionaryCollection *")]
        public static extern ResourceDictionaryCollection* CreateResourceDictionaryCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRichTextBox@MoonManagedFactory@Moonlight@@SAPEAVRichTextBox@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RichTextBox *")]
        public static extern RichTextBox* CreateRichTextBox();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRichTextBoxView@MoonManagedFactory@Moonlight@@SAPEAVRichTextBoxView@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RichTextBoxView *")]
        public static extern RichTextBoxView* CreateRichTextBoxView();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRotateTransform@MoonManagedFactory@Moonlight@@SAPEAVRotateTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RotateTransform *")]
        public static extern RotateTransform* CreateRotateTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRoutedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVRoutedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RoutedEventArgs *")]
        public static extern RoutedEventArgs* CreateRoutedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRowDefinition@MoonManagedFactory@Moonlight@@SAPEAVRowDefinition@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RowDefinition *")]
        public static extern RowDefinition* CreateRowDefinition();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRowDefinitionCollection@MoonManagedFactory@Moonlight@@SAPEAVRowDefinitionCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::RowDefinitionCollection *")]
        public static extern RowDefinitionCollection* CreateRowDefinitionCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateRun@MoonManagedFactory@Moonlight@@SAPEAVRun@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Run *")]
        public static extern Run* CreateRun();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSampleReadyEventArgs@MoonManagedFactory@Moonlight@@SAPEAVSampleReadyEventArgs@2@_J0PEAXHPEAUAudioFormat@2@PEAUVideoFormat@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SampleReadyEventArgs *")]
        public static extern SampleReadyEventArgs* CreateSampleReadyEventArgs([NativeTypeName("gint64")] long sampleTime, [NativeTypeName("gint64")] long frameDuration, void* sampleData, int sampleDataLength, [NativeTypeName("Moonlight::AudioFormat *")] AudioFormat* aformat, [NativeTypeName("Moonlight::VideoFormat *")] VideoFormat* vformat);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateScaleTransform@MoonManagedFactory@Moonlight@@SAPEAVScaleTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ScaleTransform *")]
        public static extern ScaleTransform* CreateScaleTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSection@MoonManagedFactory@Moonlight@@SAPEAVSection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Section *")]
        public static extern Section* CreateSection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSecuritySettings@MoonManagedFactory@Moonlight@@SAPEAVSecuritySettings@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SecuritySettings *")]
        public static extern SecuritySettings* CreateSecuritySettings();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSendCompletedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVSendCompletedEventArgs@2@PEAVMoonError@2@PEBD111PEAX@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SendCompletedEventArgs *")]
        public static extern SendCompletedEventArgs* CreateSendCompletedEventArgs([NativeTypeName("Moonlight::MoonError *")] MoonError* error, [NativeTypeName("const char *")] sbyte* message, [NativeTypeName("const char *")] sbyte* receiverName, [NativeTypeName("const char *")] sbyte* receiverDomain, [NativeTypeName("const char *")] sbyte* response, void* managedUserState);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSetter@MoonManagedFactory@Moonlight@@SAPEAVSetter@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Setter *")]
        public static extern Setter* CreateSetter();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSetterBase@MoonManagedFactory@Moonlight@@SAPEAVSetterBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SetterBase *")]
        public static extern SetterBase* CreateSetterBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSetterBaseCollection@MoonManagedFactory@Moonlight@@SAPEAVSetterBaseCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SetterBaseCollection *")]
        public static extern SetterBaseCollection* CreateSetterBaseCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateShaderEffect@MoonManagedFactory@Moonlight@@SAPEAVShaderEffect@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::ShaderEffect *")]
        public static extern ShaderEffect* CreateShaderEffect();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateShape@MoonManagedFactory@Moonlight@@SAPEAVShape@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Shape *")]
        public static extern Shape* CreateShape();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSineEase@MoonManagedFactory@Moonlight@@SAPEAVSineEase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SineEase *")]
        public static extern SineEase* CreateSineEase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSizeChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVSizeChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SizeChangedEventArgs *")]
        public static extern SizeChangedEventArgs* CreateSizeChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSkewTransform@MoonManagedFactory@Moonlight@@SAPEAVSkewTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SkewTransform *")]
        public static extern SkewTransform* CreateSkewTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSolidColorBrush@MoonManagedFactory@Moonlight@@SAPEAVSolidColorBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SolidColorBrush *")]
        public static extern SolidColorBrush* CreateSolidColorBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSpan@MoonManagedFactory@Moonlight@@SAPEAVSpan@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Span *")]
        public static extern Span* CreateSpan();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSplineColorKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVSplineColorKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SplineColorKeyFrame *")]
        public static extern SplineColorKeyFrame* CreateSplineColorKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSplineDoubleKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVSplineDoubleKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SplineDoubleKeyFrame *")]
        public static extern SplineDoubleKeyFrame* CreateSplineDoubleKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSplinePointKeyFrame@MoonManagedFactory@Moonlight@@SAPEAVSplinePointKeyFrame@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SplinePointKeyFrame *")]
        public static extern SplinePointKeyFrame* CreateSplinePointKeyFrame();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStoryboard@MoonManagedFactory@Moonlight@@SAPEAVStoryboard@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Storyboard *")]
        public static extern Storyboard* CreateStoryboard();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStroke@MoonManagedFactory@Moonlight@@SAPEAVStroke@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Stroke *")]
        public static extern Stroke* CreateStroke();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStrokeCollection@MoonManagedFactory@Moonlight@@SAPEAVStrokeCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::StrokeCollection *")]
        public static extern StrokeCollection* CreateStrokeCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStyle@MoonManagedFactory@Moonlight@@SAPEAVStyle@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Style *")]
        public static extern Style* CreateStyle();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStylusInfo@MoonManagedFactory@Moonlight@@SAPEAVStylusInfo@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::StylusInfo *")]
        public static extern StylusInfo* CreateStylusInfo();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateStylusPointCollection@MoonManagedFactory@Moonlight@@SAPEAVStylusPointCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::StylusPointCollection *")]
        public static extern StylusPointCollection* CreateStylusPointCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSystemTimeSource@MoonManagedFactory@Moonlight@@SAPEAVSystemTimeSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SystemTimeSource *")]
        public static extern SystemTimeSource* CreateSystemTimeSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateSystemTimeSource@MoonManagedFactory@Moonlight@@SAPEAVSystemTimeSource@2@PEAVDeployment@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::SystemTimeSource *")]
        public static extern SystemTimeSource* CreateSystemTimeSource([NativeTypeName("Moonlight::Deployment *")] Deployment* deployment);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBlock@MoonManagedFactory@Moonlight@@SAPEAVTextBlock@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBlock *")]
        public static extern TextBlock* CreateTextBlock();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBox@MoonManagedFactory@Moonlight@@SAPEAVTextBox@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBox *")]
        public static extern TextBox* CreateTextBox();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBoxModelChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVTextBoxModelChangedEventArgs@2@HPEAVPropertyChangedEventArgs@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBoxModelChangedEventArgs *")]
        public static extern TextBoxModelChangedEventArgs* CreateTextBoxModelChangedEventArgs(int changed, [NativeTypeName("Moonlight::PropertyChangedEventArgs *")] PropertyChangedEventArgs* property);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextBoxView@MoonManagedFactory@Moonlight@@SAPEAVTextBoxView@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextBoxView *")]
        public static extern TextBoxView* CreateTextBoxView();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextChangedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVTextChangedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextChangedEventArgs *")]
        public static extern TextChangedEventArgs* CreateTextChangedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextElement@MoonManagedFactory@Moonlight@@SAPEAVTextElement@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextElement *")]
        public static extern TextElement* CreateTextElement();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTextElementCollection@MoonManagedFactory@Moonlight@@SAPEAVTextElementCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TextElementCollection *")]
        public static extern TextElementCollection* CreateTextElementCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTileBrush@MoonManagedFactory@Moonlight@@SAPEAVTileBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TileBrush *")]
        public static extern TileBrush* CreateTileBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTileLayerInvalidatedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVTileLayerInvalidatedEventArgs@2@HHHH@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TileLayerInvalidatedEventArgs *")]
        public static extern TileLayerInvalidatedEventArgs* CreateTileLayerInvalidatedEventArgs(int level, int x, int y, int layer);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeline@MoonManagedFactory@Moonlight@@SAPEAVTimeline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Timeline *")]
        public static extern Timeline* CreateTimeline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineCollection@MoonManagedFactory@Moonlight@@SAPEAVTimelineCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineCollection *")]
        public static extern TimelineCollection* CreateTimelineCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineGroup@MoonManagedFactory@Moonlight@@SAPEAVTimelineGroup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineGroup *")]
        public static extern TimelineGroup* CreateTimelineGroup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineMarker@MoonManagedFactory@Moonlight@@SAPEAVTimelineMarker@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineMarker *")]
        public static extern TimelineMarker* CreateTimelineMarker();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineMarkerCollection@MoonManagedFactory@Moonlight@@SAPEAVTimelineMarkerCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineMarkerCollection *")]
        public static extern TimelineMarkerCollection* CreateTimelineMarkerCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimelineMarkerRoutedEventArgs@MoonManagedFactory@Moonlight@@SAPEAVTimelineMarkerRoutedEventArgs@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimelineMarkerRoutedEventArgs *")]
        public static extern TimelineMarkerRoutedEventArgs* CreateTimelineMarkerRoutedEventArgs();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeManager@MoonManagedFactory@Moonlight@@SAPEAVTimeManager@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimeManager *")]
        public static extern TimeManager* CreateTimeManager();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeSource@MoonManagedFactory@Moonlight@@SAPEAVTimeSource@2@PEAVDeployment@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimeSource *")]
        public static extern TimeSource* CreateTimeSource([NativeTypeName("Moonlight::Deployment *")] Deployment* deployment);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTimeSource@MoonManagedFactory@Moonlight@@SAPEAVTimeSource@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TimeSource *")]
        public static extern TimeSource* CreateTimeSource();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTouchDevice@MoonManagedFactory@Moonlight@@SAPEAVTouchDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TouchDevice *")]
        public static extern TouchDevice* CreateTouchDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTouchPoint@MoonManagedFactory@Moonlight@@SAPEAVTouchPoint@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TouchPoint *")]
        public static extern TouchPoint* CreateTouchPoint();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTouchPointCollection@MoonManagedFactory@Moonlight@@SAPEAVTouchPointCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TouchPointCollection *")]
        public static extern TouchPointCollection* CreateTouchPointCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTransform@MoonManagedFactory@Moonlight@@SAPEAVTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Transform *")]
        public static extern Transform* CreateTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTransformCollection@MoonManagedFactory@Moonlight@@SAPEAVTransformCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TransformCollection *")]
        public static extern TransformCollection* CreateTransformCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTransformGroup@MoonManagedFactory@Moonlight@@SAPEAVTransformGroup@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TransformGroup *")]
        public static extern TransformGroup* CreateTransformGroup();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTranslateTransform@MoonManagedFactory@Moonlight@@SAPEAVTranslateTransform@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TranslateTransform *")]
        public static extern TranslateTransform* CreateTranslateTransform();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerAction@MoonManagedFactory@Moonlight@@SAPEAVTriggerAction@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerAction *")]
        public static extern TriggerAction* CreateTriggerAction();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerActionCollection@MoonManagedFactory@Moonlight@@SAPEAVTriggerActionCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerActionCollection *")]
        public static extern TriggerActionCollection* CreateTriggerActionCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerBase@MoonManagedFactory@Moonlight@@SAPEAVTriggerBase@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerBase *")]
        public static extern TriggerBase* CreateTriggerBase();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateTriggerCollection@MoonManagedFactory@Moonlight@@SAPEAVTriggerCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::TriggerCollection *")]
        public static extern TriggerCollection* CreateTriggerCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUIElementCollection@MoonManagedFactory@Moonlight@@SAPEAVUIElementCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::UIElementCollection *")]
        public static extern UIElementCollection* CreateUIElementCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUnderline@MoonManagedFactory@Moonlight@@SAPEAVUnderline@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Underline *")]
        public static extern Underline* CreateUnderline();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUnmanagedStylusPoint@MoonManagedFactory@Moonlight@@SAPEAVUnmanagedStylusPoint@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::UnmanagedStylusPoint *")]
        public static extern UnmanagedStylusPoint* CreateUnmanagedStylusPoint();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateUserControl@MoonManagedFactory@Moonlight@@SAPEAVUserControl@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::UserControl *")]
        public static extern UserControl* CreateUserControl();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoBrush@MoonManagedFactory@Moonlight@@SAPEAVVideoBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoBrush *")]
        public static extern VideoBrush* CreateVideoBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoCaptureDevice@MoonManagedFactory@Moonlight@@SAPEAVVideoCaptureDevice@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoCaptureDevice *")]
        public static extern VideoCaptureDevice* CreateVideoCaptureDevice();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoCaptureDeviceCollection@MoonManagedFactory@Moonlight@@SAPEAVVideoCaptureDeviceCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoCaptureDeviceCollection *")]
        public static extern VideoCaptureDeviceCollection* CreateVideoCaptureDeviceCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoFormatCollection@MoonManagedFactory@Moonlight@@SAPEAVVideoFormatCollection@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoFormatCollection *")]
        public static extern VideoFormatCollection* CreateVideoFormatCollection();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVideoStream@MoonManagedFactory@Moonlight@@SAPEAVVideoStream@2@PEAVMedia@2@HII_KPEAXI@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VideoStream *")]
        public static extern VideoStream* CreateVideoStream([NativeTypeName("Moonlight::Media *")] Media* media, int codec_id, [NativeTypeName("guint32")] uint width, [NativeTypeName("guint32")] uint height, [NativeTypeName("guint64")] ulong duration, [NativeTypeName("gpointer")] void* extra_data, [NativeTypeName("guint32")] uint extra_data_size);

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateVisualBrush@MoonManagedFactory@Moonlight@@SAPEAVVisualBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::VisualBrush *")]
        public static extern VisualBrush* CreateVisualBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWebBrowser@MoonManagedFactory@Moonlight@@SAPEAVWebBrowser@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WebBrowser *")]
        public static extern WebBrowser* CreateWebBrowser();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWebBrowserBrush@MoonManagedFactory@Moonlight@@SAPEAVWebBrowserBrush@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WebBrowserBrush *")]
        public static extern WebBrowserBrush* CreateWebBrowserBrush();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWindow@MoonManagedFactory@Moonlight@@SAPEAVWindow@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::Window *")]
        public static extern Window* CreateWindow();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWindowSettings@MoonManagedFactory@Moonlight@@SAPEAVWindowSettings@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WindowSettings *")]
        public static extern WindowSettings* CreateWindowSettings();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateWriteableBitmap@MoonManagedFactory@Moonlight@@SAPEAVWriteableBitmap@2@XZ", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::WriteableBitmap *")]
        public static extern WriteableBitmap* CreateWriteableBitmap();

        [DllImport("", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?CreateYUVConverter@MoonManagedFactory@Moonlight@@SAPEAVYUVConverter@2@PEAVMedia@2@PEAVVideoStream@2@@Z", ExactSpelling = true)]
        [return: NativeTypeName("Moonlight::YUVConverter *")]
        public static extern YUVConverter* CreateYUVConverter([NativeTypeName("Moonlight::Media *")] Media* media, [NativeTypeName("Moonlight::VideoStream *")] VideoStream* stream);
    }
}
