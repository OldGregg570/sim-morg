using System;
using System.Collections.Generic;

namespace SimMorg {

  public static class Constants {
     public static int RUN_LENGTH { get { return 2; } }

     public static Tuple<int, int> [] dirs {
      get {
        return new Tuple<int, int> [] {
         new Tuple<int, int>(1, 0),   // East
         new Tuple<int, int>(0, 1),   // South
         new Tuple<int, int>(-1, 0),  // West
         new Tuple<int, int>(0, -1),  // North
         new Tuple<int, int>(1, 1),   // Southeast
         new Tuple<int, int>(-1, 1),  // Southwest
         new Tuple<int, int>(-1, -1), // Northwest
         new Tuple<int, int>(1, -1)   // Northeast
        };
      }
    }
  }

  /**
  * Distance Strategies
  */
  interface DistanceStrategy {
    int distance (Tuple<int, int> left, Tuple<int, int> right);
  }

  class Manhattan : DistanceStrategy {
    public int distance (Tuple<int, int> left, Tuple<int, int> right) {
        return Math.Abs (right.Item1 - left.Item1) + Math.Abs (right.Item2 - left.Item2);
    }
  }

  /**
  * Movement Strategies
  */
  interface MovementStrategy {
    Tuple<int, int> move (int t, Tuple<int, int> position, Tuple<int, int> target);
  }

  class SimpleMove : MovementStrategy {
    /** If we have a target, move as a predator. Otherwise, move as prey */
    public Tuple<int, int> move(int t, Tuple<int, int> position, Tuple<int, int> target) {
      return target == null ? movePrey(t, position, target) : movePredator(t, position, target);
    }

    /** Move in a circle */
    private Tuple<int, int> movePrey(int t, Tuple<int, int> position, Tuple<int, int> target) {
      Tuple<int, int> d = Constants.dirs[((t / Constants.RUN_LENGTH) % (Constants.dirs.Length / 2)) + 4];
      return new Tuple<int, int>(position.Item1 + d.Item1, position.Item2 + d.Item2);
    }

    /** Follow the target */
    private Tuple<int, int> movePredator(int t, Tuple<int, int> position, Tuple<int, int> target) {
      int dx = target.Item1 < position.Item1 ? -2 : (target.Item1 == position.Item1 ? 0 : 2);
      int dy = target.Item2 < position.Item2 ? -2 : (target.Item2 == position.Item2 ? 0 : 2);
      return new Tuple<int, int>(position.Item1 + dx, position.Item2 + dy);
    }
  }

  class Ooze : MovementStrategy {
    /** If we have a target, move as a predator. Otherwise, move as prey */
    public Tuple<int, int> move (int t, Tuple<int, int> position, Tuple<int, int> target) {
      return target == null ? movePrey (t, position, target) : movePredator (t, position, target);
    }

    /** Move in a circle */
    private Tuple<int, int> movePrey (int t, Tuple<int, int> position, Tuple<int, int> target) {
      Tuple<int, int> d = Constants.dirs[(t / Constants.RUN_LENGTH) % (Constants.dirs.Length / 2)];
      return new Tuple<int, int>(position.Item1 + d.Item1, position.Item2 + d.Item2);
    }

    /** Follow the target */
    private Tuple<int, int> movePredator (int t, Tuple<int, int> position, Tuple<int, int> target) {
      int dx = target.Item1 < position.Item1 ? -1 : (target.Item1 == position.Item1 ? 0 : 1);
      int dy = target.Item2 < position.Item2 ? -1 : (target.Item2 == position.Item2 ? 0 : 1);
      return new Tuple<int, int> (position.Item1 + dx, position.Item2 + dy);
    }
  }

  class Paddle : MovementStrategy {
    /** If we have a target, move as a predator. Otherwise, move as prey */
    public Tuple<int, int> move (int t, Tuple<int, int> position, Tuple<int, int> target) {
      return target == null ? movePrey (t, position, target) : movePredator (t, position, target);
    }

    /** Move in a circle */
    private Tuple<int, int> movePrey (int t, Tuple<int, int> position, Tuple<int, int> target) {
      Tuple<int, int> d = Constants.dirs[((t / Constants.RUN_LENGTH) % (Constants.dirs.Length / 2)) + 4];
      return new Tuple<int, int>(position.Item1 + d.Item1, position.Item2 + d.Item2);
    }

    /** Follow the target */
    private Tuple<int, int> movePredator (int t, Tuple<int, int> position, Tuple<int, int> target) {
      int dx = target.Item1 < position.Item1 ? -2 : (target.Item1 == position.Item1 ? 0 : 2);
      int dy = target.Item2 < position.Item2 ? -2 : (target.Item2 == position.Item2 ? 0 : 2);
      return new Tuple<int, int> (position.Item1 + dx, position.Item2 + dy);
    }
  }
  /**
  * Feeding Strategies
  */
  abstract class FeedingStrategy {
    public abstract bool feed(Morg predator, Morg prey);
  }

  class SimpleFeed: FeedingStrategy {
    public override bool feed(Morg predator, Morg prey) {
      int distance = predator.distance(prey);
      if (distance == 2) {
        if (predator.preyTypes.Contains(prey.type)) {
          prey.kill();
          predator.life += prey.life;
        }
      }
      return distance == 2;
    }
  }

  class Absorb : FeedingStrategy {
    public override bool feed(Morg predator, Morg prey) {
      int distance = predator.distance(prey);
      if (distance <= 1) {
        if (predator.preyTypes.Contains(prey.type)) {
          prey.kill ();
          predator.life += prey.life;
        }
      }
      return distance <= 1;
    }
  }

  class Envelop : FeedingStrategy {
    public override bool feed(Morg predator, Morg prey) {
      int distance = predator.distance(prey);
      if (distance == 0) {
        if (predator.preyTypes.Contains(prey.type)) {
          prey.kill ();
          predator.life += prey.life / 2;
        }
      }
      return distance == 0;
    }
  }
}
