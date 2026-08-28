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
            new Mains(1, "타우 머리 국밥", FoodType.식사,9000),
            new Mains(2, "용 꼬리 국밥", FoodType.식사,10000),
            new Mains(3, "얼큰 용암 슬라임 찌개", FoodType.식사,7000),
            new Mains(4, "복불복 미믹 찌개", FoodType.식사,8000),                      
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
                Console.WriteLine();
                myKiosk.PrintMyBag(myMenu);
                Console.WriteLine();                
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

    public void PrintMyBag(List<Food> myMenu)
    {
        Console.WriteLine("[장바구니]");
        Console.WriteLine("=====================");
        Console.WriteLine();
        int totalM = 0;
        for (int i = 0; i < myMenu.Count;i++)
        {
            Console.WriteLine($"{myMenu[i].FName} * {myMenu[i].Count}개 = {myMenu[i].Calculate()} ");
            totalM += myMenu[i].Calculate();
        }
    }

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
                totalM += myMenu[i].Calculate();
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
                    MenuCountReset();
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

    public void MenuCountReset()
    {
        for (int i = 0; i < Menu.Length; i++)
        {
            Menu[i].CountReSet();
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
        switch (orderM)
        {
            case 1:
                SelectMenuInfo(MenuType.메인);                
                int orderNM = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 1, 4) - 1;                
                Console.WriteLine("개수를 입력해주세요");

                int orderCM = ConsoleInput.ReadIntInRange("개수 : ", 0, 100);                
                Console.Clear();

                Mains mains = (Mains)Menu[orderNM];
                mains.Count = orderCM;
                if (!myMenu.Contains(mains))
                {
                    myMenu.Add(mains);
                }
                break;

            case 2:
                SelectMenuInfo(MenuType.사이드);
                int orderNS = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 5, 8);
                Console.WriteLine("개수를 입력해주세요");
                
                int orderCS = ConsoleInput.ReadIntInRange("개수 : ", 0, 100);                
                Console.Clear();

                if (orderNS == 5 || orderNS == 6)
                {
                    Drink drink = (Drink)Menu[orderNS -1];
                    drink.Count = orderCS;
                    if (!myMenu.Contains(drink))
                    {
                        myMenu.Add(drink);
                    }
                    myMenu.Add(drink);                    
                }
                else 
                {
                    Side side = (Side)Menu[orderNS - 1];
                    side.Count = orderCS;
                    if (!myMenu.Contains(side))
                    {
                        myMenu.Add(side);
                    }
                    myMenu.Add(side);                    
                }
                break;

            default:
                break;
        }
    }
}

public abstract class Food
{
    protected int FNum;
    public string FName { get; protected set; }
    public FoodType FType { get; protected set; }
    public int FPrice { get; protected set; }

    private int count = 0;
    public int Count
    {
        get { return count; }
        set { count += value; }
    }

    public Food(int fNum, string fName, FoodType fType, int fPrince)
    {
        FNum = fNum;
        FName = fName;
        FType = fType;
        FPrice = fPrince;
    }
    public virtual void PrintInfo()
    {
        Console.WriteLine($"{FNum}. [{FType}] {FName} : {FPrice}원");
    }
    public void CountReSet()
    {
        count = 0;
    }
    public abstract int Calculate();
}

public class Mains : Food
{
    public Mains(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }

    public override int Calculate()
    {
        return FPrice * Count;
    }
}

public class Side : Food
{
    public Side(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }
    public override int Calculate()
    {        
        return FPrice * Count;
    }
}

public class Drink : Food
{
    public Drink(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {

    }

    public override void PrintInfo()
    {
        Console.WriteLine($"{FNum}. [{FType}] {FName} : {FPrice}원 [\"3병 이상 구매시 10%할인\"]");
    }
    public override int Calculate()
    {
        if (Count > 0 && Count < 3)
        {
            return FPrice * Count;
        }
        else if (Count >= 3)
        {
            return (int)((FPrice * Count) - (0.1 * (FPrice * Count)));
        }
        else
        {
            return 0;
        }
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