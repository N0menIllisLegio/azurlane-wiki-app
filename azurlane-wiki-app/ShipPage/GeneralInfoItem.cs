using System.ComponentModel;
using System.Runtime.CompilerServices;
using azurlane_wiki_app.Annotations;

namespace azurlane_wiki_app.ShipPage
{
    public class GeneralInfoItem : INotifyPropertyChanged
    {
        private string _description;
        private string _icon;
        public string Name { get; set; }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public int IconWidth { get; set; }
        public int IconHeight { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}