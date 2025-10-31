using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp_Chebotarev;
using System.Linq;

public class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
    private ObservableCollection<Room> rooms;
    public ObservableCollection<Room> Rooms
    {
        get => rooms;
        set
        {
            rooms = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Reservation> reservations;
    public ObservableCollection<Reservation> Reservations
    {
        get => reservations;
        set
        {
            reservations = value;
            OnPropertyChanged();
        }
    }
    
    private ObservableCollection<Guest> guests;
    public ObservableCollection<Guest> Guests
    {
        get => guests;
        set
        {
            guests = value;
            OnPropertyChanged();
        }
    }

    public ViewModel()
    {
        LoadRooms();
        LoadGuests();
        LoadReservations();
    }

    private void LoadRooms()
    {
        using (var context = new DBEntities())
        {
            Rooms = new ObservableCollection<Room>(context.Rooms.ToList());
        }
    }

    private void LoadGuests()
    {
        using (var context = new DBEntities())
        {
            Guests = new ObservableCollection<Guest>(context.Guests.ToList());
        }
    }

    private void LoadReservations()
    {
        using (var context = new DBEntities())
        {
            Reservations = new ObservableCollection<Reservation>(context.Reservations.ToList());
        }
    }
}
