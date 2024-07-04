/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */
/*
 * transform.cpp:
 *
 * Contact:
 *   Moonlight List (moonlight-list@lists.ximian.com)
 *
 * Copyright 2007 Novell, Inc. (http://www.novell.com)
 *
 * See the LICENSE file included with the distribution for details.
 */

#include <config.h>

#include <math.h>

#include "transform.h"
#include "factory.h"

namespace Moonlight {


    //
    // Composite Transform
    //

    CompositeTransform::CompositeTransform() {
        SetObjectType(Type::COMPOSITETRANSFORM);
    }

    void
    CompositeTransform::UpdateTransform() {
        double sx = GetScaleX();
        double sy = GetScaleY();

        // XXX you don't want to know.  don't make these 0.00001, or
        // else cairo spits out errors about non-invertable matrices
        // (or worse, crashes)
        //
        // the 0.0 scales are caused in at least one instance by us
        // being too aggressive at starting animations at time=0 when
        // they're supposed to (unset, or 0:0:0 BeginTime)
        //
        if (sx == 0.0)
            sx = 0.00002;
        if (sy == 0.0)
            sy = 0.00002;

        double cx = GetCenterX();
        double cy = GetCenterY();

        cairo_matrix_t _matrix;

        cairo_matrix_init_translate(&_matrix, cx, cy);
        cairo_matrix_translate(&_matrix, GetTranslateX(), GetTranslateY());

        double radians = GetRotation() / 180 * M_PI;
        cairo_matrix_rotate(&_matrix, radians);

        cairo_matrix_t skew;
        cairo_matrix_init_identity(&skew);

        double ax = GetSkewX();
        if (ax != 0.0)
            skew.xy = tan(ax * M_PI / 180);

        double ay = GetSkewY();
        if (ay != 0.0)
            skew.yx = tan(ay * M_PI / 180);
        cairo_matrix_multiply(&_matrix, &skew, &_matrix);

        cairo_matrix_scale(&_matrix, sx, sy);

        cairo_matrix_translate(&_matrix, -cx, -cy);

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);
    }


    //
    // GeneralTransform
    //

    void
    GeneralTransform::OnPropertyChanged(PropertyChangedEventArgs *args, MoonError *error) {
        if (args->GetProperty()->GetOwnerType() == Type::DEPENDENCY_OBJECT) {
            DependencyObject::OnPropertyChanged(args, error);
            return;
        }

        need_update = true;
        //
        // If the transform changes, we need to notify our owners
        // that they must repaint (all of our properties have
        // a visible effect.
        //
        // There is no need to override this on the base classes
        // as they are sealed, so no new properties can be added
        // and I do not believe that we can create new instances
        // of transform from C#, and in that case, we would only
        // be slower.
        //
        NotifyListenersOfPropertyChange(args, error);
    }

    void
    GeneralTransform::UpdateTransform() {
        g_warning("GeneralTransform::UpdateTransform has been called. The derived class should have overridden it.");
    }

    void
    GeneralTransform::MaybeUpdateTransform() {
        if (need_update) {
            UpdateTransform();
            need_update = false;
        }
    }

    void
    GeneralTransform::GetTransform(cairo_matrix_t *value) {
        cairo_matrix_t _matrix;

        MaybeUpdateTransform();

#define M(row, col) _m44[col * 4 + row]
        _matrix.xx = M(0, 0);
        _matrix.yx = M(1, 0);
        _matrix.xy = M(0, 1);
        _matrix.yy = M(1, 1);
        _matrix.x0 = M(0, 3);
        _matrix.y0 = M(1, 3);
#undef M

        *value = _matrix;
    }

    Matrix *
    GeneralTransform::GetMatrix() {
        cairo_matrix_t _matrix;

        GetTransform(&_matrix);
        return new Matrix(&_matrix);
    }

    Point
    GeneralTransform::Transform(Point point) {
        double p[4] = { point.x, point.y, 0.0, 1.0 };

        MaybeUpdateTransform();
        Matrix3D::TransformPoint(p, _m44, p);

        if (p[3] != 0.0) {
            double w = 1.0 / p[3];

            return Point(p[0] * w, p[1] * w);
        }

        return Point(NAN, NAN);
    }

    void
    GeneralTransform::TransformPoint(GeneralTransform *t, Point *p, Point *r) {
        *r = t->Transform(*p);
    }

    Point
    GeneralTransform::TransformXY(double x, double y) {
        return Transform(Point(x, y));
    }


    //
    // InternalTransform
    //
    GeneralTransform *
    InternalTransform::GetInverse() {
        InternalTransform *transform;
        double inverse[16];

        Matrix3D::Inverse(inverse, _m44);

        transform = MoonUnmanagedFactory::CreateInternalTransform();
        Matrix3D::Init(transform->_m44, inverse);

        return transform;
    }

    Matrix3D *
    InternalTransform::GetMatrix3D() {
        Matrix3D *matrix = new Matrix3D(_m44);
        return matrix;
    }

    bool
    InternalTransform::TryTransform(Point inPoint, Point *outPoint) {
        double p[4] = { inPoint.x, inPoint.y, 0.0, 1.0 };

        Matrix3D::TransformPoint(p, _m44, p);

        *outPoint = Point(p[0], p[1]);

        return true;
    }

    //
    // Transform
    //

    GeneralTransform *
    Transform::GetInverse() {
        MatrixTransform *transform;
        cairo_matrix_t inverted;
        Matrix *matrix;

        GetTransform(&inverted);

        if (cairo_matrix_invert(&inverted) != CAIRO_STATUS_SUCCESS)
            return NULL;

        matrix = new Matrix(&inverted);
        transform = MoonUnmanagedFactory::CreateMatrixTransform();
        ((MatrixTransform *)transform)->SetMatrix(matrix);
        matrix->unref();

        return transform;
    }

    bool
    Transform::TryTransform(Point inPoint, Point *outPoint) {
        double p[4] = { inPoint.x, inPoint.y, 0.0, 1.0 };

        MaybeUpdateTransform();
        Matrix3D::TransformPoint(p, _m44, p);

        *outPoint = Point(p[0], p[1]);

        return true;
    }

    void
    InternalTransform::UpdateTransform() {
        // do nothing
    }


    //
    // RotateTransform
    //

    void
    RotateTransform::UpdateTransform() {
        double angle, center_x, center_y;
        double radians;

        cairo_matrix_t _matrix;

        angle = GetAngle();
        center_x = GetCenterX();
        center_y = GetCenterY();

        radians = angle / 180.0 * M_PI;

        if (center_x == 0.0 && center_y == 0.0) {
            cairo_matrix_init_rotate(&_matrix, radians);
        }
        else {
            cairo_matrix_init_translate(&_matrix, center_x, center_y);
            cairo_matrix_rotate(&_matrix, radians);
            cairo_matrix_translate(&_matrix, -center_x, -center_y);
        }

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);

        // printf ("Returning2 %g %g %g %g %g %g\n", value->xx, value->yx, value->xy, value->yy, value->x0, value->y0);
    }


    //
    // TranslateTransform
    //

    void
    TranslateTransform::UpdateTransform() {
        double x = GetX();
        double y = GetY();

        cairo_matrix_t _matrix;

        cairo_matrix_init_translate(&_matrix, x, y);

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);

        // printf ("translating dx %g dy %g", x, y);
        // printf ("TranslateTransform %g %g %g %g %g %g\n", value->xx, value->yx, value->xy, value->yy, value->x0, value->y0);
    }

    //
    // ScaleTransform
    //
    void
    ScaleTransform::UpdateTransform() {
        double sx = GetScaleX();
        double sy = GetScaleY();

        // XXX you don't want to know.  don't make these 0.00001, or
        // else cairo spits out errors about non-invertable matrices
        // (or worse, crashes)
        //
        // the 0.0 scales are caused in at least one instance by us
        // being too aggressive at starting animations at time=0 when
        // they're supposed to (unset, or 0:0:0 BeginTime)
        //
        if (sx == 0.0)
            sx = 0.00002;
        if (sy == 0.0)
            sy = 0.00002;

        double cx = GetCenterX();
        double cy = GetCenterY();

        cairo_matrix_t _matrix;

        if (cx == 0.0 && cy == 0.0) {
            cairo_matrix_init_scale(&_matrix, sx, sy);
        }
        else {
            cairo_matrix_init_translate(&_matrix, cx, cy);
            cairo_matrix_scale(&_matrix, sx, sy);
            cairo_matrix_translate(&_matrix, -cx, -cy);
        }

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);

        // printf ("scaling sx %g sy %g at center cx %g cy %g\n", sx, sy, cx, cy);
        // printf ("ScaleTransform %g %g %g %g %g %g\n", value->xx, value->yx, value->xy, value->yy, value->x0, value->y0);
    }

    //
    // SkewTransform
    //

    void
    SkewTransform::UpdateTransform() {
        double cx = GetCenterX();
        double cy = GetCenterY();
        cairo_matrix_t _matrix;

        bool translation = ((cx != 0.0) || (cy != 0.0));
        if (translation)
            cairo_matrix_init_translate(&_matrix, cx, cy);
        else
            cairo_matrix_init_identity(&_matrix);

        double ax = GetAngleX();
        if (ax != 0.0)
            _matrix.xy = tan(ax * M_PI / 180);

        double ay = GetAngleY();
        if (ay != 0.0)
            _matrix.yx = tan(ay * M_PI / 180);

        if (translation)
            cairo_matrix_translate(&_matrix, -cx, -cy);

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);

        // printf ("SkewTransform %g %g %g %g %g %g\n", value->xx, value->yx, value->xy, value->yy, value->x0, value->y0);
    }

    //
    // Matrix
    //

    Matrix::Matrix() {
        SetObjectType(Type::MATRIX);
        cairo_matrix_init_identity(&matrix);
    }

    Matrix::Matrix(cairo_matrix_t *m) {
        SetObjectType(Type::MATRIX);
        matrix = *m;
        SetM11(matrix.xx);
        SetM12(matrix.yx);
        SetM21(matrix.xy);
        SetM22(matrix.yy);
        SetOffsetX(matrix.x0);
        SetOffsetY(matrix.y0);
    }

    void
    Matrix::OnPropertyChanged(PropertyChangedEventArgs *args, MoonError *error) {
        if (args->GetProperty()->GetOwnerType() != Type::MATRIX) {
            DependencyObject::OnPropertyChanged(args, error);
            return;
        }

        if (args->GetId() == Matrix::M11Property)
            matrix.xx = args->GetNewValue()->AsDouble();
        else if (args->GetId() == Matrix::M12Property)
            matrix.yx = args->GetNewValue()->AsDouble();
        else if (args->GetId() == Matrix::M21Property)
            matrix.xy = args->GetNewValue()->AsDouble();
        else if (args->GetId() == Matrix::M22Property)
            matrix.yy = args->GetNewValue()->AsDouble();
        else if (args->GetId() == Matrix::OffsetXProperty)
            matrix.x0 = args->GetNewValue()->AsDouble();
        else if (args->GetId() == Matrix::OffsetYProperty)
            matrix.y0 = args->GetNewValue()->AsDouble();

        NotifyListenersOfPropertyChange(args, error);
    }

    cairo_matrix_t
    Matrix::GetUnderlyingMatrix() {
        return matrix;
    }

    //
    // MatrixTransform
    //

    void
    MatrixTransform::OnSubPropertyChanged(DependencyProperty *prop, DependencyObject *obj, PropertyChangedEventArgs *subobj_args) {
        need_update = true;

        DependencyObject::OnSubPropertyChanged(prop, obj, subobj_args);

        NotifyListenersOfPropertyChange(MatrixTransform::MatrixProperty, NULL);
    }

    void
    MatrixTransform::UpdateTransform() {
        cairo_matrix_t _matrix;
        Matrix *matrix = GetMatrix();
        if (matrix)
            _matrix = matrix->GetUnderlyingMatrix();
        else
            cairo_matrix_init_identity(&_matrix);

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);
    }

    //
    // TransformGroup
    //

    void
    TransformGroup::OnPropertyChanged(PropertyChangedEventArgs *args, MoonError *error) {
        if (args->GetProperty()->GetOwnerType() != Type::TRANSFORMGROUP) {
            Transform::OnPropertyChanged(args, error);
            return;
        }

        if (args->GetId() == TransformGroup::ChildrenProperty) {
            need_update = true;
        }

        NotifyListenersOfPropertyChange(args, error);
    }

    void
    TransformGroup::OnCollectionChanged(Collection *col, CollectionChangedEventArgs *args) {
        if (!PropertyHasValueNoAutoCreate(TransformGroup::ChildrenProperty, col)) {
            Transform::OnCollectionChanged(col, args);
            return;
        }

        need_update = true;
        NotifyListenersOfPropertyChange(TransformGroup::ChildrenProperty, NULL);
    }

    void
    TransformGroup::OnCollectionItemChanged(Collection *col, DependencyObject *obj, PropertyChangedEventArgs *args) {
        if (!PropertyHasValueNoAutoCreate(TransformGroup::ChildrenProperty, col)) {
            Transform::OnCollectionItemChanged(col, obj, args);
            return;
        }

        // Unit tests shows that the "cache" matrix value (exposed in SL2) is not updated when child items are changed.
        // However SL2 does re-compute this value (if dirty) before drawing anything that depends on it.
        // Currently Moonlight behave differently by always returning the "up to date" matrix
        need_update = true;
        NotifyListenersOfPropertyChange(TransformGroup::ChildrenProperty, NULL);
    }

    void
    TransformGroup::UpdateTransform() {
        TransformCollection *children = GetChildren();
        cairo_matrix_t _matrix;

        cairo_matrix_init_identity(&_matrix);

        int count = children->GetCount();
        for (int i = 0; i < count; i++) {
            Transform *transform = children->GetValueAt(i)->AsTransform();
            cairo_matrix_t matrix;

            transform->GetTransform(&matrix);
            cairo_matrix_multiply(&_matrix, &_matrix, &matrix);
        }

        Matrix3D::Affine(_m44,
            _matrix.xx, _matrix.xy,
            _matrix.yx, _matrix.yy,
            _matrix.x0, _matrix.y0);
    }

};
