using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class AuxiliaryItem : BaseEquipmentItem
    {
        public int Health 
        { 
            get => health;
            set
            {
                health = value;
                OnPropertyChanged(nameof(Health));
            }
        }
        public int Firepower 
        { 
            get => firepower;
            set
            {
                firepower = value;
                OnPropertyChanged(nameof(Firepower));
            }
        }

        [DisplayName("Anti-Air")]
        public int AA 
        { 
            get => aA;
            set
            {
                aA = value;
                OnPropertyChanged(nameof(AA));
            }
        }

        [DisplayName("Torpedo")]
        public int Torp 
        { 
            get => torp;
            set
            {
                torp = value;
                OnPropertyChanged(nameof(Torp));
            }
        }
        public int Aviation 
        { 
            get => aviation;
            set
            {
                aviation = value;
                OnPropertyChanged(nameof(Aviation));
            }
        }
        public float Reload 
        { 
            get => reload;
            set
            {
                reload = value;
                OnPropertyChanged(nameof(Reload));
            }
        }
        public int Evasion 
        { 
            get => evasion;
            set
            {
                evasion = value;
                OnPropertyChanged(nameof(Evasion));
            }
        }
        public int Oxygen 
        { 
            get => oxygen;
            set
            {
                oxygen = value;
                OnPropertyChanged(nameof(Oxygen));
            }
        }
        public int Accuracy 
        { 
            get => accuracy;
            set
            {
                accuracy = value;
                OnPropertyChanged(nameof(Accuracy));
            }
        }
        public int Speed 
        { 
            get => speed;
            set
            {
                speed = value;
                OnPropertyChanged(nameof(Speed));
            }
        }
        public string Note { get; set; }

        private int bufHealth;
        private int bufFirepower;
        private int bufAA;
        private int bufTorp;
        private int bufAviation;
        private float bufReload;
        private int bufEvasion;
        private int bufOxygen;
        private int bufAccuracy;
        private int bufSpeed;

        private int health;
        private int firepower;
        private int aA;
        private int torp;
        private int aviation;
        private float reload;
        private int evasion;
        private int oxygen;
        private int accuracy;
        private int speed;

        public AuxiliaryItem(Equipment equipment) : base(equipment)
        {
            Health = equipment.HealthMax ?? 0;
            Firepower = equipment.FPMax ?? 0;
            AA = equipment.AAMax ?? 0;
            Torp = equipment.TorpMax ?? 0;
            Aviation = equipment.AvMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            Evasion = equipment.EvasionMax ?? 0;
            Oxygen = equipment.OxygenMax ?? 0;
            Accuracy = equipment.AccMax ?? 0;
            Speed = equipment.SpdMax ?? 0;

            bufHealth = equipment.Health ?? 0;
            bufFirepower = equipment.Firepower ?? 0;
            bufAA = equipment.AA ?? 0;
            bufTorp = equipment.Torpedo ?? 0;
            bufAviation = equipment.Aviation ?? 0;
            bufReload = equipment.RoF ?? 0;
            bufEvasion = equipment.Evasion ?? 0;
            bufOxygen = equipment.Oxygen ?? 0;
            bufAccuracy = equipment.Acc ?? 0;
            bufSpeed = equipment.Spd ?? 0;

            Note = equipment.Notes;
        }

        public override void ChangeStats(DPSData dpsData = null)
        {
            base.ChangeStats(dpsData);

            Health = Swap(Health, ref bufHealth);
            Firepower = Swap(Firepower, ref bufFirepower);
            AA = Swap(AA, ref bufAA);
            Torp = Swap(Torp, ref bufTorp);
            Aviation = Swap(Aviation, ref bufAviation);
            Reload = Swap(Reload, ref bufReload);
            Evasion = Swap(Evasion, ref bufEvasion);
            Oxygen = Swap(Oxygen, ref bufOxygen);
            Accuracy = Swap(Accuracy, ref bufAccuracy);
            Speed = Swap(Speed, ref bufSpeed);
        }
    }
}
