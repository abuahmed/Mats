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
using MATS.Core.Models;
using MATS.Repository.Interfaces;
using System.IO;
using System.Windows.Forms;

namespace MATS.WPF.ViewModel
{
    public class AttachmentViewModel : ViewModelBase
    {
        #region Fields
        private IUnitOfWork _unitOfWork;
        private ObservableCollection<AttachmentDTO> _filteredAttachments;
        private AttachmentDTO _selectedAttachment;
        private ICommand _downloadAttachmentViewCommand, _uploadAttachmentViewCommand, _deleteAttachmentViewCommand, _closeAttachmentViewCommand;
        private TicketDTO _listType;
        #endregion

        #region Constructor
        public AttachmentViewModel(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            SelectedAttachment = new AttachmentDTO();
            FilteredAttachments = new ObservableCollection<AttachmentDTO>();

            Messenger.Default.Register<TicketDTO>(this, (message) =>
            {
                Ticket = message;
            });

            if (Singleton.Edition == MATSEdition.CompactEdition)
            {
                UploadVisibility = "Collapsed";
                DownloadVisibility = "Visible";
            }
            else
            {
                UploadVisibility = "Visible";
                DownloadVisibility = "Collapsed";
            }
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
        public TicketDTO Ticket
        {
            get { return _listType; }
            set
            {
                _listType = value;
                RaisePropertyChanged<TicketDTO>(() => Ticket);
                if (Ticket != null)
                {
                    GetLiveAttachments();
                }

            }
        }

        private string _fileLocation;
        private string _uploadVisibility;
        private string _downloadVisibility;

        public string FileLocation
        {
            get { return _fileLocation; }
            set
            {
                _fileLocation = value;
                RaisePropertyChanged<string>(() => FileLocation);
            }
        }
        public string UploadVisibility
        {
            get { return _uploadVisibility; }
            set
            {
                _uploadVisibility = value;
                RaisePropertyChanged<string>(() => UploadVisibility);
            }
        }
        public string DownloadVisibility
        {
            get { return _downloadVisibility; }
            set
            {
                _downloadVisibility = value;
                RaisePropertyChanged<string>(() => DownloadVisibility);
            }
        }

        public AttachmentDTO SelectedAttachment
        {
            get { return _selectedAttachment; }
            set
            {
                _selectedAttachment = value;
                RaisePropertyChanged<AttachmentDTO>(() => SelectedAttachment);
                if (SelectedAttachment != null)
                {

                }
            }
        }
        public ObservableCollection<AttachmentDTO> FilteredAttachments
        {
            get { return _filteredAttachments; }
            set
            {
                _filteredAttachments = value;
                RaisePropertyChanged<ObservableCollection<AttachmentDTO>>(() => FilteredAttachments);

                if (FilteredAttachments.Any())
                {
                    SelectedAttachment = FilteredAttachments.FirstOrDefault();
                }
                else
                {
                    SelectedAttachment = new AttachmentDTO
                    {

                    };
                }
            }
        }

        #endregion

        #region Commands

        public ICommand UploadAttachmentViewCommand
        {
            get { return _uploadAttachmentViewCommand ?? (_uploadAttachmentViewCommand = new RelayCommand(ExecuteUploadAttachmentViewCommand)); }
        }
        private void ExecuteUploadAttachmentViewCommand()
        {
            var file = new OpenFileDialog { Filter = "Files(*.*)|*.*" };
            var result = file.ShowDialog();
            if (result == DialogResult.OK)
            {
                var sourcefilePath = file.FileName;
                SelectedAttachment = new AttachmentDTO
                {
                    TicketId = Ticket.Id,
                    AttachmentName = sourcefilePath.Substring(sourcefilePath.LastIndexOf('\\') + 1),
                    AttachmentStatus = AttachmentStatus.Uploading,
                    Ticket = Ticket
                };

                var destinationFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\MATS_Files" + "\\";
                if (!Directory.Exists(destinationFile))
                    Directory.CreateDirectory(destinationFile);

                destinationFile = Path.Combine(destinationFile, Ticket.ROWGUID + "_" +
                    sourcefilePath.Substring(sourcefilePath.LastIndexOf('\\') + 1));

                var fi = new FileInfo(sourcefilePath);

                if (!fi.Exists) return;
                try
                {
                    File.Copy(sourcefilePath, destinationFile);
                }
                catch { }

                SaveAttachment();
                GetLiveAttachments();
            }


        }

        public ICommand DownloadAttachmentViewCommand
        {
            get { return _downloadAttachmentViewCommand ?? (_downloadAttachmentViewCommand = new RelayCommand(ExecuteDownloadAttachmentViewCommand)); }
        }
        private void ExecuteDownloadAttachmentViewCommand()
        {
            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() != DialogResult.OK) return;
            FileLocation = folder.SelectedPath;
            try
            {
                var sourcefileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + "\\MATS_Download_Files" + "\\";
                if (!Directory.Exists(sourcefileName))
                    Directory.CreateDirectory(sourcefileName);
                sourcefileName = Path.Combine(sourcefileName, Ticket.ROWGUID + "_" + SelectedAttachment.AttachmentName);

                var destFileName = SelectedAttachment.AttachmentName;
                var destinationFile = Path.Combine(folder.SelectedPath, destFileName);

                var destFile = new FileInfo(destinationFile);
                if (destFile.Exists)
                    File.Delete(destFile.FullName);

                var sourceFile = new FileInfo(sourcefileName);
                if (sourceFile.Exists)
                {
                    File.Copy(sourcefileName, destinationFile);
                }
                else
                    System.Windows.MessageBox.Show("Can't download requested file!, " + Environment.NewLine +
                        "file may be moved or deleted or there exists same file with the one to download" + Environment.NewLine +
                        "tray again...");
            }
            catch
            {
                System.Windows.MessageBox.Show("Can't Download file!" + Environment.NewLine + "tray again later...");
            }
        }

