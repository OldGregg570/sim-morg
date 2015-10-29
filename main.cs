using System;
using System.Collections.Generic;
using System.IO;

namespace SimMorg {

  /** Class containing program entry point */
  class Program {
    static SimulationConfig config;
    static Simulation sim;

    internal Simulation Simulation {
      get {
        throw new System.NotImplementedException();
      }

      set {
      }
    }

    internal SimulationConfig SimulationConfig {
      get {
        throw new System.NotImplementedException();
      }

      set {
      }
    }

    public static int Main(string[] args) {
      config = new SimulationConfig(new List<string>(args));
      sim = new Simulation(config.inputFile, config.displayStrategy);

      while (sim.running()) {
        sim.display();
        sim.tick();
      }
      Console.WriteLine("Simulation complete!");
      return 0;
    }
  }

  class SimulationConfig {
    private string _inputFile = "../../in/new-format.morg";
    public string inputFile { get { return this._inputFile; } }
  
    private IDisplayStrategy _displayStrategy = new ConsolePrint();
    public IDisplayStrategy displayStrategy { get { return this._displayStrategy; } }

    public SimulationConfig(List<String> argv) {
      if (argv.Contains("-p")) { 
        this._displayStrategy = new BoardPrint();
      }

      foreach (string arg in argv) {
        string f = "../../in/" + arg;
        Console.WriteLine(f);
        if (File.Exists(f)) {
          _inputFile = f;
          break;
        }
      }
    }
  }
}