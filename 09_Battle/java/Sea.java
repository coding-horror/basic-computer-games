class Sea {
    private int tiles[];
    private boolean hits[];

    private int size;
    public Sea(int make_size) {
        size = make_size;
        tiles = new int[size*size];
    }

    public int size() { return size; }

    public String encodedDump() {
        StringBuilder out = new StringBuilder();
        for (int x = 0; x < size; ++x) {
            for (int y = 0; y < size; ++y) 
                out.append(Integer.toString(get(x, y)));
            out.append('\n');
        }
        return out.toString();   
    }

    /* return true if x,y is in the sea and empty
     * return false if x,y is occupied or is out of range
     */
    public boolean isEmpty(int x, int y) {
        if ((x<0)||(x>=size)||(y<0)||(y>=size)) return false;
        return (get(x,y) == 0);
    }

    /* return the ship number, or zero if no ship */
    public int get(int x, int y) {
        return tiles[index(x,y)];
    }

    public void set(int x, int y, int value) {
        tiles[index(x, y)] = value;
    }

    public int shipHit(int x, int y) {
        if (hits[index(x,y)]) return get(x, y);
        else return 0;
    }

    public void recordHit(int x, int y) {
        hits[index(x, y)] = true;
    }

    private int index(int x, int y) {
        if ((x < 0) || (x >= size))
            throw new ArrayIndexOutOfBoundsException("Program error: x cannot be " + x);
        if ((y < 0) || (y >= size))
            throw new ArrayIndexOutOfBoundsException("Program error: y cannot be " + y);

        return y*size + x;
    }
}
