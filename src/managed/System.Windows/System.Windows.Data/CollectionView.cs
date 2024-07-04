using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.Specialized;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using MS.Internal;

namespace System.Windows
{

    abstract class CollectionView : ICollectionView, INotifyPropertyChanged, IDeferRefresh
    {

        public static ICollectionView Create(IEnumerable collection)
        {
            if (collection is IList)
                return new ListCollectionView((IList)collection);
            return new EnumerableCollectionView(collection);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler CurrentChanged;
        public event CurrentChangingEventHandler CurrentChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        object currentItem;
        int currentPosition;
        INPCProperty<CultureInfo> culture;
        Predicate<object> filter;
        INPCProperty<bool> isCurrentAfterLast;
        INPCProperty<bool> isCurrentBeforeFirst;
        INPCProperty<bool> isempty;

        public bool CanFilter => true;

        public bool CanGroup => true;

        public bool CanSort => true;

        public CultureInfo Culture
        {
            get => culture.Value;
            set => culture.Value = value;
        }

        public object CurrentItem
        {
            get
            {
                ThrowIfDeferred();
                return currentItem;
            }
            protected set => currentItem = value;
        }

        public int CurrentPosition
        {
            get
            {
                ThrowIfDeferred();
                return currentPosition;
            }
            protected set => currentPosition = value;
        }

        protected int DeferLevel
        {
            get; set;
        }

        int IDeferRefresh.DeferLevel
        {
            get => DeferLevel;
            set => DeferLevel = value;
        }

        public Predicate<object> Filter
        {
            get => filter;
            set
            {
                filter = value;
                Refresh();
            }
        }

        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get; private set;
        }

        public ReadOnlyObservableCollection<object> Groups
        {
            get; protected set;
        }

        public bool IsCurrentAfterLast
        {
            get
            {
                ThrowIfDeferred();
                return isCurrentAfterLast.Value;
            }
            protected set
            {
                if (IsCurrentAfterLast != value)
                    isCurrentAfterLast.Value = value;
            }
        }

        public bool IsCurrentBeforeFirst
        {
            get
            {
                ThrowIfDeferred();
                return isCurrentBeforeFirst.Value;
            }
            protected set
            {
                if (IsCurrentBeforeFirst != value)
                    isCurrentBeforeFirst.Value = value;
            }
        }

        public bool IsEmpty
        {
            get => isempty.Value;
            set
            {
                if (IsEmpty != value)
                    isempty.Value = value;
            }
        }

        protected Func<PropertyChangedEventHandler> PropertyChangedFunc
        {
            get; set;
        }

        public SortDescriptionCollection SortDescriptions
        {
            get; private set;
        }

        public IEnumerable SourceCollection
        {
            get; private set;
        }

        protected CollectionView(IEnumerable collection)
        {
            PropertyChangedFunc = () => PropertyChanged;
            culture = INPCProperty.Create(() => Culture, PropertyChangedFunc);
            isCurrentAfterLast = INPCProperty.Create(() => IsCurrentAfterLast, PropertyChangedFunc);
            isCurrentBeforeFirst = INPCProperty.Create(() => IsCurrentBeforeFirst, PropertyChangedFunc);
            isempty = INPCProperty.Create(() => IsEmpty, PropertyChangedFunc);

            GroupDescriptions = new ObservableCollection<GroupDescription>();
            SortDescriptions = new SortDescriptionCollection();
            SourceCollection = collection;
        }

        protected virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);

            if (e.Action != NotifyCollectionChangedAction.Replace)
                RaisePropertyChanged("Count");
        }

        protected void RaiseCurrentChanged(EventArgs e)
        {
            CurrentChanged?.Invoke(this, e);
        }

        protected void RaiseCurrentChanging(CurrentChangingEventArgs e)
        {
            CurrentChanging?.Invoke(this, e);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ThrowIfDeferred()
        {
            if (DeferLevel != 0)
                throw new InvalidOperationException("Cannot access the CollectionView while refresh is deferred");
        }

        public abstract bool Contains(object item);
        public abstract IDisposable DeferRefresh();
        public abstract bool MoveCurrentTo(object item);
        public abstract bool MoveCurrentToFirst();
        public abstract bool MoveCurrentToLast();
        public abstract bool MoveCurrentToNext();
        public abstract bool MoveCurrentToPosition(int position);
        public abstract bool MoveCurrentToPrevious();
        public abstract void Refresh();
        public abstract IEnumerator GetEnumerator();
    }
}

