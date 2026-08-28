using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Xml.Linq;
class Program
{
    const string FOOD_STORE_NAME = "던전 국밥";
    static void Main()
    {
        Food[] menu = new Food[] 
        {             
            new Rice(1, "타우 머리 국밥", FoodType.식사,9000),
            new Rice(2, "용 꼬리 국밥", FoodType.식사,10000),
            new Stew(3, "얼큰 용암 슬라임 찌개", FoodType.식사,7000),
            new Stew(4, "복불복 미믹 찌개", FoodType.식사,8000),                      
            new Drink(5, "탄산 슬라임", FoodType.음료, 1000),
            new Drink(6, "세계수 이슬", FoodType.음료, 2000),
            new Side(7, "다진 만드라고라",FoodType.추가, 300),
            new Side(8, "하피 알",FoodType.추가, 500)
        };
        Kiosk myKiosk = new Kiosk(menu);
        
        List<Food> myMenu = new List<Food>();

        bool isShutDown = false;

        while (!isShutDown)
        {
            bool isOrder = true;
            Console.WriteLine("=====================");
            Console.WriteLine($"===== {FOOD_STORE_NAME} =====");
            Console.WriteLine("=====================");
            Console.WriteLine();

            for (int i = 0; i < menu.Length; i++)
            {
                menu[i].PrintInfo();
            }

            while (isOrder)
            {
                Console.WriteLine("1. 주문  2. 전체 비우기  3. 결제  4.영업 종료");
                int order = ConsoleInput.ReadIntInRange("선택 번호 : ", 1, 4);
                myKiosk.Order(order, myMenu,ref isOrder, ref isShutDown);
            }
        }
        Console.WriteLine("===== 영업 종료 =====");
    }
}

// === Class ===

public class Kiosk
{
    private int totalMoney;
    public int TotalMoney 
    {
        get { return totalMoney; }
        set { totalMoney += value; }
    }
    private int totalOrder;
    public int TotalOrder
    {
        get { return totalOrder; }
        set { totalOrder += value; }
    }
    public Food[] Menu;

    public Kiosk(Food[] menu)
    {
        Menu = menu;
    }
    
    // 매서드
    public void Order(int order, List<Food> myMenu,ref bool isOrder, ref bool isShutDown)
    {
        int totalM = 0;
        if (order == 1)
        {
            Console.Clear();
            Console.WriteLine("***메뉴 타입 선택해주세요***");
            Console.WriteLine("1. 메인  2. 사이드");
            int orderM = ConsoleInput.ReadIntInRange("메뉴 타입 : ", 1, 2);
            Console.Clear();
            SelectMenuType(orderM, myMenu);            
        }
        else if (order == 2)
        {
            myMenu.Clear();
            Console.Clear();
            return;
        }
        else if (order == 3)
        {
            for (int i = 0; i < myMenu.Count; i++)
            {
                totalM += myMenu[i].FPrice;
            }
            if(myMenu.Count == 0)
            {
                Console.WriteLine($"장바구니가 비어있습니다. 메뉴를 선택해주세요");
            }
            else
            {
                Console.WriteLine($"총 결제 금액은 {totalM}입니다. 가지고 계신 금액을 입력해주세요.");
                int myMoney = ConsoleInput.ReadIntInRange("내가 낼 금액 : ", 0, int.MaxValue);

                if (myMoney >= totalM)
                {
                    Console.WriteLine($"결제되었습니다. 거스름돈 {myMoney - totalM}원");
                    TotalMoney = totalM;
                    TotalOrder = 1;
                    myMenu.Clear();
                    ConsoleInput.Pause();
                    Console.Clear();
                    isOrder = false;
                    return;
                }
                else
                {
                    Console.WriteLine($"결제가 거부되었습니다. 금액이 {totalM - myMoney}부족합니다.");
                    ConsoleInput.Pause();
                    Console.Clear();
                    return;
                }
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("===== 영업 결과 =====");
            Console.WriteLine($"총 주문 건수 : {TotalOrder}");
            Console.WriteLine($"총 매출액 : {TotalMoney}");
            isOrder = false;
            isShutDown = true;
            return;
        }
    }

    public void SelectMenuInfo(MenuType menuType)
    {        
        Console.WriteLine($"***{menuType} 메뉴를 선택해주세요***");
        Console.WriteLine();
        if (menuType == MenuType.메인)
        {
            Console.WriteLine($"=== [{FoodType.식사}] ===");
            for (int i = 0; i < Menu.Length; i++)
            {
                if (Menu[i].FType == FoodType.식사)
                {
                    Menu[i].PrintInfo();
                }
            }
            Console.WriteLine();
            Console.WriteLine($"***{FoodType.식사} 번호를 선택해주세요***");
        }
        else
        {
            Console.WriteLine($"=== [{FoodType.음료}] ===");
            for (int i = 0; i < Menu.Length; i++)
            {
                if (Menu[i].FType == FoodType.음료)
                {
                    Menu[i].PrintInfo();
                }
            }
            Console.WriteLine($"=== [{FoodType.추가}] ===");
            for (int i = 0; i < Menu.Length; i++)
            {
                if (Menu[i].FType == FoodType.추가)
                {
                    Menu[i].PrintInfo();
                }
            }
            Console.WriteLine();
            Console.WriteLine($"***{MenuType.사이드} 번호를 선택해주세요***");
        }
    }

    public void SelectMenuType(int orderM, List<Food> myMenu)
    {
        int orderN = 0;
        switch (orderM)
        {
            case 1:
                SelectMenuInfo(MenuType.메인);
                
                orderN = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 1, 4);
                Console.Clear();
                myMenu.Add(Menu[orderN]);                
                break;
            case 2:
                SelectMenuInfo(MenuType.사이드);
                
                orderN = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 5, 8);
                Console.Clear();
                myMenu.Add(Menu[orderN]);                
                break;
            default:
                break;
        }
    }
}


public abstract class Food
{
    protected int FNum;
    protected string FName;
    public FoodType FType { get; protected set; }
    public int FPrice { get; protected set; }

    public Food(int fNum, string fName, FoodType fType, int fPrince)
    {
        FNum = fNum;
        FName = fName;
        FType = fType;
        FPrice = fPrince;
    }
    public void PrintInfo()
    {
        Console.WriteLine($"{FNum}. [{FType}] {FName} : {FPrice}원");
    }

    public abstract void Calculate();
}

public class Rice : Food
{
    public Rice(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }

    public override void Calculate()
    {
        
    }
}

public class Stew : Food
{
    public Stew(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }
    public override void Calculate()
    {

    }
}

public class Side : Food
{
    public Side(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }
    public override void Calculate()
    {

    }
}

public class Drink : Food
{
    public Drink(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }
    public override void Calculate()
    {

    }
}

// === enum ===
public enum MenuType
{
    메인 = 1,
    사이드
}

public enum FoodType
{
    식사 = 1,
    추가,
    음료
}