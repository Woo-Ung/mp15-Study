// https://github.com/Woo-Ung/mp15-Study.git

using System;
using System.Collections.Generic;
using System.Net;

class Program
{
    const string FOOD_STORE_NAME = "던전 국밥";
    const int FOOD_COUNT = 8;
    static void Main()
    {
        Food[] menu = new Food[FOOD_COUNT]
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
            Console.SetCursorPosition(0, 0);
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
                myKiosk.Order(order, myMenu, ref isOrder, ref isShutDown);
            }
        }
        Console.WriteLine("===== 영업 종료 =====");
    }
}

// === Kiosk ===

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

    public bool[] isCanCookNum = { true, true, true };

    public Food[] Menu;

    public Kiosk(Food[] menu)
    {
        Menu = menu;
    }

    // 매서드

    public void PrintMyBag(List<Food> myMenu)
    {
        Console.WriteLine("");
        Console.WriteLine("======== [장바구니] ========");
        Console.WriteLine();
        int totalM = 0;
        for (int i = 0; i < myMenu.Count; i++)
        {
            Console.WriteLine($"{myMenu[i].FName} * {myMenu[i].Count}개 = {myMenu[i].Calculate()}원 ");
            totalM += myMenu[i].Calculate();
        }
        Console.WriteLine();
        Console.WriteLine($"===== 합계 금액 : {totalM}원 =====");
    }

    public void Order(int order, List<Food> myMenu, ref bool isOrder, ref bool isShutDown)
    {
        int totalM = 0;
        int totalCookCount = 0;

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
                totalCookCount += myMenu[i].CookTimeCalculate();
            }
            if (myMenu.Count == 0)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"장바구니가 비어있습니다. 메뉴를 선택해주세요");
                Console.SetCursorPosition(0, 0);

            }
            else
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"총 결제 금액은 {totalM}입니다. 가지고 계신 금액을 입력해주세요.");
                int myMoney = ConsoleInput.ReadIntInRange("내가 낼 금액 : ", 0, int.MaxValue);

                if (myMoney >= totalM)
                {
                    Console.WriteLine($"결제되었습니다. 거스름돈 {myMoney - totalM}원");
                    TotalMoney = totalM;
                    TotalOrder = 1;

                    Cooking(TotalOrder, totalCookCount);
                    myMenu.Clear();
                    MenuCountReset();
                    ConsoleInput.Pause();
                    Console.Clear();
                    isOrder = false;
                    return;
                }

                else
                {
                    Console.WriteLine($"결제가 거부되었습니다. 금액이 {totalM - myMoney}원 부족합니다.");
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
            Console.WriteLine();
            Console.WriteLine($"총 주문 건수 : {TotalOrder}회");
            Console.WriteLine();
            Console.WriteLine($"총 매출액 : {TotalMoney}원");
            Console.WriteLine();
            isOrder = false;
            isShutDown = true;
            return;
        }
    }

    public async Task<int> canCooking()
    {
        bool isEmpty = false;
        int Num = 0;
        while (!isEmpty)
        {
            for (int i = 0; i < isCanCookNum.Length; i++)
            {
                if (isCanCookNum[i])
                {
                    Num = i;
                    isCanCookNum[i] = false;
                    isEmpty = true;
                    break;
                }
            }
            await Task.Delay(100);
        }
        return Num;
    }
    public async Task Cooking(int cookOrderNum, int totalCookCount)
    {
        const int W_LINE = 26;

        int canCookNum = 0;

        canCookNum = await canCooking();

        for (int i = 0; i < totalCookCount; i++)
        {
            await Task.Delay(1000);
            Console.SetCursorPosition(0, W_LINE - 1);
            Console.WriteLine("========================================================");
            Console.SetCursorPosition(0, W_LINE + canCookNum);
            Console.Write($"{cookOrderNum}번 주문 조리중 : {i} / {totalCookCount}");
            Console.SetCursorPosition(15, 21);
        }
        Console.SetCursorPosition(0, W_LINE + canCookNum);
        Console.Write("                                                                ");
        Console.SetCursorPosition(0, W_LINE + canCookNum);
        Console.WriteLine($"{cookOrderNum}번 주문 조리 완료 ");
        isCanCookNum[canCookNum] = true;
        Console.SetCursorPosition(15, 21);
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

        else if (menuType == MenuType.추가)
        {
            Console.WriteLine($"=== [{FoodType.추가}] ===");
            for (int i = 0; i < Menu.Length; i++)
            {
                if (Menu[i].FType == FoodType.추가)
                {
                    Menu[i].PrintInfo();
                }
            }
            Console.WriteLine();
            Console.WriteLine($"***{MenuType.추가} 번호를 선택해주세요***");            
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
                int orderNM = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 1, 4);
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("개수를 입력해주세요");

                int orderCM = ConsoleInput.ReadIntInRange("개수 : ", 1, 100);
                Console.Clear();

                Mains mains = (Mains)Menu[orderNM - 1];
                mains.Count = orderCM;
                if (!myMenu.Contains(mains))
                {
                    myMenu.Add(mains);
                }
                
                SelectMenuInfo(MenuType.추가);
                Console.WriteLine();
                Console.WriteLine("*** 추가를 원하지 않을 시 9번을 눌러주세요 ***");
                Console.WriteLine();

                int orderNA = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 7, 9);

                if (orderNA == 9)
                {
                    Console.Clear();
                    break;
                }
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("개수를 입력해주세요");

                int orderCA = ConsoleInput.ReadIntInRange("개수 : ", 1, 100);
                Console.Clear();

                Side add = (Side)Menu[orderNA - 1];
                add.Count = orderCA;
                if (!myMenu.Contains(add))
                {
                    myMenu.Add(add);
                }

                break;

            case 2:
                SelectMenuInfo(MenuType.사이드);
                int orderNS = ConsoleInput.ReadIntInRange("메뉴 번호 : ", 5, 6);
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("개수를 입력해주세요");

                int orderCS = ConsoleInput.ReadIntInRange("개수 : ", 1, 100);
                Console.Clear();

                Drink drink = (Drink)Menu[orderNS - 1];
                drink.Count = orderCS;
                if (!myMenu.Contains(drink))
                {
                    myMenu.Add(drink);
                }

                break; 

            default:
                break;
        }
    }
}
// === Food class ===
public abstract class Food
{
    protected int FNum;
    public string FName { get; protected set; }
    public FoodType FType { get; protected set; }
    public int FPrice { get; protected set; }
    public int CookCount { get; protected set; }

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

    public int CookTimeCalculate()
    {
        return CookCount * Count;
    }
}

public class Mains : Food
{
    
    public Mains(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {
        CookCount = 20;
    }

    public override int Calculate()
    {
        return FPrice * Count;
    }    
}

public class Side : Food
{
    const float DISCOUNT = 0.3f;
    public Side(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {
        CookCount = 1;
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"{FNum}. [{FType}] {FName} : {FPrice}원 [\"2개 이상 구매시 30%할인\"]");
    }

    public override int Calculate()
    {
        if (Count > 0 && Count < 2)
        {
            return FPrice * Count;
        }
        else if (Count >= 2)
        {
            return (int)((FPrice * Count) - (DISCOUNT * (FPrice * Count)));
        }
        else
        {
            return 0;
        }
    }
}

public class Drink : Food
{
    const float DISCOUNT = 0.1f;
    public Drink(int fNum, string fName, FoodType fType, int fPrince) : base(fNum, fName, fType, fPrince)
    {
        CookCount = 1;
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
            return (int)((FPrice * Count) - (DISCOUNT * (FPrice * Count)));
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
    사이드,
    추가
}

public enum FoodType
{
    식사 = 1,
    추가,
    음료
}