/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * textelement.h: 
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2010 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */

#ifndef __TEXTELEMENT_H__
#define __TEXTELEMENT_H__

#include <glib.h>
#include <cairo.h>

#include "frameworkelement.h"
#include "downloader.h"
#include "thickness.h"
#include "fontfamily.h"
#include "fontstretch.h"
#include "fontstyle.h"
#include "fontweight.h"
#include "fontsource.h"
#include "textlayout.h"
#include "enums.h"
#include "brush.h"
#include "fonts.h"
#include "textpointer.h"

#define TEXTBLOCK_FONT_FAMILY  "Portable User Interface"
#define TEXTBLOCK_FONT_STRETCH FontStretchesNormal
#define TEXTBLOCK_FONT_WEIGHT  FontWeightsNormal
#define TEXTBLOCK_FONT_STYLE   FontStylesNormal

namespace Moonlight {

class ITextLayoutContainer {
	virtual void DocumentPropertyChanged (TextElement *onElement, PropertyChangedEventArgs *args) = 0;
	virtual void DocumentCollectionChanged (TextElement *onElement, Collection *col, CollectionChangedEventArgs *args) = 0;
};

/* @Namespace=System.Windows.Documents */
class TextElement : public DependencyObject, public ITextAttributes, public IDocumentNode {
	TextFontDescription *font;
	GPtrArray *downloaders;
	
	void AddFontResource (const char *resource);
	void AddFontSource (Downloader *downloader);
	
	void CleanupDownloaders ();
	
	void DownloaderComplete (Downloader *downloader);
	
	static void downloader_complete (EventObject *sender, EventArgs *calldata, gpointer closure);

	bool UpdateFontDescription (bool force);
	
protected:
	/* @GeneratePInvoke,ManagedAccess=Internal */
	TextElement ();
	
	virtual ~TextElement ();
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

	void NotifyLayoutContainerOnPropertyChanged (PropertyChangedEventArgs *args);
	void NotifyLayoutContainerOnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);

	bool LocalValueOverrides (int prop_id);

	virtual void CopyValuesOnSplit (TextElement *into);

public:
	/* @PropertyType=FontFamily,DefaultValue=FontFamily(TEXTBLOCK_FONT_FAMILY),GenerateAccessors */
	const static int FontFamilyProperty;
	/* @PropertyType=double,AutoCreator=CreateDefaultFontSize,GenerateAccessors */
	const static int FontSizeProperty;
	/* @PropertyType=FontStretch,DefaultValue=FontStretch(TEXTBLOCK_FONT_STRETCH),GenerateAccessors */
	const static int FontStretchProperty;
	/* @PropertyType=FontStyle,DefaultValue=FontStyle(TEXTBLOCK_FONT_STYLE),GenerateAccessors */
	const static int FontStyleProperty;
	/* @PropertyType=FontWeight,DefaultValue=FontWeight(TEXTBLOCK_FONT_WEIGHT),GenerateAccessors */
	const static int FontWeightProperty;
	/* @PropertyType=Brush,AutoCreator=CreateBlackBrush,GenerateAccessors */
	const static int ForegroundProperty;
	/* @PropertyType=XmlLanguage,DefaultValue=\"en-US\",Validator=LanguageValidator,GenerateAccessors */
	const static int LanguageProperty;
	/* @PropertyType=TextDecorations,DefaultValue=TextDecorationsNone,HiddenDefaultValue,ManagedFieldAccess=Internal,GenerateManagedAccessors=false,ManagedPropertyType=TextDecorationCollection,GenerateAccessors,Coercer=TextElement::CoerceTextDecorations */
	const static int TextDecorationsProperty;
	/* @PropertyType=FontResource,GenerateManagedDP=false,GenerateManagedAccessors=false,GenerateAccessors */
	const static int FontResourceProperty;

