using System;
using System.Collections.Generic;

namespace SimMorg {

  /**
  * Class containing program entry point
  */
  class Program {
    public static void Main(string[] args) {
      List<Morg> morgs = new List<Morg>();
      string line;
      string inputFile = args[0];

      // Set up the display strategy to either print to the console or print
      // a bitmap of each step
      IDisplayStrategy displayStrategy = new ConsolePrint();
      if (args.Length > 1 && args[1] == "-p") {
        displayStrategy = new BoardPrint();
      }

      // Read the file and display it line by line.
      System.IO.StreamReader file = new System.IO.StreamReader(inputFile);
      int trialCount = int.Parse(file.ReadLine());
      while((line = file.ReadLine()) != null) {
        string[] tokens = line.Split(',');
        String name = tokens[0];
        String type = tokens[1];
        int x = int.Parse(tokens[2]);
        int y = int.Parse(tokens[3]);
        morgs.Add (new Morg (name, type, x, y));
      }
      file.Close();

      Simulation sim = new Simulation (trialCount, morgs, displayStrategy);
      while (sim.running()) {
        sim.display();
        sim.tick();
      }
      Console.WriteLine("Simulation complete!");
    }
  }
}
