package testMeditrash;

public class MutPair<F, S> {
    private F first = null;
    private S second = null;

    public MutPair(F first, S second) {
        this.first = first;
        this.second = second;
    }

    public F getFirst() {
        return first;
    }

    public S getSecond() {
        return second;
    }

    public void setFirst(F first) {
        this.first = first;
    }

    public void setSecond(S second) {
        this.second = second;
    }

    @Override
    public String toString() {
        return "MutPair{" +
                "first=" + first.toString() +
                ", second=" + second.toString() +
                '}';
    }
}
