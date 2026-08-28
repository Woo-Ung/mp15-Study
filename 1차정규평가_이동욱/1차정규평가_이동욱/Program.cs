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
            new Side("다진 만드라고라",FoodType.추가, 300),
            new Side("하피 알",FoodType.추가, 500),            
            new Drink("탄산 슬라임", FoodType.음료, 1000),
            new Drink("세계수 이슬", FoodType.음료, 2000)
        };

        List<Food> myMenu = new List<Food>();

        Console.WriteLine("=====================");
        Console.WriteLine($"===== {FOOD_STORE_NAME} =====");
        Console.WriteLine("=====================");
        Console.WriteLine();

        for (int i = 0; i < memu.Length; i++)
        {
            memu[i].PrintInfo();
        }

        Console.WriteLine("1. 주문  2. 전체 비우기  3. 결제  4.영업 종료");

        Console.WriteLine("***메뉴를 선택해주세요***");
        Console.WriteLine("1. 메인  2. 사이드");



    }
    public void SelectMenuType(MenuType mType)
    {
        switch (mType)
        {
            case MenuType.메인:
                Console.WriteLine(FoodType.국밥);
                Console.WriteLine(FoodType.찌개);
                break;
            case MenuType.사이드:
                Console.WriteLine(FoodType.추가);
                Console.WriteLine(FoodType.음료);
                break;
            default:
                break;
        }
    }

}

public enum MenuType
{
    메인 = 1,
    사이드
}

public enum FoodType
{
    국밥 = 1,
    찌개,
    추가,
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
    public void PrintInfo()
    {
        Console.WriteLine($"[{FType}] {FName} : {FPrice}원");
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