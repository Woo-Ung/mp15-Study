using System;
class Program
{
    const string FOOD_STORE_NAME = "던전 국밥";
    static void Main()
    {
        Food[] memu = new Food[] { };




    }
}

public enum MenuType
{
    메인,
    추가
}

public enum FoodType
{
    국밥,
    찌개,
    사이드,
    음료
}


public abstract class Food
{
    protected string FName;
    protected string FType;
    protected int FPrice;

    public Food(string fName, string fType, int fPrince)
    {
        FName = fName;
        FType = fType;
        FPrice = fPrince;
    }
}