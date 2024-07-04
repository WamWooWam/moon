/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * grid.h
 *
 * Copyright 2008 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 * 
 */

#ifndef __MOON_GRID_H__
#define __MOON_GRID_H__

#include <glib.h>
#include "panel.h"

namespace Moonlight {

/* @IncludeInKinds */
/* @Namespace=System.Windows */
struct GridLength {
 public:
	double val;
	GridUnitType type;
	
	GridLength () {
		val = 0;
		type = GridUnitTypeAuto;
	}
	
	GridLength (double v, GridUnitType t)
	{
		val = v;
		type = t;
	}
	
	bool operator == (const GridLength &v) const
	{
		return (fabs (val - v.val) < DBL_EPSILON && type == v.type);
	}
	
	bool operator != (const GridLength &point) const
	{
		return !(*this == point);
	}
};

/* @Namespace=System.Windows.Controls */
class ColumnDefinition : public DependencyObject {
 protected:
	/* @GeneratePInvoke */
	ColumnDefinition ();
	
	virtual ~ColumnDefinition ();

	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
 	/* @PropertyType=double,DefaultValue=INFINITY,GenerateAccessors */
	const static int MaxWidthProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors */
	const static int MinWidthProperty;
 	/* @PropertyType=GridLength,DefaultValue=GridLength (1.0\, GridUnitTypeStar),GenerateAccessors */
	const static int WidthProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors,ManagedSetterAccess=Private,ManagedFieldAccess=Private */
	const static int ActualWidthProperty;
	
	// property accessors
	double GetActualWidth ();
	void SetActualWidth (double value);

	double GetMaxWidth();
	void SetMaxWidth (double value);

	double GetMinWidth();
	void SetMinWidth (double value);

	GridLength* GetWidth();
	void SetWidth (GridLength *value);
};

/* @Namespace=System.Windows.Controls */
class RowDefinition : public DependencyObject {
 protected:
	/* @GeneratePInvoke */
	RowDefinition ();

	virtual ~RowDefinition ();
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

 public:
 	/* @PropertyType=GridLength,DefaultValue=GridLength (1.0\, GridUnitTypeStar),GenerateAccessors */
	const static int HeightProperty;
 	/* @PropertyType=double,DefaultValue=INFINITY,GenerateAccessors */
	const static int MaxHeightProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors */
	const static int MinHeightProperty;
 	/* @PropertyType=double,DefaultValue=0.0,GenerateAccessors,ManagedSetterAccess=Private,ManagedFieldAccess=Private */
	const static int ActualHeightProperty;

	// property accessors
	double GetActualHeight ();
	void SetActualHeight (double value);

	double GetMaxHeight();
	void SetMaxHeight (double value);

	double GetMinHeight();
	void SetMinHeight (double value);

	GridLength* GetHeight();
	void SetHeight (GridLength *value);
};

/* @Namespace=System.Windows.Controls */
class ColumnDefinitionCollection : public DependencyObjectCollection {
 protected:
	/* @GeneratePInvoke,ManagedAccess=Internal */
	ColumnDefinitionCollection ();

	virtual ~ColumnDefinitionCollection ();
	
	virtual bool AddedToCollection (Value *value, MoonError *error);
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	virtual Type::Kind GetElementType () { return Type::COLUMNDEFINITION; }
};


/* @Namespace=System.Windows.Controls */
class RowDefinitionCollection : public DependencyObjectCollection {
 protected:
	/* @GeneratePInvoke,ManagedAccess=Internal */
	RowDefinitionCollection ();
	
	virtual ~RowDefinitionCollection ();
	
	virtual bool AddedToCollection (Value *value, MoonError *error);
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;
	
 public:
	virtual Type::Kind GetElementType () { return Type::ROWDEFINITION; }
};

struct Segment {
	double original_size;
	double max;
	double min;
	double desired_size;
	double offered_size;
	double stars;
	GridUnitType type;

	Segment ();
	Segment (double offered_size, double min, double max, GridUnitType type);

 private:
	void Init (double offered_size, double min, double max, GridUnitType type);
};

/* @Namespace=System.Windows.Controls */
class Grid : public Panel {
	int row_matrix_dim;
	int col_matrix_dim;
	Segment **row_matrix;
	Segment **col_matrix;
	
	RowDefinitionCollection* GetRowDefinitionsNoAutoCreate ();
	ColumnDefinitionCollection* GetColumnDefinitionsNoAutoCreate ();

