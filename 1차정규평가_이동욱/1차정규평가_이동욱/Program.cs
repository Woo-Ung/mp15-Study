using System;
using System.ComponentModel.DataAnnotations;
class Program
{
    const string FOOD_STORE_NAME = "던전 국밥";
    static void Main()
    {
        Food[] memu = new Food[] 
        {             
            new Rice("타우 머리 국밥", FoodType.국밥,9000),
            new Rice("용 꼬리 국밥", FoodType.국밥,10000),
            new Stew("얼큰 용암 슬라임 찌개", FoodType.찌개,7000),
            new Stew("복불복 미믹 찌개", FoodType.찌개,8000),
            new Side("하피 알",FoodType.사이드, 500),
            new Side("다진 만드라고라",FoodType.사이드, 300),
            new Drink("탄산 슬라임", FoodType.음료, 1000),
            new Drink("세계수 이슬", FoodType.음료, 2000)

        };




    }
    public void SelectMenuType(int mType)
    {
        switch (mType)
        {
            case 0:
                Console.WriteLine(FoodType.국밥);
                Console.WriteLine(FoodType.찌개);
                break;
            case 1:
                Console.WriteLine(FoodType.사이드);
                Console.WriteLine(FoodType.음료);
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
    protected FoodType FType;
    protected int FPrice;

    public Food(string fName, FoodType fType, int fPrince)
    {
        FName = fName;
        FType = fType;
        FPrice = fPrince;
    }
}

public class Rice : Food
{
    public Rice(string fName, FoodType fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}

public class Stew : Food
{
    public Stew(string fName, FoodType fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}

public class Side : Food
{
    public Side(string fName, FoodType fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}

public class Drink : Food
{
    public Drink(string fName, FoodType fType, int fPrince) : base(fName, fType, fPrince)
    {

    }
}