	static bool CoerceTextDecorations (DependencyObject *obj, DependencyProperty *p, const Value *value, Value **coerced, MoonError *error);

	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);
	virtual void OnSubPropertyChanged (DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args);

	virtual IDocumentNode* GetParentDocumentNode ();
	virtual DependencyObject* AsDependencyObject () { return this; }
	virtual DependencyObjectCollection *GetDocumentChildren () { return NULL; }
	virtual DependencyObject *Split (int loc, TextElement *into) { return NULL; }
	virtual void SerializeText (GString *str) { }
	virtual void SerializeXaml (GString *str);
	virtual void SerializeXamlStartElement (GString* str) { }
	virtual void SerializeXamlEndElement (GString* str) { }
	virtual void SerializeXamlProperties (bool force, GString *str);

	//
	// ITextAttributes Interface Methods
	//
	virtual TextFontDescription *FontDescription () { return font; }
	virtual FlowDirection Direction () { return FlowDirectionLeftToRight; }
	virtual TextDecorations Decorations () { return GetTextDecorations (); }
	virtual Brush *Foreground (bool selected) { return GetForeground (); }
	virtual Brush *Background (bool selected) { return NULL; }

	/* @GeneratePInvoke */
	TextPointer *GetContentStart ();
	/* @GeneratePInvoke */
	TextPointer *GetContentEnd ();
	/* @GeneratePInvoke */
	TextPointer *GetElementStart ();
	/* @GeneratePInvoke */
	TextPointer *GetElementEnd ();

	TextPointer GetContentStart_np ();
	TextPointer GetContentEnd_np ();
	TextPointer GetElementStart_np ();
	TextPointer GetElementEnd_np ();
	
	//
	// Property Accessors
	//
	void SetFontFamily (FontFamily *family);
	FontFamily *GetFontFamily ();
	
	void SetFontSize (double size);
	double GetFontSize ();
	
	void SetFontStretch (FontStretch *stretch);
	FontStretch *GetFontStretch ();
	
	void SetFontStyle (FontStyle *style);
	FontStyle *GetFontStyle ();
	
	void SetFontWeight (FontWeight *weight);
	FontWeight *GetFontWeight ();
	
	void SetForeground (Brush *fg);
	Brush *GetForeground ();
	
	void SetLanguage (const char *language);
	const char *GetLanguage ();
	
	void SetTextDecorations (TextDecorations decorations);
	TextDecorations GetTextDecorations ();

	void SetFontResource (FontResource *resource);
	FontResource* GetFontResource ();
};


//
// TextElement subclasses used by TextBlock
//


/* @Namespace=System.Windows.Documents */
class Inline : public TextElement {
	bool autogen;
	
 protected:
	/* @GeneratePInvoke,ManagedAccess=Protected */
	Inline ();

	virtual ~Inline () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	// internal properties to inherit the FontSource between inlines and textblocks
	/* @PropertyType=FontSource,GenerateManagedDP=false,GenerateAccessors */
	const static int FontSourceProperty;
	
	virtual bool PermitsMultipleParents () { return false; }
	
	void SetFontSource (FontSource *source);
	FontSource *GetFontSource ();
	
	//
	// Non-DependencyProperty Accessors
	//
	void SetAutogenerated (bool autogen) { this->autogen = autogen; }
	bool GetAutogenerated () { return autogen; }
	
	//
	// Convenience Methods
	//
	virtual bool Equals (Inline *item);
};

/* @Namespace=System.Windows.Documents */
class LineBreak : public Inline {
 protected:
	/* @GeneratePInvoke */
	LineBreak ();

	virtual ~LineBreak () {}

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeText (GString *str);
	virtual void SerializeXaml (GString *str);
	virtual void SerializeXamlStartElement (GString *str);
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
};

/* @ContentProperty="Text" */
/* @Namespace=System.Windows.Documents */
class Run : public Inline {
 protected:
	/* @GeneratePInvoke */
	Run ();
	
	virtual ~Run () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

