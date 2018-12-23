using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;



namespace MyGame
{
    static class Game
    {
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
        public static BaseObject[] _obj;
        public static void Load()
        {
            _obj = new BaseObject[30];
            for (int i = 0; i <=15; i++)
            {
                _obj[i] = new BaseObject(new Point(600, i * 15), new Point(10 - i, 10 - i), new Size(20, 20));
            }
            for (int i = 15; i < _obj.Length; i++)
            {
                _obj[i] = new Star(new Point(800, i * 15), new Point(+i, 0), new Size(5, 5));
            }

        }

        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        //Ширина и высота игрового поля
        public static int Width
        {
            get;set;
        }
        public static int Height
        {
            get;set;
        }
        static Game()
        {

        }
        public static void Init (Form form)
        {
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
            //Графическое устройства для вывода графики
            Graphics g;

            //Предоставляет доступ к главному буферу графического контекста
            //для текущего приложения.

            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            //Создаём объект (поверхность рисования) и связывем его с формой
            //Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            //Сязываем буфер в памяти с графическим объекотм, что бы рисовать в буфере.
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
        }
        public static void Draw()
        {
            ////Проверяем вывод графики
            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            //Buffer.Render();

            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _obj)
                obj.Draw();
            Buffer.Render();
        }
        public static void Update()
        {
            foreach (BaseObject obj in _obj)
                obj.Update();
        }

       
    }
}
