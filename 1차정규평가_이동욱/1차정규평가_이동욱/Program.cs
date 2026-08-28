using System;
class Program
{
    const string FOOD_STORE_NAME = "던전 국밥";
    static void Main()
    {
        


    }
}

public class Food
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