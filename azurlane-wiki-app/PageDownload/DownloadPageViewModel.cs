using azurlane_wiki_app.Properties;
using azurlane_wiki_app.Data.Downloaders;
using azurlane_wiki_app.PageEquipmentList;
using azurlane_wiki_app.PageShipGirlList;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace azurlane_wiki_app.PageDownload
{
    public class DownloadPageViewModel : INotifyPropertyChanged
    {
        private DataDownloader _firstDownloader;
        private DataDownloader _secondDownloader;
        private bool _downloading;
        private Statuses _iconsStatus;
        private Statuses _shipGirlsStatus;
        private Statuses _equipmentStatus;
        private Statuses _skillsStatus;
        private Statuses _mapsStatus;

        public RelayCommand Download { get; set; }
        public RelayCommand OpenGraphicalShipGirlPageCommand { get; set; }
        public RelayCommand OpenTableShipGirlPageCommand { get; set; }
        public RelayCommand OpenEquipmentListPageCommand { get; set; }

        public bool Downloading
        {
            get => _downloading;
            set
            {
                _downloading = value;
                OnPropertyChanged(nameof(Downloading));
            }
        }

        public DataDownloader FirstDownloader
        {
            get => _firstDownloader;
            set
            {
                _firstDownloader = value;
                OnPropertyChanged(nameof(FirstDownloader));
            }
        }

        public DataDownloader SecondDownloader
        {
            get => _secondDownloader;
            set
            {
                _secondDownloader = value;
                OnPropertyChanged(nameof(SecondDownloader));
            } 
        }

        public Statuses IconsStatus
        {
            get => _iconsStatus;
            set
            {
                _iconsStatus = value;
                OnPropertyChanged(nameof(IconsStatus));
            }
        }

        public Statuses ShipGirlsStatus
        {
            get => _shipGirlsStatus;
            set
            {
                _shipGirlsStatus = value;
                OnPropertyChanged(nameof(ShipGirlsStatus));
            }
        }

        public Statuses EquipmentStatus
        {
            get => _equipmentStatus;
            set
            {
                _equipmentStatus = value;
                OnPropertyChanged(nameof(EquipmentStatus));
            }
        }

        public Statuses SkillsStatus
        {
            get => _skillsStatus;
            set
            {
                _skillsStatus = value;
                OnPropertyChanged(nameof(SkillsStatus));
            }
        }

        public Statuses MapsStatus
        {
            get => _mapsStatus;
            set
            {
                _mapsStatus = value;
                OnPropertyChanged(nameof(MapsStatus));
            }
        }

        public ISnackbarMessageQueue SnackBarMessageQueue { get; set; } 
            = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));


        /// <summary>
        /// Download Page's view model
        /// </summary>
        /// <param name="mainWindow">To access other VMs for update after download</param>
        public DownloadPageViewModel(MainWindow mainWindow)
        {
            Download = new RelayCommand(async obj =>
            {
                if (!Downloading)
                {
                    Downloading = true;

                    IconDownloader iconDownloader = new IconDownloader();
                    ShipDownloader shipDownloader = new ShipDownloader(6);
                    EquipmentDownloader equipmentDownloader = new EquipmentDownloader(2);
                    SkillDownloader skillDownloader = new SkillDownloader();
                    WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();

                    IconsStatus = iconDownloader.Status;
                    ShipGirlsStatus = shipDownloader.Status;
                    EquipmentStatus = equipmentDownloader.Status;
                    SkillsStatus = skillDownloader.Status;
                    MapsStatus = wtgShipGirlDownloader.Status;

                    iconDownloader.PropertyChanged += StatusChangedEventHandler;
                    shipDownloader.PropertyChanged += StatusChangedEventHandler;
                    equipmentDownloader.PropertyChanged += StatusChangedEventHandler;
                    skillDownloader.PropertyChanged += StatusChangedEventHandler;
                    wtgShipGirlDownloader.PropertyChanged += StatusChangedEventHandler;

                    FirstDownloader = iconDownloader;
                    await Task.Run(() => iconDownloader.Download());

                    Task[] tasks = new Task[2];

                    FirstDownloader = shipDownloader;
                    SecondDownloader = equipmentDownloader;

                    tasks[0] = Task.Run(() => shipDownloader.Download());
                    tasks[1] = Task.Run(() => equipmentDownloader.Download());

                    await Task.WhenAll(tasks);

                    FirstDownloader = skillDownloader;
                    SecondDownloader = wtgShipGirlDownloader;

                    tasks[0] = Task.Run(() => skillDownloader.Download());
                    tasks[1] = Task.Run(() => wtgShipGirlDownloader.Download());

                    await Task.WhenAll(tasks);

                    if (mainWindow != null)
                    {
                        mainWindow.GraphicalShipGirlListPageVM = new GraphicalShipGirlListPageViewModel();
                        mainWindow.TableShipGirlListPageVM = new TableShipGirlListPageViewModel();
                        mainWindow.EquipmentListPageVM = new EquipmentListPageViewModel();
                    }
                    
                    Downloading = false;
                }
                else
                {
                    SnackBarMessageQueue.Enqueue("Already downloading!");
                }
            });

            OpenGraphicalShipGirlPageCommand = new RelayCommand(obj =>
            {
                if (!Downloading)
                {
                    Navigation.Navigate(new GraphicalShipGirlListPage());
                }
                else
                {
                    SnackBarMessageQueue.Enqueue("Can't leave this page until download completes!");
                }
            });

            OpenTableShipGirlPageCommand = new RelayCommand(obj =>
            {
                if (!Downloading)
                {
                    Navigation.Navigate(new TableShipGirlListPage());
                }
                else
                {
                    SnackBarMessageQueue.Enqueue("Can't leave this page until download completes!");
                }
            });

            OpenEquipmentListPageCommand = new RelayCommand(obj =>
            {
                if (!Downloading)
                {
                    Navigation.Navigate(new EquipmentListPage());
                }
                else
                {
                    SnackBarMessageQueue.Enqueue("Can't leave this page until download completes!");
                }
            });
        }

        private void StatusChangedEventHandler(object sender, PropertyChangedEventArgs a)
        {
            switch (sender)
            {
                case object downloader when downloader is IconDownloader iconDownloader:
                    IconsStatus = iconDownloader.Status;
                    break;
                case object downloader when downloader is ShipDownloader shipDownloader:
                    ShipGirlsStatus = shipDownloader.Status;
                    break;
                case object downloader when downloader is EquipmentDownloader equipmentDownloader:
                    EquipmentStatus = equipmentDownloader.Status;
                    break;
                case object downloader when downloader is SkillDownloader skillDownloader:
                    SkillsStatus = skillDownloader.Status;
                    break;
                case object downloader when downloader is WTGShipGirlDownloader wtgShipGirlDownloader:
                    MapsStatus = wtgShipGirlDownloader.Status;
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
