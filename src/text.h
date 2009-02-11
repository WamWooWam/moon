/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * text.h: 
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */

#ifndef __TEXT_H__
#define __TEXT_H__

#include <glib.h>
#include <cairo.h>

#include <frameworkelement.h>
#include <downloader.h>
#include <moon-path.h>
#include <thickness.h>
#include <layout.h>
#include <brush.h>
#include <font.h>
#include <fontfamily.h>

#define TEXTBLOCK_FONT_FAMILY  "Portable User Interface"
#define TEXTBLOCK_FONT_STRETCH FontStretchesNormal
#define TEXTBLOCK_FONT_WEIGHT  FontWeightsNormal
#define TEXTBLOCK_FONT_STYLE   FontStylesNormal
#define TEXTBLOCK_FONT_SIZE    14.666666984558105

/* @Namespace=System.Windows.Documents */
class Inline : public DependencyObject, public ITextSource {
	TextFontDescription *font;
	bool autogen;
	
 protected:
	virtual ~Inline ();
	
 public:
 	/* @PropertyType=FontFamily,DefaultValue=FontFamily(TEXTBLOCK_FONT_FAMILY),GenerateAccessors */
	const static int FontFamilyProperty;
 	/* @PropertyType=double,DefaultValue=TEXTBLOCK_FONT_SIZE,GenerateAccessors */
	const static int FontSizeProperty;
 	/* @PropertyType=FontStretch,DefaultValue=TEXTBLOCK_FONT_STRETCH,GenerateAccessors */
	const static int FontStretchProperty;
 	/* @PropertyType=FontStyle,DefaultValue=TEXTBLOCK_FONT_STYLE,GenerateAccessors */
	const static int FontStyleProperty;
 	/* @PropertyType=FontWeight,DefaultValue=TEXTBLOCK_FONT_WEIGHT,GenerateAccessors */
	const static int FontWeightProperty;
 	/* @PropertyType=Brush,DefaultValue=new SolidColorBrush("black"),GenerateAccessors */
	const static int ForegroundProperty;
 	/* @PropertyType=TextDecorations,DefaultValue=TextDecorationsNone,ManagedPropertyType=TextDecorationCollection,GenerateAccessors */
	const static int TextDecorationsProperty;
	/* @PropertyType=string,DefaultValue=\"en-US\",Version=2.0,ManagedPropertyType=XmlLanguage,Validator=NonNullStringValidator */
	const static int LanguageProperty;
	
	// internal property to inherit the font filename between inlines and textblocks
 	/* @PropertyType=string,GenerateManagedDP=false,GenerateAccessors */
	const static int FontFilenameProperty;
	
	/* @GenerateCBinding,GeneratePInvoke,ManagedAccess=Protected */
	Inline ();
	
	virtual void OnSubPropertyChanged (DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args);
	
	//
	// Property Accessors
	//
	void SetFontFamily (FontFamily *value);
	FontFamily *GetFontFamily ();
	
	void SetFontFilename (const char *value);
	const char *GetFontFilename ();
	
	void SetFontSize (double value);
	double GetFontSize ();
	
	void SetFontStretch (FontStretches value);
	FontStretches GetFontStretch ();
	
	void SetFontStyle (FontStyles value);
	FontStyles GetFontStyle ();
	
	void SetFontWeight (FontWeights value);
	FontWeights GetFontWeight ();
	
	void SetForeground (Brush *value);
	Brush *GetForeground ();
	
	void SetTextDecorations (TextDecorations decorations);
	TextDecorations GetTextDecorations ();
	
	//
	// Non-DependencyProperty Accessors
	//
	void SetAutogenerated (bool autogen) { this->autogen = autogen; }
	bool GetAutogenerated () { return autogen; }
	
	//
	// ITextSource Interface Methods
	//
	virtual TextDecorations Decorations () { return GetTextDecorations (); }
	virtual TextFontDescription *FontDescription () { return font; }
	virtual Brush *Foreground (bool selected) { return GetForeground (); }
	virtual Brush *Background (bool selected) { return NULL; }
	
	//
	// Convenience Methods
	//
	void UpdateFontDescription ();
	
	virtual bool Equals (Inline *item);
};


/* @Namespace=System.Windows.Documents */
class LineBreak : public Inline {
 protected:
	virtual ~LineBreak () {}
	
 public:
	/* @GenerateCBinding,GeneratePInvoke */
	LineBreak () { SetObjectType (Type::LINEBREAK); }
};


/* @ContentProperty="Text" */
/* @Namespace=System.Windows.Documents */
class Run : public Inline {
 protected:
	virtual ~Run () {}
	
