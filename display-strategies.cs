using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SimMorg {

  /** Interface for displaying the morgs */
  interface IDisplayStrategy {
    void print(List<Morg> morgs, int iteration);
  }

  /** Print all morgs to the console */
  class ConsolePrint : IDisplayStrategy {
    public void print(List<Morg> morgs, int iteration) {
      Console.WriteLine("Iteration Number " + iteration);
      foreach (Morg m in morgs) {
        Console.WriteLine(m);
      }
      Console.ReadLine();
    }
  }

  /** Print all morgs to a bitmap for each iteration */
  class BoardPrint : IDisplayStrategy {
    Form window;

    public BoardPrint() {
      window = new Form();
      Type con = window.GetType();
      PropertyInfo p = con.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
      p.SetValue(window, true, null);
      window.Size = new System.Drawing.Size(800, 800);
      window.Show();
    }

    [STAThread]
    public void print(List<Morg> morgs, int iteration) {
      int alive = 0;
      foreach (Morg m in morgs) {
        alive += m.alive ? 1 : 0;
      }
      using (Bitmap bmp = new Bitmap(1000, 1000))
      using (Graphics g = Graphics.FromImage(bmp)) {
        g.FillRectangle(new SolidBrush(Color.Black), 0, 0, 1000, 1000);
        g.DrawString(String.Format("Iteration: {0}\nMorgs: {1}", iteration, alive), 
            new Font("Arial", 8), 
            new SolidBrush(Color.White), 
            new PointF(10.0F, 10.0F));

        foreach (Morg m in morgs) {
          if (m.alive) {
            g.FillRectangle(new SolidBrush(m.color), m.position.Item1 * 6, m.position.Item2 * 6, 6, 6);
          } else {
            if (0 == (iteration / 4) % 2) {
              g.FillRectangle(new SolidBrush(Color.DarkRed), m.position.Item1 * 6, m.position.Item2 * 6, 6, 6);
            }
          }
        }

        if (alive == 1) {
          g.DrawString(String.Format("Winner: " + morgs[0].name, iteration, alive),
            new Font("Arial", 18),
            new SolidBrush(Color.White),
            new PointF(10.0F, 50.0F));
        }
        window.BackgroundImage = bmp;
        window.Refresh();
        System.Threading.Thread.Sleep(200);
      }
    }
  }
}
