namespace Trucks.Common;

public static class ValidationConstants
{
    //Truck
    public const int TruckRegistrationNumberLenght = 8;
    public const int TruckVinNumberLenght = 17;
    public const int TruckTankCapacityMinLenght = 950;
    public const int TruckTankCapacityMaxLenght = 1420;
    public const int TruckCargoCapacityMinLenght = 5000;
    public const int TruckCargoCapacityMaxLenght = 29000;
    public const int TruckCategoryTypeMinValue = 0;
    public const int TruckCategoryTypeMaxValue = 3;
    public const int TruckMakeTypeMinValue = 0;
    public const int TruckMakeTypeMaxValue = 4;

    //Client
    public const int ClientNameMinLenght = 3;
    public const int ClienrNameMaxLenght = 40;
    public const int ClientNationalityMinLenght = 2;
    public const int ClientNationalityMaxLenght = 40;

    //Despatcher
    public const int DespatcherNameMinLenght = 2;
    public const int DespatcherNameMaxLenght = 40;
}
