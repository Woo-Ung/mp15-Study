using System;
class Program
{
    const string FOOD_STORE_NAME = "던전 국밥";
    static void Main()
    {
        Food[] memu = new Food[] { };




    }
    public void SelectMenuType(int mType)
    {
        switch (mType)
        {
            case 0:
                break;
            case 1:
                break;
            default:
                break;
        }
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

public class Rice : Food
{
    public Rice(string fName, string fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}

public class Stew : Food
{
    public Stew(string fName, string fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}

public class Side : Food
{
    public Side(string fName, string fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}

public class Drink : Food
{
    public Drink(string fName, string fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}