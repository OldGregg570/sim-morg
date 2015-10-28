using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimMorg {
  /** Class containing morg simulation logic */
  class Simulation {
    protected List<Morg> morgs;
    private int iteration = 0;
    private int trialCount;
    private IDisplayStrategy displayStrategy;

    /** Class constructor. Sets up initial simulation configuration. */
    public Simulation (int trialCount, List<Morg> morgs, IDisplayStrategy displayStrategy) {
      this.morgs = morgs;
      this.displayStrategy = displayStrategy;
      this.trialCount = trialCount;
    }

    /** Returns whether the simulation is running or not. Simulation */
    public bool running () {
      return this.morgs.Count > 0 && this.iteration <= this.trialCount;
    }

    /** Run an iteration of the simulation */
    public void tick () {
      foreach (Morg m in this.morgs) {
        m.move(this.iteration);
        m.feed();
        m.notifyAll();
        if (m.prey == null || !m.prey.alive) m.hunt(this.morgs);
      }
      this.iteration++;
    }

    /** Display the current state of the simulation */
    public void display () {
      this.displayStrategy.print(morgs, this.iteration);
    }
  }
}
