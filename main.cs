using System;
using System.Collections.Generic;

namespace SimMorg {

  /**
  * Class containing program entry point
  */
  class Program {
    public static int Main(string[] args) {
      List<Morg> morgs = new List<Morg>();
      string line;
      string inputFile;
      MorgFactory morgFactory = new MorgFactory ();
    
      if (args.Length < 1) {
        Console.WriteLine("Warning: No input file provided. Using default file.");
        inputFile = "../../in/new-format.morg";
      } else {
        inputFile = args[0];
      }

      // Set up the display strategy to either print to the console or print
      // a bitmap of each step
      IDisplayStrategy displayStrategy = new ConsolePrint();
      if (args.Length > 1 && args[1] == "-p") {
        displayStrategy = new BoardPrint();
      }

            // Read the file and display it line by line.
      Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
      System.IO.StreamReader file = new System.IO.StreamReader(inputFile);

      while((line = file.ReadLine()) != null) {
        String[] tokens = line.Split(',');
        String type = tokens[0];
        int x = int.Parse(tokens[1]);
        int y = int.Parse(tokens[2]);
        String movementStrategy = tokens[3];
        String feeding = tokens[4];

        String[] feedingTokens = feeding.Split(' ');
        String feedingStrategy = feedingTokens[0];

        morgs.Add ( morgFactory.createMorg(type, x, y, movementStrategy, feedingStrategy) );
      }

      file.Close();

      Simulation sim = new Simulation (1000, morgs, displayStrategy);
      while (sim.running()) {
        sim.display();
        sim.tick();
      }
      Console.WriteLine("Simulation complete!");
      return 0;
    }
  }

  class MorgFactory {
    public MorgFactory () {
    }

    public Morg createMorg (String type, int x, int y, String movement, String feeding) {
      Morg m = new Morg ("Jan Michael Vincent", type, x, y);

      switch (feeding.ToLower()) {
        case "Paddles": Console.WriteLine ("PADDLES");
          break;
      }

      switch (movement.ToLower()) {
        case "Envelops" : Console.WriteLine("ENVELOPS");
          break;
      }
      m = new Paddling(m);
      m = new Enveloping(m);
      return m;
    }
  }
}