	virtual void CopyValuesOnSplit (TextElement *into);
	
 public:
	/* @PropertyType=FlowDirection,DefaultValue=FlowDirectionLeftToRight,GenerateAccessors */
	const static int FlowDirectionProperty;
	/* @PropertyType=string,ManagedFieldAccess=Internal,GenerateAccessors,Coercer=Run::CoerceText */
	const static int TextProperty;

	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);
	
	virtual bool Equals (Inline *item);
	virtual bool PermitsMultipleParents () { return false; }

	//
	// ITextAttributes Interface Method Overrides
	//
	virtual FlowDirection Direction () { return GetFlowDirection (); }

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeText (GString *str);
	virtual void SerializeXaml (GString *str);
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlProperties (bool force, GString *str);

	void SerializeXaml (GString *str, int start, int length = -1);
	void SerializeXamlProperties (bool force, GString *str, int start, int length = -1);

	virtual DependencyObject* Split (int loc, TextElement *into);

	//
	// Property Accessors
	//
	void SetFlowDirection (FlowDirection direction);
	FlowDirection GetFlowDirection ();
	
	void SetText (const char *text);
	const char *GetText ();

	static bool CoerceText (DependencyObject *obj, DependencyProperty *p, const Value *value, Value **coerced, MoonError *error);
};


//
// TextElement subclasses used by RichTextBox
//


/* @Namespace=System.Windows.Documents */
class Block : public TextElement {
 protected:
	/* @GeneratePInvoke,ManagedAccess=Protected */
	Block ();

	virtual ~Block () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	/* @PropertyType=TextAlignment,DefaultValue=TextAlignmentLeft,GenerateAccessors */
	const static int TextAlignmentProperty;

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeXamlProperties (bool force, GString *str);
	
	//
	// Property Accessors
	//
	void SetTextAlignment (TextAlignment alignment);
	TextAlignment GetTextAlignment ();
};

/* @Namespace=System.Windows.Documents */
/* @ContentProperty=Inlines */
class Paragraph : public Block {
 protected:
	/* @GeneratePInvoke */
	Paragraph ();
	
	virtual ~Paragraph () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	/* @PropertyType=InlineCollection,AutoCreateValue,GenerateAccessors,ManagedSetterAccess=Private,ManagedFieldAccess=Internal */
	const static int InlinesProperty;
	
	//
	// Property Accessors
	//
	void SetInlines (InlineCollection *inlines);
	InlineCollection *GetInlines ();

	virtual void OnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual DependencyObjectCollection *GetDocumentChildren () { return GetInlines(); }
	virtual DependencyObject *Split (int loc, TextElement *into);
	virtual void SerializeText (GString *str);
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlEndElement (GString *str);
};

/* @Namespace=System.Windows.Documents */
/* @ContentProperty=Blocks */
class Section : public Block {
 protected:
	/* @GeneratePInvoke,ManagedAccess=Internal */
	Section ();
	
	virtual ~Section () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

	virtual void CopyValuesOnSplit (TextElement *into);

 public:
	/* @PropertyType=BlockCollection,AutoCreateValue,GenerateAccessors,ManagedSetterAccess=Private,ManagedFieldAccess=Internal*/
	const static int BlocksProperty;
	/* @PropertyType=bool,DefaultValue=true,GenerateAccessors,ManagedFieldAccess=Internal */
	const static int HasTrailingParagraphBreakOnPasteProperty;
	
	//
	// Property Accessors
	//
	void SetBlocks (BlockCollection *value);
	BlockCollection *GetBlocks ();
	
	void SetHasTrailingParagraphBreakOnPaste (bool value);
	bool GetHasTrailingParagraphBreakOnPaste ();

	virtual void OnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual DependencyObjectCollection *GetDocumentChildren () { return GetBlocks(); }
	virtual DependencyObject* Split (int loc, TextElement *into);
	virtual void SerializeText (GString *str);
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlEndElement (GString *str);
};

