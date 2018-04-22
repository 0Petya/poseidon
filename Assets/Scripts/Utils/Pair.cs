public class Pair<T1, T2> {
  public T1 fst { get; private set; }
  public T2 sec { get; private set; }
  internal Pair(T1 first, T2 second) {
    fst = first;
    sec = second;
  }
}