        public bool SaveAttachment()
        {
            try
            {
                if (SelectedAttachment.Id == 0)
                {
                    _unitOfWork.Repository<AttachmentDTO>().Insert(SelectedAttachment);
                }
                else
                {
                    _unitOfWork.Repository<AttachmentDTO>().Update(SelectedAttachment);
                }
                _unitOfWork.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ICommand DeleteAttachmentViewCommand
        {
            get { return _deleteAttachmentViewCommand ?? (_deleteAttachmentViewCommand = new RelayCommand<Object>(ExecuteDeleteAttachmentViewCommand, CanSave)); }
        }
        private void ExecuteDeleteAttachmentViewCommand(object obj)
        {
            try
            {
                if (System.Windows.MessageBox.Show("Are you Sure You want to Delete this Attachment item?", "Delete Attachment item", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _unitOfWork.Repository<AttachmentDTO>().Delete(SelectedAttachment);
                    _unitOfWork.Commit();
                    GetLiveAttachments();
                }
            }
            catch { }
        }

        public ICommand CloseAttachmentViewCommand
        {
            get { return _closeAttachmentViewCommand ?? (_closeAttachmentViewCommand = new RelayCommand<Object>(ExecuteCloseAttachmentViewCommand, CanSave)); }
        }
        private void ExecuteCloseAttachmentViewCommand(object obj)
        {
            try
            {


                if (SaveAttachment())
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

        private void GetLiveAttachments()
        {
            try
            {
                var Attachments = _unitOfWork.Repository<AttachmentDTO>()
                    .GetAllIncludingChilds(t => t.Ticket).Where(a => a.TicketId == Ticket.Id)
                    .ToList().OrderBy(l => l.AttachmentName);

                FilteredAttachments = new ObservableCollection<AttachmentDTO>(Attachments);
            }
            catch
            {
                FilteredAttachments = new ObservableCollection<AttachmentDTO>();
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