	void AllocateDesiredSize (int row_count, int col_count);
	void AssignSize (Segment **matrix, int start, int end, double *size, GridUnitType type, bool desired_size);
	void CreateMatrices (int row_count, int col_count);
	void DestroyMatrices ();
	void ExpandStarRows (Size availableSize);
	void ExpandStarCols (Size availableSize);

	void SaveMeasureResults ();
	void RestoreMeasureResults ();

 protected:
	/* @GeneratePInvoke */
	Grid ();
	
	virtual ~Grid ();

	virtual void PostRender (Context *ctx, Region *region, bool skip_children);
	
	friend class MoonUnmanagedFactory;
	friend class MoonManagedFactory;

 public:
 	/* @PropertyType=gint32,DefaultValue=0,Attached,GenerateAccessors,Validator=PositiveIntValidator */
	const static int ColumnProperty;
	/* @PropertyType=ColumnDefinitionCollection,AutoCreateValue,ManagedFieldAccess=Internal,ManagedSetterAccess=Internal,GenerateAccessors */
	const static int ColumnDefinitionsProperty;
 	/* @PropertyType=gint32,DefaultValue=1,Attached,GenerateAccessors,Validator=IntGreaterThanZeroValidator */
	const static int ColumnSpanProperty;
 	/* @PropertyType=gint32,DefaultValue=0,Attached,GenerateAccessors,Validator=PositiveIntValidator */
	const static int RowProperty;
	/* @PropertyType=RowDefinitionCollection,AutoCreateValue,ManagedFieldAccess=Internal,ManagedSetterAccess=Internal,GenerateAccessors */
	const static int RowDefinitionsProperty;
 	/* @PropertyType=gint32,DefaultValue=1,Attached,GenerateAccessors,Validator=IntGreaterThanZeroValidator */
	const static int RowSpanProperty;

	/* 
	 * NOTE: The ShowGridLines property defaults to false but appears
	 * to be uninitialized before InitializeComponents is called
	 * causing the current moon-unit test to fail.
	 */
 	/* @PropertyType=bool,DefaultValue=false,GenerateAccessors */
	const static int ShowGridLinesProperty;
	
	virtual void OnPropertyChanged (PropertyChangedEventArgs *args, MoonError *error);
	virtual void OnCollectionChanged (Collection *col, CollectionChangedEventArgs *args);
	virtual void OnCollectionItemChanged (Collection *col, DependencyObject *obj, PropertyChangedEventArgs *args);
	virtual void ComputeBounds ();

	virtual Size MeasureOverrideWithError (Size availableSize, MoonError *error);
	virtual Size ArrangeOverrideWithError (Size finalSize, MoonError *error);

	// property accessors
	ColumnDefinitionCollection *GetColumnDefinitions ();
	void SetColumnDefinitions (ColumnDefinitionCollection* value);

	static gint32 GetColumn (DependencyObject *obj);
	static void SetColumn (DependencyObject *obj, gint32 value);

	static gint32 GetColumnSpan (DependencyObject *obj);
	static void SetColumnSpan (DependencyObject *obj, gint32 value);

	RowDefinitionCollection *GetRowDefinitions ();
	void SetRowDefinitions (RowDefinitionCollection* value);

	static gint32 GetRow (DependencyObject *obj);
	static void SetRow (DependencyObject *obj, gint32 value);

	static gint32 GetRowSpan (DependencyObject *obj);
	static void SetRowSpan (DependencyObject *obj, gint32 value);

	bool GetShowGridLines ();
	void SetShowGridLines (bool value);
	
	static double Clamp (double val, double min, double max);
};

// We need this class to figure out what kinds of elements the grid
// contains before the grid starts measuring them.
class GridWalker {
 public:
	bool HasAutoAuto () { return has_auto_auto; }
	bool HasStarAuto () { return has_star_auto; }
	bool HasAutoStar () { return has_auto_star; }
	GridWalker (Grid *grid, Segment **row_matrix, int row_count, Segment **col_matrix, int col_count);

 private:
	bool has_auto_auto;
	bool has_star_auto;
	bool has_auto_star;
};

class GridNode : public List::Node {
public:
	int row;
	int col;
	double size;
	Segment ** matrix;

	GridNode (Segment **matrix, int row, int col, double size) {
		this->matrix = matrix;
		this->row = row;
		this->col = col;
		this->size = size;
	}
};

};
#endif /* __MOON_PANEL_H__ */
