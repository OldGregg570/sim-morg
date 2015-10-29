using System;
using System.Collections.Generic;

namespace SimMorg {
  /** Class containing morg simulation logic */
  class Simulation {
    protected List<Morg> morgs = new List<Morg>();
    private int iteration = 0;
    private IDisplayStrategy displayStrategy;
    private MorgFactory morgFactory = new MorgFactory();

    /** Class constructor. Sets up initial simulation configuration. */
    public Simulation (String inputFile, IDisplayStrategy displayStrategy) {
      string line;
      this.displayStrategy = displayStrategy;

      // Read the file and display it line by line.
      System.IO.StreamReader file = new System.IO.StreamReader(inputFile);

      while ((line = file.ReadLine()) != null) {
        String[] tokens = line.Split(',');
        String type = tokens[0];
        int x = int.Parse(tokens[1]);
        int y = int.Parse(tokens[2]);
        String movementStrategy = tokens[3];
        String feeding = tokens[4];

        String[] feedingTokens = feeding.Split(' ');
        String feedingStrategy = feedingTokens[0];

        morgs.Add(morgFactory.createMorg(type, x, y, movementStrategy, feedingStrategy, feedingTokens));
      }
      file.Close();
    }

    internal MorgFactory MorgFactory {
      get {
        throw new System.NotImplementedException();
      }

      set {
      }
    }

    /** Returns whether the simulation is running or not. Simulation */
    public bool running () {
      return this.morgs.Count > 0 && this.iteration <= 100;
    }

    /** Run an iteration of the simulation */
    public void tick () {
      foreach (Morg m in this.morgs) {
        if (m.alive) {
          m.move(this.iteration);
          m.feed();
          m.notifyAll();
          if (m.prey == null || !m.prey.alive) m.hunt(this.morgs);
          if (m.life <= 0) m.kill();
        }
      }

      //this.morgs.RemoveAll(m => !m.alive);
      
      this.iteration++;
    }

    /** Display the current state of the simulation */
    public void display () {
      this.displayStrategy.print(morgs, this.iteration);
    }
  }
}
