﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using BlueMonkey.Model;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Xamarin.Forms;

namespace BlueMonkey.ViewModels
{
    /// <summary>
    /// View Model for ReceiptPage
    /// </summary>
    public class ReceiptPageViewModel : BindableBase, IDestructible
    {
        /// <summary>
        /// IEditExpense use case model.
        /// </summary>
        private readonly IEditExpense _editExpense;

        /// <summary>
        /// Resource disposer.
        /// </summary>
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        /// <summary>
        /// Receipt image for Expense.
        /// </summary>
        public ReadOnlyReactiveProperty<ImageSource> Receipt { get; }

        /// <summary>
        /// PickPhotoAsyncCommand.
        /// </summary>
        public AsyncReactiveCommand PickPhotoAsyncCommand { get; }

        /// <summary>
        /// TakePhotoAsyncCommand
        /// </summary>
        public AsyncReactiveCommand TakePhotoAsyncCommand { get; }

        /// <summary>
        /// Initialize Instance.
        /// </summary>
        /// <param name="editExpense"></param>
        public ReceiptPageViewModel(IEditExpense editExpense)
        {
            _editExpense = editExpense;

            Receipt = _editExpense.ObserveProperty(x => x.Receipt)
                .Where(x => x != null)
                .Select(x => ImageSource.FromStream(x.GetStream))
                .ToReadOnlyReactiveProperty().AddTo(Disposable);

            PickPhotoAsyncCommand = new AsyncReactiveCommand().AddTo(Disposable);
            PickPhotoAsyncCommand.Subscribe(async _ => await _editExpense.PickPhotoAsync());

            TakePhotoAsyncCommand = new AsyncReactiveCommand().AddTo(Disposable);
            TakePhotoAsyncCommand.Subscribe(async _ => await _editExpense.TakePhotoAsync());
        }

        /// <summary>
        /// Free resources.
        /// </summary>
        public void Destroy()
        {
            Disposable.Dispose();
        }
    }
}
