using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimMorg {

  /** Interface for displaying the morgs */
  interface IDisplayStrategy {
    void print (List<Morg> morgs, int iteration);
  }

  /** Print all morgs to the console */
  class ConsolePrint : IDisplayStrategy {
    public void print (List<Morg> morgs, int iteration) {
      Console.WriteLine("Iteration Number " + iteration);
      foreach (Morg m in morgs) {
        Console.WriteLine(m);
      }
      Console.ReadLine();
    }
  }

  /** Print all morgs to a bitmap for each iteration */
  class BoardPrint : IDisplayStrategy {
    public void print (List<Morg> morgs, int iteration) {
      Bitmap bmp = new Bitmap(100, 100);
      String fname = String.Format("./out/iteration-{0}.bmp", iteration);

      for (int x = 0; x < bmp.Width; x++) {
        for (int y = 0; y < bmp.Height; y++) {
          bmp.SetPixel(x, y, Color.Black);
        }
      }

      foreach (Morg m in morgs) {
        if (m.alive) bmp.SetPixel(m.position.Item1, m.position.Item2, m.color);
      }
      bmp.Save(fname);
    }
  }
}