 public:
 	/* @PropertyType=string,ManagedFieldAccess=Internal,GenerateAccessors */
	const static int TextProperty;
	
	/* @GenerateCBinding,GeneratePInvoke */
	Run () { SetObjectType (Type::RUN); }
	
	virtual bool Equals (Inline *item);
	
	//
	// Property Accessors
	//
	void SetText (const char *text);
	const char *GetText ();
};


class TextBlockDynamicPropertyValueProvider;

/* @ContentProperty="Inlines" */
/* @Namespace=System.Windows.Controls */
class TextBlock : public FrameworkElement {
	friend class TextBlockDynamicPropertyValueProvider;
	
	TextFontDescription *font;
	Downloader *downloader;
	TextLayout *layout;
	
	Size constraint;
	double actual_height;
	double actual_width;
	bool setvalue;
	bool dirty;
	
	void CalcActualWidthHeight (cairo_t *cr);
	void Layout (cairo_t *cr);
	void Paint (cairo_t *cr);
	
	char *GetTextInternal (InlineCollection *inlines);
	bool SetTextInternal (const char *text);
	
	void DownloaderComplete ();
	
	static void downloader_complete (EventObject *sender, EventArgs *calldata, gpointer closure);
	
 protected:
	virtual ~TextBlock ();
	
 public:
 	/* @PropertyType=FontFamily,DefaultValue=FontFamily(TEXTBLOCK_FONT_FAMILY),GenerateAccessors */
	const static int FontFamilyProperty;
 	/* @PropertyType=double,DefaultValue=TEXTBLOCK_FONT_SIZE,GenerateAccessors */
	const static int FontSizeProperty;
 	/* @PropertyType=FontStretch,DefaultValue=TEXTBLOCK_FONT_STRETCH,GenerateAccessors */
	const static int FontStretchProperty;
 	/* @PropertyType=FontStyle,DefaultValue=TEXTBLOCK_FONT_STYLE,GenerateAccessors */
	const static int FontStyleProperty;
 	/* @PropertyType=FontWeight,DefaultValue=TEXTBLOCK_FONT_WEIGHT,GenerateAccessors */
	const static int FontWeightProperty;
 	/* @PropertyType=Brush,GenerateAccessors */
	const static int ForegroundProperty;
 	/* @PropertyType=InlineCollection,AutoCreateValue,ManagedFieldAccess=Internal,ManagedSetterAccess=Internal,GenerateAccessors */
	const static int InlinesProperty;
 	/* @PropertyType=string,DefaultValue=\"\",GenerateAccessors */
	const static int TextProperty;
	/* @PropertyType=double,DefaultValue=NAN,Version=2.0,GenerateAccessors */
	const static int LineHeightProperty;
	/* @PropertyType=LineStackingStrategy,DefaultValue=LineStackingStrategyMaxHeight,Version=2.0,GenerateAccessors */
	const static int LineStackingStrategyProperty;
	/* @PropertyType=Thickness,DefaultValue=Thickness (0),Version=2.0,GenerateAccessors */
	const static int PaddingProperty;
	/* @PropertyType=TextAlignment,DefaultValue=TextAlignmentLeft,Version=2.0,GenerateAccessors */
	const static int TextAlignmentProperty;
 	/* @PropertyType=TextDecorations,DefaultValue=TextDecorationsNone,ManagedPropertyType=TextDecorationCollection,GenerateAccessors */
	const static int TextDecorationsProperty;
 	/* @PropertyType=TextWrapping,DefaultValue=TextWrappingNoWrap,GenerateAccessors */
	const static int TextWrappingProperty;
	
	// internal property to inherit the font filename between inlines and textblocks
 	/* @PropertyType=string,GenerateManagedDP=false,GenerateAccessors */
	const static int FontFilenameProperty;
	
	/* @GenerateCBinding,GeneratePInvoke */
	TextBlock ();
	
	void SetFontSource (Downloader *downloader);
	
