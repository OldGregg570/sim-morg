using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimMorg {
  interface Observer {
    void move (int t);
    void feed ();
  }

  class MorgDecorator : Morg {
    protected Morg decoratedMorg;
    public MorgDecorator (Morg m) : base (m) {
      this.decoratedMorg = m;
    }
  }

  class Oozing : MorgDecorator {
    public Oozing (Morg m) : base (m) {
      this.movementStrategy = new Ooze();
    }
  }

  class Paddling : MorgDecorator {
    public Paddling (Morg m) : base (m) {
      this.movementStrategy = new Paddle();
    }
  }

  class Absorbing : MorgDecorator {
    public Absorbing (Morg m) : base (m) {
      this.feedingStrategy = new Absorb();
    }
  }

  class Enveloping : MorgDecorator {
    public Enveloping (Morg m) : base (m) {
      this.feedingStrategy = new Envelop();
    }
  }

  class Morg : Observer {
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

    protected List<String> _preyTypes = new List<String>();
    public List<String> preyTypes {
      get { return this._preyTypes; }
    }
    protected List<Observer> observers = new List<Observer>();
    protected MovementStrategy movementStrategy = new SimpleMove ();
    protected FeedingStrategy feedingStrategy = new SimpleFeed();
    protected DistanceStrategy distanceStrategy = new Manhattan ();


    /** Copy constructor */
    public Morg (Morg m) {
      this._color = m.color;
      this._name = m.name;
      this._type = m.type;
      this._alive = m.alive;
      this._preyTypes = m._preyTypes;
      this.position = m.position;
      this.target = m.target;
      this.prey = m.prey;
      this.observers = m.observers;
    }

    /** Microorganism constructor */
    public Morg (String name, String type, int x, int y) {
      this._name = name;
      this._type = type;
      this.position = new Tuple<int, int>(x, y);
      this.prey = prey;

      if (this.prey != null) {
        this.target = this.prey.position;
        this.prey.registerObserver(this);
      }

      this._color = Color.Red;
    }

    public void kill () {
      this._alive = false;
      this.unregisterAllObservers();
    }

    public int distance(Morg prey) {
      return distanceStrategy.distance(this.position, prey.position);
    }

    public void move (int t) {
      this.position = this.movementStrategy.move(t, this.position, this.target);
    }
    public void feed () {
      if (this.prey != null) this.feedingStrategy.feed(this, this.prey);
    }

    /** Called if the morg doesn't have a prey */
    public void hunt (List<Morg> morgs) {
      foreach (Morg m in morgs) {
        if (this != m && m.alive && this.preyTypes.Contains(m.type)) {
          this.prey = m;
          m.registerObserver(this);
        }
      }
    }
    /** Register an observer to observe this morg. The observer will be notified
        whenever this morg's notifyAll() method is called */
    public void registerObserver (Morg observer) {
      this.observers.Add(observer);
    }

    /** Unregister an observer of this morg */
    public void unregisterObserver (Morg observer) {
      this.observers.Remove(observer);
    }

    /** Unregister an observer of this morg */
    public void unregisterAllObservers () {
      this.observers.Clear();
    }

    /** Call the notify method for all morgs observing this morg */
    public void notifyAll () {
      foreach (Morg predator in this.observers) {
        predator.notify(this);
      }
    }

    /** Called when an observed prey is updated */
    private void notify (Morg prey) {
      this.target = prey.position;
    }

    /** Returns a formatted printable string representation of this Morg */
    public override string ToString () {
      return String.Format("{0} {1} {2} {3}", this.name.PadRight(10),
          (this.prey == null ? "None" : this.prey.name).PadRight(10),
           this.alive ? String.Format("({0}, {1})", this.position.Item1, this.position.Item2) : "Dead",
           this.type);
    }
  }
}
