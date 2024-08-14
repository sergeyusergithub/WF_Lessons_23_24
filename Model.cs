using System;
using System.Collections.Generic;
using System.Linq;
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

        // Поле координат последнего выстрела
        public string? LastShotCoord;


        // конструктор по умолчанию. Инициализация полей модели.
        public Model()
        {
            LastShot = ShotStatus.Miss;
            WoundedStatus = false;

            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    PlayerShips[i, j] = CoordStatus.None;
                    EnemyShips[i,j] = CoordStatus.None;
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
                    (y != 0 && PlayerShips[x, y - 1] == CoordStatus.Ship))
                {
                    result = ShotStatus.Wounded;
                }
                PlayerShips[x, y] = CoordStatus.Got;
                UndiscoverCells--;
                if(UndiscoverCells == 0)
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
            string result = "00";

            int x, y; // координаты выстрела в цифровом виде

            Random rand = new Random();

            if(LastShot == ShotStatus.Kill) WoundedStatus = false;

            if ((LastShot == ShotStatus.Kill || LastShot == ShotStatus.Miss) && !WoundedStatus)
            {
                x = rand.Next(0, 9);
                y = rand.Next(0, 9);
            } else
            {
                x = int.Parse(LastShotCoord.Substring(0, 1));
                y = int.Parse(LastShotCoord.Substring(1));
                if (LastShot == ShotStatus.Wounded)
                {
                    
                    if (x != 9 && EnemyShips[x + 1, y] == CoordStatus.Got) x = x - 1;
                    if (y != 9 && EnemyShips[x, y + 1] == CoordStatus.Got) y = y - 1;
                    if (x != 0 && EnemyShips[x - 1, y] == CoordStatus.Got) x = x + 1;
                    if (y != 0 && EnemyShips[x, y - 1] == CoordStatus.Got) y = y + 1;
                    {

                    }
                }
            }





            result = x.ToString() + y.ToString();

            return result;

        }

    }
}
