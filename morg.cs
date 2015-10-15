using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimMorg {
  class Morg {
    private Color _color;
    public Color color {
      get { return this._color; }
    }

    private String _name;
    public String name {
      get { return this._name; }
    }

    private String _type;
    public String type {
      get { return this._type; }
    }

    private Tuple<int, int> _position;
    public Tuple<int, int> position {
      get { return this._position; }
      set { this._position = value; }
    }

    private Tuple<int, int> _target;
    public Tuple<int, int> target {
      get { return this._target; }
      set { this._target = value; }
    }

    private Morg _prey;
    public Morg prey {
      get { return this._prey; }
      set { this._prey = value; }
    }

    private bool _alive = true;
    public bool alive {
      get { return this._alive; }
    }

    private List<String> _preyTypes = new List<String>();
    public List<String> preyTypes {
      get { return this._preyTypes; }
    }
    private List<Morg> observers = new List<Morg>();
    private MovementStrategy movementStrategy = new Ooze();
    private FeedingStrategy feedingStrategy = new Envelop();
    private DistanceStrategy distanceStrategy = new Manhattan();

    /**
    * Microorganism constructor
    */
    public Morg (String name, String type, int x, int y) {

      this._name = name;
      this._type = type;
      this.position = new Tuple<int, int>(x, y);
      this.prey = prey;

      if (this.prey != null) {
        this.target = this.prey.position;
        this.prey.registerObserver(this);
      }

      if (this.type == "A") {
        this.movementStrategy = new Paddle();
        this.feedingStrategy = new Absorb();
        this._preyTypes.Add ("B");
        this._preyTypes.Add ("C");

        this._color = Color.Red;
      } else if (type == "B") {
        this.movementStrategy = new Ooze();
        this.feedingStrategy = new Envelop();
        this._preyTypes.Add ("A");

        this._color = Color.Yellow;
      } else if (type == "C") {
        this.movementStrategy = new Paddle();
        this.feedingStrategy = new Envelop();
        this._preyTypes.Add ("A");
        this._preyTypes.Add ("B");

        this._color = Color.Green;
      }
    }

    public void kill () {
      this._alive = false;
      this.unregisterAllObservers();
    }

    public int distance(Morg prey) {
      return distanceStrategy.distance(this.position, prey.position);
    }

    public void update (int t) {
      this.position = this.movementStrategy.move(t, this.position, this.target);
      if (this.prey != null) this.feedingStrategy.feed(this, this.prey);
      this.notifyAll();
    }

    public void hunt (List<Morg> morgs) {
      foreach (Morg m in morgs) {
        if (this != m && m.alive && this.preyTypes.Contains(m.type)) {
          this.prey = m;
          m.registerObserver(this);
        }
      }
    }
    /**
    * Register an observer to observe this morg. The observer will be notified
    * whenever this morg's notifyAll() method is called
    */
    public void registerObserver (Morg observer) {
      this.observers.Add(observer);
    }

    /**
    * Unregister an observer of this morg
    */
    public void unregisterObserver (Morg observer) {
      this.observers.Remove(observer);
    }

    /**
    * Unregister an observer of this morg
    */
    public void unregisterAllObservers () {
      this.observers.Clear();
    }

    /**
    * Call the notify method for all morgs observing this morg
    */
    public void notifyAll () {
      foreach (Morg predator in this.observers) {
        predator.notify(this);
      }
    }

    /**
    * Called when an observed prey is updated
    */
    private void notify (Morg prey) {
      this.target = prey.position;
    }

    /**
    * Returns a formatted printable string representation of this Morg
    */
    public override string ToString () {
      return String.Format("{0} {1} {2} {3}", this.name.PadRight(10),
          (this.prey == null ? "None" : this.prey.name).PadRight(10),
           this.alive ? String.Format("({0}, {1})", this.position.Item1, this.position.Item2) : "Dead",
           this.type);
    }
  }
}