/* @Namespace=System.Windows.Documents */
/* @ContentProperty=Inlines */
class Span : public Inline {
 protected:
	/* @GeneratePInvoke */
	Span ();

	virtual ~Span () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

 public:
	/* @PropertyType=InlineCollection,AutoCreator=Span::CreateInlineCollection,GenerateAccessors,ManagedFieldAccess=Internal,ReadOnly */
	const static int InlinesProperty;
	
	//
	// Property Accessors
	//
	void SetInlines (InlineCollection *inlines);
	InlineCollection *GetInlines ();

	virtual void OnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual DependencyObjectCollection *GetDocumentChildren () { return GetInlines(); }
	virtual DependencyObject* Split (int loc, TextElement *into);
	virtual void SerializeText (GString *str);

	static Value *CreateInlineCollection (Type::Kind kind, DependencyProperty *property, DependencyObject *forObj);
};

/* @Namespace=System.Windows.Documents */
class Bold : public Span {
 protected:
	/* @GeneratePInvoke */
	Bold ();

	virtual ~Bold () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlEndElement (GString *str);
};

/* @Namespace=System.Windows.Documents */
class Italic : public Span {
 protected:
	/* @GeneratePInvoke */
	Italic ();
	
	virtual ~Italic () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlEndElement (GString *str);
};

/* @Namespace=System.Windows.Documents */
class Underline : public Span {
 protected:
	/* @GeneratePInvoke */
	Underline ();

	virtual ~Underline () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlEndElement (GString *str);
};

/* @Namespace=System.Windows.Documents */
/* @CallInitialize */
class Hyperlink : public Span {
 protected:
	/* @GeneratePInvoke */
	Hyperlink ();
	
	virtual ~Hyperlink () {}

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	/* @PropertyType=object */
	const static int CommandParameterProperty;
	/* @PropertyType=object,ManagedPropertyType=ICommand */
	const static int CommandProperty;
	/* @PropertyType=Brush,AutoCreator=CreateBlackBrush,GenerateAccessors */
	const static int MouseOverForegroundProperty;
	/* @PropertyType=TextDecorations,DefaultValue=TextDecorationsUnderline,ManagedPropertyType=TextDecorationCollection */
	const static int MouseOverTextDecorationsProperty;
	/* @PropertyType=Uri,GenerateAccessors,IsConstPropertyType,DefaultValue=new Uri () */
	const static int NavigateUriProperty;
	/* @PropertyType=string,GenerateAccessors,DefaultValue=\"\" */
	const static int TargetNameProperty;

	//
	// Property Accessors
	//
	void SetMouseOverForeground (Brush *brush);
	Brush *GetMouseOverForeground ();

	void SetTargetName (const char *value);
	const char *GetTargetName ();

	void SetNavigateUri (const Uri *value);
	const Uri *GetNavigateUri ();

	/* @DelegateType=RoutedEventHandler */
	const static int ClickEvent;

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeXamlStartElement (GString *str);
	virtual void SerializeXamlEndElement (GString *str);
	virtual void SerializeXamlProperties (bool force, GString *str);
};

/* @Namespace=System.Windows.Documents */
/* @ContentProperty=Child */
class InlineUIContainer : public Inline {
 protected:
	/* @GeneratePInvoke */
	InlineUIContainer ();
	
	virtual ~InlineUIContainer () {}
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	/* @PropertyType=UIElement,GenerateAccessors,ManagedFieldAccess=Internal */
	const static int ChildProperty;
	
	//
	// Property Accessors
	//
	void SetChild (UIElement *child);
	UIElement *GetChild ();

	//
	// IDocumentNode Interface Method Overrides
	//
	virtual void SerializeXaml (GString *str);
	virtual void SerializeXamlStartElement (GString *str);

	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);
};

};
#endif /* __TEXTELEMENT_H__ */
