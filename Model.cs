using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WF_Lessons_23_24
{
    // статус выстрела
    public enum ShotStatus 
    { 
        Miss, //промах
        Wounded, //ранен
        Kill, // убит
        EndBattle // конец боя
    }

    // Статус координат
    public enum CoordStatus
    {
        None, // Пусто
        Ship, // Корабль
        Shot, // Выстрел
        Got // Попал

    }

    // Типы корабля
    public  enum ShipType
    {
        x4,
        x3,
        x2,
        x1
    }

    // Направление размещения корабля
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public class Model
    {
        // поле массив координат своих кораблей (кораблей игрока)
        public CoordStatus[,] PlayerShips = new CoordStatus[10, 10];

        // поле массив координат кораблей противника
        public CoordStatus[,] EnemyShips = new CoordStatus[10, 10];

        // количество клеток кораблей противника
        public int UndiscoverCells = 20;

        // поле статуса последнего выстрела
        public ShotStatus LastShot;

        // Поле статуса ранения
        public bool WoundedStatus;

        // Поле статуса первого выстрела
        bool FirstShort;

        // Поле количество попаданий по кораблю
        int WoundedCount = 1;

        // Поле координат последнего выстрела
        public string? LastShotCoord;


        // конструктор по умолчанию. Инициализация полей модели.
        public Model()
        {
            LastShot = ShotStatus.Miss;
            WoundedStatus = false;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    PlayerShips[i, j] = CoordStatus.None;
                    EnemyShips[i, j] = CoordStatus.None;
                }
            }
        }

        // Выстрел.
        // Входной параметр - координаты выстрела в виде строки из 2х цифр (xy).
        public ShotStatus Shot(string ShotCoord)
        {
            ShotStatus result = ShotStatus.Miss;

            int x, y; // координаты выстрела в числовом виде

            x = int.Parse(ShotCoord.Substring(0, 1));
            y = int.Parse(ShotCoord.Substring(1, 1));

            if (PlayerShips[x, y] == CoordStatus.None)
            {
                result = ShotStatus.Miss;
            } else
            {
                result = ShotStatus.Kill;
                if ((x != 9 && PlayerShips[x + 1, y] == CoordStatus.Ship) ||
                    (y != 9 && PlayerShips[x, y + 1] == CoordStatus.Ship) ||
                    (x != 0 && PlayerShips[x - 1, y] == CoordStatus.Ship) ||
                    (y != 0 && PlayerShips[x, y - 1] == CoordStatus.Ship) ||
                    (x < 8 && PlayerShips[x + 2, y] == CoordStatus.Ship) ||
                    (y < 8 && PlayerShips[x, y + 2] == CoordStatus.Ship) ||
                    (x > 1 && PlayerShips[x - 2, y] == CoordStatus.Ship) ||
                    (y > 1 && PlayerShips[x, y - 2] == CoordStatus.Ship) ||
                    (x < 7 && PlayerShips[x + 3, y] == CoordStatus.Ship) ||
                    (y < 7 && PlayerShips[x, y + 3] == CoordStatus.Ship) ||
                    (x > 2 && PlayerShips[x - 3, y] == CoordStatus.Ship) ||
                    (y > 2 && PlayerShips[x, y - 3] == CoordStatus.Ship))
                {
                    result = ShotStatus.Wounded;
                }
                PlayerShips[x, y] = CoordStatus.Got;
                UndiscoverCells--;
                if (UndiscoverCells == 0)
                {
                    result = ShotStatus.EndBattle;
                }
            }

            return result;

        }


        // Генерация выстрела
        // Метод выстрела компьютера (генерация случайных координат)
        public string ShotGen()
        {

            int x, y; // координаты выстрела в цифровом виде

            Random rand = new Random();
            int q = rand.Next(1, 4);

            if (LastShot == ShotStatus.Kill)
            {
                WoundedStatus = false;
                WoundedCount = 0;
            }
            if ((LastShot == ShotStatus.Kill || LastShot == ShotStatus.Miss) && !WoundedStatus)
            {
                x = rand.Next(0, 9);
                y = rand.Next(0, 9);
            } else
            {
                x = int.Parse(LastShotCoord.Substring(0, 1));
                y = int.Parse(LastShotCoord.Substring(1));

                if (LastShot == ShotStatus.Wounded || FirstShort)
                {
                    FirstShort = true;

                    if (x < 9 && EnemyShips[x + 1, y] == CoordStatus.Got)
                    { x = x - 1; FirstShort = false; }
                    if (y < 9 && EnemyShips[x, y + 1] == CoordStatus.Got)
                    { y = y - 1; FirstShort = false; }
                    if (x > 0 && EnemyShips[x - 1, y] == CoordStatus.Got)
                    { x = x + 1; FirstShort = false; }
                    if (y > 0 && EnemyShips[x, y - 1] == CoordStatus.Got)
                    { y = y + 1; FirstShort = false; }

                    if (!FirstShort)
                    {
                        WoundedCount++;
                    }

                    if (FirstShort)
                    {
                        
                        switch (q)
                        {
                            case 1:
                                x = x + 1;
                                if (x > 9) x = x - 1;
                                break;
                            case 2:
                                x = x - 1;
                                if (x < 0) x = x + 1;
                                break;
                            case 3:
                                y = y + 1;
                                if (y > 9) y = y - 1;
                                break;
                            case 4:
                                y = y - 1;
                                if (y < 0) y = y + 1;
                                break;
                        }
                    }
                }
                if (LastShot == ShotStatus.Miss && WoundedStatus && !FirstShort)
                {
                    if (x < 8 && EnemyShips[x + 2, y] == CoordStatus.Got) x = x + 3;
                    else
                    if (y < 8 && EnemyShips[x, y + 2] == CoordStatus.Got) y = y + 3;
                    else
                    if (x > 1 && EnemyShips[x - 2, y] == CoordStatus.Got) x = x - 3;
                    else
                    if (y > 1 && EnemyShips[x, y - 2] == CoordStatus.Got) y = y - 3;
                    else

                    if (x < 7 && EnemyShips[x + 3, y] == CoordStatus.Got) x = x + 4;
                    else
                    if (y < 7 && EnemyShips[x, y + 3] == CoordStatus.Got) y = y + 4;
                    else
                    if (x > 2 && EnemyShips[x - 3, y] == CoordStatus.Got) x = x - 4;
                    else
                    if (y > 2 && EnemyShips[x, y - 3] == CoordStatus.Got) y = y - 4;
                    else

                    if (x < 9 && EnemyShips[x + 1, y] == CoordStatus.Got) x = x + 2;
                    else
                    if (y < 9 && EnemyShips[x, y + 1] == CoordStatus.Got) y = y + 2;
                    else
                    if (x > 0 && EnemyShips[x - 1, y] == CoordStatus.Got) x = x - 2;
                    else
                    if (y > 0 && EnemyShips[x, y - 1] == CoordStatus.Got) y = y - 2;
                }

            }

           

            string result;

            result = x.ToString() + y.ToString();

            return result;

        }

        public bool CheckCoord(string xy, ShipType type, Direction direction = Direction.Vertical)
        {
            bool result = true;



            return result;
        }


        // Добавляет или удаляет корабль
        // xy - координаты корабля, type - тип корабля, direction - направление размещения корабля, deleting - удалять или добалять корабль
        // В случае успешной операции возвращает true.
        public bool AddDelShip(string xy, ShipType type, Direction direction = Direction.Vertical, bool deleting = false)
        {
            bool result = true;

            if (deleting || CheckCoord(xy, type, direction))
            {
                int x = int.Parse(xy.Substring(0, 1));
                int y = int.Parse(xy.Substring(1));

                CoordStatus status = new CoordStatus();

                if (deleting)
                {
                    status = CoordStatus.None;
                } else
                {
                    status = CoordStatus.Ship;
                }

                PlayerShips[x, y] = status;

                if(direction == Direction.Vertical)
                {
                    switch (type)
                    {
                        case ShipType.x2:
                            PlayerShips[x, y + 1] = status;
                            break;
                        case ShipType.x3:
                            PlayerShips[x, y + 1] = status;
                            PlayerShips[x, y + 2] = status;
                            break;
                        case ShipType.x4:
                            PlayerShips[x, y + 1] = status;
                            PlayerShips[x, y + 2] = status;
                            PlayerShips[x, y + 3] = status;
                            break;
                    }
                } else
                {
                    switch (type)
                    {
                        case ShipType.x2:
                            PlayerShips[x + 1, y] = status;
                            break;
                        case ShipType.x3:
                            PlayerShips[x + 1, y] = status;
                            PlayerShips[x + 2, y] = status;
                            break;
                        case ShipType.x4:
                            PlayerShips[x + 1, y] = status;
                            PlayerShips[x + 2, y] = status;
                            PlayerShips[x + 3, y] = status;
                            break;
                    }
                }

            } else
            {
                result = false;
            }

            return result;
        }

        // Удаляет все корабли с поля
        public void DelShips()
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    PlayerShips[i, j] = CoordStatus.None;
                }
            }
        }

        

    }
}
