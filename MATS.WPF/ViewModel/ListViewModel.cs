using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MATS.Core;
using MATS.Core.Enumerations;
using MATS.Core.Extensions;
//using MATS.Core.Extensions;
using MATS.Core.Models;
using MATS.Repository.Interfaces;

namespace MATS.WPF.ViewModel
{
    public class ListViewModel : ViewModelBase
    {
        #region Fields
        private IUnitOfWork _unitOfWork;
        private ListTypes _listType;
        private ObservableCollection<ListDTO> _filteredLists;
        private ListDTO _selectedList;
        private ICommand _saveListViewCommand, _addNewListViewCommand, _deleteListViewCommand, _closeListViewCommand;
        #endregion

        #region Constructor
        public ListViewModel(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            SelectedList = new ListDTO();
            FilteredLists = new ObservableCollection<ListDTO>();

            Messenger.Default.Register<ListTypes>(this, (message) =>
            {
                ListType = message;
            });
        }
        #endregion

        #region Public Properties
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
            set
            {
                _unitOfWork = value;
                RaisePropertyChanged<IUnitOfWork>(() => UnitOfWork);
            }
        }
        public ListTypes ListType
        {
            get { return _listType; }
            set
            {
                _listType = value;
                RaisePropertyChanged<ListTypes>(() => ListType);
                HeaderText = "List ";//of " + new EnumerationExtension(ListTypes.City.GetType()).GetDescription(ListType);
                GetLiveLists();

                //if (HeaderText.ToLower().Contains("amh"))
                //    InputLanguage = "am-ET";
                //else
                //    InputLanguage = "en-US";
            }
        }

        private string _inputLanguage;
        public string InputLanguage
        {
            get { return _inputLanguage; }
            set
            {
                _inputLanguage = value;
                RaisePropertyChanged<string>(() => this.InputLanguage);
            }
        }

        private string _headerText;
        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                RaisePropertyChanged<string>(() => this.HeaderText);
            }
        }

        public ListDTO SelectedList
        {
            get { return _selectedList; }
            set
            {
                _selectedList = value;
                RaisePropertyChanged<ListDTO>(() => SelectedList);
            }
        }
        public ObservableCollection<ListDTO> FilteredLists
        {
            get { return _filteredLists; }
            set
            {
                _filteredLists = value;
                RaisePropertyChanged<ObservableCollection<ListDTO>>(() => FilteredLists);

                if (FilteredLists.Any())
                {
                    SelectedList = FilteredLists.FirstOrDefault();
                }
                else
                {
                    SelectedList = new ListDTO
                    {
                        Type = ListType
                    };
                }
            }
        }
        #endregion

        #region Commands
        public ICommand AddNewListViewCommand
        {
            get { return _addNewListViewCommand ?? (_addNewListViewCommand = new RelayCommand(ExecuteAddNewListViewCommand)); }
        }
        private void ExecuteAddNewListViewCommand()
        {
            SelectedList = new ListDTO
            {
                Type = ListType
            };
        }

        public ICommand SaveListViewCommand
        {
            get { return _saveListViewCommand ?? (_saveListViewCommand = new RelayCommand<Object>(ExecuteSaveListViewCommand, CanSave)); }
        }
        private void ExecuteSaveListViewCommand(object obj)
        {
            SaveList();
            GetLiveLists();
        }
        public bool SaveList()
        {
            try
            {
                if (SelectedList.Id == 0)
                {
                    _unitOfWork.Repository<ListDTO>().Insert(SelectedList);
                }
                else
                {
                    _unitOfWork.Repository<ListDTO>().Update(SelectedList);
                }
                _unitOfWork.Commit();                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ICommand DeleteListViewCommand
        {
            get { return _deleteListViewCommand ?? (_deleteListViewCommand = new RelayCommand<Object>(ExecuteDeleteListViewCommand, CanSave)); }
        }
        private void ExecuteDeleteListViewCommand(object obj)
        {
            try
            {
                if (MessageBox.Show("Are you Sure You want to Delete this List item?", "Delete list item", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _unitOfWork.Repository<ListDTO>().Delete(SelectedList);
                    _unitOfWork.Commit();
                    GetLiveLists();
                }
            }
            catch { }
        }

        public ICommand CloseListViewCommand
        {
            get { return _closeListViewCommand ?? (_closeListViewCommand = new RelayCommand<Object>(ExecuteCloseListViewCommand, CanSave)); }
        }
        private void ExecuteCloseListViewCommand(object obj)
        {
            try
            {
                if (SaveList())
                    CloseWindow(obj);
            }
            catch
            { }
        }
        private void CloseWindow(object obj)
        {
            if (obj == null) return;
            var window = obj as Window;
            if (window == null) return;
            window.DialogResult = true;
            window.Close();
        }

        #endregion

        private void GetLiveLists()
        {
            try
            {
                var lists = _unitOfWork.Repository<ListDTO>().GetAll()
                    .Where(l => l.Type == ListType)
                    .OrderBy(l => l.DisplayName);
                FilteredLists = new ObservableCollection<ListDTO>(lists);
            }
            catch
            {
                FilteredLists = new ObservableCollection<ListDTO>();
            }
        }

        #region Validation
        public static int Errors { get; set; }
        public bool CanSave(object obj)
        {
            if (Errors == 0)
                return true;
            return false;
        }
        #endregion

    }
}
