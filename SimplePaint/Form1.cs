using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimplePaint
{
    public partial class frmGraphic : Form
    {
        public frmGraphic()
        {
            InitializeComponent();
        }

        private DrawState state = DrawState.None;
        private LinkedList<Point> shapes = new LinkedList<Point>();
        private List<Point> selectedShapes = new List<Point>();

        private static Point p = new Point(0, 0, Color.Black);
        private static Polyline polyline = null;

        Color currentColor = Color.Black;
        int radiusH;

        // Usa a fórmula de Pitágoras para calcular o raio
        private int CalculateRadius(int cX, int cY)
        {
            int deltaX = Math.Abs(cX - p.X);
            int deltaY = Math.Abs(cY - p.Y);
            int radius = Convert.ToInt32(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)));

            return radius;
        }

        // Adiciona uma figura qualquer à lista ligada de figuras,
        // desenha a figura adicionada no contexto gráfico do
        // PictureBox e limpa o StatusStrip de mensagens
        private void AddShape(Point shape)
        {
            shapes.InsertAfterEnd(shape);
            shape.Draw(shape.Color, pbDrawArea.CreateGraphics());
            stMessage.Items[1].Text = "";
        }

        // Configura o ponto auxiliar p
        private void SetPoint(int x, int y)
        {
            p.X = x;
            p.Y = y;
            p.Color = currentColor;
        }

        // Desenha as figuras da lista ligada no PictureBox
        private void pbDrawArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            shapes.StartSequentialTraversal();
            while (shapes.CanTraverse())
            {
                Point currentShape = shapes.Current.Info;
                currentShape.Draw(currentShape.Color, g);
            }
        }

        // Atualiza a coordenada do cursor do mouse no StatusStrip
        private void pbDrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            stMessage.Items[3].Text = e.X + " , " + e.Y;
        }

        // Verifica o que o usuário está tentando desenhar,
        // caso necessário, permite o usuário continuar o desenho
        // e quando acabar, chama a função de adicionar figuras
        private void pbDrawArea_MouseClick(object sender, MouseEventArgs e)
        {
            switch (state)
            {
                case DrawState.WaitingForPoint:
                    AddShape(new Point(e.X, e.Y, currentColor));
                    state = DrawState.None;
                    break;

                case DrawState.WaitingForLineStart:
                    SetPoint(e.X, e.Y);
                    state = DrawState.WaitingForLineEnd;
                    stMessage.Items[1].Text = "Click on the endpoint of your line segment:";
                    break;

                case DrawState.WaitingForLineEnd:
                    AddShape(new Line(p.X, p.Y, e.X, e.Y, currentColor));
                    state = DrawState.None;
                    break;

                case DrawState.WaitingForCircleCenter:
                    SetPoint(e.X, e.Y);
                    state = DrawState.WaitingForCircleRadius;
                    stMessage.Items[1].Text = "Determine the distance between the center of the circle and the radius:";
                    break;

                case DrawState.WaitingForCircleRadius:
                    int radius = CalculateRadius(e.X, e.Y);
                    AddShape(new Circle(p.X, p.Y, radius, currentColor));
                    state = DrawState.None;
                    break;

                case DrawState.WaitingForEllipseCenter:
                    SetPoint(e.X, e.Y);
                    state = DrawState.WaitingForEllipseRadiusH;
                    stMessage.Items[1].Text = "Determine the distance between the center of the ellipse and the horizontal radius:";
                    break;

                case DrawState.WaitingForEllipseRadiusH:
                    state = DrawState.WaitingForEllipseRadiusV;
                    radiusH = CalculateRadius(e.X, e.Y);
                    stMessage.Items[1].Text = "Determine the distance between the center of the ellipse and the vertical radius:";
                    break;

                case DrawState.WaitingForEllipseRadiusV:
                    int radiusV = CalculateRadius(e.X, e.Y);
                    AddShape(new Ellipse(p.X, p.Y, radiusH, radiusV, currentColor));
                    state = DrawState.None;
                    break;

                case DrawState.WaitingForTopLeftDiagonal:
                    SetPoint(e.X, e.Y);
                    state = DrawState.WaitingForBottomRightDiagonal;
                    stMessage.Items[1].Text = "Click on the bottom right corner of the rectangle:";
                    break;

                case DrawState.WaitingForBottomRightDiagonal:
                    AddShape(new Rectangle(p.X, p.Y, e.X, e.Y, currentColor));
                    state = DrawState.None;
                    break;

                case DrawState.WaitingForPolylineStart:
                    polyline = new Polyline(e.X, e.Y, currentColor);
                    state = DrawState.WaitingToContinuePolyline;
                    stMessage.Items[1].Text = "Continue your polyline:";
                    break;

                case DrawState.WaitingToContinuePolyline:
                    polyline.AddPoint(new Point(e.X, e.Y, currentColor));
                    polyline.Draw(currentColor, pbDrawArea.CreateGraphics());
                    stMessage.Items[1].Text = "To finish, click the polyline button.";
                    break;
            }
        }

        // Leitura do arquivo texto para pegar os dados
        // e colocá-los na lista ligada de figuras
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader shapeFile = new StreamReader(dlgOpen.FileName);

                    string line = shapeFile.ReadLine();
                    if (line == null) return;

                    // Cabeçalho
                    double xInfLeft = Convert.ToDouble(line.Substring(0, 5).Trim());
                    double yInfLeft = Convert.ToDouble(line.Substring(5, 5).Trim());
                    double xSupRight = Convert.ToDouble(line.Substring(10, 5).Trim());
                    double ySupRight = Convert.ToDouble(line.Substring(15, 5).Trim());

                    // Lê o arquivo até acabar
                    while ((line = shapeFile.ReadLine()) != null)
                    {
                        string type = line.Substring(0, 5).Trim();
                        int xBase = Convert.ToInt32(line.Substring(5, 5).Trim());
                        int yBase = Convert.ToInt32(line.Substring(10, 5).Trim());
                        int colorR = Convert.ToInt32(line.Substring(15, 5).Trim());
                        int colorG = Convert.ToInt32(line.Substring(20, 5).Trim());
                        int colorB = Convert.ToInt32(line.Substring(25, 5).Trim());

                        Color color = Color.FromArgb(colorR, colorG, colorB);

                        switch (type)
                        {
                            case "poli":
                                // Se não tem um polilinha em leitura
                                // começa a leitura do polilinha
                                if (polyline == null)
                                    polyline = new Polyline(xBase, yBase, color);

                                // Espera aparecer uma linha nula para
                                // terminar a inserção do polilinha e
                                // preparar para caso venha outro polilinha
                                while ((line = shapeFile.ReadLine()) != "")
                                {
                                    int x = Convert.ToInt32(line.Substring(5, 5).Trim());
                                    int y = Convert.ToInt32(line.Substring(10, 5).Trim());
                                    polyline.AddPoint(new Point(x, y, color));
                                }

                                shapes.InsertAfterEnd(polyline);
                                polyline = null;
                                break;

                            case "p":
                                shapes.InsertAfterEnd(new Point(xBase, yBase, color));
                                break;

                            case "l":
                                int xFinal = Convert.ToInt32(line.Substring(30, 5).Trim());
                                int yFinal = Convert.ToInt32(line.Substring(35, 5).Trim());
                                shapes.InsertAfterEnd(new Line(xBase, yBase, xFinal, yFinal, color));
                                break;


                            case "c":
                                int raio = Convert.ToInt32(line.Substring(30, 5).Trim());

                                shapes.InsertAfterEnd(new Circle(xBase, yBase, raio, color));
                                break;

                            case "e":
                                int raio1 = Convert.ToInt32(line.Substring(30, 5));
                                int raio2 = Convert.ToInt32(line.Substring(35, 5));

                                shapes.InsertAfterEnd(new Ellipse(xBase, yBase, raio1, raio2, color));
                                break;

                            case "r":
                                int x2 = Convert.ToInt32(line.Substring(30, 5));
                                int y2 = Convert.ToInt32(line.Substring(35, 5));

                                shapes.InsertAfterEnd(new Rectangle(xBase, yBase, x2, y2, color));
                                break;
                        }
                    }

                    // Encerra a leitura do arquivo e
                    // desenha as figuras no PictureBox
                    shapeFile.Close();
                    this.Text = dlgOpen.FileName;
                    pbDrawArea.Invalidate();
                }

                catch (Exception)
                {
                    throw new Exception("An error occured while reading the file!");
                }
            }
        }

        // Salva as figuras desenhadas no
        // PictureBox em um arquivo texto
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                StreamWriter shapeFile = new StreamWriter(dlgSave.FileName);
                shapeFile.WriteLine("0    0    510  330  ");

                shapes.StartSequentialTraversal();
                while (shapes.CanTraverse())
                {
                    Point figuraAtual = shapes.Current.Info;
                    shapeFile.WriteLine(figuraAtual.ToString());
                }

                shapeFile.Close();
            }
        }

        // Prepara o programa para o desenho de um ponto
        private void btnPoint_Click(object sender, EventArgs e)
        {
            stMessage.Items[1].Text = "Click on the location of the desired point:";
            state = DrawState.WaitingForPoint;
        }

        // Prepara o programa para o desenho de uma retas
        private void btnLine_Click(object sender, EventArgs e)
        {
            stMessage.Items[1].Text = "Click on the starting point of your line:";
            state = DrawState.WaitingForLineStart;
        }

        // Prepara o programa para o desenho de um círculo
        private void btnCircle_Click(object sender, EventArgs e)
        {
            stMessage.Items[1].Text = "Click on the location of the center of the circle:";
            state = DrawState.WaitingForCircleCenter;
        }

        // Prepara o programa para o desenho de uma elipse
        private void btnEllipse_Click(object sender, EventArgs e)
        {
            stMessage.Items[1].Text = "Click on the location of the center of the ellipse:";
            state = DrawState.WaitingForEllipseCenter;
        }

        // Prepara o programa para o desenho de um retângulo
        private void btnRectangle_Click(object sender, EventArgs e)
        {
            stMessage.Items[1].Text = "Click on the top left corner of the rectangle:";
            state = DrawState.WaitingForTopLeftDiagonal;
        }

        // Prepara o programa para o desenho de um polilinha
        private void btnPolyline_Click(object sender, EventArgs e)
        {
            // 1º clique no botão começará o desenho do polilinha
            if (state != DrawState.WaitingToContinuePolyline)
            {
                stMessage.Items[1].Text = "Click on the starting point of your polyline:";
                state = DrawState.WaitingForPolylineStart;
            }

            // 2º clique no botão terminará o desenho do polilinha
            else
            {
                stMessage.Items[1].Text = "";
                shapes.InsertAfterEnd(polyline);
                polyline = null;
                state = DrawState.None;
            }

        }

        // Muda a cor de uma ou mais figuras
        private void btnColor_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog() == DialogResult.OK)
            {
                // Se está mudando a cor de objetos selecionados
                // é mudado sequencialmente a cor de cada desenho
                // e são redesenhadas as figuras com +1 pixel de expessura
                if (selectedShapes.Count > 0)
                    foreach (var shape in selectedShapes)
                        shape.Color = dlgColor.Color;

                // Se está mudando a cor para inserir
                // uma nova figura, só muda a cor atual
                else
                    currentColor = dlgColor.Color;

                pbDrawArea.Invalidate();
            }
        }

        // Fecha o programa
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Limpa o PictureBox, as figuras de auxílio,
        // as listas de figuras e figuras selecionadas
        private void btnClean_Click(object sender, EventArgs e)
        {
            shapes = new LinkedList<Point>();
            selectedShapes = new List<Point>();
            p = new Point(0, 0, Color.Black);
            polyline = null;
            pbDrawArea.Invalidate();
        }

        // Seleciona uma figura a partir do
        // índice da mesma na lista ligada
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIndex.Text, out int number) &&
                number >= 0 && number < shapes.NumberOfNodes)
            {
                var shape = shapes.GetByIndex(number);
                if (shape != null && !selectedShapes.Contains(shape))
                {
                    selectedShapes.Add(shape);
                    shape.Draw(Color.Red, pbDrawArea.CreateGraphics(), 2);
                }
            }
            else
            {
                MessageBox.Show("Invalid index!", "Invalid input!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Limpa a lista de figuras selecionadas
        // e redesenha o PictureBox
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Se não há dados, não é necessário fazer nada
            if (selectedShapes.Count == 0)
                return;

            selectedShapes = new List<Point>();
            pbDrawArea.Invalidate();
        }
    }
}
