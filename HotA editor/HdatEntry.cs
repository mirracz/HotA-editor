using System.ComponentModel;

namespace HotA_editor
{
    public class HdatEntry : INotifyPropertyChanged
    {
        private string _name;
        private string _folderName;
        private string _data1;
        private string _data2;
        private string _data3;
        private string _data4;
        private string _data5;
        private string _data6;
        private string _data7;
        private string _data8;
        private byte[] _data9;
        private int[] _data10;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; NotifyPropertyChanged("FolderName"); }
        }

        public string Data1
        {
            get { return _data1; }
            set { _data1 = value; NotifyPropertyChanged("Data1"); }
        }

        public string Data2
        {
            get { return _data2; }
            set { _data2 = value; NotifyPropertyChanged("Data2"); }
        }

        public string Data3
        {
            get { return _data3; }
            set { _data3 = value; NotifyPropertyChanged("Data3"); }
        }

        public string Data4
        {
            get { return _data4; }
            set { _data4 = value; NotifyPropertyChanged("Data4"); }
        }

        public string Data5
        {
            get { return _data5; }
            set { _data5 = value; NotifyPropertyChanged("Data5"); }
        }

        public string Data6
        {
            get { return _data6; }
            set { _data6 = value; NotifyPropertyChanged("Data6"); }
        }

        public string Data7
        {
            get { return _data7; }
            set { _data7 = value; NotifyPropertyChanged("Data7"); }
        }

        public string Data8
        {
            get { return _data8; }
            set { _data8 = value; NotifyPropertyChanged("Data8"); }
        }

        public byte[] Data9
        {
            get { return _data9; }
            set { _data9 = value; NotifyPropertyChanged("Data9"); }
        }

        public int[] Data10
        {
            get { return _data10; }
            set { _data10 = value; NotifyPropertyChanged("Data10"); }
        }

        public string DisplayMember
        {
            get { return Name; }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayMember"));
            }
        }
    }
}