	//
	// Overrides
	//
	virtual void Render (cairo_t *cr, Region *region, bool path_only = false);
	virtual Size MeasureOverride (Size availableSize);
	virtual Size ArrangeOverride (Size finalSize);
	virtual void ComputeBounds ();
	virtual Point GetTransformOrigin ();
	virtual void GetSizeForBrush (cairo_t *cr, double *width, double *height);
	virtual void OnPropertyChanged (PropertyChangedEventArgs *args);
	virtual void OnSubPropertyChanged (DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args);
	virtual void OnCollectionItemChanged (Collection *col, DependencyObject *obj, PropertyChangedEventArgs *args);
	virtual void OnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);
	
	void SetFontFamily (FontFamily *family);
	FontFamily *GetFontFamily ();
	
	void SetFontSize (double size);
	double GetFontSize ();
	
	void SetFontStretch (FontStretches stretch);
	FontStretches GetFontStretch ();
	
	void SetFontStyle (FontStyles style);
	FontStyles GetFontStyle ();
	
	void SetFontWeight (FontWeights weight);
	FontWeights GetFontWeight ();
	
	void SetForeground (Brush *fg);
	Brush *GetForeground ();
	
	void SetInlines (InlineCollection *inlines);
	InlineCollection *GetInlines ();
	
	void SetLineHeight (double height);
	double GetLineHeight ();
	
	void SetLineStackingStrategy (LineStackingStrategy strategy);
	LineStackingStrategy GetLineStackingStrategy ();
	
	void SetPadding (Thickness *padding);
	Thickness *GetPadding ();
	
	void SetText (const char *text);
	const char *GetText ();
	
	void SetTextAlignment (TextAlignment alignment);
	TextAlignment GetTextAlignment ();
	
	void SetTextDecorations (TextDecorations decorations);
	TextDecorations GetTextDecorations ();
	
	void SetTextWrapping (TextWrapping wrapping);
	TextWrapping GetTextWrapping ();
	
	const char *GetFontFilename ();
	void SetFontFilename (const char *value);
};


/* @Namespace=System.Windows.Documents */
class Glyphs : public FrameworkElement {
	TextFontDescription *desc;
	Downloader *downloader;
	
	moon_path *path;
	gunichar *text;
	List *attrs;
	Brush *fill;
	int index;
	
	double origin_x;
	double origin_y;
	double height;
	double width;
	double left;
	double top;
	
	int origin_y_specified:1;
	int simulation_none:1;
	int uri_changed:1;
	int invalid:1;
	int dirty:1;
	
	void Layout ();
	void SetIndicesInternal (const char *in);
	
	void DownloaderComplete ();
	
	static void downloader_complete (EventObject *sender, EventArgs *calldata, gpointer closure);
	
	void DownloadFont (Surface *surface, const char *url);
	
 protected:
	virtual ~Glyphs ();
	
 public:
 	/* @PropertyType=Brush,GenerateAccessors */
	const static int FillProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors */
	const static int FontRenderingEmSizeProperty;
 	/* @PropertyType=string,ManagedPropertyType=Uri,GenerateAccessors,Validator=NonNullStringValidator */
	const static int FontUriProperty;
 	/* @PropertyType=string,GenerateAccessors */
	const static int IndicesProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors */
	const static int OriginXProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors */
	const static int OriginYProperty;
 	/* @PropertyType=StyleSimulations,DefaultValue=StyleSimulationsNone,GenerateAccessors */
	const static int StyleSimulationsProperty;
 	/* @PropertyType=string,GenerateAccessors */
	const static int UnicodeStringProperty;
	
	/* @GenerateCBinding,GeneratePInvoke */
	Glyphs ();
	
	virtual void GetSizeForBrush (cairo_t *cr, double *width, double *height);
	virtual void Render (cairo_t *cr, Region *region, bool path_only = false);
	virtual void ComputeBounds ();
	virtual bool InsideObject (cairo_t *cr, double x, double y);
	virtual Point GetTransformOrigin ();
	virtual Point GetOriginPoint ();
	virtual void SetSurface (Surface *surface);
	virtual void OnPropertyChanged (PropertyChangedEventArgs *args);
	virtual void OnSubPropertyChanged (DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args);
	
	//
	// Property Accessors
	//
	void SetFill (Brush *fill);
	Brush *GetFill ();
	
	void SetFontRenderingEmSize (double size);
	double GetFontRenderingEmSize ();
	
	void SetFontUri (const char *uri);
	const char *GetFontUri ();
	
	void SetIndices (const char *indices);
	const char *GetIndices ();
	
	void SetOriginX (double origin);
	double GetOriginX ();
	
	void SetOriginY (double origin);
	double GetOriginY ();
	
	void SetStyleSimulations (StyleSimulations style);
	StyleSimulations GetStyleSimulations ();
	
	void SetUnicodeString (const char *unicode);
	const char *GetUnicodeString ();
};

/* @Namespace=System.Windows.Input */
class InputMethod : public DependencyObject {
	
 protected:
	virtual ~InputMethod () {}
	
 public:
	/* @PropertyType=bool,Attached */
	const static int IsInputMethodEnabledProperty;
	
 	/* @ManagedAccess=Internal,GeneratePInvoke,GenerateCBinding */
	InputMethod () { SetObjectType (Type::INPUTMETHOD); }
};

#endif /* __TEXT_H__ */
