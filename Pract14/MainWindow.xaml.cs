using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pract14
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            ToolTipService.SetShowDuration(canvas, 3000);
            ToolTipService.SetInitialShowDelay(canvas, 300);
            ToolTipService.SetBetweenShowDelay(canvas, 1000);
        }

        private void drawSquare(DrawingVisual visual, double x, double y, double width)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen p = new Pen(Brushes.ForestGreen, 1.0f);
                dc.DrawRectangle(Brushes.LawnGreen, p, new Rect(new Point(x, y), new Size(width, width)));
                
            }
        }


        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            if (e.ChangedButton == MouseButton.Left)
            {
                var initialCenter = new Point(ActualWidth / 2, ActualHeight / 2);

                FullFractalGeneration(initialCenter, 5);
            }
        }


        private void FullFractalGeneration(Point center, int value)
        {
            GeneratePointsRecursivelyUp(center, value);

            GeneratePointsRecursivelyDown(center, value);
        }
        private void GeneratePointsRecursivelyDown(Point center, int currentStep)
        {
            if (currentStep < 0)
            {
                return;
            }

            var initialCenter = new Point(center.X, center.Y);
            StartingPoint(initialCenter, 2);
            int initialStep = 45;
            GeneratePoints(initialCenter, initialStep, 1, currentStep);

            GeneratePointsRecursivelyDown(new Point(initialCenter.X - 45, initialCenter.Y + 45), currentStep - 1);
        }

        private void GeneratePointsRecursivelyUp(Point center, int currentStep, bool skipFirstStep = true)
        {
            if (currentStep < 0)
            {
                return;
            }

            var initialCenter = new Point(center.X, center.Y);
            StartingPoint(initialCenter, 2);
            int initialStep = 45;

            if (!skipFirstStep) // Проверяем, нужно ли пропустить первый шаг
            {
                GeneratePoints(initialCenter, initialStep, 1, currentStep);
            }

            // После первого шага пропускаем (или не пропускаем) в зависимости от параметра skipFirstStep
            GeneratePointsRecursivelyUp(new Point(initialCenter.X + 45, initialCenter.Y - 45), currentStep - 1, false);
        }




        void GeneratePoints(Point center, int step, int currentStep, int maxSteps)
        {
            if (currentStep > maxSteps)
            {
                return;
            }

            double offsetX = 45 * currentStep;
            double offsetY = 45 * currentStep;

            Point pos = new Point(center.X + offsetX, center.Y + offsetY);
            StartingPoint(pos, 2);

            Point pos2 = new Point(center.X - offsetX, center.Y - offsetY);
            StartingPoint(pos2, 2);

            GeneratePoints(center, step, currentStep + 1, maxSteps); // Рекурсивный вызов метода для увеличения шага
        }


        private void StartingPoint(Point pos, int n)
        {
            if (n <= 0) return;
            DrawSquaresRecursive(pos, 2);
            StartingPoint(new Point(pos.X - 15, pos.Y - 15), n - 1);
            StartingPoint(new Point(pos.X + 15, pos.Y + 15), n - 1);
            StartingPoint(new Point(pos.X + 15, pos.Y - 15), n - 1);
            StartingPoint(new Point(pos.X - 15, pos.Y + 15), n - 1);
        }

        private void DrawSquaresRecursive(Point pos, int n)
        {
            if (n <= 0) return;  // Останавливаем рекурсию, если n <= 0
            DrawSquare(pos, 5.0f);  // Рисуем квадрат в текущей позиции

            // Вызываем рекурсивно DrawSquaresRecursive для каждой из четырех сторон от текущей позиции
            DrawSquaresRecursive(new Point(pos.X - 5, pos.Y - 5), n - 1);
            DrawSquaresRecursive(new Point(pos.X + 5, pos.Y + 5), n - 1);
            DrawSquaresRecursive(new Point(pos.X + 5, pos.Y - 5), n - 1);
            DrawSquaresRecursive(new Point(pos.X - 5, pos.Y + 5), n - 1);

        }
        private void DrawSquare(Point pos, double width)
        {
            var dv = new DrawingVisualObject(rnd.Next(0, 1000), new string(Enumerable.Range(0, 5).Select(x => (char)rnd.Next(65, 90)).ToArray()));
            drawSquare(dv, pos.X, pos.Y, width);
            canvas.AddVisual(dv);
        }



        ToolTip tt = new System.Windows.Controls.ToolTip();
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(canvas);
            DrawingVisualObject dvo = canvas.GetVisualObject(pos);

            if (dvo == null) { canvas.ToolTip = null; tt.IsOpen = false; return; };

            tt.Content = $"ID: {dvo.Id}\nName: {dvo.Name}";
            canvas.ToolTip = tt;
            tt.IsOpen = true;

        }

       
    }
}

